using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using app.Model;
using DBModel=Retrospective.Data.Model;
using AutoMapper;
using Retrospective.Data;
using MongoDB.Bson;

namespace app.Controllers
{
    [Route("api/[controller]")]
    public class TeamController
    {

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        
        private readonly Database database;

        public TeamController(ILogger logger, IMapper mapper,Database database)
        {
            _logger=logger;
            _mapper=mapper;
            this.database=database;
        }


        /// <summary>
        /// returns the users on a team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IEnumerable<User> TeamMembers(string teamId)
        {
          
            
            var teamMember = database.Users.GetTeamUsers(new ObjectId(teamId));
            

            _logger.LogDebug("db returning {0} team members", teamMember.Count);

            var users =_mapper.Map<List<DBModel.User>,List<app.Model.User> >(teamMember);

            return (IEnumerable<User>)users;

        }
    }
}