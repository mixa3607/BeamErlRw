using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamAtomsChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.Atoms;
        public EEncodingType EncodingType { get; set; } = EEncodingType.Latin1; 
        public string[] Atoms { get; set; }

        public static BeamAtomsChunk Read(EndianBinaryReader reader, EEncodingType encodingType)
        {
            var atomsCount = reader.ReadUInt32();
            var atoms = new string[atomsCount];
            for (int i = 0; i < atomsCount; i++)
            {
                var atomLen = reader.ReadByte();
                atoms[i] = reader.ReadString(atomLen, encodingType);
            }
            return new BeamAtomsChunk()
            {
                Atoms = atoms,
                EncodingType = encodingType
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            var encoding = Enc.Enc.Get(EncodingType);
            writer.Write(Atoms.Length);
            foreach (var atom in Atoms)
            {
                var atomBytes = encoding.GetBytes(atom);
                writer.Write((byte)atomBytes.Length);
                writer.Write(atomBytes);
            }
        }
    }
}