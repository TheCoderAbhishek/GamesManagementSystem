using GamesManagementSystem.Application.Interfaces;
using GamesManagementSystem.Domain.Entities;
using GamesManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GamesManagementSystem.Infrastructure.Repositories
{
    public class GameRepository(AppDbContext context) : IGameRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Games.ToListAsync();
        }
    }
}
