using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class GameObject : GameComponent
    {
        // Components assigned to this object.
        private LinkedList<GameComponent> mComponents = new LinkedList<GameComponent>();

        public GameObject(String id) : base(id)
        {
        }

        /// <summary>
        /// Initialize the game object.
        /// </summary>
        public override void Initialize()
        {
            foreach (GameComponent gc in mComponents)
            {
                gc.Initialize();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deltaTime"></param>
        public override void Update(int deltaTime)
        {
            foreach (GameComponent gc in mComponents)
            {
                gc.Update(deltaTime);
            }
        }

        /// <summary>
        /// Renders this object to the screen.
        /// </summary>
        public override void Render(SpriteBatch spriteBatch)
        {
            foreach (GameComponent gc in mComponents)
            {
                gc.Render(spriteBatch);
            }
        }

        /// <summary>
        /// Releases resources used by this object.
        /// </summary>
        public override void Shutdown()
        {
            foreach (GameComponent gc in mComponents)
            {
                gc.Shutdown();
            }

            mComponents.Clear();
        }

        /// <summary>
        /// Add a new component to this object.
        /// </summary>
        /// <param name="component">A game component.</param>
        /// <returns>This object's reference.</returns>
        public GameObject AddComponent(GameComponent component)
        {
            mComponents.AddLast(component);
            return this;
        }

        /// <summary>
        /// Remove a game component from this object.
        /// </summary>
        /// <param name="id">The component id.</param>
        /// <returns>This objects reference.</returns>
        public GameObject RemoveComponent(String id)
        {
            foreach (GameComponent gc in mComponents)
            {
                if (gc.GetId() == id)
                {
                    mComponents.Remove(gc);
                    break;
                }
            }

            return this;
        }

        /// <summary>
        /// Retrieves a GameComponent in this object.
        /// </summary>
        /// <param name="id">The id of the component.</param>
        /// <returns>The component, if it is a child of
        /// this component. Null, otherwise.
        /// </returns>
        public T GetComponent<T> (String id) where T : GameComponent
        {
            foreach (GameComponent gc in mComponents)
            {
                if (gc.GetId().CompareTo(id) == 0)
                {
                    return (T)gc;
                }
            }
            return (T)null;
        }
    }
}
