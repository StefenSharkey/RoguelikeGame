﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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

            base.Update(gameTime);
        }

        private void UpdateMovement(KeyboardState currentKeyboardState)
        {
            if (currentEntityState == State.Walking)
            {
                speed = Vector2.Zero;
                direction = Vector2.Zero;

                if (!isKnockback && knockbackDistanceElapsed >= knockbackDistanceTotal)
                {
                    //if (!(isKnockback && (knockbackDirection == Direction.Top || knockbackDirection == Direction.Bottom)))
                    //{
                    if (currentKeyboardState.IsKeyDown(Keys.W) && !boundsCollisionDirection.HasFlag(Direction.Top) && !entityCollisionDirection.HasFlag(Direction.Top))
                    {
                        speed.Y = entitySpeed;
                        direction.Y = MOVE_UP;
                    }
                    else if (currentKeyboardState.IsKeyDown(Keys.S) && !boundsCollisionDirection.HasFlag(Direction.Bottom) && !entityCollisionDirection.HasFlag(Direction.Bottom))
                    {
                        speed.Y = entitySpeed;
                        direction.Y = MOVE_DOWN;
                    }
                    //}

                    //if (!(isKnockback && (knockbackDirection == Direction.Left || knockbackDirection == Direction.Right)))
                    //{
                    if (currentKeyboardState.IsKeyDown(Keys.A) && !boundsCollisionDirection.HasFlag(Direction.Left) && !entityCollisionDirection.HasFlag(Direction.Left))
                    {
                        speed.X = entitySpeed;
                        direction.X = MOVE_LEFT;
                    }
                    else if (currentKeyboardState.IsKeyDown(Keys.D) && !boundsCollisionDirection.HasFlag(Direction.Right) && !entityCollisionDirection.HasFlag(Direction.Right))
                    {
                        speed.X = entitySpeed;
                        direction.X = MOVE_RIGHT;
                    }
                    //}
                }
            }
        }
    }
}
