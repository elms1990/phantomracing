using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PhantomRacing
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        private const int WINDOW_WIDTH = 1024;
        private const int WINDOW_HEIGHT = 768;

        private KinectManager mKinect;
        private World mWorld;
        private KeyboardHandler mKeyboard;
        private AssetLoader mAssetLoader;

        private SpriteFont mFont;

        // Object reference to GraphicsDevice.
        private GraphicsDeviceManager graphics;

        // Use this to draw stuff on the screen.
        private SpriteBatch spriteBatch;

        // Playerlist
        private Player[] mPlayers = new Player[2];

        private Rectangle mBlittingRectangle;

        private const int REFERENCE_DISTANCE = 120;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";

            mBlittingRectangle = new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();

            // Create world instance
            World.CreateInstance(2048, 2048, 128);
            mWorld = World.GetInstance();

            // Initialize AssetLoader
            AssetLoader.CreateInstance(Content);
            mAssetLoader = AssetLoader.GetInstance();
            mFont = AssetLoader.GetInstance().LoadAsset<SpriteFont>("font");

            // Initialize Renderer
            Renderer.CreateInstance(graphics.GraphicsDevice);

            // Initialize KeyboardHandler
            KeyboardHandler.CreateInstance();
            mKeyboard = KeyboardHandler.GetInstance();

            // Initialize Kinect
            KinectManager.CreateInstance();
            mKinect = KinectManager.GetInstance();

            // Load default keys for player 1
            KeyboardHandler.GetInstance().Map("p1_up", Keys.W)
                .Map("p1_left", Keys.A)
                .Map("p1_down", Keys.S)
                .Map("p1_right", Keys.D)
                .Map("p1_rleft", Keys.Left)
                .Map("p1_rright", Keys.Right)
                .Map("p1_shoot", Keys.Up);

            // Load default keys for player 2
            KeyboardHandler.GetInstance().Map("p2_up", Keys.NumPad8)
                .Map("p2_left", Keys.NumPad4)
                .Map("p2_down", Keys.NumPad2)
                .Map("p2_right", Keys.NumPad6)
                .Map("p2_rleft", Keys.NumPad7)
                .Map("p2_rright", Keys.NumPad9)
                .Map("p2_shoot", Keys.NumPad5);
            
            // Allocate players
            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i] = new Player(i + 1);
                mPlayers[i].AddComponent(new TransformComponent()).
                    AddComponent(new PlayerInputComponent(mPlayers[i], i + 1)).
                    AddComponent(new BulletComponent(mPlayers[i])).
                    AddComponent(new PhysicsComponent(mPlayers[i], CollisionType.Circle,
                        "player", new List<string>() { "player", "bullet" })).
                    AddComponent(new RenderComponent(mPlayers[i], Content.Load<Texture2D>("player"))).
                    AddComponent(new LifeComponent(mPlayers[i], 100, 75));
                mPlayers[i].Index = (PlayerIndex)(i + 1);

                ((TransformComponent)mPlayers[i].GetComponent("Transform")).Position.X = 75 + i * 300;
                ((TransformComponent)mPlayers[i].GetComponent("Transform")).Position.Y = 300 + i * 120;

                mPlayers[i].Initialize();
            }

            mKinect.StartKinect();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            mKinect.StopKinect();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            mKeyboard.Update();

            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i].Update(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            }

            mWorld.Update();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            if (mKinect.GetArena() != null)
            {
                spriteBatch.Draw(mKinect.GetArena(), mBlittingRectangle, null, Color.White,
                    0, Vector2.Zero, SpriteEffects.None, 1);
            }

            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i].Render(spriteBatch);
            }

            spriteBatch.DrawString(mFont, "Max Depth: " + mKinect.GetMaxDepth(), Vector2.Zero, Color.Black,
                0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0.99f);
            spriteBatch.End();
        }
    }
}
