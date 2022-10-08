using System.Text.RegularExpressions;
using Throw;

namespace ExpressionParserLibrary;

public static class ExpressionEvaluator
{
    public static decimal? EvaluateExpression(string expression)
    {
        decimal calculated = new(0);

        var validExpression = CleanCheckExpression(expression);
        validExpression.ThrowIfNull("Invalid characters in expression");

        var parsed = ParseExpression(validExpression);
        parsed.ThrowIfNull("Invalid expression");

        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var i = 0; i < parsed.Count; i++) calculated = decimal.Add(calculated, parsed[i]);

        return calculated;
    }

    private static IList<decimal>? ParseExpression(string expression)
    {
        const string operators = @"+-*/";
        var result = new List<decimal>();
        var position = 0;
        var anchor = 0;
        bool running;
        var isSubtraction = false;

        do
        {
            // keep going until the first operator or end of string
            if (operators.Contains(expression[position]) || position == expression.Length - 1)
            {
                //check there aren't 2 operators together
                if (position != expression.Length - 1 && operators.Contains(expression[position + 1]))
                    return null;

                //if at end of string then add 1 to the length to capture whole number
                var eos = position == expression.Length - 1 ? 1 : 0;
                var isConverted = decimal.TryParse(expression.AsSpan(anchor, position - anchor + eos),
                    out var numberResult);

                isConverted.Throw("Invalid conversion").IfFalse();


                result.Add(isSubtraction ? decimal.Negate(numberResult) : numberResult);

                //if operator is - then next value needs to be negative
                isSubtraction = expression[position] == '-';

                anchor = position + 1;
            }

            running = position != expression.Length - 1;
            position++;
        } while (running);


        return result;
    }

    private static string? CleanCheckExpression(string expression)
    {
        //remove all white space
        var clean = Regex.Replace(expression, @"\s+", "");

        //regex checks only integers and +-*/ operators are in the string
        return Regex.IsMatch(clean, @"^([0-9-*+/])*$") ? clean : null;
    }
}