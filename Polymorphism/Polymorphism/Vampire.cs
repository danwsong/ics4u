using System;
namespace Polymorphism
{
    public class Vampire : Human
    {
        // Same properties as humans
        public Vampire(string name) : base(name)
        {
        }

        // Speak functions for vampires
        override public void Speak()
        {
            int rand = random.Next(2);
            if (rand == 0)
                Console.WriteLine("How about I pay you in human blood?");
            else if (rand == 1)
                Console.WriteLine("Look, you don't need the garlic, I'll pay, okay?");
        }
    }
}
