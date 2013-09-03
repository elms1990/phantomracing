using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public static class PhysicsHelper
    {
        // Collider center vector
        private static Vector2 sCenterVec = new Vector2();

        // Collided center vector
        private static Vector2 sCenterVec2 = new Vector2();

        public static bool Collide(GameObject collider, CollisionType colliderType, 
            GameObject collided, CollisionType collidedType)
        {
            switch (colliderType)
            {
                case CollisionType.Circle:
                    if (collidedType == CollisionType.Circle)
                    {
                        TransformComponent colliderTransform = (TransformComponent)collider.GetComponent("Transform");
                        RenderComponent colliderRender = (RenderComponent)collider.GetComponent("Render");
                        TransformComponent collidedTransform = (TransformComponent)collided.GetComponent("Transform");
                        RenderComponent collidedRender = (RenderComponent)collided.GetComponent("Render");

                        return TestCircleCircleCollision((int)(colliderTransform.Position.X + colliderRender.GetWidth() / 2),
                            (int)(colliderTransform.Position.Y + colliderRender.GetHeight() / 2),
                            colliderRender.GetWidth() / 2,
                            (int)(collidedTransform.Position.X + collidedRender.GetWidth() / 2),
                            (int)(collidedTransform.Position.Y + collidedRender.GetHeight() / 2),
                            collidedRender.GetWidth() / 2);
                    }
                    break;   
            }

            return false;
        }

        public static bool Collide(GameObject collider, int[] field)
        {
            return false;
        }

        private static bool TestCircleCircleCollision(int collider_center_x, int collider_center_y, int collider_radius,
            int collided_center_x, int collided_center_y, int collided_radius)
        {
            sCenterVec.X = collider_center_x;
            sCenterVec.Y = collider_center_y;
            sCenterVec2.X = collided_center_x;
            sCenterVec2.Y = collided_center_y;

            float dist;

            Vector2.Distance(ref sCenterVec, ref sCenterVec2, out dist);

            return dist <= collider_radius + collided_radius;
        }
    }
}
