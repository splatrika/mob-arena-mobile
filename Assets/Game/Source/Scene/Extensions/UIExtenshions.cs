using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public static class UIExtenshions
    {
        public static void InstantiateUI<T>(this DiContainer container,
            string resourceName) where T : MonoBehaviour
        {
            var logger = container.Resolve<ILogger>();

            var prefab = Resources.Load<RectTransform>(resourceName);
            if (!prefab || !prefab.TryGetComponent<T>(out _))
            {
                logger.LogError(nameof(UIExtenshions),
                    $"There is no valid UI prefab named {resourceName} " +
                    "in the resources folder");
                return;
            }

            var canvas = GameObject.FindObjectOfType<Canvas>();
            if (!canvas)
            {
                canvas = new GameObject("Canvas").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }

            if (!GameObject.FindObjectOfType<EventSystem>())
            {
                new GameObject("EventSystem").AddComponent<EventSystem>();
            }

            var ui = container.InstantiatePrefab(prefab);

            ui.transform.SetParent(canvas.transform, false);
        }
    }
}
