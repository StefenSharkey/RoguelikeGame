using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoguelikeGameNamespace {
    class Label {
        public Vector2 position;
        public SpriteFont spriteFont;
        public string text;

        public Label() {
        }

        public Label(Vector2 position, SpriteFont spriteFont, string text) {
            this.position = position;
            this.spriteFont = spriteFont;
            this.text = text;
        }

        public float GetWidth() {
            return spriteFont.MeasureString(text).X;
        }

        public float GetHeight() {
            return spriteFont.MeasureString(text).Y;
        }
    }
}
