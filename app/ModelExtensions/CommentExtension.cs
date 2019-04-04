namespace app.ModelExtensions
{
    public static class CommentExtension
    {
        public static app.Model.Comment ToViewModelComment(
             this Retrospective.Domain.Model.Comment comment,
             string activeUser)
        {
            var viewModelComment = new app.Model.Comment
            {
                CommentId = comment.CommentId,
                SessionId = comment.MeetingId,
                CategoryNum = comment.CategoryNumber,
                Text =  comment.Text,
                UpdateUser = comment.LastUpdateUser,
                VoteCount =  comment.VotedUp.Count,
                ThisUserVoted = comment.VotedUp.Contains(activeUser)
            };
            return viewModelComment;
        }

       
    }

}