using System;
using BeamErlRw.Beam.Terms.Ext;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamAttributesChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.Attributes;
        public IExtTerm Attributes { get; set; }

        public static BeamAttributesChunk Read(EndianBinaryReader reader,uint size)
        {
            //var bytes = reader.ReadBytes((int)size);
            var oldPos = reader.Position;
            var term = BeamTerm.BinaryToTerm(reader);
            var newPos = reader.Position;
            if (oldPos + size != newPos)
            {
                throw new Exception();
            }
            return new BeamAttributesChunk()
            {
                Attributes = term
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            BeamTerm.TermToBinary(writer, Attributes);
        }
    }
}