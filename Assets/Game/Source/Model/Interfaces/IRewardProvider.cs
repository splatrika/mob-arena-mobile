namespace Splatrika.MobArenaMobile.Model
{
    public interface IRewardProvider
    {
        delegate void RewardedAction(int points);

        event RewardedAction Rewarded;
    }
}
