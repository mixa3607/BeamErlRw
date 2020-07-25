using System.IO;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamLocalFuncsChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.LocalFuncs;
        public BeamLocalFunc[] LocalFuncs { get; set; }

        public static BeamLocalFuncsChunk Read(BinaryReader reader,uint size)
        {
            var count = reader.ReadUInt32();
            var funcs = new BeamLocalFunc[count];
            for (int i = 0; i < count; i++)
            {
                funcs[i] = new BeamLocalFunc()
                {
                    NameAtomId = reader.ReadUInt32(),
                    ArgsCount = reader.ReadUInt32(),
                    Label = reader.ReadUInt32()
                };
            }
            return new BeamLocalFuncsChunk()
            {
                LocalFuncs = funcs
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(LocalFuncs.Length);
            foreach (var func in LocalFuncs)
            {
                writer.Write(func.NameAtomId);
                writer.Write(func.ArgsCount);
                writer.Write(func.Label);
            }
        }
    }
}