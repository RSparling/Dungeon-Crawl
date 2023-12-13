using Dungeon_Crawl.src.PlayerCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Combat.Effects
{
    public class TemporaryStatIncrease : IEffect
    {
        private Character.Stat stat;
        private int ammount;
        private int turnsRemaining;
        private bool isPlayer = false;

        private string OnApplyText = string.Empty;
        private string OnUpdateText = string.Empty;
        private string OnEndText = string.Empty;

        public TemporaryStatIncrease(Character.Stat statToBoost, int ammount, int turns, string OnApplyText, string OnUpdateText, string OnEndText)
        {
            this.stat = statToBoost;
            this.ammount = ammount;
            turnsRemaining = turns;
            this.OnUpdateText = OnUpdateText;
            this.OnEndText = OnEndText;
            this.OnApplyText = OnApplyText;
        }

        public void OnApply(ICombatant affected)
        {
            if (affected is IEffected effected)
            {
                effected.ModifyStat(stat, ammount);
            }
            isPlayer = affected.GetName() == PlayerCore.Player.Instance.GetComponent<PlayerCombat>().GetName();

            if (!isPlayer || OnApplyText != string.Empty)
                return;

            PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnApplyText);
        }

        public void OnRemove(ICombatant affected)
        {
            if (affected is IEffected effected)
            {
                effected.ModifyStat(stat, -ammount);
            }
            if (isPlayer && OnEndText != string.Empty)
                PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnEndText);
        }

        public bool Update(ICombatant affected)
        {
            if (isPlayer && OnUpdateText != string.Empty)
                PlayerCore.Player.Instance.GetComponent<PlayerUI>().UpdateCombatLog(OnUpdateText);
            return turnsRemaining-- <= 0;
        }
    }
}