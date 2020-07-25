using System.IO;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermBinary : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.Binary;
        public byte[] DataBytes { get; set; }
        public string String => Enc.Enc.Get(EEncodingType.Utf8).GetString(DataBytes);

        public static TermBinary Read(BinaryReader reader)
        {
            var len = reader.ReadUInt32();
            var bytes = reader.ReadBytes((int) len);
            return new TermBinary()
            {
                DataBytes = bytes
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte) EExtTermType.Binary);
            writer.Write((uint) DataBytes.LongLength);
            writer.Write(DataBytes);
        }

        public override string ToString()
        {
            return String;
        }
    }
}