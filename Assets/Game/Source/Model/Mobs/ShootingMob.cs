using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class ShootingMob : Mob<ShootingMobConfiguration>, IShooting
    {
        public bool IsGunTaken
            => ((ShootingStrategy)AttackingStrategy).IsGunTaken;

        private readonly IPlayerCharacter _playerCharacter;

        public event Action GunTaken;
        public event IShooting.ShotAction Shot;
        public event Action GunHidden;


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

            ((ShootingStrategy)AttackingStrategy).GunTaken += OnGunTaken;
            ((ShootingStrategy)AttackingStrategy).Shot += OnShot;
            ((ShootingStrategy)AttackingStrategy).GunHidden += OnGunHidden;
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


        protected override void OnDispose()
        {
            ((ShootingStrategy)AttackingStrategy).GunTaken -= OnGunTaken;
            ((ShootingStrategy)AttackingStrategy).Shot -= OnShot;
            ((ShootingStrategy)AttackingStrategy).GunHidden -= OnGunHidden;
        }


        private void OnGunTaken()
        {
            GunTaken?.Invoke();
        }


        private void OnShot(Vector3 direction)
        {
            Shot?.Invoke(direction);
        }


        private void OnGunHidden()
        {
            GunHidden?.Invoke();
        }
    }
}
