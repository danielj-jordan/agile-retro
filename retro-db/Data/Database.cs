using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Retrospective.Data
{


    public class Database: IDatabase
    {
        private string database;
        

        public Database(string databaseName)
        {
            BsonClassMap.RegisterClassMap<Comment>(cm =>
            {
                cm.AutoMap();
                cm.MapIdMember(c=>c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
            });
        
            this.database=databaseName;
            var client = new MongoClient("mongodb://localhost:27017");
            MongoDatabase= client.GetDatabase(database);
        }

        public IMongoDatabase MongoDatabase{get; private set;}
      


   





    }



}
         