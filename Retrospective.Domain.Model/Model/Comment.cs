using System;
using System.Collections.Generic;

namespace Retrospective.Domain.Model
{
    public class Comment
    {
        public string CommentId {get;set;}

        public string MeetingId {get; set;}
        public int CategoryNumber { get; set; }
        public string Text { get; set; }
        public string LastUpdateUserId { get; set; }
        public System.DateTime? LastUpdateDate {get;set;}
        public List<string> VotedUp {get;set;}

        

    }
}