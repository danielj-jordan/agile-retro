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
        private DataComment comments;
        private DataUser users;
        private DataMeeting sessions;
        private DataTeam teams;
        

        public Database()
        {
            this.database="e2e_test";
            Map();
            Open();
        }

        public Database(string databaseName){
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

            var client = new MongoClient("mongodb://localhost:27017");
            MongoDatabase= client.GetDatabase(database);
        }

        public IMongoDatabase MongoDatabase{get; private set;}


        public DataComment Comments { 
            get{
                if(this.comments==null){
                    this.comments= new DataComment(this);
                }
                return this.comments;
            }
        }

        public DataUser Users{
            get{
                if(this.users==null){
                    this.users= new DataUser(this);
                }
                return this.users;
            }

        }

        public DataMeeting Meetings{
            get{
                if(this.sessions==null){
                    this.sessions=new DataMeeting(this);
                }
                return this.sessions;
            }
        }

        public DataTeam Teams{
            get{
                if(this.teams==null){
                    this.teams=new DataTeam(this);
                }
                return this.teams;
            }
        }

    }



}
         