using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamStringsChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.Strings;
        public byte[] StringsBytes { get; set; }

        public static BeamStringsChunk Read(BinaryReader reader, uint size)
        {
            var bytes = reader.ReadBytes((int) size);
            //var val = BeamTermReader.BinaryToTerm(reader);
            return new BeamStringsChunk()
            {
                StringsBytes = bytes
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(StringsBytes);
        }
    }
}