using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Platformer.Characters
{
    class Zombie : NonPlayableCharacter
    {
        const string ASSET_NAME = "Characters/Zombie";
        const int MOVE_SPEED = 50;

        public Zombie(Vector2 position, Level level)
            : base(PersonalityType.Attacking, 250, position, level)
        {
            MoveSpeed = MOVE_SPEED;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content, ASSET_NAME);
        }
    }
}
