using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Destroy.All.Hippies
{
    class TitleMenuState : IGameObject
    {
        double mCurrentRotation = 0;
        string mStateName = "title";
        bool mInit = false;
        //StateSystem mSystem;
        SceneManager mSceneMgr;
        #region IGameObject Members

        public TitleMenuState(SceneManager sceneMgr)
        {
            mSceneMgr = sceneMgr;
        }

        public bool Update(double elapsedTime, MOIS.Keyboard keyState)
        {
            return keyState.IsKeyDown(MOIS.KeyCode.KC_RETURN);
        }

        public void init()
        {
            //Nodes are children of mainNode
            if (Init) return;

            SceneNode node = mSceneMgr.GetSceneNode("mainNode").CreateChildSceneNode(mStateName+"Node");
            var overlay = OverlayManager.Singleton.Create("TitleOverlay");

            // Create a panel.
            var panel = (PanelOverlayElement)OverlayManager.Singleton.CreateOverlayElement("Panel", "TitlePanel");
            // Set panel properties.
            panel.MaterialName = "Hippies/TitleScreen";
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

        public bool Init { get; set; }

        public void hideState()
        {
            OverlayManager.Singleton.GetByName("TitleOverlay").Hide();
        }

        #endregion
    }
}
