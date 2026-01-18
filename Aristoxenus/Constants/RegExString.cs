using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Aristoxenus.Constants
{
    public class RegExString
    {
        public const string IntervalSymbol
            = $@"(?<{Keywords.IntervalSymbol}>(#+|b+)?((?:1[43210]|[0-9])|VII|vii|VI|vi|v|V|iv|IV|iii|III|II|ii|i|I))";
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
}
