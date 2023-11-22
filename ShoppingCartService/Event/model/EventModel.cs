namespace ShoppingCartService.Event;

public record EventModel(
    long SequenceNumber, 
    DateTimeOffset OccuredAt, 
    string Name, 
    object Content);