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


    public class TeamData
    {
        private string collection="team";
        private IDatabase database;


        public TeamData(IDatabase database)
        {
            this.database=database;
            
        }



        /// <summary>
        /// Save a single team object
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public Team SaveTeam (Team team)
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




    }
}
