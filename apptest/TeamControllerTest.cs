using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using app.Domain;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Retrospective.Data.Model;
using Retrospective.Domain;
using Xunit;

namespace apptest
{

  [Collection("Controller Test collection")]
  public class TeamControllerTest
  {

    AutoMapper.Mapper mapper = null;
    TestFixture fixture;

    TeamManager teamManager;

    public TeamControllerTest(TestFixture fixture)
    {
      this.fixture = fixture;
      var config = new MapperConfiguration(c =>
      {
        c.AddProfile<app.Domain.DomainProfile>();
      });

      mapper = new Mapper(config);

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<TeamManager>();

      teamManager = new TeamManager(logger,  fixture.Database);
    }

    private void MockHttpContextValid(app.Controllers.TeamController controller, User user)
    {
      controller.ControllerContext = new ControllerContext();
      controller.ControllerContext.HttpContext = new DefaultHttpContext();
      controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
      {
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
      }, "someAuthTypeName"));

    }

    [Fact]
    public void GetTeamMembers()
    {

      string teamId = fixture.TeamId.ToString();

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();

      var controller = new app.Controllers.TeamController(logger, mapper, teamManager);

      MockHttpContextValid(controller, fixture.Owner);

      var teamMembers = controller.TeamMembers(teamId);

      Assert.True(teamMembers.ToList().Count() > 0);

    }

    [Fact]
    public void GetUserTeams()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();

      var controller = new app.Controllers.TeamController(logger, mapper, teamManager);

      MockHttpContextValid(controller, fixture.Owner);
      var teams = controller.Teams();

      Assert.True(teams.Value.ToList().Count() > 0);

    }

    [Fact]
    public void SaveTeam()
    {

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();

      var controller = new app.Controllers.TeamController(logger, mapper, teamManager);

      app.Model.Team team = new app.Model.Team();
      team.Name = "test team";

      MockHttpContextValid(controller, fixture.Owner);
      var result = controller.Team(team);

      Assert.True(!String.IsNullOrEmpty(result.Value.TeamId));

    }

    [Fact]
    public void UpdateTeam()
    {

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();

      var controller = new app.Controllers.TeamController(logger, mapper, teamManager);

      MockHttpContextValid(controller, fixture.Owner);

      var teamStart = mapper.DefaultContext.Mapper.Map<Retrospective.Domain.Model.Team, app.Model.Team>(
          mapper.DefaultContext.Mapper.Map<Retrospective.Data.Model.Team, Retrospective.Domain.Model.Team>(fixture.TestTeam));

      teamStart.Name += " more";

      var teamEnd = controller.Team(teamStart);

      Assert.Equal(teamStart.TeamId.ToString(), teamEnd.Value.TeamId);

      Assert.Contains("more", teamEnd.Value.Name);

    }

  }
}