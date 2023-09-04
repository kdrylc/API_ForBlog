using API_ForBlog.Models;

namespace API_ForBlog.Service
{
    public interface IArticleService
    {
        Task<MainResponse> AddArticle(ArticleDTO artDTO);
        Task<MainResponse> UpdateArticle(UpdateArticleDTO artDTO);
        Task<MainResponse> DeleteArticle(DeleteArticleDTO artDTO);
        Task<MainResponse> GetAllArticle();
        Task<MainResponse> GetArticleByArticleID(int id);
        Task<MainResponse> GetArticleByCategoryID(int id);
        Task<MainResponse> GetArticleByUserID(int id);
    }
}
