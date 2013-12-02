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
        private InputState[] mOldState = new InputState[4];

        // New keyboard state
        private InputState[] mNewState = new InputState[4];

        // Individual key states
        private LinkedList<String>[] mPressed = new LinkedList<string>[4], 
            mJustPressed = new LinkedList<string>[4], 
            mReleased = new LinkedList<string>[4], 
            mJustReleased = new LinkedList<string>[4];

        // Mapped buttons
        private Dictionary<string, Buttons>[] mMapped = new Dictionary<string,Buttons>[4];

        // Singleton instance
        private static GamePadHandler sInstance = null;

        /// <summary>
        /// Hidden constructor.
        /// </summary>
        private GamePadHandler()
        {
            for (int i = 0; i < mMapped.Length; i++)
            {
                mMapped[i] = new Dictionary<string, Buttons>();
                mNewState[i] = new InputState();
            }
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

        public InputState GetState()
        {
            return null;
        }

        /// <summary>
        /// Retrieves the state of this keyboard.
        /// </summary>
        /// <returns>Current state.</returns>
        public InputState GetState(PlayerIndex index)
        {
            return mNewState[(int)index];
        }

        /// <summary>
        /// Updates all mapped keys.
        /// </summary>
        public void Update()
        {
            // Clears old input lists

            for (int i = 0; i < 4; i++)
            {
                mPressed[i] = new LinkedList<string>();
                mReleased[i] = new LinkedList<string>();
                mJustPressed[i] = new LinkedList<string>();
                mJustReleased[i] = new LinkedList<string>();
            }

            // New state is now the old state
            mOldState = mNewState;

            // Merges all players states
            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                GamePadState curState = GamePad.GetState(index);

                Buttons b;
                foreach (string key in mMapped[(int)index].Keys)
                {
                    mMapped[(int)index].TryGetValue(key, out b);
                    if (curState.IsButtonDown(b))
                    {
                        // Was pressed and is currently pressed
                        if (mOldState[(int)index].IsJustPressed(key) || mOldState[(int)index].IsPressed(key))
                        {
                            mPressed[(int)index].AddLast(key);
                        }
                        else
                        {
                            // The button was released or just released.
                            mJustPressed[(int)index].AddLast(key);
                        }
                    }
                    else
                    {
                        // Was released and is currently released
                        if (mOldState[(int)index].IsJustPressed(key) || mOldState[(int)index].IsPressed(key))
                        {
                            mJustReleased[(int)index].AddLast(key);
                        }
                        else
                        {
                            // The button was released or just released.
                            mReleased[(int)index].AddLast(key);
                        }
                    }
                }

            }

            for (int i = 0; i < 4; i++)
            {
                mNewState[i] = new InputState(mPressed[i], mJustPressed[i], mReleased[i], mJustReleased[i],
                    new LinkedList<KeyValuePair<string, float>>(),
                    new LinkedList<KeyValuePair<string, Vector3>>());
            }
        }

        public IInput Map(string id, Object button)
        {
            return this;
        }

        /// <summary>
        /// Adds a new key to the mapping.
        /// </summary>
        /// <param name="id">Id of the virtual key.</param>
        /// <param name="button">Physical key.</param>
        /// <returns>An instance of this class.</returns>
        public GamePadHandler MapPlayer(string id, Object button, PlayerIndex index)
        {
            mMapped[(int)index].Add(id, (Buttons)button);

            return sInstance;
        }

        /// <summary>
        /// Removes a key from mapping.
        /// </summary>
        /// <param name="id">Virtual key id.</param>
        public void Unmap(string id, PlayerIndex index)
        {
            mMapped[(int)index].Remove(id);
        }

        public void Unmap(string id)
        {

        }
    }
}
