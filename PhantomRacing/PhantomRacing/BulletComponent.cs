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

        public void SpawnBullet()
        {
            Bullet b = new Bullet((float)(mParentTransform.Position.X + 
                mParentRender.GetWidth() / 2),
                (float)(mParentTransform.Position.Y + 
                mParentRender.GetHeight() / 2), mParentTransform.Rotation);
            b.AddComponent(new TransformComponent()).
                AddComponent(new RenderComponent(b, AssetLoader.GetInstance().LoadAsset<Texture2D>("bullet")));
            b.Initialize();
            mBullets.Add(b);
        }
    }
}
