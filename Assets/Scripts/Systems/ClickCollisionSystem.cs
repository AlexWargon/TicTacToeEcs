using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace TicTacToeEcs {
    [UpdateAfter(typeof(MouseClickSystem))]
    public partial class ClickCollisionSystem : SystemBase {
        private EntityQuery _mouseClicksQuery;
        private GameRunTimeData _runTimeData;

        protected override void OnStartRunning() {
            _mouseClicksQuery = GetEntityQuery(typeof(MouseClickEvent));
            _runTimeData = Service<GameRunTimeData>.Get();
        }

        protected override void OnUpdate() {
            if (_mouseClicksQuery.IsEmpty) return;

            var clicks = _mouseClicksQuery.ToComponentDataArray<MouseClickEvent>(Allocator.Temp);

            for (var i = 0; i < clicks.Length; i++) {
                var clickEvent = clicks[i];
                Entities.WithAll<Cell>().WithNone<Sign, ClickTakenEvent>().ForEach((in Entity cell, in Translation translation, in CellCollider collider) => {
                    if (translation.Value.x + collider.scaleX >= clickEvent.position.x &&
                        translation.Value.x <= clickEvent.position.x &&
                        translation.Value.y + collider.scaleY >= clickEvent.position.y &&
                        translation.Value.y <= clickEvent.position.y)
                        EntityManager.AddComponent<ClickTakenEvent>(cell);
                }).WithStructuralChanges().Run();
            }

            clicks.Dispose();
            Entities.ForEach((in Entity entity, in MouseClickEvent clickEvent) => {
                    EntityManager.DestroyEntity(entity);
                })
                .WithStructuralChanges().Run();
        }
    }
}