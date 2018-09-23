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
    [ApiController]
    public class TeamController: Controller
    {

        private readonly ILogger<TeamController>  _logger;
        private readonly IMapper _mapper;
        
        private readonly Database database;

        public TeamController(ILogger<TeamController> logger, IMapper mapper,Database database)
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

        /// <summary>
        /// returns the teams for a user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [HttpGet("[action]/{email}")]
        public ActionResult<IEnumerable<Team>> Teams(string email)
        {
            if(string.IsNullOrEmpty(email)){
                _logger.LogWarning("no user supplied");
                return new BadRequestResult();
            }

            _logger.LogDebug("looking for {0}", email);
            var teams = database.Teams.GetUserTeams(email);
            
            _logger.LogDebug("db returning {0} teams for the user", teams.Count);

            return (_mapper.Map<List<DBModel.Team>,List<app.Model.Team> >(teams)).ToList();

        }

         /// <summary>
        /// returns the retrospective meetings for this id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public ActionResult<Team> Team(string id)
        {
            if(string.IsNullOrEmpty(id)){
                _logger.LogWarning("no team id supplied");
                return new BadRequestResult();
            }

            _logger.LogDebug("looking for {0}", id);
            var team = database.Teams.Get(id);
            
            return (_mapper.Map<DBModel.Team,app.Model.Team> (team));

        }

    }
}