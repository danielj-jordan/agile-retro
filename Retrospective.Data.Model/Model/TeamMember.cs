using System;

namespace Retrospective.Data.Model
{
    public class TeamMember
    {

        public string UserName {get;set;}
        public DateTime InviteDate {get;set;}
        public DateTime? RemoveDate {get;set;}
        public DateTime? StartDate {get;set;}

        /// <summary>
        /// Role of the user on the team:
        /// member, stakeholder, owner
        /// </summary>
        /// <value></value>
        public string Role {get;set;}
        
    }
}