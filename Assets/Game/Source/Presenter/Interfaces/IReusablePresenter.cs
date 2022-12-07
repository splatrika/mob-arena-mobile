namespace Splatrika.MobArenaMobile.Presenter
{
    public interface IReusablePresenter<TModel>
    {
        bool Active { get; }

        void Init(TModel model);
    }
}
