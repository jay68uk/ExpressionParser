namespace ExpressionParserApi.Startup;

public static class SwaggerConfiguration
{
    public static WebApplication ConfigureSwagger(this WebApplication app, bool developmentOnly)
    {
        if (!app.Environment.IsDevelopment() && developmentOnly) return app;
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}