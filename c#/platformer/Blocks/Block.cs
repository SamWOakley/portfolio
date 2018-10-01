using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer.Blocks
{
    class Block : WorldObject
    {
        public class DirtBlock : Block
        {
            public DirtBlock()
                : base(0.8f, @"Blocks/Dirt") { }
        }

        public class GrassBlock : Block
        {
            public GrassBlock()
                : base(0.9f, @"Blocks/Grass") { }
        }

        public class StoneBlock : Block
        {
            public StoneBlock()
                : base(1, @"Blocks/Stone") { }
        }

        public class IceBlock : Block
        {
            public IceBlock()
                : base(0.1f, @"Blocks/Ice") { }
        }

        public class ExitBlock : Block
        {
            public ExitBlock()
                : base(1, @"Blocks/Exit") { }
        }

        public class NullBlock : Block
        {
            public NullBlock()
                : base(0, @"Blocks/EmptyBlock") { }
        }

        private readonly float friction;

        public const int BLOCK_WIDTH = 64;
        public const int BLOCK_HEIGHT = 64;

        private string assetName;

        private Block(string assetName)
            : this(0, assetName) { }

        public Block(float friction, string assetName)
        {
            this.assetName = assetName;
            this.friction = friction;
        }

        public void LoadContent(ContentManager content, Vector2 position)
        {
            base.LoadContent(content, assetName);
            Position = new Vector2(position.X * Size.Width, position.Y * Size.Height);
            //Position = position;
        }

        public float GetFriction()
        {
            return friction;
        }
    }
}
