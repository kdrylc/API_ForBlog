using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_ForBlog.Data
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public string fullName { get; set; }
        public string Title  { get; set; }
        public string Detail  { get; set; }
        public DateTime Created { get; set; }
        [ForeignKey("ArticleId")]
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
