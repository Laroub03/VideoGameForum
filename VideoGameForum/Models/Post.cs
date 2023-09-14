using System.ComponentModel.DataAnnotations;

namespace VideoGameForum.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
