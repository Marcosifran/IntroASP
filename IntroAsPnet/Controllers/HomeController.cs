using IntroAsPnet.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IntroAsPnet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Registro()
        {
            return View();
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LigasTorneos()
        {
            return View();
        }
        public IActionResult Equipos()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
