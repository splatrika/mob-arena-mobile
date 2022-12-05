using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class WalkingMob : Mob<WalkingMobConfiguration>
    {
        private readonly IPlayerCharacter _playerCharacter;


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
        }


        protected override void OnStart(
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
    }
}
