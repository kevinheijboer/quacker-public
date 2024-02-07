using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TimelineService.Api.Data;
using TimelineService.Api.Models;

namespace TimelineService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TimelineController : ControllerBase
    {
        private readonly ILogger<TimelineController> _logger;
        private readonly ApplicationDbContext _context;

        public TimelineController(ILogger<TimelineController> logger, ApplicationDbContext context)
        {
            _logger = logger;

            _logger.LogDebug("Timeline constructor");

            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Quack>>> GetTimeline()
        {
            _logger.LogInformation("Requested timeline");

            ClaimsPrincipal principal = User;

            var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value).SingleOrDefault());

            var user = await _context.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.UserId == userId);

            _logger.LogInformation($"Retrieved user {user.Username} for authorisation");

            var timeline = new List<Quack>();

            // Add own quacks
            timeline.AddRange(await _context.Quacks.Where(q => q.UserId == userId).ToListAsync());

            _logger.LogInformation($"Added own quacks");

            // Add quacks from following
            foreach (var followedUser in user.Following)
            {
                timeline.AddRange(await _context.Quacks.Where(q => q.UserId == followedUser.UserId).ToListAsync());
            }

            _logger.LogInformation($"Added followers quacks to timeline");

            //// Add quacks that mention
            var mentions = await _context.Mentions.Where(m => m.Value == "@" + user.Username).ToListAsync();

            foreach (var mention in mentions)
            {
                if (!timeline.Any(q => q.Id == mention.QuackId))
                {
                    timeline.Add(await _context.Quacks.FirstOrDefaultAsync(q => q.Id == mention.QuackId));
                }
            }

            _logger.LogInformation($"Added mentioned quacks to timeline");

            return Ok(timeline);
        }
    }
}
