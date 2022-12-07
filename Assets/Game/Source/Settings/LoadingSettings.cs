using Splatrika.MobArenaMobile.UI;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Settings
{
    [CreateAssetMenu(menuName = "MobArenaMobile/Settings/Loading")]
    public class LoadingSettings : ScriptableObject
    {
        public string LoadingUIPrefab => _loadingUIPrefab;

        [SerializeField]
        private string _loadingUIPrefab;


        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_loadingUIPrefab))
            {
                return;
            }
            var prefab = Resources.Load<LoadingUI>(_loadingUIPrefab);
            if (!prefab)
            {
                _loadingUIPrefab = "";
            }
        }
    }
}
