using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Platformer
{
    class Cloud : WorldObject
    {
        const string ASSET_NAME = "Cloud";

        public Cloud(int startPos, int movement, int speed)
        {
            Position = new Vector2(startPos, 300);

            Movement = new Vector2(movement * speed, 0);

            SetColor(new Color(200, 200, 200, 150));
        }

        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, ASSET_NAME);
        }

        public void Update(GameTime gameTime)
        {
            Position += Movement * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Position.X > Camera.WorldWidth)
                Position = new Vector2(0, Position.Y);
            else if (Position.X + Size.Width < 0)
                Position = new Vector2(Camera.WorldWidth, Position.Y);
        }
    }
}
