using System;

namespace Retrospective.Domain.Model
{
    public class TeamMember
    {
        public string UserId {get;set;}
        public DateTime? RemoveDate {get;set;}
        public DateTime? StartDate {get;set;}

        /// <summary>
        /// Role of the user on the team:
        /// member, stakeholder, owner
        /// </summary>
        /// <value></value>
        public TeamRole Role {get;set;}
        
    }
}