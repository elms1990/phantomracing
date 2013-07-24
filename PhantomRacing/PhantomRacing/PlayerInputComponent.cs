using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace PhantomRacing
{
    public class PlayerInputComponent : GameComponent
    {
        // Parent reference
        private GameObject mParent;

        // Parent TransformComponent reference
        private TransformComponent mTransform;

        public PlayerInputComponent(GameObject parent) : base("PlayerInput")
        {
            mParent = parent;
        }

        public override void Initialize()
        {
            mTransform = mParent.GetComponent<TransformComponent>("Transform");
        }

        public override void Update(int deltaTime)
        {
        }

        public override void Shutdown()
        {
            mParent = null;
            mTransform = null;
        }
    }
}
