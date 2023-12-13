using Dungeon_Crawl.src.Combat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dungeon_Crawl.src.PlayerCore.Components
{
    public class PlayerControls : IComponent
    {
        private Player player;
        private SubMenuState subMenuState = SubMenuState.attacks;
        private const string button_TurnLeft = "button_TurnLeft";
        private const string button_TurnRight = "button_TurnRight";
        private const string button_MoveForward = "button_MoveForward";
        private const string button_MoveBack = "button_MoveBack";
        private const string button_MoveLeft = "button_MoveLeft";
        private const string button_MoveRight = "button_MoveRight";

        private bool allowInput = true;

        public PlayerControls(Player player)
        {
            this.player = player;
        }

        public void Initialize()
        {
        }

        public void Update()
        {
        }

        public void OnNavigationButtonClick(object sender, EventArgs eventArgs)
        {
            if (!allowInput)
                return;
            Button button = (Button)sender;

            if (button == null)
                return;
            PlayerMovement movement = player.GetComponent<PlayerMovement>();
            switch (button.Name)
            {
                case button_TurnLeft:
                    movement.TurnLeft();
                    break;

                case button_TurnRight:
                    movement.TurnRight();
                    break;

                case button_MoveForward:
                    movement.MoveForward();
                    break;

                case button_MoveBack:
                    movement.MoveBackward();
                    break;

                case button_MoveLeft:
                    movement.MoveLeft();
                    break;

                case button_MoveRight:
                    movement.MoveRight();
                    break;
            }
        }

        //passes the index to the player to have the selected action preformed
        public void OnCombatSubMenuOptionDoubleClick(object sender, EventArgs e)
        {
            if (!allowInput)
                return;
            ListBox listView = sender as ListBox;
            if (listView == null)
                return;
            if (listView.SelectedItems.Count <= 0)
                return;
            // Get the first selected item (since MultiSelect is false)
            int index = listView.SelectedIndex;

            switch (subMenuState)
            {
                case SubMenuState.attacks:
                    player.GetComponent<PlayerCombat>().DoAttack(index);
                    return;

                case SubMenuState.skills:
                    player.GetComponent<PlayerCombat>().UseSkill(index);
                    return;

                case SubMenuState.items:
                    player.GetComponent<PlayerCombat>().UseItem(index);
                    return;

                default:
                    return;
            }
        }

        public void OnCombatSubMenuButtonClick(object sender, EventArgs eventArgs)
        {
            if (!allowInput)
                return;

            Button button = (Button)sender;
            DungeonForm form = Form.ActiveForm as DungeonForm;
            switch (button.Name)
            {
                case "button_Skill":
                    form.SetSubmenuOptions(player.GetComponent<PlayerCombat>().GetSkills());
                    subMenuState = SubMenuState.skills;
                    break;

                case "button_Attack":
                    form.SetSubmenuOptions(player.GetComponent<PlayerCombat>().GetAttacks());
                    subMenuState = SubMenuState.attacks;
                    break;

                case "button_Item":
                    form.SetSubmenuOptions(player.GetComponent<PlayerCombat>().GetItems());
                    subMenuState = SubMenuState.items;
                    break;

                case "button_Run":
                    if (Dungeon_Crawl.src.Combat.Combat.Roll(3) < player.GetComponent<PlayerCombat>().GetStat(Character.Stat.Agility))
                        Encounter.CurrentEncounter.EndCombat();
                    else
                        Encounter.CurrentEncounter.EndPlayerTurn();
                    return;

                default:
                    break;
            }
        }

        public void ForceUpdateCombatList()
        {
            DungeonForm form = Form.ActiveForm as DungeonForm;
            switch (subMenuState)
            {
                case SubMenuState.attacks:
                    form.SetSubmenuOptions(player.GetComponent<PlayerCombat>().GetAttacks());
                    return;

                case SubMenuState.skills:
                    form.SetSubmenuOptions(player.GetComponent<PlayerCombat>().GetSkills());
                    return;

                case SubMenuState.items:
                    form.SetSubmenuOptions(player.GetComponent<PlayerCombat>().GetItems());
                    return;

                default:
                    return;
            }
        }

        public void ResetCombatSubmenu()
        {
            DungeonForm form = Form.ActiveForm as DungeonForm;
            form.SetSubmenuOptions(player.GetComponent<PlayerCombat>().GetAttacks());
            subMenuState = SubMenuState.attacks;
        }

        public void SuspendInput()
        {
            allowInput = false;
        }

        public void AllowInput()
        {
            allowInput = true;
        }

        internal enum SubMenuState
        { attacks, skills, items }
    }
}