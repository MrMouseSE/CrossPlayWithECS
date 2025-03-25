using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockSystem;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputHolderSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<InputHolderComponent> _inputHodlerComponentStash;

        private InputAction _moveAction;
        private InputAction _rotateAction;
        private readonly InputAction _forceDown;
        private Stash<StepAwaiterComponent> _stepAwaiterComponentStash;

        public InputHolderSystem(InputActionAsset asset)
        {
            _moveAction = asset.FindAction("BlockHorizontalMove");
            _rotateAction = asset.FindAction("BlockRotation");
            _forceDown = asset.FindAction("BlockForceDown");
        }

        public void OnAwake()
        {
            _filter = World.Filter.With<InputHolderComponent>().Build();
            _inputHodlerComponentStash = World.GetStash<InputHolderComponent>();
        }

        public void OnUpdate(float deltaTime)
        {
            ref var inputHolderComponent = ref _inputHodlerComponentStash.Get(_filter.First());
            if (_moveAction.WasPerformedThisFrame()) inputHolderComponent.HorizontalMovementDirection = Vector3.right * _moveAction.ReadValue<float>();
            if (_rotateAction.WasPerformedThisFrame()) inputHolderComponent.SetBlockRotation((int)_rotateAction.ReadValue<float>());
            inputHolderComponent.IsForcedDown = _forceDown.WasPerformedThisFrame();
        }

        public void Dispose()
        {
        }
    }
}