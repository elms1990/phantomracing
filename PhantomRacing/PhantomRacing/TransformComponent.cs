using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class TransformComponent : GameComponent
    {
        // Position of the object
        public Vector2 Position;

        // Scale vector of the object
        public Vector2 Scale;

        // Angle of Rotation
        public float Rotation;

        public TransformComponent()
            : base("Transform")
        {
            Position = new Vector2(0);
            Scale = new Vector2(1);
            Rotation = 0;
        }
    }
}
