using InformationSystems.Shared.Utils;

namespace InformationSystems.Strings.Hashing;

public class ModularHashFunction : Singleton<ModularHashFunction>, IHashFunction
{
    public const int DEFAULT_MODULUS = 11;

    public int Modulus { get; }

    public ModularHashFunction()
    {
        Modulus = DEFAULT_MODULUS;
    }

    public ModularHashFunction(int modulus)
    {
        Modulus = modulus;
    }

    public int Hash(in ReadOnlySpan<char> span, int start, int end)
    {
        int hash = 0;

        foreach (char value in span[start..(end + 1)])
            hash += value;

        return hash % Modulus;
    }

    public bool TryGetPrevious(out int hash, out int start, out int end)
    {
        (hash, start, end) = (0, 0, 0);
        return false;
    }
}
