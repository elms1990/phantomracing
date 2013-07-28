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

        // Component Children
        private List<GameComponent> mChildren = new List<GameComponent>();

        public GameComponent(String id)
        {
            mId = id;
        }

        public String GetId()
        {
            return mId;
        }

        public virtual void Initialize()
        {
            foreach (GameComponent g in mChildren)
            {
                g.Initialize();
            }
        }

        public virtual void Update(float timeStep)
        {
            foreach (GameComponent g in mChildren)
            {
                g.Update(timeStep);
            }
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {
            foreach (GameComponent g in mChildren)
            {
                g.Render(spriteBatch);
            }
        }

        public virtual void Shutdown()
        {
            foreach (GameComponent g in mChildren)
            {
                g.Shutdown();
            }
        }

        /// <summary>
        /// Add a new component to this object.
        /// </summary>
        /// <param name="go">The new component.</param>
        /// <returns>An instance of this object.</returns>
        public GameComponent AddComponent(GameComponent g)
        {
            mChildren.Add(g);
            return this;
        }

        /// <summary>
        /// Removes a component owned by this object.
        /// </summary>
        /// <param name="id">The id of the object.</param>
        /// <returns>An instance of this object.</returns>
        public GameComponent RemoveComponent(String id)
        {
            foreach (GameComponent g in mChildren)
            {
                if (id.CompareTo(g.GetId()) == 0)
                {
                    mChildren.Remove(g);
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Retrieves a child of this object.
        /// </summary>
        /// <param name="id">Child id.</param>
        /// <returns>A reference to the child if found.
        /// Null otherwise.</returns>
        public GameComponent GetComponent(String id)
        {
            foreach (GameComponent g in mChildren)
            {
                if (id.CompareTo(g.GetId()) == 0)
                {
                    return g;
                }
            }

            return null;
        }
    }
}
