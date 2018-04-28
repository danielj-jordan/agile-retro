using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Retrospective.Data
{


    public class UserData: Database
    {
        private string collection="user";



    

        /// <summary>
        /// Save a single user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User SaveUser (User user)
        {
            return new User();

        }

        /// <summary>
        /// Returns a single user by the email address
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public User GetUser(string email)
        {
            return new User();


        }

        /// <summary>
        /// Retrieve all the users on a team
        /// </summary>
        /// <param name="teamObjectId"></param>
        /// <returns></returns>
        public List<User> GetTeamUsers (ObjectId teamObjectId)
        {
            return new List<User>();

        }

    }
}