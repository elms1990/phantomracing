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
        // A reference to the owner of this game component.
        private GameObject mParent;

        // Reference to parent's TransformComponent.
        private TransformComponent mTransform;

        // Object's texture
        private Texture2D mTexture;

        // Object's point of rotation
        private Vector2 mCenter = new Vector2(0);

        // Destiny rectangle
        private Rectangle mDestiny = new Rectangle();

        public RenderComponent(GameObject parent, Texture2D texture)
            : base("Render")
        {
            mParent = parent;
            mTexture = texture;
        }

        public override void Initialize()
        {
            mTransform = mParent.GetComponent<TransformComponent>("Transform");
        }

        public override void Update(int deltaTime)
        {
            // Update center coordinates
            mCenter.X = (mTexture.Width * mTransform.Scale.X)/2;
            mCenter.Y = (mTexture.Height * mTransform.Scale.Y)/2;

            // Apply rotation and scaling 
            mDestiny.X = (int) (mTransform.Position.X + mCenter.X);
            mDestiny.Y = (int) (mTransform.Position.Y + mCenter.Y);
            mDestiny.Width = (int) (mTexture.Width * mTransform.Scale.X);
            mDestiny.Height = (int) (mTexture.Height * mTransform.Scale.Y);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mDestiny, null, Color.White, mTransform.Rotation,
                mCenter, SpriteEffects.None, 0);
        }

        public override void Shutdown()
        {
            mParent = null;
            mTransform = null;
            mTexture = null;
        }
    }
}
