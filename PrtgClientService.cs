using PrtgAPI;

public interface IPrtgClientService
{
    PrtgClient GetPrtgClient();
}

public class PrtgClientService : IPrtgClientService
{
    private readonly string _baseUrl;
    private readonly string _username;
    private readonly string _password;

    public PrtgClientService(string baseUrl, string username, string password)
    {
        _baseUrl = baseUrl;
        _username = username;
        _password = password;
    }

    public PrtgClient GetPrtgClient()
    {
        var client = new PrtgClient(_baseUrl, _username, _password, ignoreSSL: true);
        client.RetryDelay = 3;
        return client;
    }
}