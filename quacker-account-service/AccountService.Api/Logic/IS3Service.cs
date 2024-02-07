using AccountService.Api.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AccountService.Api.Logic
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(IFormFile file, string fileName);
    }
}