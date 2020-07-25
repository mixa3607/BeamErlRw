using System.Collections.Generic;
using BeamErlRw.Beam.Chunks;

namespace BeamErlRw.Beam
{
    public interface IBeamReaderOptions : ITermAtomsReadOptions
    {
        bool UseUnifiedAtomsChunk { get; set; }
        HashSet<EChunkType> ChunkTypesAsRaw { get; set; }
        void OverrideLinesChunkToRaw();
        void OverrideChunkToRaw(EChunkType type);
    }

    public interface ITermAtomsReadOptions
    {
        bool UseUnifiedIntegerExtAtom { get; set; }
    }

    public class BeamReaderOptions : IBeamReaderOptions
    {
        public bool UseUnifiedAtomsChunk { get; set; } = true;
        public HashSet<EChunkType> ChunkTypesAsRaw { get; set; } = new HashSet<EChunkType>();
        public bool UseUnifiedIntegerExtAtom { get; set; } = true;

        public void OverrideLinesChunkToRaw() 
            => ChunkTypesAsRaw.Add(EChunkType.Lines);

        public void OverrideChunkToRaw(EChunkType type)
            => ChunkTypesAsRaw.Add(type);
    }
}