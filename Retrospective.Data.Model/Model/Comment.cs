using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class Comment
    {
       
        public ObjectId? Id {get;set;}

        public string Text {get; set;}
        public ObjectId RetrospectiveId{get;set;}
        public int CategoryNumber {get;set;}

        //email address for the last user to update the record
        public string LastUpdateUser {get;set;}

        public DateTime?  LastUpdateDate {get;set;}

        //email address for the users who voted up the item
        public string[] VotedUp {get;set;}

        

    }
}
