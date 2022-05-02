using Unity.Entities;
using Unity.Mathematics;

namespace TicTacToeEcs {
    public struct Position : IComponentData {
        public int2 value;
    }
}