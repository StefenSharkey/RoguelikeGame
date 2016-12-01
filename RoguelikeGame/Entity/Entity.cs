using RoguelikeGameNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Threading;

namespace EntityNamespace
{
    public class Entity : Sprite
    {
        public double health
        {
            get;
            protected set;
        } = 100.0;

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

        public Vector2 speed = Vector2.Zero;
        public Vector2 direction = Vector2.Zero;

        public bool collidable
        {
            get;
            protected set;
        } = true;

        [Flags]
        public enum Direction
        {
            None = 0,
            Top = 1 << 0,
            Left = 1 << 1,
            Bottom = 1 << 2,
            Right = 1 << 3
        }
        protected Direction boundsCollisionDirection = Direction.None;
        protected Direction entityCollisionDirection = Direction.None;

        public bool isKnockback = false;
        public Direction knockbackDirection
        {
            get;
            protected set;
        } = Direction.None;
        protected int knockbackSpeed;
        protected double knockbackDistanceElapsed;
        protected double knockbackDistanceTotal;
        public bool isKnockbackImmune = false;
        public double knockbackImmunityStart
        {
            get;
            protected set;
        }
        protected double knockbackImmunityEnd = 2.0;


        protected double collisionKnockbackDistance;
        protected int collisionKnockbackSpeed;

        public Entity(string assetName, int startPosX, int startPosY)
        {
            this.assetName = assetName;
            this.startPosX = startPosX;
            this.startPosY = startPosY;
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            position = new Vector2(startPosX, startPosY);
            base.LoadContent(contentManager, assetName);
        }

        public virtual void Update(GameTime gameTime)
        {
            // Collision
            CheckBoundsCollisions();

            Direction collisionDirection = Direction.None;

            foreach (Entity entity in RoguelikeGame.enemies)
            {
                collisionDirection |= CheckCollisions(entity);
            }

            entityCollisionDirection = collisionDirection;

            // Knockback
            if (isKnockback)
            {
                if (knockbackDistanceElapsed <= knockbackDistanceTotal)
                {

                    knockbackDistanceElapsed += Math.Sqrt(Math.Pow(position.X - prevPosition.X, 2) + Math.Pow(position.Y - prevPosition.Y, 2));

                    switch (knockbackDirection)
                    {
                        case Direction.Left:
                            direction.X += MOVE_LEFT;
                            speed.X += knockbackSpeed;

                            if (boundsCollisionDirection == Direction.Left || entityCollisionDirection == Direction.Left)
                            {
                                knockbackDistanceElapsed = knockbackDistanceTotal;
                            }

                            break;
                        case Direction.Right:
                            direction.X += MOVE_RIGHT;
                            speed.X += knockbackSpeed;

                            if (boundsCollisionDirection == Direction.Right || entityCollisionDirection == Direction.Right)
                            {
                                knockbackDistanceElapsed = knockbackDistanceTotal;
                            }

                            break;
                        case Direction.Top:
                            direction.Y += MOVE_UP;
                            speed.Y += knockbackSpeed;

                            if (boundsCollisionDirection == Direction.Top || entityCollisionDirection == Direction.Top)
                            {
                                knockbackDistanceElapsed = knockbackDistanceTotal;
                            }

                            break;
                        case Direction.Bottom:
                            direction.Y += MOVE_DOWN;
                            speed.Y += knockbackSpeed;

                            if (boundsCollisionDirection == Direction.Bottom || entityCollisionDirection == Direction.Bottom)
                            {
                                knockbackDistanceElapsed = knockbackDistanceTotal;
                            }

                            break;
                    }
                }
                else
                {
                    isKnockback = false;
                }
            }

            if (isKnockbackImmune && ((DateTime.Now - DateTime.MinValue).TotalSeconds - knockbackImmunityStart) >= knockbackImmunityEnd)
            {
                isKnockbackImmune = false;
            }

            base.Update(gameTime, speed, direction);
        }

        private new void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            this.speed = speed;
            this.direction = direction;
            Update(gameTime);
        }

