using System.Collections.Generic;

namespace MoreSpeeds;

internal static class CustomSpeeds
{
    public static readonly Dictionary<string, float> Speeds = new()
    {
        { "1.25x", 1.25f },
        { "1.5x", 1.5f },
        { "1.75x", 1.75f },
        { "2.0x", 2f },
        { "2.5x", 2.5f },
        { "3x", 3f },
        { "4x", 4f },
        { "5x", 5f },
        { "10x", 10f },
    };
}
