using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermNil : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.Nil;

        public static TermNil Read(BinaryReader reader)
        {
            return new TermNil();
        }

        public static TermNil Read()
        {
            return new TermNil();
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte)EExtTermType.Nil);
        }

        public override string ToString()
        {
            return "Nil";
        }
    }
}