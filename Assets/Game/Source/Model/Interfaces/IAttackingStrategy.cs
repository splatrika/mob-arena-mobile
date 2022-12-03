namespace Splatrika.MobArenaMobile.Model
{
    public interface IAttackingStrategy: IUpdatable
    {
        void Start();
        void Stop();
    }
}