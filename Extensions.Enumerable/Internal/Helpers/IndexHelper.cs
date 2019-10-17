using System.Runtime.CompilerServices;

namespace Extensions.Enumerable.Internal.Helpers
{
    internal static class IndexHelper
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static (int, int) Decompose(int rawIndex, int maxInBucket)
        {
            var partIndex = rawIndex / maxInBucket;
            var entryIndex = rawIndex % maxInBucket;

            return (partIndex, entryIndex);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Compose((int, int) decomposed, int maxInBucket)
        {
            return decomposed.Item1 * maxInBucket + decomposed.Item2;
        }

    }
}