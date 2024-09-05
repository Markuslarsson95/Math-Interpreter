using System.Globalization;
using System.Text.RegularExpressions;

namespace Math_Interpreter
{
    public class Program
    {
        public static void Main()
        {
            string expression = "1 + (36 / 2 * 4) / (3 * 3 + 3,5) - 2";
            // Ersätter alla kommatecken till punkt för att konverteringen längre ner ska fungera korrekt
            expression = expression.Replace(",", ".");
            string result = EvaluateExpression(expression).ToString();
            Console.WriteLine($"Epression: {expression}");
            Console.WriteLine($"Result: {result}");
            Console.ReadLine();
        }
        static double EvaluateExpression(string expression)
        {
            // Behöver först hantera paranteser, göra om paranteser till det uträknade talet och tar bort parantesen från "expression" strängen
            expression = HandleParentheses(expression);

            // Räknar först all multiplikation och division för att göra "expression" strängen enklare att räkna ut
            expression = HandleMultiplicationAndDivision(expression);

            // Efter det ta hand om addition och subtraktionen
            return HandleAdditionAndSubtraction(expression);
        }

        static string HandleParentheses(string expression)
        {
            // Pattern för att hitta vad som finns inuti paranteser
            var parenthesesPattern = new Regex(@"\(([^()]+)\)");

            // Så länge det finns en match från min pattern ska den räkna ut vad värdet blir inuti parantesen och ersätta det i "expression" strängen, exempel: "2 + (4 / 2)" blir "2 + 2"
            while (parenthesesPattern.IsMatch(expression))
            {
                expression = parenthesesPattern.Replace(expression, match =>
                {
                    // Tilldelar en "innerExpression" som är värdet innanför parantesen
                    string innerExpression = match.Groups[1].Value;

                    // Räkna ut vad resultatet blir innanför parantesen
                    double result = EvaluateExpression(innerExpression);

                    // Ersätter värdet inne i parantesen till det uträknade värdet, med hjälp av "CultureInfo.InvariantCulture" returneras stränged med "." istället för "," vid decimaler
                    return result.ToString(CultureInfo.InvariantCulture);
                });
            }

            return expression;
        }

        static string HandleMultiplicationAndDivision(string expression)
        {
            // Pattern för att hitta multiplikation (*) och division (/)
            var mulAndDivPattern = new Regex(@"(\d+(\.\d+)?) (\*|\/) (\d+(\.\d+)?)");

            /* Så länge det finns en match från min pattern ska den räkna ut vad multiplikationen och divisionen blir och sedan returnerar en sträng utan multiplikationer och divisioner,
             den returnerar alltså den uträknade multiplikationens och divisionens värde, exempel: "2 + 4 + 2 * 2 + 4 / 2" blir "2 + 4 + 4 + 2"*/
            while (mulAndDivPattern.IsMatch(expression))
            {
                expression = mulAndDivPattern.Replace(expression, match =>
                {
                    // Tilldelar talet till vänster om operationen
                    double leftNumber = double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);

                    // Tilldelar vilken operation det är
                    string operation = match.Groups[3].Value;

                    // Tilldelar talet till höger om operationen
                    double rightNumber = double.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                    double result = 0;

                    // Beroende på vilken operation det är uförs uträkningen med rätt funktion
                    if (operation == "*")
                        result = CalculateMultiplication(leftNumber, rightNumber);
                    else if (operation == "/")
                        result = CalculateDivision(leftNumber, rightNumber);

                    // Returnerar det nya talet som ersätter multiplikationen och divisionen i strängen, med hjälp av "CultureInfo.InvariantCulture" returneras stränged med "." istället för "," vid decimaler
                    return result.ToString(CultureInfo.InvariantCulture);
                });
            }

            return expression;
        }

        static double HandleAdditionAndSubtraction(string expression)
        {
            // Hämta nummren från strängen och lägg till i listan "numbers"
            var numbersInString = Regex.Split(expression, @"[\+\-]").ToList();
            List<double> numbers = [];
            foreach (var number in numbersInString)
            {
                numbers.Add(double.Parse(number, CultureInfo.InvariantCulture));
            }

            // Hämta operatörerna + och - och lägg till i listan "operators"
            var operatorsInString = Regex.Matches(expression, @"[\+\-]").ToList();
            List<char> operators = [];
            foreach (var operater in operatorsInString)
            {
                operators.Add(operater.Value[0]);
            }

            // Tilldelar först numret till resultatet
            double result = numbers[0];

            // Gå igenom all operatorer
            for (int i = 0; i < operators.Count; i++)
            {
                // Tilldela opertion
                char operation = operators[i];

                // Få näst kommande nummer
                double nextNumber = numbers[i + 1];

                // Bestäm vilket metod som ska anropas beroende på operation
                if (operation == '+')
                {
                    result = CalculateAddition(result, nextNumber);
                }
                else if (operation == '-')
                {
                    result = CalculateSubtraction(result, nextNumber);
                }
            }

            return result;
        }
        static double CalculateDivision(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Division by zero is not allowed.");
            }
            return a / b;
        }

        static double CalculateMultiplication(double a, double b)
        {
            return a * b;
        }

        static double CalculateAddition(double num1, double num2)
        {
            return num1 + num2;
        }

        static double CalculateSubtraction(double num1, double num2)
        {
            return num1 - num2;
        }
    }
}