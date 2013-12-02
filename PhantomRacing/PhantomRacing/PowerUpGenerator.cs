using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public class PowerUpGenerator : GameObject
    {
        private readonly int SPAWN_INTERVAL = 10000;
        private readonly int MAX_POWERUPS = 3;
        private int mCurrentTimer = 0;
        private int mAlivePowerUps = 0;
        private bool mSpawn = false;
        private readonly string[] mPowerUps = { "Heal" };

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
        }

        public void SpawnPowerUp(String which)
        {
            switch (which)
            {
                case "Heal":
                    CreateHealPowerUp();
                    break;
            }
        }

        private HealPowerUp CreateHealPowerUp()
        {
            HealPowerUp heal = new HealPowerUp();
            TransformComponent transform = new TransformComponent();
            RenderComponent renderer = new RenderComponent(heal, AssetLoader.GetInstance().LoadAsset<Texture2D>("heal"));


            heal.AddComponent(transform).
                AddComponent(renderer).
                AddComponent(new PhysicsComponent(this, CollisionType.Circle, "powerup", new List<String>() { "player" }));
            heal.Initialize();

            bool collide;
            int i = 0;
            int max = 10;
            do
            {
                Random rnd = new Random();
                transform.Position.X = rnd.Next(Renderer.GetInstance().GetWidth() - renderer.GetWidth() - 1);
                transform.Position.Y = rnd.Next(Renderer.GetInstance().GetHeight() - renderer.GetHeight() - 1);

                collide = PhysicsHelper.CheckPixelCollision(this, KinectManager.GetInstance().GetRawArena(),
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
    }
}
