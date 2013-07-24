using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public abstract class GameComponent
    {
        // UID of the component
        private String mId;

        /// <summary>
        /// Create a game component.
        /// </summary>
        /// <param name="id">Unique id of the component.</param>
        public GameComponent(String id)
        {
            mId = id;
        }

        /// <summary>
        /// Retrieves the id from this component.
        /// </summary>
        /// <returns>A string containing the id.</returns>
        public String GetId()
        {
            return mId;
        }

        /// <summary>
        /// Initialization routine. Here we initialize
        /// the internals of the component.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Update the component logic here.
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last frame.</param>
        public virtual void Update(int deltaTime)
        {
        }

        /// <summary>
        /// Renders to the screen;
        /// </summary>
        public virtual void Render(SpriteBatch spriteBatch)
        {
        }

        /// <summary>
        /// Free unused resources. After calling this method,
        /// the component will become unusable.
        /// </summary>
        public virtual void Shutdown()
        {
        }
    }
}
