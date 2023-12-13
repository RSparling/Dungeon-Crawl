using Dungeon_Crawl.src.PlayerCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.PlayerCore
{
    public class Player
    {
        private Dictionary<Type, IComponent> components = new Dictionary<Type, IComponent>();

        private static Player instance;

        public static Player Instance { get => instance; private set => instance = value; }

        public T GetComponent<T>() where T : IComponent
        {
            Type type = typeof(T);
            if (components.ContainsKey(type))
            {
                return (T)components[type];
            }
            return default(T);
        }

        public void AddComponent<T>(T component) where T : IComponent
        {
            Type type = typeof(T);
            if (!components.ContainsKey(type))
            {
                components[type] = component;
                component.Initialize();
            }
            else
            {
                // Handle the case where a component of this type already exists.
                // This could be an error, a replacement, or simply ignoring the add.
            }
        }

        public Player()
        {
            instance = this;
            PlayerStats playerStats = new PlayerStats(this);
            PlayerCombat playerCombat = new PlayerCombat(playerStats);
            PlayerMovement playerMovement = new PlayerMovement();
            PlayerUI playerUI = new PlayerUI(this);
            PlayerControls playerControls = new PlayerControls(this);
            AddComponent(playerStats);
            AddComponent(playerCombat);
            AddComponent(playerMovement);
            AddComponent(playerUI);
            AddComponent(playerControls);
        }
    }
}