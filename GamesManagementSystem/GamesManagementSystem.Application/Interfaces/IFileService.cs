using Microsoft.AspNetCore.Http;

namespace GamesManagementSystem.Application.Interfaces
{
    public interface IFileService
    {
        Task SaveFileAsync(IFormFile file, string physicalPath);
    }
}
