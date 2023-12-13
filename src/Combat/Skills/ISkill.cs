using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat
{
    public interface ISkill
    {
        string GetName();

        bool IsSuccessful(ICombatant attacker, ICombatant defender);

        void OnSuccess(ICombatant attack, ICombatant defender);

        void OnFail(ICombatant attack, ICombatant defender);
    }
}