using Splatrika.MobArenaMobile.Presenter;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Settings
{
    [CreateAssetMenu(menuName = "MobArenaMobile/Settings/Bullets")]
    public class BulletsSettings : ScriptableObject
    {
        public int PoolSize => _poolSize;
        public int LayerMask => _layerMask.value;
        public float LifeTime => _lifeTime;
        public BulletPresenter Prefab => _prefab;

        [SerializeField]
        private int _poolSize;

        [SerializeField]
        private LayerMask _layerMask;

        [SerializeField]
        private float _lifeTime;

        [SerializeField]
        private BulletPresenter _prefab;
    }
}
