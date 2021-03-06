﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class HealthBar : GameObject
    {
        // Parent reference
        private GameObject mParent;

        // Max player life
        private int mMaxLife;

        // Player starting life
        private int mStartingLife;

        // Players current life
        private int mCurrentLife;

        private int mSavedStartingLife;

        // Transform object
        private TransformComponent mTransform;

        // Parent transform
        private TransformComponent mParentTransform;

        // Healthbar box renderer
        private RenderComponent mBoxRenderer;

        // Health renderer
        private RenderComponent mHealthRenderer;

        private Event mEvent;

        public HealthBar(GameObject parent, int maxLife, int startingLife)
            : base("HealthBar")
        {
            mParent = parent;
            mMaxLife = maxLife;
            mStartingLife = startingLife;
            mCurrentLife = startingLife;
            mSavedStartingLife = startingLife;

            mEvent = new Event();
        }

        public override void Initialize()
        {
            base.Initialize();

            mBoxRenderer = (RenderComponent)GetComponent("HealthBarBoxRender");
            mHealthRenderer = (RenderComponent)GetComponent("HealthBarFillRender");
            mTransform = (TransformComponent)GetComponent("Transform");
            mTransform.Position.Z = 1f;
            mParentTransform = (TransformComponent)mParent.GetComponent("Transform");
        }

        public override void Update(float timeStep)
        {
            base.Update(timeStep);

            mTransform.Position = mParentTransform.Position;
            mTransform.Scale = mParentTransform.Scale;
            mHealthRenderer.SetWidth((int) ((((float) mCurrentLife) / mMaxLife) * mBoxRenderer.GetWidth()));
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }

        public void TakeDamage(int dmg)
        {
            mCurrentLife -= dmg;

            if (mCurrentLife <= 0)
            {
                mCurrentLife = 0;
                mEvent.Sender = this;
                mEvent.Receiver = mParent;
                mEvent.EventName = "Killed";

                mParent.SendEvent(mEvent);
            }

            if (mCurrentLife > mMaxLife)
            {
                mCurrentLife = mMaxLife;
            }
        }

        public void Reset()
        {
            mCurrentLife = mSavedStartingLife;
        }
        
    }
}
