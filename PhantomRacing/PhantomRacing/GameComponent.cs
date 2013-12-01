using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class GameComponent
    {
        // Component id
        private String mId;

        public GameComponent(String id)
        {
            mId = id;
        }

        public String GetId()
        {
            return mId;
        }

        public void SetId(String id)
        {
            mId = id;
        }

        public virtual void Initialize()
        {
        }

        public virtual void Update(float timeStep)
        {
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
        }

        public virtual void Shutdown()
        {
        }

        public virtual void SaveState()
        {

        }

        public virtual void LoadState()
        {

        }
    }
}
