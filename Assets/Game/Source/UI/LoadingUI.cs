using System.Linq;
using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.UI
{
    public class LoadingUI : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private string _showState;

        [SerializeField]
        private string _hideState;

        private IScenesService _service;


        [Inject]
        public void Init(IScenesService service)
        {
            _service = service;

            _service.LoadingStarted += OnLoadingStarted;
            _service.LoadingFinished += OnLoadingFinished;
        }


        public float GetShowDuration()
        {
            var clip = _animator.runtimeAnimatorController.animationClips
                .First(x => x.name == _showState);

            return clip.length;
        }


        private void OnLoadingStarted()
        {
            _animator.Play(_showState);
        }


        private void OnLoadingFinished()
        {
            _animator.Play(_hideState);
        }
    }
}
