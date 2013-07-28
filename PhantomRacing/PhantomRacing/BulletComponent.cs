using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class BulletComponent : GameComponent
    {
        // Alive bullets
        private List<Bullet> mBullets = new List<Bullet>();

        // Parent reference
        private GameObject mParent;

        // Parent Transform
        private TransformComponent mParentTransform;

        // Parent Render
        private RenderComponent mParentRender;

        // Remove list
        private LinkedList<Bullet> mRemove = new LinkedList<Bullet>();

        public BulletComponent(GameObject parent)
            : base("Bullet")
        {
            mParent = parent;
        }

        public override void Initialize()
        {
            base.Initialize();

            mParentTransform = (TransformComponent)mParent.GetComponent("Transform");
            mParentRender = (RenderComponent)mParent.GetComponent("Render");
        } 

        public override void Update(float timeStep)
        {
            // Clean bullets that are no longer used
            foreach (Bullet b in mRemove)
            {
                b.Shutdown();
                mBullets.Remove(b);
            }
            mRemove.Clear();

            foreach (Bullet b in mBullets)
            {
                b.Update(timeStep);
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            foreach (Bullet b in mBullets)
            {
                b.Render(spriteBatch);
            }
        }

        /// <summary>
        /// Spawn projectile in the world.
        /// </summary>
        public void SpawnBullet()
        {
            Bullet b = new Bullet(this);
            TransformComponent tc = new TransformComponent();
            RenderComponent rc = new RenderComponent(b, AssetLoader.GetInstance().LoadAsset<Texture2D>("bullet"));
            b.AddComponent(tc).
                AddComponent(rc);
            tc.Position.X = (float)(mParentTransform.Position.X + mParentRender.GetWidth() / 2 - rc.GetWidth() / 2
                - mParentRender.GetWidth() / 2 * Math.Sin(mParentTransform.Rotation));
            tc.Position.Y = (float)(mParentTransform.Position.Y + mParentRender.GetHeight() / 2 - rc.GetHeight() / 2
                + mParentRender.GetHeight() / 2 * Math.Cos(mParentTransform.Rotation));
            tc.Rotation = mParentTransform.Rotation;
            b.Initialize();
            mBullets.Add(b);
        }

        /// <summary>
        /// Mark a bullet for removal on next update.
        /// </summary>
        /// <param name="b">Bullet to be removed.</param>
        public void MarkRemoval(Bullet b)
        {
            mRemove.AddLast(b);
        }
    }
}
