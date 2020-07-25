using BeamErlRw.Beam.Chunks;

namespace BeamErlRw.Beam
{
    public class BeamFile
    {
        public BeamHeader Header { get; set; } = new BeamHeader();
        public IBeamChunk[] Chunks { get; set; } = new IBeamChunk[0];

        public override string ToString()
        {
            return $"Size: {Header?.Size}\nChunks count: {Chunks?.Length}";
        }
    }
}