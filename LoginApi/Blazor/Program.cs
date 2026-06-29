using Blazor;
using Blazor.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder =
    WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");

builder.RootComponents.Add<HeadOutlet>(
    "head::after");

builder.Services.AddScoped(
    serviceProvider =>
        new HttpClient
        {
            BaseAddress =
                new Uri("https://localhost:7001/")
        });

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<
    JwtAuthenticationStateProvider>();

builder.Services.AddScoped<
    AuthenticationStateProvider>(
        serviceProvider =>
            serviceProvider.GetRequiredService<
                JwtAuthenticationStateProvider>());

await builder.Build().RunAsync();