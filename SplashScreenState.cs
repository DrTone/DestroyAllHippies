using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Destroy.All.Hippies
{
    class SplashScreenState : IGameObject
    {
        //StateSystem mSystem;
        SceneManager mSceneMgr;
        string mStateName = "splash";
        double mDelayInSeconds = 6;
        bool mInit = false;

        public SplashScreenState(SceneManager sceneMgr)
        {
            mSceneMgr = sceneMgr;
        }

        public bool Init { get; set; }

        #region IGameObject Members

        public bool Update(double elapsedTime, MOIS.Keyboard keyState)
        {
            mDelayInSeconds -= elapsedTime;
            if (mDelayInSeconds <= 0)
            {
                mDelayInSeconds = 0;
                System.Console.WriteLine("Splash state finished");
                return true;
            }

            return false;
        }

        public void init()
        {
            //Load splash screen overlay
            //Nodes are children of mainNode
            if (Init) return;

            //Create state node - attache everything to this
            SceneNode node = mSceneMgr.GetSceneNode("mainNode").CreateChildSceneNode(mStateName+"Node");

            //Splash screen
            var overlay = OverlayManager.Singleton.Create("SplashOverlay");

            // Create a panel.
            var panel = (PanelOverlayElement)OverlayManager.Singleton.CreateOverlayElement("Panel", "PanelElement");
            // Set panel properties.
            panel.MaterialName = "Hippies/SplashScreen";
            panel.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            panel.Top = 0;
            panel.Left = 0;
            panel.Width = 1024;
            panel.Height = 768;

            // Add the panel to the overlay.
            overlay.Add2D(panel);

            // Make the overlay visible.
            overlay.Show();

            Init = true;
        }

        public void hideState()
        {
            //Stop state being shown
            OverlayManager.Singleton.GetByName("SplashOverlay").Hide();
        }

        #endregion
    }
}
