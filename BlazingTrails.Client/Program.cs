using System.Security.Claims;
using BlazingTrails.Client.Features.Auth;
using BlazingTrails.Client.State;
using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazingTrails.Client;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        builder.Services.AddHttpClient("SecureAPIClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
        builder.Services.AddMediatR(typeof(Program).Assembly);

        builder.Services.AddOidcAuthentication(options =>
        {
            builder.Configuration.Bind("Auth0", options.ProviderOptions);
            options.ProviderOptions.ResponseType = "code";
        });
        builder.Services.AddOidcAuthentication(options =>
        {
            builder.Configuration.Bind("Auth0", options.ProviderOptions);
            options.ProviderOptions.ResponseType = "code";
            options.UserOptions.NameClaim = ClaimTypes.GivenName;
        }).AddAccountClaimsPrincipalFactory<CustomUserFactory<RemoteUserAccount>>();

        builder.Services.AddScoped<AppState>();

        builder.Services.AddBlazoredLocalStorage();

        await builder.Build().RunAsync();
    }
}