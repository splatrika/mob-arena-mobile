namespace Splatrika.MobArenaMobile.Presenter
{
    public class PresenterInfo<TModel, TConfiguration>
    {
        public TConfiguration configuration { get; private set; }
        public IReusablePresenter<TModel> Presenter { get; private set; }


        public PresenterInfo(
            TConfiguration configuration,
            IReusablePresenter<TModel> presenter)
        {
            this.configuration = configuration;
            Presenter = presenter;
        }
    }
}
