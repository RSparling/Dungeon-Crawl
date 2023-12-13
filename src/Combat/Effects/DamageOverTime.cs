using Dungeon_Crawl.src.PlayerCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat.Effects
{
    internal class DamageOverTime : IEffect
    {
        private int ammount;
        private int turnsRemaining;
        private bool isPlayer = false;

        private string OnApplyText = string.Empty;
        private string OnUpdateText = string.Empty;
        private string OnEndText = string.Empty;

        public DamageOverTime(int ammount, int turns, string OnApplyText, string OnUpdateText, string OnEndText)
        {
            this.ammount = ammount;
            turnsRemaining = turns;
            this.OnUpdateText = OnUpdateText;
            this.OnEndText = OnEndText;
            this.OnApplyText = OnApplyText;
        }

        public void OnApply(ICombatant affected)
        {
            affected.TakeDamage(ammount);
            isPlayer = affected.GetName() == PlayerCore.Player.Instance.GetComponent<PlayerCombat>().GetName();

            if (OnApplyText != string.Empty)
                PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnApplyText);
        }

        public void OnRemove(ICombatant affected)
        {
            if (OnEndText != string.Empty)
                PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnEndText);
        }

        public bool Update(ICombatant affected)
        {
            affected.TakeDamage(ammount);
            if (OnUpdateText != string.Empty)
                PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnUpdateText);
            return turnsRemaining-- <= 0;
        }
    }
}