using Unity.Entities;

namespace TicTacToeEcs {
    [GenerateAuthoringComponent]
    public struct CellView : IComponentData {
        public Entity value;
    }
}