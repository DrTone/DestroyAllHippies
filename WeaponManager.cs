using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mogre;

namespace Destroy.All.Hippies
{
    class WeaponManager
    {
        List<Weapon> mWeapons = new List<Weapon>();
        int mNumWeapons;
        int mNumFiredWeapons = 0;
        SceneNode mManagerNode;

        public WeaponManager(RectangleF playArea, SceneManager sceneMgr, int numberWeapons=1)
        {
            mNumWeapons = numberWeapons;
            mManagerNode = sceneMgr.CreateSceneNode("WeaponManagerNode");
            initWeapons();
        }

        private void initWeapons()
        {
            for (int i = 0; i < mNumWeapons; ++i)
            {
                mWeapons.Add(new Weapon("Bullet" + i.ToString(), "cube.mesh", "Hippies/Bullet"));
                mManagerNode.AddChild(mWeapons[i].getGameNode());
            }
        }

        public void Fire(Mogre.Vector3 pos)
        {
            if (mNumFiredWeapons >= mNumWeapons)
                return;

            mWeapons[mNumFiredWeapons].SetPosition(pos);
            mWeapons[mNumFiredWeapons].Fired = true;
            ++mNumFiredWeapons;
        }

        public void Update(float elapsedTime)
        {
            for (int i = 0; i < mNumWeapons; ++i)
            {
                if (mWeapons[i].Fired)
                    mWeapons[i].Update(elapsedTime);
            }
        }

        public bool Intersects(SceneNode objectNode)
        {
            for (int i = 0; i < mNumWeapons; ++i)
            {
                if (mWeapons[i].Intersects(objectNode))
                    return true;
            }

            return false;
        }
    }
}
