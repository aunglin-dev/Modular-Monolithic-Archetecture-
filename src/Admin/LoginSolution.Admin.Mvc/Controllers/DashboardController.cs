using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginSolution.Admin.Mvc.Controllers;

[Authorize(Roles = "Admin")]
public sealed class DashboardController : Controller { public IActionResult Index() => View(); }
