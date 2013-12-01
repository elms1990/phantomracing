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

        private Vector3 mSavedPosition = new Vector3(0);
        private Vector2 mSavedScale = new Vector2(1);
        private float mSavedRotation = 0;

        public TransformComponent()
            : base("Transform")
        {
        }

        public override void SaveState()
        {
            mSavedPosition.X = Position.X;
            mSavedPosition.Y = Position.Y;
            mSavedPosition.Z = Position.Z;
            mSavedScale.X = Scale.X;
            mSavedScale.Y = Scale.Y;
            mSavedRotation = Rotation;
        }

        public override void LoadState()
        {
            Position.X = mSavedPosition.X;
            Position.Y = mSavedPosition.Y;
            Position.Z = mSavedPosition.Z;
            Scale.X = mSavedScale.X;
            Scale.Y = mSavedScale.Y;
            Rotation = mSavedRotation;
        }
    }
}
