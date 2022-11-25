using System;
using UnityEngine;

namespace Splatrika.MobArenaMobile.Model
{
    public class FakeScenesService : IScenesService
    {
        private readonly ILogger _logger;

        public event Action LoadingStarted;
        public event Action LoadingFinished;


        public FakeScenesService(ILogger logger)
        {
            _logger = logger;
        }


        public void Load(string scene)
        {
            _logger.Log($"Load scene: {scene}");
        }
    }
}
