using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon_Crawl.src.PlayerCore;
using Dungeon_Crawl.src.PlayerCore.Components;

namespace Dungeon_Crawl.src.Combat.Skills

{
    public class AngryJiggle : ISkill
    {
        string ISkill.GetName()
        {
            return "AngryJiggle";
        }

        bool ISkill.IsSuccessful(ICombatant attacker, ICombatant defender)
        {
            return Combat.Roll(3) < attacker.GetStat(Character.Stat.Agility) - 2;
        }

        void ISkill.OnFail(ICombatant attack, ICombatant defender)
        {
            Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(attack.GetName() + " jiggles too hard at " + defender.GetName() + " and misses!");
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_miss);
        }

        void ISkill.OnSuccess(ICombatant attack, ICombatant defender)
        {
            int damage = Combat.Roll() - 2;
            if (damage < 1)
                damage = 1;
            Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(attack.GetName() + " jiggled really hard at " + defender.GetName() + " and deals " + damage + " damage.");
            defender.TakeDamage(damage);
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_hit1);
        }
    }
}