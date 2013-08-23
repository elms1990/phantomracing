using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class World
    {
        private static World sInstance = null;

        private int mWidth;
        private int mHeight;
        private int mBinSize;

        // Small optimizations
        private float mBinInverse;
        private int mHorizontalBins;
        private int mVerticalBins;

        private LinkedList<GameObject>[] mWorld;

        private LinkedList<GameObject> mRegistered;

        private World(int width, int height, int binSize)
        {
            mWidth = width;
            mHeight = height;
            mBinSize = binSize;
            mBinInverse = 1.0f / binSize;

            mHorizontalBins = (int)Math.Ceiling(width * mBinInverse);
            mVerticalBins = (int)Math.Ceiling(height * mBinInverse);

            mRegistered = new LinkedList<GameObject>();

            mWorld = new LinkedList<GameObject>[mHorizontalBins * mVerticalBins];
            for (int i = 0; i < mHorizontalBins * mVerticalBins; i++)
            {
                mWorld[i] = new LinkedList<GameObject>();
            }
        }

        public static void CreateInstance(int width, int height, int binSize)
        {
            if (sInstance == null)
            {
                sInstance = new World(width, height, binSize);
            }
        }

        public static World GetInstance()
        {
            return sInstance;
        }

        /// <summary>
        /// Retrieves the objects on a given region.
        /// </summary>
        /// <param name="region">A rectangle defining the region.</param>
        /// <returns>A set containing all objects in the region. This
        /// set might be empty, if no object is found.</returns>
        public HashSet<GameObject> QueueRegion(Rectangle region)
        {
            HashSet<GameObject> objs = new HashSet<GameObject>();

            int rows = (int)(region.Height * mBinInverse);
            for (int j = 0; j <= rows; j++)
            {
                for (int i = (int)(region.X * mBinInverse); i <= (int)(region.Width * mBinInverse); i++)
                {
                    foreach (GameObject go in mWorld[j * mHorizontalBins + i])
                    {
                        objs.Add(go);
                    }
                }
            }

            return objs;
        }

        /// <summary>
        /// Add an object to the world.
        /// </summary>
        /// <param name="go">Object to be added.</param>
        public void Add(GameObject go)
        {
            TransformComponent transform = (TransformComponent)go.GetComponent("Transform");
            RenderComponent render = (RenderComponent)go.GetComponent("Render");

            // This object cannot be queued
            if (transform == null || render == null)
                return;

            mRegistered.AddLast(go);

            int rows = (int)(render.GetHeight() * mBinInverse);
            for (int j = 0; j <= rows; j++)
            {
                for (int i = (int)(transform.Position.X * mBinInverse); i <= (int)(render.GetWidth() * mBinInverse); i++)
                {
                    mWorld[mHorizontalBins * j + i].AddLast(go);
                }
            }
        }

        /// <summary>
        /// Removes a given object from the world.
        /// </summary>
        /// <param name="go">Object to be removed.</param>
        public void Remove(GameObject go)
        {
            TransformComponent transform = (TransformComponent)go.GetComponent("Transform");
            RenderComponent render = (RenderComponent)go.GetComponent("Render");

            // This object cannot be queued
            if (transform == null || render == null)
                return;

            mRegistered.Remove(go);

            int rows = (int)(render.GetHeight() * mBinInverse);
            for (int j = 0; j <= rows; j++)
            {
                for (int i = (int)(transform.Position.X * mBinInverse); i <= (int)(render.GetWidth() * mBinInverse); i++)
                {
                    mWorld[mHorizontalBins * j + i].Remove(go);
                }
            }

        }

        /// <summary>
        /// Removes all objects registered in the world.
        /// This, by no means, cleans all references.
        /// </summary>
        public void Clear()
        {
            mRegistered.Clear();

            for (int j = 0; j <= mVerticalBins; j++)
            {
                for (int i = 0; i <= mHorizontalBins; i++)
                {
                    mWorld[j * mHorizontalBins + i].Clear();
                }
            }
        }

        /// <summary>
        /// Retrieves a registered GameObject from the world.
        /// </summary>
        /// <param name="id">GameObject id.</param>
        /// <returns>Returns the GameObject if it was found.
        /// Returns null, otherwise.</returns>
        public GameObject FindGameObject(String id)
        {
            foreach (GameObject go in mRegistered)
            {
                if (go.GetId().CompareTo(id) == 0)
                    return go;
            }

            return null;
        }

        /// <summary>
        /// Retrieves a list containing all registered objects.
        /// </summary>
        /// <returns>The original linked list containing
        /// all objects. Manually changing this list might cause
        /// inconsistencies. </returns>
        public LinkedList<GameObject> GetRegisteredObjects()
        {
            return mRegistered;
        }
    }
}
