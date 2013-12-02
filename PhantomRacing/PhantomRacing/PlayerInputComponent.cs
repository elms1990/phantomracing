using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PhantomRacing
{
    public class PlayerInputComponent : GameComponent
    {
        // Parent reference
        private Player mParent = null;

        // Parent's transform component reference
        private TransformComponent mTransform = null;

        // Physics component reference
        private PhysicsComponent mPhysics = null;

        // Bullet component
        private BulletComponent mBullet = null;

        // Reload time, in millisecs
        private int mReloadTime = 200;

        // Elapsed reload time
        private int mCurReloadTime = 0;

        // Flag signalizing that this player shot
        private bool mShot = false;

        // Player index
        private int mPlayerIndex;

        private Vector2 mSpeed = new Vector2(150);

        public PlayerInputComponent(Player parent, int index)
            : base("PlayerInput")
        {
            mParent = parent;
            mPlayerIndex = index;
        }

        public override void Initialize()
        {
            mTransform = (TransformComponent)mParent.GetComponent("Transform");
            mPhysics = (PhysicsComponent)mParent.GetComponent("Physics");
            mBullet = (BulletComponent)mParent.GetComponent("Bullet");
        }

        public override void Update(float timeStep)
        {
            mPhysics.Speed.X = 0;
            mPhysics.Speed.Y = 0;

            // Input update
            InputState ks = GamePadHandler.GetInstance().GetState((PlayerIndex)mPlayerIndex - 1);
            if (ks.IsPressed("p" + mPlayerIndex + "_left"))
            {
                mPhysics.Speed.X = -mSpeed.X;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_right"))
            {
                mPhysics.Speed.X = mSpeed.X;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_up"))
            {
                mPhysics.Speed.Y = -mSpeed.Y;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_down"))
            {
                mPhysics.Speed.Y = mSpeed.Y;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_rleft"))
            {
                mTransform.Rotation -= mParent.RotationSpeed * timeStep;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_rright"))
            {
                mTransform.Rotation += mParent.RotationSpeed * timeStep;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_shoot") && !mShot)
            {
                mBullet.SpawnBullet();
                mShot = true;
            }

            if (mShot)
            {
                mCurReloadTime += (int)(1000 * timeStep);
                if (mCurReloadTime >= mReloadTime)
                {
                    mCurReloadTime = 0;
                    mShot = false;
                }
            }
        }
    }
}
