using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destroy.All.Hippies
{
    interface IGameObject
    {
        bool Update(double elapsedTime, MOIS.Keyboard keyState);
        void init();
        void hideState();
        bool Init { get; set; }
    }
}
