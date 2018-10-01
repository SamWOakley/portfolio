using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Platformer.Items;
using Platformer.Blocks;

namespace Platformer.Characters
{
    sealed class Player : Character
    {
        const string ASSET_NAME = @"Characters/Pirate";
        const string JUMP_ASSET_NAME = @"SoundEffects/Jump";
        const string HURT_ASSET_NAME = @"SoundEffects/PlayerHurt";

        public int goldCoins;
        public int GoldCoins
        {
            get { return goldCoins; }
            set
            {
                goldCoins = value;
            }
        }

        int attackStage;
        int attackCooldown;

        KeyboardState previousKeyboardState;
        Weapon currentWeapon;

        SoundEffect jumpSound;
        SoundEffect hurtSound;


        public Player()
            : base(3)
        {
            GoldCoins = 0;
            MoveSpeed = 300;
            CanJump = true;
        }

        public override void Init(Vector2 position, Level level)
        {
            CurrentHealth = MaxHealth;
            this.Position = position;
            this.level = level;
            CurrentBlock = new Block.NullBlock();
        }

        public void LoadContent(ContentManager content)
        {
            jumpSound = content.Load<SoundEffect>(JUMP_ASSET_NAME);
            hurtSound = content.Load<SoundEffect>(HURT_ASSET_NAME);
            base.LoadContent(content, ASSET_NAME);
        }

        public void Update(GameTime gameTime, List<Item> items, List<NonPlayableCharacter> nonPlayableCharacters)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            if (CurrentBlock.GetType() == typeof(Block.ExitBlock))
                this.GetLevel().Win();

            Direction moveDirection;
            UpdateMovement(currentKeyboardState, out moveDirection);
            UpdateCharacterCollisions(nonPlayableCharacters);
            UpdateJump(currentKeyboardState);

            foreach (Item item in items)
            {
                for (int i = 0; i < GetBoundingBoxes().Length; i++)
                {
                    if (GetBoundingBoxes()[i].Intersects(new Rectangle((int)item.Position.X, (int)item.Position.Y, item.Size.Width, item.Size.Height)))
                        Collect(item);
                }
            }

            if (Movement.Y == 0)
                UpdateDuck(currentKeyboardState);

            if (currentKeyboardState.IsKeyDown(Keys.R))
                IsDead = true;
            /*else if (currentKeyboardState.IsKeyDown(Keys.Q) && previousKeyboardState.IsKeyDown(Keys.Q) == false)
                weaponDrawn = weaponDrawn ? false : true;*/

            if (currentWeapon != null)
            {
                if (currentKeyboardState.IsKeyDown(Keys.X))
                    Attack(nonPlayableCharacters);

                if (attackStage > 0)
                    UpdateAttack();
            }

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime, moveDirection);

            if (currentWeapon != null)
            {
                if (FaceDirection == Direction.Right)
                {
                    currentWeapon.Position = new Vector2(GetBoundingBoxes()[2].X + 25, GetBoundingBoxes()[2].Y);
                    if (attackStage == 0)
                        currentWeapon.Rotation = MathHelper.ToRadians(45);
                }
                else
                {
                    currentWeapon.Position = new Vector2(GetBoundingBoxes()[1].X - 15, GetBoundingBoxes()[1].Y);
                    if (attackStage == 0)
                        currentWeapon.Rotation = MathHelper.ToRadians(315);
                }
            }
        }

        private void UpdateMovement(KeyboardState currentKeyboardState, out Direction moveDirection)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Left))
                moveDirection = Direction.Left;
            else if (currentKeyboardState.IsKeyDown(Keys.Right))
                moveDirection = Direction.Right;
            else
                moveDirection = Direction.None;
        }

        private void UpdateDuck(KeyboardState currentKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                Ducking = true;
            }
            else if (Ducking == true)
                Ducking = false;
        }

        private void UpdateJump(KeyboardState currentKeyboardState)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Z) == true && previousKeyboardState.IsKeyDown(Keys.Z) == false && Movement.Y == 0)
                Jump();
            if (currentKeyboardState.IsKeyDown(Keys.Z) == true && Movement.Y < 0)
                Movement = new Vector2(Movement.X, Movement.Y + Movement.Y * 0.035f);
        }

        private void UpdateCharacterCollisions(List<NonPlayableCharacter> nonPlayableCharacters)
        {
            foreach (NonPlayableCharacter npc in nonPlayableCharacters)
            {
                for (int i = 0; i < npc.GetBoundingBoxes().Length; i++)
                {
                    for (int j = 0; j < this.GetBoundingBoxes().Length; j++)
                    {
                        if (npc.GetBoundingBoxes()[i].Intersects(this.GetBoundingBoxes()[j]))
                        {
                            if (npc.Personality == NonPlayableCharacter.PersonalityType.Attacking)
                            {
                                Hurt();
                            }
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (currentWeapon != null && Ducking == false)
                currentWeapon.Draw(spriteBatch);
        }

        public override void Hurt()
        {
            if (HurtTime == 0)
                hurtSound.Play();
            base.Hurt();
        }

        new private void Jump()
        {
            jumpSound.Play();
            base.Jump();
        }

        private void Collect(Item item)
        {
            item.Collect();

            if (item.GetType() == typeof(Coin))
                GoldCoins += 1;
            else if (item.GetType().IsSubclassOf(typeof(Weapon)))
                currentWeapon = (Weapon)item;

            GetLevel().RemoveItem(item);
        }


        private void Attack(List<NonPlayableCharacter> npcs)
        {
            if (attackStage == 0 && currentWeapon != null)
            {
                attackStage = 1;

                currentWeapon.Use();


                foreach (NonPlayableCharacter npc in npcs)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (npc.GetBoundingBoxes()[i].Intersects(new Rectangle((int)currentWeapon.Position.X, (int)currentWeapon.Position.Y, currentWeapon.Size.Width, currentWeapon.Size.Height)))
                            npc.Hurt();
                    }
                }
            }
        }

        private void UpdateAttack()
        {
            attackStage += 1;
            //currentWeapon.Rotation += MathHelper.ToRadians(10);

            if (attackStage >= 5)
            {
                attackStage = 0;
            }

            currentWeapon.Update(attackStage, (FaceDirection == Direction.Right) ? true : false);
        }

        public override void Reset()
        {
            GoldCoins = 0;
            base.Reset();
        }
    }
}
