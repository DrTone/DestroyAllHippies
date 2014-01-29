using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Mogre;

namespace Destroy.All.Hippies
{
    class EnemyManager
    {
        List<Enemy> mEnemies = new List<Enemy>();
        int mNumEnemies;
        RectangleF mPlayArea;
        SceneNode mManagerNode;
        Vector3 mDirection = new Vector3();
        SceneManager mSceneMgr;

        public EnemyManager(RectangleF playArea, SceneManager sceneMgr, int numberEnemies = 3)
        {
            mPlayArea = playArea;
            mNumEnemies = numberEnemies;
            mSceneMgr = sceneMgr;
            mManagerNode = mSceneMgr.CreateSceneNode("EnemyManagerNode");
            initEnemies();
        }

        public int GetNumberEnemies()
        {
            return mNumEnemies;
        }

        public SceneNode GetEnemyNode()
        {
            return mManagerNode;
        }

        public SceneNode GetEnemy(int en)
        {
            return mEnemies[en].getGameNode();
        }

        private void initEnemies()
        {
            //New enemies added to main manager node
            for (int i = 0; i < mNumEnemies; ++i)
            {
                mEnemies.Add(new Enemy("Hippy" + i.ToString(), "cube.mesh", "Hippies/HippyCharacter"));
                mEnemies[i].init(mSceneMgr);
                mManagerNode.AddChild(mEnemies[i].getGameNode());
            }
        }

        public void SetScale(Vector3 scale)
        {
            //Scale applies to all enemies in manager
            foreach (Enemy en in mEnemies)
            {
                en.SetScale(scale);
            }
        }

        public void SetPosition(Vector3 startPos, int offset)
        {
            //Set pos for first enemy then offset for rest
            for (int i = 0; i < mNumEnemies; ++i)
            {
                startPos.y += i * offset;
                mEnemies[i].SetPosition(startPos);
            }
        }

        public void Update(float elapsedTime)
        {
            //All enemies travel in same direction for now
            mDirection.x = -elapsedTime;
            for (int i = 0; i < mNumEnemies; ++i)
            {
                if (!mEnemies[i].Dead)
                {
                    mEnemies[i].Update(mDirection);
                }
            }
        }

        public void KillEnemy(int en)
        {
            mEnemies[en].Dead = true;
            mEnemies[en].SetMaterial("Hippies/Dead");
        }
    }
}
