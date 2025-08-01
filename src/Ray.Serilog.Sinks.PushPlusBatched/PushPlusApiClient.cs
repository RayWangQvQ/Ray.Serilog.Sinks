﻿using System.Text;
using Ray.Serilog.Sinks.Batched;

namespace Ray.Serilog.Sinks.PushPlusBatched;

public class PushPlusApiClient : PushService
{
    //http://www.pushplus.plus/doc/

    private const string Host = "http://www.pushplus.plus/send";

    private readonly Uri _apiUrl;
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _token;
    private readonly string? _topic;
    private readonly string _channel;
    private readonly string _webhook;

    public PushPlusApiClient(
        string token,
        string? topic = null,
        string channel = "",
        string webhook = ""
    )
    {
        _apiUrl = new Uri(Host);
        _token = token;
        _topic = topic;
        _channel = channel;
        _webhook = webhook;
    }

    protected override string ClientName => "PushPlus";

    private PushPlusChannelType ChannelType
    {
        get
        {
            var re = PushPlusChannelType.wechat;

            if (_channel.IsNullOrEmpty())
                return re;

            bool suc = Enum.TryParse(_channel, true, out PushPlusChannelType channel);
            if (suc)
                re = channel;

            return re;
        }
    }

    protected override string NewLineStr => "<br/>";

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
            token = _token,

            topic = _topic,
            channel = this.ChannelType.ToString(),
            webhook = _webhook,

            title,
            content = message,

            template = PushPlusMsgType.html.ToString(),
        }.ToJsonStr();

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_apiUrl, content);
        return response;
    }
}

public enum PushPlusMsgType
{
    html,
    json,
    markdown,
    cloudMonitor,
    jenkins,
    route,
}

public enum PushPlusChannelType
{
    wechat,
    webhook,
    cp,
    sms,
    mail,
}
