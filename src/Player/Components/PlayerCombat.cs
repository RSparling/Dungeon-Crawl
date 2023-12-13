using Dungeon_Crawl.src.Character;
using Dungeon_Crawl.src.Combat;
using Dungeon_Crawl.src.Combat.Skills;
using Dungeon_Crawl.src.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Crawl.src.PlayerCore.Components
{
    public class PlayerCombat : IComponent, ICombatant
    {
        private PlayerStats playerStats;

        private ICombatant enemy;

        private List<ISkill> attacks;

        private List<ISkill> skills;

        private List<IEffect> activeEffects;

        private List<IItem> items;

        public void Initialize()
        {
            attacks = new List<ISkill>();
            skills = new List<ISkill>();
            activeEffects = new List<IEffect>();
            items = new List<IItem>();
            attacks.Add(new NormalSlash());
            attacks.Add(new HeavySlash());
            attacks.Add(new RiskySlash());

            skills.Add(new FireBolt());

            items.Add(new ModerateHealthPotion());
        }

        public void Update()
        {
            return;
        }

        public PlayerCombat(PlayerStats playerStats)
        {
            this.playerStats = playerStats;
        }

        public int GetFatigue()
        {
            return playerStats.GetStat(Stat.FatiguePoints);
        }

        public int GetHitpoints()
        {
            return playerStats.GetStat(Stat.Hitpoints);
        }

        public int GetMaxFatigue()
        {
            return playerStats.GetStat(Stat.MaxFatiguePoints);
        }

        public int GetMaxHitpoints()
        {
            return playerStats.GetStat(Stat.MaxHitpoints);
        }

        public string GetName()
        {
            return "Player";
        }

        public int GetStat(Stat stat)
        {
            return playerStats.GetStat(stat);
        }

        public void Heal(int heal)
        {
            playerStats.Heal(heal);
        }

        public bool IsDead()
        {
            return playerStats.isDead;
        }

        public void SetCurrentEnemy(ICombatant combatant)
        {
            enemy = combatant;
        }

        public void TakeDamage(int damage)
        {
            playerStats.TakeDamage(damage);
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

        internal List<string> GetSkills()
        {
            List<string> skillNames = new List<string>();
            foreach (ISkill skill in skills)
            {
                skillNames.Add(skill.GetName());
            }
            return skillNames;
        }

        internal List<string> GetAttacks()
        {
            List<string> attackNames = new List<string>();
            foreach (ISkill attack in attacks)
            {
                attackNames.Add(attack.GetName());
            }
            return attackNames;
        }

        public List<string> GetItems()
        {
            List<string> itemNames = new List<string>();
            foreach (IItem item in items)
            {
                itemNames.Add(item.GetName());
            }
            return itemNames;
        }

        public void DoAttack(int index)
        {
            //try to preform attack, catch if out of range
            try
            {
                if (attacks[index].IsSuccessful(this, enemy))
                    attacks[index].OnSuccess(this, enemy);
                else
                    attacks[index].OnFail(this, enemy);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("ERROR: Attempted to preform out of bounds attack at Index: " + index);
                return;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("ERROR: Attempted to attack an enemy that doesnt exist.");
                return;
            }
            Encounter.CurrentEncounter.EndPlayerTurn();
        }

        public void UseSkill(int index)
        {
            //try to preform attack, catch if out of range
            try
            {
                if (skills[index].IsSuccessful(this, enemy))
                    skills[index].OnSuccess(this, enemy);
                else
                    skills[index].OnFail(this, enemy);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("ERROR: Attempted to preform out of bounds skill at Index: " + index);
                return;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("ERROR: Attempted to use skill on an enemy that doesnt exist.");
                return;
            }
            Encounter.CurrentEncounter.EndPlayerTurn();
        }

        public void UseItem(int index)
        {
            try
            {
                items[index].Use(this);
                items.RemoveAt(index);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("ERROR: Attempted to access out of bounds item: " + index);
                return;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("ERROR: Attempted to use non-existent item.");
                return;
            }

            Player.Instance.GetComponent<PlayerControls>().ForceUpdateCombatList();
        }

        public void AddEffect(IEffect effect)
        {
            effect.OnApply(this);
            activeEffects.Add(effect);
            if (effect.Update(this))
                effect.OnRemove(this);
        }

        public void AddItem(IItem item)
        {
            items.Add(item);
        }
    }
}