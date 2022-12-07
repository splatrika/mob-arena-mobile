using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public struct DamageableConfiguration
    {
        public int Health;
        public Predicate<IDamager> AllowedDamagers;

        public DamageableConfiguration(
            int lifes,
            Predicate<IDamager> allowedDamagers)
        {
            Health = lifes;
            AllowedDamagers = allowedDamagers;
        }
    }
}
