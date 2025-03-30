// В файле Startup.cs

// ... (using statements) ...

using Runtime.Combat.Systems;
using Runtime.Movement.Systems;
using Runtime.Spawning.Systems;
using Runtime.Targeting.Systems;
using Runtime.Unit.Systems;
using Scellecs.Morpeh;
using UnityEngine;

namespace Runtime.Core.Systems // или Runtime.Core.Systems
{
    public class Startup : MonoBehaviour
    {
        private World _world;

        private void Start()
        {
            _world = World.Default;
            var systemsGroup = _world.CreateSystemsGroup();
           
            systemsGroup.AddSystem(new PopulationControlSystem());
            systemsGroup.AddSystem(new PlayerSpawnSystem());
            systemsGroup.AddSystem(new EnemySpawnSystem());
            
            systemsGroup.AddSystem(new DefensePointAssignmentSystem());
            systemsGroup.AddSystem(new TargetSelectionSystem());
            
            systemsGroup.AddSystem(new PathfindingFriendlySystem());
            systemsGroup.AddSystem(new PathfindingTargetSystem()); 
           
             systemsGroup.AddSystem(new MovementSystem());
             systemsGroup.AddSystem(new FriendlyArrivalSystem());

            
            systemsGroup.AddSystem(new AttackSystem());
            systemsGroup.AddSystem(new DeathSystem());

            _world.AddSystemsGroup(order: 0, systemsGroup);
            _world.Commit();
        }
    }
}