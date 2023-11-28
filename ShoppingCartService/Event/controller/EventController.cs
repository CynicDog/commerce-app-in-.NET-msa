using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartService.Event;

[Route("/event")]
public class EventController
{
    private readonly IEventStore eventStore;

    public EventController(IEventStore eventStore)
    {
        this.eventStore = eventStore;
    }

    // TODO:
    //      1. Deploy EventStoreDB on Kubernetes (Done)
    //      2. Compose the operation of multiple manifests into a singly managed bash file
    
    // http 'http://localhost:5000/event?start=0&end=10'
    [HttpGet("")]
    public async Task<EventModel[]> Get([FromQuery] long start, [FromQuery] long end = long.MaxValue)
        => (await this.eventStore.GetEvents(start, end)).ToArray(); 
}