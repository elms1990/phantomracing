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

        private static readonly int mNumberOfPlayers = 2;

        // Playerlist
        private Player[] mPlayers = new Player[mNumberOfPlayers];
        private Event mEvent = new Event();

        private Rectangle mBlittingRectangle;

        private const int REFERENCE_DISTANCE = 120;

        private GameState mCurrentGameState = GameState.PreGame;

        // ModeSelection Stuff
        private uint mMenuPos = 0;
        private Vector2 mItemPos = new Vector2();
        private bool mSelected = false;
        private bool mArenaSelected = false;

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

            GamePadHandler.CreateInstance();

            // Initialize Kinect
            KinectManager.CreateInstance();
            mKinect = KinectManager.GetInstance();

            // Load default keys for player 1
            GamePadHandler.GetInstance().MapPlayer("p1_up", Buttons.LeftThumbstickUp, PlayerIndex.One).
                MapPlayer("p1_left", Buttons.LeftThumbstickLeft, PlayerIndex.One).
                MapPlayer("p1_down", Buttons.LeftThumbstickDown, PlayerIndex.One).
                MapPlayer("p1_right", Buttons.LeftThumbstickRight, PlayerIndex.One).
                MapPlayer("p1_rleft", Buttons.LeftTrigger, PlayerIndex.One).
                MapPlayer("p1_rright", Buttons.RightTrigger, PlayerIndex.One).
                MapPlayer("p1_shoot", Buttons.A, PlayerIndex.One);

            GamePadHandler.GetInstance().MapPlayer("p2_up", Buttons.LeftThumbstickUp, PlayerIndex.Two).
                MapPlayer("p2_left", Buttons.LeftThumbstickLeft, PlayerIndex.Two).
                MapPlayer("p2_down", Buttons.LeftThumbstickDown, PlayerIndex.Two).
                MapPlayer("p2_right", Buttons.LeftThumbstickRight, PlayerIndex.Two).
                MapPlayer("p2_rleft", Buttons.LeftTrigger, PlayerIndex.Two).
                MapPlayer("p2_rright", Buttons.RightTrigger, PlayerIndex.Two).
                MapPlayer("p2_shoot", Buttons.A, PlayerIndex.Two);
            //KeyboardHandler.GetInstance().Map("p1_up", Keys.W)
            //    .Map("p1_left", Keys.A)
            //    .Map("p1_down", Keys.S)
            //    .Map("p1_right", Keys.D)
            //    .Map("p1_rleft", Keys.Left)
            //    .Map("p1_rright", Keys.Right)
            //    .Map("p1_shoot", Keys.Up);

            // Load default keys for player 2
            KeyboardHandler.GetInstance().Map("p2_up", Keys.NumPad8)
                .Map("p2_left", Keys.NumPad4)
                .Map("p2_down", Keys.NumPad2)
                .Map("p2_right", Keys.NumPad6)
                .Map("p2_rleft", Keys.NumPad7)
                .Map("p2_rright", Keys.NumPad9)
                .Map("p2_shoot", Keys.NumPad5);

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
            GamePadHandler.GetInstance().Update();

            if (mCurrentGameState == GameState.ModeSelection &&
                mSelected)
            {
                if (mMenuPos == 0)
                {
                    mCurrentGameState = GameState.PlayerPlacementStart;
                    mKinect.SetMode(true);
                }
                else
                {
                    mKinect.SetMode(false);
                    mCurrentGameState = GameState.ArenaScan;
                }
            }

            if (mCurrentGameState == GameState.ArenaScan && mArenaSelected)
            {
                mCurrentGameState = GameState.PlayerPlacementStart;
            }

            switch (mCurrentGameState)
            {
                case GameState.PreGame:
                    mCurrentGameState = GameState.ModeSelection;
                    break;

                case GameState.ModeSelection:
                    ModeSelectUpdate(gameTime);
                    break;

                case GameState.ArenaScan:
                    ArenaScanningUpdate(gameTime);
                    break;

                case GameState.PlayerPlacementStart:
                    PlayerPlacementStartUpdate(gameTime);
                    break;

                case GameState.PlayerPlacementRunning:
                    PlayerPlacementRunningUpdate(gameTime);
                    break;

                case GameState.RunningStart:
                    RunningStartUpdate(gameTime);
                    break;

                case GameState.Running:
                    RunningUpdate(gameTime);
                    break;
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

            switch (mCurrentGameState)
            {
                case GameState.PreGame:
                    GraphicsDevice.Clear(Color.Black);
                    break;

                case GameState.ModeSelection:
                    ModeSelectDraw();
                    break;

                case GameState.PlayerPlacementStart:
                    PlayerPlacementStartDraw();
                    mCurrentGameState = GameState.PlayerPlacementRunning;
                    break;

                case GameState.PlayerPlacementRunning:
                    PlayerPlacementRunningDraw();
                    break;

                case GameState.ArenaScan:
                    ArenaScanningDraw();
                    break;

                case GameState.RunningStart:
                case GameState.Running:
                    RunningDraw();
                    break;
            }

            spriteBatch.End();
        }

        private void ArenaScanningDraw()
        {
            if (mKinect.GetArena() != null)
            {
                spriteBatch.Draw(mKinect.GetArena(), mBlittingRectangle, null, Color.White,
                    0, Vector2.Zero, SpriteEffects.None, 1);
            }
        }

        private void ArenaScanningUpdate(GameTime gameTime)
        {
            InputState iS = GamePadHandler.GetInstance().GetState(PlayerIndex.One);

            if (iS.IsJustPressed("p1_shoot"))
            {
                mKinect.SetScanned(true);
                mArenaSelected = true;
            }
        }

        private void PlayerPlacementStartUpdate(GameTime gameTime)
        {
            // Allocate players
            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i] = new Player(i + 1);
                mPlayers[i].AddComponent(new TransformComponent()).
                    AddComponent(new PlayerPlacementComponent(mPlayers[i], i + 1)).
                    //AddComponent(new BulletComponent(mPlayers[i])).
                    //AddComponent(new PhysicsComponent(mPlayers[i], CollisionType.Circle,
                    //    "player", new List<string>() { "player", "bullet" })).
                    AddComponent(new RenderComponent(mPlayers[i], Content.Load<Texture2D>("player" + (i + 1))));
                    //AddComponent(new LifeComponent(mPlayers[i], 30, 30));
                mPlayers[i].Index = (PlayerIndex)(i + 1);

                ((TransformComponent)mPlayers[i].GetComponent("Transform")).Position.X = 75 + i * 300;
                ((TransformComponent)mPlayers[i].GetComponent("Transform")).Position.Y = 300 + i * 120;

                mPlayers[i].Initialize();
                //mPlayers[i].SaveState();
            }
        }

        private void PlayerPlacementStartDraw()
        {
            RunningDraw();
        }

        private void PlayerPlacementRunningUpdate(GameTime gameTime)
        {
            int placed = 0;

            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i].Update(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);

                if ((mPlayers[i].GetComponent("PlayerPlacement") as PlayerPlacementComponent).IsPlaced())
                {
                    placed++;
                }
            }

            if (placed == mPlayers.Length)
            {
                mEvent.EventName = "Save";
                GameObject.BroadcastEvent(mEvent);
                mCurrentGameState = GameState.RunningStart;
            }
        }

        private void PlayerPlacementRunningDraw()
        {
            RunningDraw();
        }

        private void ModeSelectUpdate(GameTime gameTime)
        {
            InputState state = GamePadHandler.GetInstance().GetState(PlayerIndex.One);

            if (state.IsPressed("p1_down"))
            {
                mMenuPos = 1;
            }
            else
            {
                if (state.IsPressed("p1_up"))
                {
                    mMenuPos = 0;
                }
            }

            if (state.IsPressed("p1_shoot"))
            {
                mSelected = true;
            }
        }

        private void ModeSelectDraw()
        {
            float dmAlpha = 0.5f;
            float dsAlpha = 0.5f;

            if (mMenuPos == 0)
            {
                dmAlpha = 1f;
            }
            else
            {
                dsAlpha = 1f;
            }

            Vector2 size = mFont.MeasureString("Deathmatch");

            GraphicsDevice.Clear(Color.Black);

            mItemPos.X = Renderer.GetInstance().GetWidth() / 2 - size.X / 2;
            mItemPos.Y = Renderer.GetInstance().GetHeight() / 2 - size.Y - 4;
            spriteBatch.DrawString(mFont, "Deathmatch", mItemPos, Color.White * dmAlpha);

            size = mFont.MeasureString("Destruction");

            mItemPos.X = Renderer.GetInstance().GetWidth() / 2 - size.X / 2;
            mItemPos.Y = Renderer.GetInstance().GetHeight() / 2 + size.Y + 4;
            spriteBatch.DrawString(mFont, "Destruction", mItemPos, Color.White * dsAlpha);
        }

        private void RunningStartUpdate(GameTime gameTime)
        {
            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i].RemoveComponent("PlayerPlacement").
                    AddComponent(new PlayerInputComponent(mPlayers[i], i + 1)).
                    AddComponent(new BulletComponent(mPlayers[i])).
                    AddComponent(new PhysicsComponent(mPlayers[i], CollisionType.Circle,
                        "player", new List<string>() { "player", "bullet" })).
                    //AddComponent(new RenderComponent(mPlayers[i], Content.Load<Texture2D>("player" + (i + 1))));
                AddComponent(new LifeComponent(mPlayers[i], 30, 30));
                //mPlayers[i].Index = (PlayerIndex)(i + 1);

                //((TransformComponent)mPlayers[i].GetComponent("Transform")).Position.X = 75 + i * 300;
                //((TransformComponent)mPlayers[i].GetComponent("Transform")).Position.Y = 300 + i * 120;

                mPlayers[i].Initialize();
                //mPlayers[i].SaveState();
            }

            mCurrentGameState = GameState.Running;
        }

        private void RunningUpdate(GameTime gameTime)
        {
            // Defeat Condition
            int defeated = 0;
            for (int i = 0; i < mPlayers.Length; i++)
            {
                if (!mPlayers[i].IsAlive())
                {
                    defeated++;
                }
            }

            // Reset
            if (defeated >= mNumberOfPlayers - 1)
            {
                mEvent.EventName = "Reset";
                GameObject.BroadcastEvent(mEvent);
            }

            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i].Update(gameTime.ElapsedGameTime.Milliseconds / 1000.0f);
            }
        }

        private void RunningDraw()
        {
            if (mKinect.GetArena() != null)
            {
                spriteBatch.Draw(mKinect.GetArena(), mBlittingRectangle, null, Color.White,
                    0, Vector2.Zero, SpriteEffects.None, 1);
            }

            for (int i = 0; i < mPlayers.Length; i++)
            {
                mPlayers[i].Render(spriteBatch);
            }
        }
    }
}
