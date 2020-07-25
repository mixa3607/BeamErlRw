using System;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    [Obsolete("Use " + nameof(BeamAtomsChunk) + " instead")]
    public class BeamUtf8AtomsChunk : IBeamChunk
    {
        private const EEncodingType AtomsEncodingType = EEncodingType.Utf8;

        public EChunkType Type { get; } = EChunkType.Utf8Atoms;
        public string[] Atoms { get; set; }

        public static BeamUtf8AtomsChunk Read(EndianBinaryReader reader, uint size)
        {
            var atomsCount = reader.ReadUInt32();
            var atoms = new string[atomsCount];
            for (int i = 0; i < atomsCount; i++)
            {
                var atomLen = reader.ReadByte();
                atoms[i] = reader.ReadString(atomLen, AtomsEncodingType);
            }
            return new BeamUtf8AtomsChunk()
            {
                Atoms = atoms
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            var enc = Enc.Enc.Get(AtomsEncodingType);
            writer.Write(Atoms.Length);
            foreach (var atom in Atoms)
            {
                var bytes = enc.GetBytes(atom);
                if (bytes.Length > byte.MaxValue)
                {
                    throw new Exception();
                }

                writer.Write((byte) bytes.Length);
                writer.Write(bytes);
            }
        }
    }
}