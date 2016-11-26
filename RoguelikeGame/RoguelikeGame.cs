using EntityNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;

namespace RoguelikeGameNamespace
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RoguelikeGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont font;

        private Player player;
        public static List<Enemy> enemies = new List<Enemy>();

        private double health = 100.0;

        private enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }

        public RoguelikeGame()
        {
            graphics = new GraphicsDeviceManager(this);
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
            IsMouseVisible = true;

            player = new Player("Player", 0, 0);
            enemies.Add(new Enemy("Enemy", 120, 80));
            enemies.Add(new Enemy("Enemy", 100, 100));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.LoadContent(this.Content);

            foreach (Enemy enemy in enemies)
            {
                enemy.LoadContent(this.Content);
            }

            font = Content.Load<SpriteFont>("Health");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            player.Update(gameTime);

            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            string healthText = "Health: " + health;
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            player.Draw(this.spriteBatch);

            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(this.spriteBatch);
            }

            spriteBatch.DrawString(font, healthText, new Vector2(0, graphics.GraphicsDevice.PresentationParameters.Bounds.Height - font.MeasureString(healthText).Y), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
