namespace InformationSystems.Strings.Hashing;

public interface IHashFunction
{
    int Hash(in ReadOnlySpan<char> span, int start, int end);

    bool TryGetPrevious(out int hash, out int start, out int end);
}
