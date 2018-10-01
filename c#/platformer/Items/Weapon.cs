using Microsoft.Xna.Framework;

using Platformer.Characters;

namespace Platformer.Items
{
    abstract class Weapon : Item
    {
        bool held;
        public bool Held { get { return held; } }

        public Weapon(Vector2 position)
            : base(position) { }

        public override void Collect()
        {
            held = true;
            base.Collect();
            Visible = true;
        }

        public virtual void Use() { }

        public virtual void Update(int attackStage, bool rightFacing) { }
    }
}
