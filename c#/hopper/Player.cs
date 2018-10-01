using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Hopper
{
    class Player
    {
        Texture2D sprite;
        Vector2 position, displacement;
        private const float PLAYER_ACCELERATION_X = 1, MAX_DISPLACEMENT_X = 10;

        public Player()
        {
            position = new Vector2(Constants.SCREEN_WIDTH / 2, 50);
            displacement = Vector2.Zero;
        }

        public void LoadContent(ContentManager contentManager)
        {
            sprite = contentManager.Load<Texture2D>("player_sprite");
        }

        public void Update(GameTime gameTime)
        {
            displacement.Y += 0.5f;

            position += displacement;

            if (position.X > Constants.SCREEN_WIDTH)
                position.X = 0;
            else if (position.X < 0)
                position.X = Constants.SCREEN_WIDTH;
        }

        public void UpdateInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Left))
                displacement.X -= PLAYER_ACCELERATION_X;
            else if (keyboardState.IsKeyDown(Keys.Right))
                displacement.X += PLAYER_ACCELERATION_X;
            else
            {
                if (Math.Abs(displacement.X) < PLAYER_ACCELERATION_X)
                    displacement.X = 0;
                else
                    displacement.X += (displacement.X < 0) ? PLAYER_ACCELERATION_X : -PLAYER_ACCELERATION_X;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, Color.White);
        }

        public void Jump()
        {
            displacement.Y = -10;
        }

        public Rectangle GetBoundingBox()
        {
            return new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
        }

        public bool IsFalling()
        {
            return (displacement.Y > 0);
        }
    }
}
