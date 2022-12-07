namespace Splatrika.MobArenaMobile.Factories
{
    public interface IModelPool<T>
    {
        T Take();
        void Return(T taken);
    }
}
