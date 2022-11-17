using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    [CreateAssetMenu(menuName = "MobArenaMobile/Settings/Player")]
    public class PlayerSettings : ScriptableObject
    {
        public PlayerCharacterMonoSettings Prefab => _prefab;
        public int DefaultHealth => _defaultHealth;
        public float ShootRegenerationTime => _shootRegenerationTime;
        public int BulletDamage => _bulletDamage;

        [SerializeField]
        private PlayerCharacterMonoSettings _prefab;

        [SerializeField]
        private int _defaultHealth;

        [SerializeField]
        private float _shootRegenerationTime;

        [SerializeField]
        private int _bulletDamage;
    }
}
