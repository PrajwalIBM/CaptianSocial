namespace CaptianSocial.Models.Entities
{
    public class Reaction
    {
        public Guid PostID { get; set; }
        public Guid UserID { get; set; }
        public int ReactionId { get; set; }
        public int ReactionType { get; set;}
    }
}
