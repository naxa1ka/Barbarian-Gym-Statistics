using RestSharp;

namespace BarbarianGymStatistics;

public class RestClientAdapter : IRestClient
{
    private readonly RestClient _restClient;

    public RestClientAdapter(RestClient restClient)
    {
        _restClient = restClient;
    }

    public Task<RestResponse> ExecuteAsync(
        RestRequest restRequest,
        CancellationToken cancellationToken = default
    )
    {
        return _restClient.ExecuteAsync(restRequest, cancellationToken);
    }
}
