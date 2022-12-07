using UnityEngine;

namespace Splatrika.MobArenaMobile.Settings
{
    [CreateAssetMenu(menuName = "MobArenaMobile/Settings/Navigation")]
    public class NavigationSettings : ScriptableObject
    {
        public int PathBufferSize => _pathBufferSize;

        [SerializeField]
        private int _pathBufferSize;
    }
}
