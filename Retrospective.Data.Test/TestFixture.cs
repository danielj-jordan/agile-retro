using System;
using System.Collections.Generic;
using Xunit;
using Retrospective.Data.Model;
using Retrospective.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core;


namespace Retrospective.Data.Test
{
    public class TestFixture : IDisposable
    {
        public User owner {get; private set;}
        public Team team {get; private set;}
        public Meeting meeting {get; private set;}

        public Retrospective.Data.Database database { get; private set; }

        public void Dispose()
        {
        
        }

        public TestFixture()
        {
            string connectionString =  Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");

            if(String.IsNullOrEmpty(connectionString))
            {
                 connectionString= "mongodb://127.0.0.1:27017";
            }
            string databaseName = "test_datalayer";

            //start with an empty database
            var client = new MongoClient (connectionString);
            client.DropDatabase (databaseName);

            //connect to the database
            Retrospective.Data.Database database = new Database (connectionString, databaseName);
            this.database = database;
            IntitialTestRecords();

        }


        private void IntitialTestRecords()
        {
            //create a user to test with
            owner = new User();
            owner.Email="owner@here.com";
            owner.Name="the Owner";

            database.Users.Save(owner);
  

            //create a team to test the subsequent save
            team= new Team();
            team.Name="test team";
    
            
            var members =  new List<TeamMember>();
            members.Add(new TeamMember(){
                UserId = (ObjectId) owner.Id,
                Role =  TeamRole.Owner,
                StartDate = DateTime.UtcNow
            });
            team.Members=members.ToArray();
            
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
            meeting=session;

        }
    
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection: ICollectionFixture<TestFixture>
    {


    }


}