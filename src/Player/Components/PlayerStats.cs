using Dungeon_Crawl.src.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.PlayerCore.Components
{
    public class PlayerStats : IComponent
    {
        private Stats stats = new Stats();

        private Player player;

        public delegate void HealthChangeEventHandler(int current);

        public delegate void EnergyChangeEventHandler(int current);

        public delegate void StatChangeHandler(Stat stat, int value);

        public event HealthChangeEventHandler OnHealthChanged;

        public event EnergyChangeEventHandler OnEnergyChanged;

        public event StatChangeHandler OnStatChange;

        public PlayerStats(Player player)
        {
            this.player = player;
            stats = new Stats();
        }

        public void Initialize()
        {
        }

        public void Update()
        {
        }

        public int GetStat(Stat stat)
        {
            return stats[stat];
        }

        public void Heal(int ammt)
        {
            stats.Heal(ammt);
            OnHealthChanged?.Invoke(stats[Stat.Hitpoints]);
        }

        public void TakeDamage(int damage)
        {
            stats.TakeDamage(damage);
            OnHealthChanged?.Invoke(stats[Stat.Hitpoints]);
        }

        public void ModifyStat(Stat stat, int value, bool preventNeg = true)
        {
            stats.Modify(stat, value, preventNeg);
            OnStatChange?.Invoke(stat, stats[stat]);
        }

        public bool isDead
        { get { return stats[Stat.Hitpoints] <= 0; } }

        public void ForcePushEvents()
        {
            OnHealthChanged?.Invoke(stats[Stat.Hitpoints]);
            OnStatChange?.Invoke(Stat.MaxHitpoints, stats[Stat.MaxHitpoints]);
        }
    }
}