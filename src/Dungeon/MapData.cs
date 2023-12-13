using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.Dungeon
{
    internal class MapData
    {
        private int[,] map =
        {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 0, 0, 0, 0, 0, 1, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1,1,1},
        {1, 0, 0, 0, 0, 0, 0, 1, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1,1,1},
        {1, 0, 0, 1, 1, 0, 0, 1, 0, 0 , 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0,0,1},
        {1, 0, 0, 1, 1, 0, 0, 1, 0, 0 , 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0,0,1},
        {1, 0, 0, 0, 0, 0, 0, 1, 0, 1 , 1, 1, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0,0,1},
        {1, 0, 0, 0, 0, 0, 0, 1, 0, 0 , 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0,0,1},
        {1, 1, 1, 1, 0, 1, 1, 1, 0, 0 , 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0,0,1},
        {1, 0, 0, 0, 0, 1, 0, 1, 0, 0 , 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0,0,1},
        {1, 0, 0, 1, 0, 1, 0, 1, 0, 0 , 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 0, 0, 0,0,1},
        {1, 0, 0, 1, 0, 1, 0, 0, 0, 0 , 1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 1, 0, 0, 0,0,1},
        {1, 0, 0, 1, 0, 0, 0, 1, 0, 0 , 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0,0,1},
        {1, 1, 1, 1, 1, 1, 1, 1, 0, 0 , 0, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0,0,1},
        {1, 0, 0, 0, 0, 0, 0, 1, 0, 0 , 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0,0,1},
        {1, 0, 0, 1, 0, 1, 0, 0, 0, 0 , 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0,0,1},
        {1, 0, 0, 0, 0, 0, 0, 1, 0, 0 , 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 0, 0,0,1},
        {1, 0, 0, 1, 0, 1, 0, 1, 0, 0 , 1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0,0,1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        };

        private static MapData instance;

        public static MapData Get
        {
            get
            {
                if (instance == null)
                    instance = new MapData();
                return instance;
            }
        }

        public int GetTile(int x, int y)
        {
            if (IsInBounds(x, y))
            {
                return map[x, y];
            }
            else
            {
                return 1;
            }
        }

        public bool IsInBounds(int x, int y)
        {
            if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetSizeX()
        {
            return map.GetLength(0);
        }

        public int GetSizeY()
        {
            return map.GetLength(1);
        }

        public bool IsWalkable(int x, int y)
        {
            if (!IsInBounds(x, y))
                return false;

            if (map[x, y] == 0)
                return true;
            return false;
        }
    }
}