using Dungeon_Crawl.src.Character;
using Dungeon_Crawl.src.PlayerCore.Components;
using Dungeon_Crawl.src.PlayerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat.Effects
{
    internal class HealOverTime : IEffect
    {
        private int ammount;
        private int turnsRemaining;
        private bool isPlayer = false;

        private string OnApplyText = string.Empty;
        private string OnUpdateText = string.Empty;
        private string OnEndText = string.Empty;

        public HealOverTime(int ammount, int turns, string OnApplyText, string OnUpdateText, string OnEndText)
        {
            this.ammount = ammount;
            turnsRemaining = turns;
            this.OnUpdateText = OnUpdateText;
            this.OnEndText = OnEndText;
            this.OnApplyText = OnApplyText;
        }

        public void OnApply(ICombatant affected)
        {
            affected.Heal(ammount);
            isPlayer = affected.GetName() == PlayerCore.Player.Instance.GetComponent<PlayerCombat>().GetName();

            if (!isPlayer || OnApplyText != string.Empty)
                return;

            PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnApplyText);
        }

        public void OnRemove(ICombatant affected)
        {
            if (isPlayer && OnEndText != string.Empty)
                PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnEndText);
        }

        public bool Update(ICombatant affected)
        {
            affected.Heal(ammount);
            if (isPlayer && OnUpdateText != string.Empty)
                PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnUpdateText);
            return turnsRemaining-- <= 0;
        }
    }
}