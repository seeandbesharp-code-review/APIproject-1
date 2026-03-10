using System.Text.Json;
using Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        dbSHOPContext _dbSHOPContext;

        public CategoriesRepository(dbSHOPContext dbSHOPContext)
        { 
            _dbSHOPContext= dbSHOPContext;
        }
        public async Task<List<Category>> GetCategories()
        {
            return await _dbSHOPContext.Categories.ToListAsync();
        }

    }
}
