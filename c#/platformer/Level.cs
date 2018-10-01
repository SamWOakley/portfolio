using System;
using System.Threading;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Platformer.Characters;
using Platformer.Blocks;
using Platformer.Items;

namespace Platformer
{
    class BlockColumn
    {
        public List<Block> Columns = new List<Block>();
    }

    class Level
    {
        Game1 game;

        Texture2D sky;
        Texture2D backgroundScenery;
        Texture2D mapPlan;

        Overlay gameOverlay;

        Player player;

        List<BlockColumn> rows;
        List<Item> collectableItems;
        List<Item> itemsToRemove;
        List<NonPlayableCharacter> NPCs;
        List<NonPlayableCharacter> npcsToRemove;
        List<LongGrass> grasses;
        List<Cloud> clouds;

        Random rand = new Random();

        public int MapWidth { get; set; }
        public int MapHeight { get; set; }

        Vector2 playerSpawnPoint;

        private readonly int levelCount;

        public Level(int levelCount)
        {
            this.levelCount = levelCount;
        }

        public Level(int levelCount, Player player)
            : this(levelCount)
        {
            this.player = player;
        }

        public void LoadContent(ContentManager content, Game1 game)
        {
            this.game = game;

            mapPlan = content.Load<Texture2D>(@"Maps/Map" + levelCount);
            MapWidth = mapPlan.Width;
            MapHeight = mapPlan.Height;

            Camera.WorldHeight = mapPlan.Height * Block.BLOCK_HEIGHT;
            Camera.WorldWidth = mapPlan.Width * Block.BLOCK_WIDTH;



            sky = content.Load<Texture2D>("Sky");
            backgroundScenery = content.Load<Texture2D>("Scenery");

            rows = new List<BlockColumn>();
            collectableItems = new List<Item>();
            itemsToRemove = new List<Item>();
            NPCs = new List<NonPlayableCharacter>();
            npcsToRemove = new List<NonPlayableCharacter>();

            Color[] colors = new Color[MapWidth * MapHeight];
            mapPlan.GetData<Color>(colors);

            int i = 0;

            grasses = new List<LongGrass>();

            for (int y = 0; y < MapHeight; y++)
            {
                BlockColumn thisRow = new BlockColumn();
                for (int x = 0; x < MapWidth; x++)
                {
                    Block aBlock = new Block.NullBlock();
                    LongGrass grass;

                    if (colors[i] == Color.Green)
                    {
                        aBlock = new Block.GrassBlock();
                        if (rand.Next(0, 10) > 5)
                        {
                            grass = new LongGrass(x * Block.BLOCK_WIDTH, (y - 1) * Block.BLOCK_HEIGHT);
                            grasses.Add(grass);
                        }
                    }
                    else if (colors[i] == Color.Brown)
                        aBlock = new Block.DirtBlock();
                    else if (colors[i] == Color.Gray)
                        aBlock = new Block.StoneBlock();
                    else if (colors[i] == Color.Aqua)
                        aBlock = new Block.IceBlock();
                    else if (colors[i] == Color.Yellow)
                    {
                        Coin coin = new Coin(new Vector2(x * Block.BLOCK_WIDTH, y * Block.BLOCK_HEIGHT));
                        collectableItems.Add(coin);
                    }
                    else if (colors[i] == Color.Red)
                    {
                        Zombie zomb = new Zombie(new Vector2(x * Block.BLOCK_WIDTH, y * Block.BLOCK_HEIGHT), this);
                        NPCs.Add(zomb);
                    }
                    else if (colors[i] == Color.Blue)
                    {
                        Weapon weapon = new Sword(new Vector2(x * Block.BLOCK_WIDTH, y * Block.BLOCK_HEIGHT));
                        collectableItems.Add(weapon);
                    }
                    else if (colors[i] == Color.Black)
                        aBlock = new Block.ExitBlock();

                    if (colors[i] == Color.White)
                        playerSpawnPoint = new Vector2(x * Block.BLOCK_WIDTH, y * Block.BLOCK_HEIGHT);

                        aBlock.LoadContent(content, new Vector2(x, y));

                    thisRow.Columns.Add(aBlock);

                    i++;
                }

                rows.Add(thisRow);
            }

            clouds = new List<Cloud>();

            int cloudDirection = rand.Next(0, 1);
            if (cloudDirection == 0)
                cloudDirection = -1;
            int cloudSpeed = rand.Next(20, 100);

            for (int j = 0; j < rand.Next(50, 100); j++)
            {
                Cloud aCloud = new Cloud(rand.Next(0, Camera.WorldWidth), cloudDirection, cloudSpeed);
                clouds.Add(aCloud);
            }

            foreach (Cloud aCloud in clouds)
                aCloud.LoadContent(content);

            foreach (LongGrass grass in grasses)
            {
                grass.LoadContent(content);
            }

            foreach (Item collectable in collectableItems)
            {
                collectable.LoadContent(content);
            }

            foreach (NonPlayableCharacter npc in NPCs)
            {
                npc.LoadContent(content);
            }

            player.Init(playerSpawnPoint, this);

            gameOverlay = new Overlay();
            gameOverlay.LoadContent(content);
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime, collectableItems, NPCs);

