using System.ComponentModel.DataAnnotations;

namespace VideoGameForum.ViewModels
{
    public class PostViewModel
    {
        [Required]
        public string Title { get; set; }
    }
}
