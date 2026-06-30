using LoginSolution.Shared.Domain.Models.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoginSolution.Shared.Domain.ServiceFilters;

public sealed class ValidationServiceFilter : IActionFilter { public void OnActionExecuting(ActionExecutingContext context) { if (context.ModelState.IsValid) return; var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray(); context.Result = new BadRequestObjectResult(ApiResponseModel.Failure("Validation failed.", errors)); } public void OnActionExecuted(ActionExecutedContext context) { } }
