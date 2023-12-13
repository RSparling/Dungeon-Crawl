using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_Crawl.src.PlayerCore
{
    public interface IComponent
    {
        void Initialize();

        void Update();
    }
}