using Unity.Entities;

namespace TicTacToeEcs {
    public struct Sign : IComponentData {
        public SignType value;
    }
}