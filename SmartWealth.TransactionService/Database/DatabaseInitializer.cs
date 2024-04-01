﻿namespace SmartWealth.TransactionService.Database;

public static class DatabaseInitializer
{
    public static void RebuildDatabase(IServiceScope scope)
    {
        DatabaseContext context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}