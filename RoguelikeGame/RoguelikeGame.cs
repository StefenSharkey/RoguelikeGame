using EntityNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace RoguelikeGameNamespace {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class RoguelikeGame : Game {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private SpriteFont titleFont;
        private SpriteFont textFontFocused;
        private SpriteFont textFont;

        private Player player;
        public static List<Enemy> enemies = new List<Enemy>();

        public enum GameState {
            StartMenu,
            Loading,
            Playing,
            Paused,
            Dead
        }
        public static GameState currentGameState = GameState.StartMenu;

        private KeyboardState currentKeyboardState;
        private KeyboardState prevKeyboardState;

        private GamePadState currentGamePadState;
        private GamePadState prevGamePadState;

        public bool wasUsingGamePad {
            get;
            private set;
        } = false;

        private Button focusedButton;

        private Button startButton;
        private Button exitButton;

        private Label titleLabel;
        private Label pauseLabel;
        private Label deadLabel;

        public RoguelikeGame() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
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
        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player.LoadContent(this.Content);

            foreach (Enemy enemy in enemies) {
                enemy.LoadContent(this.Content);
            }

            titleFont = Content.Load<SpriteFont>("Title");
            textFontFocused = Content.Load<SpriteFont>("FocusedText");
            textFont = Content.Load<SpriteFont>("Text");

            titleLabel = new Label();
            titleLabel.spriteFont = titleFont;
            titleLabel.text = "Roguelike Game";
            titleLabel.position = new Vector2((graphics.GraphicsDevice.Viewport.Width - titleLabel.spriteFont.MeasureString(titleLabel.text).X) / 2, 0);

            pauseLabel = new Label();
            pauseLabel.spriteFont = textFont;
            pauseLabel.text = "Paused";
            pauseLabel.position = new Vector2((graphics.GraphicsDevice.Viewport.Width - titleLabel.spriteFont.MeasureString(pauseLabel.text).X) / 2, (graphics.GraphicsDevice.Viewport.Height - titleLabel.spriteFont.MeasureString(pauseLabel.text).Y) / 2);

            deadLabel = new Label();
            deadLabel.spriteFont = textFont;
            deadLabel.text = "You died!";
            deadLabel.position = new Vector2((graphics.GraphicsDevice.Viewport.Width - titleLabel.spriteFont.MeasureString(deadLabel.text).X) / 2, 0);

            startButton = new Button();
            startButton.spriteFont = textFont;
            startButton.spriteFontFocused = textFontFocused;
            startButton.text = "Start";
            startButton.position = new Vector2((graphics.GraphicsDevice.Viewport.Width - textFont.MeasureString(startButton.text).X) / 2, titleLabel.spriteFont.MeasureString(titleLabel.text).Y + 5);
            startButton.rectangle = new Rectangle((int) startButton.position.X, (int) startButton.position.Y, (int) startButton.spriteFontFocused.MeasureString(startButton.text).X, (int) startButton.spriteFontFocused.MeasureString(startButton.text).Y);

            exitButton = new Button();
            exitButton.spriteFont = textFont;
            exitButton.spriteFontFocused = textFontFocused;
            exitButton.text = "Exit";
            exitButton.position = new Vector2((graphics.GraphicsDevice.Viewport.Width - textFont.MeasureString(exitButton.text).X) / 2, startButton.position.Y + textFont.MeasureString(startButton.text).Y + 5);
            exitButton.rectangle = new Rectangle((int) exitButton.position.X, (int) exitButton.position.Y, (int) exitButton.spriteFontFocused.MeasureString(exitButton.text).X, (int) exitButton.spriteFontFocused.MeasureString(exitButton.text).Y);

            startButton.prevButton = exitButton;
            startButton.nextButton = exitButton;
            exitButton.prevButton = startButton;
            exitButton.nextButton = startButton;
            
            focusedButton = startButton;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            IsUsingGamePad();

            switch (currentGameState) {
                case GameState.Dead:
                case GameState.StartMenu:
                    if (currentGamePadState.Buttons.Back == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Escape)) {
                        Exit();
                    }

                    MouseState mouseState = Mouse.GetState();
                    Point mousePos = new Point(mouseState.X, mouseState.Y);

                    if (startButton.rectangle.Contains(mousePos)) {
                        focusedButton = startButton;
                    } else if (exitButton.rectangle.Contains(mousePos)) {
                        focusedButton = exitButton;
                    }

                    Console.WriteLine("Focused Button: " + focusedButton.nextButton);

                    if (currentGamePadState.ThumbSticks.Left.Y <= -0.5 && prevGamePadState.ThumbSticks.Left.Y > -0.5 ||
                            currentGamePadState.DPad.Down == ButtonState.Pressed && prevGamePadState.DPad.Down != ButtonState.Pressed ||
                            currentKeyboardState.IsKeyDown(Keys.S) && !prevKeyboardState.IsKeyDown(Keys.S)) {
                        focusedButton = focusedButton.nextButton;
                    } else if (currentGamePadState.ThumbSticks.Left.Y >= 0.5 && prevGamePadState.ThumbSticks.Left.Y < 0.5 ||
                            currentGamePadState.DPad.Up == ButtonState.Pressed && prevGamePadState.DPad.Up != ButtonState.Pressed ||
                            currentKeyboardState.IsKeyDown(Keys.W) && !prevKeyboardState.IsKeyDown(Keys.W)) {
                        focusedButton = focusedButton.prevButton;
                    }

                    if (currentGamePadState.Buttons.Start == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.Enter) || (mouseState.LeftButton == ButtonState.Pressed && focusedButton.rectangle.Contains(mousePos))) {
                        if (focusedButton == startButton) {
                            currentGameState = GameState.Playing;
                        } else if (focusedButton == exitButton) {
                            Exit();
                        }
                    }

                    break;
                case GameState.Playing:
                    if ((currentGamePadState.Buttons.Start == ButtonState.Pressed && prevGamePadState.Buttons.Start != ButtonState.Pressed) || (currentKeyboardState.IsKeyDown(Keys.Escape) && !prevKeyboardState.IsKeyDown(Keys.Escape))) {
                        currentGameState = GameState.Paused;
                    }

                    player.Update(gameTime, currentKeyboardState, currentGamePadState, wasUsingGamePad);

                    foreach (Enemy enemy in enemies) {
                        enemy.Update(gameTime);
                    }

                    if (player.health <= 0.0) {
                        currentGameState = GameState.Dead;
                    }
                    break;
                case GameState.Paused:
                    if ((currentGamePadState.Buttons.Start == ButtonState.Pressed && prevGamePadState.Buttons.Start != ButtonState.Pressed) || (currentKeyboardState.IsKeyDown(Keys.Escape) && !prevKeyboardState.IsKeyDown(Keys.Escape))) {
                        currentGameState = GameState.Playing;
                    }

                    break;
            }

            prevKeyboardState = currentKeyboardState;
            prevGamePadState = currentGamePadState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (currentGameState) {
                case GameState.StartMenu:
                    spriteBatch.DrawString(titleLabel.spriteFont, titleLabel.text, titleLabel.position, Color.Black);

                    if (focusedButton == startButton) {
                        spriteBatch.DrawString(startButton.spriteFontFocused, startButton.text, startButton.GetFocusedPosition(), Color.Black);
                    } else {
                        spriteBatch.DrawString(startButton.spriteFont, startButton.text, startButton.position, Color.Black);
                    }

                    if (focusedButton == exitButton) {
                        spriteBatch.DrawString(exitButton.spriteFontFocused, exitButton.text, exitButton.GetFocusedPosition(), Color.Black);
                    } else {
                        spriteBatch.DrawString(exitButton.spriteFont, exitButton.text, exitButton.position, Color.Black);
                    }
                    break;
                case GameState.Paused:
                case GameState.Playing:
                    string healthText = "Health: " + player.health;
                    player.Draw(this.spriteBatch);

                    foreach (Enemy enemy in enemies) {
                        enemy.Draw(this.spriteBatch);
                    }

                    spriteBatch.DrawString(textFont, healthText, new Vector2(0, graphics.GraphicsDevice.Viewport.Height - textFont.MeasureString(healthText).Y), Color.Black);

                    if (currentGameState == GameState.Paused) {
                        spriteBatch.DrawString(titleLabel.spriteFont, pauseLabel.text, pauseLabel.position, Color.Black);
                    }
                    break;
                case GameState.Dead:
                    spriteBatch.DrawString(titleLabel.spriteFont, deadLabel.text, deadLabel.position, Color.Black);
                    spriteBatch.DrawString(startButton.spriteFont, startButton.text, startButton.position, Color.Black);
                    spriteBatch.DrawString(exitButton.spriteFont, exitButton.text, exitButton.position, Color.Black);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Checks if the last user input was through a controller or keyboard.
        /// </summary>
        /// <returns>True if the user is using a gamepad; otherwise, false.</returns>
        private bool IsUsingGamePad() {
            // TODO: Add analog stick and trigger tolerance.
            // If the previous and current packet numbers are the same, that means there was no controller input between ticks.
            if (prevGamePadState.PacketNumber == 0 || prevGamePadState.PacketNumber == currentGamePadState.PacketNumber) {
                // If the list of pressed keys is not empty, then the user is using a keyboard. Otherwise, return the previous status, which defaults to the keyboard.
                if (currentKeyboardState.GetPressedKeys().Length != 0) {
                    wasUsingGamePad = false;
                    return false;
                } else {
                    return wasUsingGamePad;
                }
            } else {
                wasUsingGamePad = true;
                return true;
            }
        }
    }
}
