using Dungeon_Crawl.Properties;
using Dungeon_Crawl.src;
using Dungeon_Crawl.src.PlayerCore;
using Dungeon_Crawl.src.PlayerCore.Components;
using Dungeon_Crawl.src.Dungeon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dungeon_Crawl.src.Combat;
using Dungeon_Crawl.src.Character;

namespace Dungeon_Crawl
{
    public partial class DungeonForm : Form
    {
        private Player player = new Player();
        private MapData map = new MapData();
        private DungeonRenderer dungeonRenderer;
        private EncounterGenerator encounterGenerator = new EncounterGenerator();
        private MonsterDictionary monsterDictionary = new MonsterDictionary();

        public Image imageMonster;

        public DungeonForm()
        {
            InitializeComponent();

            dungeonRenderer = new DungeonRenderer(imagePlane_DungeonEnviroment.Width, imagePlane_DungeonEnviroment.Height, Resources.DungeonWall);

            DoubleBuffered = true; //preven flickering
            imagePlane_DungeonEnviroment.Paint += RenderDungeonBackground;

            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            movement.OnTurn += SetCompassDirection;
            movement.OnViewChanged += dungeonRenderer.UpdateViewData;

            movement.OnMove += encounterGenerator.PlayerStep;

            CombatUIVisible(false);
            NavigationUIVisible(false);
            movement.UpdateRendererView();
            SetupPlayerControls();

            //AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.music_exploration);
            pictureBox_MonsterSprite.Paint += DrawMonsterImage;
            imagePlane_DungeonEnviroment.Visible = false;
            pictureBox_TitleScreen.Image = Resources.Title_Screen;
            button_PlayGame.Click += StartGame;
            AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.music_TitleScreen);
            button_PlayGame.BringToFront();
        }

        private void SetupPlayerControls()
        {
            PlayerControls controls = player.GetComponent<PlayerControls>();
            //navigation controls
            button_MoveBack.Click += new EventHandler(controls.OnNavigationButtonClick);
            button_MoveForward.Click += new EventHandler(controls.OnNavigationButtonClick);
            button_MoveLeft.Click += new EventHandler(controls.OnNavigationButtonClick);
            button_MoveRight.Click += new EventHandler(controls.OnNavigationButtonClick);

            button_TurnLeft.Click += new EventHandler(controls.OnNavigationButtonClick);
            button_TurnRight.Click += new EventHandler(controls.OnNavigationButtonClick);

            //Combat Controls
            button_Skill.Click += new EventHandler(controls.OnCombatSubMenuButtonClick);
            button_Attack.Click += new EventHandler(controls.OnCombatSubMenuButtonClick);
            button_Run.Click += new EventHandler(controls.OnCombatSubMenuButtonClick);
            button_Item.Click += new EventHandler(controls.OnCombatSubMenuButtonClick);
            //option selection
            CombatSubMenu.DoubleClick += new EventHandler(controls.OnCombatSubMenuOptionDoubleClick);
        }

        //Render Functions
        public void InvalidateDungeonBackground()
        {
            imagePlane_DungeonEnviroment.Invalidate();
        }

        private void RenderDungeonBackground(object sender, PaintEventArgs e)
        {
            Bitmap frame = dungeonRenderer.RenderFrame();
            e.Graphics.DrawImage(frame, 0, 0);
        }

        //Navigation UI Functions
        public void NavigationUIVisible(bool setVisible)
        {
            if (setVisible)
            {
                groupNavigation.Show();
            }
            else
            {
                groupNavigation.Hide();
            }
        }

        public void SetCompassDirection(int direction)
        {
            Image image = Resources.DungeonWall;
            switch (direction)
            {
                case 0:
                    image = Resources.image_CompassNorth;
                    return;

                case 1:
                    image = Resources.image_CompassEast;
                    break;

                case 2:
                    image = Resources.image_CompassSouth;
                    break;

                case 3:
                    image = Resources.image_CompassWest;
                    break;

                default:
                    image = Resources.DungeonWall;
                    break;
            }
            picture_Compass.BackgroundImage = image;
        }

        //Combat UI
        private void DrawMonsterImage(object sender, PaintEventArgs pe)
        {
            // Draw the image
            if (imageMonster != null)
            {
                // Adjust these parameters as needed
                pe.Graphics.DrawImage(imageMonster, new Point(0, 0));
            }
        }

        public void RefreshMonsterImage()
        {
            pictureBox_MonsterSprite.Invalidate();
        }

        public void SetHPText(int getHitpoints, int getMaxHitpoints)
        {
            textbox_HitPoints.Clear();

            textbox_HitPoints.Text = getHitpoints + "/" + getMaxHitpoints;
        }

        public void SetHPBarPercentage(float percent)
        {
            if (percent < 0)
                percent = 0;
            if (percent > 100)
                percent = 100;

            bar_Player_HealthBar.Value = (int)(100f * percent);
        }

        public int GetSelectedItem()
        {
            return CombatSubMenu.SelectedIndex;
        }

        public void SetSubmenuOptions(List<string> options)
        {
            CombatSubMenu.Items.Clear();
            CombatSubMenu.Items.AddRange(options.ToArray());
        }

        internal void SetCombatLog(List<string> combatLog)
        {
            string log = String.Empty;

            combatLog.Count();

            //add the last ten lines of the combatLog to log with the line number next to it with the most recent at the top
            for (int i = combatLog.Count() - 1; i >= 0; i--)
            {
                log += (combatLog.Count() - i) + ": " + combatLog[i] + "\r\n";
            }

            textbox_CombatLog.Text = log;
        }

        public void CombatUIVisible(bool setVisible)
        {
            if (setVisible)
            {
                groupCombatOptions.Show();
                groupPlayerVitals.Show();
                groupCombatPanel.Show();
                pictureBox_MonsterSprite.Show();
            }
            else
            {
                groupCombatOptions.Hide();
                groupPlayerVitals.Hide();
                groupCombatPanel.Hide();
                pictureBox_MonsterSprite.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void StartGame(object sender, EventArgs e)
        {
            AudioManager.Get.StopMusic();
            AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.music_exploration);
            pictureBox_TitleScreen.Visible = false;
            button_PlayGame.Visible = false;
            imagePlane_DungeonEnviroment.Visible = true;
            NavigationUIVisible(true);
        }

        private void DungeonForm_Load(object sender, EventArgs e)
        {
        }

        public void GameOver()
        {
            pictureBox_TitleScreen.Image = Resources.image_GameOver;
            AudioManager.Get.StopMusic();
            AudioManager.Get.PlayMusic(AudioManager.SoundLibrary.music_GameOver);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}