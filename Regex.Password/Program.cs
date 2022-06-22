using System;

namespace Regex.Password
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var continueLoop = true;
            while (continueLoop)
            {
                Console.WriteLine("Password:", Console.ForegroundColor = ConsoleColor.Yellow);
                var value = Console.ReadLine();
                var success = UseRegex(value);

                if (!success)
                    Console.WriteLine("Invalid password!\n", Console.ForegroundColor = ConsoleColor.Red);
                else
                    Console.WriteLine("Password is valid.\n", Console.ForegroundColor = ConsoleColor.Green);

                if (Equals(value.ToString(), "exit"))
                    continueLoop = false;
            }
        }

        private static bool UseRegex(string value)
        {
            // Are all digits 6 digits?
            var isSixNumber = value.Length == 6;

            // Are all digits made up of numbers?
            System.Text.RegularExpressions.Regex regex = new(DigitPattern());
            var isDigit = regex.IsMatch(value);

            if (isDigit && isSixNumber)
            {
                // Are all digits sequential in ascending order?
                regex = new(AscendingSequentialPattern(6));
                var isNotSequentialAsc = !regex.IsMatch(value);

                // Are all digits sequential in descending order?
                regex = new(DescendingSequentialPattern(6));
                var isNotSequentialDesc = !regex.IsMatch(value);

                // Are the first 4 digits sequential in ascending order?
                regex = new(AscendingSequentialPattern(4));
                var isNotFirstFourSequentialAsc = !regex.IsMatch(value[..4]);

                // Are the first 4 digits sequential in descending order?
                regex = new(DescendingSequentialPattern(4));
                var isNotFirstFourSequentialDesc = !regex.IsMatch(value[..4]);

                // Are the first 4 digits sequential in ascending order?
                regex = new(AscendingSequentialPattern(4));
                var isNotLastFourSequentialAsc = !regex.IsMatch(value[2..6]);

                // Are the first 4 digits sequential in descending order?
                regex = new(DescendingSequentialPattern(4));
                var isNotLastFourSequentialDesc = !regex.IsMatch(value[2..6]);

                // 4 digits repeating?
                regex = new(RepeatConsecutiveFourDigitPattern());
                var isNotRepeatFourDigits = !regex.IsMatch(value);

                if (isNotSequentialAsc && isNotSequentialDesc && isNotFirstFourSequentialAsc && isNotFirstFourSequentialDesc && isNotLastFourSequentialAsc && isNotLastFourSequentialDesc && isNotRepeatFourDigits)
                    return true;
            }
            return false;
        }

        //  "0123456789"
        private static string DigitPattern()
             => "^\\d*\\d$";

        // characterNumber = 4 => "123400" | "001234"
        private static string AscendingSequentialPattern(int characterNumber)
            => "^(?:0(?=1)|1(?=2)|2(?=3)|3(?=4)|4(?=5)|5(?=6)|6(?=7)|7(?=8)|8(?=9)|9(?=0)){" + (characterNumber - 1).ToString() + "}\\d$";

        // characterNumber = 4 => "432100" | "004321"
        private static string DescendingSequentialPattern(int characterNumber)
            => "^(?:0(?=9)|1(?=0)|2(?=1)|3(?=2)|4(?=3)|5(?=4)|6(?=5)|7(?=6)|8(?=7)|9(?=8)){" + (characterNumber - 1).ToString() + "}\\d$";

        // repeatDigit = 4 => "001100" | "000011"
        public static string RepeatDigitPattern(int repeatDigit)
        {
            var pattern = "(\\d)";
            for (int i = 0; i <= repeatDigit; i++)
            {
                pattern += "[\\d]*\\1";
            }
            return pattern;
        }

        // numberOfDigit = 3 => "010101" | "012012"
        public static string RepeatGroupDigitPattern(int numberOfDigit)
            => "^([0-9]{" + numberOfDigit + "})(?:[0-9])*\\1|[^0-9]+";

        //  "000011" | "110000"
        public static string RepeatConsecutiveFourDigitPattern()
           => "(0000|1111|2222|3333|4444|5555|6666|7777|8888|9999)";
    }
}
