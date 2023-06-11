namespace AtmMachine
{
    public class Program
    {
        private static bool stopApp = false;
        private static int amount;

        // possibleResults is a general parameter where it keeps possible banknote combinations.
        private static List<Dictionary<string, int>> possibleResults = new List<Dictionary<string, int>>();

        //Main method is the method from which the application starts.
        private static void Main(string[] args)
        {
            while (!stopApp) // It stops the program when the user gives the exit signal.
            {
                GetPossibleBanknoteCombinations();
            }
        }

        // GetPossibleBanknoteCombinations method is main method where
        // it takes input from the user and print the result on the screen after the calculations.
        private static void GetPossibleBanknoteCombinations()
        {
            Console.WriteLine("Enter \"exit\" to close the program!");
            Console.WriteLine("Enter Amount: ");

            bool isSuccess = ValidateInput(Console.ReadLine(), out amount);
            if (!isSuccess || amount == 0 || amount % 10 > 0)
            {
                if (!stopApp)
                {
                    string errorMessage = !isSuccess
                    ? "You can only enter integers."
                    : "Payments can only be made for 10 euros and its multiples.";

                    Console.WriteLine(errorMessage);
                }
                return; // it completes this flow and move on to the new while loop.
            }

            CalculatePossibleBanknoteCombinations();
            PrintPossibleBanknoteCombinations();
            Reset();
        }

        //It checks whether the input has passed the required validations.
        private static bool ValidateInput(string? input, out int amount)
        {
            if (input == "exit" || input == "\"exit\"")
            {
                stopApp = true;
                amount = 0;
                return false;
            }

            return int.TryParse(input, out amount);
        }

        //Method by which possible banknote combinations are calculated.
        private static void CalculatePossibleBanknoteCombinations()
        {

            int ahundredCount = amount / 100;
            for (int i = ahundredCount; 0 <= i; i--)
            {
                int tempAmount = amount - (100 * i);

                int afiftyCount = tempAmount / 50;
                for (int j = afiftyCount; 0 <= j; j--)
                {
                    int remaining = tempAmount - (50 * j);
                    var _dic = CreateDic(i, j, remaining / 10);
                    possibleResults.Add(_dic);
                }
            }
        }

        //The dictionary is created with the given parameters.
        private static Dictionary<string, int> CreateDic(int countOfaHundred, int countOffifty, int countOften)
        {
            return new Dictionary<string, int>()
            {
                { "100",countOfaHundred },
                { "50",countOffifty },
                { "10",countOften}
            };
        }

        //As a result of the whole process, the result is printed on the console in a specific design.
        private static void PrintPossibleBanknoteCombinations()
        {
            Console.WriteLine("Result:");

            foreach (var result in possibleResults)
            {
                var output = String.Empty;
                if (result["100"] != 0)
                    output += "100 x " + result["100"];

                if (result["50"] != 0)
                {
                    if (output != string.Empty)
                        output += " + ";

                    output += "50 x " + result["50"];
                }


                if (result["10"] != 0)
                {
                    if (output != string.Empty)
                        output += " + ";

                    output += "10 x " + result["10"];
                }

                Console.WriteLine(output);
            }
        }

        //The program can be used more than once. The dictionary(possibleResults) needs to be reset.
        private static void Reset()
        {
            possibleResults = new List<Dictionary<string, int>>();
            Console.WriteLine("--------------");
        }
    }
}