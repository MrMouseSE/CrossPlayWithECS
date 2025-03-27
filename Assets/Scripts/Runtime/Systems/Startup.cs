using Scellecs.Morpeh;
using UnityEngine;

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
            var targetSelectionSystem = new TargetSelectionSystem();
            var pathfindingEnemySystem = new PathfindingEnemySystem();
            var movementSystem = new MovementSystem();
            var populationControlSystem = new PopulationControlSystem();
            var attackSystem = new AttackSystem();
            var deathSystem = new DeathSystem();

            systemsGroup.AddSystem(populationControlSystem);
            systemsGroup.AddSystem(playerSpawnSystem);
            systemsGroup.AddSystem(enemySpawnSystem);
            systemsGroup.AddSystem(targetSelectionSystem);
            systemsGroup.AddSystem(pathfindingEnemySystem);
            systemsGroup.AddSystem(movementSystem);
            systemsGroup.AddSystem(attackSystem);
            systemsGroup.AddSystem(deathSystem);

            //systemsGroup.Initialize();
            _world.AddSystemsGroup(order: 0, systemsGroup);
            _world.Commit();
        }
    }
}