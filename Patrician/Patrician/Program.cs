using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace Patrician
{
    class MainClass
    {
        static bool[,] maze;
        static int width, height;
        static int endX, endY;

        // Solve the maze using breadth-first search, returning the correct
        // path as a sequence of letters representing directions of movement, 
        // or "no path" if no path can be found
        static string SolveMaze()
        {
            // Array to store whether a location has already been visited
            bool[,] visited = new bool[width, height];
            // Queue stores the next locations to visit
            Queue<Tuple<int, int, string>> queue = new Queue<Tuple<int, int, string>>();
            // Set the first location to visit to (1, 1), the starting location of the maze,
            // with a path with no directions
            queue.Enqueue(new Tuple<int, int, string>(1, 1, ""));
            // Continue visiting locations until the queue is empty
            while (queue.Count > 0)
            {
                // Dequeue the next location to visit
                Tuple<int, int, string> next = queue.Dequeue();
                // Get all the information stored in the current location metadata
                int x = next.Item1, y = next.Item2;
                string path = next.Item3;
                // If the target location is reached, return the path that reached the target location
                if (x == endX && y == endY)
                    return path;
                // If the current location location is a wall or has already been visited, skip the location
                if (maze[x, y] || visited[x, y])
                    continue;
                // Set the location to have already been visited
                visited[x, y] = true;
                // Add the four locations around the current location as the next to be visited,
                // adding the direction required to reach those locations as the next letter in the path
                if (x != 0)
                    queue.Enqueue(new Tuple<int, int, string>(x - 1, y, path + "l"));
                if (x != width - 1)
                    queue.Enqueue(new Tuple<int, int, string>(x + 1, y, path + "r"));
                if (y != 0)
                    queue.Enqueue(new Tuple<int, int, string>(x, y - 1, path + "u"));
                if (y != height - 1)
                    queue.Enqueue(new Tuple<int, int, string>(x, y + 1, path + "d"));
            }
            // If the target location is never reached, return that no path exists
            return "no path";
        }

        // Helper function to print out text as a certain color
        static void PrintColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
        }

        // Display the maze, a path, and the treasure hunter if displayPlayer is true
        static void DisplaySolution(string path, bool displayPlayer)
        {
            // Create an array that will store which locations are part of the correct path
            int[,] solution = new int[width, height];
            int curX = 1, curY = 1;
            while (path != "")
            {
                // Get the next direction in the path
                char next = path[0];
                // Remove the direction from the path
                path = path.Substring(1);
                // Move the pointer to the solution array depending on the direction
                if (next == 'l')
                    solution[curX--, curY] = 1;
                if (next == 'r')
                    solution[curX++, curY] = 2;
                if (next == 'u')
                    solution[curX, curY--] = 3;
                if (next == 'd')
                    solution[curX, curY++] = 4;
            }

            // Loop through the solution and maze array
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // If the treasure hunter is to be displayed, and the x and y
                    // correspond to the end of the path, draw the treasure hunter 
                    if (displayPlayer && x == curX && y == curY)
                        PrintColor("█ ", ConsoleColor.DarkRed);
                    // If the x and y correspond to the target location, draw the treasure chest
                    else if (x == endX && y == endY)
                        PrintColor("$ ", ConsoleColor.DarkYellow);
                    // If the x and y correspond to a wall of the maze, draw the maze wall
                    else if (maze[x, y])
                        PrintColor("█ ", ConsoleColor.DarkGray);
                    // If the x and y correspond to a part of the solution path, draw an arrow
                    // that points in the direction of movement
                    else if (solution[x, y] == 1)
                        PrintColor("← ", ConsoleColor.Green);
                    else if (solution[x, y] == 2)
                        PrintColor("→ ", ConsoleColor.Green);
                    else if (solution[x, y] == 3)
                        PrintColor("↑ ", ConsoleColor.Green);
                    else if (solution[x, y] == 4)
                        PrintColor("↓ ", ConsoleColor.Green);
                    // Otherwise, don't draw anything in that space
                    else
                        Console.Write("  ");
                }
                // Move down to the next row each time a row is finished drawing
                Console.WriteLine();
            }
        }

        public static void Main(string[] args)
        {
            // Introduction
            Console.Clear();
            Console.WriteLine("MAZE SOLVER");
            Console.WriteLine("Welcome to the maze solver, a program that solves your mazes.");

            // Prompt for the user to input the maze file
            Console.WriteLine("To get started enter in the location of the maze file:");
            string fileLocation = Console.ReadLine().Trim();
            while (!File.Exists(fileLocation))
            {
                Console.WriteLine("It appears that the file you entered does not exist. Please try again:");
                fileLocation = Console.ReadLine().Trim();
            }
            // Open the maze file
            StreamReader reader = new StreamReader(new FileStream(fileLocation, FileMode.Open));
            try
            {
                height = int.Parse(reader.ReadLine());
                width = int.Parse(reader.ReadLine());
                maze = new bool[width, height];
                for (int y = 0; y < height; y++)
                {
                    string currentLine = reader.ReadLine();
                    for (int x = 0; x < width; x++)
                    {
                        switch (currentLine[x])
                        {
                            case ' ':
                                maze[x, y] = false;
                                break;
                            case 'X':
                                maze[x, y] = true;
                                break;
                            case '$':
                                maze[x, y] = false;
                                endX = x;
                                endY = y;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("The maze file is corrupt and cannot be read from.");
                Console.Write("\nPress any key to exit...");
                Console.ReadKey(true);
                return;
            }
            finally
            {
                reader.Close();
            }
            // Other errors in the data input that are not directly related to inputting the maze from the file
            if (width < 2 || height < 2)
            {
                Console.WriteLine("The maze is too small; it must be at least 2 blocks in width and height.");
                Console.Write("\nPress any key to exit...");
                Console.ReadKey(true);
                return;
            }
            if (maze[1, 1])
            {
                Console.WriteLine("The initial location for the treasure hunter cannot be a wall.");
                Console.Write("\nPress any key to exit...");
                Console.ReadKey(true);
                return;
            }
            if (endX == 1 && endY == 1)
            {
                Console.WriteLine("If the initial location of the maze is the same as the treasure\nthe maze is already solved.");
                Console.Write("\nPress any key to exit...");
                Console.ReadKey(true);
                return;
            }
            Console.Clear();

            // Draw only the maze by using the display solution function and
            // not giving a path or displaying the treasure hunter
            Console.WriteLine("Maze:\n");
            DisplaySolution("", false);

            // Solve the maze using breadth-first search
            string path = SolveMaze();
            // If there is no path for the maze
            if (path == "no path")
            {
                Console.WriteLine("\nThere is no solution to this maze.");
            }
            // Otherwise output the solution
            else
            {
                Console.Write("\nPress any key to display solution...");
                Console.ReadKey(true);
                Console.Clear();

                // Output the path
                Console.WriteLine("Solution:\n");
                DisplaySolution(path, false);
                Console.Write("\nPress any key to display an animation of the solution...");
                Console.ReadKey(true);

                // Display successive lengths of the path to create an animation of the
                // treasure hunter walking through the maze
                for (int i = 1; i <= path.Length; i++)
                {
                    Console.Clear();
                    Console.WriteLine("Animation:\n");
                    DisplaySolution(path.Substring(0, i), true);
                    Console.WriteLine();
                    // Steps once every 100 ms
                    Thread.Sleep(1000 / 10);
                }
                Console.Write("\nPress any key to exit...");
                Console.ReadKey(true);
            }
        }
    }
}
