using AguasSetubal.Data;
using AguasSetubal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace AguasSetubal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string message)
        {
            ViewBag.Message = message;
            var listCounters = _context.RequisicaoContador.Where(r => r.IsValid == false).ToList();
            ViewBag.NewCounters = listCounters.Any() ? listCounters.Count().ToString() : null;

            var listCountersAdmin = _context.RequisicaoContador.Where(r => r.IsValid == true && r.IsRequested == false).ToList();
            ViewBag.NewCountersAdmin = listCountersAdmin.Any() ? listCountersAdmin.Count().ToString() : null;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Método que lida com erros
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Método para lidar com erros 404 (Página Não Encontrada)
        public IActionResult NotFound()
        {
            return View();
        }

        // Método para lidar com erros 403 (Acesso Negado)
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}

