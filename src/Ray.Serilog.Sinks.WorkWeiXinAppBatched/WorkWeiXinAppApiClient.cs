using System.Text;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.WorkWeiXinAppBatched;

public class WorkWeiXinAppApiClient : PushService
{
    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new();
    private readonly string _corpId;
    private readonly string _agentId;
    private readonly string _secret;

    private readonly string _toUser;
    private readonly string _toParty;
    private readonly string _toTag;

    public WorkWeiXinAppApiClient(
        string corpid,
        string agentId,
        string secret,
        string toUser = "",
        string toParty = "",
        string toTag = ""
    )
    {
        _corpId = corpid;
        _agentId = agentId;
        _secret = secret;
        _toUser = toUser;
        _toParty = toParty;
        _toTag = toTag;
        _apiUrl = new Uri("https://qyapi.weixin.qq.com/cgi-bin/message/send");
    }

    protected override string ClientName => "WorkWeiXinApp";

    protected override string? NewLineStr => "\n";

    protected override async Task<HttpResponseMessage> DoSendAsync(
        string message,
        string title = ""
    )
    {
        // access_token
        var accessToken = await GetAccessTokenAsync(_corpId, _secret);
        var apiUrlWithToken = new Uri($"{_apiUrl}?access_token={accessToken}");

        var json = new
        {
            touser = _toUser,
            toparty = _toParty,
            totag = _toTag,
            agentid = _agentId,
            msgtype = "text",
            text = new { content = message },
        }.ToJsonStr();

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(apiUrlWithToken, content);
        return response;
    }

    private async Task<string> GetAccessTokenAsync(string corpId, string secret)
    {
        var token = "";

        try
        {
            var uri = new Uri(
                $"https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={corpId}&corpsecret={secret}"
            );
            var response = await _httpClient.GetAsync(uri);
            var content = await response.Content.ReadAsStringAsync();

            var re = content.JsonDeserialize<WorkWeiXinAppTokenResponse>();

            if (re.errcode == 0)
                return re.access_token;
        }
        catch (Exception)
        {
            //ignore
        }

        return token;
    }
}
