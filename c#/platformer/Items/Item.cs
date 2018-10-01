using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Platformer.Characters;

namespace Platformer.Items
{
    abstract class Item : WorldObject
    {

        protected bool Collected { get; set; }

        public virtual void LoadContent(ContentManager content)
        {
        }

        public virtual void Collect()
        {
            Remove();
        }

        public Item(Vector2 position)
            : base(position) { }
    }
}