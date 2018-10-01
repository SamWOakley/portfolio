using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Camera
    {
        public static int ViewWidth { get; set; }
        public static int ViewHeight { get; set; }

        public static int WorldWidth { get; set; }
        public static int WorldHeight { get; set; }

        private static int MoveSpeed = 10;

        private static Vector2 position;
        public static Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(MathHelper.Clamp(value.X, 0, WorldWidth - ViewWidth), MathHelper.Clamp(value.Y, 0, WorldHeight - ViewHeight));
            }
        }

        private static float scale = 1.0f;
        public static float Scale
        {
            get { return scale; }
            set
            {
                scale = MathHelper.Clamp(value, (float)Game1.ScreenHeight / (float)WorldHeight, 2.0f);

                float screenWidth = Game1.ScreenWidth / scale;
                float screenHeight = Game1.ScreenHeight / scale;

                ViewWidth = (int)screenWidth;
                ViewHeight = (int)screenHeight;
            }
        }

        public static Vector2 WorldToScreen(Vector2 objectPosition)
        {
            return objectPosition - Position;
        }

        public static void Move(Vector2 targetPos)
        {
            Vector2 target = new Vector2(targetPos.X - (ViewWidth / 2), targetPos.Y - (ViewHeight / 2));

            Position = Vector2.Lerp(Position, target, 0.1f);
        }

        public BasicEffect Effect { get; set; }

        public SpriteBatch SpriteBatch { get; set; }
    }
}
