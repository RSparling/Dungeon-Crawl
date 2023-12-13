// Import necessary namespaces
using Dungeon_Crawl.src.Combat;
using Dungeon_Crawl.src.Combat.Skills;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Character
{
    public class Monster : ICombatant
    {
        private Stats stats = new Stats(); // Declare a private variable to store the monster's stats

        //private Image image;

        private ICombatant enemy;

        private List<ISkill> attacks = new List<ISkill>();
        private List<IEffect> activeEffects = new List<IEffect>();

        public Monster(Image image, string name, int hitpoints, int maxHitpoints, int fatigue, int maxFatigue, int strength, int agility, int vitality, int intelligence, ISkill[] moves)
        {
            this.name = name; // Set the monster's name to the provided name
            this.image = image;
            stats[Stat.Hitpoints] = hitpoints; // Set the monster's hitpoints to the provided hitpoints
            stats[Stat.MaxHitpoints] = maxHitpoints; // Set the monster's maximum hitpoints to the provided maximum hitpoints
            stats[Stat.FatiguePoints] = fatigue; // Set the monster's fatigue to the provided fatigue
            stats[Stat.MaxFatiguePoints] = maxFatigue; // Set the monster's maximum fatigue to the provided maximum fatigue
            stats[Stat.Strength] = strength; // Set the monster's strength to the provided strength
            stats[Stat.Agility] = agility; // Set the monster's dexterity to the provided dexterity
            stats[Stat.Vitality] = vitality; // Set the monster's vitality to the provided vitality
            stats[Stat.Intelligence] = intelligence;
            attacks.AddRange(moves);
        }

        public Image image;

        public void SetMonsterImage()
        {
            //CombatUI.Get.SetMonsterImage(image);
        }

        public void SetStats(Stats stats)
        {
            this.stats = stats; // Set the monster's stats to the provided stats object
        }

        private string name = "Monster";

        public int GetFatigue()
        {
            return 10; // Return a constant value of 10 for the monster's fatigue
        }

        public int GetHitpoints()
        {
            return stats[Stat.Hitpoints]; // Return the current hitpoints of the monster
        }

        public int GetMaxFatigue()
        {
            return 10; // Return a constant value of 10 for the monster's maximum fatigue
        }

        public int GetMaxHitpoints()
        {
            return stats[Stat.MaxHitpoints]; // Return the maximum hitpoints of the monster
        }

        public int GetStat(Stat stat)
        {
            return stats[stat]; // Return the value of the specified stat for the monster
        }

        public void Heal(int heal)
        {
            stats.Heal(heal); // Heal the monster by the specified amount
        }

        public bool IsDead()
        {
            return stats[Stat.Hitpoints] <= 0; // Check if the monster's hitpoints are less than or equal to 0
        }

        public void TakeDamage(int damage)
        {
            stats.TakeDamage(damage); // Reduce the monster's hitpoints by the specified amount
        }

        //preform random skill with monster as attacker and player as defender
        public void Attack()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());

            int i = rand.Next(0, attacks.Count); // Get a random number between 0 and the number of attacks the monster has
            if (attacks[i].IsSuccessful(this, enemy))
                attacks[i].OnSuccess(this, enemy); // If the attack is successful, perform the attack
            else
                attacks[i].OnFail(this, enemy); // If the attack is unsuccessful, perform the fail action
        }

        public string GetName()
        {
            return name;
        }

        public void SetCurrentEnemy(ICombatant combatant)
        {
            enemy = combatant;
        }

        public void ProcessEffects()
        {
            List<IEffect> effectsToRemove = new List<IEffect>();

            foreach (IEffect effect in activeEffects)
            {
                if (effect.Update(this))
                {
                    effect.OnRemove(this);
                    effectsToRemove.Add(effect);
                }
            }

            foreach (IEffect effect in effectsToRemove)
            {
                activeEffects.Remove(effect);
            }
        }

        public void AddEffect(IEffect effect)
        {
            effect.OnApply(this);
            activeEffects.Add(effect);
        }
    }
}