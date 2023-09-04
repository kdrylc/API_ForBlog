using API_ForBlog.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_ForBlog.Models
{
    public class CommentDTO
    {
        [Key]
        public int Id { get; set; }
        public string fullName { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("ArticleId")]
        public int ArticleId { get; set; }

    }
}
