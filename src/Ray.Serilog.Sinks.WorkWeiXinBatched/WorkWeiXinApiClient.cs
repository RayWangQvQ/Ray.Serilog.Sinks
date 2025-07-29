using System.Text;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.WorkWeiXinBatched;

public class WorkWeiXinApiClient : PushService
{
    private readonly WorkWeiXinMsgType _msgType;
    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new();

    public WorkWeiXinApiClient(string webHookUrl, WorkWeiXinMsgType msgType)
    {
        _msgType = msgType;
        _apiUrl = new Uri(webHookUrl);
    }

    protected override string ClientName => "企业微信机器人";

    /// <summary>
    /// 换行符
    /// （经测试，使用\r\n可以正常换多行，使用\n仅可以换单行）
    /// 不能用<br/>换行
    /// </summary>
    protected override string NewLineStr => "\r\n";

    protected override string BuildMsg(string message, string title = "")
    {
        //附加标题
        if (string.IsNullOrWhiteSpace(title))
        {
            title = Constants.DefaultTitle;
        }
        var msg = $"## {title} {Environment.NewLine}{Environment.NewLine}{message}";

        if (!string.IsNullOrEmpty(NewLineStr))
            msg = msg.Replace(Environment.NewLine, NewLineStr);

        return msg;
    }

    protected override async Task<HttpResponseMessage> DoSendAsync(
        string message,
        string title = ""
    )
    {
        object json;

        if (_msgType == WorkWeiXinMsgType.markdown)
        {
            json = new
            {
                msgtype = _msgType.ToString(),
                markdown = new { content = message }
            };
        }
        else
        {
            json = new
            {
                msgtype = _msgType.ToString(),
                text = new { content = message }
            };
        }

        var content = new StringContent(json.ToJsonStr(), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_apiUrl, content);
        return response;
    }
}
