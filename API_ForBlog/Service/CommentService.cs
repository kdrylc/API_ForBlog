using API_ForBlog.Context;
using API_ForBlog.Data;
using API_ForBlog.Models;
using Azure;
using Microsoft.EntityFrameworkCore;

namespace API_ForBlog.Service
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _db;

        public CommentService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<MainResponse> AddComment(CommentDTO cmDTO)
        {
          var  response = new MainResponse();
            await _db.AddAsync(new Comment
            {
                ArticleId = cmDTO.ArticleId,
                fullName = cmDTO.fullName,
                Title = cmDTO.Title,
                Detail = cmDTO.Detail,
                Created = DateTime.Now
               
            });
            await _db.SaveChangesAsync();
            response.IsSuccess = true;
            response.Content = "Article Added";
            return response;
        }

        public async Task<MainResponse> DeleteComment(DeleteCommentDTO cmDTO)
        {

            var response = new MainResponse();
            try
            {
                if (cmDTO.Id < 0)
                {
                    response.ErrorMessage = "Please pass artcile Id";
                    return response;
                }
                var existCategory = _db.Comments.Where(f => f.Id == cmDTO.Id).FirstOrDefault();
                if (existCategory != null)
                {
                    _db.Remove(existCategory);
                    await _db.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = "Comment Info Deleted";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Content = "Comment not found with specify category Id";
                }
            }
            catch (Exception ex)
            {

                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> GetAllComment()
        {
            var response = new MainResponse();
            try
            {
                response.Content = await _db.Comments.ToListAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> GetCommentByCommentId(int id)
        {
            var response = new MainResponse();
            try
            {
                response.Content =
                    await _db.Comments.Where(f => f.Id == id).FirstOrDefaultAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> UpdateComment(UpdateCommentDTO cmDTO)
        {
            var response = new MainResponse();
            try
            {
                if (cmDTO.Id < 0)
                {
                    response.ErrorMessage = "Please pass article ID";
                    return response;
                }

                var existingArt = _db.Comments.Where(f => f.Id == cmDTO.Id).FirstOrDefault();

                if (existingArt != null)
                {
                    existingArt.ArticleId = cmDTO.Id;
                    existingArt.fullName = cmDTO.fullName;
                    existingArt.Title = cmDTO.Title;
                    existingArt.Detail=cmDTO.Detail;
                    existingArt.Created = DateTime.Now;
                    await _db.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = "Record Updated";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Content = "Comment not found with specify artcile ID";
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
