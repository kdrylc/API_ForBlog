using API_ForBlog.Context;
using API_ForBlog.Data;
using API_ForBlog.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API_ForBlog.Service
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _db;

        public ArticleService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<MainResponse> AddArticle(ArticleDTO artDTO)
        {
            var response = new MainResponse();
            try
            {
                if (_db.Articles.Any(f=>f.Title.ToLower() == artDTO.Title.ToLower()))
                {
                    response.ErrorMessage = "Article is already exist with this category";
                    response.IsSuccess = false;
                }
                else
                {
                    await _db.AddAsync(new Article
                    {
                       CategoryId = artDTO.CategoryId,
                       Title = artDTO.Title,
                       Summary = artDTO.Summary,
                       Detail = artDTO.Detail,
                       UserId = artDTO.UserId,
                       Created = DateTime.Now
                    });
                    await _db.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.Content = "Article Added";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> DeleteArticle(DeleteArticleDTO artDTO)
        {
            var response = new MainResponse();
            try
            {
                if (artDTO.Id < 0)
                {
                    response.ErrorMessage = "Please pass artcile Id";
                    return response;
                }
                var existCategory = _db.Articles.Where(f => f.Id == artDTO.Id).FirstOrDefault();
                if (existCategory != null)
                {
                    _db.Remove(existCategory);
                    await _db.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = "Article Info Deleted";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Content = "Article not found with specify category Id";
                }
            }
            catch (Exception ex)
            {

                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> GetAllArticle()
        {
            var response = new MainResponse();
            try
            {
                response.Content = await _db.Articles.ToListAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> GetArticleByArticleID(int id)
        {
            var response = new MainResponse();
            try
            {
                response.Content =
                    await _db.Articles.Where(f => f.Id == id).FirstOrDefaultAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> GetArticleByCategoryID(int id)
        {
            var response = new MainResponse();
            var respose2 = new CategoryDTO();
            try
            {
                response.Content = await _db.Articles.Where(f => f.CategoryId == id).FirstOrDefaultAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;

        }

        public async Task<MainResponse> GetArticleByUserID(int id)
        {
            var response = new MainResponse();
            try
            {
                response.Content = await _db.Articles.Where(f=>f.UserId==id).FirstOrDefaultAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)

            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;

        }

        public async Task<MainResponse> UpdateArticle(UpdateArticleDTO artDTO)
        {
            var response = new MainResponse();
            try
            {
                if (artDTO.Id < 0)
                {
                    response.ErrorMessage = "Please pass article ID";
                    return response;
                }

                var existingArt = _db.Articles.Where(f => f.Id == artDTO.Id).FirstOrDefault();

                if (existingArt != null)
                {
                    existingArt.CategoryId = artDTO.CategoryId;
                    existingArt.Title = artDTO.Title;
                    existingArt.Summary = artDTO.Summary;
                    existingArt.Detail = artDTO.Detail;
                    existingArt.Created = DateTime.Now;
                    await _db.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = "Record Updated";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Content = "Article not found with specify artcile ID";
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }
    }
}
