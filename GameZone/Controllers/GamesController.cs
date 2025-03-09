
namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesServices categories;
        private readonly IDevicesServices devices;
        private readonly IGamesServices games;

        public GamesController(ICategoriesServices categories, IDevicesServices devices, IGamesServices games)
        {
            this.categories = categories;
            this.devices = devices;
            this.games = games;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            GamesViewModel model = new GamesViewModel()
            {
                Categories = categories.GetSelectedItems(),

                Devices = devices.GetSelectedItems()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GamesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = categories.GetSelectedItems();

                model.Devices = devices.GetSelectedItems();

                return View(model);
            }

            await games.AddGame(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
