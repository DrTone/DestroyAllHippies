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
        SceneManager mSceneMgr;
        Vector3 mDirection = new Vector3();
        RectangleF mPlayArea;

        public WeaponManager(RectangleF playArea, SceneManager sceneMgr, int numberWeapons=1)
        {
            mPlayArea = playArea;
            mNumWeapons = numberWeapons;
            mSceneMgr = sceneMgr;
            mManagerNode = mSceneMgr.CreateSceneNode("WeaponManagerNode");
            initWeapons();
        }

        public SceneNode GetWeaponNode()
        {
            return mManagerNode;
        }

        private void initWeapons()
        {
            //New weapons added to main manager node
            //All invisible in first instance
            for (int i = 0; i < mNumWeapons; ++i)
            {
                mWeapons.Add(new Weapon("Bullet" + i.ToString(), "cube.mesh", "Hippies/Bullet"));
                mWeapons[i].init(mSceneMgr);
                mManagerNode.AddChild(mWeapons[i].getGameNode());
                mWeapons[i].getGameNode().SetVisible(false);
            }
        }

        public void SetScale(Vector3 scale)
        {
            //Scale applies to all weapons in manager
            foreach (Weapon w in mWeapons)
            {
                w.SetScale(scale);
            }
        }

        public void Fire(Mogre.Vector3 pos)
        {
            if (mNumFiredWeapons >= mNumWeapons)
                return;

            mWeapons[mNumFiredWeapons].SetPosition(pos);
            mWeapons[mNumFiredWeapons].Fired = true;
            mWeapons[mNumFiredWeapons].getGameNode().SetVisible(true);
            ++mNumFiredWeapons;
            //DEBUG
            Console.WriteLine("Fired = {0}", mNumFiredWeapons);
        }

        public void Update(float elapsedTime)
        {
            //All weapons travel in same direction for now
            mDirection.x = elapsedTime;
            for (int i = 0; i < mNumWeapons; ++i)
            {
                if (mWeapons[i].Fired)
                {
                    mWeapons[i].Update(mDirection);
                    Vector3 pos = mWeapons[i].GetPosition();
                    if (pos.x >= mPlayArea.Right)
                    {
                        mWeapons[i].Fired = false;
                        mWeapons[i].getGameNode().SetVisible(false);
                        --mNumFiredWeapons;
                        //DEBUG
                        Console.WriteLine("Fired = {0}", mNumFiredWeapons);
                    }
                }   
            }
        }

        public bool Intersects(SceneNode objectNode)
        {
            for (int i = 0; i < mNumWeapons; ++i)
            {
                if (mWeapons[i].Fired && mWeapons[i].Intersects(objectNode))
                {
                    mWeapons[i].Fired = false;
                    mWeapons[i].getGameNode().SetVisible(false);
                    --mNumFiredWeapons;
                    //DEBUG
                    Console.WriteLine("Fired = {0}", mNumFiredWeapons);
                    return true;
                }
            }

            return false;
        }
    }
}
