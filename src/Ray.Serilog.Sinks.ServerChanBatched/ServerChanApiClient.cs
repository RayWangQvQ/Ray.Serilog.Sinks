using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.ServerChanBatched;

public class ServerChanApiClient : PushService
{
    //http://sc.ftqq.com/9.version

    private const string Host = "http://sc.ftqq.com";

    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new();

    public ServerChanApiClient(string scKey)
    {
        _apiUrl = new Uri($"{Host}/{scKey}.send");
    }

    protected override string ClientName => "Server酱";

    /// <summary>
    /// 需要两个才可以换行
    /// 只能换单行
    /// <br/>无效
    /// </summary>
    protected override string? NewLineStr => Environment.NewLine + Environment.NewLine;

    protected override string BuildMsg(string message, string title = "")
    {
        var msg = base.BuildMsg(message, title);
        msg +=
            $"{Environment.NewLine}### 检测到当前为老版Server酱,即将失效,建议更换其他推送方式或更新至Server酱Turbo版";
        return msg;
    }

    protected override async Task<HttpResponseMessage> DoSendAsync(
        string message,
        string title = ""
    )
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            title = Constants.DefaultTitle;
        }

        var dic = new Dictionary<string, string> { { "text", title }, { "desp", message } };
        var content = new FormUrlEncodedContent(dic);
        var response = await _httpClient.PostAsync(_apiUrl, content);
        return response;
    }
}
