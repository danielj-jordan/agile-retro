using System;
using MongoDB.Driver;
using Retrospective.Data;

namespace demodata
{
  class Program
  {
    static int Main(string[] args)
    {
      var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
      var databaseName = Environment.GetEnvironmentVariable("DB_NAME");


      if (String.IsNullOrWhiteSpace(connectionString))
      {
        Console.WriteLine("DB_CONNECTIONSTRING environment variable is not set");
        return -1;
      }

      if (String.IsNullOrWhiteSpace(databaseName))
      {
        Console.WriteLine("DB_NAME environment varialbe is not set");
        return -1;
      }

      //start with an empty database
      var client = new MongoClient(connectionString);

      //connect to the database
      Retrospective.Data.Database database = new Database(databaseName);

      DemoData demoData = new DemoData(database);
      demoData.Initialize();
      return 0;
    }
  }
}
