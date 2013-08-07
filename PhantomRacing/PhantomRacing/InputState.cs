using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class InputState
    {
        // Stores buttons and their states.
        private LinkedList<String> mPressed, mJustPressed, mReleased, mJustReleased;

        // Stores scalar values
        private LinkedList<KeyValuePair<String, float>> mScalar;

        // Stores vectorial quantities
        private LinkedList<KeyValuePair<String, Vector3>> mVector;


        // State machine
        //                          Next State
        // Cur            Pressed           Released
        // JustPressed    Pressed           JustReleased    
        // Pressed        Pressed           JustReleased
        // JustReleased   JustPressed       Released
        // Released       JustPressed       Released

        public InputState()
        {
            mPressed = new LinkedList<string>();
            mJustPressed = new LinkedList<string>();
            mReleased = new LinkedList<string>();
            mJustReleased = new LinkedList<string>();
            mVector = new LinkedList<KeyValuePair<string, Vector3>>();
            mScalar = new LinkedList<KeyValuePair<string, float>>();
        }

        public InputState(LinkedList<String> pressed, LinkedList<String> justPressed,
            LinkedList<String> released, LinkedList<String> justReleased,
            LinkedList<KeyValuePair<String, float>> scalar, 
            LinkedList<KeyValuePair<String, Vector3>> vector)
        {
            mPressed = pressed;
            mJustPressed = justPressed;
            mReleased = released;
            mJustReleased = released;
            mScalar = scalar;
            mVector = vector;
        }

        public LinkedList<string> GetJustPressed()
        {
            return mJustPressed;
        }

        public LinkedList<string> GetPressed()
        {
            return mPressed;
        }

        public LinkedList<string> GetJustReleased()
        {
            return mJustReleased;
        }

        public LinkedList<string> GetReleased()
        {
            return mReleased;
        }

        public LinkedList<KeyValuePair<string, float>> GetScalar()
        {
            return mScalar;
        }

        public LinkedList<KeyValuePair<string, Vector3>> GetVector()
        {
            return mVector;
        }

        public bool IsJustPressed(String key)
        {
            return mJustPressed.Contains(key);
        }

        public bool IsPressed(String key)
        {
            return mPressed.Contains(key);
        }

        public bool IsJustReleased(String key)
        {
            return mJustReleased.Contains(key);
        }

        public bool IsReleased(String key)
        {
            return mReleased.Contains(key);
        }

        public float? GetScalar(String key)
        {
            foreach (KeyValuePair<String, float> kvp in mScalar)
            {
                if (kvp.Key.CompareTo(key) == 0)
                {
                    return kvp.Value;
                }
            }

            return null;
        }

        public Vector3? GetVector(String key)
        {
            foreach (KeyValuePair<String, Vector3> kvp in mVector)
            {
                if (kvp.Key.CompareTo(key) == 0)
                {
                    return kvp.Value;
                }
            }

            return null;
        }
    }
}
