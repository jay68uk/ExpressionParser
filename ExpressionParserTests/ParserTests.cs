using ExpressionParserLibrary;

namespace ExpressionParserTests;

public class ExpressionCalculatorTests
{
    [Theory]
    [InlineData("2+-1", "Invalid expression (Parameter 'parsed')")]
    [InlineData("2+(1-1)", "Invalid characters in expression (Parameter 'validExpression')")]
    public void Invalid_Expression_ReturnNull(string expression, string exception)
    {
        void Action()
        {
            ExpressionEvaluator.EvaluateExpression(expression);
        }

        var ex = Assert.Throws<ArgumentNullException>(Action);

        Assert.Equal(exception, ex.Message);
    }

    [Theory]
    [InlineData("2 +1", 3)]
    [InlineData("22+11", 33)]
    [InlineData("2- 1", 1)]
    [InlineData(" 2 * 2 ", 4)]
    [InlineData("4/2", 2)]
    [InlineData("2+4/2+3*2-5", 5)]
    [InlineData("20/10*5/4", 2.5)]
    public void Valid_Expression_ReturnArray(string expression, decimal expected)
    {
        var check = ExpressionEvaluator.EvaluateExpression(expression);

        Assert.Equal(expected, check);
    }
}