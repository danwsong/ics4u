using System;
using System.Linq;

namespace Polymorphism
{
    public class Citizen
    {
        public string name;
        public string currency;
        public double money;
        public double happiness;

        protected static Random random = new Random();

        // Constructor for all citizens
        public Citizen(string name, string currency, double happiness = 10)
        {
            this.name = name;
            this.currency = currency;
            // Some amount of money between $800 and $1200
            this.money = 800 + random.NextDouble() * 400;
            this.happiness = happiness;
        }

        // Return the amount of money with the currency for the citizen as a string
        public string GetMoney(bool withConversion = false)
        {
            if (!withConversion || currency == "dollar")
                return Math.Round(money) + " " + currency + "s";
            else
                return Math.Round(MoneyInCurrency(money, currency)) + " " + currency + "s, or " + Math.Round(money) + " dollars";
        }

        // Converts the given money in dollars to an amount in the given currency
        static double MoneyInCurrency(double money, string currency)
        {
            if (currency == "gem")
                return money / 1.732;
            else if (currency == "gold piece")
                return money / 0.827;
            return money;
        }

        // Decreases the citizen's money and happiness based on the amount of tax charged
        // and causes the citizen to speak
        public void PayTaxes(double tax)
        {
            happiness -= money <= tax ? happiness : 1 / Math.Log10(Math.Round(money) / Math.Round(tax));
            happiness = happiness < 0 ? 0 : happiness;
            money -= tax;
            Console.WriteLine(name + " was taxed " + Math.Round(MoneyInCurrency(tax, currency)) + " " + currency + "s.");
            Console.WriteLine(name + "'s new happiness level is {0}.", (int)happiness);
            Console.Write(name + ": ");
            Speak();
        }

        // Unimplemented speak function to be implemented in classes that inherit from Citizen
        virtual public void Speak()
        {

        }
    }
}
