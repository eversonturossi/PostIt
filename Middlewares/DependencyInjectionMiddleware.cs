namespace PostIt.Middlewares;

public static partial class DependencyInjectionMiddleware
{
    public static void AddDependencyInjectionMiddleware(this IServiceCollection services)
    {
        services.AddTransient<ISQLiteConnectionFactory, SQLiteConnectionFactory>();
        services.AddTransient<ICheckDBService, CheckDBService>();
    }
}