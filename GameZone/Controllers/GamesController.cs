
using Microsoft.AspNetCore.Http.HttpResults;

namespace GameZone.Controllers
{
    public class GamesController : Controller
    {
        private readonly ICategoriesServices categories;
        private readonly IDevicesServices devices;
        private readonly IGamesServices _gameService;

        public GamesController(ICategoriesServices categories, IDevicesServices devices, IGamesServices games)
        {
            this.categories = categories;
            this.devices = devices;
            this._gameService = games;
        }
        public IActionResult Index([FromQuery] PaginationViewModel pagination, string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;
            var games = searchTerm is not null
                ? _gameService.GetGamesBySearch(pagination.CurrentPage, pagination.PageSize, searchTerm)
                : _gameService.GetAll(pagination.CurrentPage, pagination.PageSize);

            var totalItems = _gameService.GetCount(searchTerm!);
            PaginationViewModel model = new()
            {
                Games = games,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pagination.PageSize),
                CurrentPage = pagination.CurrentPage,
                PageSize = pagination.PageSize
            };
            return View(model);
        }

        public IActionResult Details(int id)
        {
            Game game = _gameService.GetGame(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateGameViewModel model = new CreateGameViewModel()
            {
                Categories = categories.GetSelectedItems(),

                Devices = devices.GetSelectedItems()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = categories.GetSelectedItems();

                model.Devices = devices.GetSelectedItems();

                return View(model);
            }

            await _gameService.AddGame(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            Game game = _gameService.GetGame(id);

            EditGameViewModel model = new()
            {
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.GameDevices.Select(x => x.DeviceId),
                Categories = categories.GetSelectedItems(),
                Devices = devices.GetSelectedItems(),
                CurrentCover = game.Cover
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditGameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = categories.GetSelectedItems();
                model.Devices = devices.GetSelectedItems();
                return View(model);
            }
            try
            {
                await _gameService.UpdateGame(id, model);
            }
            catch
            {
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = _gameService.RemoveGame(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
