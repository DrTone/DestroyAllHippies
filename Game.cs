using Mogre;
using Mogre.TutorialFramework;
using System;


namespace Game
{
    class Hippies : BaseApplication
    {
        //State system
        string[] mStates = new string[2] { "splash", "title" };
        int mCurrentState = 0;
        StateSystem mStateSystem = new StateSystem();
        float mElapsedTime;

        public static void Main()
        {
            new Hippies().Go();
        }

        protected override bool Configure()
        {
            if (mRoot.RestoreConfig() || mRoot.ShowConfigDialog())
            {
                // If returned true, user clicked OK so initialise
                // Here we choose to let the system create a default rendering window by passing 'true'
                mWindow = mRoot.Initialise(true, "Hippies");

                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void CreateScene()
        {
            //Lighting
            mSceneMgr.AmbientLight = new ColourValue(1, 1, 1);

            //Add main node
            SceneNode node = mSceneMgr.RootSceneNode.CreateChildSceneNode("mainNode");

            //Add states
            mStateSystem.AddState(mStates[0], new SplashScreenState(mStates[0], mSceneMgr));
            mStateSystem.AddState(mStates[1], new TitleMenuState(mStates[1], mSceneMgr));

            mStateSystem.ChangeState(mStates[mCurrentState]);
        }

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameRenderingQueued += new FrameListener.FrameRenderingQueuedHandler(ProcessState);
            Overlay debug = OverlayManager.Singleton.GetByName("Core/DebugOverlay");
            //debug.Hide();
            OverlayElement stats = OverlayManager.Singleton.GetOverlayElement("Core/StatPanel");
            OverlayElement logo = OverlayManager.Singleton.GetOverlayElement("Core/LogoPanel");
            //stats.Hide();
            //logo.Hide();
            
        }

        bool ProcessState(FrameEvent evt)
        {
            if (mStateSystem.Update(evt.timeSinceLastFrame))
            {
                //Require state change
                mStateSystem.ChangeState(mStates[++mCurrentState]);
            }

            return true;
        }
    }
}