using InformationSystems.Strings.Hashing;

namespace InformationSystems.Strings.Extensions;

public static class StringExtensions
{
    public static int HashIndexOf(in this ReadOnlySpan<char> span, in ReadOnlySpan<char> entry, IHashFunction? hashFunction = null)
    {
        hashFunction ??= ModularHashFunction.Instance;

        int entryHash = hashFunction.Hash(entry, 0, entry.Length - 1);

        for (int i = 0; i <= span.Length - entry.Length; i++)
        {
            int currentEntryHash = hashFunction.Hash(span, i, i + entry.Length - 1);

            if (entryHash == currentEntryHash && entry.SequenceEqual(span[i..(i + entry.Length)]))
                return i;
        }

        return -1;
    }

    public static int HashIndexOf(this string value, string entry, IHashFunction? hashFunction = null)
        => HashIndexOf(value.AsSpan(), entry.AsSpan(), hashFunction);
}
