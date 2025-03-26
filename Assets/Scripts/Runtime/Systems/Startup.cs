using Runtime.Providers;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Systems
{
    public class Startup : MonoBehaviour
    {
        private World _world;

        private void Start()
        {
            _world = World.Default;

            var systemsGroup = _world.CreateSystemsGroup();

            var enemySpawnSystem = new EnemySpawnSystem();
            var playerSpawnSystem = new PlayerSpawnSystem();
            var pathfindingEnemySystem = new PathfindingEnemySystem();
            var movementSystem = new MovementSystem();
            
            systemsGroup.AddSystem(playerSpawnSystem);
            systemsGroup.AddSystem(enemySpawnSystem);
            systemsGroup.AddSystem(pathfindingEnemySystem);
            systemsGroup.AddSystem(movementSystem);

            systemsGroup.Initialize();
            _world.AddSystemsGroup(order: 0, systemsGroup);
            _world.Commit();
        }
    }
}