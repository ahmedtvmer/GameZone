
using System.Threading.Tasks;

namespace GameZone.Services
{
    public class GamesServices : IGamesServices
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;
        public GamesServices(AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}/Assets/Images/Games";
        }

        public async Task AddGame(GamesViewModel model)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(model.Cover.FileName)}";
            var coverPath = $"{_imagesPath}/{coverName}";
            using var stream = File.Create(coverPath);
            await model.Cover.CopyToAsync(stream);

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
    }
}
