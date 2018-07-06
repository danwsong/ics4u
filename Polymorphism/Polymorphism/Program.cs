using System;
using System.Collections.Generic;
using System.Linq;

namespace Polymorphism
{
    class MainClass
    {
        static int startYear = 2017;
        static string[] months = { 
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December" 
        };
        static string[] names = {
            "Emma", "Olivia", "Ava", "Sophia", "Isabella", "Mia", "Charlotte", "Abigail", "Emily", "Harper",
            "Noah", "Liam", "William", "Mason", "James", "Benjamin", "Jacob", "Michael", "Elijah", "Ethan"
        };
        // The number of months that have passed since January 2017
        static int date = 0;
        static List<Citizen> citizens = new List<Citizen>();
        static bool continuePlaying = true;
        static Random random = new Random();

        // Output the introductory help screen to the console
        static void ShowIntroduction()
        {
            Console.WriteLine("Welcome to Polymorphopolis\n");
            Console.WriteLine("You are a new citizen of the city, and your occupation");
            Console.WriteLine("is collecting taxes from each of the city's citizens.");
            Console.WriteLine("The city collects taxes from its citizens every month,");
            Console.WriteLine("which you will be responsible for doing. Additionally,");
            Console.WriteLine("every month, some people may move away, while others");
            Console.WriteLine("may move in. Your goal is to collect as much tax as");
            Console.WriteLine("possible, while keeping the citizens happy, so that they");
            Console.WriteLine("don't move away.\n");
            Console.Write("Press any key to start...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // Create a citizen that is one of the seven species, chosen randomly, returns the name of the newly created citizen
        static string CreateCitizen()
        {
            // Maximum number of citizens allows in city is 12
            if (citizens.Count == 12)
                return "";
            // Randomly chosen citizen type
            int citizenType = random.Next(7);
            // Don't allow repeat names by filtering out names that are already in use
            string[] potentialNames = names.Where(nameCand => !citizens.Select(citizen => citizen.name).Contains(nameCand)).ToArray();
            string name = potentialNames[random.Next(potentialNames.Length)];
            // Choose a citizen species to create based on citizenType
            switch (citizenType)
            {
                case 0:
                    citizens.Add(new Human(name));
                    break;
                case 1:
                    citizens.Add(new Dwarf(name));
                    break;
                case 2:
                    citizens.Add(new Nobbs(name));
                    break;
                case 3:
                    citizens.Add(new Troll(name));
                    break;
                case 4:
                    citizens.Add(new Vampire(name));
                    break;
                case 5:
                    citizens.Add(new Werewolf(name));
                    break;
                case 6:
                    citizens.Add(new Zombie(name));
                    break;
            }
            // Add on the species of the citizen to the name
            name += " the " + citizens[citizens.Count - 1].GetType().ToString().Split('.').Last();
            return name;
        }

        // Create a new city with 8 citizens
        static void InitializeCitizens()
        {
            for (int i = 0; i < 8; i++)
            {
                CreateCitizen();
            }
        }

        // Remove the citizen with the given index in the citizens list, returns the name of the citizen
        static string RemoveCitizen(int indexToRemove)
        {
            string name;
            // Name of the citizen to be removed plus the species of the citizen
            name = citizens[indexToRemove].name + " the " + citizens[indexToRemove].GetType().ToString().Split('.').Last();
            // Remove the citizen
            citizens.RemoveAt(indexToRemove);
            return name;
        }

        // Add new citizens or remove citizens from the city, returns a list of citizens that were added or removed
        static void MonthlyPopulation(ref List<string> added, ref List<string> removed)
        {
            int createRand = random.Next(16);
            string temp;
            // 1/4 chance that a new citizens moves into the city
            if (createRand < 4)
                if ((temp = CreateCitizen()) != "")
                    added.Add(temp);
            // 1/16 chance that another citizen moves into the city
            if (createRand < 1)
                if ((temp = CreateCitizen()) != "")
                    added.Add(temp);
            // Remove citizens that have a happiness level of 0 or less
            for (int i = 0; i < citizens.Count; i++)
                if ((int)citizens[i].happiness <= 0)
                    removed.Add(RemoveCitizen(i));
        }

        // Creates a prompt for the user that allows them to collect taxes, returns whether the user met the monthly tax goal
        static bool CollectTaxes()
        {
            // Generate a random tax goal, between $95 and $105
            int monthlyTaxGoal = (int)(95 + random.NextDouble() * 10);
            // Tax goal is proportionate to the number of citizens
            monthlyTaxGoal *= citizens.Count;
            // Tax collecting prompt for each citizen
            for (int i = 0; i < citizens.Count; i++)
            {
                string name = citizens[i].name + " the " + citizens[i].GetType().ToString().Split('.').Last(); 
                Console.WriteLine(months[date % 12] + " " + (date / 12 + startYear) + "\n");
                Console.WriteLine("Your remaining tax goal is ${0} and you have {1} more citizen{2} to collect taxes from.\n", monthlyTaxGoal, citizens.Count - i, citizens.Count - i != 1 ? "s" : "");
                Console.WriteLine("You are currently collecting taxes from " + name + ".");
                Console.WriteLine(name + " has a happiness level of " + (int)citizens[i].happiness + " and has " + citizens[i].GetMoney(true) + ".\n");
                Console.Write("How much would you like to tax " + name + "? $");
                int tax;
                while (!int.TryParse(Console.ReadLine(), out tax) || tax < 0)
                    Console.Write("Invalid amount. How much would you like to tax " + name + "? $");
                // Only allow the user to collect as much tax as the citizen has, or as much is remaining in the monthly tax goal
                tax = (int)Math.Min(Math.Min(tax, monthlyTaxGoal), Math.Round(citizens[i].money));
                // Call the tax paying function on the citizen
                citizens[i].PayTaxes(tax);
                // Decrease the monthly tax goal
                monthlyTaxGoal -= tax;
                Console.Write("\nPress any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
                // If the monthly tax goal has been reached, break from the prompt loop
                if (monthlyTaxGoal <= 0)
                    break;
            }
            // Return whether the monthly tax goal was reached
            return monthlyTaxGoal <= 0;
        }

        static string[] speciesList = { "Human", "Vampire", "Werewolf", "Zombie", "Nobbs", "Troll", "Dwarf" };

        // Show a table of citizens with headings by species
        static void ShowCitizens()
        {
            Console.WriteLine(months[date % 12] + " " + (date / 12 + startYear) + "\n");
            // Top of table
            Console.WriteLine("┏" + new string('━', 10) + "┳" + new string('━', 11) + "┓");
            // Flag that is true once the first line of the table has been outputted
            bool isFirst = false;
            // Display citizens by species
            for (int j = 0; j < speciesList.Length; j++)
            {
                // Current species to display
                string species = speciesList[j];
                // Filter the citizens for that species
                Citizen[] curSpecies = citizens.Where(citizen => citizen.GetType().ToString() == "Polymorphism." + species).ToArray();
                // Output a species section separator if a line has already been outputted and the next species section has citizens
                if (curSpecies.Length != 0 && isFirst)
                    Console.WriteLine("┣" + new string('━', 10) + "╋" + new string('━', 11) + "┫");
                // Output the list of citizens that are that species
                for (int i = 0; i < curSpecies.Length; i++)
                {
                    isFirst = true;
                    // Output the header for the section in the first row
                    if (i == 0)
                        Console.WriteLine("┃ {0,-8} ┃ {1,-9} ┃", species, curSpecies[i].name);
                    else
                        Console.WriteLine("┃          ┃ {0,-9} ┃", curSpecies[i].name);
                }
            }
            // Bottom of table
            Console.WriteLine("┗" + new string('━', 10) + "┻" + new string('━', 11) + "┛\n");
            Console.Write("Press any key to start collecting taxes...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // Show monthly information, with list of people that moved away or moved in
        static void ShowMonthly()
        {
            Console.WriteLine(months[date % 12] + " " + (date / 12 + startYear) + "\n");
            List<string> added = new List<string>(), removed = new List<string>(), temp;
            if (date == 0)
            {
                Console.WriteLine("Welcome to your first month in Polymorphopolis, which");
                Console.WriteLine("has a population of {0} citizens.\n", citizens.Count);
            }
            else
            {
                MonthlyPopulation(ref added, ref removed);
                temp = added.Where(adder => !removed.Contains(adder)).ToList();
                removed = removed.Where(remover => !added.Contains(remover)).ToList();
                added = temp;
                if (added.Count > 0)
                    Console.Write("This month, the following people moved into the city: ");
                for (int i = 0; i < added.Count; i++)
                {
                    if (added.Count > 2)
                    {
                        if (i == added.Count - 1)
                            Console.Write("and " + added[i]);
                        else
                            Console.Write(added[i] + ", ");
                    }
                    else if (i == 0)
                        Console.Write(added[i]);
                    else if (added.Count > 1)
                        Console.Write(" and " + added[i]);
                    else
                        Console.Write(added[i]);
                }
                if (added.Count > 0 && removed.Count > 0)
                    Console.Write(", while the following people moved away: ");
                else if (removed.Count > 0)
                    Console.Write("This month, the following people moved away: ");
                for (int i = 0; i < removed.Count; i++)
                {
                    if (removed.Count > 2)
                    {
                        if (i == removed.Count - 1)
                            Console.Write("and " + removed[i]);
                        else
                            Console.Write(removed[i] + ", ");
                    }
                    else if (i == 0)
                        Console.Write(removed[i]);
                    else if (removed.Count > 1)
                        Console.Write(" and " + removed[i]);
                    else
                        Console.Write(removed[i]);
                }
                if (removed.Count == 0 && added.Count == 0)
                    Console.Write("This month, nobody moved into or out of the city.");
                Console.WriteLine("\n");
            }
            Console.Write("Press any key to view a list of citizens...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // Advance the month by one, updating the properties of the citizens
        static void AdvanceMonth()
        {
            // Increment the date
            date++;
            for (int i = 0; i < citizens.Count; i++)
            {
                // If the citizen is already moving out next month, don't update properties
                if ((int)citizens[i].happiness <= 0)
                    continue;
                // Increment each citizen's happiness by some value between 0 and 1
                citizens[i].happiness += random.NextDouble();
                // Increment each citizen's money by a factor between 1.0 and 1.1
                citizens[i].money += citizens[i].money * 0.1 * random.NextDouble();
            }
        }

        public static void Main(string[] args)
        {
            // Stage-based game engine
            ShowIntroduction();
            InitializeCitizens();
            while (continuePlaying && citizens.Count > 0)
            {
                ShowMonthly();
                ShowCitizens();
                // If the tax goal is not reached, the game is over
                if (!CollectTaxes())
                    break;
                Console.WriteLine(months[date % 12] + " " + (date / 12 + startYear) + "\n");
                Console.WriteLine("You reached the monthly tax goal! Onto the next!\n");
                AdvanceMonth();
                // Prompt the user if they want to continue playing the game
                Console.Write("Do you want to continue playing? (Y / N) ");
                string response;
                while ((response = Console.ReadLine().ToUpper()) != "N" && response != "Y")
                    Console.Write("Invalid response. Do you want to continue playing? (Y / N) ");
                continuePlaying = response == "Y";
                Console.Clear();
            }

            // If the game was not intentially exited
            if (continuePlaying)
            {
                // If the game was ended because the monthly tax was missed
                if (citizens.Count() > 0)
                {
                    Console.WriteLine("GAME OVER\n");
                    Console.WriteLine("You didn't reach the monthly tax goal and you were fired!\n");
                }
                // If the game was ended because the city became empty
                else
                {
                    Console.WriteLine("GAME OVER\n");
                    Console.WriteLine("Your city has no more people! Better luck next time!\n");
                }
                Console.Write("Press any key to continue...");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }
}
