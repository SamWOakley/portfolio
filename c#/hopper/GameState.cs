using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Hopper
{
    interface IGameState
    {
        void LoadContent(ContentManager contentManager);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }
}
