using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Hopper
{
    class PlayState : IGameState
    {
        Player player;
        List<Platform> platforms;

        public PlayState()
        {
            player = new Player();
            platforms = new List<Platform>();

            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                Platform platform = new Platform(new Vector2(random.Next(Constants.SCREEN_WIDTH), random.Next(Constants.SCREEN_HEIGHT)));
                platforms.Add(platform);
            }
        }

        public void LoadContent(ContentManager contentManager)
        {
            player.LoadContent(contentManager);
            Platform.LoadContent(contentManager);
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            foreach (Platform platform in platforms)
            {
                if (platform.GetBoundingBox().Intersects(player.GetBoundingBox()) && player.IsFalling())
                    player.Jump();

            }

            player.UpdateInput(keyboardState);
            player.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            player.Draw(spriteBatch);

            foreach (Platform platform in platforms)
            {
                platform.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

    }
}
