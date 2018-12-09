namespace Retrospective.Domain.Model
{
    public class Team
    {
        public string TeamId {get; set;}

        public string Name {get;set;}

        public string Owner {get;set;}

        public TeamMember[] Members {get;set;}
    }
}
