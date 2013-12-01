using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class LifeComponent : GameComponent
    {
        // Parent reference
        private GameObject mParent;

        // Health bar instance
        private HealthBar mHealthBar;

        // Max player life
        private int mMaxLife;

        // Player starting life
        private int mStartingLife;

        // Saved State
        private int mSavedStartingLife;
        private int mSavedMaxLife;

        public LifeComponent(GameObject parent, int maxLife, int startingLife)
            : base("Life")
        {
            mParent = parent;
            mMaxLife = maxLife;
            mStartingLife = startingLife;
        }

        public override void Initialize()
        {
            // Health bar object 
            mHealthBar = new HealthBar(mParent, mMaxLife, mStartingLife);

            // Create health box and fill render components
            RenderComponent hbox = new RenderComponent(mHealthBar, AssetLoader.GetInstance().LoadAsset<Texture2D>("health_bar_box"));
            RenderComponent hfill = new RenderComponent(mHealthBar, AssetLoader.GetInstance().LoadAsset<Texture2D>("health_bar_fill"));

            RenderComponent mParentRender = (RenderComponent)mParent.GetComponent("Render");

            // Draws on top of the player
            hbox.TopMargin = - (hbox.GetHeight() + 5);
            hbox.LeftMargin = (mParentRender.GetWidth() - hbox.GetWidth()) / 2;

            hfill.TopMargin = - (hbox.GetHeight() + 5);
            hfill.LeftMargin = (int) (mParentRender.GetWidth() - hbox.GetWidth()) / 2;
            hfill.SetHeight(hbox.GetHeight());

            hbox.SetId("HealthBarBoxRender");
            hfill.SetId("HealthBarFillRender");

            mHealthBar.AddComponent(new TransformComponent()).
                AddComponent(hbox).
                AddComponent(hfill);

            mHealthBar.Initialize();
        }

        public override void Update(float timeStep)
        {
            mHealthBar.Update(timeStep);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            mHealthBar.Render(spriteBatch);
        }

        /// <summary>
        /// Applies an amount of damage to the player.
        /// A negative amount means healing.
        /// </summary>
        /// <param name="dmg">Amount of health.</param>
        public void TakeDamage(int dmg)
        {
            mHealthBar.TakeDamage(dmg);
        }

        public override void SaveState()
        {
            mSavedMaxLife = mMaxLife;
            mSavedStartingLife = mStartingLife;
        }

        public override void LoadState()
        {
            mMaxLife = mSavedMaxLife;
            mStartingLife = mSavedStartingLife;
            mHealthBar.Reset();
        }
    }
}