            foreach (NonPlayableCharacter npc in NPCs)
                npc.Update(gameTime, player);

            gameOverlay.Update(player.MaxHealth, player.CurrentHealth, player.GoldCoins);
			
            Camera.Move(new Vector2(player.Position.X + player.Size.Width / 2, player.Position.Y + player.Size.Height / 2));

            if (player.IsDead == true)
            {
                //Game.Lose();
                Reset();
            }

            foreach (Cloud aCloud in clouds)
                aCloud.Update(gameTime);

            foreach (Item item in itemsToRemove)
                collectableItems.Remove(item);

            foreach (NonPlayableCharacter npc in npcsToRemove)
                NPCs.Remove(npc);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sky, new Rectangle((int)-Camera.Position.X / 50, 0, Camera.WorldWidth, Camera.WorldHeight), Color.White);

            spriteBatch.Draw(backgroundScenery, new Rectangle((int)-Camera.Position.X / 20, (int)-Camera.Position.Y / 20, Camera.WorldWidth, Camera.WorldHeight), Color.White);

            foreach (Item collectable in collectableItems)
                collectable.Draw(spriteBatch);

            foreach (NonPlayableCharacter npc in NPCs)
                npc.Draw(spriteBatch);

            player.Draw(spriteBatch);
            foreach (LongGrass grass in grasses)
                grass.Draw(spriteBatch);

            Vector2 firstSquare = new Vector2(Camera.Position.X / Block.BLOCK_WIDTH, Camera.Position.Y / Block.BLOCK_HEIGHT);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            if (firstX < 0)
                firstX = 0;

            if (firstY < 0)
                firstY = 0;

            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                        rows[y].Columns[x].Draw(spriteBatch);
                }
            }

            foreach (Cloud aCloud in clouds)
                aCloud.Draw(spriteBatch);

            gameOverlay.Draw(spriteBatch);
        }

        private void Reset()
        {
            player.Reset();

            foreach (NonPlayableCharacter npc in NPCs)
                npc.Reset();

            foreach (Item collectable in collectableItems)
                collectable.Reset();
        }

        public Block GetBlock(int x, int y)
        {
            if (y > MapHeight - 1 || x < 0 || x > MapWidth - 1 || y < 0)
                return null;

            return rows[y].Columns[x];
        }

        public void Win()
        {
            game.ChangeLevel();
            //Game.Win();
        }

        public void RemoveItem(Item item)
        {
            itemsToRemove.Add(item);
        }

        public void RemoveCharacter(NonPlayableCharacter npc)
        {
            npcsToRemove.Add(npc);
        }

    }
}
