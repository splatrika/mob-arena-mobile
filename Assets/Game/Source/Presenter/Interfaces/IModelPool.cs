using Splatrika.MobArenaMobile.Model;

namespace Splatrika.MobArenaMobile.Presenter
{
    public interface IModelPool<TModel, TConfiguration>
        where TModel : IReusable<TConfiguration>
    {
        TModel Take();
        void Return(TModel model);
    }
}
