using BeamErlRw.Beam.Terms.Ext;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamCompileInfoChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.CompilationInfo;
        public IExtTerm Info { get; set; }

        public static BeamCompileInfoChunk Read(EndianBinaryReader reader, uint size)
        {
            var info = BeamTerm.BinaryToTerm(reader);
            return new BeamCompileInfoChunk()
            {
                Info = info
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            BeamTerm.TermToBinary(writer, Info);
        }
    }
}