using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class Renderer
    {
        private static Renderer sInstance = null;

        private GraphicsDevice mDevice;

        private Renderer(GraphicsDevice graphicsDevice)
        {
            mDevice = graphicsDevice;
        }

        public static void CreateInstance(GraphicsDevice graphicsDevice)
        {
            if (sInstance == null)
            {
                sInstance = new Renderer(graphicsDevice);
            }
        }

        public static Renderer GetInstance()
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

        public GraphicsDevice GetGraphicsDevice()
        {
            return mDevice;
        }
    }
}
