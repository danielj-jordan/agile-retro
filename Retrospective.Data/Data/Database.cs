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
        private DataSession sessions;
        private DataTeam teams;
        

        public Database(string databaseName)
        {
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
            BsonClassMap.RegisterClassMap<RetrospectiveSession>(cm =>{
                cm.AutoMap();
                cm.MapIdMember(c=>c.Id).SetIdGenerator(ObjectIdGenerator.Instance);
            }); 



            this.database=databaseName;
            var client = new MongoClient("mongodb://localhost:27017");
            MongoDatabase= client.GetDatabase(database);
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

        public DataSession Sessions{
            get{
                if(this.sessions==null){
                    this.sessions=new DataSession(this);
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
         