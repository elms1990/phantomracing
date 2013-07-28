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

        public Player()
            : base("Player")
        {
        }

        public override void Initialize()
        {
            base.Initialize();
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
        }
    }
}
