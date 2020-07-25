using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermList : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.List;
        public IExtTerm[] Elements { get; set; }
        public IExtTerm Tail { get; set; }

        public static TermList Read(EndianBinaryReader reader)
        {
            var len = reader.ReadUInt32();
            var elements = new IExtTerm[len];
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = BeamTerm.BinaryToTerm(reader, false);
            }

            var tail = BeamTerm.BinaryToTerm(reader, false);

            return new TermList()
            {
                Elements = elements,
                Tail = tail
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write((byte) EExtTermType.List);
            writer.Write(Elements.Length);
            foreach (var element in Elements)
            {
                BeamTerm.TermToBinary(writer, element, false);
            }
            BeamTerm.TermToBinary(writer, Tail, false);
        }
    }
}