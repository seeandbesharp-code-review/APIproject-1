using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entitys;

namespace Services
{
    public interface IRatingService
    {
        Task<Rating> AddRating(Rating rating);
    }
}
