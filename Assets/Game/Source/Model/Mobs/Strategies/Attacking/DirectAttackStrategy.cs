using System;

namespace Splatrika.MobArenaMobile.Model
{
    public class DirectAttackStrategy : IAttackingStrategy, IDamager
    {
        public int DamageAmount { get; private set; }

        private IDamageable _target;
        private readonly RegeneratingAction _attacking;

        public event Action Attacked;


        public DirectAttackStrategy(
            ITimeScaleService timeScaleService)
        {
            _attacking = new RegeneratingAction(
                action: Attack,
                timeScaleService: timeScaleService);
        }


        public void Setup(int damageAmount, IDamageable target,
            float regenerationTime)
        {
            DamageAmount = damageAmount;
            _target = target;
            _attacking.Reset(regenerationTime);
        }


        public void Start()
        {
            _attacking.Start();
        }


        public void Stop()
        {
            _attacking.Stop();
        }


        public void Update(float deltaTime)
        {
            _attacking.Update(deltaTime);
        }


        private void Attack()
        {
            _target.Damage(this);
        }
    }
}
