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

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController: Controller
    {

        private readonly ILogger<TeamController>  _logger;
        private readonly IMapper _mapper;
        private readonly TeamManager teamManager;


        public TeamController(ILogger<TeamController> logger, IMapper mapper,TeamManager teamManager)
        {
            _logger=logger;
            _mapper=mapper;
            this.teamManager=teamManager;
        }

        private string GetActiveUser(){
            return HttpContext.User.Identity.Name;
        }


        /// <summary>
        /// returns the users on a team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("[action]")]
        public IEnumerable<User> TeamMembers(string teamId)
        {

            _logger.LogDebug("active user " + HttpContext.User.ToString());

            
            var teamMembers = teamManager.GetTeamMembers(GetActiveUser(),teamId);
           
           // _logger.LogDebug("db returning {0} team members", team.TeamMembers.);

            var users =_mapper.Map<List<DomainModel.User>,List<app.Model.User> >(teamMembers);

            return (IEnumerable<User>)users;

        }

        /// <summary>
        /// returns the teams for a user
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<Team>> Teams()
        {
            _logger.LogInformation(GetActiveUser());
            var teams = teamManager.GetUserTeams(GetActiveUser(), GetActiveUser());
            return (_mapper.Map<List<DomainModel.Team>,List<app.Model.Team> >(teams)).ToList();

        }

         /// <summary>
        /// returns the retrospective meetings for this id
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("[action]/{id}")]
        public ActionResult<Team> Team(string id)
        {
            if(string.IsNullOrEmpty(id)){
                _logger.LogWarning("no team id supplied");
                return new BadRequestResult();
            }

            var team = teamManager.GetTeam(GetActiveUser(), id);
            
            return (_mapper.Map<DomainModel.Team,app.Model.Team> (team));

        }


        /// <summary>
        /// saves the team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("[action]")]
        public ActionResult<Team> Team([FromBody] Team team)
        {
            
            var user = HttpContext.User.ToString();

            var saved= teamManager.SaveTeam(GetActiveUser(),_mapper.Map<app.Model.Team,DomainModel.Team>(team));

            return (_mapper.Map<DomainModel.Team,app.Model.Team> (saved));

        }



    }
}