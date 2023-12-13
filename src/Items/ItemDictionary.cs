using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Items
{
    public class ItemDictionary
    {
        private static List<IItem> itemPrototypes;

        private static void Instantiate()
        {
            itemPrototypes = new List<IItem>();
            itemPrototypes.Add(new MajorHealthPotion());
            itemPrototypes.Add(new ModerateHealthPotion());
            itemPrototypes.Add(new ModerateHealthPotion());
            itemPrototypes.Add(new ModerateHealthPotion());
            itemPrototypes.Add(new ModerateHealthPotion());
            itemPrototypes.Add(new ModerateHealthPotion());
            itemPrototypes.Add(new ModerateHealthPotion());
            itemPrototypes.Add(new ModerateHealthPotion());
            itemPrototypes.Add(new ModerateHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
            itemPrototypes.Add(new MinorHealthPotion());
        }

        public static IItem GetCopyOfRandomItem()
        {
            if (itemPrototypes == null)
                Instantiate();
            Random rand = new Random(Guid.NewGuid().ToString().GetHashCode());
            return itemPrototypes[rand.Next(0, itemPrototypes.Count)];
        }
    }
}