using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class Team
    {
        public ObjectId? Id {get;set;}
        public string Name {get; set;}
        public TeamMember[] Members {get;set;}

        public Invitation[] Invited {get;set;}
        

    }
}
