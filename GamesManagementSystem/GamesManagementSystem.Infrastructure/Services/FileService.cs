using GamesManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GamesManagementSystem.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public async Task SaveFileAsync(IFormFile file, string physicalPath)
        {
            await using var fileStream = new FileStream(physicalPath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }
    }
}
