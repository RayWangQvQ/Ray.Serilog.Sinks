using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.ServerChanBatched;

public class ServerChanTurboApiClient : PushService
{
    //http://sc.ftqq.com/9.version

    private const string Host = "https://sctapi.ftqq.com";

    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new();

    public ServerChanTurboApiClient(string scKey)
    {
        _apiUrl = new Uri($"{Host}/{scKey}.send");
    }

    protected override string ClientName => "Server酱Turbo版";

    /// <summary>
    /// 需要两个才可以换行
    /// 只能换单行
    /// <br/>无效
    /// </summary>
    protected override string? NewLineStr => Environment.NewLine + Environment.NewLine;

    protected override async Task<HttpResponseMessage> DoSendAsync(
        string message,
        string title = ""
    )
    {
        var dic = new Dictionary<string, string>
        {
            { "title", title }, //标题必填
            { "desp", message },
        };
        var content = new FormUrlEncodedContent(dic);
        var response = await _httpClient.PostAsync(_apiUrl, content);
        return response;
    }
}
