using System.ComponentModel.DataAnnotations;

namespace API_ForBlog.Data
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
