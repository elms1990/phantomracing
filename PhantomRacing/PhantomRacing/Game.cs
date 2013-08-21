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
        // Object reference to GraphicsDevice.
        private GraphicsDeviceManager graphics;

        // Use this to draw stuff on the screen.
        private SpriteBatch spriteBatch;

        // Playerlist
        private Player[] mPlayers = new Player[1];

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            //graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
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

            // Initialize AssetLoader
            AssetLoader.CreateInstance(Content);

            // Initialize Viewport
            Viewport.CreateInstance(graphics.GraphicsDevice);

            // Initialize KeyboardHandler
            KeyboardHandler.CreateInstance();

            // Load default keys for player 1
            KeyboardHandler.GetInstance().Map("up", Keys.W)
                .Map("left", Keys.A)
                .Map("down", Keys.S)
                .Map("right", Keys.D)
                .Map("rleft", Keys.Left)
                .Map("rright", Keys.Right)
                .Map("shoot", Keys.Up);
            
            // Allocate players
            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i] = new Player();
                mPlayers[i].AddComponent(new TransformComponent()).
                    AddComponent(new PlayerInputComponent(mPlayers[i])).
                    AddComponent(new BulletComponent(mPlayers[i])).
                    AddComponent(new RenderComponent(mPlayers[i], Content.Load<Texture2D>("player"))).
                    AddComponent(new LifeComponent(mPlayers[i], 100, 75));
                mPlayers[i].Index = (PlayerIndex)(i + 1);
                mPlayers[i].Initialize();
            }
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
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardHandler.GetInstance().Update();

            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i].Update(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i].Render(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
