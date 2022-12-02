namespace Splatrika.MobArenaMobile.Presenter
{
    public interface IPresenterPool<TModel, TConfiguration>
    {
        PresenterInfo<TModel, TConfiguration> Take(int kindId);
        void Return(IReusablePresenter<TModel> presenter);
    }
}
