using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class KeyboardHandler : IInput
    {
        // Old keyboard state
        private InputState mOldState = null; 

        // New keyboard state
        private InputState mNewState = new InputState();

        // Individual key states
        private LinkedList<String> mPressed, mJustPressed, mReleased, mJustReleased;

        // Mapped keys
        private Dictionary<string, Keys> mMapped = new Dictionary<string, Keys>();

        // Singleton instance
        private static KeyboardHandler sInstance = null;

        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private KeyboardHandler()
        {
        }

        /// <summary>
        /// Create a singleton instance of this class.
        /// </summary>
        public static void CreateInstance()
        {
            if (sInstance == null)
            {
                sInstance = new KeyboardHandler();
            }
        }

        /// <summary>
        /// Retrieves the singleton instance of this class.
        /// </summary>
        /// <returns>Singleton instance.</returns>
        public static KeyboardHandler GetInstance()
        {
            return sInstance;
        }

        /// <summary>
        /// Retrieves the state of this keyboard.
        /// </summary>
        /// <returns>Current state.</returns>
        public InputState GetState()
        {
            return mNewState;
        }

        /// <summary>
        /// Updates all mapped keys.
        /// </summary>
        public void Update()
        {
            KeyboardState curState = Keyboard.GetState();

            // New state is now the old state
            mOldState = mNewState;

            // Clears old input lists
            mPressed = new LinkedList<string>();
            mReleased = new LinkedList<string>();
            mJustPressed = new LinkedList<string>();
            mJustReleased = new LinkedList<string>();

            Keys k;
            foreach (string key in mMapped.Keys)
            {
                mMapped.TryGetValue(key, out k);
                if (curState.IsKeyDown(k))
                {
                    // Was pressed and is currently pressed
                    if (mOldState.IsJustPressed(key) || mOldState.IsPressed(key))
                    {
                        mPressed.AddLast(key);
                    }
                    else
                    {
                        // The key was released or just released.
                        mJustPressed.AddLast(key);
                    }
                }
                else
                {
                    // Was released and is currently released
                    if (mOldState.IsJustPressed(key) || mOldState.IsPressed(key))
                    {
                        mJustReleased.AddLast(key);
                    }
                    else
                    {
                        // The key was released or just released.
                        mReleased.AddLast(key);
                    }
                }
            }

            mNewState = new InputState(mPressed, mJustPressed, mReleased, mJustReleased,
                new LinkedList<KeyValuePair<string, float>>(),
                new LinkedList<KeyValuePair<string, Vector3>>());
        }

        /// <summary>
        /// Adds a new key to the mapping.
        /// </summary>
        /// <param name="id">Id of the virtual key.</param>
        /// <param name="button">Physical key.</param>
        /// <returns>An instance of this class.</returns>
        public IInput Map(string id, Object button)
        {
            mMapped.Add(id, (Keys)button);

            return sInstance;
        }

        /// <summary>
        /// Removes a key from mapping.
        /// </summary>
        /// <param name="id">Virtual key id.</param>
        public void Unmap(string id)
        {
            mMapped.Remove(id);
        }
    }
}
