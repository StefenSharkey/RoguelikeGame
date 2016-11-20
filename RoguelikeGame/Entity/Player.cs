using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EntityNamespace
{
    class Player : Entity
    {
        public Player(string assetName, int startPosX, int startPosY)
        {
            this.assetName = assetName;
            this.startPosX = startPosX;
            this.startPosY = startPosY;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();

            UpdateMovement(currentKeyboardState);

            previousKeyboardState = currentKeyboardState;

            base.Update(gameTime, speed, direction);
        }

        private void UpdateMovement(KeyboardState currentKeyboardState)
        {
            if (currentEntityState == State.Walking)
            {
                speed = Vector2.Zero;
                direction = Vector2.Zero;

                if (currentKeyboardState.IsKeyDown(Keys.W))
                {
                    speed.Y = entitySpeed;
                    direction.Y = MOVE_UP;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.S))
                {
                    speed.Y = entitySpeed;
                    direction.Y = MOVE_DOWN;
                }

                if (currentKeyboardState.IsKeyDown(Keys.A))
                {
                    speed.X = entitySpeed;
                    direction.X = MOVE_LEFT;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.D))
                {
                    speed.X = entitySpeed;
                    direction.X = MOVE_RIGHT;
                }
            }
        }
    }
}
