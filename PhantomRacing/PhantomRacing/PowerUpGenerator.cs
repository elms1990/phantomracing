using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class PowerUpGenerator : GameObject
    {
        private readonly int SPAWN_INTERVAL = 6000;
        private readonly int MAX_POWERUPS = 5;
        private int mCurrentTimer = 0;
        private int mAlivePowerUps = 0;
        private bool mSpawn = false;
        private readonly string[] mPowerUps = { "Heal" };
        private List<GameObject> mActive = new List<GameObject>();

        public PowerUpGenerator()
            : base("PowerUpGenerator")
        {
        }

        public override void Update(float timeStep)
        {
            mCurrentTimer += (int)(timeStep * 1000);

            if (mCurrentTimer >= SPAWN_INTERVAL)
            {
                mCurrentTimer = 0;
                mSpawn = true;
                Random rnd = new Random();
                SpawnPowerUp(mPowerUps[rnd.Next(mPowerUps.Length - 1)]);
            }

            foreach (GameObject go in mActive)
            {
                go.Update(timeStep);
            }

        }

        public override void Render(SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);

            foreach (GameObject go in mActive)
            {
                go.Render(spriteBatch);
            }
        }

        public void SpawnPowerUp(String which)
        {
            if (mActive.Count >= MAX_POWERUPS)
            {
                return;
            }

            switch (which)
            {
                case "Heal":
                    CreateHealPowerUp();
                    break;
            }
        }

        private HealPowerUp CreateHealPowerUp()
        {
           
            TransformComponent transform = new TransformComponent();
            transform.Position.Z = 0.77f;
            HealPowerUp heal = new HealPowerUp(this);
            RenderComponent renderer = new RenderComponent(heal, AssetLoader.GetInstance().LoadAsset<Texture2D>("heal"));

            heal.AddComponent(transform).
                AddComponent(renderer).
                AddComponent(new PhysicsComponent(heal, CollisionType.Circle, "powerup", new List<String>() { "player" }));
            heal.Initialize();

            mActive.Add(heal);

            bool collide;
            int i = 0;
            int max = 10;
            do
            {
                Random rnd = new Random();
                transform.Position.X = rnd.Next(Renderer.GetInstance().GetWidth() - renderer.GetWidth() - 1);
                transform.Position.Y = rnd.Next(Renderer.GetInstance().GetHeight() - renderer.GetHeight() - 1);

                collide = PhysicsHelper.CheckPixelCollision(heal, KinectManager.GetInstance().GetRawArena(),
               ((float)KinectManager.GetInstance().GetColorFrameWidth()) / renderer.GetWidth(),
                ((float)KinectManager.GetInstance().GetColorFrameHeight()) / renderer.GetHeight());

                i++;

                if (i == max)
                {
                    break;
                }
            } while (collide);
            

            return heal;
        }

        public void Remove(GameObject obj)
        {
            mActive.Remove(obj);
        }
    }
}
