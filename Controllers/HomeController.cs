using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagementMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // Allow anonymous access to the home page
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    // Require authentication for the Privacy page
    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
