using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dungeon_Crawl.src.Character;
using Dungeon_Crawl.src.Combat;
using Dungeon_Crawl.src.PlayerCore;
using Dungeon_Crawl.src.PlayerCore.Components;

namespace Dungeon_Crawl.src.Combat.Skills
{
    internal class RiskySlash : ISkill
    {
        private PlayerUI playerUI;

        public string GetName()
        { return "Risky Attack"; }

        public bool IsSuccessful(ICombatant attacker, ICombatant defender)
        {
            playerUI = Player.Instance.GetComponent<PlayerUI>();
            int roll = Combat.Roll();
            return roll <= attacker.GetStat(Stat.Agility) - 7;
        }

        public void OnSuccess(ICombatant attacker, ICombatant defender)
        {
            int damage = Combat.Roll(2) + (attacker.GetStat(Stat.Strength) / 2);
            defender.TakeDamage((damage / 2) * 3);
            playerUI.UpdateCombatLog(attacker.GetName() + " manage to hit them with a risky blow dealing: " + damage + " damage.");
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_hit2);
        }

        public void OnFail(ICombatant attacker, ICombatant defender)
        {
            int damage = Combat.Roll(2) + (attacker.GetStat(Stat.Strength) / 2);
            attacker.TakeDamage(damage);
            playerUI.UpdateCombatLog(attacker.GetName() + " overexerted their fighting spirit and took: " + damage + " damage.");
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_fizzle);
        }
    }
}