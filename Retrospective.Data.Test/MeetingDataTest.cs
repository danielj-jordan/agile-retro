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
    public class MeetingDataTest
    {
        private DatabaseFixture fixture;

        public MeetingDataTest(DatabaseFixture fixture)
        {
            this.fixture=fixture;
        }
        
        [Fact]
        public void SaveMeeting()
        {
            Meeting session = new Meeting();
            session.TeamId=(ObjectId)fixture.team.Id;
            session.Name ="test retrospective session";
            List<Category> categories= new List<Category>();
            categories.Add(new Category(1, "test category 1"));
            categories.Add(new Category(2, "test category 2"));
            categories.Add(new Category(3, "test category 3"));
            session.Categories=categories.ToArray();
            

            //DataSession retroData = new DataSession(fixture.database);
            var savedRetro= fixture.database.Meetings.Save(session);
            //var savedRetro= retroData.Save(session);
            Console.WriteLine("created session id:{0}", session.Id);
            Assert.True(session.Id!=null);
        }


        [Fact]
        public void UpdateMeeting()
        {

            ObjectId begin= (ObjectId)fixture.retrospectiveSession.Id;
            fixture.retrospectiveSession.Name+=" more";

            fixture.database.Meetings.Save(fixture.retrospectiveSession);

            Assert.True(begin==(ObjectId)fixture.retrospectiveSession.Id);
            Assert.Contains(" more", fixture.retrospectiveSession.Name);
        }

        [Fact]
        public void TestGetMeetings()
        {
            ObjectId teamId= (ObjectId)fixture.team.Id;
            DataMeeting sessionData= new DataMeeting(fixture.database);

            List<Meeting> sessions= sessionData.GetMeetings(teamId);
            Assert.True(sessions.Count()>0);
            foreach(var session in sessions)
            {
                Assert.True(session.TeamId==teamId);
            }


        }

    }
}