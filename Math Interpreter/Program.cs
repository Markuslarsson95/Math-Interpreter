using System.Text.RegularExpressions;

internal class Program
{
    private static void Main()
    {
        //string expression = "1 + (36 / 2) / (3 * 3) - 2";
        string expression = "20 + 20 - 5 - 5 + 5";
        string result = EvaluateExpression(expression);
        Console.WriteLine($"Epression: {expression}");
        Console.WriteLine($"Result: {result}");
        Console.ReadLine();        
    }
    public static string EvaluateExpression(string expression)
    {
        // Hämta nummren från strängen
        var numbers = Regex.Split(expression, @"[\+\-]")
            .Select(Convert.ToDouble)
            .ToList();

        // Hämta symboler från strängen
        var symbols = Regex.Matches(expression, @"[\+\-]")
            .Cast<Match>()
            .Select(m => m.Value[0])
            .ToList();

        // Tilldelar först numret till resultatet
        double result = numbers[0];

        // Gå igenom alla symboler
        for (int i = 0; i < symbols.Count; i++)
        {
            // Bestäm vilken operation som ska genomföras
            char operation = symbols[i];

            // Få näst kommande nummer
            double nextNumber = numbers[i + 1];

            if (operation == '+')
            {
                result = CalculateAddition(result, nextNumber);
            }
            else if (operation == '-')
            {
                result = CalculateSubtraction(result, nextNumber);
            }
        }

        return result.ToString();
    }

    public static double CalculateAddition(double num1, double num2)
    {
        return num1 + num2;
    }

    public static double CalculateSubtraction(double num1, double num2)
    {
        return num1 - num2;
    }
}