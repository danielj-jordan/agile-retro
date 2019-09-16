using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using app.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Retrospective.Data.Model;
using Retrospective.Domain;
using Xunit;
using Retrospective.Domain.ModelExtensions;
using app.ModelExtensions;

namespace apptest
{

  [Collection("Controller Test collection")]
  public class TeamControllerTest
  {

    TestFixture fixture;

    TeamManager teamManager;

    public TeamControllerTest(TestFixture fixture)
    {
      this.fixture = fixture;


      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<TeamManager>();

      teamManager = new TeamManager(logger, fixture.Database);
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

      var controller = new app.Controllers.TeamController(logger, teamManager);

      MockHttpContextValid(controller, fixture.Owner);

      var teamMembers = controller.TeamMembers(teamId);

      Assert.True(teamMembers.ToList().Count() > 0);

    }

    [Fact]
    public void GetUserTeams()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();

      var controller = new app.Controllers.TeamController(logger, teamManager);

      MockHttpContextValid(controller, fixture.Owner);
      var teams = controller.Teams();

      Assert.True(teams.Value.ToList().Count() > 0);

    }

    [Fact]
    public void SaveTeam()
    {

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();

      var controller = new app.Controllers.TeamController(logger, teamManager);

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

      var controller = new app.Controllers.TeamController(logger, teamManager);

      MockHttpContextValid(controller, fixture.Owner);

      var teamStart = fixture.TestTeam.ToDomainModel().ToViewModel();

      teamStart.Name += " more";

      var teamEnd = controller.Team(teamStart);

      Assert.Equal(teamStart.TeamId.ToString(), teamEnd.Value.TeamId);

      Assert.Contains("more", teamEnd.Value.Name);

    }

    [Fact]
    public void Uninvite_WithValidInvite_RemovesInvite()
    {
      //arrange
      string teamId = "123";
      string invitedEmail = "test@localhost.com";

      var stubTeamManager = new Mock<ITeamManager>();
      stubTeamManager.Setup(m => m.GetTeam(It.IsAny<string>(),
      It.IsAny<string>())).Returns(new Retrospective.Domain.Model.Team
      {
        TeamId = teamId,
        Name = "test team",
        Members = new Retrospective.Domain.Model.TeamMember[]
         {
           new Retrospective.Domain.Model.TeamMember
           {

           }
         },
        Invited = new Retrospective.Domain.Model.Invitation[]
         {
           new Retrospective.Domain.Model.Invitation
           {
              Name="Joe Cool",
              Email=invitedEmail,
              InviteDate=DateTime.Now,
              Role = Retrospective.Domain.Model.TeamRole.Member

           }
         }

      });

      stubTeamManager.Setup(m => m.SaveTeam(
        It.IsAny<string>(), It.IsAny<Retrospective.Domain.Model.Team>())
      ).Returns(new Retrospective.Domain.Model.Team());

      //act
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();
      var controller = new app.Controllers.TeamController(logger, stubTeamManager.Object);
      MockHttpContextValid(controller, fixture.Owner);

      controller.Uninvite(teamId, new app.Model.Invitation
      {
        Email = invitedEmail
      });

      //assert
      stubTeamManager.Verify(m => m.SaveTeam(
        It.IsAny<string>(),
        It.Is<Retrospective.Domain.Model.Team>(t => t.Invited.Length == 0)), Times.Once);
    }

    [Fact]
    public void Uninvite_WithValidInviteAndNoCurrentInvites_RemovesInvite()
    {
      //arrange
      string teamId = "123";
      string invitedEmail = "test@localhost.com";

      var stubTeamManager = new Mock<ITeamManager>();
      stubTeamManager.Setup(m => m.GetTeam(It.IsAny<string>(),
      It.IsAny<string>())).Returns(new Retrospective.Domain.Model.Team
      {
        TeamId = teamId,
        Name = "test team",
        Members = new Retrospective.Domain.Model.TeamMember[]
         {
           new Retrospective.Domain.Model.TeamMember
           {

           }
         },
        Invited = null
      });

      stubTeamManager.Setup(m => m.SaveTeam(
        It.IsAny<string>(), It.IsAny<Retrospective.Domain.Model.Team>())
      ).Returns(new Retrospective.Domain.Model.Team());

      //act
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();
      var controller = new app.Controllers.TeamController(logger, stubTeamManager.Object);
      MockHttpContextValid(controller, fixture.Owner);

      controller.Uninvite(teamId, new app.Model.Invitation
      {
        Email = invitedEmail
      });

      //assert
      stubTeamManager.Verify(m => m.SaveTeam(
        It.IsAny<string>(),
        It.Is<Retrospective.Domain.Model.Team>(t => t.Invited.Length == 0)), Times.Once);
    }


