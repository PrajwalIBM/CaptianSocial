namespace CaptianSocial.Models
{
    public class AddPostViewModel
    {
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
    }
}
