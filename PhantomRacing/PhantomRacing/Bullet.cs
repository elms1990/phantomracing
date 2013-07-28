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

        // Parent Component
        private BulletComponent mParent;

        public Bullet(BulletComponent parent) : base("Bullet")
        {
            mParent = parent;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Gets the reference of transform and render components
            mTransform = (TransformComponent)GetComponent("Transform");
            mRender = (RenderComponent)GetComponent("Render");
            
            // Update speed vector
            Speed.X = - (float)(Speed.X * Math.Sin(mTransform.Rotation));
            Speed.Y = (float)(Speed.Y * Math.Cos(mTransform.Rotation));
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);

            mTransform.Position.X += timeStep * Speed.X;
            mTransform.Position.Y += timeStep * Speed.Y;

            // Remove bullet if out of screen.
            if (mTransform.Position.X < 0 || mTransform.Position.X > Viewport.GetInstance().GetWidth() ||
                mTransform.Position.Y < 0 || mTransform.Position.Y > Viewport.GetInstance().GetHeight())
            {
                mParent.MarkRemoval(this);
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            mTransform = null;
            mParent = null;
            mRender = null;
        }
    }
}
