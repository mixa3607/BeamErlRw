using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamAbstractCodeChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.AbstractCode;
        public byte[] CodeBytes { get; set; }

        public static BeamAbstractCodeChunk Read(BinaryReader reader, uint size)
        {
            return new BeamAbstractCodeChunk()
            {
                CodeBytes = reader.ReadBytes((int)size)
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(CodeBytes);
        }
    }
}