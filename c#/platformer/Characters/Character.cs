using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Platformer.Blocks;

namespace Platformer.Characters
{
    abstract class Character : WorldObject
    {
        public enum Direction
        {
            Left,
            Right,
            None
        }

        Direction MoveDirection { get; set; }
        protected Direction FaceDirection { get; set; }


        protected Level level { get; set; }

        private const int GROUND_ACCELERATION = 20;

        private const int AIR_ACCELERATION = 10;

        private int maxHealth = 1;
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }

        private int currentHealth;
        public int CurrentHealth
        {
            get { return currentHealth; }
            protected set { currentHealth = value; }
        }

        private bool isDead;
        public bool IsDead
        {
            get { return isDead; }
            set
            {
                isDead = value;
            }
        }
        protected bool Ducking { get; set; }
        protected bool CanJump { get; set; }

        protected int WalkTimeCount { get; set; }
        protected int FallTimeCount { get; set; }
        protected int HurtTime { get; set; }
        int standTime;

        protected int MoveSpeed { get; set; }

        private Block currentBlock;
        protected Block CurrentBlock
        {
            get { return currentBlock; }
            set
            {
                currentBlock = value;
            }
        }

        public Character()
        { }

        public Character(int health)
        {
            MaxHealth = health;
            currentHealth = MaxHealth;
            IsDead = false;
            Visible = true;
            CurrentBlock = new Block.NullBlock();
        }

        public Character(Level level)
        {
            this.level = level;
            /*if (MaxHealth < 1)
                MaxHealth = 1;*/
            currentHealth = MaxHealth;
            IsDead = false;
            Visible = true;
            CurrentBlock = new Block.NullBlock();
        }

        public Character(Level level, Vector2 position)
            : this(level)
        {
            Position = position;
            StartPosition = position;
        }

        public Character(Level level, Vector2 position, int health)
            : this(level, position)
        {
            MaxHealth = health;
            currentHealth = MaxHealth;
        }

        public virtual void Init(Vector2 position, Level level) { }

        public virtual void Update(GameTime gameTime, Direction moveDirection)
        {
            this.MoveDirection = moveDirection;

            UpdateMovement();

            //Apply 'gravity'
            Movement = new Vector2(Movement.X, Movement.Y + 20);

            Vector2 nextPos = Position + Movement * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (nextPos.X < 0 || nextPos.X > (level.MapWidth - 1) * Block.BLOCK_WIDTH)
                Movement = new Vector2(0, Movement.Y);

            int blockPosX = (int)(Position.X + Size.Width) / Block.BLOCK_WIDTH;
            int blockPosY = (int)(Position.Y + Size.Height) / Block.BLOCK_HEIGHT;

            for (int x = blockPosX - 1; x < blockPosX + 1; x++)
            {
                for (int y = blockPosY - 1; y < blockPosY + 1; y++)
                {
                    DoCollisions(level.GetBlock(x, y), nextPos);
                }
            }

            if (Movement.X != 0)
            {
                if (WalkTimeCount > 20)
                    WalkTimeCount = 1;
                WalkTimeCount++;
            }
            else
                WalkTimeCount = 0;

            if (Movement.Y != 0)
                FallTimeCount++;
            else
                FallTimeCount = -10;


            if (Movement.X > 0)
                FaceDirection = Direction.Right;
            else if (Movement.X < 0)
                FaceDirection = Direction.Left;

            Position += Movement * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (HurtTime > 0)
                HurtTime--;

            if (CurrentHealth < 1 || Position.Y > level.MapHeight * Block.BLOCK_HEIGHT)
                Die();

            if (Movement.X == 0)
                WalkTimeCount = 0;

            if (Ducking)
                FaceDirection = Direction.None;

            //Rotation = Movement.X / 4000;
        }

