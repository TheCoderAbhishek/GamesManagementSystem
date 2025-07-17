using GamesManagementSystem.Application.Interfaces;
using GamesManagementSystem.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GamesManagementSystem.Web.Controllers
{
    public class GamesController(IGameRepository gameRepository, IFileService fileService, IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IGameRepository _gameRepository = gameRepository;
        private readonly IFileService _fileService = fileService;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<IActionResult> Index()
        {
            var games = await _gameRepository.GetAllAsync();
            return View(games);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GameViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                string? relativeImagePath = null;

                if (viewModel.BannerImageFile != null)
                {
                    // 1. Define the web-accessible path for the database
                    string uploadsFolder = "/images/banners/";
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.BannerImageFile.FileName;
                    relativeImagePath = uploadsFolder + uniqueFileName;

                    // 2. Define the full physical path for saving the file
                    string physicalDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", "banners");
                    string physicalPath = Path.Combine(physicalDirectory, uniqueFileName);

                    // 3. Ensure the directory exists
                    if (!Directory.Exists(physicalDirectory))
                    {
                        Directory.CreateDirectory(physicalDirectory);
                    }

                    // 4. Call the simplified service
                    await _fileService.SaveFileAsync(viewModel.BannerImageFile, physicalPath);
                }

                var game = new Domain.Entities.Game
                {
                    Name = viewModel.Name,
                    DevelopedBy = viewModel.DevelopedBy,
                    ReleaseDate = viewModel.ReleaseDate,
                    Description = viewModel.Description,
                    BannerImagePath = relativeImagePath
                };

                await _gameRepository.AddAsync(game);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            // Map the domain entity to the view model
            var viewModel = new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                DevelopedBy = game.DevelopedBy,
                ReleaseDate = game.ReleaseDate,
                Description = game.Description
            };

            ViewData["ExistingImagePath"] = game.BannerImagePath;
            return View(viewModel);
        }

        // POST: Games/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GameViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var gameToUpdate = await _gameRepository.GetByIdAsync(id);
                if (gameToUpdate == null) return NotFound();

                // Handle file upload
                if (viewModel.BannerImageFile != null)
                {
                    // Note: In a real app, you'd also delete the old file
                    string uploadsFolder = "/images/banners/";
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.BannerImageFile.FileName;
                    string physicalDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", "banners");
                    string physicalPath = Path.Combine(physicalDirectory, uniqueFileName);

                    if (!Directory.Exists(physicalDirectory))
                    {
                        Directory.CreateDirectory(physicalDirectory);
                    }

                    await _fileService.SaveFileAsync(viewModel.BannerImageFile, physicalPath);
                    gameToUpdate.BannerImagePath = uploadsFolder + uniqueFileName;
                }

                // Map updated fields from view model to the entity
                gameToUpdate.Name = viewModel.Name;
                gameToUpdate.DevelopedBy = viewModel.DevelopedBy;
                gameToUpdate.ReleaseDate = viewModel.ReleaseDate;
                gameToUpdate.Description = viewModel.Description;

                await _gameRepository.UpdateAsync(gameToUpdate);
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _gameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gameRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
