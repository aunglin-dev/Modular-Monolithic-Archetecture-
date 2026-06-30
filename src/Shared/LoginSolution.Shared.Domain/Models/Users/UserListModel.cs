using LoginSolution.Shared.Domain.Models.Common;

namespace LoginSolution.Shared.Domain.Models.Users;

public sealed class UserListModel
{
    public IReadOnlyList<UserResponseModel> Users { get; set; } = Array.Empty<UserResponseModel>();
    public PaginationResponseModel Pagination { get; set; } = new();
}
