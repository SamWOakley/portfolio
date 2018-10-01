using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Platformer
{
    class Overlay
    {
        SpriteFont OverlayFont;

        Texture2D HealthIcon;
        Texture2D CoinIcon;

        const string COIN_ASSET_NAME = "Coins";
        const string HEALTH_ASSET_NAME = "Hearts";

        int CurrentHearts;
        int MaxHearts;

        int GoldCoins;

        public void LoadContent(ContentManager content)
        {
            CoinIcon = content.Load<Texture2D>(COIN_ASSET_NAME);
            HealthIcon = content.Load<Texture2D>(HEALTH_ASSET_NAME);
            OverlayFont = content.Load<SpriteFont>("GameFont");
        }
        
        public void Update(int maxHearts, int currentHearts, int goldCoins)
        {
            MaxHearts = maxHearts;
            CurrentHearts = currentHearts;
            GoldCoins = goldCoins;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CoinIcon, new Rectangle(0, 50, CoinIcon.Width, CoinIcon.Height), Color.White);
            spriteBatch.DrawString(OverlayFont, GoldCoins.ToString(), new Vector2(20, 45), Color.White);

            for (int i = 0; i < CurrentHearts; i++)
            {
                spriteBatch.Draw(HealthIcon, new Rectangle(i * HealthIcon.Width, 0, HealthIcon.Width / 2, HealthIcon.Height), new Rectangle(0, 0, HealthIcon.Width / 2, HealthIcon.Height), Color.White);
            }
            if (CurrentHearts < MaxHearts)
            {
                for (int i = CurrentHearts; i < MaxHearts; i++)
                {
                    spriteBatch.Draw(HealthIcon, new Rectangle(i * HealthIcon.Width, 0, HealthIcon.Width / 2, HealthIcon.Height), new Rectangle(HealthIcon.Width / 2, 0, HealthIcon.Width / 2, HealthIcon.Height), Color.White);
                }
            }
        }
    }
}
