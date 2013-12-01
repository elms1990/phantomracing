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

        // Physics component
        private PhysicsComponent mPhysics;

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
            mPhysics = (PhysicsComponent)GetComponent("Physics");

            mTransform.Position.Z = 0.6f;

            // Update speed vector
            Speed.X = - (float)(Speed.X * Math.Sin(mTransform.Rotation));
            Speed.Y = (float)(Speed.Y * Math.Cos(mTransform.Rotation));
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);

            mPhysics.Speed.X = Speed.X;
            mPhysics.Speed.Y = Speed.Y;

            // Remove bullet if out of screen.
            if (mTransform.Position.X + mRender.GetWidth() < 0 || mTransform.Position.X > Renderer.GetInstance().GetWidth() ||
                mTransform.Position.Y + mRender.GetHeight() < 0 || mTransform.Position.Y > Renderer.GetInstance().GetHeight())
            {
                mParent.MarkRemoval(this);
            }
        }

        public override void Shutdown()
        {
            base.Shutdown();

            mTransform = null;
            mParent = null;
            mRender = null;
        }

        protected override void OnEvent(Event e)
        {
            if (e.EventName.CompareTo("collision") == 0 ||
                e.EventName == "Reset")
            {
                mParent.MarkRemoval(this);
            }
        }
    }
}
