using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GameZone.Models;
using Microsoft.AspNetCore.Authorization;

namespace GameZone.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly IGamesServices _gamesServices;

    public HomeController(IGamesServices gamesServices)
    {
        _gamesServices = gamesServices;
    }

    public IActionResult Index()
    {
        return View(_gamesServices.GetAll());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
