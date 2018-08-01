using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Retrospective.Data.Model;
using Retrospective.Data;
using MongoDB.Bson;

namespace retro_db_test
{
    [Collection("Database collection")]
    public class RetrospectiveSessionDataTest
    {
        private DatabaseFixture fixture;

        public RetrospectiveSessionDataTest(DatabaseFixture fixture)
        {
            this.fixture=fixture;
        }
        
        [Fact]
        public void SaveRestrospectiveSession()
        {
            RetrospectiveSession session = new RetrospectiveSession();
            session.TeamId=(ObjectId)fixture.team.Id;
            session.Name ="test retrospective session";
            List<Category> categories= new List<Category>();
            categories.Add(new Category(1, "test category 1"));
            categories.Add(new Category(2, "test category 2"));
            categories.Add(new Category(3, "test category 3"));
            session.Categories=categories.ToArray();
            

            DataSession retroData = new DataSession(fixture.database);
            var savedRetro= retroData.SaveRetrospectiveSession(session);
            Console.WriteLine("created session id:{0}", session.Id);
            Assert.True(session.Id!=null);
        }


        [Fact]
        public void UpdateRestrospectiveSession()
        {

            ObjectId begin= (ObjectId)fixture.retrospectiveSession.Id;
            fixture.retrospectiveSession.Name+=" more";

            DataSession sessionData= new DataSession(fixture.database);
            sessionData.SaveRetrospectiveSession(fixture.retrospectiveSession);

            Assert.True(begin==(ObjectId)fixture.retrospectiveSession.Id);
            Assert.Contains(" more", fixture.retrospectiveSession.Name);
        }

        [Fact]
        public void TestGetTeamRetrospectiveSessions()
        {
            ObjectId teamId= (ObjectId)fixture.team.Id;
            DataSession sessionData= new DataSession(fixture.database);

            List<RetrospectiveSession> sessions= sessionData.GetTeamRetrospectiveSessions(teamId);
            Assert.True(sessions.Count()>0);
            foreach(var session in sessions)
            {
                Assert.True(session.TeamId==teamId);
            }


        }

    }
}