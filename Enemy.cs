using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Destroy.All.Hippies
{
    class Enemy : GameEntity
    {
        //Enemy attributes
        public bool Dead { get; set; }
        public Enemy(string name, string meshName, string matName)
            : base(name, meshName, matName)
        {
            Dead = false;
        }
    }
}
