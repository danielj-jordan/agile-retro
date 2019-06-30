using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class TeamMember
    {
        public ObjectId UserId {get;set;}
        public DateTime? RemoveDate {get;set;}
        public DateTime? StartDate {get;set;}

        /// <summary>
        /// Role of the user on the team:
        /// member, stakeholder, manager
        /// </summary>
        /// <value></value>  
        public TeamRole Role {get;set;}
        
    }
}