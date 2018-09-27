using System;
using System.Linq;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Driver.Linq;

namespace Retrospective.Data
{


    public class DataTeam
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
        public List<Team> GetUserTeams (string email)
        {
            var teams = database.MongoDatabase.GetCollection<Team>(collection);

            var query = from team in teams.AsQueryable<Team>()
            where team.TeamMembers.Contains(email)
                select team;

            return query.ToList<Team>();

        }


        /// <summary>
        /// Get the teams owned by a given user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<Team> GetOwnedTeams(string email)
        {
            var teams = database.MongoDatabase.GetCollection<Team>(collection);
            var query = from team in teams.AsQueryable<Team>()
            where team.Owner==email
                select team;

            return query.ToList<Team>();

        }




    }
}
