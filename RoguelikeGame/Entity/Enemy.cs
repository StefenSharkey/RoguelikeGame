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

            if (entity is Player)
            {
                entity.OnDamage(10.0);
            }
        }
    }
}
