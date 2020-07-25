using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamCodeChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.Code;
        public uint InstructionsSet { get; set; }
        public uint OpcodeMax { get; set; }
        public uint LabelsCount { get; set; }
        public uint FunctionsCount { get; set; }
        public byte[] CodeBytes { get; set; }

        public static BeamCodeChunk Read(BinaryReader reader, uint size)
        {
            var instructionsSet = reader.ReadUInt32();
            var opcodeMax = reader.ReadUInt32();
            var labelCount = reader.ReadUInt32();
            var functionCount = reader.ReadUInt32();
            var code = reader.ReadBytes((int)size - sizeof(uint)*4);
            return new BeamCodeChunk()
            {
                InstructionsSet = instructionsSet,
                OpcodeMax = opcodeMax,
                LabelsCount = labelCount,
                FunctionsCount = functionCount,
                CodeBytes = code
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(InstructionsSet);
            writer.Write(OpcodeMax);
            writer.Write(LabelsCount);
            writer.Write(FunctionsCount);
            writer.Write(CodeBytes);
        }
    }
}