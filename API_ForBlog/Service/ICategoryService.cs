using API_ForBlog.Models;

namespace API_ForBlog.Service
{
    public interface ICategoryService
    {
        Task<MainResponse> AddCategory(CategoryDTO catDTO);
        Task<MainResponse> UpdateCategory(UpdateCategoryDTO updtDTO);
        Task<MainResponse> DeleteCategory(DeleteCategoryDTO delDTO);
        Task<MainResponse> GetAllCategory();
        Task<MainResponse> GetCategoryByCategoryID(int id);
    }
}
