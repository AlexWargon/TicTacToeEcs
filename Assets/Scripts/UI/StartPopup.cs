using System;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToeEcs {
    public class StartPopup : UIElement {

        [SerializeField] private Button _startButton;
        [SerializeField] private TMP_InputField _inputFieldSize;
        private int _currentSize;
        private GameConfigs _configs;
        private GameBoot _gameBoot;
        private void Start() {
            _startButton.onClick.AddListener(StartGame);
            _inputFieldSize.characterValidation = TMP_InputField.CharacterValidation.Integer;
            _inputFieldSize.onValueChanged.AddListener(OnInputValueChanged);
            _configs = Service<GameConfigs>.Get();
            _gameBoot = Service<GameBoot>.Get();
            _inputFieldSize.text = _configs.Size.ToString();
        }

        private void OnInputValueChanged(string value) {
            _currentSize = int.Parse(value);
        }

        private void StartGame() {
            _configs.Size = _currentSize;
            Hide(() => {
                _gameBoot.InitGame();
            });
        }
    }
}