using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class Bullet : GameObject
    {
        // Speed (pixels / s)
        public Vector2 Speed = new Vector2(650);

        // Transform component
        private TransformComponent mTransform;

        // Render Component
        private RenderComponent mRender;

        // Angle of displacement
        private float mRotation = 0;

        // Starting position of this bullet
        private float mStartingX;
        private float mStartingY;

        public Bullet(float x, float y, float rotation) : base("Bullet")
        {
            mRotation = rotation;
            mStartingX = x;
            mStartingY = y;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Gets the reference of transform and render components
            mTransform = (TransformComponent)GetComponent("Transform");
            mRender = (RenderComponent)GetComponent("Render");

            // Update starting position
            mTransform.Position.X = mStartingX;
            mTransform.Position.Y = mStartingY;
            
            // Update speed vector
            Speed.X = - (float)(Speed.X * Math.Sin(mRotation));
            Speed.Y = (float)(Speed.Y * Math.Cos(mRotation));
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);

            mTransform.Position.X += timeStep * Speed.X;
            mTransform.Position.Y += timeStep * Speed.Y;

            //// Remove bullet.
            //if (Position.X < 0 || Position.X > Game.graphics.GraphicsDevice.Viewport.Width ||
            //    Position.Y < 0 || Position.Y > Game.graphics.GraphicsDevice.Viewport.Height)
            //{
            //    Parent.RemoveBullet(this);
            //    Shutdown();
            //}
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }

    //    public override void Shutdown()
    //    {
    //        mBodyTexture = null;;
    //    }
    }
}
