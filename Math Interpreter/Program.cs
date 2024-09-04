using System.Globalization;
using System.Text.RegularExpressions;

internal class Program
{
	public static void Main()
	{
		//string expression = "1 + (36 / 2) / (3 * 3) - 2";
		string expression = "2,10 - 1 + 3 * 4 + 2 * 2,55";
		// Ersätter alla kommatecken till punkt för att konverteringen längre ner ska fungera korrekt
		//expression = expression.Replace(",", ".");
		string result = EvaluateExpression(expression);
		Console.WriteLine($"Epression: {expression}");
		Console.WriteLine($"Result: {result}");
		Console.ReadLine();
	}
	static string EvaluateExpression(string expression)
	{
		// Behöver först hantera multiplikation
		// Räknar först all multiplikation för att göra "expression" strängen enklare att räkna ut
		expression = HandleMultiplication(expression);

		// Efter det ta hand om addition och subtraktionen
		return HandleAdditionAndSubtraction(expression).ToString();
	}

	static string HandleMultiplication(string expression)
	{
		// Pattern för att hitta multiplikation (*)
		var multiplicationPattern = new Regex(@"(\d+(\.\d+)?) \* (\d+(\.\d+)?)");

		/* Så länge det finns en match från min pattern ska den räkna ut vad multiplikationen blir och sedan returnerar en sträng utan multiplikationer,
		 den returnerar alltså den uträknade multiplikationens värde, exempel: "2 + 4 + 2 * 2" blir "2 + 4 + 4"*/
		while (multiplicationPattern.IsMatch(expression))
		{
			expression = multiplicationPattern.Replace(expression, match =>
			{
				// Hämta nummren som ska multipliceras och anropar metoden som ger tillbaka resultatet
				var numbers = match.Groups.Cast<Group>()
					.Skip(1)
					.Where(g => !string.IsNullOrWhiteSpace(g.Value) && g.Value != "*")
					.Select(g => double.Parse(g.Value/*, CultureInfo.InvariantCulture)*/))
					.ToArray();

				double result = CalculateMultiplication(numbers[0], numbers[1]);

				// Returnerar det nya talet som ersätter multiplikationen i strängen
				return result.ToString();
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
			//numbers.Add(double.Parse(number/*, CultureInfo.InvariantCulture*/));
			double.TryParse(number, out double result1);
			numbers.Add(result1);
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