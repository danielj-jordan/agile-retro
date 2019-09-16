using System.Linq;
using Microsoft.Extensions.Logging;
using Retrospective.Data;
using DomainModel = Retrospective.Domain.Model;
using DBModel = Retrospective.Data.Model;
using Retrospective.Domain.ModelExtensions;

namespace Retrospective.Domain
{
    public interface IUserManager: IBaseManager
    {
         DomainModel.User GetUserFromEmail(string email);
         DomainModel.User UpdateUser(DomainModel.User user);

    }
}