using System;
using AutoMapper;
using Xunit;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;
using Moq;
using DBModel = Retrospective.Data.Model;
using Retrospective.Data;
using System.Collections.Generic;
using System.Linq;

namespace Retrospective.Domain.Test
{

  [Collection("Domain Test collection")]
  public class TeamManagerTest
  {

    TestFixture fixture;

    public TeamManagerTest(TestFixture fixture)
    {
      this.fixture = fixture;
    }

    [Fact]
    public void GetUserTeams()
    {

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager>();
      var manager = new Retrospective.Domain.TeamManager(logger, fixture.Database);
      var teams = manager.GetUserTeams(fixture.SampleUser.Id.ToString(), fixture.SampleUser.Id.ToString());
      Assert.True(teams.Count > 0);

    }

    [Fact]
    public void GetTeamMembers()
    {

      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager>();
      var manager = new Retrospective.Domain.TeamManager(logger, fixture.Database);
      var users = manager.GetTeamMembers(fixture.SampleUser.Id.ToString(), fixture.TeamId.ToString());
      Assert.True(users.Count > 0);

    }

    [Fact]
    public void GetTeam()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager>();
      var manager = new Retrospective.Domain.TeamManager(logger, fixture.Database);
      Retrospective.Domain.Model.Team team = manager.GetTeam(fixture.SampleUser.Id.ToString(), fixture.TeamId.ToString());
      Assert.Equal(team.TeamId, fixture.TeamId.ToString());
    }

    [Fact]
    public void GetTeam_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager>();
      var manager = new Retrospective.Domain.TeamManager(logger, fixture.Database);

