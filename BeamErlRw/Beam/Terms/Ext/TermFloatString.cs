using System.IO;
using BeamErlRw.Beam.Exceptions;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermFloatString : IExtTerm
    {
        public const int FloatSize = 31;

        public EInternalTermType Type { get; } = EInternalTermType.FloatString;
        public byte[] DataBytes { get; set; }
        public string FloatString => Enc.Enc.Get(EEncodingType.Utf8).GetString(DataBytes);

        public static TermFloatString Read(BinaryReader reader)
        {
            var bytes = reader.ReadBytes(FloatSize);
            return new TermFloatString()
            {
                DataBytes = bytes
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            if (DataBytes.Length != FloatSize)
            {
                throw new WriteBytesCountException(DataBytes.Length, FloatSize);
            }

            writer.Write((byte) EExtTermType.FloatString);
            writer.Write(DataBytes);
        }

        public override string ToString()
        {
            return FloatString;
        }
    }
}