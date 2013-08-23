using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class GameObject
    {
        // Children component list
        private List<GameComponent> mChildren = new List<GameComponent>();

        // Object ID
        private String mId;

        /// <summary>
        /// GameObject constructor.
        /// </summary>
        /// <param name="id">Object id so it can be retrieved.</param>
        public GameObject(String id)
        {
            mId = id;
        }

        /// <summary>
        /// Gets the object id.
        /// </summary>
        /// <returns></returns>
        public String GetId()
        {
            return mId;
        }

        /// <summary>
        /// Sets the object id.
        /// </summary>
        /// <param name="id">A new id.</param>
        public void SetId(String id)
        {
            mId = id;
        }

        /// <summary>
        /// Initialize this object.
        /// </summary>
        public virtual void Initialize()
        {
            foreach (GameComponent g in mChildren)
            {
                g.Initialize();
            }

            World.GetInstance().Add(this);
        }

        /// <summary>
        /// Update the game logic of this object.
        /// </summary>
        /// <param name="timeStep"></param>
        public virtual void Update(float timeStep)
        {
            foreach (GameComponent g in mChildren)
            {
                g.Update(timeStep);
            }
        }

        /// <summary>
        /// Renders this object to the screen.
        /// </summary>
        /// <param name="spriteBatch">Graphics object used to draw.</param>
        public virtual void Render(SpriteBatch spriteBatch)
        {
            foreach (GameComponent g in mChildren)
            {
                g.Render(spriteBatch);
            }
        }

        /// <summary>
        /// Disposes this object. After calling this method,
        /// this object becomes unreliable.
        /// </summary>
        public virtual void Shutdown()
        {
            foreach (GameComponent g in mChildren)
            {
                g.Shutdown();
            }

            World.GetInstance().Remove(this);
        }

        /// <summary>
        /// Add a new component to this object.
        /// </summary>
        /// <param name="go">The new component.</param>
        /// <returns>An instance of this object.</returns>
        public GameObject AddComponent(GameComponent g)
        {
            mChildren.Add(g);
            return this;
        }

        /// <summary>
        /// Removes a component owned by this object.
        /// </summary>
        /// <param name="id">The id of the object.</param>
        /// <returns>An instance of this object.</returns>
        public GameObject RemoveComponent(String id)
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

        /// <summary>
        /// Sends an event message to an object.
        /// </summary>
        /// <param name="e">Sending event.</param>
        public void SendEvent(Event e)
        {
            if (e.Sender == null)
                e.Sender = this;
            e.Receiver.ReceiveEvent(e);
        }

        /// <summary>
        /// Receives an event message and handles it.
        /// </summary>
        /// <param name="e">Receiving event.</param>
        public void ReceiveEvent(Event e)
        {
            OnEvent(e);
        }

        /// <summary>
        /// This method is called when this object receives an event.
        /// </summary>
        /// <param name="e">Received event.</param>
        protected virtual void OnEvent(Event e)
        {
        }

        /// <summary>
        /// Sends this message to all objects in the game.
        /// The target field on the struct will be ignored.
        /// </summary>
        /// <param name="e">Event message.</param>
        public static void BroadcastEvent(Event e)
        {
            World.GetInstance().BroadcastEvent(e);
        }
    }
}