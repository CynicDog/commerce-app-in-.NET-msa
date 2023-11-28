using System;

namespace ShoppingCartService.Event
{
    public record EventModel(long SequenceNumber, DateTimeOffset OccurredAt, string Name, object Content)
    {
        // Mismatch exists between `Content` property with the type of object and `Content` column with the type of string     
        // Used is parameterless default constructor for Dapper to resolve the mismatch    
        public EventModel() : this(0, DateTimeOffset.MinValue, "", null)
        {
            
        }
    }
}