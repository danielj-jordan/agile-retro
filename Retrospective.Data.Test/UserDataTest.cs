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
            user.Teams= new ObjectId[0];
        

            fixture.database.Users.SaveUser(user);


            Console.WriteLine("created user id:{0}", user.Id);
            Assert.True(user.Id!=null);

        }

        [Fact]
        public void UpdateUser()
        {
            ObjectId begin= (ObjectId)fixture.owner.Id;
            fixture.owner.Name+=" more";

            fixture.database.Users.SaveUser(fixture.owner);

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
            user.Teams= new ObjectId[0];

            fixture.database.Users.SaveUser(user);

            List<User> foundbyEmail =fixture.database.Users.FindUserByEmail("nobody@here.com");
            Assert.True(foundbyEmail.Count>0);

            var foundById =fixture.database.Users.GetUser((ObjectId)user.Id);
            Assert.True(foundById.Id==user.Id);
        }


        [Fact]
        public void FindUsersOnTeam()
        {
            Team team = new Team();
            team.Id=null;
            team.Name="another test team";
            
            DataTeam teamData = new DataTeam(fixture.database);
            teamData.Save(team);

            User user = new User();
            user.Id=null;
            user.Email="nobody@here.com";
            user.Name="nobody";
            user.Teams= new ObjectId[1];
            user.Teams[0]= (ObjectId)team.Id;


            fixture.database.Users.SaveUser(user);

            List<User> found =fixture.database.Users.GetTeamUsers((ObjectId)team.Id);
            Assert.True(found.Count>0);

        }
    }   

}