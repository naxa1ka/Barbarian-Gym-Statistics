using RestSharp;

namespace BarbarianGymStatistics;

public class RestClient : IRestClient
{
    public RestClient()
    {
    }

    public Task<RestResponse> ExecuteAsync(RestRequest restRequest, CancellationToken cancellationToken = default)
    {
        
    }
}