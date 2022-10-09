using ExpressionParserApi.ApiModels;
using ExpressionParserApi.DtoModels;
using ExpressionParserLibrary;
using Swashbuckle.AspNetCore.Annotations;

namespace ExpressionParserApi.Startup;

internal static class MapEndpoints
{
    internal static void MapExpressionEndpoints(this WebApplication app)
    {
        _ = app.MapPost("/api/evaluate", [SwaggerOperation(Summary = "Expression evaluator",
                Description = "Evaluates an expression of non-negative integers and the four basic operators (+-*/)")]
            [SwaggerResponse(200, "Success")]
            [SwaggerResponse(400, "Bad Request")]
            [SwaggerResponse(500, "Internal server error")]
            async (ILoggerFactory loggerFactory, ExpressionDtoModel expressionModel) =>
            {
                var logger = loggerFactory.CreateLogger("Expression Evaluator Endpoint");

                if (string.IsNullOrEmpty(expressionModel.Expression))
                {
                    logger.LogError("Json not in correct format or expression property is empty.");
                    return Results.BadRequest("Expression is missing or badly structured");
                }

                var expression = expressionModel.Expression;

                try
                {
                    logger.LogInformation("Start expression evaluation of {Expression}", expression);
                    var result = await Task
                        .Run(() => ExpressionEvaluator.EvaluateExpression(expression))
                        .ConfigureAwait(false);

                    var returnModel = new ExpressionApiModel
                        { Evaluated = result, Expression = expression };

                    logger.LogInformation("Finished expression evaluation of {Expression} completed successfully",
                        expression);
                    return Results.Ok(returnModel);
                }
                catch (ArgumentNullException ex)
                {
                    logger.LogError(ex, "Problem with expression: {Expression}", expression);
                    return Results.BadRequest(ex.Message);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing request: {Expression}", expression);
                    return Results.StatusCode(500);
                }
            }).WithDisplayName("Expression Evaluator");
    }
}