      Assert.Throws<Exception.AccessDenied>(
          () =>
          {
            manager.GetTeam("", fixture.TeamId.ToString());
          }
      );
    }

    [Fact]
    public void SaveNewTeam()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager>();
      var manager = new Retrospective.Domain.TeamManager(logger, fixture.Database);

      Retrospective.Domain.Model.Team team = new Retrospective.Domain.Model.Team();
      team.TeamId = null;
      team.Name = "test team";
      team.Members = new Domain.Model.TeamMember[]
      {
                new Domain.Model.TeamMember
                {
                    UserId= fixture.OwnerUser.Id.ToString(),
                    Role = Model.TeamRole.Owner
                }
      };

      var result = manager.SaveTeam(fixture.OwnerUser.Id.ToString(), team);

      Assert.True(!String.IsNullOrEmpty(result.TeamId));
    }

    [Fact]
    public void UpdateTeam()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager>();
      var manager = new Retrospective.Domain.TeamManager(logger, fixture.Database);

      var teamStart = manager.GetTeam(fixture.OwnerUser.Id.ToString(), fixture.TeamId.ToString());
      teamStart.Name += "more";
      var teamEnd = manager.SaveTeam(fixture.OwnerUser.Id.ToString(), teamStart);

      Assert.Equal(teamStart.TeamId, teamEnd.TeamId);

      Assert.Contains("more", teamEnd.Name);

      Assert.True(!String.IsNullOrEmpty(teamEnd.TeamId));
    }

    [Fact]
    public void UpdateTeam_NoAccess()
    {
      var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager>();
      var manager = new Retrospective.Domain.TeamManager(logger, fixture.Database);

      var teamStart = manager.GetTeam(fixture.SampleUser.Id.ToString(), fixture.TeamId.ToString());
      teamStart.Name += "more";

      Assert.Throws<Exception.AccessDenied>(
          () =>
          {
            manager.SaveTeam("", teamStart);
          }
      );
    }

    [Fact]
    public void GetUserInvitedTeams_WithExistingInvite_ReturnsTeams()
    {
      string activeUser = ObjectId.GenerateNewId().ToString();
      string email = "test@here.com";

      //arrange
      var stubLogger = new Mock<ILogger<Retrospective.Domain.TeamManager>>();
      var stubDatabase = new Mock<IDatabase>();
      var stubDatabaseTeams = new Mock<IDataTeam>();

      stubDatabase.SetupGet(m => m.Teams).Returns(stubDatabaseTeams.Object);
      stubDatabaseTeams.Setup(m => m.GetTeamInvitations(It.IsAny<string>())).Returns(
          new List<DBModel.Team>
          {
              new DBModel.Team
                {
                    Id = ObjectId.GenerateNewId(),

                    Members = new DBModel.TeamMember[]{
                                new DBModel.TeamMember{
                                    UserId=ObjectId.GenerateNewId(),
                                    StartDate=DateTime.UtcNow,
                                    Role = DBModel.TeamRole.Member
                                }
                    },
                    Invited= new DBModel.Invitation[]{
                        new DBModel.Invitation{
                            Email=email,
                            InviteDate=DateTime.UtcNow,
                            Role=DBModel.TeamRole.Member
                        }
                    }
                }
          }
      );

      //act
      var teamManager = new TeamManager(stubLogger.Object, stubDatabase.Object);
      var teams = teamManager.GetUserInvitedTeams(activeUser, email);

      //assert
      Assert.True(teams.Count == 1);
    }

    [Fact]
    public void GetUserInvitedTeams_WithOneInviteButAsExistingTeamMember_ReturnsNoTeams()
    {
      string activeUser = ObjectId.GenerateNewId().ToString();
      string email = "test@here.com";

      //arrange
      var stubLogger = new Mock<ILogger<Retrospective.Domain.TeamManager>>();
      var stubDatabase = new Mock<IDatabase>();
      var stubDatabaseTeams = new Mock<IDataTeam>();

      stubDatabase.SetupGet(m => m.Teams).Returns(stubDatabaseTeams.Object);
      stubDatabaseTeams.Setup(m => m.GetTeamInvitations(It.IsAny<string>())).Returns(
          new List<DBModel.Team>
          {
              new DBModel.Team
                {
                    Id = ObjectId.GenerateNewId(),

                    Members = new DBModel.TeamMember[]{
                                new DBModel.TeamMember{
                                    UserId=new ObjectId(activeUser),
                                    StartDate=DateTime.UtcNow,
                                    Role = DBModel.TeamRole.Member
                                }
                    },
                    Invited= new DBModel.Invitation[]{
                        new DBModel.Invitation{
                            Email=email,
                            InviteDate=DateTime.UtcNow,
                            Role=DBModel.TeamRole.Member
                        }
                    }
                }
          }
      );

      //act
      var teamManager = new TeamManager(stubLogger.Object, stubDatabase.Object);
      var teams = teamManager.GetUserInvitedTeams(activeUser, email);

      //assert
      Assert.True(teams.Count == 0);
    }

    [Fact]
    public void AcceptInvitation_WithValidInvite_ReturnsUpdatedTeam()
    {
      string activeUser = ObjectId.GenerateNewId().ToString();
      string teamId = ObjectId.GenerateNewId().ToString();
      string email = "test@here.com";


      //arrange
      var stubLogger = new Mock<ILogger<Retrospective.Domain.TeamManager>>();
      var stubDatabase = new Mock<IDatabase>();
      var stubDatabaseTeams = new Mock<IDataTeam>();

      var team = new DBModel.Team
      {
        Id = ObjectId.GenerateNewId(),

        Members = new DBModel.TeamMember[]{
                },
        Invited = new DBModel.Invitation[]{
                    new DBModel.Invitation{
                        Email=email,
                        InviteDate=DateTime.UtcNow,
                        Role=DBModel.TeamRole.Member
                    }
                }
      };

      stubDatabase.SetupGet(m => m.Teams).Returns(stubDatabaseTeams.Object);
      stubDatabaseTeams.Setup(m => m.Get(It.IsAny<string>())).Returns(team);
      stubDatabaseTeams.Setup(m => m.Save(It.Is<DBModel.Team>(t => t.Members.Count() == 1 && t.Invited.Count() == 0)))
      .Returns(team).Verifiable();

      //act
      var teamManager = new TeamManager(stubLogger.Object, stubDatabase.Object);
      var savedTeam = teamManager.AcceptInvitation(activeUser, teamId, email);

      //assert
      stubDatabaseTeams.VerifyAll();
    }

    [Fact]
    public void AcceptInvitation_WithValidInviteButAlreadyMember_ReturnsUpdatedTeam()
    {
      string activeUser = ObjectId.GenerateNewId().ToString();
      string teamId = ObjectId.GenerateNewId().ToString();
      string email = "test@here.com";


      //arrange
      var stubLogger = new Mock<ILogger<Retrospective.Domain.TeamManager>>();
      var stubDatabase = new Mock<IDatabase>();
      var stubDatabaseTeams = new Mock<IDataTeam>();

      var team = new DBModel.Team
      {
        Id = ObjectId.GenerateNewId(),

        Members = new DBModel.TeamMember[]{
            new DBModel.TeamMember{
              UserId= new ObjectId(activeUser),
              Role=DBModel.TeamRole.Member,
              StartDate=DateTime.UtcNow
                }
        },
        Invited = new DBModel.Invitation[]{
                    new DBModel.Invitation{
                        Email=email,
                        InviteDate=DateTime.UtcNow,
                        Role=DBModel.TeamRole.Member
                    }
                }
      };

      stubDatabase.SetupGet(m => m.Teams).Returns(stubDatabaseTeams.Object);
      stubDatabaseTeams.Setup(m => m.Get(It.IsAny<string>())).Returns(team);
      stubDatabaseTeams.Setup(m => m.Save(It.Is<DBModel.Team>(t => t.Members.Count() == 1 && t.Invited.Count() == 0)))
      .Returns(team).Verifiable();

      //act
      var teamManager = new TeamManager(stubLogger.Object, stubDatabase.Object);
      var savedTeam = teamManager.AcceptInvitation(activeUser, teamId, email);

      //assert
      stubDatabaseTeams.VerifyAll();
    }
  }
}