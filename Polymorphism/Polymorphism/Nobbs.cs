using System;
namespace Polymorphism
{
    public class Nobbs : Human
    {
        // Same properties as humans, except less happiness
        public Nobbs(string name) : base(name, 5)
        {
        }

        // Speak functions for Nobbs
        override public void Speak()
        {
            int rand = random.Next(2);
            if (rand == 0)
                Console.WriteLine("I didn't have much to begin with and you still take more?");
            else if (rand == 1)
                Console.WriteLine("Hey, I need this money to buy cigarettes.");
        }
    }
}
