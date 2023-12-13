using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat
{
    public interface IEffect

    {
        void OnApply(ICombatant affected);

        bool Update(ICombatant affected);

        void OnRemove(ICombatant affected);
    }
}