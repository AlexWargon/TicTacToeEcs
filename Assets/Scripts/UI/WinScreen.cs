using System;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TicTacToeEcs {
    public class WinScreen : UIElement {
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private Image _signImage;
        [SerializeField] private Sprite _cross;
        [SerializeField] private Sprite _circle;
        [SerializeField] private Button _restart;

        private void Start() {
            _restart.onClick.AddListener(() => {
                Service<GameBoot>.Get().DestroyEcs();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        public void SetWinner(SignType sign) {
            _signImage.sprite = sign switch {
                SignType.Cross => _cross,
                SignType.Circle => _circle,
                _ => _signImage.sprite
            };
            _signImage.gameObject.SetActive(true);
            _resultText.text = "WIN";
        }
        public void SetGameOver() {
            _resultText.text = "GAME OVER";
            _signImage.gameObject.SetActive(false);
        }
    }
}