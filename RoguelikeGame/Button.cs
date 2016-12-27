using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoguelikeGameNamespace {
    class Button {
        public Vector2 position;
        public Rectangle rectangle;
        public SpriteFont spriteFont;
        public SpriteFont spriteFontFocused;
        public string text;

        public Button prevButton = null;
        public Button nextButton = null;

        public Button() {

        }

        public Button(Vector2 position, Rectangle rectangle, SpriteFont spriteFont, SpriteFont spriteFontFocused, string text) {
            this.position = position;
            this.rectangle = rectangle;
            this.spriteFont = spriteFont;
            this.spriteFontFocused = spriteFontFocused;
            this.text = text;
        }

        public Button(Vector2 position, Rectangle rectangle, SpriteFont spriteFont, SpriteFont spriteFontFocused, string text, Button prevButton, Button nextButton) : this(position, rectangle, spriteFont, spriteFontFocused, text) {
            this.prevButton = prevButton;
            this.nextButton = nextButton;
        }

        public Vector2 GetFocusedPosition() {
            Vector2 oldSize = spriteFont.MeasureString(text);
            Vector2 newSize = spriteFontFocused.MeasureString(text);
            Vector2 center = new Vector2(position.X + (oldSize.X / 2.0F), position.Y + (oldSize.Y / 2.0F));

            return new Vector2(center.X - newSize.X / 2.0F, center.Y - newSize.Y / 2.0F);
        }
    }
}
