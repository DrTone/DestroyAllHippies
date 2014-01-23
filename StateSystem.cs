using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Destroy.All.Hippies
{
    class StateSystem
    {
        Dictionary<string, IGameObject> mStateStore = new Dictionary<string, IGameObject>();
        IGameObject mCurrentState = null;

        public bool Update(double elapsedTime, MOIS.Keyboard keyState)
        {
            if (mCurrentState == null)
            {
                return false; // nothing to process
            }
            return mCurrentState.Update(elapsedTime, keyState);
        }

        public void AddState(string stateId, IGameObject state)
        {
            System.Diagnostics.Debug.Assert(Exists(stateId) == false);
            mStateStore.Add(stateId, state);
        }

        public void ChangeState(string stateId)
        {
            if (mCurrentState != null)
            {
                mCurrentState.hideState();
            }
            System.Diagnostics.Debug.Assert(Exists(stateId));
            mCurrentState = mStateStore[stateId];
            mCurrentState.init();
        }

        /// <summary>
        /// Check if a state exists.
        /// </summary>
        /// <param name="stateId">The id of the state to check.</param>
        /// <returns>True for an existant state otherwise false</returns>
        public bool Exists(string stateId)
        {
            return mStateStore.ContainsKey(stateId);
        }
    }
}
