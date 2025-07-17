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
    }
}
