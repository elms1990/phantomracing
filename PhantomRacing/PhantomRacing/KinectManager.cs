using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    /// <summary>
    /// KinectManager class stores information regarding
    /// the kinect sensor status as well as implementing
    /// routines to initialize, stop and handle kinect data.
    /// Warning: This is a singleton class.
    /// </summary>
    public class KinectManager
    {
        // Singleton instance of the KinectManager.
        private static KinectManager cInstance = null;

        // Active Kinect sensor.
        private KinectSensor mKinectSensor = null;

        // Contains depth data information
        private DepthImagePixel[] mDepthData;
        private byte[] mColorInformation;

        // Processed depth data
        private Color[] mConvertedDepthData = null;

        private byte[] mArena = null;

        private int mMinDepth = 0;

        // Destruction mode
        private bool mNonStop = true;
        private bool mScanned = false;

        private int mMaxDepth = 0;

        private Texture2D mDepthBuffer = null;

        private int mThreshold = Globals.SEPARATION_THRESHOLD;

        private int MAX_WAIT = 0;
        private int mCurWait = 0;
        private bool mCooldown = false;

        /// <summary>
        /// Hidden Constructor
        /// </summary>
        private KinectManager()
        {
        }

        public static void CreateInstance()
        {
            if (cInstance == null)
            {
                cInstance = new KinectManager();
            }
        }

        public static KinectManager GetInstance()
        {
            return cInstance;
        }

        /// <summary>
        /// Starts the Kinect Sensor
        /// <summary>
        /// <throws name="NoKinectSensorAvailableException">If no Kinect Sensor
        /// can be found, an exception is thrown.</throws>
        public void StartKinect()
        {
            // Silently halts application.
            if (KinectSensor.KinectSensors.Count == 0)
            {
                throw new NoKinectSensorAvailableException();
            }

            // Picks up the first available kinect sensor and stores a reference to it.
            mKinectSensor = KinectSensor.KinectSensors[0];

            // Enables Kinect Color Camera
            mKinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            // Initialize Depth Sensor
            //mKinectSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);

            // Register an information handler
            //mKinectSensor.DepthFrameReady += new EventHandler<DepthImageFrameReadyEventArgs>(DepthFrameReady);
            mKinectSensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(ColorFrameReady);

            mDepthData = new DepthImagePixel[mKinectSensor.DepthStream.FramePixelDataLength];
            mArena = new byte[mKinectSensor.ColorStream.FrameWidth * mKinectSensor.ColorStream.FrameHeight];
            mColorInformation = new byte[mKinectSensor.ColorStream.FrameWidth * mKinectSensor.ColorStream.FrameHeight * 4];

            mKinectSensor.Start();
        }

        public void Step(int deltaTime)
        {
            mCurWait += deltaTime;

            if (mCurWait >= MAX_WAIT)
            {
                mCurWait = 0;
                mCooldown = false;
            }
        }

        private void ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                if (!mNonStop && mScanned)
                {
                    return;
                }

                if (colorFrame == null || mCooldown)
                {
                    return;
                }

                colorFrame.CopyPixelDataTo(mColorInformation);

                mDepthBuffer = new Texture2D(Renderer.GetInstance().GetGraphicsDevice(),
                    mKinectSensor.ColorStream.FrameWidth, mKinectSensor.ColorStream.FrameHeight);

                //for (int j = 0; j < mKinectSensor.ColorStream.FrameHeight; j++)
                //{
                //    for (int i = 0; i < mKinectSensor.ColorStream.FrameWidth; i++)
                //    {
                //        for (int k = 0; k < 4; k++)
                //        {
                //            byte old = mColorInformation[j * mKinectSensor.ColorStream.FrameWidth + i + k];
                //            mColorInformation[j * mKinectSensor.ColorStream.FrameWidth + i + k] =
                //                mColorInformation[(i + k) * mKinectSensor.ColorStream.FrameHeight + j];
                //            mColorInformation[(i + k) * mKinectSensor.ColorStream.FrameHeight + j] = old;
                //        }
                //    }
                //}

                for (int i = 0; i < mColorInformation.Length - 3; i += 4)
                {
                    uint pixel = (uint)((mColorInformation[i] + mColorInformation[i + 1] +
                        mColorInformation[i + 2]) / 3);

                    if (pixel >= mThreshold)
                    {
                        pixel = 0xff;
                    }
                    else
                    {
                        pixel = 0x0;
                    }

                    mColorInformation[i] = 0;
                    mColorInformation[i + 1] = 0;
                    mColorInformation[i + 2] = 0;
                    mColorInformation[i + 3] = (byte)(0xff - (byte)pixel);
                    mArena[i / 4] = (byte)pixel;
                }

                //for (int j = 0; j < mKinectSensor.ColorStream.FrameHeight; j++)
                //{
                //    for (int i = 0; i < mKinectSensor.ColorStream.FrameWidth; i++)
                //    {
                //        byte old = mArena[j * mKinectSensor.ColorStream.FrameWidth + i];
                //        mArena[j * mKinectSensor.ColorStream.FrameWidth + i] =
                //            mArena[j * mKinectSensor.ColorStream.FrameWidth + mKinectSensor.ColorStream.FrameWidth - i - 1];
                //        mArena[j * mKinectSensor.ColorStream.FrameWidth + mKinectSensor.ColorStream.FrameWidth - i - 1] = old;

                //    }
                //}
                
                mDepthBuffer.SetData(mColorInformation);
            }

            mCooldown = true;
        }

        public void SetMode(bool nonStop)
        {
            mNonStop = nonStop;
        }

        public void SetScanned(bool scanned)
        {
            mScanned = scanned;
        }

        public bool IsNonStopMode()
        {
            return mNonStop;
        }

        public void PurgeRegion(GameObject collider)
        {
            float scaleX = ((float)GetColorFrameWidth()) / Renderer.GetInstance().GetWidth();
            float scaleY = ((float)GetColorFrameHeight()) / Renderer.GetInstance().GetHeight();

            TransformComponent colliderTransform = (TransformComponent)collider.GetComponent("Transform");
            RenderComponent colliderRender = (RenderComponent)collider.GetComponent("Render");
            int scaledX = (int)(scaleX * colliderTransform.Position.X);
            int scaledY = (int)(scaleY * colliderTransform.Position.Y);
            int scaledW = (int)(scaleX * colliderRender.GetWidth());
            int scaledH = (int)(scaleY * colliderRender.GetHeight());
            int frameW = KinectManager.GetInstance().GetColorFrameWidth();
            int frameH = KinectManager.GetInstance().GetColorFrameHeight();
            Color[] pixelBuffer = colliderRender.GetPixelBuffer();

            // Prevents out of index issues, because out of window verification is
            // done after this step.
            if (scaledX < 0 || scaledY < 0 || scaledX + scaledW >= frameW || scaledY + scaledH >= frameH)
            {
                return;
            }

            for (int j = -4; j < colliderRender.GetHeight() + 4; j++)
            {
                for (int i = -4; i < colliderRender.GetWidth() + 4; i++)
                {
                    mColorInformation[4 * (int)(frameW * (scaledY + j) + scaledX + i * scaleX) - 1] = 0;
                    mArena[(int)(frameW * (scaledY + j) + scaledX + i * scaleX)] = 0xff;
                }

            }

             mDepthBuffer.SetData(mColorInformation);
        }

        private void DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame == null)
                {
                    return;
                }

                mMaxDepth = 0;
                for (int i = 0; i < mDepthData.Length; i++)
                {
                    if (mDepthData[i].Depth > mMaxDepth &&
                        mDepthData[i].Depth < 4000)
                    {
                        if (mDepthData[i].Depth < mMaxDepth) 
                        {
                            mMaxDepth = mDepthData[i].Depth;
                        }
                    }
                    //mMaxDepth = (mDepthData[i].Depth != 0 && 
                    //    mDepthData[i].Depth < mMaxDepth) ? mDepthData[i].Depth : mMaxDepth;
                }
            }
        }

        public void ProcessDepthData()
        {
            if (mDepthData == null)
            {
                return;
            }

            //if (mDepthBuffer == null)
            //{
                //mDepthBuffer = new Texture2D(Renderer.GetInstance().GetGraphicsDevice(),
                //    mKinectSensor.DepthStream.FrameWidth, mKinectSensor.DepthStream.FrameHeight);
            //}

            if (mConvertedDepthData == null)
            {
                mConvertedDepthData = new Color[mKinectSensor.DepthStream.FrameWidth * mKinectSensor.DepthStream.FrameHeight];
            }

            mMaxDepth = 0;
            for (int i = 0; i < mDepthData.Length; i++)
            {
                if (mDepthData[i].Depth > mMaxDepth &&
                    mDepthData[i].Depth < 4000)
                {
                    //if (mDepthData[i].Depth < mMaxDepth)
                    //
                        mMaxDepth = mDepthData[i].Depth;
                    //}
                }
                //mMaxDepth = (mDepthData[i].Depth != 0 && 
                //    mDepthData[i].Depth < mMaxDepth) ? mDepthData[i].Depth : mMaxDepth;
            }

            //mDepthBuffer.GetData(mConvertedDepthData);
            for (int i = 0; i < mDepthData.Length; i++)
            {

                //mConvertedDepthData[i] = new Color(255 - (int)(mDepthData[i].Depth / (255.0f * 4000)), 0, 0);
                //qif (mDepthData[i].Depth > 400 && mDepthData[i].Depth < mMaxDepth)
                //{
                    
                //}
                //mConvertedDepthData[i] = (mDepthData[i].Depth != 0 &&
                //mDepthData[i].Depth < mMaxDepth - mThreshold) ? Color.Red : Color.Green;
            }
            mDepthBuffer.SetData(mConvertedDepthData);
        }

        public Texture2D GetArena()
        {
            return mDepthBuffer;
        }

        public int GetMaxDepth() {
            return mMaxDepth;
        }

        public int GetColorFrameWidth()
        {
            return mKinectSensor.ColorStream.FrameWidth;
        }

        public int GetColorFrameHeight()
        {
            return mKinectSensor.ColorStream.FrameHeight;
        }

        public byte[] GetRawArena()
        {
            return mArena;
        }

        /// <summary>
        /// Turn off the Kinect and its sensors.
        /// </summary>
        public void StopKinect()
        {
            // We did not find an available sensor. Nothing to do here.
            if (mKinectSensor == null)
                return;

            // Turn off the sensor and free available memory
            mKinectSensor.Stop();
            mKinectSensor.Dispose();
        }
    }
}
