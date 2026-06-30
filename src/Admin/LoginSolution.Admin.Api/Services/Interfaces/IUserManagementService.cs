using LoginSolution.Shared.Domain.Models.Common;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.Admin.Api.Services.Interfaces;

public interface IUserManagementService
{
    Task<UserListModel> GetUsersAsync(PaginationRequestModel pagination, CancellationToken cancellationToken = default);
    Task<UserResponseModel> GetUserAsync(int id, CancellationToken cancellationToken = default);
    Task<UserResponseModel> CreateUserAsync(CreateUserModel model, CancellationToken cancellationToken = default);
    Task<UserResponseModel> UpdateUserAsync(int id, UpdateUserModel model, CancellationToken cancellationToken = default);
    Task SetStatusAsync(int id, bool isActive, CancellationToken cancellationToken = default);
}
