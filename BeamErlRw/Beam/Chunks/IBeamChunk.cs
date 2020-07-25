using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public interface IBeamChunk
    {
        public EChunkType Type { get; }

        void Write(EndianBinaryWriter writer);
    }
}