using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace EntityNamespace
{
    class Entity : Sprite
    {
        public int startPosX
        {
            get;
            protected set;
        } = 0;
        public int startPosY
        {
            get;
            protected set;
        } = 0;

        public int entitySpeed
        {
            get;
            protected set;
        } = 100;

        protected const int MOVE_UP = -1;
        protected const int MOVE_DOWN = 1;
        protected const int MOVE_LEFT = -1;
        protected const int MOVE_RIGHT = 1;
        protected enum State
        {
            Walking
        }

        protected State currentEntityState = State.Walking;

        protected KeyboardState previousKeyboardState;

        protected Vector2 speed = Vector2.Zero;
        protected Vector2 direction = Vector2.Zero;

        public virtual void LoadContent(ContentManager contentManager)
        {
            position = new Vector2(startPosX, startPosY);
            base.LoadContent(contentManager, assetName);
        }

        public virtual void Update(GameTime gameTime)
        {
            base.Update(gameTime, speed, direction);
        }

        private new void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            this.speed = speed;
            this.direction = direction;
            Update(gameTime);
        }
    }
}
