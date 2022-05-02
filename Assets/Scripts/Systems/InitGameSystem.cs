using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

namespace TicTacToeEcs {
    public partial class InitGameSystem : SystemBase {
        private Entity _cellViewPrefab;
        private GameConfigs _configs;
        private GameRunTimeData _runTimeData;

        protected override void OnStartRunning() {
            _runTimeData = Service<GameRunTimeData>.Get();
            _configs = Service<GameConfigs>.Get();
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            _cellViewPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(_configs.CellPrefab, settings);
        }

        protected override void OnUpdate() {
            Entities.ForEach((ref Entity eventEntity, in InitGameEvent initEvent) => {
                for (var i = 0; i < _configs.Size; i++)
                for (var j = 0; j < _configs.Size; j++) {
                    var entity = EntityManager.Instantiate(_cellViewPrefab);
                    EntityManager.AddComponentData(entity, new Cell());
                    EntityManager.AddComponentData(entity, new Translation {
                        Value = new float3(i, j, 0)
                    });
                    EntityManager.AddComponentData(entity, new Position {
                        value = new int2(i, j)
                    });
                    // EntityManager.AddComponentData(entity, new CellView {
                    //     value = entity
                    // });
                    EntityManager.AddComponentData(entity, new CellCollider {
                        scaleX = _configs.OffsetSize,
                        scaleY = _configs.OffsetSize
                    });
                }

                _runTimeData.CurrentSign = Random.value > 0.5f ? SignType.Circle : SignType.Cross;
                if (Random.value > 0.5f)
                    _runTimeData.Player = GetSign(ref _runTimeData.AI, _runTimeData.Player);
                else
                    _runTimeData.AI = GetSign(ref _runTimeData.Player, _runTimeData.AI);

                SignType GetSign(ref SignType opponent1, SignType opponent2) {
                    opponent1 = _runTimeData.CurrentSign;
                    opponent2 = opponent1 switch {
                        SignType.Cross => SignType.Circle,
                        SignType.Circle => SignType.Cross,
                        _ => opponent2
                    };
                    return opponent2;
                }

                if (_runTimeData.CurrentSign == _runTimeData.AI) {
                    var e = EntityManager.CreateEntity();
                    EntityManager.AddComponent<AiTurnFlag>(e);
                }
            }).WithStructuralChanges().Run();
        }
    }
}