using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebStatus.Models;

namespace WebStatus.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
    {
    private readonly ILogger<HomeController> _logger = logger;

    public IActionResult Index()
    {
        return Redirect("/healthchecks-ui");
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
