using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermInt32 : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.Int32;
        public int Number { get; set; }

        public static TermInt32 Read(BinaryReader reader)
        {
            var val = reader.ReadInt32();
            return new TermInt32()
            {
                Number = val
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte)EExtTermType.Int32);
            writer.Write(Number);
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}