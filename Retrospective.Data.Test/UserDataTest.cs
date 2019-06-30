using System;
using System.Linq;
using Xunit;
using Retrospective.Data.Model;
using Retrospective.Data;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Retrospective.Data.Test
{
    [Collection("Database collection")]
    public class UserDataTest
    {
        private TestFixture fixture;

        public UserDataTest(TestFixture fixture)
        {
            this.fixture=fixture;
        }



        [Fact]
        public void SaveUser()
        {

            User user = new User();
            user.Id=null;
            user.Email="nobody@here.com";
            user.Name="nobody";

            fixture.database.Users.Save(user);

            Console.WriteLine("created user id:{0}", user.Id);
            Assert.True(user.Id!=null);

        }

        [Fact]
        public void UpdateUser()
        {
            ObjectId begin= (ObjectId)fixture.owner.Id;
            fixture.owner.Name+=" more";

            fixture.database.Users.Save(fixture.owner);

            Assert.True(begin==(ObjectId)fixture.owner.Id);
            Assert.Contains(" more", fixture.owner.Name);
        }

        [Fact]
        public void FindUser()
        {
            User user = new User();
            user.Id=null;
            user.Email="nobody@here.com";
            user.Name="nobody";
           
            fixture.database.Users.Save(user);

            List<User> foundbyEmail =fixture.database.Users.FindUserByEmail("nobody@here.com");
            Assert.True(foundbyEmail.Count>0);

            var foundById =fixture.database.Users.Get((ObjectId)user.Id);
            Assert.True(foundById.Id==user.Id);
        }


/* 
        [Fact]
        public void FindUsersOnTeam()
        {

            User user = new User();
            user.Id=null;
            user.Email="nobody@here.com";
            user.Name="nobody";
            fixture.database.Users.Save(user);


            Team team = new Team();
            team.Id=null;
            team.Name="another test team";
            team.Members= new TeamMember[1]{
                new TeamMember{
                    UserId=(ObjectId)user.Id,
                    UserName=user.Name,
                    Role= TeamRole.Member,
                    StartDate=DateTime.UtcNow
                }
            };
            
            DataTeam teamData = new DataTeam(fixture.database);
            teamData.Save(team);

            List<User> found =fixture.database.Users.GetTeamUsers((ObjectId)user.Id);
            Assert.True(found.Count>0);
        }


        */
    }   

}