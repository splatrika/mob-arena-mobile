using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class DamagablePartial : IDamagablePartial
    {
        public bool IsDied => Health == 0;
        public int Health { get; private set; }

        private Predicate<IDamager> _allowedDamagers;

        public event Action Died;
        public event Action<int> HealthUpdated;


        public void Damage(IDamager damager)
        {
            if (!_allowedDamagers.Invoke(damager))
            {
                return;
            }
            Health -= damager.DamageAmount;
            Health = Mathf.Max(0, Health);
            HealthUpdated?.Invoke(Health);
            if (Health == 0)
            {
                Died?.Invoke();
            }
        }


        public void Setup(DamagableConfiguration configuration)
        {
            Health = configuration.Health;
            _allowedDamagers = configuration.AllowedDamagers;
        }
    }
}
