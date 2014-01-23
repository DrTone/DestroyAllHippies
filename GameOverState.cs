using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Destroy.All.Hippies
{
    class GameOverState : IGameObject
    {
        SceneManager mSceneMgr;
        string mStateName = "over";

        public GameOverState(SceneManager sceneMgr)
        {
            mSceneMgr = sceneMgr;
        }

        public bool Init { get; set; }

        public bool Update(double elapsedTime, MOIS.Keyboard keyState)
        {
            return false;
        }

        public void init()
        {
            //Load game over overlay
            //Nodes are children of mainNode
            if (Init) return;

            //Create state node - attache everything to this
            SceneNode node = mSceneMgr.GetSceneNode("mainNode").CreateChildSceneNode(mStateName+"Node");

            //Game over screen
            var overlay = OverlayManager.Singleton.Create("GameOverOverlay");
            // Create a panel.
            var panel = (PanelOverlayElement)OverlayManager.Singleton.CreateOverlayElement("Panel", "GameOverPanel");
            // Set panel properties.
            panel.MaterialName = "Hippies/GameOver";
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
            OverlayManager.Singleton.GetByName("GameOverOverlay").Hide();
        }
    }
}
