namespace CaptianSocial.Models.Entities
{
    public class Comment
    {
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public required string Message {  get; set; }
    }
}
