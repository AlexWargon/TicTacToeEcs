using Unity.Entities;
using UnityEngine;

namespace TicTacToeEcs {
    [UpdateAfter(typeof(InitGameSystem))]
    public partial class SetupCameraSystem : SystemBase {
        private GameConfigs _configs;

        protected override void OnCreate() { }

        protected override void OnStartRunning() {
            _configs = Service<GameConfigs>.Get();
        }

        protected override void OnUpdate() {
            Entities.ForEach((ref Entity eventEntity, in InitGameEvent initEvent) => {
                var camera = Camera.main;
                camera.orthographic = true;
                camera.orthographicSize = _configs.Size * _configs.OffsetSize / 2;
                camera.transform.position = new Vector3(_configs.Size * _configs.OffsetSize / 2,
                    _configs.Size * _configs.OffsetSize / 2, -10);
                EntityManager.DestroyEntity(eventEntity);
            }).WithStructuralChanges().Run();
        }
    }
}