using API_ForBlog.Context;
using API_ForBlog.Data;
using API_ForBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace API_ForBlog.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<MainResponse> AddCategory(CategoryDTO catDTO)
        {
            var response = new MainResponse();
            try
            {
                if (_db.Categories.Any(f=>f.Name.ToLower()== catDTO.Name.ToLower()))
                {
                    response.ErrorMessage = "Category is already exist with this category";
                    response.IsSuccess = false;
                }
                else
                {
                    await _db.AddAsync(new Category
                    {
                        Name = catDTO.Name,
                        Description = catDTO.Description,
                        Created = DateTime.Now.Date,
                    });
                    await _db.SaveChangesAsync();
                    response.IsSuccess = true;
                    response.Content = "Category  Added";
                }

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> DeleteCategory(DeleteCategoryDTO delDTO)
        {
            var response = new MainResponse();
            try
            {
                if (delDTO.Id < 0)
                {
                    response.ErrorMessage = "Please pass category Id";
                    return response;
                }
                var existCategory = _db.Categories.Where(f=>f.Id == delDTO.Id).FirstOrDefault();
                if (existCategory !=null)
                {
                    _db.Remove(existCategory);
                    await _db.SaveChangesAsync();

                    response.IsSuccess= true;
                    response.Content = "Category Info Deleted";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Content = "Category not found with specify category Id";
                }
            }
            catch (Exception ex)
            {

                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> GetAllCategory()
        {
            var response = new MainResponse();
            try
            {
                response.Content = await _db.Categories.ToListAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> GetCategoryByCategoryID(int id)
        {
            var response = new MainResponse();
            try
            {
                response.Content =
                    await _db.Categories.Where(f => f.Id == id).FirstOrDefaultAsync();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                response.IsSuccess = false;
            }
            return response;
        }

        public async Task<MainResponse> UpdateCategory(UpdateCategoryDTO updtDTO)
        {
            var response = new MainResponse();
            try
            {
                if (updtDTO.Id < 0)
                {
                    response.ErrorMessage = "Please pass student ID";
                    return response;
                }

                var existingCat = _db.Categories.Where(f => f.Id == updtDTO.Id).FirstOrDefault();

                if (existingCat != null)
                {
                    existingCat.Name = updtDTO.Name;
                    existingCat.Description = updtDTO.Description;
                    existingCat.Created = DateTime.Now;
                    await _db.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Content = "Record Updated";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Content = "Student not found with specify student ID";
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
