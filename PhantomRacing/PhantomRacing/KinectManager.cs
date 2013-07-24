using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

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

        /// <summary>
        /// Hidden Constructor
        /// </summary>
        private KinectManager()
        {
        }

        /// <summary>
        /// Creates an instance of the KinectManager class.
        /// </summary>
        public static KinectManager Instance
        {
            get
            {
                if (cInstance == null)
                    cInstance = new KinectManager();
                return cInstance;
            }
        }

        /// <summary>
        /// Starts the Kinect Sensor
        /// <summary>
        /// <throws name="NoKinectSensorAvailableException">If no Kinect Sensor
        /// can be found, an exception is thrown.</throws>
        public void StartKinect()
        {
            // Warns the user that he forgot to plug in his kinect sensor.
            if (KinectSensor.KinectSensors.Count == 0)
            {
                throw new NoKinectSensorAvailableException();
            }

            // Picks up the first available kinect sensor and stores a reference to it.
            mKinectSensor = KinectSensor.KinectSensors[0];

            // Enables Kinect Color Camera
            mKinectSensor.ColorStream.Enable();

            // Enables Kinect Depth Camera
            mKinectSensor.DepthStream.Enable();
        }

        /// <summary>
        /// Turn off the Kinect and its sensors.
        /// </summary>
        public void StopKinect()
        {
            // We did not find an available sensor. Nothing to do here.
            if (mKinectSensor == null)
                return;

            // Turn off the sensor and frees avaiable memory
            mKinectSensor.Stop();
            mKinectSensor.Dispose();
        }

    }
}
