using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class ShootingMob : Mob<ShootingMobConfiguration>
    {
        private readonly IPlayerCharacter _playerCharacter;


        public ShootingMob(
            ILogger logger,
            NavigationConfiguration navigationConfiguration,
            INavigationService navigationService,
            ITimeScaleService timeScaleService,
            IBulletService bulletService,
            IPlayerCharacter playerCharacter)
            : base(
                new GoToPointStrategy(navigationConfiguration,
                    navigationService, logger, timeScaleService),
                new ShootingStrategy(bulletService, timeScaleService),
                logger)
        {
            _playerCharacter = playerCharacter;
        }


        protected override void OnStart(
            ShootingMobConfiguration configuration,
            IFollowingStrategy following,
            IAttackingStrategy attacking)
        {
            var gunTarget = new PositionAdapter(() => _playerCharacter.Center);

            ((GoToPointStrategy)following)
                .Setup(configuration.ShootingPosition, configuration.Speed);
            ((ShootingStrategy)attacking)
                .Setup(configuration.BulletsDamage, configuration.BulletsSpeed,
                configuration.ShotPoint, gunTarget,
                configuration.GunRegenerationTime);
        }
    }
}
