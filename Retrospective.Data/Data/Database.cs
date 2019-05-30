using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Retrospective.Data
{


    public class Database: IDatabase
    {
        private string connectionString;
        private string database;
        private IDataComment comments;
        private IDataUser users;
        private IDataMeeting sessions;
        private IDataTeam teams;

        

        public Database()
        {
            this.connectionString =   Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
            this.database=Environment.GetEnvironmentVariable("DB_NAME");
            Map();
            Open();
        }

        public Database(string databaseName){
             this.connectionString =   Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
            this.database=databaseName;
            Map();
            Open();
        }

        public Database(string connectionString, string databaseName){
             this.connectionString =  connectionString;
            this.database=databaseName;
            Map();
            Open();
        }
        

        private void Map()
        {
            if(BsonClassMap.IsClassMapRegistered(typeof(Comment)))return;

            BsonClassMap.RegisterClassMap<Comment>(cm =>{
                cm.AutoMap();
                cm.MapIdMember(c=>c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
            });
            BsonClassMap.RegisterClassMap<User>(cm =>{
                cm.AutoMap();
                cm.MapIdMember(c=>c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
            });      
            BsonClassMap.RegisterClassMap<Team>(cm =>{
                cm.AutoMap();
                cm.MapIdMember(c=>c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
            }); 
            BsonClassMap.RegisterClassMap<Meeting>(cm =>{
                cm.AutoMap();
                cm.MapIdMember(c=>c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
            }); 

            

        }
        private void Open(){
            var client = new MongoClient(this.connectionString);
            MongoDatabase= client.GetDatabase(this.database);
        }

        public IMongoDatabase MongoDatabase{get; private set;}


        public IDataComment Comments { 
            get{
                if(this.comments==null){
                    this.comments= new DataComment(this);
                }
                return this.comments;
            }
        }

        public IDataUser Users{
            get{
                if(this.users==null){
                    this.users= new DataUser(this);
                }
                return this.users;
            }

        }

        public IDataMeeting Meetings{
            get{
                if(this.sessions==null){
                    this.sessions=new DataMeeting(this);
                }
                return this.sessions;
            }
        }

        public IDataTeam Teams{
            get{
                if(this.teams==null){
                    this.teams=new DataTeam(this);
                }
                return this.teams;
            }
        }

    }



}
         