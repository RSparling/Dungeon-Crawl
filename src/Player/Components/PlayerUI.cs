using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Crawl.src.PlayerCore.Components
{
    internal class PlayerUI : IComponent
    {
        private Player player;
        private DungeonForm form;

        private int playerMaxHP;
        private int playerMaxEnergy;

        private List<string> combatLog = new List<string>();

        public PlayerUI(Player player)
        {
            this.player = player;
            form = DungeonForm.ActiveForm as DungeonForm;
        }

        public void Initialize()
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();

            stats.OnEnergyChanged += OnPlayerEnergyChange;
            stats.OnHealthChanged += OnPlayerHealthUpdate;
            stats.OnStatChange += OnStatChange;
            playerMaxHP = stats.GetStat(Character.Stat.MaxHitpoints);
            playerMaxEnergy = stats.GetStat(Character.Stat.MaxFatiguePoints);
        }

        public void Update()
        {
        }

        private void OnPlayerHealthUpdate(int value)
        {
            if (form == null)
                form = Form.ActiveForm as DungeonForm;

            if (form == null)
                return;
            float current = value; //allows for division to be non int
            form.SetHPBarPercentage(current / playerMaxHP);
            form.SetHPText(value, playerMaxHP);
        }

        private void OnPlayerEnergyChange(int value)
        {
            return; //in place in case time permits energy system
        }

        private void OnStatChange(Character.Stat stat, int value)
        {
            if (stat != Character.Stat.MaxFatiguePoints && stat != Character.Stat.MaxHitpoints)
                return;

            switch (stat)
            {
                case Character.Stat.MaxHitpoints:
                    playerMaxHP = value;
                    return;

                case Character.Stat.MaxFatiguePoints:
                    playerMaxEnergy = value;
                    return;

                default:
                    return;
            }
        }

        public void UpdateCombatLog(string line)
        {
            combatLog.Add(line);
            form.SetCombatLog(combatLog);
        }

        public void PrepForCombat()
        {
            player.GetComponent<PlayerStats>().ForcePushEvents();
        }
    }
}