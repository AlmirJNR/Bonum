namespace Bonum.Shared.Configs;

public class RabbitMqConfig
{
    public string Host { get; private init; }
    public ushort Port { get; private init; }
    public string VirtualHost { get; private init; }
    public string Username { get; private init; }
    public string Password { get; private init; }

    public RabbitMqConfig(string host, ushort port, string virtualHost, string username, string password)
    {
        Host = host;
        Port = port;
        VirtualHost = virtualHost;
        Username = username;
        Password = password;
    }
}