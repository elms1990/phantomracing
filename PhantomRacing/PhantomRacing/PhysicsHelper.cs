using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public static class PhysicsHelper
    {
        /// <summary>
        /// Checks if an object collided with another one.
        /// </summary>
        /// <param name="g1">Collider.</param>
        /// <param name="g2">Collided.</param>
        /// <returns>True, if the two objects collided. False, otherwise.</returns>
        public static bool TestCircleCollision(GameObject g1, GameObject g2)
        {
            return false;
        }

        /// <summary>
        /// Checks if an object collided with a pixel surface.
        /// </summary>
        /// <param name="g1">Collider.</param>
        /// <param name="region">A pixel region, where
        /// 0 means flat and 1 means collidable.</param>
        /// <returns>True if a collision occured. False, otherwise.</returns>
        public static bool TestPixelCollision(GameObject g1, int[] region)
        {
            return false;
        }
    }
}
