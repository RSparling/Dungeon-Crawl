using Dungeon_Crawl.src.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon_Crawl.src.PlayerCore;
using Dungeon_Crawl.src.PlayerCore.Components;

namespace Dungeon_Crawl.src
{
    public interface IItem
    {
        string GetName();

        void Use(PlayerCombat player);
    }
}