        protected Direction CheckCollisions(Entity entity)
        {
            Direction collisionDirection = Direction.None;

            if (this != entity && entity.collidable && Intersects(entity))
            {
                double angle = GetAngleRelativeTo(entity);
                Vector2 angleVector = new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle));

                if (angle <= 45.0 || angle > 315.0)
                {
                    // Left edge is colliding.
                    collisionDirection = Direction.Left;
                }
                else if (angle <= 135.0)
                {
                    // Bottom edge is colliding.
                    collisionDirection = Direction.Bottom;
                }
                else if (angle <= 225.0)
                {
                    // Right edge is colliding.
                    collisionDirection = Direction.Right;
                }
                else
                {
                    // Top edge is colliding.
                    collisionDirection = Direction.Top;
                }

                entity.OnCollide(this, collisionDirection, angle);
            }

            return collisionDirection;
        }

        protected void CheckBoundsCollisions()
        {
            Viewport maxBounds = RoguelikeGame.graphics.GraphicsDevice.Viewport;

            // Left edge is colliding.
            if (position.X <= 0)
            {
                position.X = 0;
                boundsCollisionDirection |= Direction.Left;
            }
            else
            {
                boundsCollisionDirection &= ~Direction.Left;
            }

            // Top edge is colliding.
            if (position.Y <= 0)
            {
                position.Y = 0;
                boundsCollisionDirection |= Direction.Top;
            }
            else
            {
                boundsCollisionDirection &= ~Direction.Top;
            }

            // Right edge is colliding.
            if ((position.X + size.Width) >= maxBounds.Width)
            {
                position.X = maxBounds.Width - size.Width;
                boundsCollisionDirection |= Direction.Right;
            }
            else
            {
                boundsCollisionDirection &= ~Direction.Right;
            }

            // Bottom edge is colliding.
            if ((position.Y + size.Height) >= maxBounds.Height)
            {
                position.Y = maxBounds.Height - size.Height;
                boundsCollisionDirection |= Direction.Bottom;
            }
            else
            {
                boundsCollisionDirection &= ~Direction.Bottom;
            }
        }

        public bool Intersects(Entity entity)
        {
            Rectangle rectangle1 = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);
            Rectangle rectangle2 = new Rectangle((int)entity.position.X, (int)entity.position.Y, entity.size.Width, entity.size.Height);

            return rectangle1.Intersects(rectangle2);
        }

        public bool Intersects(Vector2 position, Rectangle size)
        {
            Rectangle rectangle1 = new Rectangle((int)this.position.X, (int)this.position.Y, this.size.Width, this.size.Height);
            Rectangle rectangle2 = new Rectangle((int)position.X, (int)position.Y, size.Width, size.Height);

            return rectangle1.Intersects(rectangle2);
        }

        public double GetAngleRelativeTo(Entity entity)
        {
            double centerX1 = position.X + (size.Width / 2.0);
            double centerX2 = entity.position.X + (entity.size.Width / 2.0);
            double centerY1 = position.Y + (size.Height / 2.0);
            double centerY2 = entity.position.Y + (entity.size.Height / 2.0);
            double angle = -180.0 * Math.Atan2(centerY1 - centerY2, centerX1 - centerX2) / Math.PI;
            angle = angle > 0 ? angle : 360 + angle;

            return angle;
        }

        protected virtual void OnCollide(Entity entity, Direction collisionDirection, double angle)
        {

        }

        public void Knockback(Direction knockbackDirection, double distance, int speed)
        {
            if (!isKnockbackImmune)
            {
                isKnockback = true;
                this.knockbackDirection = knockbackDirection;
                knockbackDistanceElapsed = 0.0;
                knockbackDistanceTotal = distance;
                knockbackSpeed = speed;
                isKnockbackImmune = true;
                knockbackImmunityStart = (DateTime.Now - DateTime.MinValue).TotalSeconds;
            }
        }

        public void CancelKnockback()
        {
            Console.WriteLine(this + ".CancelKnockback()");
            isKnockback = false;
            knockbackDistanceTotal = 0.0;
        }
    }
}
