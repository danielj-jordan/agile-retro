using System;

namespace Retrospective.Domain.Model
{
    public class Invitation
    {

        public string Name {get;set;}

        public string Email {get;set;}

        public DateTime InviteDate {get;set;}
        
        public TeamRole Role {get;set;}
        
    }
}