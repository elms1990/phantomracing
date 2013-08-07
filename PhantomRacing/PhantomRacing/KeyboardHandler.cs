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
        private InputState mOldState = null; 
        private InputState mNewState = new InputState();

        private LinkedList<String> mPressed, mJustPressed, mReleased, mJustReleased;

        private Dictionary<string, Keys> mMapped = new Dictionary<string, Keys>();

        private static KeyboardHandler sInstance = null;

        private KeyboardHandler()
        {
        }

        public static void CreateInstance()
        {
            if (sInstance == null)
            {
                sInstance = new KeyboardHandler();
            }
        }

        public static KeyboardHandler GetInstance()
        {
            return sInstance;
        }

        public InputState GetState()
        {
            return mNewState;
        }

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

        public IInput Map(string id, Object button)
        {
            mMapped.Add(id, (Keys)button);

            return sInstance;
        }

        public void Unmap(string id)
        {
            mMapped.Remove(id);
        }
    }
}
