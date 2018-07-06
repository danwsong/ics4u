using System;

namespace Polymorphism
{
    public class Werewolf : Human, IWolf
    {
        // Same properties as humans
        public Werewolf(string name) : base(name)
        {
        }

        // Different speak functionality based on IWolf interface
        override public void Speak()
        {
            int rand = random.Next(2);
            if (rand == 0)
                Growl();
            else
                Howl();
        }

        // Speak functions that implement IWolf
        public void Growl()
        {
            Console.WriteLine("Grrrrrrrrrrrrr...");
        }

        public void Howl()
        {
            Console.WriteLine("A-ooooooooooooo!");
        }
    }
}
