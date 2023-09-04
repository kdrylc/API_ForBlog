using API_ForBlog.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using API_ForBlog.Models;

namespace API_ForBlog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GettAllCategory()
        {
            try
            {
                var response = await _categoryService.GetAllCategory();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);

            }
        }

        [HttpGet("GetCategoryByCategoryID/{id}")]
        public async Task<IActionResult> GetCategoryByCategoryID(int id)
        {
            try
            {
                var response = await _categoryService.GetCategoryByCategoryID(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }
        }

        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody]  CategoryDTO categoryDTO)
        {
            try
            {
                var response = await _categoryService.AddCategory(categoryDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);

            }
        }

        [HttpPost("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO categoryDTO)
        {
            try
            {
                var response = await _categoryService.UpdateCategory(categoryDTO);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }

        }

        [HttpPost("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory([FromBody] DeleteCategoryDTO categoryDTO)
        {
            try
            {
                var response = await _categoryService.DeleteCategory(categoryDTO);
                return Ok(response);

            }
            catch (Exception ex)
            {
                return ErrorResponse.ReturnErrorResponse(ex.Message);
            }

        }

    }
}
