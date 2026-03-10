using DTOs;
using Entities;
using Repositories;

namespace Servers
{
    public interface IPrudectsService
    {
        Task<PageResponseDTO<ProductDTO>> GetProducts(string? name, int? minPrice, int? maxprice, int[]? categoriesId,
            int? limit, string? orderby, int? offset);

    }
}