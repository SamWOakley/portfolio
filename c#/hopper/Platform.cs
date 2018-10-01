using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Hopper
{
    class Platform
    {
        Vector2 position;
        static Texture2D sprite;

        public Platform(Vector2 position)
        {
            this.position = position;
        }

        public static void LoadContent(ContentManager contentManager)
        {
            sprite = contentManager.Load<Texture2D>("platform_sprite");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }
    }
}
