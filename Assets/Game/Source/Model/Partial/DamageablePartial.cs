using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class DamageablePartial : IDamageablePartial
    {
        public bool IsDied => Health == 0;
        public int Health { get; private set; }

        private Predicate<IDamager> _allowedDamagers;

        public event Action Died;
        public event Action<int> HealthUpdated;
        public event Action Damaged;


        public void Damage(IDamager damager)
        {
            if (!_allowedDamagers.Invoke(damager))
            {
                return;
            }
            if (IsDied)
            {
                return;
            }
            Health -= damager.DamageAmount;
            Health = Mathf.Max(0, Health);
            HealthUpdated?.Invoke(Health);
            Damaged?.Invoke();
            if (Health == 0)
            {
                Died?.Invoke();
            }
        }


        public void Setup(DamageableConfiguration configuration)
        {
            Health = configuration.Health;
            _allowedDamagers = configuration.AllowedDamagers;
        }
    }
}
