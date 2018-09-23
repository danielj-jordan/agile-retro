using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Retrospective.Data.Model;

namespace Retrospective.Data {

    public class DataMeeting {
        private string collection = "meeting";

        private IDatabase database;

        public DataMeeting (IDatabase database) {
            this.database = database;

        }

        /// <summary>
        /// Save a single retrospective object
        /// </summary>
        /// <param name="retrospective"></param>
        /// <returns></returns>
        public Meeting Save (Meeting meeting) {

            Console.WriteLine("saving");

            if (meeting.Id is null) {
                database.MongoDatabase.GetCollection<Meeting> (collection).InsertOne (meeting);
                
            } else {
                var filter = MongoDB.Driver.Builders<Meeting>.Filter.Eq ("Id", meeting.Id);
                var saved = database.MongoDatabase.GetCollection<Meeting> (collection).ReplaceOne (filter, meeting);

    
             }

            return meeting;
        }


        public List<Meeting> GetMeetings (string teamObjectId) {
            return  this.GetMeetings(new ObjectId(teamObjectId));
        
        }
        

        /// <summary>
        /// Get all Retrospective objects for a given team
        /// </summary>
        /// <param name="teamObjectId"></param>
        /// <returns></returns>
        public List<Meeting> GetMeetings (ObjectId teamObjectId) {
            var sessions = database.MongoDatabase.GetCollection<Meeting> (collection);
            var query = from session in sessions.AsQueryable<Meeting> ()
            where session.TeamId == teamObjectId
            select session;

            return query.ToList<Meeting> ();

        }


       public Meeting Get (string meetingId) {
            return  this.Get(new ObjectId(meetingId));
        
        }

                /// <summary>
        /// Get all Retrospective objects for a given team
        /// </summary>
        /// <param name="teamObjectId"></param>
        /// <returns></returns>
        public Meeting Get (ObjectId retroId) {
            var sessions = database.MongoDatabase.GetCollection<Meeting> (collection);
            var query = from session in sessions.AsQueryable<Meeting> ()
            where session.Id == retroId
            select session;

            return query.FirstOrDefault();

        }
    }

}