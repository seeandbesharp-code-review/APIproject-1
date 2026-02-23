using Entitys;

namespace Repository
{
    public interface IProductRepository
    {
        Task<Product> GetProductById(int id);
        Task<(List<Product> Items, int TotalCount)> GetProducts(string? description, int? minPrice, int? maxPrice, int[]? categoryIds,
            int? limit, string? orderby, int? position);
    }
}