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
        private IDatabase database;

        public UserDataTest(DatabaseFixture fixture)
        {
            database=fixture.database;
        }



        [Fact]
        public void SaveUser()
        {

            User user = new User();
            user.Id=null;
            user.Email="nobody@here.com";
            user.Name="nobody";
            user.Teams= new ObjectId[0];
        
            
            UserData userData = new UserData(database);
            userData.SaveUser(user);


            Console.WriteLine("created user id:{0}", user.Id);
            Assert.True(user.Id!=null);

        }

        [Fact]
        public void FindUser()
        {
            User user = new User();
            user.Id=null;
            user.Email="nobody@here.com";
            user.Name="nobody";
            user.Teams= new ObjectId[0];
            UserData userData = new UserData(database);
            userData.SaveUser(user);


            List<User> foundbyEmail =userData.FindUserByEmail("nobody@here.com");
            Assert.True(foundbyEmail.Count>0);


            var foundById =userData.GetUser((ObjectId)user.Id);
            Assert.True(foundById.Id==user.Id);
        }
    }   

}