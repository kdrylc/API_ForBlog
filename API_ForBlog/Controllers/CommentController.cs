using API_ForBlog.Models;
using API_ForBlog.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ForBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _c;

        public CommentController(ICommentService c)
        {
            _c = c;
        }

        [HttpGet]
        public async Task<IActionResult> GettAlComment()
        {
            try
            {
                var response = await _c.GetAllComment();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);

            }
        }

        [HttpGet("GetCommentByCommentId/{id}")]
        public async Task<IActionResult> GetCommentByCommentId(int id)
        {
            try
            {
                var response = await _c.GetCommentByCommentId(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }
        }

        [HttpPost("AddComment")]
        public async Task<IActionResult> AddCategory([FromBody] CommentDTO cmDTO)
        {
            try
            {
                var response = await _c.AddComment(cmDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);

            }
        }

        [HttpPost("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDTO cmDTO)
        {
            try
            {
                var response = await _c.UpdateComment(cmDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }

        }

        [HttpPost("DeleteComment")]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentDTO cmDTO)
        {
            try
            {
                var response = await _c.DeleteComment(cmDTO);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }

        }


    }
}
