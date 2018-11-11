using System;
using AutoMapper;
using Xunit;

namespace Retrospective.Domain.Test {

    [Collection ("Domain Test collection")]
    public class TeamManagerTest {

        AutoMapper.Mapper mapper = null;
        TestFixture fixture;

        public TeamManagerTest (TestFixture fixture) {
            this.fixture = fixture;
            var config = new MapperConfiguration (c => {
                c.AddProfile<Retrospective.Domain.DomainProfile> ();
            });

            mapper = new Mapper (config);

        }

        [Fact]
        public void GetUserTeams () {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger, mapper, fixture.Database);
            var teams = manager.GetUserTeams (fixture.SampleUser, fixture.SampleUser);
            Assert.True (teams.Count > 0);

        }

        [Fact]
        public void GetTeamMembers () {

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger, mapper, fixture.Database);
            var users = manager.GetTeamMembers (fixture.SampleUser, fixture.TeamId.ToString ());
            Assert.True (users.Count > 0);

        }

        [Fact]
        public void GetTeam () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger, mapper, fixture.Database);
            Retrospective.Domain.Model.Team team = manager.GetTeam (fixture.SampleUser, fixture.TeamId.ToString ());
            Assert.Equal (team.TeamId, fixture.TeamId.ToString ());
        }

        [Fact]
        public void GetTeam_NoAccess () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger, mapper, fixture.Database);

            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.GetTeam ("", fixture.TeamId.ToString ());
                }
            );
        }

        [Fact]
        public void SaveNewTeam () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger, mapper, fixture.Database);

            Retrospective.Domain.Model.Team team = new Retrospective.Domain.Model.Team ();
            team.TeamId = this.fixture.TeamId.ToString ();
            team.Name = "test team";
            team.Owner = fixture.Owner;

            var result = manager.SaveTeam (fixture.Owner, team);

            Assert.True (!String.IsNullOrEmpty (result.TeamId));
        }

        [Fact]
        public void UpdateTeam () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger, mapper, fixture.Database);

            var teamStart = manager.GetTeam (fixture.Owner, fixture.TeamId.ToString ());
            teamStart.Name += "more";
            var teamEnd = manager.SaveTeam (fixture.Owner, teamStart);

            Assert.Equal (teamStart.TeamId, teamEnd.TeamId);

            Assert.Contains ("more", teamEnd.Name);

            Assert.True (!String.IsNullOrEmpty (teamEnd.TeamId));
        }

        [Fact]
        public void UpdateTeam_NoAccess () {
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<Retrospective.Domain.TeamManager> ();
            var manager = new Retrospective.Domain.TeamManager (logger, mapper, fixture.Database);

            var teamStart = manager.GetTeam (fixture.SampleUser, fixture.TeamId.ToString ());
            teamStart.Name += "more";

            Assert.Throws<Exception.AccessDenied> (
                () => {
                    manager.SaveTeam ("", teamStart);
                }
            );
        }

    }
}