using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class Meeting
    {
       
        public ObjectId? Id {get;set;}

        public ObjectId TeamId {get;set;}

        public string Name {get; set;}

        public Category[] Categories {get; set;}

    
    }
}
