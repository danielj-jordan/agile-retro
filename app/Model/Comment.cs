namespace app.Model
{
    public class Comment
    {
        public string CommentId {get;set;}

        public string SessionId {get; set;}
        public int CategoryNum { get; set; }
        public string Text { get; set; }
        public string UpdateUser { get; set; }

    }
}