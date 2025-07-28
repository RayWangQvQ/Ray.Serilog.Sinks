using System.Security.Cryptography;
using System.Text;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.DingTalkBatched;

public class DingTalkApiClient : PushService
{
    //https://developers.dingtalk.com/document/app/overview-of-group-robots

    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new();

    public DingTalkApiClient(string webHookUrl)
    {
        if (null != webHookUrl && webHookUrl.Contains("secret="))
        {
            _apiUrl = new Uri(ToGetSignUrl(webHookUrl));
        }
        else
        {
            _apiUrl = new Uri(webHookUrl);
        }
    }

    protected override string ClientName => "钉钉机器人";

    /// <summary>
    /// <br/>换行无效
    /// 文档里是\n换行
    /// 只能换1行
    /// </summary>
    protected override string? NewLineStr => Environment.NewLine + Environment.NewLine;

    protected override string BuildMsg(string message, string title = "")
    {
        //附加标题
        var msg = $"## {title} {Environment.NewLine}{message}";

        if (!string.IsNullOrEmpty(NewLineStr))
            msg = msg.Replace(Environment.NewLine, NewLineStr);

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

        var json = new
        {
            msgtype = nameof(DingMsgType.markdown),
            markdown = new { title = title, text = message },
        }.ToJsonStr();
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_apiUrl, content);
        return response;
    }

    // hmac256加密并返回base64
    public static string ToBase64hmac(string strText, string strKey)
    {
        HMACSHA256 myHMACSHA256 = new HMACSHA256(Encoding.UTF8.GetBytes(strKey));
        byte[] byteText = myHMACSHA256.ComputeHash(Encoding.UTF8.GetBytes(strText));
        return System.Convert.ToBase64String(byteText);
    }

    // 推送钉钉消息url加上签名
    public static string ToGetSignUrl(string webHookUrl)
    {
        var secret = "";
        var temp = webHookUrl.Replace("https://oapi.dingtalk.com/robot/send?", "");
        string[] vs = temp.Split("&");
        for (int i = 0; i < vs.Length; i++)
        {
            if (vs[i].StartsWith("secret="))
            {
                secret = vs[i].Replace("secret=", "");
                break;
            }
        }
        var current = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        var string_to_sign = current + "\n" + secret;
        var sign = ToBase64hmac(string_to_sign, secret);
        return webHookUrl + "&timestamp=" + current + "&sign=" + sign;
    }
}

public enum DingMsgType
{
    text,
    markdown,
    actionCard,
    feedCard,
    empty,
}
