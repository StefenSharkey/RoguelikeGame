﻿using EntityNamespace;

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

        private SpriteFont titleFont;
        private SpriteFont textFont;

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
        private GameState currentGameState = GameState.StartMenu;

        private Vector2 titlePos;
        private Vector2 startButtonPos;
        private Vector2 exitButtonPos;

        private string titleText = "Roguelike Game";
        private string startButtonText = "Start";
        private string exitButtonText = "Exit";

        private Rectangle startButtonRectangle;
        private Rectangle exitButtonRectangle;

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

            currentGameState = GameState.StartMenu;

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

            titleFont = Content.Load<SpriteFont>("Title");
            textFont = Content.Load<SpriteFont>("Text");

            titlePos = new Vector2((graphics.GraphicsDevice.Viewport.Width - titleFont.MeasureString(titleText).X) / 2, 0);
            startButtonPos = new Vector2((graphics.GraphicsDevice.Viewport.Width - textFont.MeasureString(startButtonText).X) / 2, titleFont.MeasureString(titleText).Y);
            exitButtonPos = new Vector2((graphics.GraphicsDevice.Viewport.Width - textFont.MeasureString(exitButtonText).X) / 2, startButtonPos.Y + textFont.MeasureString(startButtonText).Y);

            startButtonRectangle = new Rectangle((int)startButtonPos.X, (int)startButtonPos.Y, (int)textFont.MeasureString(startButtonText).X, (int)textFont.MeasureString(startButtonText).Y);
            exitButtonRectangle = new Rectangle((int)exitButtonPos.X, (int)exitButtonPos.Y, (int)textFont.MeasureString(exitButtonText).X, (int)textFont.MeasureString(exitButtonText).Y);
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

            switch (currentGameState)
            {
                case GameState.StartMenu:
                    MouseState mouseState = Mouse.GetState();
                    Point mousePos = new Point(mouseState.X, mouseState.Y);

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        if (startButtonRectangle.Contains(mousePos))
                        {
                            currentGameState = GameState.Playing;
                        }
                        else if (exitButtonRectangle.Contains(mousePos))
                        {
                            Exit();
                        }
                    }

                    break;
                case GameState.Playing:
                    player.Update(gameTime);

                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Update(gameTime);
                    }
                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (currentGameState)
            {
                case GameState.StartMenu:
                    spriteBatch.DrawString(titleFont, titleText, titlePos, Color.Black);
                    spriteBatch.DrawString(textFont, startButtonText, startButtonPos, Color.Black);
                    spriteBatch.DrawString(textFont, exitButtonText, exitButtonPos, Color.Black);
                    break;
                case GameState.Playing:
                    string healthText = "Health: " + health;
                    player.Draw(this.spriteBatch);

                    foreach (Enemy enemy in enemies)
                    {
                        enemy.Draw(this.spriteBatch);
                    }

                    spriteBatch.DrawString(textFont, healthText, new Vector2(0, graphics.GraphicsDevice.Viewport.Height - textFont.MeasureString(healthText).Y), Color.Black);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
