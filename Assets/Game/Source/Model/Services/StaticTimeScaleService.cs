namespace Splatrika.MobArenaMobile.Model
{
    public class StaticTimeScaleService : ITimeScaleService
    {
        public float TimeScale => 1;

        public void Up(float value)
        {
        }
    }
}
