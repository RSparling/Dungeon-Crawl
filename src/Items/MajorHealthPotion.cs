using Dungeon_Crawl.src.Combat;
using Dungeon_Crawl.src.Combat.Effects;
using Dungeon_Crawl.src.PlayerCore;
using Dungeon_Crawl.src.PlayerCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Items
{
    internal class MajorHealthPotion : IItem
    {
        public string GetName()
        {
            return "Major Health Potion";
        }

        public void Use(PlayerCombat player)
        {
            IEffect effect = new HealOverTime(20, 1, "Your body feels hot as your woulds close.", "Your wounds have closed up.", String.Empty);
            player.AddEffect(effect);
        }
    }
}