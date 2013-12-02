using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhantomRacing
{
    public class HealPowerUp : GameObject
    {
        public HealPowerUp() : base("HealPowerUp") 
        {
        }

        protected override void OnEvent(Event e)
        {
            if (e.EventName == "collision" &&
                e.Sender.GetId().Contains("player"))
            {
                e.EventName = "Receive";
                e.Sender.SendEvent(e);
            }
        }
    }
}
