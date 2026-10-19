using ExpressionCalculator;
while (true)
{
    Console.WriteLine("Enter a mathematical expression:");
    string input = Console.ReadLine() ?? "";

    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("Input cannot be empty. Please try again.");
        continue;
    }

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    try
    {
        var output = MathParser.Evaluate(input);
        Console.WriteLine($"Result: {output}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}