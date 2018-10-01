using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Platformer.Characters;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont gameFont;

        Level level;
        Player player;

        int levelCount;

        Matrix viewMatrix;
        Matrix projectionMatrix;

        Vector3 cameraPosition;

        BasicEffect effect;

        private static int screenWidth = 850;
        public static int ScreenWidth
        {
            get { return screenWidth; }
        }

        private static int screenHeight = 600;
        public static int ScreenHeight
        {
            get { return screenHeight; }
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Camera.ViewWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = Camera.ViewHeight = ScreenHeight;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            levelCount = 1;
            player = new Player();
            level = new Level(levelCount, player);

            cameraPosition = new Vector3(0, 0, -10);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 100.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, new Vector3(cameraPosition.X, cameraPosition.Y, 0), Vector3.Up);
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            level.LoadContent(this.Content, this);
            player.LoadContent(this.Content);
            gameFont = Content.Load<SpriteFont>("GameFont");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content./// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                Camera.Scale += 0.01f;
            else if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                Camera.Scale -= 0.01f;

            level.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(ClearOptions.DepthBuffer | ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            level.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void Win()
        {
            this.Exit();
        }

        public void Lose()
        {
            this.Exit();
        }

        internal void ChangeLevel()
        {
            levelCount++;
            level = new Level(levelCount, player);
            LoadContent();
        }
    }
}
