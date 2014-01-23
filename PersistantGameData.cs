using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destroy.All.Hippies
{
    class PersistantGameData
    {
        public bool JustWon { get; set; }
        public LevelDescription CurrentLevel { get; set; }
        public PersistantGameData()
        {
            JustWon = false;
        }
    }

}
