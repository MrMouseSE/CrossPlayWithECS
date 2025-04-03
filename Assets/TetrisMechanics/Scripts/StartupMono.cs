using Scellecs.Morpeh;
using TetrisMechanics.Scripts.BlockDestroySystem;
using TetrisMechanics.Scripts.BlockMovementByInputSystem;
using TetrisMechanics.Scripts.BlockSpawnSystem;
using TetrisMechanics.Scripts.BlockSystem;
using TetrisMechanics.Scripts.UnitsSpawnSystem;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TetrisMechanics.Scripts
{
    public class StartupMono : MonoBehaviour
    {
        [Header("Game Rules Data")]
        public ScoreData ScoreData;
        public UnitSpawnRules UnitSpawnRules;
        
        [Space]
        public InputActionAsset InputActions;
        public TextMeshPro ScoreText;
        
        private World _world;
        
        void Start()
        {
            _world = World.Default;
            
            StaticScoreHolder.ScoreText = ScoreText;

            var spawnSystem = _world.CreateSystemsGroup();
            spawnSystem.AddSystem(new SpawnSystem());
            
            
            var inputMovementSystem = _world.CreateSystemsGroup();
            inputMovementSystem.AddSystem(new InputHolderSystem(InputActions));
            inputMovementSystem.AddSystem(new VerticalBorderCheckSystem());
            inputMovementSystem.AddSystem(new InputForceDownSystem());
            inputMovementSystem.AddSystem(new InputMovementSystem());
            inputMovementSystem.AddSystem(new InputValuesRestoreSystem());
            
            var blockMovementSystem = _world.CreateSystemsGroup();
            blockMovementSystem.AddSystem(new BlockMoveSystem());
            
            var landingSystem = _world.CreateSystemsGroup();
            landingSystem.AddSystem(new BlockLandingSystem());
            landingSystem.AddSystem(new LandedBlockSetToHolderSystem());
            landingSystem.AddSystem(new HorizontalLineFillCheckSystem(ScoreData));
            landingSystem.AddSystem(new LandedBlockDestroySystem());
            landingSystem.AddSystem(new MoveBlockAfterDestroySystem());
            landingSystem.AddSystem(new CurrentSelectedSystem());
            
            
            _world.AddSystemsGroup(100, spawnSystem);
            _world.AddSystemsGroup(300, blockMovementSystem);
            _world.AddSystemsGroup(400, inputMovementSystem);
            _world.AddSystemsGroup(500, landingSystem);
        }
    }
}