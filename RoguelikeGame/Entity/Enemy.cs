using Microsoft.Xna.Framework;

using System;

namespace EntityNamespace
{
    public class Enemy : Entity
    {
        public Enemy(string assetName, int startPosX, int startPosY) : base(assetName, startPosX, startPosY)
        {
            collisionKnockbackDistance = 200.0;
            collisionKnockbackSpeed = 500;
        }

        protected override void OnCollide(Entity entity, Direction collisionDirection, double angle)
        {
            base.OnCollide(entity, collisionDirection, angle);

            Vector2 angleVector = new Vector2((float)Math.Cos(angle), (float)-Math.Sin(angle));

            if (collisionDirection == Direction.Left)
            {
                // Left edge is colliding.
                if (entity is Player && !entity.isKnockbackImmune)
                {
                    entity.Knockback(Direction.Right, collisionKnockbackDistance, collisionKnockbackSpeed);
                }
                else
                {
                    entity.position.X = position.X + size.Width;
                }
            }
            else if (collisionDirection == Direction.Right)
            {
                // Right edge is colliding.
                if (entity is Player && !entity.isKnockbackImmune)
                {
                    entity.Knockback(Direction.Left, collisionKnockbackDistance, collisionKnockbackSpeed);
                }
                else
                {
                    entity.position.X = position.X - entity.size.Width;
                }
            }

            if (collisionDirection == Direction.Top)
            {
                // Top edge is colliding.
                if (entity is Player && !entity.isKnockbackImmune)
                {
                    entity.Knockback(Direction.Bottom, collisionKnockbackDistance, collisionKnockbackSpeed);
                }
                else
                {
                    entity.position.Y = position.Y + size.Height;
                }
            }
            else if (collisionDirection == Direction.Bottom)
            {
                // Bottom edge is colliding.
                if (entity is Player && !entity.isKnockbackImmune)
                {
                    entity.Knockback(Direction.Top, collisionKnockbackDistance, collisionKnockbackSpeed);
                }
                else
                {
                    entity.position.Y = position.Y - entity.size.Height;
                }
            }
        }
    }
}
