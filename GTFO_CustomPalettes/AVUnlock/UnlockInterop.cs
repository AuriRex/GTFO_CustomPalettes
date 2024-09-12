using CustomPalettes.Core;
using GameData;
using System.Runtime.CompilerServices;

namespace CustomPalettes.AVUnlock
{
    internal static class UnlockInterop
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void Register()
        {
            AllVanity.Unlock.RegisterUnlockMethod(Impl.UnlockFunc);
        }

        private static class Impl
        {
            internal static AllVanity.UnlockState UnlockFunc(VanityItemsTemplateDataBlock block)
            {
                //L.Warning($"{nameof(UnlockFunc)} is running");

                if (block.type != ClothesType.Palette)
                    return AllVanity.UnlockState.Skip;

                if (!block.name.StartsWith(PaletteManager.BLOCK_PREFIX))
                    return AllVanity.UnlockState.Skip;
                
                if (!PaletteManager.TryGetPaletteFromBlock(block, out var palette))
                    return AllVanity.UnlockState.Skip;

                if (!palette.Locked)
                    return AllVanity.UnlockState.TryUnlock;

                return AllVanity.UnlockState.TryLock;
            }
        }
    }
}
