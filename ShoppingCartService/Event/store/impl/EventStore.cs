using System.Data.SqlClient;
using System.Text.Json;
using Dapper;

namespace ShoppingCartService.Event.impl;

public class EventStore : IEventStore
{
    private string connectionTemplate = 
        @"Data Source=localhost;Initial Catalog=ShoppingCart;User Id=sa;Password=yourStrongPassword!";
    
    public async Task<IEnumerable<EventModel>> GetEvents(long firstEventSeq, long lastEventSeq)
    {
        await using var connection = new SqlConnection(this.connectionTemplate);

        return await connection.QueryAsync<EventModel>(
            SqlQueries.ReadEventSql,
            new
            {
                Start = firstEventSeq,
                End = lastEventSeq
            }
        );
    }

    public async Task Raise(string eventName, object content)
    {
        await using var connection = new SqlConnection(this.connectionTemplate);

        await connection.ExecuteAsync(
            SqlQueries.WriteEventSql,
            new
            {
                Name = eventName,
                OccuredAt = DateTimeOffset.Now,
                Content = JsonSerializer.Serialize(content)
            }
        );
    }
}