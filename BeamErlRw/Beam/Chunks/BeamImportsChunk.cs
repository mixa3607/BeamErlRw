using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamImportsChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.Imports;
        public BeamImport[] Imports { get; set; }

        public static BeamImportsChunk Read(BinaryReader reader, uint size)
        {
            var count = reader.ReadUInt32();
            var imports = new BeamImport[count];
            for (int i = 0; i < count; i++)
            {
                imports[i] = new BeamImport()
                {
                    ModuleNameAtomId = reader.ReadUInt32(),
                    FuncNameAtomId = reader.ReadUInt32(),
                    ArgsCount = reader.ReadUInt32(),
                };
            }

            return new BeamImportsChunk()
            {
                Imports = imports
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Imports.Length);
            foreach (var import in Imports)
            {
                writer.Write(import.ModuleNameAtomId);
                writer.Write(import.FuncNameAtomId);
                writer.Write(import.ArgsCount);
            }
        }
    }
}