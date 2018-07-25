namespace app.Model
{
    public class Comment
    {
        public string CommentId {get;set;}
        public int CategoryId { get; set; }
        public string Text { get; set; }
        public string UpdateUser { get; set; }

    }
}