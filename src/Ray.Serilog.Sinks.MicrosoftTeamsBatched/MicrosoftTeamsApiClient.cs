using System.Text;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.MicrosoftTeamsBatched;

public class MicrosoftTeamsApiClient : PushService
{
    //https://docs.microsoft.com/en-us/microsoftteams/platform/webhooks-and-connectors/how-to/add-incoming-webhook

    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new();

    public MicrosoftTeamsApiClient(string webhook)
    {
        _apiUrl = new Uri(webhook);
    }

    protected override string ClientName => "MicrosoftTeams";

    protected override string? NewLineStr => "<br/>";

    protected override async Task<HttpResponseMessage> DoSendAsync(string message, string title = "")
    {
        var json = new { text = message }.ToJsonStr();

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_apiUrl, content);
        return response;
    }
}
