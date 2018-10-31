using System;
using System.Collections.Generic;
using Xunit;
using Retrospective.Data.Model;
using Retrospective.Data;
using MongoDB.Bson;


namespace retro_db_test
{
    public class DatabaseFixture : IDisposable
    {
        public Database database= new Database("test_datalayer");
        public User owner {get; private set;}
        public Team team {get; private set;}
        public Meeting retrospectiveSession {get; private set;}

        public void Dispose()
        {
        
        }

        public DatabaseFixture()
        {
            Setup();

        }

        private void Setup()
        {
            //create a user to test with
            owner = new User();
            owner.Email="owner@here.com";
            owner.Name="the Owner";

            database.Users.SaveUser(owner);
  

            //create a team to test the subsequent save
            team= new Team();
            team.Name="test team";
            team.Owner = owner.Email;
            
            var members =  new List<string>();
            members.Add(owner.Email);
            team.TeamMembers=members.ToArray();
            
            DataTeam teamData = new DataTeam(database);
            var savedTeam= teamData.Save(team);


            //create a test retrospective session
            Meeting session = new Meeting();
            session.TeamId=(ObjectId)team.Id;
            session.Name ="fixture retrospective session";
            List<Category> categories= new List<Category>();
            categories.Add(new Category(1, "test category 1"));
            categories.Add(new Category(2, "test category 2"));
            categories.Add(new Category(3, "test category 3"));
            session.Categories=categories.ToArray();
            

            DataMeeting retroData = new DataMeeting(database);
            
            retroData.Save(session);
            retrospectiveSession=session;

        }
    
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection: ICollectionFixture<DatabaseFixture>
    {


    }


}