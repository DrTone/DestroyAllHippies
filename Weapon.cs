using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Destroy.All.Hippies
{
    class Weapon : GameEntity
    {
        //Weapon attributes
        private const float WEAPON_MOVE_RATE = 200;

        public Weapon(string name, string meshName, string matName)
            : base(name, meshName, matName)
        {
        }

        public void init(SceneManager sceneMgr)
        {
            //Create player billboards
            base.init(sceneMgr);
            MoveRate = WEAPON_MOVE_RATE;
        }
    }
}
