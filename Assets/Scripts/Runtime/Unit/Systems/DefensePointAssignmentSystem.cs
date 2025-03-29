using System.Collections.Generic;
using Runtime.Movement.Components;
using Runtime.Unit.Components;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Runtime.Unit.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DefensePointAssignmentSystem : ISystem
    {
        public World World { get; set; }

        private Filter _newFriendlyUnitsFilter;
        private Filter _allDefensePointsFilter;
        private Filter _occupiedDefensePointsFilter; // Для проверки занятости

        private Stash<PlayerMarker> _playerMarkerStash; // Используем существующие
        private Stash<UnitComponent> _unitStash;
        private Stash<NavMeshAgentComponent> _navMeshAgentStash;
        private Stash<AssignedDefensePoint> _assignedPointStash;
        private Stash<IsMovingToDefensePoint> _isMovingStash;
        private Stash<IsOccupied> _isOccupiedStash;
        private Stash<DefensePointComponent> _defensePointStash;
        private Stash<IsDefaultDefensePoint> _isDefaultPointStash; // Для поиска дефолтной точки

        private Entity _defaultDefensePointEntity = default;

        // Кэш для оптимизации (простой вариант)
        private List<Entity> _availableDefensePoints = new List<Entity>();

        public void OnAwake()
        {
            // Фильтр для новых юнитов игрока, которым еще не назначена точка
            _newFriendlyUnitsFilter = World.Filter
                .With<PlayerMarker>()
                .With<UnitComponent>()
                .With<NavMeshAgentComponent>()
                .Without<AssignedDefensePoint>()
                .Build();

            // Фильтры для точек обороны
            _allDefensePointsFilter = World.Filter.With<DefensePointComponent>().With<UnitComponent>().Build(); // Предполагаем UnitComponent для позиции
            _occupiedDefensePointsFilter = World.Filter.With<DefensePointComponent>().With<IsOccupied>().Build();

            // Стэши
            _unitStash = World.GetStash<UnitComponent>();
            _navMeshAgentStash = World.GetStash<NavMeshAgentComponent>();
            _assignedPointStash = World.GetStash<AssignedDefensePoint>();
            _isMovingStash = World.GetStash<IsMovingToDefensePoint>();
            _isOccupiedStash = World.GetStash<IsOccupied>();
            _defensePointStash = World.GetStash<DefensePointComponent>();
             _isDefaultPointStash = World.GetStash<IsDefaultDefensePoint>();

            // Находим и кэшируем дефолтную точку
            var defaultPointFilter = World.Filter.With<DefensePointComponent>().With<IsDefaultDefensePoint>().Build();
            foreach (var entity in defaultPointFilter)
            {
                _defaultDefensePointEntity = entity;
                break; // Предполагаем, что она одна
            }
            if (_defaultDefensePointEntity == default)
            {
                Debug.LogError("Default Defense Point not found!");
            }
        }

        public void OnUpdate(float deltaTime)
        {
             // *** ОПТИМИЗАЦИЯ: Обновление списка доступных точек ***
            // Вместо того чтобы каждый раз искать ближайшую среди всех,
            // лучше поддерживать актуальный список свободных точек.
            // Этот список можно обновлять здесь или в отдельной системе,
            // реагирующей на добавление/удаление IsOccupied.
            // Пока что реализуем простой поиск ближайшей свободной.
            // Для 1000 юнитов это МОЖЕТ БЫТЬ МЕДЛЕННО! Рассмотрите оптимизацию позже.

            UpdateAvailableDefensePointsList(); // Обновляем список доступных точек

             foreach (var unitEntity in _newFriendlyUnitsFilter)
            {
                ref var unitComponent = ref _unitStash.Get(unitEntity);
                Vector3 unitPosition = unitComponent.RootTransform.position;

                Entity bestPointEntity = default;
                float minSqrDistance = float.MaxValue;

                // Ищем ближайшую СВОБОДНУЮ точку
                foreach (var pointEntity in _availableDefensePoints) // Используем кэшированный список свободных
                {
                    // Проверка на всякий случай, если точка была занята с момента обновления кэша
                    if (_isOccupiedStash.Has(pointEntity)) continue;

                    // Получаем позицию точки (предполагаем UnitComponent на точке)
                    ref var pointUnitComponent = ref _unitStash.Get(pointEntity);
                    Vector3 pointPosition = pointUnitComponent.RootTransform.position;

                    float sqrDistance = (unitPosition - pointPosition).sqrMagnitude;
                    if (sqrDistance < minSqrDistance)
                    {
                        minSqrDistance = sqrDistance;
                        bestPointEntity = pointEntity;
                    }
                }

                Entity assignedEntity;
                if (bestPointEntity != default)
                {
                    // Нашли свободную точку
                    assignedEntity = bestPointEntity;
                    _isOccupiedStash.Add(assignedEntity); // Помечаем точку как занятую
                     // Debug.Log($"Unit {unitEntity.ID} assigned to point {assignedEntity.ID}");
                }
                else
                {
                    // Свободных точек нет, используем дефолтную
                    assignedEntity = _defaultDefensePointEntity;
                     // Debug.Log($"Unit {unitEntity.ID} assigned to DEFAULT point {assignedEntity.ID}");
                     // Дефолтную точку не помечаем как занятую, она может быть переполнена
                }

                // Назначаем точку юниту
                _assignedPointStash.Set(unitEntity, new AssignedDefensePoint { TargetPointEntity = assignedEntity });
                _isMovingStash.Add(unitEntity); // Добавляем маркер движения к точке
            }
        }

        // Простой метод обновления списка доступных точек (вызывать в OnUpdate)
        // Для оптимизации лучше переделать на систему, реагирующую на события
        private void UpdateAvailableDefensePointsList()
        {
            _availableDefensePoints.Clear();
            foreach(var pointEntity in _allDefensePointsFilter)
            {
                if (!_isOccupiedStash.Has(pointEntity))
                {
                    _availableDefensePoints.Add(pointEntity);
                }
            }
        }

        public void Dispose() { }
    }
}