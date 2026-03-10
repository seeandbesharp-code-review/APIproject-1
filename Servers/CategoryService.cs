using AutoMapper;
namespace Servers;
using Entities;
using Repositories;
using DTOs;


public class CategoryService : ICategoryService
{
    private readonly ICategoriesRepository _categoryRepository;
    IMapper _mapper;

    public CategoryService(ICategoriesRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryDTO>> GetCategories()
    {
        return _mapper.Map<List<Category>, List<CategoryDTO>>(await _categoryRepository.GetCategories());
    }

}
