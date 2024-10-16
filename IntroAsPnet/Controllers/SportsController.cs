using IntroAsPnet.Servicios;
using IntroAsPnet.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;


namespace IntroAsPnet.Controllers;
public class SportsController : Controller
{
    private readonly AllSportsApiService _apiService;
    public SportsController(AllSportsApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpGet]
    public IActionResult SearchPlayer()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SearchPlayer(string firstName,string lastName)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
        {
            return View(new List<PlayerModel>());
        }
        try
        {
            var players = await _apiService.SearchPlayersByFullNameAsync(firstName,lastName);
            return View(players);
        }
        catch(Exception ex)
        {
            ViewBag.Error = $"Error al bucar jugadores: {ex.Message}";
            return View(new List<PlayerModel>());
        }
    }

}
