using Dungeon_Crawl.Properties;
using Dungeon_Crawl.src.Combat;
using Dungeon_Crawl.src.Combat.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Character
{
    public class MonsterDictionary
    {
        private static MonsterDictionary instance;

        private List<Monster> monsterDictionary = new List<Monster>();

        public static MonsterDictionary Get
        {
            get
            {
                if (instance == null)
                    instance = new MonsterDictionary();
                return instance;
            }
        }

        public MonsterDictionary()
        {
            monsterDictionary.Add(new Monster(Resources.monster_Slime, "Slime", 4, 4, 4, 4, 6, 11, 7, 3, new ISkill[] { new AngryJiggle() }));
            monsterDictionary.Add(new Monster(Resources.monster_Demon, "Demon", 15, 15, 15, 15, 15, 14, 10, 4, new ISkill[] { new RiskySlash(), new FireBolt() }));
            monsterDictionary.Add(new Monster(Resources.monster_Skeleton, "Skeleton", 12, 12, 10, 10, 10, 8, 7, 4, new ISkill[] { new NormalSlash(), new HeavySlash(), new RiskySlash() }));
        }

        public Monster GetRandomMonster()
        {
            Random rand = new Random(Guid.NewGuid().ToString().GetHashCode());
            int roll = rand.Next(0, monsterDictionary.Count);

            return monsterDictionary[roll];
        }
    }
}