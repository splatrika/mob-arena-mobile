using Splatrika.MobArenaMobile.Model;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Splatrika.MobArenaMobile.UI
{
    public class CreditsUI : MonoBehaviour
    {
        [SerializeField]
        private Button _back;

        private IScenesService _scenesService;


        [Inject]
        public void Init(IScenesService scenesService)
        {
            _scenesService = scenesService;

            _back.onClick.AddListener(OnBackButtonClicked);
        }


        private void OnBackButtonClicked()
        {
            _scenesService.Load(Scenes.StartScreen);
        }
    }
}
