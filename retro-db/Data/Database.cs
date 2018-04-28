using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Retrospective.Data
{


    public class Database
    {
        private string database = "retro";
        protected IMongoDatabase mongoDatabase;

        public Database()
        {
            BsonClassMap.RegisterClassMap<Comment>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c=>c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
            });
        
            var client = new MongoClient("mongodb://localhost:27017");
            this.mongoDatabase= client.GetDatabase(database);
        }



   





    }



}
         