using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace PhantomRacing
{
    public class GamePadHandler : IInput
    {
        // Old gamepad state
        private InputState mOldState = null;

        // New keyboard state
        private InputState mNewState = new InputState();

        // Individual key states
        private LinkedList<String> mPressed, mJustPressed, mReleased, mJustReleased;

        // Mapped buttons
        private Dictionary<string, Buttons> mMapped = new Dictionary<string, Buttons>();

        // Singleton instance
        private static GamePadHandler sInstance = null;

        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private GamePadHandler()
        {
        }

        /// <summary>
        /// Create a singleton instance of this class.
        /// </summary>
        public static void CreateInstance()
        {
            if (sInstance == null)
            {
                sInstance = new GamePadHandler();
            }
        }

        /// <summary>
        /// Retrieves the singleton instance of this class.
        /// </summary>
        /// <returns>Singleton instance.</returns>
        public static GamePadHandler GetInstance()
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
            // Clears old input lists
            mPressed = new LinkedList<string>();
            mReleased = new LinkedList<string>();
            mJustPressed = new LinkedList<string>();
            mJustReleased = new LinkedList<string>();

            // New state is now the old state
            mOldState = mNewState;

            // Merges all players states
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                GamePadState curState = GamePad.GetState(index);

                Buttons b;
                foreach (string key in mMapped.Keys)
                {
                    mMapped.TryGetValue(key, out b);
                    if (curState.IsButtonDown(b))
                    {
                        // Was pressed and is currently pressed
                        if (mOldState.IsJustPressed(key) || mOldState.IsPressed(key))
                        {
                            mPressed.AddLast(key);
                        }
                        else
                        {
                            // The button was released or just released.
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
                            // The button was released or just released.
                            mReleased.AddLast(key);
                        }
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
            mMapped.Add(id, (Buttons)button);

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
