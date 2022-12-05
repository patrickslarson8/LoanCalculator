using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class UI
    {

        public static string GetUserInputAsString(string prompt)
        {
            // Loop until input is considered valid
            string deNulledInput = "";
            while (true)
            {
                // Tell user what to input and get it
                PrintMessage(prompt);
                string? userInput = Console.ReadLine();

                // Handle null input
                if (string.IsNullOrWhiteSpace(userInput)) 
                {
                    PrintMessage("That input is invalid.");
                    continue;
                }
                else { deNulledInput = userInput; }

                // confirm input
                if (ConfirmUserInput(deNulledInput))
                {
                    return deNulledInput;
                }
            }
        }
        public static double GetUserInputAsDouble(string prompt)
        {
            // Loop until input is considered valid
            while (true)
            {
                double userInputDouble;

                // Tell user what to input and get it
                string userInputString = GetUserInputAsString(prompt);

                // Handle bad input
                if (!double.TryParse(userInputString, out userInputDouble))
                {
                    PrintMessage("That input is invalid.");
                    continue;
                }
                else { return userInputDouble; }
            }
        }
        public static bool ConfirmUserInput(string userInput)
        {
            while (true)
            {
                // Ask user to confirm their input
                Console.WriteLine($"You entered: [{userInput}] is that correct? [y/n]");
                string? confirmationInput = Console.ReadLine();

                // prevent the null condition and loop
                if (string.IsNullOrWhiteSpace(confirmationInput))
                {
                    Console.WriteLine("Please only press the 'y' or 'n' key followed by the enter key.");
                }
                else if (confirmationInput.ToLower().StartsWith('y'))
                {
                    return true;
                }
                else if (confirmationInput.ToLower().StartsWith('n'))
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Please only press the 'y' or 'n' key followed by the enter key.");
                }
            }
        }
        public static void PrintMessage(string message)
            { Console.WriteLine(message); }
        public static double GetFencedUserInput(List<double> validEntries, string prompt)
        {
            double userInput = 0;
            while (true)
            {
                userInput = GetUserInputAsDouble(prompt);
                foreach (double value in validEntries)
                {
                    if (userInput == value) { return value; }
                }
                PrintMessage("That input is not allowable for this field");
            }
        }
        public static double GetInputAsPercentage(string prompt)
        {
            double userInput = GetUserInputAsDouble(prompt);
            return userInput / 100;
        }
        public static void PrintDollars(string message, double money)
        {
            string prettyMoney = money.ToString("C", CultureInfo.CurrentCulture);
            PrintMessage(message + prettyMoney);
        }
        public static void PrintPercent(string message, double percent)
        {
            string prettyPercent = percent.ToString("P", CultureInfo.CurrentCulture);
            PrintMessage(message + prettyPercent);
        }
    }
}
