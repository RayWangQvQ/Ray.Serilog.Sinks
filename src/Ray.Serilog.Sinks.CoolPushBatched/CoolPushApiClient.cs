using System.Text;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.CoolPushBatched;

public class CoolPushApiClient : PushService
{
    private const string Host = "https://push.xuthus.cc/send";

    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new();

    public CoolPushApiClient(string sKey)
    {
        _apiUrl = new Uri($"{Host}/{sKey}");
    }

    protected override string ClientName => "酷推";

    protected override string? NewLineStr => Environment.NewLine + Environment.NewLine;

    protected override string BuildMsg(string message, string title = "")
    {
        //附加标题
        var msg = title + Environment.NewLine + message;

        if (!string.IsNullOrEmpty(NewLineStr))
            msg = msg.Replace(Environment.NewLine, NewLineStr);

        return msg;
    }

    protected override async Task<HttpResponseMessage> DoSendAsync(string message, string title = "")
    {
        var content = new StringContent(message, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_apiUrl, content);
        return response;
    }
}
