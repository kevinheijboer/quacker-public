using AccountService.Api.Data;
using AccountService.Api.Logic;
using AccountService.Api.Models;
using AccountService.Api.Models.Payloads.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccountService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ILogger<AccountsController> _logger;
        private readonly IS3Service _s3service;
        private readonly ApplicationDbContext _context;

        public AccountsController(ILogger<AccountsController> logger, ApplicationDbContext context, IS3Service s3service)
        {
            _logger = logger;
            _context = context;
            _s3service = s3service;
        }

        [HttpGet("{username}")]
        [Authorize]
        public async Task<ActionResult<Account>> GetAccount(string username)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(u => u.Username == username);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPut("{userId}")]
        [Authorize]

        public async Task<ActionResult<Account>> UpdateAccount(string userId)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(userId));

            if (account == null)
            {
                return NotFound();
            }

            if (Request.Form.Files.Count >= 1)
            {
                var file = Request.Form.Files.ToList().FirstOrDefault();

                if (file.ContentType.Contains("image"))
                {
                    try
                    {
                        account.ProfilePictureURL = await _s3service.UploadFileAsync(file, account.Username);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            var req = JsonConvert.DeserializeObject<EditProfileRequest>(Request.Form["profile"]);

            account.Name = req.Name;
            account.Bio = req.Bio;
            account.Location = req.Location;
            account.Website = req.Website;

            await _context.SaveChangesAsync();

            return Ok(account);
        }
    }
}
