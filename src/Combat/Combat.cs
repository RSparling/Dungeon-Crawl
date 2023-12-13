/// <summary>
/// The Combat class handles combat-related functionality in the game.
/// </summary>
/// <returns>
/// None
/// </returns>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat
{
    internal class Combat
    {
        private static Combat instance; // Declare a static instance of the Combat class
        private static Random random = new Random(); // Declare a static instance of the Random class

        public Combat()
        {
            instance = this; // Set the instance variable to the current instance of the Combat class
            random = new Random(Guid.NewGuid().GetHashCode()); // Generate a new random seed based on a unique identifier
        }

        public Combat Get // Declare a static property to get the instance of the Combat class
        {
            get
            {
                if (instance == null) // Check if the instance is null
                    instance = new Combat(); // If it is null, create a new instance of the Combat class
                return instance; // Return the instance of the Combat class
            }
        }

        public static int Roll(int dice = 1, int modifier = 0) // Declare a static method to roll dice
        {
            int total = 0; // Initialize a variable to store the total value of the dice rolls
            for (int x = 0; x < dice; x++) // Loop through the number of dice rolls
            {
                total += random.Next(0, 7); // Add a random number between 0 and 6 to the total
            }
            return total + modifier; // Return the total value of the dice rolls plus the modifier
        }
    }
}