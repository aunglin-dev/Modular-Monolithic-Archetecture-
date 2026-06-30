using System.Net.Http.Headers;
using System.Net.Http.Json;
using LoginSolution.IBanking.Mvc.Services.Interfaces;
using LoginSolution.Shared.Domain.Models.Accounts;
using LoginSolution.Shared.Domain.Models.Authentication;
using LoginSolution.Shared.Domain.Models.Common;
using LoginSolution.Shared.Domain.Models.Users;

namespace LoginSolution.IBanking.Mvc.Services;

public sealed class IBankingApiService : IIBankingApiService
{
    private readonly HttpClient _http; private readonly IHttpContextAccessor _context;
    public IBankingApiService(HttpClient http, IHttpContextAccessor context) { _http = http; _context = context; }
    public async Task<LoginResponseModel?> LoginAsync(LoginRequestModel model, CancellationToken ct = default) => (await PostAsync<LoginRequestModel, LoginResponseModel>("api/authentication/login", model, ct))?.Data;
    public async Task<UserResponseModel?> GetProfileAsync(CancellationToken ct = default) => (await GetAsync<UserResponseModel>("api/profile/me", ct))?.Data;
    public async Task<AccountResponseModel?> GetAccountAsync(CancellationToken ct = default) => (await GetAsync<AccountResponseModel>("api/account", ct))?.Data;
    public async Task<AccountBalanceModel?> GetBalanceAsync(CancellationToken ct = default) => (await GetAsync<AccountBalanceModel>("api/account/balance", ct))?.Data;
    public async Task<bool> ChangePasswordAsync(ChangePasswordModel model, CancellationToken ct = default) => (await PostAsync<ChangePasswordModel, object>("api/authentication/change-password", model, ct))?.IsSuccess == true;
    private async Task<ApiResponseModel<T>?> GetAsync<T>(string url, CancellationToken ct) { AddBearer(); return await _http.GetFromJsonAsync<ApiResponseModel<T>>(url, ct); }
    private async Task<ApiResponseModel<TOut>?> PostAsync<TIn,TOut>(string url, TIn model, CancellationToken ct) { AddBearer(); var response = await _http.PostAsJsonAsync(url, model, ct); return await response.Content.ReadFromJsonAsync<ApiResponseModel<TOut>>(cancellationToken: ct); }
    private void AddBearer() { var token = _context.HttpContext?.Session.GetString("AccessToken"); _http.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token) ? null : new AuthenticationHeaderValue("Bearer", token); }
}
