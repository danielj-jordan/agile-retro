using System;

using DomainModel= Retrospective.Domain.Model;

namespace Retrospective.Domain
{
    public class AccessRules
    {
         public static bool IsTeamMember(string activeUser, DomainModel.Team team){
            //confirm that this user is a member of the team
            if(team.Owner.Equals(activeUser, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            var found = Array.Exists(team.TeamMembers, 
                member => member.Equals(activeUser, StringComparison.OrdinalIgnoreCase));

            if(found) return true;

            return false;

        }

        public static bool IsTeamOwner(string activeUser, DomainModel.Team team){
            //confirm that this user is a member of the team
            if(team.Owner.Equals(activeUser, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;

        }
    }
}