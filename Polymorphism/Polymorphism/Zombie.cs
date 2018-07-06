using System;
namespace Polymorphism
{
    public class Zombie : Human
    {
        // Same properties as humans
        public Zombie(string name) : base(name)
        {
        }

        // Speak functions for zombies
        override public void Speak()
        {
            int rand = random.Next(2);
            if (rand == 0)
                Console.WriteLine("Do I still need to pay if I'm dead?");
            else if (rand == 1)
                Console.WriteLine("I'm trying to keep my body from disintegrating, and here you are, taking my money.");
        }
    }
}
