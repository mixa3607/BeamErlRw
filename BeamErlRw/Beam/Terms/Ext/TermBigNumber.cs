using System;
using System.Linq;
using System.Numerics;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermBigNumber : IExtTerm
    {
        private const byte NegativeMark = 1;
        private const byte PositiveMark = 0;

        public EInternalTermType Type { get; } = EInternalTermType.BigInt;
        public BigInteger Number { get; set; }
        public bool AddOneByte { get; set; } = false;

        public static TermBigNumber Read(EndianBinaryReader reader, ETermSize size)
        {
            var len = size switch
            {
                ETermSize.Large => reader.ReadUInt32(),
                ETermSize.Small => reader.ReadByte(),
                _ => throw new ArgumentOutOfRangeException(nameof(size), size, null)
            };
            var sign = reader.ReadByte();
            var valBytes = reader.ReadBytes((int) len);
            var val = new BigInteger(valBytes, true, true);
            if (sign == NegativeMark)
                val *= -1;

            return new TermBigNumber()
            {
                Number = val,
                AddOneByte = valBytes.Length >= 2 && valBytes[0] == 0
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            var bytes = Number.ToByteArray(true, true);
            var sign = Number.Sign == -1 ? NegativeMark : PositiveMark;
            bytes = new byte[AddOneByte ? 1 : 0].Concat(bytes).ToArray();
            if (bytes.Length <= byte.MaxValue)
            {
                writer.Write((byte) EExtTermType.Big8);
                writer.Write((byte) bytes.Length);
            }
            else
            {
                writer.Write((byte) EExtTermType.Big32);
                writer.Write(bytes.Length);
            }

            writer.Write(sign);
            writer.Write(bytes);
        }

        public override string ToString()
        {
            return Number.ToString();
        }
    }
}