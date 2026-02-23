using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;

namespace Repository
{
    public class RatingRepository: IRatingRepository
    {
        dbSHOPContext _dbSHOPContext;

        public RatingRepository(dbSHOPContext dbSHOPContext)
        {
            _dbSHOPContext = dbSHOPContext;
        }

        public async Task<Rating> AddRating(Rating rating)
        {
            await _dbSHOPContext.AddAsync(rating);
            await _dbSHOPContext.SaveChangesAsync();
            return rating;
        }

    }
}
