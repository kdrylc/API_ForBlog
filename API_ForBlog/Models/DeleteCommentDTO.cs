using System.ComponentModel.DataAnnotations;

namespace API_ForBlog.Models
{
    public class DeleteCommentDTO
    {
        [Key]
        public int Id { get; set; }
    }
}
