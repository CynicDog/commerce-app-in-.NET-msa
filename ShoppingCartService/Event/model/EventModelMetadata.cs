using System;

namespace ShoppingCartService.Event;

public record EventModelMetadata(DateTimeOffset OccurredAt, string EventName);