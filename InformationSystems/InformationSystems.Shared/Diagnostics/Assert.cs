using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace InformationSystems.Shared.Diagnostics;

public static class Assert
{
    public static void True(
        [DoesNotReturnIf(false)] bool value,
        [CallerArgumentExpression("value")] string argumentExpression = "")
    {
        if (!value)
            throw new AssertionException($"Expected true, got false: {argumentExpression}");
    }

    public static void False(
        [DoesNotReturnIf(true)] bool value,
        [CallerArgumentExpression("value")] string argumentExpression = "")
    {
        if (value)
            throw new AssertionException($"Expected false, got true: {argumentExpression}");
    }
}
