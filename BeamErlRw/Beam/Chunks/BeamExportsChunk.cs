using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamExportsChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.Exports;
        public BeamExport[] Exports { get; set; }

        public static BeamExportsChunk Read(BinaryReader reader, uint size)
        {
            var count = reader.ReadUInt32();
            var exports = new BeamExport[count];
            for (int i = 0; i < count; i++)
            {
                exports[i] = new BeamExport()
                {
                    NameAtomId = reader.ReadUInt32(),
                    ArgsCount = reader.ReadUInt32(),
                    Label = reader.ReadUInt32()
                };
            }

            return new BeamExportsChunk()
            {
                Exports = exports
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Exports.Length);
            foreach (var beamExport in Exports)
            {
                writer.Write(beamExport.NameAtomId);
                writer.Write(beamExport.ArgsCount);
                writer.Write(beamExport.Label);
            }
        }
    }
}