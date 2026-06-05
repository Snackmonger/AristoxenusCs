namespace Aristoxenus.Core.Shared.Interfaces
{
    /// <summary>
    /// A contract for serializing and deserializing objects by means of
    /// a string that symbolizes their exact values.
    /// </summary>
    public interface ISymbolSerializable<T>
    {
        /// <summary>
        /// Convert this data into a string symbol by which it can be
        /// serialized.
        /// </summary>
        public string ToSymbol();

        /// <summary>
        /// Initialize this data type from the given string symbol.
        /// </summary>
        public abstract static T FromSymbol(string symbol);
    }
}
