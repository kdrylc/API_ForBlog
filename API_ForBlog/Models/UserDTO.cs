using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API_ForBlog.Models
{
    public class UserDTO: IdentityUser
    {
        [Key]
        public int Id { get; set; }
    }
}
