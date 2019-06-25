using System;
using MongoDB.Driver;
using Retrospective.Data;

namespace demodata
{
  class Program
  {
    static void Main(string[] args)
    {
      var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
      var databaseName = Environment.GetEnvironmentVariable("DB_NAME");

      //start with an empty database
      var client = new MongoClient(connectionString);

      //connect to the database
      Retrospective.Data.Database database = new Database(databaseName);

      DemoData demoData = new DemoData(database);
      demoData.Initialize();
    }
  }
}
