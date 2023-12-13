using Dungeon_Crawl.src.Combat.Effects;
using Dungeon_Crawl.src.PlayerCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat.Skills
{
    internal class FireBolt : ISkill
    {
        public string GetName()

        {
            return "Fire Bolt";
        }

        public bool IsSuccessful(ICombatant attacker, ICombatant defender)
        {
            return Combat.Roll(3) < attacker.GetStat(Character.Stat.Intelligence) + 2;
        }

        public void OnFail(ICombatant attack, ICombatant defender)
        {
            PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(attack.GetName() + " fizzled their spell and took some damge from magical backlash.");
            attack.TakeDamage(1);
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_fizzle);
        }

        public void OnSuccess(ICombatant attack, ICombatant defender)
        {
            int damage = Combat.Roll(1) + 1;
            PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(attack.GetName() + " flings a spell at " + defender.GetName() + " dealing " + damage + " damage.");

            bool isOnFire = true;
            if (isOnFire)
            {
                IEffect effect = new DamageOverTime(1, 3,
                    defender.GetName() + " catches fire!",
                    defender.GetName() + " is still burning!",
                    defender.GetName() + " is no longer on fire!");

                defender.AddEffect(effect);
            }
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_spell);
        }
    }
}