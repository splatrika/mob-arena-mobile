namespace Splatrika.MobArenaMobile.Factories
{
    public interface IPresenterPool<T>
    {
        T Take(int kindId);
        void Return(int kindId, T taken);
    }
}
