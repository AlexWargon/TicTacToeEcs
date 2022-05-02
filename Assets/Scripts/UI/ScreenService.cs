using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TicTacToeEcs {
    public class ScreenService : MonoBehaviour, IInitService {
        [SerializeField] private List<UIElement> _uiElementsList;
        private Dictionary<Type, UIElement> _uiElements;

        public void Init() {
            _uiElements = new Dictionary<Type, UIElement>();
            foreach (var uiElement in _uiElementsList) _uiElements.Add(uiElement.GetType(), uiElement);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Show<T>(Action callback = null) where T : UIElement {
            var type = typeof(T);
            if (!_uiElements.ContainsKey(type))
                Debug.LogError($"{type.Name} not registered");
            var uiElement = _uiElements[type];
            uiElement.Show(callback);
            return (T) uiElement;
        }

        public T Hide<T>(Action callback = null) where T : UIElement {
            var type = typeof(T);
            if (!_uiElements.ContainsKey(type))
                Debug.LogError($"{type.Name} not registered");
            var uiElement = _uiElements[type];
            uiElement.Hide(callback);
            return (T) uiElement;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>() where T : UIElement {
            return (T) _uiElements[typeof(T)];
        }
    }
}