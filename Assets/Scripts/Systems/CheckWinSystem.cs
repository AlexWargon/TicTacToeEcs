using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TicTacToeEcs {
    [UpdateAfter(typeof(DrawSignSystem))]
    public partial class CheckWinSystem : SystemBase {
        private GameConfigs _configs;
        private GameRunTimeData _runTimeData;
        private ScreenService _screenService;
        private EntityQuery _freeCells;
        protected override void OnStartRunning() {
            _runTimeData = Service<GameRunTimeData>.Get();
            _configs = Service<GameConfigs>.Get();
            _screenService = Service<ScreenService>.Get();
            _freeCells = GetEntityQuery(typeof(Cell), ComponentType.Exclude<Sign>());
        }

        protected override void OnUpdate() {
            Entities.WithAll<CheckWinEvent>().ForEach((in Entity entity, in Position position, in Sign sing) => {
                var chainLenght = GetLongestChain(_runTimeData.Cells, position.value);
                if (chainLenght >= _configs.Size) {
                    _screenService.Get<WinScreen>().SetWinner(sing.value);
                    _screenService.Show<WinScreen>();
                    var gameOver = EntityManager.CreateEntity();
                    EntityManager.AddComponent<GameOverFlag>(gameOver);
                }
                else
                if(_freeCells.IsEmpty){
                    _screenService.Get<WinScreen>().SetGameOver();
                    _screenService.Show<WinScreen>();
                }
                EntityManager.RemoveComponent<CheckWinEvent>(entity);
                
                _runTimeData.CurrentSign = sing.value == SignType.Cross ? SignType.Circle : SignType.Cross;
                if (_runTimeData.CurrentSign == _runTimeData.AI) {
                    var e = EntityManager.CreateEntity();
                    EntityManager.AddComponent<AiTurnFlag>(e);
                }
            }).WithStructuralChanges().WithoutBurst().Run();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLongestChain(Dictionary<int2, Entity> cells, int2 position) {
            var startEntity = cells[position];
            if (!EntityManager.HasComponent<Sign>(startEntity)) return 0;
            var startType = EntityManager.GetComponentData<Sign>(startEntity).value;

            var horizontalLenght = Count(cells, position, startType, new int2(1, 0), new int2(-1, 0));
            var verticalLenght = Count(cells, position, startType, new int2(0, 1), new int2(0, -1));
            var diagonal1 = Count(cells, position, startType, new int2(-1, -1), new int2(1, 1));
            var diagonal2 = Count(cells, position, startType, new int2(-1, 1), new int2(1, -1));

            return Mathf.Max(horizontalLenght, verticalLenght, diagonal1, diagonal2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int Count(IReadOnlyDictionary<int2, Entity> cells, int2 pos, SignType signType, int2 direction1,
            int2 direction2) {
            var i = 1;
            var currentPosition = pos + direction1;
            while (cells.TryGetValue(currentPosition, out var entity)) {
                if (!EntityManager.HasComponent<Sign>(entity)) break;

                var type = EntityManager.GetComponentData<Sign>(entity).value;
                if (type != signType) break;
                i++;
                currentPosition += direction1;
            }

            currentPosition = pos + direction2;
            while (cells.TryGetValue(currentPosition, out var entity)) {
                if (!EntityManager.HasComponent<Sign>(entity)) break;

                var type = EntityManager.GetComponentData<Sign>(entity).value;
                if (type != signType) break;
                i++;
                currentPosition += direction2;
            }

            return i;
        }
    }
}