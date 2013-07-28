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

        public PlayerInputComponent(Player parent)
            : base("PlayerInput")
        {
            mParent = parent;
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
            KeyboardState ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.A))
            {
                mTransform.Position.X -= mParent.Speed.X * timeStep;
            }
            if (ks.IsKeyDown(Keys.D))
            {
                mTransform.Position.X += mParent.Speed.X * timeStep;
            }
            if (ks.IsKeyDown(Keys.W))
            {
                mTransform.Position.Y -= mParent.Speed.Y * timeStep;
            }
            if (ks.IsKeyDown(Keys.S))
            {
                mTransform.Position.Y += mParent.Speed.Y * timeStep;
            }
            if (ks.IsKeyDown(Keys.Left))
            {
                mTransform.Rotation -= mParent.RotationSpeed * timeStep;
            }
            if (ks.IsKeyDown(Keys.Right))
            {
                mTransform.Rotation += mParent.RotationSpeed * timeStep;
            }
            if (ks.IsKeyDown(Keys.Up) && !mShot)
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
