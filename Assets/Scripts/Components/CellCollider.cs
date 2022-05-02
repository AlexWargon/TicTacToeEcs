using Unity.Entities;

namespace TicTacToeEcs {
    public struct CellCollider : IComponentData {
        public float scaleX;
        public float scaleY;
    }
}