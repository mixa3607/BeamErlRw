using System;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermTuple : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.Tuple;
        public IExtTerm[] Elements { get; set; }

        public static TermTuple Read(EndianBinaryReader reader, ETermSize size)
        {
            var len = size switch
            {
                ETermSize.Small => reader.ReadByte(),
                ETermSize.Large => reader.ReadUInt32(),
                _ => throw new NotSupportedException(),
            };
            var elements = new IExtTerm[len];
            for (long i = 0; i < elements.LongLength; i++)
            {
                elements[i] = BeamTerm.BinaryToTerm(reader, false);
            }

            return new TermTuple()
            {
                Elements = elements,
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            if (Elements.Length <= byte.MaxValue)
            {
                writer.Write((byte) EExtTermType.Tuple8);
                writer.Write((byte) Elements.Length);
            }
            else
            {
                writer.Write((byte) EExtTermType.Tuple32);
                writer.Write(Elements.Length);
            }

            foreach (var element in Elements)
            {
                BeamTerm.TermToBinary(writer, element, false);
            }
        }
    }
}