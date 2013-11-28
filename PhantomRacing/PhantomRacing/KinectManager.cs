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

        private int mMaxDepth = 0;

        private Texture2D mDepthBuffer = null;

        private int mThreshold = 100;

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

        private void ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorFrame = e.OpenColorImageFrame())
            {
                //if (mDepthBuffer != null)
                //{
                //    return;
                //}

                if (colorFrame == null)
                {
                    return;
                }

                colorFrame.CopyPixelDataTo(mColorInformation);

                mDepthBuffer = new Texture2D(Renderer.GetInstance().GetGraphicsDevice(),
                    mKinectSensor.ColorStream.FrameWidth, mKinectSensor.ColorStream.FrameHeight);

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
                
                mDepthBuffer.SetData(mColorInformation);
            }
        }

        private void DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame == null)
                {
                    return;
                }

                depthFrame.CopyDepthImagePixelDataTo(mDepthData);
                mMinDepth = depthFrame.MinDepth;
                mMaxDepth = depthFrame.MaxDepth;

                ProcessDepthData();
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
                mDepthBuffer = new Texture2D(Renderer.GetInstance().GetGraphicsDevice(),
                    mKinectSensor.DepthStream.FrameWidth, mKinectSensor.DepthStream.FrameHeight);
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
