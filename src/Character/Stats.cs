using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Character
{
    public class Stats
    {
        private Dictionary<Stat, int> stats;

        public Stats()
        {
            stats = new Dictionary<Stat, int>();
            stats.Add(Stat.Strength, 10);
            stats.Add(Stat.Agility, 10);
            stats.Add(Stat.Intelligence, 10);
            stats.Add(Stat.Vitality, 10);
            stats.Add(Stat.Hitpoints, 10);
            stats.Add(Stat.MaxHitpoints, 10);
            stats.Add(Stat.FatiguePoints, 10);
            stats.Add(Stat.MaxFatiguePoints, 10);
        }

        public int this[Stat stat]
        {
            get { return stats[stat]; }
            set { stats[stat] = value; }
        }

        public void TakeDamage(int damage)
        {
            this[Stat.Hitpoints] -= damage;
            if (this[Stat.Hitpoints] < 0)
                this[Stat.Hitpoints] = 0;
        }

        public void Heal(int heal)
        {
            this[Stat.Hitpoints] += heal;
            if (this[Stat.Hitpoints] > this[Stat.MaxHitpoints])
                this[Stat.Hitpoints] = this[Stat.MaxHitpoints];
        }

        public void Modify(Stat stat, int ammount, bool preventNeg = true)

        {
            this[stat] += ammount;
            if (!preventNeg)
                return;

            if (this[stat] < 0)
                this[stat] = 0;
        }
    }

    public enum Stat
    {
        Strength,
        Agility,
        Intelligence,
        Vitality,
        Hitpoints,
        MaxHitpoints,
        FatiguePoints,
        MaxFatiguePoints
    }
}