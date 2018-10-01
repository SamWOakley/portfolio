using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Platformer.Characters
{
    abstract class NonPlayableCharacter : Character
    {
        public enum PersonalityType
        {
            Ignorant,
            Friendly,
            Fearing,
            Attacking
        }

        public readonly PersonalityType Personality;

        private int AwarenessRadius;

        public NonPlayableCharacter(PersonalityType personality, int awareness, Vector2 position, Level level)
            : base(level, position)
        {
            Personality = personality;
            AwarenessRadius = awareness;
        }

        public void Update(GameTime gameTime, Player player)
        {
            Direction moveDirection;

            AIUpdate(player, out moveDirection);
            base.Update(gameTime, moveDirection);

            if (IsDead == true)
                GetLevel().RemoveCharacter(this);
        }

        public virtual void LoadContent(ContentManager content)
        {
            base.LoadContent(content, null);
        }

        private void AIUpdate(Player player, out Direction moveDirection)
        {
            /*if (Personality == PersonalityType.Ignorant)
            {
            }

            else if (Personality == PersonalityType.Friendly)
            {
                if (Vector2.Distance(this.Position, player.Position) < AwarenessRadius)
                {
                    Movement.X = this.Position.X - player.Position.X;
                    if (player.Position.Y > this.Position.Y)
                        Jump();
                }

            }

            else if (Personality == PersonalityType.Fearing)
            {
                if (Vector2.Distance(this.Position, player.Position) < AwarenessRadius)
                {
                    Movement.X = this.Position.X - player.Position.X;
                }
                if (player.Position.Y < this.Position.Y)
                    Jump();
            }*/
            moveDirection = Direction.None;

            if (Personality == PersonalityType.Attacking)
            {
                if (Vector2.Distance(this.Position, player.Position) < AwarenessRadius)
                {
                    //Movement.X = player.Position.X - this.Position.X;

                    if (player.Position.X - this.Position.X > 0)
                    {
                        moveDirection = Direction.Right;

                        if (GetLevel().GetBlock((int)(this.Position.X / Blocks.Block.BLOCK_WIDTH) + 1, (int)(this.Position.Y / Blocks.Block.BLOCK_HEIGHT) + 1) == null)
                        {
                            if (CanJump)
                                Jump();
                            else moveDirection = Direction.None;
                        }
                        else if (GetLevel().GetBlock((int)(this.Position.X / Blocks.Block.BLOCK_WIDTH) + 1, (int)(this.Position.Y / Blocks.Block.BLOCK_HEIGHT)) != null)
                            Jump();
                    }
                    else if (player.Position.X - this.Position.X < 0)
                    {
                        moveDirection = Direction.Left;

                        /*if (GetLevel().GetBlock((int)(this.Position.X / Blocks.Block.BLOCK_WIDTH) - 1, (int)(this.Position.Y / Blocks.Block.BLOCK_HEIGHT) + 1).GetType() == typeof(Blocks.EmptyBlock))
                            if (CanJump)
                                Jump();
                            else moveDirection = MoveDirection.None;
                        else if (GetLevel().GetBlock((int)(this.Position.X / Blocks.Block.BLOCK_WIDTH) - 1, (int)(this.Position.Y / Blocks.Block.BLOCK_HEIGHT)).GetType().IsSubclassOf(typeof(Blocks.SolidBlock)))
                            Jump();*/
                    }
                    if (player.Position.Y < this.Position.Y)
                    {
                        Jump();
                        if (Movement.Y < 0)
                            Movement = new Vector2(Movement.X, Movement.Y * 0.035f);
                    }
                }
            }
        }
    }
}
