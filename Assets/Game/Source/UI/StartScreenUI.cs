using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Splatrika.MobArenaMobile.UI
{
    public class StartScreenUI : MonoBehaviour
    {
        [SerializeField]
        private Button _play;

        private IScenesService _scenesService;


        [Inject]
        public void Init(IScenesService scenesService)
        {
            _scenesService = scenesService;

            _play.onClick.AddListener(OnPlayButtonClicked);
        }


        private void OnPlayButtonClicked()
        {
            _scenesService.Load(Scenes.DemoLevel);
        }
    }
}
