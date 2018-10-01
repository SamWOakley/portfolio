using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Platformer.Items
{
    class Sword : Weapon
    {
        const string ASSET_NAME = @"Collectables/Weapons/Sword";

        public Sword(Vector2 position)
            : base(position) { }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content, ASSET_NAME);
            Origin = new Vector2(this.Size.Width / 2, this.Size.Height / 2);
        }

        public override void Update(int attackStage, bool rightFacing)
        {
            this.Rotation = MathHelper.ToRadians(attackStage * ((rightFacing) ? 10 : -10));
        }
    }
}