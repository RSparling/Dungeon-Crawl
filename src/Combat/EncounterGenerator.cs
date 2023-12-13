using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat
{
    internal class EncounterGenerator
    {
        private float baseEncounterChance = 10;
        private float increaseEncounterChancePerStep = 0.5f;
        private float currentEncounterChance = 3;

        public void PlayerStep(System.Drawing.Point value) //parameter is unused
        {
            currentEncounterChance += increaseEncounterChancePerStep;

            Random rand = new Random(Guid.NewGuid().ToString().GetHashCode());
            int roll = rand.Next(0, 100);
            if (roll <= currentEncounterChance)
            {
                Encounter encounter = new Encounter();
                encounter.StartCombat();
                currentEncounterChance = baseEncounterChance;
            }
        }
    }
}