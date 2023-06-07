using System.Net.Http.Json;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using MediatR;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail
{
    public class GetTrailHandler : IRequestHandler<GetTrailRequest, GetTrailRequest.Response?>
    {
        private readonly HttpClient _httpClient;

        public GetTrailHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetTrailRequest.Response?> Handle(GetTrailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var url = GetTrailRequest.RouteTemplate.Replace("{trailId}", request.TrailId.ToString());

                return await _httpClient.GetFromJsonAsync<GetTrailRequest.Response>(url);
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }
    }

}
