namespace CaptianSocial.Models
{
    public class AddReactionViewModel
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public int ReactionId { get; set; }
        public int ReactionType { get; set;}
    }
}
