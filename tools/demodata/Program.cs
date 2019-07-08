using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Retrospective.Data;

namespace demodata
{
  class Program
  {
    static int Main(string[] args)
    {


      var serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);
      var serviceProvider = serviceCollection.BuildServiceProvider();

      var logger = serviceProvider.GetService<ILogger<Program>>();

      try
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

        DemoData demoData = new DemoData(database, serviceProvider.GetService<ILogger<DemoData>>());
        demoData.Initialize();
        return 0;
      }
      catch (Exception ex)
      {
        Console.WriteLine("An error occured.");
        logger.LogCritical(ex, "An error occured" );
        return -2;
      }
    }

    private static void ConfigureServices(IServiceCollection services)
    {
      services.AddLogging(configure => configure.AddConsole())
              .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
    }
  }
}
