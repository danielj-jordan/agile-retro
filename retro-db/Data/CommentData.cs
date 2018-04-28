using System;
using MongoDB.Driver;
using Retrospective.Data.Model;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Retrospective.Data
{


    public class CommentData: Database
    {
        private string collection="comment";

        /// <summary>
        /// save a single comment to the database
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public Comment SaveComment(Comment comment)
        {

            if(comment.Id is null) {
                //if comment does not already have an Id then insert
                System.Console.WriteLine("** inserting **");
                this.mongoDatabase.GetCollection<Comment>(collection).InsertOne(comment);
                
            }
            else {
                //otherwise update existing
                var filter = MongoDB.Driver.Builders<Comment>.Filter.Eq("Id", comment.Id);
                var saved = this.mongoDatabase.GetCollection<Comment>(collection).ReplaceOne(filter, comment);
                System.Console.WriteLine(saved);
            }

            return comment;
        }

        /// <summary>
        /// get all the comments for retrospective
        /// </summary>
        public List<Comment> GetComments(ObjectId retrospectiveObjectId)
        {
                return new List<Comment>();


        }

    }

}


