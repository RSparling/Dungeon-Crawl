using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat
{
    public interface IEffected
    {
        void ModifyStat(Character.Stat stat, int ammount);
    }
}