using System.ComponentModel.DataAnnotations;

namespace API_ForBlog.Models
{
    public class DeleteArticleDTO
    {
        [Key]
        public int Id { get; set; }
    }
}
