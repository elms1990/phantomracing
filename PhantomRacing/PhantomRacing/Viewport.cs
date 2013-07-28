using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class Viewport
    {
        private static Viewport sInstance = null;

        private GraphicsDevice mDevice;

        private Viewport(GraphicsDevice device)
        {
            mDevice = device;
        }

        public static void CreateInstance(GraphicsDevice device)
        {
            if (sInstance == null)
                sInstance = new Viewport(device);
        }

        public static Viewport GetInstance()
        {
            return sInstance;
        }

        public int GetWidth()
        {
            return mDevice.Viewport.Width;
        }

        public int GetHeight()
        {
            return mDevice.Viewport.Height;
        }
    }
}
