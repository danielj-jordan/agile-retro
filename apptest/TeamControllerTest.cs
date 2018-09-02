using System;
using System.Linq;
using System.Collections.Generic;
using app.Domain;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Retrospective.Data.Model;
using Xunit;

namespace apptest {

    [Collection ("Controller Test collection")]
    public class TeamControllerTest {

        AutoMapper.Mapper mapper = null;
        TestFixture fixture;

        public TeamControllerTest (TestFixture fixture) {
            this.fixture = fixture;
            var config = new MapperConfiguration (c => {
                c.AddProfile<app.Domain.DomainProfile> ();
            });

            mapper = new Mapper (config);

        }

        [Fact]
        public void GetTeamMembers () {

            string teamId = fixture.TeamId.ToString ();

            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController> ();

            var controller = new app.Controllers.TeamController (logger, mapper, fixture.Database);

            var teamMembers = controller.TeamMembers (teamId);

            Assert.True (teamMembers.ToList ().Count () > 0);

        }

        [Fact]
        public void GetUserTeams () {



            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.TeamController> ();

            var controller = new app.Controllers.TeamController (logger, mapper, fixture.Database);

            var teams = controller.Teams (fixture.SampleUser);

            Assert.True (teams.Value.ToList ().Count () > 0);

        }


    }
}