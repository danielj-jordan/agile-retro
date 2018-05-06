using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Retrospective.Data
{


    public class SessionData
    {
        private string collection="session";
    

        /// <summary>
        /// Save a single retrospective object
        /// </summary>
        /// <param name="retrospective"></param>
        /// <returns></returns>
        public Session SaveRetrospectiveSession(Session retrospective)
        {
            return new Session();

        }

        /// <summary>
        /// Get all Retrospective objects for a given team
        /// </summary>
        /// <param name="teamObjectId"></param>
        /// <returns></returns>
        public List<Session> GetTeamRetrospectiveSessions(ObjectId teamObjectId)
        {
            return new List<Session>();
            

        }
    }

}