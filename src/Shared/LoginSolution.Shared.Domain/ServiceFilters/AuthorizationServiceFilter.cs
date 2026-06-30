using LoginSolution.Shared.Domain.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoginSolution.Shared.Domain.ServiceFilters;

public sealed class AuthorizationServiceFilter : IAuthorizationFilter { public void OnAuthorization(AuthorizationFilterContext context) { if (context.HttpContext.User.Identity?.IsAuthenticated == true) return; context.Result = new UnauthorizedObjectResult(ApiResponseModel.Failure("Authentication is required.")); } }
