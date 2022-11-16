using System;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Scene
{
    public static class SettingsExtensions
    {
        public static void BindSettingsFromResource<T>
            (this DiContainer container, string resourceName)
            where T : ScriptableObject
        {
            var settings = Resources.Load<T>(resourceName);
            if (!settings)
            {
                throw new NullReferenceException(
                    $"{resourceName} wasn't found in Resoureces folder");
            }
            container.Bind<T>()
                .FromInstance(settings);
        }
    }
}
