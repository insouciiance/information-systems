using System;

namespace InformationSystems.Shared.Extensions;

public static class SingleExtensions
{
    public const int TOLERANCE = 1;

    public static unsafe bool IsEqualTo(this float lhs, float rhs)
    {
        int castLhs = *(int*)&lhs;
        int castRhs = *(int*)&rhs;

        if (castLhs >> 31 != castRhs >> 31)
            return castLhs == castRhs;

        int diff = Math.Abs(castLhs - castRhs);

        return diff <= TOLERANCE;
    }
}
