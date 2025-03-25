using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace TetrisMechanics.Scripts.BlockMovementByInputSystem
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InputHolderProvider : MonoProvider<InputHolderComponent>
    {
    }
}