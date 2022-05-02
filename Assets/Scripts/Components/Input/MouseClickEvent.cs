using Unity.Entities;
using Unity.Mathematics;

namespace TicTacToeEcs {
    public struct MouseClickEvent : IComponentData {
        public float2 position;
    }
}