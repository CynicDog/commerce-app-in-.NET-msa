namespace ShoppingCartService.Event.impl;

public static class SqlQueries
{
    public const string WriteEventSql =
        @"insert into EventStore(Name, OccuredAt, Content) values (@Name, @OccuredAt, @Content)";

    public const string ReadEventSql =
        @"select * from EventStore where ID >= @Start and ID <= @End";
}