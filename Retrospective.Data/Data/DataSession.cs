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

    public class DataSession {
        private string collection = "session";

        private IDatabase database;

        public DataSession (IDatabase database) {
            this.database = database;

        }

        /// <summary>
        /// Save a single retrospective object
        /// </summary>
        /// <param name="retrospective"></param>
        /// <returns></returns>
        public RetrospectiveSession SaveRetrospectiveSession (RetrospectiveSession retrospective) {

            if (retrospective.Id is null) {
                database.MongoDatabase.GetCollection<RetrospectiveSession> (collection).InsertOne (retrospective);
            } else {
                var filter = MongoDB.Driver.Builders<RetrospectiveSession>.Filter.Eq ("Id", retrospective.Id);
                var saved = database.MongoDatabase.GetCollection<RetrospectiveSession> (collection).ReplaceOne (filter, retrospective);
            }

            return retrospective;
        }

        /// <summary>
        /// Get all Retrospective objects for a given team
        /// </summary>
        /// <param name="teamObjectId"></param>
        /// <returns></returns>
        public List<RetrospectiveSession> GetTeamRetrospectiveSessions (ObjectId teamObjectId) {
            var sessions = database.MongoDatabase.GetCollection<RetrospectiveSession> (collection);
            var query = from session in sessions.AsQueryable<RetrospectiveSession> ()
            where session.TeamId == teamObjectId
            select session;

            return query.ToList<RetrospectiveSession> ();

        }


                /// <summary>
        /// Get all Retrospective objects for a given team
        /// </summary>
        /// <param name="teamObjectId"></param>
        /// <returns></returns>
        public RetrospectiveSession Get (ObjectId retroId) {
            var sessions = database.MongoDatabase.GetCollection<RetrospectiveSession> (collection);
            var query = from session in sessions.AsQueryable<RetrospectiveSession> ()
            where session.Id == retroId
            select session;

            return query.FirstOrDefault();

        }
    }

}