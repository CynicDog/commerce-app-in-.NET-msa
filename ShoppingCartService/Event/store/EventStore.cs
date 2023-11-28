using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartService.Event;

public interface IEventStore
{
    Task<IEnumerable<EventModel>> GetEvents(long firstEventSeq, long lastEventSeq);
    Task Raise(string eventName, object content); 
}