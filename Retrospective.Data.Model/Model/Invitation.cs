using System;
using MongoDB.Bson;

namespace Retrospective.Data.Model
{
    public class Invitation
    {

        public string Name {get;set;}

        public string Email {get;set;}

        public DateTime InviteDate {get;set;}
        
        public TeamRole Role {get;set;}
        
    }
}