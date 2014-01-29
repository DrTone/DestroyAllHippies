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
        uint mNumEnemies = 3;
        bool mEnemyDead = false;
        PersistantGameData mGameData;

        Player mPlayer;
        List<Enemy> mEnemies = new List<Enemy>();
        
        Vector3 mPlayerStartPos = new Vector3(-200, 0, 0);
        Vector3 mPlayerDiffPos = new Vector3();
        Vector3 mEnemyStartPos = new Vector3(200, -50, 0);
        Vector3 mEnemyDiffPos = new Vector3();

        WeaponManager mWeaponManager;
        EnemyManager mEnemyManager;

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

            //Level data
            mGameTime -= elapsedTime;
            if (mGameTime <= 0)
            {
                return true;
            }

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

            mPlayer.Update(mPlayerDiffPos);
            mWeaponManager.Update((float)elapsedTime);
            mEnemyManager.Update((float)elapsedTime);

            //See what is interacting
            for (int en = 0; en < mEnemyManager.GetNumberEnemies(); ++en)
            {
                if (mWeaponManager.Intersects(mEnemyManager.GetEnemy(en)))
                {
                    //Bullet hit enemy
                    mEnemyManager.KillEnemy(en);
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

            //Create enemies
            mEnemyManager = new EnemyManager(new RectangleF(-320 / 2, 768 / 2, 640, 768), mSceneMgr);
            mEnemyManager.SetPosition(mEnemyStartPos, 50);
            mEnemyManager.SetScale(objectScale);
            node.AddChild(mEnemyManager.GetEnemyNode());

            //Create weapons
            mWeaponManager = new WeaponManager(new RectangleF(-320 / 2, 768 / 2, 640, 768), mSceneMgr);
            objectScale.x = 0.3f;
            objectScale.y = 0.3f;
            objectScale.z = 0.001f;
            node.AddChild(mWeaponManager.GetWeaponNode());
            mWeaponManager.SetScale(objectScale);

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
