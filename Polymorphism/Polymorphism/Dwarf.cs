using System;
namespace Polymorphism
{
    public class Dwarf : Citizen
    {
        // Dwarves use gold piece currency
        public Dwarf(string name) : base(name, "gold piece")
        {
        }

        // Speak functions for dwarves
        override public void Speak() {
            int rand = random.Next(3);
            if (rand == 0)
                Console.WriteLine("Come back to take my gold again?");
            else if (rand == 1)
                Console.WriteLine("I'll have you know, I mined this gold with my own hands.");
            else
                Console.WriteLine("Did the tax rates go up again?");
        }
    }
}
