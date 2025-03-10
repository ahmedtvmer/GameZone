
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GameZone.Services
{
    public class GamesServices : IGamesServices
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        private readonly IDevicesServices _devicesServices;
        public GamesServices(AppDbContext context,
            IWebHostEnvironment webHostEnvironment,
            IDevicesServices devicesServices)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}";
            _devicesServices = devicesServices;
        }

        public IEnumerable<Game> GetAll(int currentPage = 1, int PageSize = 6)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevices)
                .ThenInclude(gd => gd.Device)
                .AsQueryable()
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .AsNoTracking()
                .ToList();
        }

        public async Task AddGame(CreateGameViewModel model)
        {
            var coverName = await SaveCover(model.Cover);

            var game = new Game
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                GameDevices = model.SelectedDevices.Select(deviceId => new GameDevice
                {
                    DeviceId = deviceId
                }).ToList()
            };

            _context.Add(game);
            _context.SaveChanges();
        }

        public Game GetGame(int id)
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevices)
                .ThenInclude(gd => gd.Device)
                .AsNoTracking()
                .FirstOrDefault(g => g.Id == id);
        }

        public bool RemoveGame(int id)
        {
            var isDeleted = false;

            var game = _context.Games.FirstOrDefault(g => g.Id == id);

            if (game is null)
                return isDeleted;

            _context.Remove(game);
            var rowsAffected = _context.SaveChanges();

            if (rowsAffected > 0)
            {
                isDeleted = true;

                var coverPath = Path.Combine(_imagesPath, game.Cover);
                if (File.Exists(coverPath))
                {
                    File.Delete(coverPath);
                }

            }

            return isDeleted;
        }

        public async Task UpdateGame(int id, EditGameViewModel model)
        {
            var game = await _context.Games
                .Include(g => g.GameDevices)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                throw new InvalidOperationException("Game not found");
            }

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;

            string oldCover = game.Cover;
            if (model.Cover != null)
            {
                game.Cover = await SaveCover(model.Cover);
            }

            var existingDevices = game.GameDevices.ToList();
            foreach (var device in existingDevices)
            {
                game.GameDevices.Remove(device);
            }
            game.GameDevices = model.SelectedDevices.Select(deviceId => new GameDevice
            {
                GameId = id,
                DeviceId = deviceId
            }).ToList();

            int rowsAffected;
            try
            {
                rowsAffected = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                throw;
            }

            if (rowsAffected > 0)
            {
                if (model.Cover != null)
                {
                    var coverPath = Path.Combine(_imagesPath, oldCover);
                    if (File.Exists(coverPath))
                    {
                        File.Delete(coverPath);
                    }
                }
            }
            else
            {
                if (model.Cover != null)
                {
                    var coverPath = Path.Combine(_imagesPath, game.Cover);
                    if (File.Exists(coverPath))
                    {
                        File.Delete(coverPath);
                    }
                }
                throw new InvalidOperationException("Failed to update game: No rows affected.");
            }
        }

        public IEnumerable<Game> GetGamesBySearch(int currentPage = 1, int PageSize = 6, string searchTerm = "")
        {
            return _context.Games
                .Include(g => g.Category)
                .Include(g => g.GameDevices)
                .ThenInclude(gd => gd.Device)
                .AsQueryable()
                .Skip((currentPage - 1) * PageSize)
                .Take(PageSize)
                .Where(g => g.Name.ToLower().Contains(searchTerm.ToLower()))
                .AsNoTracking()
                .ToList();

        }

        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var coverPath = $"{_imagesPath}/{coverName}";
            using var stream = File.Create(coverPath);
            await cover.CopyToAsync(stream);
            return coverName;
        }

        public int GetCount(string searchTerm)
        {
            return searchTerm is null
                ? _context.Games.Count()
                : _context.Games.Where(g => g.Name.ToLower().Contains(searchTerm.ToLower())).Count();
        }
    }
}
