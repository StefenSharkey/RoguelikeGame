using RoguelikeGameNamespace;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;

namespace EntityNamespace
{
    public class Entity : Sprite
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

        public bool collidable
        {
            get;
            protected set;
        } = true;
        protected enum Direction
        {
            Top,
            Left,
            Bottom,
            Right,
            None
        }
        protected Direction collisionDirection = Direction.None;

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
            foreach (Entity entity in RoguelikeGame.enemies)
            {
                CheckCollisions(entity);
            }

            base.Update(gameTime, speed, direction);
        }

        private new void Update(GameTime gameTime, Vector2 speed, Vector2 direction)
        {
            this.speed = speed;
            this.direction = direction;
            Update(gameTime);
        }

        protected void CheckCollisions(Entity entity)
        {
            if (this != entity && entity.collidable && Intersects(entity))
            {
                double angle = GetAngleRelativeTo(entity);

                if (angle <= 45.0 || angle > 315.0)
                {
                    // Left edge is colliding.
                    position.X = entity.position.X + entity.size.Width;
                    collisionDirection = Direction.Left;
                }
                else if (angle <= 135.0)
                {
                    // Bottom edge is colliding.
                    position.Y = entity.position.Y - size.Height;
                    collisionDirection = Direction.Bottom;
                }
                else if (angle <= 225.0)
                {
                    // Right edge is colliding.
                    position.X = entity.position.X - size.Width;
                    collisionDirection = Direction.Right;
                }
                else
                {
                    // Top edge is colliding.
                    position.Y = entity.position.Y + entity.size.Height;
                    collisionDirection = Direction.Top;
                }
            }
            else
            {
                collisionDirection = Direction.None;
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
