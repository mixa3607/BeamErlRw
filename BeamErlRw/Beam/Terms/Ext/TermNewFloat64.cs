using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermNewFloat64 : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.NewFloat64;
        public double Value { get; set; }

        public static TermNewFloat64 Read(EndianBinaryReader reader)
        {
            return new TermNewFloat64()
            {
                Value = reader.ReadDouble()
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte) EExtTermType.NewFloat64);
            writer.Write(Value);
        }

        public override string ToString()
        {
            return "";
        }
    }
}