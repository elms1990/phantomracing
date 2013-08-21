using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    /// <summary>
    /// Simple rendering component.
    /// </summary>
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

        // Virtual width
        private int mWidth;

        // Virtual height
        private int mHeight;

        // Margins
        public int LeftMargin = 0;
        public int TopMargin = 0;

        public RenderComponent(GameObject parent, Texture2D texture)
            : base("Render")
        {
            mTexture = texture;
            mParent = parent;

            mWidth = mTexture.Width;
            mHeight = mTexture.Height;
        }

        public void SetWidth(int width)
        {
            mWidth = width;
        }

        public void SetHeight(int height)
        {
            mHeight = height;
        }

        public int GetWidth()
        {
            return mWidth;
        }

        public int GetHeight()
        {
            return mHeight;
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

            mRectangle.X = (int)(LeftMargin + mTransform.Position.X + (2 * Center.X * mTransform.Scale.X) / 2);
            mRectangle.Y = (int)(TopMargin + mTransform.Position.Y + (2 * Center.Y * mTransform.Scale.Y) / 2);
            mRectangle.Width = (int)(mWidth * mTransform.Scale.X);
            mRectangle.Height = (int)(mHeight * mTransform.Scale.Y);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mRectangle, null, Color.White,
                mTransform.Rotation, Center, SpriteEffects.None, 0);
        }
    }
}
