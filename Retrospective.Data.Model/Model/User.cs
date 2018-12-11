using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class User
    {
        public ObjectId? Id {get;set;}
        public string Name {get; set;}
        public string Email {get;set;}

        //list of the teams that this operson is on
        public ObjectId[] Teams {get;set;}

        public bool IsDemoUser {get;set;}
        public DateTime? SubscriptionEnd {get; set;}

    }
}
