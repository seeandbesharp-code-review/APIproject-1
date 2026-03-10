using Entities;
using Repositories;
using DTOs;

namespace Servers
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetCategories();
    }
}