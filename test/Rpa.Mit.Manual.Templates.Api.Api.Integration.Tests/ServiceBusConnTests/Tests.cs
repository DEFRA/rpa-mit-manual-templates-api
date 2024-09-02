using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

using Microsoft.Extensions.Configuration;

namespace Rpa.Mit.Manual.Templates.Api.Core.Integration.Tests;

public class ServiceBusConnTests : IAsyncLifetime
{
    private readonly ServiceBusClient client;
    public static IConfiguration _config => BuildConfig();
    public ServiceBusAdministrationClient _adminClient { get; }

    public ServiceBusConnTests()
    {
        var hostName = _config["PAYMENTHUB:CONNECTION"];
        client = new ServiceBusClient(hostName);
        _adminClient = new ServiceBusAdministrationClient(hostName);
    }

    [Fact]
    public async Task Can_Connect_And_RequestTopic_Exists()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        var topicName = _config["PAYMENTHUB:TOPIC"];

        var result = await _adminClient.TopicExistsAsync(topicName, cts.Token);

        Assert.True(result);
    }

    [Fact]
    public async Task Can_Connect_And_ResponseSubscription_Exists()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        var topicName = _config["PAYMENTHUB:RESPONSE_TOPIC"];
        var subscriptionName = _config["PAYMENTHUB:RESPONSE_SUBSCRIPTION"];
        var result = await _adminClient.SubscriptionExistsAsync(topicName, subscriptionName, cts.Token);

        Assert.True(result);
    }

    [Fact]
    public async Task Can_Connect_And_ResponseTopic_Exists()
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
        var topicName = _config["PAYMENTHUB:RESPONSE_TOPIC"];

        var result = await _adminClient.TopicExistsAsync(topicName, cts.Token);

        Assert.True(result);
    }

    public ServiceBusReceiver GetReceiver(string queue)
       => client.CreateReceiver(queue, options: new()
       {
           ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete,
           Identifier = $"Test-Receiver"
       });

    private static IConfiguration BuildConfig() => new ConfigurationBuilder()
                                                .AddUserSecrets<ServiceBusConnTests>()
                                                .Build();

    public Task InitializeAsync()
        => Task.CompletedTask;

    public async Task DisposeAsync()
        => await (client?.DisposeAsync() ?? ValueTask.CompletedTask);
}