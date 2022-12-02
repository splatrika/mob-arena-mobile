using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Settings
{
    [Serializable]
    public struct MobKindSettings
    {
        public int KindId => _kindId;
        public Sprite Icon => _icon;
        public string Name => _name;
        public string Prefab => _prefab;

        [SerializeField]
        [HideInInspector]
        private int _kindId;

        [SerializeField]
        private Sprite _icon;

        [SerializeField]
        private string _name;

        [SerializeField]
        private string _prefab;


        public MobKindSettings(
            int kindId,
            Sprite icon,
            string name,
            string prefab)
        {
            _kindId = kindId;
            _icon = icon;
            _name = name;
            _prefab = prefab;
        }
    }
}
