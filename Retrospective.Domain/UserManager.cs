using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Retrospective.Data;
using DomainModel = Retrospective.Domain.Model;
using DBModel = Retrospective.Data.Model;

namespace Retrospective.Domain
{
  public class UserManager : BaseManager
  {
    private readonly ILogger<UserManager> logger;
    private readonly IMapper mapper;
    private readonly IDatabase database;

    public UserManager(ILogger<UserManager> logger, IMapper mapper, IDatabase database) : base(logger, mapper, database)
    {
      this.logger = logger;
      this.mapper = mapper;
      this.database = database;
    }

    public DomainModel.User GetUserFromEmail(string email)
    {
        var users = database.Users.FindUserByEmail(email);

        if(users.Count>1)
        {
            logger.LogWarning ("There are {0} user records for email address {1}", users.Count, email);
        }

        if(users.Count==0)
        {
            logger.LogInformation("No users were found for email address {1}", email);
            return null;
        }
        //return the found user
        return mapper.Map<DBModel.User, DomainModel.User>(users.First());

    }

    public DomainModel.User UpdateUser(DomainModel.User user)
    {
            logger.LogDebug ("saving user {0} {1}", user.Email, user.UserId);

            var dbUser = database.Users.SaveUser (mapper.Map<DomainModel.User, DBModel.User> (user));
            return mapper.Map<DBModel.User, DomainModel.User> (dbUser);
    }


  }
}