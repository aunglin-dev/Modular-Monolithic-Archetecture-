using LoginSolution.Shared.Domain.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoginSolution.Shared.Domain.ServiceFilters;

public sealed class ActiveUserServiceFilter : IActionFilter { public void OnActionExecuting(ActionExecutingContext context) { if (context.HttpContext.User.HasClaim("is_active", "true")) return; context.Result = new ObjectResult(ApiResponseModel.Failure("The user account is inactive.")) { StatusCode = StatusCodes.Status403Forbidden }; } public void OnActionExecuted(ActionExecutedContext context) { } }
