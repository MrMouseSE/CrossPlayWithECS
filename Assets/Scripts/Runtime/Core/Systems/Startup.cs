using Runtime.Combat.Systems;
using Runtime.Movement.Systems;
using Runtime.Reward.Systems;
using Runtime.Spawning.Systems;
using Runtime.Targeting.Systems;
using Runtime.Unit.Systems;
using Scellecs.Morpeh;
using UnityEngine;

namespace Runtime.Core.Systems
{
    public class Startup : MonoBehaviour
    {
        private World _world;

        private void Start()
        {
            Application.targetFrameRate = 120;
            
            _world = World.Default;
            var systemsGroup = _world.CreateSystemsGroup();
            
            systemsGroup.AddSystem(new SpawnSystem());
            
            systemsGroup.AddSystem(new DefensePointAssignmentSystem());
            // systemsGroup.AddSystem(new TargetSelectionSystem());
            // systemsGroup.AddSystem(new TargetAnimationSystem());
            //
            // systemsGroup.AddSystem(new PathfindingFriendlySystem());
            // systemsGroup.AddSystem(new PathfindingTargetSystem());
            //
            // systemsGroup.AddSystem(new MovementSystem());
            // systemsGroup.AddSystem(new MovementAnimationSystem());
            // systemsGroup.AddSystem(new FriendlyArrivalSystem());
            //
            // systemsGroup.AddSystem(new AttackSystem());
            // systemsGroup.AddSystem(new DeathSystem());
            //
            // systemsGroup.AddSystem(new HealthRewardSystem());
            
            
            
#if UNITY_EDITOR
            systemsGroup.AddSystem(new UnitDebugSystem());
#endif

            _world.AddSystemsGroup(order: 0, systemsGroup);
            _world.Commit();
        }
    }
}