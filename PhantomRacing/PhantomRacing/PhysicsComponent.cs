using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class PhysicsComponent : GameComponent
    {
        // Reference to parent object
        private GameObject mParent;

        // Parent transform
        private TransformComponent mTransform;

        // Parent render
        private RenderComponent mRender;

        // Re-usable rectangle
        private Rectangle mRect = new Rectangle();

        // Collision type
        private CollisionType mType;

        public PhysicsComponent(GameObject parent, CollisionType type)
            : base("Physics")
        {
            mParent = parent;
            mType = type;
        }

        public override void Initialize()
        {
            mTransform = (TransformComponent)mParent.GetComponent("Transform");
            mRender = (RenderComponent)mParent.GetComponent("Render");
        }

        public override void Update(float timeStep)
        {
            mRect.X = (int)mTransform.Position.X;
            mRect.Y = (int)mTransform.Position.Y;
            mRect.Width = mRender.GetWidth();
            mRect.Height = mRender.GetHeight();

            HashSet<GameObject> objs = World.GetInstance().QueueRegion(mRect);

            foreach (GameObject go in objs)
            {
                if (go != mParent)
                {
                    PhysicsComponent physics = (PhysicsComponent) go.GetComponent("Physics");

                    if (physics != null)
                    {
                       // if (g
                    }
                }
            }
        }
    }
}
