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

        // TimeStep
        public static float TimeStep = 0;

        public static bool Collide(GameObject collider, CollisionType colliderType, 
            GameObject collided, CollisionType collidedType)
        {
            bool collide = false;

            switch (colliderType)
            {
                case CollisionType.Circle:
                    if (collidedType == CollisionType.Circle)
                    {
                        TransformComponent colliderTransform = (TransformComponent)collider.GetComponent("Transform");
                        RenderComponent colliderRender = (RenderComponent)collider.GetComponent("Render");
                        PhysicsComponent colliderPhysics = (PhysicsComponent)collider.GetComponent("Physics");
                        TransformComponent collidedTransform = (TransformComponent)collided.GetComponent("Transform");
                        RenderComponent collidedRender = (RenderComponent)collided.GetComponent("Render");
                        PhysicsComponent collidedPhysics = (PhysicsComponent)collided.GetComponent("Physics");
                        
                        collide = TestCircleCircleCollision((int)(colliderTransform.Position.X + colliderRender.GetWidth() / 2),
                            (int)(colliderTransform.Position.Y + colliderRender.GetHeight() / 2),
                            colliderRender.GetWidth() / 2,
                            (int)(collidedTransform.Position.X + collidedRender.GetWidth() / 2),
                            (int)(collidedTransform.Position.Y + collidedRender.GetHeight() / 2),
                            collidedRender.GetWidth() / 2);

                        if (collide)
                        {
                            MoveOut(colliderTransform, colliderRender.GetWidth() / 2, colliderPhysics,
                                collidedTransform, collidedRender.GetWidth() / 2, collidedPhysics);
                        }
                    }
                    break;   
            }

            return collide;
        }

        public static bool CheckPixelCollision(GameObject collider, byte[] physicalObjects, float scaleX, float scaleY)
        {
            TransformComponent colliderTransform = (TransformComponent)collider.GetComponent("Transform");
            RenderComponent colliderRender = (RenderComponent)collider.GetComponent("Render");
            int scaledX = (int) (scaleX * colliderTransform.Position.X);
            int scaledY = (int) (scaleY * colliderTransform.Position.Y);
            int scaledW = (int)(scaleX * colliderRender.GetWidth());
            int scaledH = (int)(scaleY * colliderRender.GetHeight());
            int frameW = KinectManager.GetInstance().GetColorFrameWidth();
            int frameH = KinectManager.GetInstance().GetColorFrameHeight();
            Color[] pixelBuffer = colliderRender.GetPixelBuffer();

            // Prevents out of index issues, because out of window verification is
            // done after this step.
            if (scaledX < 0 || scaledY < 0 || scaledX >= frameW || scaledY >= frameH)
            {
                return false;
            }

            Console.Out.WriteLine(colliderRender.GetHeight());

            for (int j = 0; j < colliderRender.GetHeight() * 0.4; j++)
            {
                for (int i = 0; i < colliderRender.GetWidth() * 0.9; i++)
                {
                    if ((frameW * (scaledY + j) + scaledX + i < physicalObjects.Length) &&
                        physicalObjects[(int) (frameW * (scaledY + j) + scaledX + i * scaleX)] == 0
                        && pixelBuffer[(j * colliderRender.GetWidth() + i + 1) - 1].A > 0)
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        private static void MoveOut(TransformComponent colliderTransform, int colliderRadius,
            PhysicsComponent colliderPhysics,
            TransformComponent collidedTransform, int collidedRadius,
            PhysicsComponent collidedPhysics)
        {
            // Move back in time    
            colliderTransform.Position.X -= colliderPhysics.Speed.X * TimeStep;
            colliderTransform.Position.Y -= colliderPhysics.Speed.Y * TimeStep;
        }

        private static bool TestCircleCircleCollision(int colliderCenterX, int colliderCenterY, int colliderRadius,
            int collidedCenterX, int collidedCenterY, int collidedRadius)
        {
            sCenterVec.X = colliderCenterX;
            sCenterVec.Y = colliderCenterY;
            sCenterVec2.X = collidedCenterX;
            sCenterVec2.Y = collidedCenterY;

            float dist;

            Vector2.Distance(ref sCenterVec, ref sCenterVec2, out dist);

            return dist < colliderRadius + collidedRadius;
        }
    }
}
