namespace Aristoxenus.SharedKernel.Interfaces
{
    public interface ISymbolSerializable<T>
    {
        public string Symbol { get; }
        public abstract static T FromSymbol(string symbol);
    }
}
