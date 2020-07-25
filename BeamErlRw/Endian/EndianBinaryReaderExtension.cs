using BeamErlRw.Enc;

namespace BeamErlRw.Endian
{
    public static class EndianBinaryReaderExtension
    {
        public static string ReadString(this EndianBinaryReader reader, int bytesCount, EEncodingType encodingType)
        {
            var enc = Enc.Enc.Get(encodingType);
            var bytes = reader.ReadBytes(bytesCount);
            return enc.GetString(bytes);
        }
    }
}