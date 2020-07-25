using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermUInt8 : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.UInt8;
        public byte Value { get; set; }

        public static TermUInt8 Read(BinaryReader reader)
        {
            var val = reader.ReadByte();
            return new TermUInt8()
            {
                Value = val
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte) EExtTermType.UInt8);
            writer.Write(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}