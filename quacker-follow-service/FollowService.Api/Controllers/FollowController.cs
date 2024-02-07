using FollowService.Api.Services;
using FollowService.Api.Data;
using FollowService.Api.Models;
using FollowService.Api.Models.Messages;
using FollowService.Api.Models.Payloads.Requests;
using FollowService.Api.Models.Payloads.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FollowService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly ILogger<FollowController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IServiceBus _serviceBus;

        public FollowController(ILogger<FollowController> logger, ApplicationDbContext context, IServiceBus serviceBus)
        {
            _logger = logger;
            _context = context;
            _serviceBus = serviceBus;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Follow(FollowRequest req)
        {
            ClaimsPrincipal principal = User;

            var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value).SingleOrDefault());

            var user = await _context.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.UserId == userId);
            var userToFollow = await _context.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.UserId == req.UserId);

            if (user == null || userToFollow == null)
            {
                return NotFound();
            }

            if (user.Following.Contains(userToFollow))
            {
                return BadRequest("You are already following this user");
            }

            user.Following.Add(userToFollow);
            userToFollow.Followers.Add(user);

            await _context.SaveChangesAsync();

            var message = new NewFollowingMessage()
            {
                Follow = true,
                UserId = userId,
                UserToFollowId = userToFollow.UserId
            };

            await _serviceBus.SendMessageAsync(message, topicName: "follow");

            return Ok();
        }

        [HttpPost("unfollow")]
        [Authorize]
        public async Task<ActionResult> Unfollow(FollowRequest req)
        {
            ClaimsPrincipal principal = User;

            var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value).SingleOrDefault());

            var user = await _context.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.UserId == userId);
            var userToUnfollow = await _context.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.UserId == req.UserId);

            if (user == null || userToUnfollow == null)
            {
                return NotFound();
            }

            if (!user.Following.Contains(userToUnfollow))
            {
                return BadRequest("You are not following this user");
            }

            user.Following.Remove(userToUnfollow);
            userToUnfollow.Followers.Remove(user);

            await _context.SaveChangesAsync();

            var message = new NewFollowingMessage()
            {
                Follow = false,
                UserId = userId,
                UserToFollowId = userToUnfollow.UserId
            };

            await _serviceBus.SendMessageAsync(message, topicName: "follow");

            return Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<User>>> GetUnfollowedUsers()
        {
            ClaimsPrincipal principal = User;

            var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value).SingleOrDefault());

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(await _context.Users.Where(x => !x.Followers.Contains(user) && x.UserId != userId).Take(5).ToListAsync());
        }

        [HttpGet("FollowCount/{username}")]
        [Authorize]
        public async Task<ActionResult<List<User>>> GetFollowCount(string username)
        {
            var user = await _context.Users.Include(u => u.Following).Include(u => u.Followers).FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            var response = new FollowCountResponse
            {
                FollowersCount = user.Followers.Count,
                FollowingCount = user.Following.Count
            };

            return Ok(response);
        }

        [HttpGet("IsFollowing/{username}")]
        [Authorize]
        public async Task<ActionResult<bool>> CheckIfFollows(string username)
        {
            ClaimsPrincipal principal = User;

            var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value).SingleOrDefault());

            var user = await _context.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Following.Any(f => f.Username == username))
            {
                return Ok(true);
            }
            return Ok(false);
        }

        [HttpGet("/followers/{username}")]
        [Authorize]
        public async Task<ActionResult<List<User>>> GetFollowers(string username)
        {
            var user = await _context.Users.Include(u => u.Followers).FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Followers.Select(f => new
            {
                f.UserId,
                f.Username,
            }).ToList());
        }

        [HttpGet("/following/{username}")]
        [Authorize]
        public async Task<ActionResult<List<User>>> GetFollowing(string username)
        {
            var user = await _context.Users.Include(u => u.Following).FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user.Following.Select(f => new
            {
                f.UserId,
                f.Username,
            }).ToList());
        }
    }   
}
