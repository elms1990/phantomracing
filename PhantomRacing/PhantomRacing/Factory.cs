using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace PhantomRacing
{
    public static class Factory
    {
        public static GameObject CreatePlayer(String id, Texture2D playerTexture)
        {
            GameObject go = new GameObject(id);
            return go.AddComponent(new TransformComponent()).
                AddComponent(new PlayerInputComponent(go)).
                AddComponent(new RenderComponent(go, playerTexture));
        }
    }
}
