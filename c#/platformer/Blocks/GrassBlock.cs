namespace Platformer.Blocks
{
    class GrassBlock : SolidBlock
    {
        const string ASSET_NAME = @"Blocks/Grass";
        const float BLOCK_FRICTION = 0.9f;

        public GrassBlock()
            : base(BLOCK_FRICTION, ASSET_NAME) { }
    }
}
