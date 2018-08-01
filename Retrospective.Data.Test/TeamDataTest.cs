using System;
using System.Linq;
using Xunit;
using Retrospective.Data.Model;
using Retrospective.Data;
using MongoDB.Bson;
using System.Collections.Generic;

namespace retro_db_test
{
    [Collection("Database collection")]
    public class TeamDataTest
    {
   
        private DatabaseFixture fixture;
        
        
        
        public TeamDataTest(DatabaseFixture fixture)
        {
            this.fixture=fixture;
        }


        [Fact]
        public void SaveTeam()
        {
            Team team= new Team();
                                      
            team.Name="test team";
            team.Owner = fixture.owner.Email;
            DataTeam teamData = new DataTeam(fixture.database);
            var savedTeam= teamData.SaveTeam(team);

            Console.WriteLine("created team id:{0}", savedTeam.Id);
            Assert.True(savedTeam.Id!=null);
        }

        [Fact]
        public void UpdateTeam()
        {
            ObjectId begin= (ObjectId)fixture.team.Id;
            fixture.team.Name+=" more";

            DataTeam teamData= new DataTeam(fixture.database);
            teamData.SaveTeam(fixture.team);

            Assert.True(begin==(ObjectId)fixture.team.Id);
            Assert.Contains(" more", fixture.team.Name);

        }
        

        [Fact]
        public void FindUserOwnedTeams()
        { 
            DataTeam teamData = new DataTeam(fixture.database);
            List<Team> ownedByEmail= teamData.GetOwnedTeams(fixture.owner.Email);
            Assert.True(ownedByEmail.Count>0, "user should own at least one team");            
        }

        [Fact]
        public void FindTeamsHavingUserAsMember()
        {
            DataTeam teamData = new DataTeam(fixture.database);
            List<Team> foundbyEmail =teamData.GetUserTeams(fixture.owner.Email);
            Assert.True(foundbyEmail.Count>0, "user should be a member on at least one team");

        }

    }   
}
