using System;
using System.IO;
using System.Collections.Generic;

namespace Matchmaker
{
    class Program
    {
        // Returns the length of the longest string in the given array
        static int MaxLength(string[] names)
        {
            int max = 0;
            // Compare the length of each name to the running maximum to find the maximum length
            foreach (string name in names)
                max = Math.Max(max, name.Length);
            return max;
        }

        // Splits a string into its component parts separated by sequences of spaces
        // Also returns whether the names were edited into starting with a capital
        static string[] SplitNames(string names, ref bool namesEdited)
        {
            // List of names
            List<string> nameList = new List<string>();
            // While the string is not empty
            while (names != null && names != "")
            {
                // Stores a series of non-space characters
                string nextName = "";
                // Remove any whitespace at the beginning of the string
                while (names.Length > 0 && names[0] == ' ')
                    names = names.Substring(1);
                // Add non-space characters to nextName until a whitespace character is reached
                while (names.Length > 0 && names[0] != ' ')
                {
                    nextName += names[0].ToString();
                    names = names.Substring(1);
                }
                // If the next name isn't blank, add it to the list of names
                if (nextName != "")
                    nameList.Add(nextName);
            }
            // Edit the names in the list such that they start with a capital and are followed by lowercase letters
            for (int i = 0; i < nameList.Count; i++)
            {
                string edited = nameList[i].Substring(0, 1).ToUpper() + nameList[i].Substring(1).ToLower();
                namesEdited = edited != nameList[i] ? true : namesEdited;
                nameList[i] = edited;
            }
            return nameList.ToArray();
        }

        // Checks if the list of names are invalid in any way
        static int IsValid(string[] names, string[] names2)
        {
            // Check each name in the list
            for (int i = 0; i < names.Length; i++)
            {
                // If a name that is in the first array cannot be found in the second array, or vice versa,
                // the name array is invalid
                if (Array.IndexOf(names, names2[i]) == -1 || Array.IndexOf(names2, names[i]) == -1)
                    return 1;
                // If more than one of the same name exists in either array, it is invalid
                if (Array.IndexOf(names, names[i]) != i || Array.IndexOf(names2, names2[i]) != i)
                    return 2;
            }
            return 0;
        }

        // Outputs whether names at each index in the two name arrays are consistent in terms of pairing
        static bool[] ConsistencyList(string[] names, string[] names2)
        {
            // Stores whether the names at each index are consistent
            bool[] inconsistent = new bool[names.Length];
            // Check each pair of names in the list
            for (int i = 0; i < names.Length; i++)
            {
                // Find the index of the name in the first list in the second array
                // Then take the name with the corresponding index in the first array
                // Compare this name with the name in the second array with the same index
                // as the original name
                // If these names are not the same, the pairing is not consistent
                // The pairing is also not consistent if a name is paired with itself
                // name1 name2 name3 name4
                // name4 name3 name2 name1
                // In this case, name1 has an index of 0 in the first array and 3 in the second array
                // Looking at the names above and below each of these, they are both name4, so the pairing is consistent
                if (names[Array.IndexOf(names2, names[i])] != names2[i] || names[i] == names2[i])
                    inconsistent[i] = true;
            }
            return inconsistent;
        }

