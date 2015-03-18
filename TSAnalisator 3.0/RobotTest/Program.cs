using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotTest
{
    class Pet
    {
        public string Name { get; set; }
        public double Age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a list of pets.
            List<Pet> petsList =
                new List<Pet>{ new Pet { Name="Barley", Age=8 },
                               new Pet { Name="Boots", Age=4 },
                               new Pet { Name="Whiskers", Age=4 },
                               new Pet { Name="Daisy", Age=4 } };

            // Group Pet objects by the Math.Floor of their age. 
            // Then project an anonymous type from each group 
            // that consists of the key, the count of the group's 
            // elements, and the minimum and maximum age in the group. 
            var query = petsList.GroupBy(
                pet => pet.Age,
                (age, pets) => new
                {
                    //Key = age,
                    Count = pets.Count(),
                    //Min = pets.Min(pet => pet.Age),
                    //Max = pets.Max(pet => pet.Age)
                });

            // Iterate over each anonymous type. 
            foreach (var result in query)
            {
                Console.WriteLine("\nAge count: " + result.Count);
                //Console.WriteLine("Number of pets in this age group: " + result.Count);
                //Console.WriteLine("Minimum age: " + result.Min);
                //Console.WriteLine("Maximum age: " + result.Max);
            }

            Console.ReadKey();
        }
    }
}
