using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Destroy.All.Hippies
{
    class Program
    {
        static void Main(string[] args)
        {
            Hippies app = new Hippies();
            app.Go();
        }
    }

    class Hippies : Mogre.ExampleApplication.Example
    {
        //State system
        List<string> mStateNames = new List<string>();
        int mCurrentState = 0;
        StateSystem mStateSystem = new StateSystem();
        PersistantGameData mGameData = new PersistantGameData();
        Player mPlayer = new Player("Hero", "spaceship.mesh", "Hippies/MainCharacter");

        public override bool Setup()
        {
            if (!base.Setup())
                return false;

            //Perform state setup
            InitializeGameData();
            InitializeGameState();

            LockCamera = true;

            return true;
        }

        public override void CreateScene()
        {
            //Lighting
            mSceneMgr.AmbientLight = new ColourValue(1, 1, 1);

            //Add main node
            SceneNode node = mSceneMgr.RootSceneNode.CreateChildSceneNode("mainNode");
        }

        private void InitializeGameState()
        {
            //State names
            mStateNames.Add("splash");
            mStateNames.Add("title");
            mStateNames.Add("play");
            mStateNames.Add("over");
            
            //Add states
            int state = 0;
            mStateSystem.AddState(mStateNames[state++], new SplashScreenState(mSceneMgr));
            mStateSystem.AddState(mStateNames[state++], new TitleMenuState(mSceneMgr));
            mStateSystem.AddState(mStateNames[state++], new GamePlayState(mSceneMgr, mGameData, mPlayer));
            mStateSystem.AddState(mStateNames[state++], new GameOverState(mSceneMgr));

            mStateSystem.ChangeState(mStateNames[mCurrentState]);
        }

        private void InitializeGameData()
        {
            LevelDescription level = new LevelDescription();
            level.Time = 50;
            mGameData.CurrentLevel = level;
        }

        public override void CreateFrameListener()
        {
            base.CreateFrameListener();
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(ProcessState);
        }

        bool ProcessState(FrameEvent evt)
        {
            if (window.IsClosed)
                return false;

            HandleInput(evt);

            if (mStateSystem.Update(evt.timeSinceLastFrame, inputKeyboard))
            {
                //Require state change
                mStateSystem.ChangeState(mStateNames[++mCurrentState]);
            }

            return !shutDown;
        }
    }
}
