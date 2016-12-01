using RoguelikeGameNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;

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

        protected Vector2 speed = Vector2.Zero;
        protected Vector2 direction = Vector2.Zero;

        public bool collidable
        {
            get;
            protected set;
        } = true;

        [Flags]
        protected enum Direction
        {
            None = 0,
            Top = 1 << 0,
            Left = 1 << 1,
            Bottom = 1 << 2,
            Right = 1 << 3
        }
        protected Direction boundsCollisionDirection = Direction.None;
        protected Direction entityCollisionDirection = Direction.None;

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
            CheckBoundsCollisions();

            Direction collisionDirection = Direction.None;

            foreach (Entity entity in RoguelikeGame.enemies)
            {
                collisionDirection |= CheckCollisions(entity);
            }

            entityCollisionDirection = collisionDirection;

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
            if (this != entity && entity.collidable && Intersects(entity))
            {
                double angle = GetAngleRelativeTo(entity);

                if (angle <= 45.0 || angle > 315.0)
                {
                    // Left edge is colliding.
                    position.X = entity.position.X + entity.size.Width;
                    return Direction.Left;
                }
                else if (angle <= 135.0)
                {
                    // Bottom edge is colliding.
                    position.Y = entity.position.Y - size.Height;
                    return Direction.Bottom;
                }
                else if (angle <= 225.0)
                {
                    // Right edge is colliding.
                    position.X = entity.position.X - size.Width;
                    return Direction.Right;
                }
                else
                {
                    // Top edge is colliding.
                    position.Y = entity.position.Y + entity.size.Height;
                    return Direction.Top;
                }
            }
            else
            {
                return Direction.None;
            }
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
            Rectangle rectangle1 = new Rectangle((int) position.X, (int) position.Y, size.Width + 1, size.Height + 1);
            Rectangle rectangle2 = new Rectangle((int) entity.position.X, (int) entity.position.Y, entity.size.Width + 1, entity.size.Height + 1);

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
    }
}
