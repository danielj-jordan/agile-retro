using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using MongoDB.Bson;

using app.Model;

using DomainModel=Retrospective.Domain.Model;
using Retrospective.Domain;

using Retrospective.Data;
using DBModel=Retrospective.Data.Model;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController: Controller
    {

        private readonly ILogger<TeamController>  _logger;
        private readonly IMapper _mapper;
        
        private readonly Database database;

        private readonly TeamManager teamManager;

        public TeamController(ILogger<TeamController> logger, IMapper mapper,Database database, TeamManager teamManager)
        {
            _logger=logger;
            _mapper=mapper;
            this.database=database;
            this.teamManager=teamManager;
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
        [Authorize]
        [HttpGet("[action]/{email}")]
        public ActionResult<IEnumerable<Team>> Teams(string email)
        {
            if(string.IsNullOrEmpty(email)){
                _logger.LogWarning("no user supplied");
                return new BadRequestResult();
            }

            _logger.LogDebug("authenticated user:" + HttpContext.User);

           var teams = teamManager.GetUserTeams(email);


            return (_mapper.Map<List<DomainModel.Team>,List<app.Model.Team> >(teams)).ToList();

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


        /// <summary>
        /// saves the team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public ActionResult<Team> Team([FromBody] Team team)
        {
            /* 
            if(string.IsNullOrEmpty(team.TeamId)){
                _logger.LogWarning("no team id supplied for meeting");
                return new BadRequestResult();
            }
            */

            var saved = database.Teams.Save(_mapper.Map<app.Model.Team, DBModel.Team>(team));
            return (_mapper.Map<DBModel.Team,app.Model.Team> (saved));

        }



    }
}