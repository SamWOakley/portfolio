using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer.Items
{
    class Pistol : Weapon
    {
        const string ASSET_NAME = @"Collectables/Weapons/Pistol";
        const string BULLET_ASSET_NAME = @"Bullet";

        Texture2D bulletTexture;

        bool firing;

        public Pistol(Vector2 position)
            : base(position) { }

        public override void LoadContent(ContentManager content)
        {
            bulletTexture = content.Load<Texture2D>(BULLET_ASSET_NAME);
            base.LoadContent(content, ASSET_NAME);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (firing)
                spriteBatch.Draw(bulletTexture, new Rectangle((int)this.Position.X, (int)this.Position.Y, 1000, 500), Color.White);
        }

        public override void Use()
        {
            firing = true;
        }

        public override void Update(int attackStage, bool rightFacing)
        {
            
        }
    }
}