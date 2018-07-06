using System;

namespace Golems
{
    class MainClass
    {
        // Length of the randomly generated control sequence
        public static int CodeLength = 4;
        // Allowed control spells
        public static string Codes = "FWEA";

        // Displays the help screen
        static void DisplayHelp()
        {
            Console.WriteLine("----- HELP -----");
            Console.WriteLine("The control spells are mapped to the following letters:");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("F");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - fire");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("W");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - water");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("E");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - earth");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("A");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" - air");
            Console.WriteLine("Type in any {0}-letter combination of the above letters", CodeLength);
            Console.WriteLine("to activate the corresponding control spells.\n");
            Console.Write("The eyes will light up red (an ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("R");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" will be displayed) for each time");
            Console.WriteLine("a control spell is activated in the correct position in the sequence.");
            Console.Write("The eyes will light up blue (a ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("B");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" will be displayed) for each time");
            Console.WriteLine("a control spell is contained in the golems' control sequence but is not");
            Console.WriteLine("in the correct position.\n");

            Console.Write("Press any key to continue...");
            Console.ReadKey();

            Console.Clear();
        }

        // Maps a set of keys to a set of console colors (F - red, W - cyan, D - green, A - gray)
        static ConsoleColor GetForegroundColor(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.F:
                    return ConsoleColor.Red;
                case ConsoleKey.W:
                    return ConsoleColor.Cyan;
                case ConsoleKey.E:
                    return ConsoleColor.DarkGreen;
                case ConsoleKey.A:
                    return ConsoleColor.DarkGray;
                default:
                    break;
            }
            return ConsoleColor.White;
        }

        // A specialized version of the Console.ReadLine function
        // Limits the user to entering certain characters
        // Does not allow the user to press enter if the sequence of
        // characters is not the correct length yet
        static string ReadSequence() {
            // Stores the character sequence currently entered
            string currentSequence = "";
            // Whether enter was pressed the last key press
            bool enterPressed = false;
            // Continue looping until the user presses the enter button
            while (!enterPressed)
            {
                // Read in a keystroke from the user, true parameter means to not
                // immediately output to the console
                ConsoleKey nextKey = Console.ReadKey(true).Key;

                // Determine what to do with the key entered
                switch (nextKey)
                {
                    case ConsoleKey.Backspace:
                        // Only backspace if there are characters left in the sequence
                        if (currentSequence.Length > 0)
                        {
                            // Clear the last character entered in the console
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            Console.Write(" ");
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                            // Remove the character from the string containing the currently entered sequence
                            currentSequence = currentSequence.Substring(0, currentSequence.Length - 1);
                        }
                        break;
                    case ConsoleKey.F:
                    case ConsoleKey.W:
                    case ConsoleKey.E:
                    case ConsoleKey.A:
                        // Only allow the user to enter F, W, E, or A while the sequence is shorter
                        // than the length of the predetermined control sequence
                        if (currentSequence.Length < CodeLength)
                        {
                            // Add the character to the string containing the sequence
                            currentSequence += nextKey.ToString();
                            // Write the character to the console using the corresponding color
                            Console.ForegroundColor = GetForegroundColor(nextKey);
                            Console.Write(nextKey.ToString());
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        break;
                    case ConsoleKey.Enter:
                        // Only allow the user to submit the sequence if it is the
                        // correct length
                        if (currentSequence.Length == CodeLength)
                        {
                            // Set the flag to break from the while loop
                            enterPressed = true;
                            // Write the enter keystroke to the console
                            Console.WriteLine();
                        }
                        break;
                    default:
                        // Don't do anything with other keystrokes
                        break;
                }
            }
            return currentSequence;
        }

        static void Main(string[] args)
        {
            Console.Clear();

            // Title/info screen
            Console.WriteLine("The golems are attacking the city of Ankh Morpork!");
            Console.WriteLine("You, the city's police force, need to stop the golems.");
            Console.WriteLine("The golems are controlled by control spells.");
            Console.WriteLine("You will need to determine the correct order of spells to disable them.\n");

            // Difficulty mode selection
            Console.Write("What difficulty mode would you like to play? (E - easy, M - medium, H - hard) ");
            string mode = Console.ReadLine().ToUpper();
            while (mode != "E" && mode != "M" && mode != "H")
            {
                Console.Write("Invalid option. What difficulty mode would you like to play? (E, M, H) ");
                mode = Console.ReadLine().ToUpper();
            }

            // Set the length of the control sequence based on the difficulty mode
            if (mode == "E")
                CodeLength = 3;
            if (mode == "M")
                CodeLength = 4;
            if (mode == "H")
                CodeLength = 5;

            // Ask the user whether they would like to view the help screen
            Console.Write("\nWould you like to view the help screen? (Y / N) ");
            string userChoice = Console.ReadLine().ToUpper();
            while (userChoice != "Y" && userChoice != "N")
            {
                Console.Write("Invalid option. Would you like to view the help screen? (Y / N) ");
                userChoice = Console.ReadLine().ToUpper();
            }

            Console.Clear();

            // Display the help screen if the user wants to
            if (userChoice == "Y")
                DisplayHelp();
            
            string code = "";
            Random random = new Random();
            // Randomly generate a sequence of characters as the control sequence
            for (int i = 0; i < CodeLength; i++)
            {
                code += Codes[random.Next(Codes.Length)];
            }
            
            Console.WriteLine("The control sequence for the golems contains {0} spells.", CodeLength);
            Console.WriteLine("You have ten tries to disable the golems.");

            int guess;
            for (guess = 0; guess < 10; guess++)
            {
                Console.Write("\nEnter your guess: ");
                // Read in the user's guess for the control sequence using the customized ReadLine method
                string userCode = ReadSequence();

                // Arrays that store how many of each type of control spells are in both sequences
                int[] secretCharCounts = new int[Codes.Length], userCharCounts = new int[Codes.Length];
                // Strings that will store the blinking sequence that the golems' eyes produce
                string responseR = "", responseB = "";
                // First determine how many characters are in the right place
                for (int j = 0; j < CodeLength; j++)
                {
                    // If the corresponding character in the user guess is the same
                    // as the one in the same position in the randomly generated
                    // sequence, the character is in the right place
                    if (userCode[j] == code[j])
                    {
                        // The golems' eyes blink red
                        responseR += "R";
                    }
                    else
                    {
                        // If the control spell didn't generate a red blink response,
                        // it could potentially be in the sequence, just not in the
                        // right position, so add 1 to the counter for the control spell
                        secretCharCounts[Codes.IndexOf(code[j])]++;
                        userCharCounts[Codes.IndexOf(userCode[j])]++;
                    }
                }
                // Next determine which characters are in the secret sequence
                // but are in the wrong position
                for (int j = 0; j < secretCharCounts.Length; j++)
                {
                    // The minimum number between the number of a certain control spells
                    // in both sequences is equal to the number of characters that are
                    // in the wrong position but are the correct control spell
                    // For example, if there are 3 W spells in the secret sequence
                    // and 1 W spell in the user guess, a single blue blink
                    // response will be produced, assuming the W spell is not
                    // in the same position as the 3 W spells (this possibility
                    // was removed in the previous loop by subtracting from the count
                    // arrays the number of spells that are in the same position
                    responseB += new string('B', Math.Min(secretCharCounts[j], userCharCounts[j]));
                }
                // If the response is the same number of R's as the length of the secret
                // sequence, the user guess is correct, so break from the guessing loop
                if (responseR == new String('R', CodeLength))
                    break;
                // If the guess didn't produce a single response from the golems, output so
                if (responseR + responseB == "")
                {
                    Console.WriteLine("Your guess was incorrect. The golems' eyes did not light up.");
                }
                // Otherwise just output how the golems' eyes blinked, with color coding
                else
                {
                    Console.Write("Your guess was incorrect. The golems' eyes lit up like so: ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(responseR);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(responseB);
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }

            // If the guess variable being greater than or equal to 10 is what
            // caused the program to break from the guessing loop, the user used
            // all 10 guesses and didn't guess correctly
            if (guess == 10)
            {
                Console.WriteLine("Oops! You ran out of guesses! The control sequence was");
                Console.WriteLine("{0}. Better luck next time!", code);
            }
            // Otherwise the user guessed correctly within 10 guesses
            else
            {
                Console.WriteLine("Congrats! You successfully disabled the golems!");
            }

            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
