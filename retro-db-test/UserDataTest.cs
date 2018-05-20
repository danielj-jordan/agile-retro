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
    public class UserDataTest
    {
        private DatabaseFixture fixture;

        public UserDataTest(DatabaseFixture fixture)
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
        
            
            UserData userData = new UserData(fixture.database);
            userData.SaveUser(user);


            Console.WriteLine("created user id:{0}", user.Id);
            Assert.True(user.Id!=null);

        }

        [Fact]
        public void UpdateUser()
        {
            ObjectId begin= (ObjectId)fixture.owner.Id;
            fixture.owner.Name+=" more";

            UserData userData= new UserData(fixture.database);
            userData.SaveUser(fixture.owner);

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
            UserData userData = new UserData(fixture.database);
            userData.SaveUser(user);

            List<User> foundbyEmail =userData.FindUserByEmail("nobody@here.com");
            Assert.True(foundbyEmail.Count>0);

            var foundById =userData.GetUser((ObjectId)user.Id);
            Assert.True(foundById.Id==user.Id);
        }
    }   

}