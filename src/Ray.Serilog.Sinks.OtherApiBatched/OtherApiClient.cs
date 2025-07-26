using System.Text;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.OtherApiBatched;

public class OtherApiClient : PushService
{
    private readonly Uri _apiUri;
    private string _json;
    private readonly string _placeholder;

    private readonly HttpClient _httpClient = new HttpClient();

    public OtherApiClient(string apiUrl, string json, string placeholder)
    {
        _json = json;
        _placeholder = placeholder;
        _apiUri = new Uri(apiUrl);
    }

    protected override string ClientName => "自定义";

    public override void BuildMsg()
    {
        base.BuildMsg();
        _json = _json.Replace(_placeholder, Msg.ToJsonStr());
    }

    protected override HttpResponseMessage DoSend()
    {
        var content = new StringContent(_json, Encoding.UTF8, "application/json");
        var response = this._httpClient.PostAsync(_apiUri, content).GetAwaiter().GetResult();
        return response;
    }
}
