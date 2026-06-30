using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.IBanking.Mvc.Controllers;

public sealed class HomeController : Controller { public IActionResult Index() => View(); public IActionResult AccessDenied() => View(); public IActionResult Error() => View(); }
