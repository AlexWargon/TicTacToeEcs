using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TicTacToeEcs {
    public partial class MouseClickSystem : SystemBase {
        private EntityQuery _gameOverFlag;
        protected override void OnCreate() {
            _gameOverFlag = EntityManager.CreateEntityQuery(typeof(GameOverFlag));

        }

        protected override void OnUpdate() {
            if(!_gameOverFlag.IsEmpty) return;
            if (Input.GetMouseButtonDown(0)) {
                var e = EntityManager.CreateEntity();
                var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                EntityManager.AddComponentData(e, new MouseClickEvent {
                    position = new float2(pos.x, pos.y)
                });
            }
        }
    }
    public struct GameOverFlag : IComponentData{}
}