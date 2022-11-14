using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public struct DamagableConfiguration
    {
        public int Health;
        public Predicate<IDamager> AllowedDamagers;

        public DamagableConfiguration(
            int lifes,
            Predicate<IDamager> allowedDamagers)
        {
            Health = lifes;
            AllowedDamagers = allowedDamagers;
        }
    }
}
