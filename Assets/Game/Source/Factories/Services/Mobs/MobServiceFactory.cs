using System;
using System.Collections.Generic;
using System.Linq;
using Splatrika.MobArenaMobile.Settings;
using UnityEngine;
using Zenject;

namespace Splatrika.MobArenaMobile.Factories
{
    public abstract class MobServiceFactory<TService, TPresenter, TSpawnPoint,
        TDatabase>
        : IFactory<TService>
        where TSpawnPoint : MobSpawnPointMonoSettings
        where TPresenter : MonoBehaviour
        where TDatabase : MobKindDatabaseSettings<TPresenter>
    {
        protected DiContainer Container { get; }

        private readonly LevelMonoSettings _levelMonoSettings;
        private readonly TDatabase _kindsDatabase;

        protected abstract TService CreateService(
            SpawnObjectServiceConfiguration<TPresenter> configuration);


        public MobServiceFactory(
            LevelMonoSettings levelMonoSettings,
            TDatabase kindsDatabase,
            DiContainer container)
        {
            _levelMonoSettings = levelMonoSettings;
            _kindsDatabase = kindsDatabase;
            Container = container;
        }


        public TService Create()
        {
            var maxModels = 0;
            var maxKindCounts = new Dictionary<int, int>(); // kindId - count
            foreach (var wave in _levelMonoSettings.Waves)
            {
                var kindCounts = new Dictionary<int, int>();
                var spawnPoints = wave.GetSpawnPoints()
                    .Where(x => x is TSpawnPoint)
                    .Select(x => x as TSpawnPoint);
                maxModels = Math.Max(maxModels, spawnPoints.Count());
                foreach (var point in spawnPoints)
                {
                    if (!kindCounts.ContainsKey(point.MobKind))
                    {
                        kindCounts.Add(point.MobKind, 0);
                    }
                    kindCounts[point.MobKind]++;
                }
                foreach (var kindId in kindCounts.Keys)
                {
                    if (!maxKindCounts.ContainsKey(kindId))
                    {
                        maxKindCounts.Add(kindId, 0);
                    }
                    maxKindCounts[kindId]
                        = Math.Max(maxKindCounts[kindId], kindCounts[kindId]);
                }
            }
            var kindConfigurations
                = new List<KindConfiguration<TPresenter>>();
            foreach (var kind in _kindsDatabase.Kinds)
            {
                var poolSize = maxKindCounts.ContainsKey(kind.KindId)
                    ? maxKindCounts[kind.KindId]
                    : 0;
                var prefab = Resources.Load<TPresenter>(kind.Prefab);
                var configuration = new KindConfiguration<TPresenter>
                    (kind.KindId, prefab, poolSize);
                kindConfigurations.Add(configuration);
            }
            var modelPoolConfiguration = new ModelPoolConfiguration(maxModels);
            var serviceConfiguration =
                new SpawnObjectServiceConfiguration<TPresenter>
                (kindConfigurations, modelPoolConfiguration);
            return CreateService(serviceConfiguration);
        }
    }
}
