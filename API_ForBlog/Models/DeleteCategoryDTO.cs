using System.ComponentModel.DataAnnotations;

namespace API_ForBlog.Models
{
    public class DeleteCategoryDTO
    {
        [Key]
        public int Id { get; set; }
    }
}
