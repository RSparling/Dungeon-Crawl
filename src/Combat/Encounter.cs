using Dungeon_Crawl.src.Character;
using Dungeon_Crawl.src.Items;
using Dungeon_Crawl.src.PlayerCore;
using Dungeon_Crawl.src.PlayerCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Crawl.src.Combat
{
    public class Encounter
    {
        private Monster monster;

        public static Encounter CurrentEncounter
        {
            get { return instance; }
            private set { instance = value; }
        }

        public Encounter()
        { instance = this; }

        private static Encounter instance;

        public void StartCombat()
        {
            DungeonForm form = DungeonForm.ActiveForm as DungeonForm;
            PlayerCore.Player.Instance.GetComponent<PlayerControls>().ResetCombatSubmenu();
            PlayerCore.Player.Instance.GetComponent<PlayerUI>().PrepForCombat();

            monster = MonsterDictionary.Get.GetRandomMonster();

            Player.Instance.GetComponent<PlayerCombat>().SetCurrentEnemy(monster);
            monster.SetCurrentEnemy(Player.Instance.GetComponent<PlayerCombat>());
            form.NavigationUIVisible(false);
            form.CombatUIVisible(true);
            form.imageMonster = monster.image;
            form.RefreshMonsterImage();
            AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.sfx_StartFight);
            AudioManager.Get.StopMusic();
            AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.music_combat);
        }

        public async void EndPlayerTurn()
        {
            if (Player.Instance.GetComponent<PlayerCombat>().IsDead())
                EndCombat();
            // Code to end the player's turn
            Player.Instance.GetComponent<PlayerControls>().SuspendInput();
            // Wait for a specified time (e.g., 2 seconds)
            await Task.Delay(2000);

            // Start the monster's turn
            PerformMonstersTurn();
            monster.ProcessEffects();
        }

        private void PerformMonstersTurn()
        {
            if (monster.IsDead())
                EndCombat();
            monster.Attack();
            Player.Instance.GetComponent<PlayerControls>().AllowInput();
            Player.Instance.GetComponent<PlayerCombat>().ProcessEffects();
        }

        public void EndCombat()
        {
            DungeonForm form = DungeonForm.ActiveForm as DungeonForm;
            if (monster.IsDead())
            {
                PlayFanfare();
            }
            else if (Player.Instance.GetComponent<PlayerCombat>().IsDead())
            {
                AudioManager.Get.StopMusic();
                AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.music_GameOver);
                MessageBox.Show("You have died!");
                form.Close();
            }
            else
            {
                AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_RunAway);
            }
            form.NavigationUIVisible(true);
            form.CombatUIVisible(false);
            Player.Instance.GetComponent<PlayerControls>().AllowInput();
        }

        private async void RollLoot()
        {
            bool isLoot = Combat.Roll(3) < 12;

            AudioManager.Get.StopMusic();
            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_WinFanFare);
            MessageBox.Show("You won the battle!");
            await Task.Delay(1000);
            if (isLoot)
            {
                IItem item = ItemDictionary.GetCopyOfRandomItem();
                Player.Instance.GetComponent<PlayerCombat>().AddItem(item);
                AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_WinFanFare);
                MessageBox.Show("You have found: " + item.GetName() + "!");
            }
            AudioManager.Get.StopMusic();
            AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.music_exploration);
        }

        private async void PlayFanfare()
        {
            AudioManager.Get.StopMusic();

            AudioManager.Get.PlaySoundEffect(AudioManager.SoundLibrary.sfx_WinFanFare);
            await Task.Delay(1000);
            AudioManager.Get.StopMusic();
            AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.music_exploration);
            RollLoot();
        }
    }
}