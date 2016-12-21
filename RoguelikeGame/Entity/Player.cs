using RoguelikeGameNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EntityNamespace {
    public class Player : Entity {
        protected KeyboardState currentKeyboardState;
        protected KeyboardState prevKeyboardState;

        protected GamePadState currentGamePadState;
        protected GamePadState prevGamePadState;

        public Player(string assetName, int startPosX, int startPosY) : base(assetName, startPosX, startPosY) {

        }

        public override void Update(GameTime gameTime) {
            UpdateMovement();
            prevKeyboardState = currentKeyboardState;
            prevGamePadState = currentGamePadState;

            base.Update(gameTime);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, GamePadState currentGamePadState) {
            this.currentKeyboardState = currentKeyboardState;
            this.currentGamePadState = currentGamePadState;

            Update(gameTime);
        }

        private void UpdateMovement() {
            if (currentEntityState == State.Walking) {
                speed = Vector2.Zero;
                direction = Vector2.Zero;

                if (currentGamePadState.ThumbSticks.Left.Y > 0 || currentGamePadState.DPad.Up == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.W) && !boundsCollisionDirection.HasFlag(Direction.Top) && !entityCollisionDirection.HasFlag(Direction.Top)) {
                    speed.Y = entitySpeed;
                    direction.Y = MOVE_UP;
                } else if (currentGamePadState.ThumbSticks.Left.Y < 0 || currentGamePadState.DPad.Down == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.S) && !boundsCollisionDirection.HasFlag(Direction.Bottom) && !entityCollisionDirection.HasFlag(Direction.Bottom)) {
                    speed.Y = entitySpeed;
                    direction.Y = MOVE_DOWN;
                }

                if (currentGamePadState.ThumbSticks.Left.X < 0 || currentGamePadState.DPad.Left == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.A) && !boundsCollisionDirection.HasFlag(Direction.Left) && !entityCollisionDirection.HasFlag(Direction.Left)) {
                    speed.X = entitySpeed;
                    direction.X = MOVE_LEFT;
                } else if (currentGamePadState.ThumbSticks.Left.X > 0 || currentGamePadState.DPad.Right == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.D) && !boundsCollisionDirection.HasFlag(Direction.Right) && !entityCollisionDirection.HasFlag(Direction.Right)) {
                    speed.X = entitySpeed;
                    direction.X = MOVE_RIGHT;
                }
            }
        }

        public override void OnDamage(double damage) {
            if (!isImmune) {
                base.OnDamage(damage);

                SetDamageImmune(2.0);
            }
        }
    }
}
