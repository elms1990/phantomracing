using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public class PhysicsComponent : GameComponent
    {
        // Reference to parent object
        private GameObject mParent;

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
            
        }

        public override void Update(float timeStep)
        {
            
        }
    }
}
