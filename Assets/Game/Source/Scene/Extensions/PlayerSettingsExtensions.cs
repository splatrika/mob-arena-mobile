using System;
using Splatrika.MobArenaMobile.Presenter;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public static class PlayerSettingsExtensions
    {
        public static void BindPlayerSettings(this DiContainer container)
        {
            var settings = Resources.Load<PlayerSettings>("PlayerSettings");
            if (!settings)
            {
                throw new NullReferenceException(
                    "Player settings wasn't found in Resoureces folder");
            }
            container.Bind<PlayerSettings>()
                .FromInstance(settings);
        }
    }
}
