using System.Net.Http.Headers;
using System.Net.Http.Json;
using LoginSolution.Admin.Mvc.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Authentication;
using LoginSolution.Shared.Domain.Models.Common;
using LoginSolution.Shared.Domain.Models.Roles;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.Admin.Mvc.Services;

public sealed class AdminApiService : IAdminApiService
{
    private readonly HttpClient _http; private readonly IHttpContextAccessor _ctx;
    public AdminApiService(HttpClient http, IHttpContextAccessor ctx) { _http = http; _ctx = ctx; }
    public async Task<LoginResponseModel?> LoginAsync(LoginRequestModel model, CancellationToken ct = default) => (await PostAsync<LoginRequestModel, LoginResponseModel>("api/admin-authentication/login", model, ct))?.Data;
    public async Task<UserListModel?> GetUsersAsync(int pageNumber = 1, int pageSize = 20, CancellationToken ct = default) => (await GetAsync<UserListModel>($"api/users?pageNumber={pageNumber}&pageSize={pageSize}", ct))?.Data;
    public async Task<UserResponseModel?> GetUserAsync(int id, CancellationToken ct = default) => (await GetAsync<UserResponseModel>($"api/users/{id}", ct))?.Data;
    public async Task<UserResponseModel?> CreateUserAsync(CreateUserModel model, CancellationToken ct = default) => (await PostAsync<CreateUserModel, UserResponseModel>("api/users", model, ct))?.Data;
    public async Task<UserResponseModel?> UpdateUserAsync(int id, UpdateUserModel model, CancellationToken ct = default) => (await PutAsync<UpdateUserModel, UserResponseModel>($"api/users/{id}", model, ct))?.Data;
    public async Task<bool> SetUserStatusAsync(int id, bool isActive, CancellationToken ct = default) { AddBearer(); var r = await _http.PatchAsync($"api/users/{id}/status?isActive={isActive}", null, ct); return r.IsSuccessStatusCode; }
    public async Task<IReadOnlyList<RoleResponseModel>> GetRolesAsync(CancellationToken ct = default) => (await GetAsync<IReadOnlyList<RoleResponseModel>>("api/roles", ct))?.Data ?? Array.Empty<RoleResponseModel>();
    public async Task<RoleResponseModel?> GetRoleAsync(int id, CancellationToken ct = default) => (await GetAsync<RoleResponseModel>($"api/roles/{id}", ct))?.Data;
    public async Task<RoleResponseModel?> CreateRoleAsync(CreateRoleModel model, CancellationToken ct = default) => (await PostAsync<CreateRoleModel, RoleResponseModel>("api/roles", model, ct))?.Data;
    public async Task<RoleResponseModel?> UpdateRoleAsync(int id, CreateRoleModel model, CancellationToken ct = default) => (await PutAsync<CreateRoleModel, RoleResponseModel>($"api/roles/{id}", model, ct))?.Data;
    private async Task<ApiResponseModel<T>?> GetAsync<T>(string url, CancellationToken ct) { AddBearer(); return await _http.GetFromJsonAsync<ApiResponseModel<T>>(url, ct); }
    private async Task<ApiResponseModel<TOut>?> PostAsync<TIn,TOut>(string url, TIn model, CancellationToken ct) { AddBearer(); var r = await _http.PostAsJsonAsync(url, model, ct); return await r.Content.ReadFromJsonAsync<ApiResponseModel<TOut>>(cancellationToken: ct); }
    private async Task<ApiResponseModel<TOut>?> PutAsync<TIn,TOut>(string url, TIn model, CancellationToken ct) { AddBearer(); var r = await _http.PutAsJsonAsync(url, model, ct); return await r.Content.ReadFromJsonAsync<ApiResponseModel<TOut>>(cancellationToken: ct); }
    private void AddBearer() { var token = _ctx.HttpContext?.Session.GetString("AccessToken"); _http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token) ? null : new AuthenticationHeaderValue("Bearer", token); }
}
