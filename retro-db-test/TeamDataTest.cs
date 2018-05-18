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
        private IDatabase database;
        
        User owner;
        
        public TeamDataTest(DatabaseFixture fixture)
        {
            database=fixture.database;

            Setup();
        }


        private void Setup()
        {

            owner = new User();
            owner.Email="owner@here.com";
            owner.Name="the Owner";
            UserData userData= new UserData(database);
            userData.SaveUser(owner);
            
        }

      



        [Fact]
        public void SaveTeam()
        {
            Team team= new Team();
                                      
            team.Name="test team";
            team.Owner = owner.Email;
            TeamData teamData = new TeamData(database);
            var savedTeam= teamData.SaveTeam(team);

            Console.WriteLine("created team id:{0}", savedTeam.Id);
            Assert.True(savedTeam.Id!=null);

        }

        [Fact]
        public void FindUserTeams()
        { 
            Team team= new Team();
            team.Name="test team";
            team.Owner = owner.Email;
            
            var members =  new List<string>();
            members.Add("owner@here.com");
            team.TeamMembers=members.ToArray<string>();
            
            TeamData teamData = new TeamData(database);
            var savedTeam= teamData.SaveTeam(team);
            
           
            List<Team> foundbyEmail =teamData.GetUserTeams("owner@here.com");
            Assert.True(foundbyEmail.Count>0);
            
                        
        }
    }   
}
