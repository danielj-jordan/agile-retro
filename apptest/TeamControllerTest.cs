using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using app.Domain;
using Retrospective.Data.Model;
using System.Linq;

namespace apptest
{
    public class TeamControllerTest
    {
        
        AutoMapper.Mapper mapper= null;
        
        public TeamControllerTest()
        {
                
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile<app.Domain.DomainProfile>();
            });

            mapper= new Mapper(config);
            
        }

        [Fact]
        public void GetTeamMembers()
        {


            

            string teamId="5b5a957aaa1a474689a71e4f";
   
            var logger = new Microsoft.Extensions.Logging.Abstractions.NullLogger<app.Controllers.NotesController>();

            var controller = new app.Controllers.TeamController(logger, mapper);

            var teamMembers = controller.TeamMembers(teamId);
            
            Assert.True(teamMembers.ToList().Count()>0);


        }

    }
}