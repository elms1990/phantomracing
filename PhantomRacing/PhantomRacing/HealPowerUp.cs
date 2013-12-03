using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public class HealPowerUp : GameObject
    {
        private PowerUpGenerator mParent;

        public HealPowerUp(PowerUpGenerator parent) : base("HealPowerUp") 
        {
            mParent = parent;
        }

        public override void Render(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Render(spriteBatch);
        }

        protected override void OnEvent(Event e)
        {
            if (e.EventName == "collision" &&
                e.Sender.GetId().Contains("player"))
            {
                e.EventName = "Heal";
                e.Receiver = e.Sender;
                e.Sender.SendEvent(e);

                World.GetInstance().Remove(this);
                mParent.Remove(this);

                return;
            }

            if (e.EventName == "Reset")
            {
                World.GetInstance().Remove(this);
                mParent.Remove(this);
            }
        }
    }
}
