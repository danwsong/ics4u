using System;

namespace Polymorphism
{
    public class Human : Citizen
    {
        // Humans use dollar currency
        public Human(string name, double happiness = 10) : base(name, "dollar", happiness)
        {
        }

        // Speak functions for humans
        override public void Speak()
        {
            int rand = random.Next(3);
            if (rand == 0)
                Console.WriteLine("Is it already that time of the month again?");
            else if (rand == 1)
                Console.WriteLine("Yeah, yeah, here's the money.");
            else
                Console.WriteLine("Here I am, toiling away, while you come and take my money every month...");
        }
    }
}
