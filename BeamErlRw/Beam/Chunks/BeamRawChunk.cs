using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamRawChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.Raw;
        public string Name { get; set; }
        public byte[] DataBytes { get; set; }

        public static BeamRawChunk Read(BinaryReader reader, uint size, string name)
        {
            return new BeamRawChunk()
            {
                Name = name,
                DataBytes = reader.ReadBytes((int)size)
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(DataBytes);
        }
    }
}