using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Retrospective.Data
{
    public interface IDatabase
    {
         IMongoDatabase MongoDatabase{get;}

         IDataComment Comments{ get;}

         IDataUser Users {get;}

    }
}
    