using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class RenderComponent : GameComponent
    {
        // Parent reference
        private GameObject mParent;

        // Parent transform reference
        private TransformComponent mTransform;

        // Objects texture
        private Texture2D mTexture = null;

        // Bounding rectangle
        private Rectangle mRectangle = new Rectangle();

        // Center of object
        public Vector2 Center = new Vector2();

        public RenderComponent(GameObject parent, Texture2D texture)
            : base("Render")
        {
            mTexture = texture;
            mParent = parent;
        }

        public int GetWidth()
        {
            return mTexture.Width;
        }

        public int GetHeight()
        {
            return mTexture.Height;
        }

        public override void Initialize()
        {
            mTransform = (TransformComponent)mParent.GetComponent("Transform");
        }

        public override void Update(float timeStep)
        {
            // Update rotating axis position
            Center.X = mTexture.Width / 2;
            Center.Y = mTexture.Height / 2;

            // Update body position
            mRectangle.X = (int)(mTransform.Position.X + (2 * Center.X * mTransform.Scale.X) / 2);
            mRectangle.Y = (int)(mTransform.Position.Y + (2 * Center.Y * mTransform.Scale.Y) / 2);
            mRectangle.Width = (int)(2 * Center.X * mTransform.Scale.X);
            mRectangle.Height = (int)(2 * Center.Y * mTransform.Scale.Y);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            // Draw body
            spriteBatch.Draw(mTexture, mRectangle, null, Color.White,
                mTransform.Rotation, Center, SpriteEffects.None, 0);
        }
    }
}
