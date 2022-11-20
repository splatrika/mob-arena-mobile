using System.Collections.Generic;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Presenter
{
    public abstract class MobKindDatabaseSettings<TPrefab> : ScriptableObject
        where TPrefab : MonoBehaviour
    {
        public IReadOnlyCollection<MobKindSettings> Kinds
            => _kinds.AsReadOnly();

        [SerializeField]
        [HideInInspector]
        private int _nextIndex = 1;

        [SerializeField]
        private List<MobKindSettings> _kinds
            = new List<MobKindSettings>();

        private int _lastCount = -1;


        private void OnValidate()
        {
            ValidateIds();
            ValidatePrefabs();
        }


        private void ValidateIds()
        {
            if (_lastCount == -1) // if just loaded
            {
                _lastCount = _kinds.Count;
            }
            if (_kinds.Count == _lastCount)
            {
                return;
            }
            if (_lastCount < _kinds.Count)
            {
                for (int i = _lastCount; i < _kinds.Count; i++)
                {
                    GiveId(i);
                }
            }
            _lastCount = _kinds.Count;
        }


        private void ValidatePrefabs()
        {
            for (int i = 0; i < _kinds.Count; i++)
            {
                var kind = _kinds[i];
                if (string.IsNullOrEmpty(kind.Prefab))
                {
                    continue;
                }
                var prefab = Resources.Load<TPrefab>(kind.Prefab);
                if (!prefab)
                {
                    _kinds[i] = new MobKindSettings(
                        kind.KindId, kind.Icon, kind.Name, "");
                }
            }
        }


        private void GiveId(int index)
        {
            _kinds[index] = new MobKindSettings(_nextIndex, _kinds[index].Icon,
                        _kinds[index].Name, _kinds[index].Prefab);
            _nextIndex++;
        }
    }
}
