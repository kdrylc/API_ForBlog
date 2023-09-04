using API_ForBlog.Data;
using System.ComponentModel.DataAnnotations;

namespace API_ForBlog.Models
{
    public class CategoryDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
