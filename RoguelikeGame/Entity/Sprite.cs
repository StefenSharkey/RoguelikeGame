using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Entity
{
    class Sprite
    {
        //The current position of the Sprite
        public Vector2 position = new Vector2(0, 0);

        //The texture object used when drawing the sprite
        private Texture2D spriteTexture;

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
            size = new Rectangle(0, 0, (int) (spriteTexture.Width * scale), (int) (spriteTexture.Height * scale));
        }

        //Update the Sprite and change it's position based on the passed in speed, direction and elapsed time.
        public void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            position += direction * speed * (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        //Draw the sprite to the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteTexture, position,
                new Rectangle(0, 0, spriteTexture.Width, spriteTexture.Height),
                Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
