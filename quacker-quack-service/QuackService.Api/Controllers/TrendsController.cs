using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuackService.Api.Data;
using QuackService.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuackService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TrendsController : ControllerBase
    {
        private readonly ILogger<QuacksController> _logger;
        private readonly ApplicationDbContext _context;

        public TrendsController(ILogger<QuacksController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Topic>>> GetTrends()
        {
            // Get the top 4 topics from the past week
            var topicsLastWeek = await _context.Topics.Where(x => x.CreatedOn > DateTime.Now.AddDays(-7)).ToListAsync();

            return Ok(topicsLastWeek.GroupBy(x => x.Value).OrderByDescending(gp => gp.Count()).Take(6).Select(g => g.Key).ToList());
        }
    }
}
