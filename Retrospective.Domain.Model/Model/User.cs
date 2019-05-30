namespace Retrospective.Domain.Model
{
    public class User
    {
        public string UserId {get;set;}

        public string Name {get;set;}

        public string Email {get;set;}

        public bool IsDemoUser {get;set;}

        public DateTime? LastLoggedIn {get;set;}

    }
}