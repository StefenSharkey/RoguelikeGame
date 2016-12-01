using Microsoft.Xna.Framework;

using System;

namespace EntityNamespace
{
    public class Enemy : Entity
    {
        public Enemy(string assetName, int startPosX, int startPosY) : base(assetName, startPosX, startPosY)
        {
        }

        protected override void OnCollide(Entity entity, Direction collisionDirection)
        {
            base.OnCollide(entity, collisionDirection);

            if (collisionDirection == Direction.Left)
            {
                // Left edge is colliding.
                entity.position.X = position.X + size.Width;
            }
            else if (collisionDirection == Direction.Right)
            {
                // Right edge is colliding.
                entity.position.X = position.X - entity.size.Width;
            }

            if (collisionDirection == Direction.Top)
            {
                // Top edge is colliding.
                entity.position.Y = position.Y + size.Height;
            }
            else if (collisionDirection == Direction.Bottom)
            {
                // Bottom edge is colliding.
                entity.position.Y = position.Y - entity.size.Height;
            }
        }
    }
}