        static void Main(string[] args)
        {
            // Help screen
            Console.Clear();
            Console.WriteLine(new string(' ', 20) + "MATCHMAKER");
            Console.WriteLine(new string(' ', 10) + "a name pair consistency checker");
            Console.WriteLine(new string('-', 52));
            Console.WriteLine("Matchmaker is a program that helps you check whether");
            Console.WriteLine("names are paired in a consistent manner. Essentially,");
            Console.WriteLine("the program checks whether the names in two lists are");
            Console.WriteLine("always partnered to each other. A name can not be");
            Console.WriteLine("partnered to itself. Consistent pairings will be shown");
            Console.WriteLine("in green, while inconsistent pairings will be shown");
            Console.WriteLine("in red. You can input the lists of names from either");
            Console.WriteLine("the console or from a file.\n");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.Clear();

            // Loop until the user wants to stop using matchmaker
            bool continueChecking = true;
            do
            {
                // Ask the user whether they would like to read the names in from the console or from a file
                Console.Write("Where would you like to read in the list of names from?\n(F - file, C - console) ");
                string readMethod = Console.ReadLine().ToUpper();
                while (readMethod != "F" && readMethod != "C")
                {
                    Console.Write("You didn't enter a valid option.\n(F - file, C - console) ");
                    readMethod = Console.ReadLine().ToUpper();
                }
                Console.Clear();

                string nameString = "", nameString2 = "";
                int numNames = -1;
                // Read in from a file or from the console depending on the user's choice
                if (readMethod == "F")
                {
                    // Ask the user for the file location
                    Console.WriteLine("Where is the name file located? (e.g., C:/Users/Someone/names.txt)");
                    string fileLocation = Console.ReadLine().Trim();
                    while (!File.Exists(fileLocation))
                    {
                        Console.WriteLine("File does not exist.\nWhere is the name file located? (e.g., C:/Users/Someone/names.txt)");
                        fileLocation = Console.ReadLine().Trim();
                    }
                    // Open the file and read in the list of names from it
                    StreamReader reader = new StreamReader(new FileStream(fileLocation, FileMode.Open));
                    int.TryParse(reader.ReadLine(), out numNames);
                    nameString = reader.ReadLine();
                    nameString2 = reader.ReadLine();
                }
                else
                {
                    // Prompt the user for both lists of names using the console
                    Console.WriteLine("Enter the first list of names:");
                    nameString = Console.ReadLine();
                    Console.WriteLine("Enter the second list of names:");
                    nameString2 = Console.ReadLine();
                }

                bool namesEdited = false;
                // Parse the strings containing multiple names into arrays containing the lists of names
                string[] names = SplitNames(nameString, ref namesEdited), names2 = SplitNames(nameString2, ref namesEdited);

                // If any part of the name file was not able to be parsed correctly, the file is invalid
                if (readMethod == "F" && (numNames != names.Length || numNames != names2.Length))
                {
                    Console.WriteLine("Error: The name file has an invalid format.");
                }
                // If the first name list is not the same size as the second, the lists are invalid
                else if (names.Length != names2.Length)
                {
                    Console.WriteLine("Error: The number of names in the lists are not the same");
                }
                // If the validation function returns 1, one or more names in one list cannot be found in the other
                else if (IsValid(names, names2) == 1)
                {
                    Console.WriteLine("Error: Some names in one of the lists are not in the other list.");
                }
                // If the validation function returns 2, there is more than one occurrence of a name in a list
                else if (IsValid(names, names2) == 2)
                {
                    Console.WriteLine("Error: The names in one or both lists are not unique.");
                }
                // The name lists are valid and can be checked for consistency
                else
                {
                    // Find the length of the longest name in the list, used to align the table
                    int nameLengthMax = MaxLength(names);
                    // Find which pairings in the name lists are consistent
                    bool[] inconsistent = ConsistencyList(names, names2);
                    // Warn the user if the names have been edited to start with a capital and end with lowercase letters
                    if (namesEdited)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Warning: Some of the names have been edited so that the first");
                        Console.WriteLine("letter of the name is capital and the rest are lowercase.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    // Loop through the consistency list
                    for (int i = 0; i < inconsistent.Length; i++)
                    {
                        // Consistent pairing is indicated by green text, inconsistent by red
                        Console.ForegroundColor = ConsoleColor.Green;
                        if (inconsistent[i])
                            Console.ForegroundColor = ConsoleColor.Red;
                        // Output the pairs using the color coding
                        Console.WriteLine("{0,-" + nameLengthMax + "}   {1,-" + nameLengthMax + "}", names[i], names2[i]);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }

                // Ask the user whether they want to continue using Matchmaker
                Console.Write("Would you like to continue using Matchmaker? (Y / N) ");
                string userChoice = Console.ReadLine().ToUpper();
                while (userChoice != "Y" && userChoice != "N")
                {
                    Console.Write("Invalid option. Would you like to continue using Matchmaker? (Y / N) ");
                    userChoice = Console.ReadLine().ToUpper();   
                }
                // Continue running or exit based on the user's choice
                continueChecking = userChoice == "Y";
                Console.Clear();
            } while (continueChecking);
        }
    }
}
