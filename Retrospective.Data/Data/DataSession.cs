using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using Retrospective.Data.Model;

namespace Retrospective.Data
{


    public class DataSession
    {
        private string collection="session";

        private IDatabase database;


        public DataSession(IDatabase database)
        {
            this.database=database;
            
        }
    

        /// <summary>
        /// Save a single retrospective object
        /// </summary>
        /// <param name="retrospective"></param>
        /// <returns></returns>
        public RetrospectiveSession SaveRetrospectiveSession(RetrospectiveSession retrospective)
        {

             if(retrospective.Id is null) {
                database.MongoDatabase.GetCollection<RetrospectiveSession>(collection).InsertOne(retrospective);
            }
            else{
                var filter=MongoDB.Driver.Builders<RetrospectiveSession>.Filter.Eq("Id", retrospective.Id);
                var saved=database.MongoDatabase.GetCollection<RetrospectiveSession>(collection).ReplaceOne(filter,retrospective);
            }
            
            return retrospective;
        }

        /// <summary>
        /// Get all Retrospective objects for a given team
        /// </summary>
        /// <param name="teamObjectId"></param>
        /// <returns></returns>
        public List<RetrospectiveSession> GetTeamRetrospectiveSessions(ObjectId teamObjectId)
        {
            var sessions = database.MongoDatabase.GetCollection<RetrospectiveSession>(collection);
            var query =  from session in sessions.AsQueryable<RetrospectiveSession>()
                where session.TeamId==teamObjectId
                select session;

            return query.ToList<RetrospectiveSession>();
            

        }
    }

}
