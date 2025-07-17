using GamesManagementSystem.Application.Interfaces;
using GamesManagementSystem.Domain.Entities;
using GamesManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GamesManagementSystem.Infrastructure.Repositories
{
    public class GameRepository(AppDbContext context, IMemoryCache memoryCache) : IGameRepository
    {
        private readonly AppDbContext _context = context;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private const string GamesCacheKey = "AllGames";

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task AddAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            // 1. Try to get the list from the cache
            if (_memoryCache.TryGetValue(GamesCacheKey, out IEnumerable<Game> games))
            {
                // Cache hit! Return the cached data.
                return games!;
            }

            // 2. Cache miss. Get data from the database.
            games = await _context.Games.AsNoTracking().ToListAsync();

            // 3. Store the result in the cache with an expiration policy.
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Keep in cache for 5 mins from last access.

            _memoryCache.Set(GamesCacheKey, games, cacheEntryOptions);

            return games;
        }

        public async Task UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
            _memoryCache.Remove(GamesCacheKey);
        }

        public async Task DeleteAsync(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
                await _context.SaveChangesAsync();
                _memoryCache.Remove(GamesCacheKey);
            }
        }
    }
}
