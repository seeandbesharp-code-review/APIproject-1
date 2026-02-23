using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;
using Repository;

namespace Services
{
    public class RatingService: IRatingService
    {
        private readonly IRatingRepository _ratingRepository;


        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public Task<Rating> AddRating(Rating rating)
        {
            return _ratingRepository.AddRating(rating);
        }
    }
}
