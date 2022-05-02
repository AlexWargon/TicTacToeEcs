using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace TicTacToeEcs {
    public partial class AiSystem : SystemBase {
        private EntityQuery _freeCells;
        private EntityQuery _gameOverFlag;
        protected override void OnCreate() {
            _freeCells = GetEntityQuery(typeof(Cell), ComponentType.Exclude<Sign>());
            _gameOverFlag = GetEntityQuery(typeof(GameOverFlag));
        }

        protected override void OnUpdate() {
            
            Entities.WithAny<AiTurnFlag>().ForEach((in Entity turnEntity) => {
                if(!_gameOverFlag.IsEmptyIgnoreFilter) return;
                var freeCellEntities = _freeCells.ToEntityArray(Allocator.Temp);

                var target = Random.Range(0, freeCellEntities.Length);

                EntityManager.AddComponent<ClickTakenEvent>(freeCellEntities[target]);
                freeCellEntities.Dispose();
                EntityManager.DestroyEntity(turnEntity);
            }).WithStructuralChanges().Run();
        }
    }
}