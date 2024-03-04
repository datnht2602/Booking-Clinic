using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorClient.Security;
[Authorize]
public class Index : ComponentBase
{
    [Inject] private IAccessTokenProvider TokenProvider { get; set; }
    public string AccessToken { get; set; }
    protected override async Task OnInitializedAsync()
    {
        var accessTokenResult = await TokenProvider.RequestAccessToken();
        AccessToken = string.Empty;

        if (accessTokenResult.TryGetToken(out var token))
        {
            AccessToken = token.Value;
        }
    }
}