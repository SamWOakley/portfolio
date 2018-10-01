using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using Platformer.Characters;

namespace Platformer.Items
{
    class Coin : Item
    {
        const string ASSET_NAME = @"Collectables/CoinCollectable";
        const string PICKUP_ASSET_NAME = @"SoundEffects/CoinPickup";

        SoundEffect PickupSound;

        public Coin(Vector2 position)
            :base(position)
        {
            StartPosition = position;
            Position = position;
        }

        public override void LoadContent(ContentManager content)
        {
            PickupSound = content.Load<SoundEffect>(PICKUP_ASSET_NAME);
            base.LoadContent(content, ASSET_NAME);
        }

        public override void Collect()
        {
            PickupSound.Play();
            base.Collect();
        }
    }
}
