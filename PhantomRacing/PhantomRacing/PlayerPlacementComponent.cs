using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PhantomRacing
{
    public class PlayerPlacementComponent : GameComponent
    {
        // Parent reference
        private Player mParent = null;

        // Parent's transform component reference
        private TransformComponent mTransform = null;

        // Player index
        private int mPlayerIndex;

        private Vector2 mSpeed = new Vector2(150);

        private bool mPlaced = false;

        public PlayerPlacementComponent(Player parent, int index)
            : base("PlayerPlacement")
        {
            mParent = parent;
            mPlayerIndex = index;
        }

        public override void Initialize()
        {
            mTransform = (TransformComponent)mParent.GetComponent("Transform");
        }

        public override void Update(float timeStep)
        {
            if (!mPlaced)
            {
                // Input update
                InputState ks = KeyboardHandler.GetInstance().GetState();

                if (ks.IsPressed("p" + mPlayerIndex + "_left"))
                {
                    mTransform.Position.X += -mSpeed.X * timeStep;
                }
                if (ks.IsPressed("p" + mPlayerIndex + "_right"))
                {
                    mTransform.Position.X += mSpeed.X * timeStep;
                }
                if (ks.IsPressed("p" + mPlayerIndex + "_up"))
                {
                    mTransform.Position.Y += -mSpeed.Y * timeStep;
                }
                if (ks.IsPressed("p" + mPlayerIndex + "_down"))
                {
                    mTransform.Position.Y += mSpeed.Y * timeStep;
                }
                if (ks.IsPressed("p" + mPlayerIndex + "_rleft"))
                {
                    mTransform.Rotation -= mParent.RotationSpeed * timeStep;
                }
                if (ks.IsPressed("p" + mPlayerIndex + "_rright"))
                {
                    mTransform.Rotation += mParent.RotationSpeed * timeStep;
                }
                if (ks.IsJustPressed("p" + mPlayerIndex + "_shoot"))
                {
                    mPlaced = true;
                }
            }
        }

        public bool IsPlaced()
        {
            return mPlaced;
        }
    }
}
