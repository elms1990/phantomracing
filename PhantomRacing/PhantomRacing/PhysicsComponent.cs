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

        // Collider rect
        private Rectangle mCollider = new Rectangle();

        // Collision type
        private CollisionType mType;

        // Collision group
        private String mGroup = "";

        // Collides with
        private List<String> mCollidesWith = new List<string>();

        // Collision event
        private Event mEvent;

        // Velocity
        public Vector2 Speed = new Vector2();

        public PhysicsComponent(GameObject parent, CollisionType type,
            String group, List<String> collidesWith)
            : base("Physics")
        {
            mParent = parent;
            mType = type;

            mEvent = new Event();
            mEvent.Sender = mParent;

            mGroup = group;
            mCollidesWith = collidesWith;
        }

        public String GetGroup() {
            return mGroup;
        }

        public bool CollidesWith(String group)
        {
            return mCollidesWith.Contains(group);
        }

        public override void Initialize()
        {
            mTransform = (TransformComponent)mParent.GetComponent("Transform");
            mRender = (RenderComponent)mParent.GetComponent("Render");
        }

        public override void Update(float timeStep)
        {
            mTransform.Position.X += Speed.X * timeStep;
            mTransform.Position.Y += Speed.Y * timeStep;
            mRect.X = (int)mTransform.Position.X;
            mRect.Y = (int)mTransform.Position.Y;
            mRect.Width = mRender.GetWidth();
            mRect.Height = mRender.GetHeight();

            PhysicsHelper.TimeStep = timeStep;

            //Per-pixel collision test
            KinectManager kinect = KinectManager.GetInstance();
            Renderer renderer = Renderer.GetInstance();

            bool collided = PhysicsHelper.CheckPixelCollision(mParent, kinect.GetRawArena(),
               ((float)kinect.GetColorFrameWidth()) / renderer.GetWidth(),
                ((float)kinect.GetColorFrameHeight()) / renderer.GetHeight());

            if (collided)
            {
                if (mParent.GetId() == "Bullet")
                {
                    mEvent.EventName = "collision";
                    mEvent.Data = "objects";
                    mEvent.Receiver = mParent;
                    mParent.SendEvent(mEvent);
                }
                else
                {
                    if (mParent.GetId().Contains("player"))
                    {
                        mTransform.Position.X -= Speed.X * timeStep;
                        mTransform.Position.Y -= Speed.Y * timeStep;
                    }
                }
            }

            HashSet<GameObject> objs = World.GetInstance().GetRegistered();

            foreach (GameObject go in objs)
            {
                if (go != mParent)
                {
                    PhysicsComponent physics = (PhysicsComponent) go.GetComponent("Physics");
                    TransformComponent transform = (TransformComponent)go.GetComponent("Transform");
                    RenderComponent render = (RenderComponent)go.GetComponent("Render");

                    if (physics != null && CollidesWith(physics.GetGroup()))
                    {
                        mCollider.X = (int)transform.Position.X;
                        mCollider.Y = (int)transform.Position.Y;
                        mCollider.Width = render.GetWidth();
                        mCollider.Height = render.GetHeight();

                        if (PhysicsHelper.Collide(mParent, mType, go, physics.mType))
                        {
                            mEvent.EventName = "collision";
                            mEvent.Data = (Object)physics.GetGroup();
                            mEvent.Receiver = mParent;
                            mParent.SendEvent(mEvent);

                            mEvent.Receiver = go;
                            mEvent.Data = (Object)GetGroup();
                            go.SendEvent(mEvent);
                        }
                    }
                }
            }
        }
    }
}
