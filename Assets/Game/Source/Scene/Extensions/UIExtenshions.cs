using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public static class UIExtenshions
    {
        public static void InstantiateUI<T>(this DiContainer container,
            string resourceName, Canvas canvas) where T : MonoBehaviour
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

            if (!canvas)
            {
                logger.LogError(nameof(UIExtenshions), "Canvas is null");
                return;
            }

            var ui = container.InstantiatePrefab(prefab);

            ui.transform.SetParent(canvas.transform, false);
        }
    }
}
