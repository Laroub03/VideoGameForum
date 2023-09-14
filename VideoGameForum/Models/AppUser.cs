using Microsoft.AspNetCore.Identity;

namespace VideoGameForum.Models
{
    public class AppUser : IdentityUser
    {
        public bool IsBanned { get; set; }
    }
}
