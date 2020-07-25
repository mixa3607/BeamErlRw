using System.IO;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermString : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.String;
        public byte[] DataBytes { get; set; }
        public string String => Enc.Enc.Get(EEncodingType.Utf8).GetString(DataBytes);

        public static TermString Read(BinaryReader reader)
        {
            var len = reader.ReadUInt16();
            var bytes = reader.ReadBytes(len);
            return new TermString()
            {
                DataBytes = bytes
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte) EExtTermType.String);
            writer.Write((ushort)DataBytes.Length);
            writer.Write(DataBytes);
        }

        public override string ToString()
        {
            return String;
        }
    }
}