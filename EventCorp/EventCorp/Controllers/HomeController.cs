using System.Diagnostics;
using CoreLibrary.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EventCorp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("Error404");  // Vista para error 404
                case 403:
                    return View("Error403");  // Vista para acceso denegado
                case 500:
                    return View("Error500");  // Vista para error interno del servidor
                default:
                    return View("Error");  // Vista genérica para otros errores
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
