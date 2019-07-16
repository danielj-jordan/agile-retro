using System;
using AutoMapper;
using Xunit;
using MongoDB.Bson;

namespace Retrospective.Domain.Test {

    [Collection ("Domain Test collection")]
    public class TeamManagerTest {

        TestFixture fixture;

        public TeamManagerTest (TestFixture fixture) {
            this.fixture = fixture;
        }

        [Fact]
        public void GetUserTeams () {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger,  fixture.Database);
            var teams = manager.GetUserTeams (fixture.SampleUser.Id.ToString(), fixture.SampleUser.Id.ToString());
            Assert.True (teams.Count > 0);

        }

        [Fact]
        public void GetTeamMembers () {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger,  fixture.Database);
            var users = manager.GetTeamMembers (fixture.SampleUser.Id.ToString(), fixture.TeamId.ToString ());
            Assert.True (users.Count > 0);

        }

        [Fact]
        public void GetTeam () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger,  fixture.Database);
            Retrospective.Domain.Model.Team team = manager.GetTeam (fixture.SampleUser.Id.ToString(), fixture.TeamId.ToString ());
            Assert.Equal (team.TeamId, fixture.TeamId.ToString ());
        }

        [Fact]
        public void GetTeam_NoAccess () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger,  fixture.Database);

            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.GetTeam ("", fixture.TeamId.ToString ());
                }
            );
        }

        [Fact]
        public void SaveNewTeam () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger,  fixture.Database);

            Retrospective.Domain.Model.Team team = new Retrospective.Domain.Model.Team ();
            team.TeamId = null;
            team.Name = "test team";
            team.Members= new Domain.Model.TeamMember[]
            {
                new Domain.Model.TeamMember
                {
                    UserId= fixture.OwnerUser.Id.ToString(),
                    Role = Model.TeamRole.Owner
                }
            };

            var result = manager.SaveTeam (fixture.OwnerUser.Id.ToString(), team);

            Assert.True (!String.IsNullOrEmpty (result.TeamId));
        }

        [Fact]
        public void UpdateTeam () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger,  fixture.Database);

            var teamStart = manager.GetTeam (fixture.OwnerUser.Id.ToString(), fixture.TeamId.ToString ());
            teamStart.Name += "more";
            var teamEnd = manager.SaveTeam (fixture.OwnerUser.Id.ToString(), teamStart);

            Assert.Equal (teamStart.TeamId, teamEnd.TeamId);

            Assert.Contains ("more", teamEnd.Name);

            Assert.True (!String.IsNullOrEmpty (teamEnd.TeamId));
        }

        [Fact]
        public void UpdateTeam_NoAccess () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger,  fixture.Database);

            var teamStart = manager.GetTeam (fixture.SampleUser.Id.ToString(), fixture.TeamId.ToString ());
            teamStart.Name += "more";

            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.SaveTeam ("", teamStart);
                }
            );
        }

    }
}