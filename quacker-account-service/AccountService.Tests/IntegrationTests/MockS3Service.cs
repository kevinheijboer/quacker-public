using AccountService.Api.Logic;
using AccountService.Api.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountService.Tests.IntegrationTests
{
    public class MockS3Service : IS3Service
    {
        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            await Task.Delay(10);
            return fileName;
        }
    }
}
