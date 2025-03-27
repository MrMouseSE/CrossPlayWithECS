using Runtime.Components;
using Runtime.StaticUtils;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace Runtime.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PopulationControlSystem : ISystem
    {
        public World World { get; set; }
        
        private Stash<EnemyUnitCount> _enemyUnitCountStash;
        private Entity _enemyUnitCountEntity;
        
        private Stash<PlayerUnitCount> _playerUnitCountStash;
        private Entity _playerUnitCountEntity;
        

        public void OnAwake()
        {
            _enemyUnitCountEntity = World.CreateEntity();
            _enemyUnitCountStash = World.GetStash<EnemyUnitCount>();
            _enemyUnitCountStash.Set(_enemyUnitCountEntity, new EnemyUnitCount() { Count = 10 });
            
            _playerUnitCountEntity = World.CreateEntity();
            _playerUnitCountStash = World.GetStash<PlayerUnitCount>();
            _playerUnitCountStash.Set(_playerUnitCountEntity, new PlayerUnitCount { Count = 5 });

        }

        public void OnUpdate(float deltaTime)
        {
           
        }

        public void Dispose()
        {
        }
    }
}