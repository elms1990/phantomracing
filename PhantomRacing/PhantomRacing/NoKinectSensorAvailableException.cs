using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public class NoKinectSensorAvailableException : Exception
    {
        public NoKinectSensorAvailableException() : base("Exception: Could not find a Kinect Sensor")
        {
        }
    }
}
