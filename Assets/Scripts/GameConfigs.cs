using System.Collections.Generic;
using UnityEngine;

namespace TicTacToeEcs {
    [CreateAssetMenu(fileName = "Config", menuName = "ScriptableObjects/GameConfigs", order = 1)]
    internal class GameConfigs : ScriptableObject, IInitService {
        public int Size;
        public float OffsetSize;
        public GameObject CellPrefab;
        public GameObject CrossPrefab;
        public GameObject CirclePrefab;
        public Dictionary<SignType, GameObject> SignsPrefabs;

        void IInitService.Init() {
            SignsPrefabs = new Dictionary<SignType, GameObject>();
            SignsPrefabs.Add(SignType.Circle, CirclePrefab);
            SignsPrefabs.Add(SignType.Cross, CrossPrefab);
        }
    }
}