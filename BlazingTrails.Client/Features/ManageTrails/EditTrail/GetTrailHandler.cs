using System.Net.Http.Json;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using MediatR;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail
{
    public class GetTrailHandler : IRequestHandler<GetTrailRequest, GetTrailRequest.Response?>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GetTrailHandler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<GetTrailRequest.Response?> Handle(GetTrailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var url = GetTrailRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString());
                var client = _httpClientFactory.CreateClient("SecureAPIClient");

                return await client.GetFromJsonAsync<GetTrailRequest.Response>(url);
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }
    }

}
