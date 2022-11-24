using UnityEngine;

namespace Splatrika.MobArenaMobile.UI
{
    public abstract class NotificationView : MonoBehaviour
    {
        public abstract void Show(string message);
    }
}
