namespace Aristoxenus.Core.SharedKernel.Interfaces
{
    public interface IMusicXmlSerializable<T>
    {
        public string ToMusicXML();

        public T FromMusicXML(string xml);
    }
}
