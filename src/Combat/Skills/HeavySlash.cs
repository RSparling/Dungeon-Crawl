using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon_Crawl.src.Character;

namespace Dungeon_Crawl.src.Combat.Skills
{
    internal class HeavySlash : ISkill
    {
        public string GetName()
        {
            return "Heavy Slash";
        }

        public bool IsSuccessful(ICombatant attacker, ICombatant defender)
        {
            int roll = Combat.Roll(3);
            return roll <= attacker.GetStat(Stat.Agility) - 6;
        }

        public void OnSuccess(ICombatant attacker, ICombatant defender)
        {
            int damage = Combat.Roll() * +attacker.GetStat(Stat.Strength) / 2;
            defender.TakeDamage(damage);

            //CombatUI.Get.UpdateCombatLog(attacker.GetName() + " manage to hit them with a heavy blow dealing: " + damage);
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_hit3);
        }

        public void OnFail(ICombatant attacker, ICombatant defender)
        {
            //CombatUI.Get.UpdateCombatLog(attacker.GetName() + " commited to hard to your attack and missed.");
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_miss);
            return;
        }
    }
}