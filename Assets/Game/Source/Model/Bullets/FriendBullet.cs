namespace Splatrika.MobArenaMobile.Model
{
    public class FriendBullet : Bullet, IFriendBullet
    {
        public FriendBullet(
            float lifeTime,
            int layerMask,
            IRaycastService raycastService,
            ITimeScaleService timeScaleService)
            : base(lifeTime, layerMask, raycastService, timeScaleService)
        {
        }
    }
}
