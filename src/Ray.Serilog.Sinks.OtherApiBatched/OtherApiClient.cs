using System.Net.Http;
using System.Text;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.OtherApiBatched;

public class OtherApiClient : PushService
{
    private readonly Uri _apiUri;
    private readonly string _jsonTemplate;
    private readonly string _placeholder;

    private readonly HttpClient _httpClient = new();

    public OtherApiClient(string apiUrl, string jsonTemplate, string placeholder)
    {
        _jsonTemplate = jsonTemplate;
        _placeholder = placeholder;
        _apiUri = new Uri(apiUrl);
    }

    protected override string ClientName => "自定义";

    protected override string BuildMsg(string message, string title = "")
    {
        var msg = base.BuildMsg(message, title);
        return _jsonTemplate.Replace(_placeholder, msg.ToJsonStr());
    }

    protected override HttpResponseMessage DoSend(string message, string title = "")
    {
        var content = new StringContent(message, Encoding.UTF8, "application/json");
        var response = _httpClient.PostAsync(_apiUri, content).GetAwaiter().GetResult();
        return response;
    }
}
