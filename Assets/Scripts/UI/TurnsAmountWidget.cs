using TMPro;
using UnityEngine;

namespace TicTacToeEcs {
    public class TurnsAmountWidget : UIElement {
        [SerializeField] private TextMeshProUGUI Text;

        public void SetValue(int value) {
            Text.text = value.ToString();
        }
    }
}