using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMob : Mob<WalkingMobConfiguration>, IAttacking
    {
        private readonly IPlayerCharacter _playerCharacter;

        public event Action Attacked;


        public WalkingMob(
            ILogger logger,
            NavigationConfiguration navigationConfiguration,
            INavigationService navigationService,
            ITimeScaleService timeScaleService,
            IPlayerCharacter playerCharacter)
            : base(
                new FollowTargetStrategy(navigationConfiguration,
                    navigationService, logger, timeScaleService),
                new DirectAttackStrategy(timeScaleService),
                logger)
        {
            _playerCharacter = playerCharacter;

            ((DirectAttackStrategy)AttackingStrategy).Attacked += OnAttacked;
        }


        protected override sealed void OnStart(
            WalkingMobConfiguration configuration,
            IFollowingStrategy following,
            IAttackingStrategy attacking)
        {
            ((FollowTargetStrategy)following).Setup(
                _playerCharacter, configuration.Speed,
                configuration.AttackDistance);

            ((DirectAttackStrategy)attacking).Setup(
                configuration.Damage, _playerCharacter,
                configuration.AttackRegenerationTime);
        }


        protected override sealed void OnDispose()
        {
            ((DirectAttackStrategy)AttackingStrategy).Attacked -= OnAttacked;
        }


        private void OnAttacked()
        {
            Attacked?.Invoke();
        }
    }
}
