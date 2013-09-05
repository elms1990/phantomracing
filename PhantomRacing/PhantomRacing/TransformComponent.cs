using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class TransformComponent : GameComponent
    {
        // Object position, in pixel coordinates
        public Vector3 Position = new Vector3(0);

        // Scaling factor
        public Vector2 Scale = new Vector2(1);

        // Angle of rotation relative to its center.
        public float Rotation = 0;

        public TransformComponent()
            : base("Transform")
        {
        }
    }
}
