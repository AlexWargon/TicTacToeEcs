using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;

namespace TicTacToeEcs {
    [UpdateAfter(typeof(ClickCollisionSystem))]
    public partial class DrawSignSystem : SystemBase {
        private Dictionary<SignType, Entity> _prefabs;
        private GameRunTimeData _runTimeData;
        private ScreenService _screenService;
        protected override void OnStartRunning() {
            _runTimeData = Service<GameRunTimeData>.Get();
            var configs = Service<GameConfigs>.Get();
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            _prefabs = new Dictionary<SignType, Entity>();
            _prefabs.Add(SignType.Circle,
                GameObjectConversionUtility.ConvertGameObjectHierarchy(configs.CirclePrefab, settings));
            _prefabs.Add(SignType.Cross,
                GameObjectConversionUtility.ConvertGameObjectHierarchy(configs.CrossPrefab, settings));
            _screenService = Service<ScreenService>.Get();
        }

        protected override void OnUpdate() {
            Entities.WithAll<ClickTakenEvent>().ForEach(
                (in Entity cell, in Translation translation, in Position position) => {
                    var sing = new Sign {
                        value = _runTimeData.CurrentSign
                    };
                    EntityManager.AddComponentData(cell, sing);
                    var signView = EntityManager.Instantiate(_prefabs[sing.value]);
                    EntityManager.AddComponentData(signView, translation);

                    EntityManager.RemoveComponent<ClickTakenEvent>(cell);
                    EntityManager.AddComponent<CheckWinEvent>(cell);
                    
                    _runTimeData.Cells.Add(position.value, cell);
                    
                    _screenService.Get<TurnsAmountWidget>().SetValue(_runTimeData.TurnsAmount++);
                    
                }).WithStructuralChanges().Run();
        }
    }
}