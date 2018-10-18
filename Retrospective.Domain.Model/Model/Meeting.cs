namespace Retrospective.Domain.Model
{
    public class Meeting
    {
        public string Id {get;set;}

        public string TeamId {get;set;}
        
        public string Name {get;set;}

        public  Category[]  Categories {get;set;}
    }
}