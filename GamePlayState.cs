using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Drawing;

namespace Destroy.All.Hippies
{
    class GamePlayState : IGameObject
    {
        string mStateName = "play";
        SceneManager mSceneMgr;
        private uint mNumEnemies = 3;
        private bool mEnemyDead = false;
        PersistantGameData mGameData;

        Player mPlayer;
        List<Enemy> mEnemies = new List<Enemy>();
        private Weapon mWeapon;

        private Vector3 mPlayerStartPos = new Vector3(-200, 0, 0);
        private Vector3 mPlayerDiffPos = new Vector3();
        private Vector3 mEnemyStartPos = new Vector3(200, -50, 0);
        private Vector3 mEnemyDiffPos = new Vector3();

        WeaponManager mWeaponManager;

        double mGameTime;

        public GamePlayState(SceneManager sceneMgr, PersistantGameData gameData, Player player)
        {
            mSceneMgr = sceneMgr;
            mGameData = gameData;
            mPlayer = player;
        }

        public bool Update(double elapsedTime, MOIS.Keyboard keyState)
        {
            //Reset diffs
            mPlayerDiffPos = Vector3.ZERO;
            mEnemyDiffPos = Vector3.ZERO;

            //Level data
            mGameTime -= elapsedTime;
            if (mGameTime <= 0)
            {
                return true;
            }

            //Move enemy
            if(!mEnemyDead)
                mEnemyDiffPos.x = -(float)elapsedTime;

            if (keyState.IsKeyDown(MOIS.KeyCode.KC_W))
            {
                mPlayerDiffPos.y = (float)elapsedTime;
            }

            if (keyState.IsKeyDown(MOIS.KeyCode.KC_S))
            {
                mPlayerDiffPos.y = -(float)elapsedTime;
            }

            if (keyState.IsKeyDown(MOIS.KeyCode.KC_RETURN))
            {
                //Fire weapon
                mWeaponManager.Fire(mPlayer.GetPosition());
            }

            for (int en = 0; en < mNumEnemies; ++en)
            {
                mEnemies[en].Update(mEnemyDiffPos);
            }
            mPlayer.Update(mPlayerDiffPos);
            mWeaponManager.Update((float)elapsedTime);

            //See what is interacting
            for (int en = 0; en < mNumEnemies; ++en)
            {
                if (mWeaponManager.Intersects(mEnemies[en].getGameNode()))
                {
                    //Bullet hit enemy
                    mEnemyDead = true;
                    mEnemies[en].SetMaterial("Hippies/Dead");
                }
            }

            return false;
        }

        public void init()
        {
            //Nodes are children of mainNode
            if (Init) return;
            
            //Create state top node
            SceneNode node = mSceneMgr.GetSceneNode("mainNode").CreateChildSceneNode(mStateName + "Node");

            //Create our player
            Vector3 objectScale;
            objectScale.x = 0.5f;
            objectScale.y = 0.5f;
            objectScale.z = 0.001f;
            mPlayer.init(mSceneMgr);
            node.AddChild(mPlayer.getGameNode());
            mPlayer.SetPosition(mPlayerStartPos);
            mPlayer.SetScale(objectScale);
            mPlayerDiffPos = Vector3.ZERO;

            //Create enemies
            Vector3 startPos = mEnemyStartPos;
            for (int i = 0; i < mNumEnemies; ++i)
            {
                mEnemies.Add(new Enemy("Hippy" + i.ToString(), "cube.mesh", "Hippies/HippyCharacter"));
                mEnemies[i].init(mSceneMgr);
                node.AddChild(mEnemies[i].getGameNode());
                startPos.y += i * 40;
                mEnemies[i].SetPosition(startPos);
                mEnemies[i].SetScale(objectScale);
            }
            mEnemyDiffPos = Vector3.ZERO;

            //Create weapons
            mWeaponManager = new WeaponManager(new RectangleF(-1024 / 2, 768 / 2, 1024, 768), mSceneMgr);
            objectScale.x = 0.3f;
            objectScale.y = 0.3f;
            objectScale.z = 0.001f;
            mWeapon = new Weapon("Bullet", "cube.mesh", "Hippies/Bullet");
            mWeapon.init(mSceneMgr);
            node.AddChild(mWeapon.getGameNode());
            mWeapon.SetPosition(mPlayerStartPos);
            mWeapon.SetScale(objectScale);

            //Level data
            mGameTime = mGameData.CurrentLevel.Time;
            Init = true;
        }

        public bool Init { get; set; }

        public void hideState()
        {
            mSceneMgr.GetSceneNode(mStateName + "Node").SetVisible(false);
        }
    }
}
