using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TicTacToeEcs {
    public class GameBoot : MonoBehaviour {
        [SerializeField] private GameConfigs _configs;
        [SerializeField] private ScreenService _screenService;

        private void Awake() {
            Service<GameConfigs>.Set(_configs);
            Service<GameRunTimeData>.Set(new GameRunTimeData());
            Service<ScreenService>.Set(_screenService);
            Service<GameBoot>.Set(this);
            var world = World.DefaultGameObjectInjectionWorld;
            var manager = world.EntityManager;
            var initEntity = manager.CreateEntity();

            manager.AddComponentData(initEntity, new InitGameEvent());
        }

        public void DestroyEcs() {
            var world = World.DefaultGameObjectInjectionWorld;
            var manager = world.EntityManager;
            foreach (var e in manager.GetAllEntities()) {
                manager.DestroyEntity(e);
            }
        }
    }
    
    public class GameRunTimeData {
        public readonly Dictionary<int2, Entity> Cells = new Dictionary<int2, Entity>();
        public SignType AI;
        public SignType CurrentSign;
        public SignType Player;
        public int TurnsAmount;
    }

    public enum SignType {
        Cross,
        Circle
    }
}