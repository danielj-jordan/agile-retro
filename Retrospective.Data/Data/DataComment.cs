using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Retrospective.Data
{


    public class DataComment: IDataComment
    {


        private string collection="comment";
        private IDatabase database;


        public DataComment(IDatabase database)
        {
            this.database=database;

        }


        /// <summary>
        /// save a single comment to the database
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public Comment SaveComment(Comment comment)
        {

            if(comment.Id is null) {
                //if comment does not already have an Id then insert
                database.MongoDatabase.GetCollection<Comment>(collection).InsertOne(comment);
                
            }
            else {
                //otherwise update existing
                var filter = MongoDB.Driver.Builders<Comment>.Filter.Eq("Id", comment.Id);
                var saved = database.MongoDatabase.GetCollection<Comment>(collection).ReplaceOne(filter, comment);
            }

            return comment;
        }

        /// <summary>
        /// get all the comments for retrospective
        /// </summary>
        public List<Comment> GetComments(ObjectId retrospectiveObjectId)
        {
                var filter = MongoDB.Driver.Builders<Comment>.Filter.Eq("RetrospectiveId", retrospectiveObjectId);
                var found= database.MongoDatabase.GetCollection<Comment>(collection).Find(filter).ToList<Comment>();
                return found;


        }

        public List<Comment> GetComments(string retrospectiveId){
            return this.GetComments(new ObjectId(retrospectiveId));
        }

        /// <summary>
        /// retreive a single comment
        /// </summary>
        public Comment GetComment(ObjectId commentId)
        {
                var filter = MongoDB.Driver.Builders<Comment>.Filter.Eq("Id", commentId);
                var found= database.MongoDatabase.GetCollection<Comment>(collection).Find(filter).FirstOrDefault();
                return found;


        }

        public void Delete (ObjectId commentId){
                var filter = MongoDB.Driver.Builders<Comment>.Filter.Eq("Id", commentId);
                var found= database.MongoDatabase.GetCollection<Comment>(collection).DeleteOne(filter);
                return ;


        }

    }

}


