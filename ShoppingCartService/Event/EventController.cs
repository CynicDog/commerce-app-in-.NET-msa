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

    [HttpGet("")]
    public EventModel[] Get([FromQuery] long start, [FromQuery] long end = long.MaxValue)
        => this.eventStore.GetEvents(start, end).ToArray(); 
}