        protected void UpdateMovement()
        {
            float blockAcceleration = GROUND_ACCELERATION * 1;

            blockAcceleration = GROUND_ACCELERATION * CurrentBlock.GetFriction();

            if (Ducking == true)
            {
                if (Movement.X > 0)
                    Movement = new Vector2(Movement.X - blockAcceleration, Movement.Y);
                else if (Movement.X < 0)
                    Movement = new Vector2(Movement.X + blockAcceleration, Movement.Y);
                if (Movement.X <= 10 && Movement.X >= -10)
                    Movement = new Vector2(0, Movement.Y);
                return;
            }

            if (MoveDirection == Direction.Left)
            {
                if (Movement.X > -MoveSpeed)
                    Movement = new Vector2(Movement.X - ((Movement.Y != 0) ? AIR_ACCELERATION : blockAcceleration), Movement.Y);
            }
            else if (MoveDirection == Direction.Right)
            {
                if (Movement.X < MoveSpeed)
                    Movement = new Vector2(Movement.X + ((Movement.Y != 0) ? AIR_ACCELERATION : blockAcceleration), Movement.Y);
            }
            else
            {
                if (Movement.X > 0)
                    Movement = new Vector2(Movement.X - ((Movement.Y != 0) ? AIR_ACCELERATION : blockAcceleration), Movement.Y);
                else if (Movement.X < 0)
                    Movement = new Vector2(Movement.X + ((Movement.Y != 0) ? AIR_ACCELERATION : blockAcceleration), Movement.Y);

                if (Movement.X <= 10 && Movement.X >= -10)
                    Movement = new Vector2(0, Movement.Y);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Movement.Y == 0)
            {
                if (FaceDirection == Direction.None)
                    Source = new Rectangle(0, 0, 64, 64);

                else if (FaceDirection == Direction.Left)
                {
                    if (WalkTimeCount > 0 && WalkTimeCount < 10)
                        Source = new Rectangle(64, 64, 64, 64);
                    else if (WalkTimeCount > 10)
                        Source = new Rectangle(64, 128, 64, 64);
                    else
                        Source = new Rectangle(64, 0, 64, 64);
                }

                else if (FaceDirection == Direction.Right)
                {
                    if (WalkTimeCount > 0 && WalkTimeCount < 10)
                        Source = new Rectangle(128, 64, 64, 64);
                    else if (WalkTimeCount > 10)
                        Source = new Rectangle(128, 128, 64, 64);
                    else
                        Source = new Rectangle(128, 0, 64, 64);
                }
            }

            else
                if (FallTimeCount < 0)
                    Source = new Rectangle(256, 0, 64, 64);
                else if (FallTimeCount > 0)
                    Source = new Rectangle(256, 64, 64, 64);

            if (Ducking == true)
                Source = new Rectangle(192, 0, 64, 64);

            if (HurtTime % 5 == 0)
                base.Draw(spriteBatch);


        }

        private void DoCollisions(Block block, Vector2 nextPos)
        {

            Rectangle characterBoundingBox = new Rectangle((int)nextPos.X, (int)nextPos.Y, 64, 65);
            Rectangle objectBoundingBox = new Rectangle((int)block.Position.X, (int)block.Position.Y, block.Size.Width, block.Size.Height);

            if (characterBoundingBox.Intersects(objectBoundingBox))
            {
                Rectangle head = GetBoundingBoxes()[0];
                Rectangle left = GetBoundingBoxes()[1];
                Rectangle right = GetBoundingBoxes()[2];
                Rectangle legs = GetBoundingBoxes()[3];

                if (head.Intersects(objectBoundingBox))
                {
                        if (Movement.Y < 0)
                        {
                            Movement = new Vector2(Movement.X, 0);
                            Console.WriteLine("T");
                        }
                }

                if (legs.Intersects(objectBoundingBox))
                {
                    CurrentBlock = block;

                        if (Movement.Y > 0)
                        {
                            Movement = new Vector2(Movement.X, 0);
                            Position = new Vector2(Position.X, block.Position.Y - Size.Height);
                            Console.WriteLine("D");
                        }
                }

                if (left.Intersects(objectBoundingBox))
                {
                        if (Movement.X < 0)
                        {
                            Movement = new Vector2(0, Movement.Y);
                            Console.WriteLine("L");
                        }
                }

                if (right.Intersects(objectBoundingBox))
                {
                        if (Movement.X > 0)
                        {
                            Movement = new Vector2(0, Movement.Y);
                            Console.WriteLine("R");
                        }
                }
            }
        }

        public Rectangle[] GetBoundingBoxes()
        {
            Rectangle head = new Rectangle((int)Position.X + 15, (int)Position.Y, 30, 24);
            Rectangle left = new Rectangle((int)Position.X + 14, (int)Position.Y + 25, 15, 20);
            Rectangle right = new Rectangle((int)Position.X + 36, (int)Position.Y + 25, 14, 20);
            Rectangle legs = new Rectangle((int)Position.X + 24, (int)Position.Y + 45, 14, 20);

            Rectangle[] boundingBoxes = { head, left, right, legs };

            return boundingBoxes;
        }

        protected virtual void Jump()
        {
            if (CanJump)
            {
                if (CurrentBlock.Position.Y - this.Position.Y < 80 && Movement.Y == 0)
                    Movement = new Vector2(Movement.X, Movement.Y - 400);
            }
        }

        public virtual void Hurt()
        {
            if (HurtTime == 0)
            {
                currentHealth -= 1;
                if (Movement.Y >= 0)
                    Movement = new Vector2(Movement.X, Movement.Y - 300);
                HurtTime = 100;
            }
        }

        private void Die()
        {
            IsDead = true;
        }

        public override void Reset()
        {
            Movement = Vector2.Zero;
            IsDead = false;
            HurtTime = 0;
            currentHealth = MaxHealth;
            Position = StartPosition;
            base.Reset();
        }

        public Level GetLevel()
        {
            return level;
        }

        void Speak(string speech)
        {
            level.ShowSpeechBubble(this.Position, speech);
        }
    }
}
