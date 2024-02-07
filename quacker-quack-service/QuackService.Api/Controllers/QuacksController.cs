using Microsoft.AspNetCore.Authorization;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuackService.Api.Data;
using QuackService.Api.Models.Payloads.Requests;
using QuackService.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Linq;
using QuackService.Api.Models.Payloads.Messages;
using QuackService.Api.Logic;
using Flurl.Http;
using QuackService.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuackService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuacksController : ControllerBase
    {
        private readonly ILogger<QuacksController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IServiceBus _serviceBus;
        private readonly ITopicFinder topicFinder;

        public QuacksController(ILogger<QuacksController> logger, ApplicationDbContext context, IServiceBus serviceBus, ITopicFinder topicFinder)
        {
            _logger = logger;
            _context = context;
            _serviceBus = serviceBus;
            this.topicFinder = topicFinder;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quack>>> GetQuacks()
        {
            return Ok(await _context.Quacks.ToListAsync());
        }


        [HttpGet("user/{username}")]
        public async Task<ActionResult<IEnumerable<Quack>>> GetQuacksFromUser(string username)
        {
            return Ok(await _context.Quacks.Where(q => q.Username == username).ToListAsync());
        }

        [HttpGet("{quackId}")]
        public async Task<ActionResult<Quack>> GetQuack(Guid quackId)
        {
            Quack quack = await _context.Quacks.FindAsync(quackId);

            if (quack != null)
            {
                return Ok(quack);
            }
            return NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Quack>> PostQuack(QuackRequest quackDto)
        {
            try
            {
                ClaimsPrincipal principal = User;

                var userId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value).SingleOrDefault());

                var username = User.Claims.Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault();


                Quack quack = new()
                {
                    Id = new Guid(),
                    Message = await ProfanityChecker.URL.PostJsonAsync(new { message = quackDto.Message }).ReceiveString(),
                    UserId = userId,
                    Username = username,
                    Mentions = new List<Mention>(),
                    Topics = new List<Topic>(),
                    CreatedOn = DateTime.Now,
                };

                var words = topicFinder.GetTopics(quackDto.Message);

                foreach (var word in words)
                {
                    if (word.Substring(0, 1) == "@")
                    {
                        quack.Mentions.Add(new Mention
                        {
                            Value = word,
                            CreatedOn = quack.CreatedOn,
                        });
                    }
                    else if (word.Substring(0, 1) == "#")
                    {
                        quack.Topics.Add(new Topic
                        {
                            Value = word,
                            CreatedOn = quack.CreatedOn,
                        });
                    }
                }

                await _context.Quacks.AddAsync(quack);
                await _context.SaveChangesAsync();

                var message = new NewQuackMessage()
                {
                    Id = quack.Id,
                    Message = quack.Message,
                    Mentions = quack.Mentions,
                    UserId = quack.UserId,
                    Username = quack.Username,
                    CreatedOn = quack.CreatedOn,
                };

                await _serviceBus.SendMessageAsync(message, topicName: "post-quack");

                return CreatedAtAction(nameof(GetQuack), new { quackId = quack.Id }, quack);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{quackId}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteQuack(Guid quackId)
        {
            try
            {
                var quack = await _context.Quacks.FindAsync(quackId);

                if (quack == null)
                {
                    return NotFound();
                }

                _context.Quacks.Remove(quack);
                await _context.SaveChangesAsync();

                await _serviceBus.SendMessageAsync(quackId, topicName: "delete-quack");

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
