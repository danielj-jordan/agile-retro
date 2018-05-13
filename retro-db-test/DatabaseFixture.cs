using System;
using Xunit;
using Retrospective.Data.Model;
using Retrospective.Data;


namespace retro_db_test
{
    public class DatabaseFixture : IDisposable
    {
        public IDatabase database= new Database("test");

        public void Dispose()
        {
        
        }
    
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection: ICollectionFixture<DatabaseFixture>
    {


    }


}