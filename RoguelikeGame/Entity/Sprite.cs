using RoguelikeGameNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EntityNamespace
{
    public class Sprite
    {
        //The current position of the Sprite
        public Vector2 position = new Vector2(0, 0);
        public Vector2 prevPosition = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        protected Texture2D spriteTexture;
        protected Color color = Color.White;

        //The asset name for the Sprite's Texture
        public string assetName
        {
            get;
            protected set;
        }

        //The Size of the Sprite (with scale applied)
        public Rectangle size;

        //The amount to increase/decrease the size of the original sprite. When
        //modified throught he property, the Size of the sprite is recalculated
        //with the new scale applied.
        private float _scale = 1.0f;

        public float scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;

                //Recalculate the Size of the Sprite with the new scale
                size = new Rectangle(0, 0, (int)(spriteTexture.Width * scale), (int)(spriteTexture.Height * scale));
            }
        }

        //Load the texture for the sprite using the Content Pipeline
        public void LoadContent(ContentManager contentManager, string assetName)
        {
            spriteTexture = contentManager.Load<Texture2D>(assetName);
            this.assetName = assetName;
            size = new Rectangle(0, 0, (int)(spriteTexture.Width * scale), (int)(spriteTexture.Height * scale));
        }

        //Update the Sprite and change its position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            prevPosition = position;
            position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this is Entity)
            {
                Entity entity = (Entity)this;

                if (entity.isKnockback && entity.knockbackImmunityStart > 0.0)
                {
                    foreach (Enemy enemy in RoguelikeGame.enemies)
                    {
                        if (enemy.Intersects(entity))
                        {
                            if (direction.X < 0)
                            {
                                // Moving left.
                                position.X = enemy.position.X + enemy.size.Width;
                            }
                            else if (direction.X > 0)
                            {
                                // Moving right.
                                position.X = enemy.position.X - size.Width;
                            }

                            if (direction.Y < 0)
                            {
                                // Moving up.
                                position.Y = enemy.position.Y + enemy.size.Height;
                            }
                            else if (direction.Y > 0)
                            {
                                // Moving down.
                                position.Y = enemy.position.Y - size.Height;
                            }

                            Console.WriteLine("enemy.Intersects()");
                            entity.CancelKnockback();
                        }
                    }

                    if (position.X < 0)
                    {
                        position.X = 0;
                        entity.CancelKnockback();
                    }
                    else if (position.X + size.Width > RoguelikeGame.graphics.GraphicsDevice.Viewport.Width)
                    {
                        position.X = RoguelikeGame.graphics.GraphicsDevice.Viewport.Width - size.Width;
                        entity.CancelKnockback();
                    }

                    if (position.Y < 0)
                    {
                        position.Y = 0;
                        entity.CancelKnockback();
                    }
                    else if (position.Y + size.Height > RoguelikeGame.graphics.GraphicsDevice.Viewport.Height)
                    {
                        position.Y = RoguelikeGame.graphics.GraphicsDevice.Viewport.Height - size.Height;
                        entity.CancelKnockback();
                    }
                }
            }
        }

        //Draw the sprite to the screen
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, position,
                new Rectangle(0, 0, spriteTexture.Width, spriteTexture.Height),
                color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
