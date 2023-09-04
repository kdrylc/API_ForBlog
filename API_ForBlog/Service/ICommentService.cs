using API_ForBlog.Models;

namespace API_ForBlog.Service
{
    public interface ICommentService
    {
        Task<MainResponse> AddComment(CommentDTO cmDTO);
        Task<MainResponse> UpdateComment(UpdateCommentDTO cmDTO);
        Task<MainResponse> DeleteComment(DeleteCommentDTO cmDTO);
        Task<MainResponse> GetAllComment();
        Task<MainResponse> GetCommentByCommentId(int id);
    }
}
