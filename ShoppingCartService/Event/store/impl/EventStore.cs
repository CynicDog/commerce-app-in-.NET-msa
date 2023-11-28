using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using EventStore.ClientAPI;

namespace ShoppingCartService.Event.impl;

public class EventStore : IEventStore
{
    private string connectionTemplate = 
        "tcp://admin:changeit@eventstore-service:1113";
    
    public async Task<IEnumerable<EventModel>> GetEvents(long firstEventSeq, long lastEventSeq)
    {
        using var connection =
            EventStoreConnection.Create(
                ConnectionSettings.Create().DisableTls().Build(),
                new Uri(connectionTemplate)
            );
        await connection.ConnectAsync();

        var found = await connection.ReadStreamEventsForwardAsync(
            "ShoppingCart",
            start: firstEventSeq,
            count: (int)(lastEventSeq - firstEventSeq),
            resolveLinkTos: false);

        return found.Events.Select(e =>
            new
            {
                Content = Encoding.UTF8.GetString(e.Event.Data),
                Metadata = JsonSerializer.Deserialize<EventModelMetadata>(Encoding.UTF8.GetString(e.Event.Metadata),
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!
            }).Select((e, i) => 
            new EventModel(
                i + firstEventSeq,
                e.Metadata.OccurredAt,
                e.Metadata.EventName,
                e.Content));
    }

    public async Task Raise(string eventName, object content)
    {
        using var connection =
            EventStoreConnection.Create(
                ConnectionSettings.Create().DisableTls().Build(),
                new Uri(connectionTemplate)
            );
        await connection.ConnectAsync();

        var response = await connection.AppendToStreamAsync(
            "ShoppingCart",
            ExpectedVersion.Any,
            new EventData(
                Guid.NewGuid(),
                "ShoppingCartEvent",
                isJson: true,
                data: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(content)),
                metadata: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(new EventModelMetadata(
                    DateTimeOffset.UtcNow,
                    eventName
                ))))
        );
    }
}