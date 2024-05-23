using RestSharp;

namespace BarbarianGymStatistics;

public interface IRestClient
{
    Task<RestResponse> ExecuteAsync(
        RestRequest restRequest,
        CancellationToken cancellationToken = default
    );
}
