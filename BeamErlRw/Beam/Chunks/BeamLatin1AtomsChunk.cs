using System;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    [Obsolete("Use " + nameof(BeamAtomsChunk) + " instead")]
    public class BeamLatin1AtomsChunk: IBeamChunk
    {
        private const EEncodingType AtomsEncodingType = EEncodingType.Latin1;
        
        public EChunkType Type { get; } = EChunkType.Latin1Atoms;
        public string[] Atoms { get; set; }

        public static BeamLatin1AtomsChunk Read(EndianBinaryReader reader, uint size)
        {
            var atomsCount = reader.ReadUInt32();
            var atoms = new string[atomsCount];
            for (int i = 0; i < atomsCount; i++)
            {
                var atomLen = reader.ReadByte();
                atoms[i] = reader.ReadString(atomLen, AtomsEncodingType);
            }
            return new BeamLatin1AtomsChunk()
            {
                Atoms = atoms,
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            var encoding = Enc.Enc.Get(AtomsEncodingType);
            writer.Write(Atoms.Length);
            foreach (var atom in Atoms)
            {
                var atomBytes = encoding.GetBytes(atom);
                writer.Write((byte) atomBytes.Length);
                writer.Write(atomBytes);
            }
        }
    }
}