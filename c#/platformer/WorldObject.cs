
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Platformer
{
    abstract class WorldObject
    {
        private Texture2D texture;
        private Color spriteColor;

        protected bool Visible { get; set; }

        public float Rotation { get; set; }

        Rectangle source;
        public Rectangle Source
        {
            get { return source; }
            set
            {
                source = value;
                Size = new Rectangle(0, 0, source.Width * (int)Scale, source.Height * (int)Scale);
            }
        }

        public Rectangle Size { get; set; }

        float scale = 1;
        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                Size = new Rectangle(0, 0, Source.Width * (int)scale, Source.Height * (int)scale);
            }
        }

        public WorldObject()
            : this(Vector2.Zero) { }

        public WorldObject(Vector2 pos)
        {
            this.Position = pos;
            StartPosition = pos;
            Visible = true;
            spriteColor = Color.White;
        }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(value.X, value.Y);
            }
        }
        protected Vector2 Movement { get; set; }
        protected Vector2 StartPosition { get; set; }
        protected Vector2 Origin { get; set; }

        public virtual void LoadContent(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
            Source = new Rectangle(0, 0, texture.Width, texture.Height);
            Size = new Rectangle(0, 0, Source.Width * (int)Scale, Source.Height * (int)Scale);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Visible == true)
                spriteBatch.Draw(texture, Camera.WorldToScreen(Position) * Camera.Scale, Source, spriteColor, Rotation, Origin, Scale * Camera.Scale, SpriteEffects.None, 1f);
        }

        protected void SetColor(Color color)
        {
            spriteColor = color;
        }

        public void Remove()
        {
            Position = Vector2.Zero;
            Visible = false;
        }

        public virtual void Reset()
        {
            Position = StartPosition;
            Visible = true;
        }
    }
}
