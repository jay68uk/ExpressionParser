using ExpressionParserApi.Startup;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.ClearProviders();

var logger = LoggingConfiguration.ConfigureLogging();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

app.Logger.LogInformation("Configure Swagger");
app.ConfigureSwagger(false);

app.UseHttpsRedirection();

app.Logger.LogInformation("Configure endpoints");
app.MapExpressionEndpoints();

app.Run();