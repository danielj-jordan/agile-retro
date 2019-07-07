using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Retrospective.Data.Model;

namespace Retrospective.Data
{


    public class DataTeam: IDataTeam
    {
        private string collection="team";
        private IDatabase database;


        public DataTeam(IDatabase database)
        {
            this.database=database;
            
        }



        /// <summary>
        /// Save a single team object
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public Team Save (Team team)
        {
            if(team.Id is null) {
                database.MongoDatabase.GetCollection<Team>(collection).InsertOne(team);
            }
            else{
                var filter=MongoDB.Driver.Builders<Team>.Filter.Eq("Id", team.Id);
                var saved=database.MongoDatabase.GetCollection<Team>(collection).ReplaceOne(filter,team);
            }
            
            return team;


        }


        /// <summary>
        /// Get the teams that have a given user as member
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public Team Get (string teamId)
        {
            var teams = database.MongoDatabase.GetCollection<Team>(collection);

            var query = from team in teams.AsQueryable<Team>()
            where team.Id==new ObjectId(teamId)
                select team;

            return query.FirstOrDefault();

        }
        /// <summary>
        /// Get the teams that have a given user as member
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<Team> GetUserTeams (ObjectId userId)
        {
            var teams = database.MongoDatabase.GetCollection<Team>(collection);
            var query = teams.AsQueryable<Team>()
                .Where(t => t.Members.Any(m=>m.UserId==userId ))
                .Select(t => t);          
            return query.ToList<Team>();
        }

        public List<Team> GetUserTeams (string userId)
        {
            return this.GetUserTeams(new ObjectId(userId));
        }


        /// <summary>
        /// Get the teams owned by a given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Team> GetOwnedTeams(ObjectId userId)
        {
            var teams = database.MongoDatabase.GetCollection<Team>(collection);
            var query = teams.AsQueryable<Team>()
                .Where(t => t.Members.Any(m=>m.UserId==userId && m.Role==TeamRole.Owner))
                .Select(t => t);            
            return query.ToList<Team>();
        }
    }
}
