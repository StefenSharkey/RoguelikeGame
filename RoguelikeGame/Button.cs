using Microsoft.Xna.Framework;

namespace RoguelikeGameNamespace {
    class Button {
        public Vector2 position;
        public Rectangle rectangle;
        public string text;

        public Button prevButton = null;
        public Button nextButton = null;

        public Button() : this(new Vector2(), new Rectangle(), "") {

        }

        public Button(Vector2 position, Rectangle rectangle, string text) {
            this.position = position;
            this.rectangle = rectangle;
            this.text = text;
        }

        public Button(Vector2 position, Rectangle rectangle, string text, Button prevButton, Button nextButton) : this(position, rectangle, text) {
            this.prevButton = prevButton;
            this.nextButton = nextButton;
        }
    }
}
