using System.ComponentModel.DataAnnotations;

namespace LoginSolution.Shared.Domain.Models.Common;

public sealed class PaginationRequestModel
{
    [Range(1, int.MaxValue)] public int PageNumber { get; set; } = 1;
    [Range(1, 100)] public int PageSize { get; set; } = 10;
}
