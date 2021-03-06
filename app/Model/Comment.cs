namespace app.Model
{
  public class Comment
  {
    public string CommentId { get; set; }
    public string MeetingId { get; set; }
    public int CategoryNum { get; set; }
    public string Text { get; set; }
    public string UpdateUserId { get; set; }
    public int VoteCount { get; set; }
    public bool ThisUserVoted { get; set; }
  }
}