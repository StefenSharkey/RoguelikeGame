using RoguelikeGameNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EntityNamespace {
    public class Player : Entity {
        private KeyboardState currentKeyboardState;
        private KeyboardState prevKeyboardState;

        private GamePadState currentGamePadState;
        private  GamePadState prevGamePadState;

        private bool isUsingGamePad;

        public Player(string assetName, int startPosX, int startPosY) : base(assetName, startPosX, startPosY) {

        }

        public override void Update(GameTime gameTime) {
            UpdateMovement();
            prevKeyboardState = currentKeyboardState;
            prevGamePadState = currentGamePadState;

            base.Update(gameTime);
        }

        public void Update(GameTime gameTime, KeyboardState currentKeyboardState, GamePadState currentGamePadState, bool isUsingGamePad) {
            this.currentKeyboardState = currentKeyboardState;
            this.currentGamePadState = currentGamePadState;
            this.isUsingGamePad = isUsingGamePad;

            Update(gameTime);
        }

        private void UpdateMovement() {
            if (currentEntityState == State.Walking) {
                speed = Vector2.Zero;
                direction = Vector2.Zero;
                float leftX = currentGamePadState.ThumbSticks.Left.X;
                float leftY = currentGamePadState.ThumbSticks.Left.Y;
                
                if (currentGamePadState.ThumbSticks.Left.Y > 0 || currentGamePadState.DPad.Up == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.W) && !boundsCollisionDirection.HasFlag(Direction.Top) && !entityCollisionDirection.HasFlag(Direction.Top)) {
                    speed.Y = entitySpeed * (isUsingGamePad ? leftY != 0 ? leftY: 1 : 1);
                    direction.Y = MOVE_UP;
                } else if (currentGamePadState.ThumbSticks.Left.Y < 0 || currentGamePadState.DPad.Down == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.S) && !boundsCollisionDirection.HasFlag(Direction.Bottom) && !entityCollisionDirection.HasFlag(Direction.Bottom)) {
                    speed.Y = entitySpeed * (isUsingGamePad ? leftY != 0 ? -leftY : 1 : 1);
                    direction.Y = MOVE_DOWN;
                }

                if (currentGamePadState.ThumbSticks.Left.X < 0 || currentGamePadState.DPad.Left == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.A) && !boundsCollisionDirection.HasFlag(Direction.Left) && !entityCollisionDirection.HasFlag(Direction.Left)) {
                    speed.X = entitySpeed * (isUsingGamePad ? leftX != 0 ? -leftX : 1 : 1);
                    direction.X = MOVE_LEFT;
                } else if (currentGamePadState.ThumbSticks.Left.X > 0 || currentGamePadState.DPad.Right == ButtonState.Pressed || currentKeyboardState.IsKeyDown(Keys.D) && !boundsCollisionDirection.HasFlag(Direction.Right) && !entityCollisionDirection.HasFlag(Direction.Right)) {
                    speed.X = entitySpeed * (isUsingGamePad ? leftX != 0 ? leftX : 1 : 1);
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
