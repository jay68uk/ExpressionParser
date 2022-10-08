using ExpressionParserApi.ApiModels;
using ExpressionParserApi.DtoModels;
using ExpressionParserLibrary;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/api/evaluate", [SwaggerOperation(Summary = "Expression evaluator",
        Description = "Evaluates an expression of non-negative integers and the four basic operators (+-*/)")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(400, "Bad Request")]
    [SwaggerResponse(500, "Internal server error")]
    async (ExpressionDtoModel expressionModel) =>
    {
        if (string.IsNullOrEmpty(expressionModel.Expression))
            return Results.BadRequest("Expression is missing or badly structured");

        try
        {
            var result = await Task.Run(() => ExpressionEvaluator.EvaluateExpression(expressionModel.Expression))
                .ConfigureAwait(false);

            var returnModel = new ExpressionApiModel { Evaluated = result, Expression = expressionModel.Expression };

            return Results.Ok(returnModel);
        }
        catch (ArgumentNullException ex)
        {
            return Results.BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return Results.StatusCode(500);
        }
    }).WithDisplayName("Expression Evaluator");

app.Run();