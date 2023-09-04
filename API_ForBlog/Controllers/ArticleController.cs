using API_ForBlog.Models;
using API_ForBlog.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_ForBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _art;

        public ArticleController(IArticleService art)
        {
            _art = art;
        }

        [HttpGet]
        public async Task<IActionResult> GettAllArticle()
        {
            try
            {
                var response = await _art.GetAllArticle();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);


            }
        }

        [HttpGet("GetArticleByArticleID/{id}")]
        public async Task<IActionResult> GetArticleByArticleID(int id)
        {
            try
            {
                var response = await _art.GetArticleByArticleID(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }
        }

        [HttpPost("AddArticle")]
        public async Task<IActionResult> AddArticle([FromBody] ArticleDTO artDTO)
        {
            try
            {
                var response = await _art.AddArticle(artDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);

            }
        }


        [HttpPost("UpdateArticle")]
        public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticleDTO artDTO)
        {
            try
            {             
                var response = await _art.UpdateArticle(artDTO);
                return Ok(response);
                
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }

        }

        [HttpPost("DeleteArticle")]
        public async Task<IActionResult> DeleteCategory([FromBody] DeleteArticleDTO artDTO)
        {
            try
            {
                var response = await _art.DeleteArticle(artDTO);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }

        }

        [HttpGet("GetArticleByCategoryID/{id}")]
        public async Task<IActionResult> GetArticleByCategoryID(int id)
        {
            try
            {
                var response = await _art.GetArticleByCategoryID(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }
        }

        [HttpGet("GetArticleByUserID/{id}")]
        public async Task<IActionResult> GetArticleByUserID(int id)
        {
            try
            {
                var response = await _art.GetArticleByUserID(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }
        }

    }
}