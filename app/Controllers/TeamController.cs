using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using app.Model;
using DomainModel = Retrospective.Domain.Model;
using Retrospective.Domain;
using app.ModelExtensions;

namespace app.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TeamController : Controller
  {

    private readonly ILogger<TeamController> _logger;
    private readonly TeamManager teamManager;


    public TeamController(ILogger<TeamController> logger, TeamManager teamManager)
    {
      _logger = logger;
      this.teamManager = teamManager;
    }

    private string GetActiveUserId()
    {
      return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
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


      var teamMembers = teamManager.GetTeamMembers(GetActiveUserId(), teamId);

      // _logger.LogDebug("db returning {0} team members", team.TeamMembers.);
      var users = teamMembers.Select(m => m.ToViewModel()).ToList();

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
      _logger.LogDebug("The active user is: {0}", GetActiveUserId());
      var teams = teamManager.GetUserTeams(GetActiveUserId(), GetActiveUserId());
      if (teams == null || teams.Count == 0)
      {
        _logger.LogInformation("There are no teams for user {0}", GetActiveUserId());
      }
      return teams.Select(t => t.ToViewModel()).ToList();

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
      if (string.IsNullOrEmpty(id))
      {
        _logger.LogWarning("no team id supplied");
        return new BadRequestResult();
      }

      var team = teamManager.GetTeam(GetActiveUserId(), id);

      return team.ToViewModel();

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

      var saved = teamManager.SaveTeam(GetActiveUserId(),team.ToDomainModel());

      return saved.ToViewModel();

    }



  }
}