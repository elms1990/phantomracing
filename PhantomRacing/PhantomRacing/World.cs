﻿using System;
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

        public HashSet<GameObject> GetRegistered()
        {
            HashSet<GameObject> objs = new HashSet<GameObject>();

            foreach (GameObject obj in mRegistered)
            {
                objs.Add(obj);
            }

            return objs;
        }

        /// <summary>
        /// Refreshes objects position in the world.
        /// </summary>
        public void Update()
        {
            // Clears old world
            for (int i = 0; i < mVerticalBins * mHorizontalBins; i++)
            {
                mWorld[i].Clear();
            }

            // Re-insterts objects in the world
            foreach (GameObject go in mRegistered)
            {
                TransformComponent transform = (TransformComponent)go.GetComponent("Transform");
                RenderComponent render = (RenderComponent)go.GetComponent("Render");

                int start_x = (int)(transform.Position.X * mBinInverse);
                int end_x = start_x + (int)(render.GetWidth() * mBinInverse);
                int start_y = (int)(transform.Position.Y * mBinInverse);
                int end_y = start_y + (int)(render.GetHeight() * mBinInverse);

                for (int j = start_y; j <= end_y; j++)
                {
                    for (int i = start_x; i <= end_x; i++)
                    {
                        mWorld[j * mHorizontalBins + i].AddLast(go);
                    }
                }
            }
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

            int start_x = (int)(region.X * mBinInverse);
            int end_x = (int)((region.X + region.Width) * mBinInverse);
            int start_y = (int)(region.Y * mBinInverse);
            int end_y = (int)((region.Y + region.Height) * mBinInverse);

            for (int j = start_y; j <= end_y; j++)
            {
                for (int i = start_x; i <= end_x; i++)
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

            int start_x = (int)(transform.Position.X * mBinInverse);
            int end_x = (int)((transform.Position.X + render.GetWidth()) * mBinInverse);
            int start_y = (int)(transform.Position.Y * mBinInverse);
            int end_y = (int)((transform.Position.Y + render.GetHeight()) * mBinInverse);

            for (int j = start_y; j <= end_y; j++)
            {
                for (int i = start_x; i <= end_x; i++)
                {
                    mWorld[j * mHorizontalBins + i].AddLast(go);
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

            int start_x = (int)(transform.Position.X * mBinInverse);
            int end_x = (int)((transform.Position.X + render.GetWidth()) * mBinInverse);
            int start_y = (int)(transform.Position.Y * mBinInverse);
            int end_y = (int)((transform.Position.Y + render.GetHeight()) * mBinInverse);

            for (int j = start_y; j <= end_y; j++)
            {
                for (int i = start_x; i <= end_x; i++)
                {
                    mWorld[j * mHorizontalBins + i].Remove(go);
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

            for (int i = 0; i < mVerticalBins * mHorizontalBins; i++)
            {
                mWorld[i].Clear();
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
        /// Broadcasts an event to all registered objects in the game.
        /// </summary>
        public void BroadcastEvent(Event e)
        {
            try
            {
                foreach (GameObject go in mRegistered)
                {
                    go.ReceiveEvent(e);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
