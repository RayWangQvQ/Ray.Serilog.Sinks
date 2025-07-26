using System.Net;
using System.Text;
using Ray.Serilog.Sinks.Batched;
using Serilog.Debugging;

namespace Ray.Serilog.Sinks.TelegramBatched;

public class TelegramApiClient : PushService
{
    //https://core.telegram.org/bots/api#available-methods

    private readonly string _chatId;
    private readonly string _proxy;
    private const string TelegramBotApiUrl = "https://api.telegram.org/bot";

    /// <summary>
    /// The API URL.
    /// </summary>
    private readonly Uri _apiUrl;

    /// <summary>
    /// The HTTP client.
    /// </summary>
    private readonly HttpClient _httpClient = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="TelegramApiClient"/> class.
    /// </summary>
    /// <param name="botToken">The Telegram bot token.</param>
    /// <param name="timeoutSeconds">The timeout seconds.</param>
    /// <exception cref="ArgumentException">Thrown if the bot token is null or empty.</exception>
    public TelegramApiClient(
        string botToken,
        string chatId,
        string proxy = "",
        int timeoutSeconds = 10
    )
    {
        if (string.IsNullOrWhiteSpace(botToken))
        {
            SelfLog.WriteLine("The bot token mustn't be empty.");
            throw new ArgumentException("The bot token mustn't be empty.", nameof(botToken));
        }

        _chatId = chatId;
        _proxy = proxy;

        _apiUrl = new Uri($"{TelegramBotApiUrl}{botToken}/sendMessage");

        if (!proxy.IsNullOrEmpty())
        {
            var webProxy = GetWebProxy(proxy);
            var proxyHttpClientHandler = new HttpClientHandler
            {
                Proxy = webProxy,
                UseProxy = true,
            };
            _httpClient = new HttpClient(proxyHttpClientHandler);
        }

        _httpClient.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    }

    protected override string ClientName => "Telegram机器人";

    protected override async Task<HttpResponseMessage> DoSendAsync(string message, string title = "")
    {
        SelfLog.WriteLine($"使用代理：{!_proxy.IsNullOrEmpty()}");

        var json = new
        {
            chat_id = _chatId,
            text = message,
            parse_mode = TeleMsgType.HTML.ToString(),
            disable_web_page_preview = true,
        }.ToJsonStr();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(this._apiUrl, content);
        return response;
    }

    protected override string BuildMsg(string message, string title = "")
    {
        var re = $"<b>{title}</b>{Environment.NewLine}{Environment.NewLine}{message}";

        return base.BuildMsg(re, title);
    }

    private WebProxy GetWebProxy(string proxyAddress)
    {
        //todo:抽象到公共方法库
        WebProxy webProxy;

        //user:password@host:port http proxy only .Tested with tinyproxy-1.11.0-rc1
        if (proxyAddress.Contains("@"))
        {
            string userPass = proxyAddress.Split("@")[0];
            string address = proxyAddress.Split("@")[1];

            string proxyUser = userPass.Split(":")[0];
            string proxyPass = userPass.Split(":")[1];

            var credentials = new NetworkCredential(proxyUser, proxyPass);

            webProxy = new WebProxy(address, true, null, credentials);
        }
        else
        {
            webProxy = new WebProxy(proxyAddress, true);
        }

        return webProxy;
    }
}

public enum TeleMsgType
{
    MarkdownV2,
    HTML,
    Markdown,
}
