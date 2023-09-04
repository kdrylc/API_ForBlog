using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API_ForBlog.Data
{
    public class User:IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; } = null!;

        [MaxLength(50)]
        public string LastName { get; set; } = null!;


        [MaxLength(6)]
        public string Gender { get; set; } = null!;
        public string? Address { get; set; }
        public bool IsAdmin { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserAvatar { get; set; }
        public virtual ICollection<Article> Articles { get; set; }

    }
}
