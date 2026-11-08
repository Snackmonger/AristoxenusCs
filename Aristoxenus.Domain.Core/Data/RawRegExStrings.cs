namespace Aristoxenus.Domain.Core.Data;

public class RawRegExStrings
{
    public const string IntervalSymbol
        = $@"(?<{Keywords.IntervalSymbol}>(#+|b+)?(12[0-7]|1[01][0-9]|[1-9][0-9]?))";
    public const string NoteName
        = $@"(?<{Keywords.NoteName}>([A-G])(#+|b+)?)";
    public const string ChordRootName
        = $@"(?<{Keywords.RootName}>([A-G](#|b)*|(#|b)*(VII|vii|VI|vi|V|v|IV|iv|III|iii|II|ii|I|i)))";
    public const string ChordMainSymbol
        = $@"(?<{Keywords.Main}>(?:maj|M|Δ|min|m|-|dim|o|aug|\+|ø))";
    public const string ChordExtension
        = $@"(?<{Keywords.Extension}>((?:maj|M|Δ)?(13|11|9|7)))";
    public const string ChordModification
        = $@"(?<{Keywords.Modification}>(?:[\w\d\+#]+))";
}
