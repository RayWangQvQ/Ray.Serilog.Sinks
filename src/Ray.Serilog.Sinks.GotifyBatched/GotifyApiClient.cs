using System.Text;
using Newtonsoft.Json.Linq;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.GotifyBatched;

public class GotifyApiClient : PushService
{
    //https://gotify.net/docs

    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new();
    private readonly string _token;

    public GotifyApiClient(string host, string token)
    {
        _token = token;
        _apiUrl = new Uri($"{host}/message");
    }

    protected override string ClientName => "Gotify";

    protected override string? NewLineStr => "\n";

    protected override async Task<HttpResponseMessage> DoSendAsync(
        string message,
        string title = ""
    )
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            title = Constants.DefaultTitle;
        }
        var json = new
        {
            title = title,
            message = message,
            extras = "{\"extras\":{\"client::display\":{\"contentType\":\"text/markdown\"}}}".JsonDeserialize<JObject>(),
        }.ToJsonStr();

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        content.Headers.Add("X-Gotify-Key", _token);
        var response = await _httpClient.PostAsync(_apiUrl, content);
        response.Content = new StringContent("");
        return response;
    }
}
