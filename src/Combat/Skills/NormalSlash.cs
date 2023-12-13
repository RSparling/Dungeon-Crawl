using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon_Crawl.src.Character;
using Dungeon_Crawl.src.PlayerCore;
using Dungeon_Crawl.src.PlayerCore.Components;

namespace Dungeon_Crawl.src.Combat.Skills
{
    internal class NormalSlash : ISkill
    {
        private PlayerUI playerUI;

        // Check if the skill is successful
        public bool IsSuccessful(ICombatant attacker, ICombatant defender)
        {
            playerUI = Player.Instance.GetComponent<PlayerUI>();
            // Roll a dice and compare it to the attacker's agility stat minus 4
            return Combat.Roll(3) > attacker.GetStat(Stat.Agility) - 4;
        }

        // Perform actions when the skill is successful
        public void OnSuccess(ICombatant attacker, ICombatant defender)
        {
            // Calculate damage based on a dice roll and the attacker's strength stat
            int damage = Combat.Roll() + attacker.GetStat(Stat.Strength) / 3;
            // Inflict damage on the defender
            defender.TakeDamage(damage);

            playerUI.UpdateCombatLog(attacker.GetName() + " slash at the enemy dealing: " + damage);
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_hit1);
        }

        // Perform actions when the skill fails
        public void OnFail(ICombatant attacker, ICombatant defender)
        {
            playerUI.UpdateCombatLog("Somehow," + attacker.GetName() + " missed.");
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_miss);
            return;
        }

        // Get the name of the skill
        public string GetName()
        {
            return "Normal Slash";
        }
    }
}