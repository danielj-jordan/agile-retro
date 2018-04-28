using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class Team
    {
        public ObjectId Id {get;set;}
        public string Name {get; set;}

        //email address for the owner of this team
        public string Owner{get;set;}

        //list of email addresses from the user collection
        public string[] TeamMembers{get;set;}
        

    }
}
