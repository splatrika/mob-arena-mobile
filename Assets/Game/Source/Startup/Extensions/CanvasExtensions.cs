using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Splatrika.MobArenaMobile.Startup
{
    public static class CanvasExtensions
    {
        public static Canvas CreateDefaultCanvas(
            this DiContainer container)
        {
            var logger = container.Resolve<ILogger>();

            var prefab = Resources.Load<Canvas>("DefaultCanvas");
            if (!prefab)
            {
                logger.LogError(nameof(CanvasExtensions),
                    "There is no valid DefaultCanvas in the resources folder");
                return null;
            }
            
            if (!GameObject.FindObjectOfType<EventSystem>())
            {
                var eventSystem = new GameObject("EventSystem")
                    .AddComponent<EventSystem>();
                eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            }

            return GameObject.Instantiate(prefab);
        }
    }
}
