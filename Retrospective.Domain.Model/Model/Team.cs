using System;

namespace Retrospective.Domain.Model
{
    public class Team
    {
        public string TeamId {get; set;}

        public string Name {get; set;}
        public TeamMember[] Members {get;set;}

        public Invitation[] Invited {get;set;}
        
    }
}
