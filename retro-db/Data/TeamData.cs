using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Retrospective.Data
{


    public class TeamData
    {
        private string collection="team";



        /// <summary>
        /// Save a single team object
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public Team SaveTeam (Team team)
        {
            return new Team();


        }

        /// <summary>
        /// Get the teams for a specific user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public List<Team> GetUserTeams (string email)
        {

            return new List<Team>();

        }




    }
}