using System;

namespace Polymorphism
{
    class Troll : Citizen
    {
        // Trolls use gems as currency, have less initial happiness
        public Troll(string name) : base(name, "gem", 5)
        {
        }

        // Speak functions for trolls
        override public void Speak()
        {
            int rand = random.Next(2);
            if (rand == 0)
                Console.WriteLine("Just because I'm a bridgekeeper doesn't mean I like taxes.");
            else if (rand == 1)
                Console.WriteLine("Alright, alright, just take the gems...");;
        }
    }
}
