using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Platformer.Blocks
{
    class LongGrass : WorldObject
    {
        const string ASSET_NAME = "LongGrass";

        public LongGrass(int x, int y)
        {
            this.Position = new Vector2(x, y);
        }

        public void LoadContent(ContentManager content)
        {
            base.LoadContent(content, ASSET_NAME);
        }
    }
}
