namespace ShoppingCartService.Event;

public interface IEventStore
{
    IEnumerable<EventModel> GetEvents(long firstEventSeq, long lastEventSeq);
    void Raise(string eventName, object content); 
}

public class EventStore : IEventStore
{
    private static long currentSeq = 0;
    private static readonly IList<EventModel> Database = new List<EventModel>();

    public IEnumerable<EventModel> GetEvents(long firstEventSeq, long lastEventSeq)
        => Database
            .Where(e => e.SequenceNumber >= firstEventSeq && e.SequenceNumber <= lastEventSeq)
            .OrderBy(e => e.SequenceNumber);

    public void Raise(string eventName, object content)
    {
        // provides atomic (thread-safe) operations for variables shared among multiple threads
        var seqNum = Interlocked.Increment(ref currentSeq);
        
        Database.Add(new EventModel(seqNum, DateTimeOffset.Now, eventName, content));
    }
}