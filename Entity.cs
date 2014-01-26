using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace Destroy.All.Hippies
{
    class GameEntity
    {
        //Base class for all entities in game
        //Attributes
        private Vector3 mPosition;
        Vector3 mDirection = new Vector3(-1, 0, 0);
        private Vector3 mScale;
        private SceneNode mSceneNode;
        private string mMaterialName;
        private string mMeshName;
        private string mGameName;

        public float MoveRate { get; set; }

        //Mogre attributes
        private Entity mEntity;

        public GameEntity(string name, string meshName, string matName)
        {
            mGameName = name;
            mMeshName = meshName;
            mMaterialName = matName;
        }

        public void init(SceneManager sceneMgr)
        {
            //Create player billboards
            mSceneNode = sceneMgr.CreateSceneNode(mGameName+"Node");
            mEntity = sceneMgr.CreateEntity(mGameName+"Entity", mMeshName);
            mEntity.SetMaterialName(mMaterialName);
            mSceneNode.AttachObject(mEntity);
            mSceneNode.SetScale(mScale);

            MoveRate = 50;
        }

        public void SetMaterial(string matName)
        {
            mMaterialName = matName;
            mEntity.SetMaterialName(mMaterialName);
        }

        public void SetScale(Vector3 scale)
        {
            mSceneNode.SetScale(scale);
        }

        public void SetPosition(Vector3 pos)
        {
            mPosition = pos;
            mSceneNode.SetPosition(mPosition.x, mPosition.y, mPosition.z);
        }

        public void SetDirection(Vector3 dir)
        {
            mDirection = dir;
        }

        public Vector3 GetPosition()
        {
            return mPosition;
        }

        public void Update(float elapsedTime)
        {
            //Move by additional position
            mPosition += (mDirection * elapsedTime * MoveRate);
            mSceneNode.SetPosition(mPosition.x, mPosition.y, mPosition.z);
        }

        public SceneNode getGameNode()
        {
            return mSceneNode;
        }

        public bool Intersects(SceneNode objectNode)
        {
            Vector3 playerPos = objectNode.Position;
            Entity ent = (Entity)objectNode.GetAttachedObject(0);
            AxisAlignedBox bounds = ent.GetWorldBoundingBox();
            Vector3 dimensions = bounds.HalfSize;
            float playerLeft = playerPos.x - dimensions.x;
            float playerRight = playerPos.x + dimensions.x;
            float playerTop = playerPos.y + dimensions.y;
            float playerBottom = playerPos.y - dimensions.y;

            ent = (Entity)mSceneNode.GetAttachedObject(mGameName + "Entity");
            bounds = ent.GetWorldBoundingBox();
            dimensions = bounds.HalfSize;
            float entityLeft = mPosition.x - dimensions.x;
            float entityRight = mPosition.x + dimensions.x;
            float entityTop = mPosition.y + dimensions.y;
            float entityBottom = mPosition.y - dimensions.y;

            //See if they intersect
            return !(playerLeft > entityRight ||
                     playerRight < entityLeft ||
                     playerTop < entityBottom ||
                     playerBottom > entityTop);
        }
    }
}
