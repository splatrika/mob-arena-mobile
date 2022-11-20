namespace Splatrika.MobArenaMobile.Model
{
    public interface ITimeScaleService
    {
        public float TimeScale { get; }

        void Up(float value);
    }
}
