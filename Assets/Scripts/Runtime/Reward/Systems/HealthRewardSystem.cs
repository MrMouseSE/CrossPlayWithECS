using Runtime.Reward.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Runtime.Reward.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class HealthRewardSystem : ISystem
    {
        public World World { get; set; }

        private Filter _unitEnemyFilter;
        private Filter _unitPlayerFilter;
        private Filter _healthRewardFilter;
       
        private Stash<HealthComponent> _healthStash;
        private Stash<HealthReward> _rewardStash;
        

        public void OnAwake()
        {
            _healthRewardFilter = World.Filter.With<HealthReward>().Build();
            _unitEnemyFilter = World.Filter.With<UnitComponent>().With<HealthComponent>().With<EnemyMarker>().Build();
            _unitPlayerFilter = World.Filter.With<UnitComponent>().With<HealthComponent>().With<PlayerMarker>().Build();
            
            _healthStash = World.GetStash<HealthComponent>();
            _rewardStash = World.GetStash<HealthReward>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var rewardEntity in _healthRewardFilter)
            {
                if (!World.IsDisposed(rewardEntity))
                {
                    ref var reward = ref _rewardStash.Get(rewardEntity);
               
                    if (reward.TypeUnitUnits == TypeUnit.Player) 
                        ApplyReward(ref reward, _unitPlayerFilter);

                    if (reward.TypeUnitUnits == TypeUnit.Enemy)
                        ApplyReward(ref reward, _unitEnemyFilter);   
                    World.RemoveEntity(rewardEntity);
                }
            }
        }

        private void ApplyReward(ref HealthReward reward, Filter unitsFilter)
        {
            foreach (var unitEntity in unitsFilter)
            {
                ref var healthComponent = ref _healthStash.Get(unitEntity);
                healthComponent.HealthPoints += reward.Value;
            }
        }
        
        public void Dispose() { }
    }
}