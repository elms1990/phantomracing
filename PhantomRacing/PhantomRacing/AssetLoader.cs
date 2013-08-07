using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace PhantomRacing
{
    public class AssetLoader
    {
        // Singleton instance
        private static AssetLoader sInstance = null;

        // Content manager reference
        private ContentManager mContentManager = null;

        /// <summary>
        /// Hidden constructor
        /// </summary>
        /// <param name="manager">Asset loader</param>
        private AssetLoader(ContentManager manager)
        {
            mContentManager = manager;
        }

        /// <summary>
        /// Create a singleton instance of this class.
        /// </summary>
        /// <param name="manager">Content manager reference.</param>
        public static void CreateInstance(ContentManager manager)
        {
            if (sInstance == null)
            {
                sInstance = new AssetLoader(manager);
            }
        }

        /// <summary>
        /// Retrieves the singleton instance.
        /// </summary>
        /// <returns>The instance or null, if CreateInstance was
        /// never called.</returns>
        public static AssetLoader GetInstance()
        {
            return sInstance;
        }

        /// <summary>
        /// Load an asset from the file system.
        /// </summary>
        /// <typeparam name="T">Type of the asset.</typeparam>
        /// <param name="asset">Name of the asset.</param>
        /// <returns>An instance of the desired asset or null
        /// if an error was found.</returns>
        public T LoadAsset<T>(String asset)
        {
            return mContentManager.Load<T>(asset);
        }
    }
}
