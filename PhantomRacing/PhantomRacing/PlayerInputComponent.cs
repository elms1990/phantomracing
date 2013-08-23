using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace PhantomRacing
{
    public class PlayerInputComponent : GameComponent
    {
        // Parent reference
        private Player mParent = null;

        // Parent's transform component reference
        private TransformComponent mTransform = null;

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

        public PlayerInputComponent(Player parent, int index)
            : base("PlayerInput")
        {
            mParent = parent;
            mPlayerIndex = index;
        }

        public override void Initialize()
        {
            base.Initialize();

            mTransform = (TransformComponent)mParent.GetComponent("Transform");
            mBullet = (BulletComponent)mParent.GetComponent("Bullet");
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);

            // Input update
            InputState ks = KeyboardHandler.GetInstance().GetState();
            if (ks.IsPressed("p" + mPlayerIndex + "_left"))
            {
                mTransform.Position.X -= mParent.Speed.X * timeStep;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_right"))
            {
                mTransform.Position.X += mParent.Speed.X * timeStep;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_up"))
            {
                mTransform.Position.Y -= mParent.Speed.Y * timeStep;
            }
            if (ks.IsPressed("p" + mPlayerIndex + "_down"))
            {
                mTransform.Position.Y += mParent.Speed.Y * timeStep;
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
