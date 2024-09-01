using Bdt.Client.Provider.Token;
using System.Net.Http.Headers;

namespace Bdt.Client.Handller;

public class CustomAuthorisationMessageHandler : DelegatingHandler
{
    private readonly ITokenProvider _tokenProvider;

    public CustomAuthorisationMessageHandler(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetAccessTokenAsync();

        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
