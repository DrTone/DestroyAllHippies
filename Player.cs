using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Destroy.All.Hippies
{
    class Player : GameEntity
    {
        //Player attributes
        private const float PLAYER_MOVE_RATE = 100;

        //Mogre attributes

        public Player(string name, string meshName, string matName)
            : base(name, meshName, matName)
        {
           
        }

        public void init(SceneManager sceneMgr)
        {
            //Create player billboards
            base.init(sceneMgr);
            MoveRate = PLAYER_MOVE_RATE;
        }
    }
}
