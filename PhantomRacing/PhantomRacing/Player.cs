using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PhantomRacing
{
    public class Player : GameObject
    {
        // Controller
        public PlayerIndex Index;

        // Speed (pixels / s)
        public Vector2 Speed = new Vector2(200);

        // Rotate ratio
        public float RotationSpeed = 5.0f;

        private TransformComponent mTransform;
        private RenderComponent mRender;
        private LifeComponent mLifeComponent;

        private bool mAlive;

        public Player(int index)
            : base("p" + index + "_player")
        {
            mAlive = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Forces this object to be drawn on top of all others.
            mTransform = (TransformComponent)GetComponent("Transform");
            mTransform.Position.Z = 0.5f;

            mRender = (RenderComponent)GetComponent("Render");
            mLifeComponent = (LifeComponent)GetComponent("Life");
        }

        public override void Update(float timeStep)
        {
            if (mAlive)
            {
                base.Update(timeStep);
            }
        }
        
        public override void Render(SpriteBatch spriteBatch)
        {
            if (mAlive)
            {
                base.Render(spriteBatch);
            }
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        public void SaveState()
        {
            foreach (GameComponent c in GetComponents())
            {
                c.SaveState();
            }
        }

        public void LoadState()
        {
            foreach (GameComponent c in GetComponents())
            {
                c.LoadState();
            }
        }

        public bool IsAlive()
        {
            return mAlive;
        }

        protected override void OnEvent(Event e)
        {
            if (e.EventName.CompareTo("collision") == 0
                && ((String)e.Data).CompareTo("bullet") == 0)
            {
                ((LifeComponent)GetComponent("Life")).TakeDamage(5);

                return;
            }

            if (e.EventName.CompareTo("Killed") == 0)
            {
                mAlive = false;
                mTransform.Position.X = -mRender.GetWidth();
                mTransform.Position.Y = -mRender.GetHeight();

                return;
            }

            if (e.EventName.CompareTo("Reset") == 0)
            {
                mAlive = true;
                LoadState();
                //mLifeComponent.LoadState();

                return;
            }

            if (e.EventName == "Save")
            {
                SaveState();

                return;
            }

            if (e.EventName == "Heal")
            {
                mLifeComponent.TakeDamage(-15);

                return;
            }
        }
    }
}
