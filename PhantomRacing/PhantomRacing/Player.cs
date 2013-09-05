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

        public Player(int index)
            : base("p" + index + "_player")
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            // Forces this object to be drawn on top of all others.
            (GetComponent("Transform") as TransformComponent).Position.Z = 0.5f;
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);
        }
        
        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }

        public override void Shutdown()
        {
            base.Shutdown();
        }

        protected override void OnEvent(Event e)
        {
            if (e.EventName.CompareTo("collision") == 0
                && ((String)e.Data).CompareTo("bullet") == 0)
            {
                ((LifeComponent)GetComponent("Life")).TakeDamage(5);
            }
        }
    }
}