    [Fact]
    public void Invite_WithValidInvite_AddsInvite()
    {
      //arrange
      string teamId = "123";
      string invitedEmail = "test@localhost.com";

      var stubTeamManager = new Mock<ITeamManager>();
      stubTeamManager.Setup(m => m.GetTeam(It.IsAny<string>(),
      It.IsAny<string>())).Returns(new Retrospective.Domain.Model.Team
      {
        TeamId = teamId,
        Name = "test team",
        Members = new Retrospective.Domain.Model.TeamMember[]
         {
           new Retrospective.Domain.Model.TeamMember
           {

           }
         },
        Invited = new Retrospective.Domain.Model.Invitation[]
         {
         }

      });

      stubTeamManager.Setup(m => m.SaveTeam(
        It.IsAny<string>(), It.IsAny<Retrospective.Domain.Model.Team>())
      ).Returns(new Retrospective.Domain.Model.Team());

      //act
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();
      var controller = new app.Controllers.TeamController(logger, stubTeamManager.Object);
      MockHttpContextValid(controller, fixture.Owner);

      controller.Invite(teamId, new app.Model.Invitation
      {
        Name = "Joe Cool",
        Email = invitedEmail,
        InviteDate = DateTime.Now,
        Role = "Member"
      });

      //assert
      stubTeamManager.Verify(m => m.SaveTeam(
        It.IsAny<string>(),
        It.Is<Retrospective.Domain.Model.Team>(t => t.Invited.Length == 1)), Times.Once);
    }

    [Fact]
    public void MyInvitations_CallGetUserInvitedTeams()
    {
      //arrange
      var stubTeamManager = new Mock<ITeamManager>();

      stubTeamManager.Setup(m => m.GetUser(It.IsAny<string>()))
        .Returns(new Retrospective.Domain.Model.User
        {
          Email = "test@localhost.com"
        });

      stubTeamManager.Setup(m => m.GetUserInvitedTeams(
        It.IsAny<string>(), It.IsAny<string>()))
        .Returns(new List<Retrospective.Domain.Model.Team>{
          new Retrospective.Domain.Model.Team
          {
            TeamId = "123",
            Name = "test team",
            Members = new Retrospective.Domain.Model.TeamMember[0],
            Invited = new Retrospective.Domain.Model.Invitation[0]
          }
        });

      //act
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();
      var controller = new app.Controllers.TeamController(logger, stubTeamManager.Object);
      MockHttpContextValid(controller, fixture.Owner);

      controller.MyInvitations();

      //assert
      stubTeamManager.Verify(m => m.GetUserInvitedTeams(
        It.IsAny<string>(),
        It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public void AcceptInvitation_CallsAccepInvitation()
    {
      //arrange
      string teamId = "123";
      var stubTeamManager = new Mock<ITeamManager>();

      stubTeamManager.Setup(m => m.GetUser(It.IsAny<string>()))
        .Returns(new Retrospective.Domain.Model.User
        {
          Email = "test@localhost.com"
        });

      stubTeamManager.Setup(m => m.AcceptInvitation(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>()))
        .Returns(new Retrospective.Domain.Model.Team
        {
          TeamId = "123",
          Name = "test team",
          Members = new Retrospective.Domain.Model.TeamMember[0],
          Invited = new Retrospective.Domain.Model.Invitation[0]
        });

      //act
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();
      var controller = new app.Controllers.TeamController(logger, stubTeamManager.Object);
      MockHttpContextValid(controller, fixture.Owner);

      controller.AcceptInvitation(teamId);

      //assert
      stubTeamManager.Verify(m => m.AcceptInvitation(
        It.IsAny<string>(),
        It.IsAny<string>(),
        It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void CreateTeam_CallsNewTeam()
    {
      //arrange
      var stubTeamManager = new Mock<ITeamManager>();

      stubTeamManager.Setup(m => m.GetUser(It.IsAny<string>()))
        .Returns(new Retrospective.Domain.Model.User
        {
          Email = "test@localhost.com"
        });

      stubTeamManager.Setup(m => m.NewTeam(It.IsAny<string>()))
        .Returns(new Retrospective.Domain.Model.Team
        {
          TeamId = "123",
          Name = "test team",
          Members = new Retrospective.Domain.Model.TeamMember[0],
          Invited = new Retrospective.Domain.Model.Invitation[0]
        });

      //act
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();
      var controller = new app.Controllers.TeamController(logger, stubTeamManager.Object);
      MockHttpContextValid(controller, fixture.Owner);

      controller.CreateTeam();

      //assert
      stubTeamManager.Verify(m => m.NewTeam(
        It.IsAny<string>()), Times.Once);
    }


    [Fact]
    public void Team_WithValidTeamId_CallsGetTeam()
    {
      //arrange
      string teamId="123";
      var stubTeamManager = new Mock<ITeamManager>();

      stubTeamManager.Setup(m => m.GetUser(It.IsAny<string>()))
        .Returns(new Retrospective.Domain.Model.User
        {
          Email = "test@localhost.com"
        });

      stubTeamManager.Setup(m => m.GetTeam(It.IsAny<string>(),
      It.IsAny<string>())).Returns(new Retrospective.Domain.Model.Team
      {
        TeamId = teamId,
        Name = "test team",
        Members = new Retrospective.Domain.Model.TeamMember[]
         {
           new Retrospective.Domain.Model.TeamMember
           {  
              UserId="345",
              Role = Retrospective.Domain.Model.TeamRole.Member
           }
         },
        Invited = null
      });

      stubTeamManager.Setup(m => m.GetTeamMembers(It.IsAny<string>(), teamId))
        .Returns(new List<Retrospective.Domain.Model.User>{
          new Retrospective.Domain.Model.User
          {
            UserId="345",
            Name ="Joe Cool"
          }
        });

      //act
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController>();
      var controller = new app.Controllers.TeamController(logger, stubTeamManager.Object);
      MockHttpContextValid(controller, fixture.Owner);

      var result = controller.Team(teamId);

      //assert
      Assert.Equal("Joe Cool", result.Value.Members.First().UserName);
    }

  }
}