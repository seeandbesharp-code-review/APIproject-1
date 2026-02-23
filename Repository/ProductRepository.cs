using System.Text.Json;
using Entitys;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ProductRepository : IProductRepository
    {
        dbSHOPContext _dbSHOPContext;

        public ProductRepository(dbSHOPContext dbSHOPContext)
        {
            _dbSHOPContext = dbSHOPContext;
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _dbSHOPContext.Products.FindAsync(id);
        }

        public async Task<(List<Product> Items, int TotalCount)> GetProducts(string? description, int? minPrice, int? maxPrice, int[]? categoryIds,
            int? limit, string? orderby, int? position)
        {
            var query = _dbSHOPContext.Products.Where(product =>
            (description == null ? (true) : (product.Description.Contains(description)))
            && ((minPrice == null) ? (true) : (product.Price >= minPrice))
            && ((maxPrice == null) ? (true) : (product.Price <= maxPrice))
            && ((categoryIds == null) ? (true) : (categoryIds.Contains(product.CategoryId))))
            .OrderBy(product=>product.Price);

            Console.WriteLine(query.ToQueryString());
            int pos=position ?? 1;
            int skip=limit ?? 20;
            List<Product> products = await query.Skip((pos - 1) * skip)
            .Take(skip).Include(product => product.Category).ToListAsync();

            var total = await query.CountAsync();
            return (products, total);
        }

    }
}
