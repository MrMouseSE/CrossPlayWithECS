using Runtime.Reward.Components;
using Scellecs.Morpeh;
using TriInspector;
using UnityEngine;

namespace Runtime.Core.Systems
{
    public class TestRewards : MonoBehaviour
    {
        private World _world;

        [Button]
        public void AddHealthReward(float value, TypeUnit typeUnits )
        {
            _world = World.Default;
            var entity = _world.CreateEntity();

            var healthStash = _world.GetStash<HealthReward>();
            healthStash.Set(entity, new HealthReward { Value = value,TypeUnitUnits = typeUnits});
        }
    }
}