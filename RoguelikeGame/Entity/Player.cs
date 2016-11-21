using RoguelikeGameNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EntityNamespace
{
    public class Player : Entity
    {
        public Player(string assetName, int startPosX, int startPosY) : base(assetName, startPosX, startPosY)
        {

        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState currentKeyboardState = Keyboard.GetState();
            UpdateMovement(currentKeyboardState);
            previousKeyboardState = currentKeyboardState;

            foreach (Entity entity in RoguelikeGame.enemies)
            {
                CheckCollisions(entity);
            }

            base.Update(gameTime, speed, direction);
        }

        private void UpdateMovement(KeyboardState currentKeyboardState)
        {
            if (currentEntityState == State.Walking)
            {
                speed = Vector2.Zero;
                direction = Vector2.Zero;

                if (currentKeyboardState.IsKeyDown(Keys.W) && collisionDirection != Direction.Top)
                {
                    speed.Y = entitySpeed;
                    direction.Y = MOVE_UP;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.S) && collisionDirection != Direction.Bottom)
                {
                    speed.Y = entitySpeed;
                    direction.Y = MOVE_DOWN;
                }

                if (currentKeyboardState.IsKeyDown(Keys.A) && collisionDirection != Direction.Left)
                {
                    speed.X = entitySpeed;
                    direction.X = MOVE_LEFT;
                }
                else if (currentKeyboardState.IsKeyDown(Keys.D) && collisionDirection != Direction.Right)
                {
                    speed.X = entitySpeed;
                    direction.X = MOVE_RIGHT;
                }
            }
        }
    }
}
