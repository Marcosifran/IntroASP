using IntroASP.Models;
using IntroASP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using static System.Net.WebRequestMethods;

namespace IntroASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBeer : ControllerBase
    {
        private readonly PostgresContext _context;

        public ApiBeer(PostgresContext context)
        {
            _context = context;
        }
        public async Task<List<BeerBrandViewModel>> Get()
            => await _context.Beers.Include(b=>b.Brand).Select(b=>new BeerBrandViewModel
            {
                Name = b.Name,
                Brand = b.Brand.Name
            })
            .ToListAsync(); 
    }
}
