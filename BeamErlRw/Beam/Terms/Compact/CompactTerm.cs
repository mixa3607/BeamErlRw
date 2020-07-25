using System;
using System.Numerics;
using BeamErlRw.Beam.Chunks;
using BeamErlRw.Beam.Exceptions;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Compact
{
    public class CompactTerm : ICompactTerm
    {
        public ECompactTermType Type { get; set; }
        public BigInteger Value { get; set; }

        public CompactTerm()
        {
        }

        public CompactTerm(ECompactTermType type, BigInteger value)
        {
            Type = type;
            Value = value;
        }

        public static CompactTerm Read(EndianBinaryReader reader)
        {
            var (type, bytes) = ReadTypeAndBytes(reader);
            if (type > ECompactTermType.Extended)
                //Tag=Extended List: contains pairs of terms. Read smallint Size.
                //Create tuple of Size, which will contain Size/2 values.
                //For Size/2 do: read and parse a term (case of value),
                //read a small int (label index), place them into the tuple.
                throw new NotImplementedException(
                    $"Extended compact types not implemented and not planning, use {nameof(BeamRawChunk)} instead");
            var value = type switch
            {
                //Tag=Integer: use the value (signed?)
                ECompactTermType.Integer => new BigInteger(bytes, false, true),
                //Tag=Literal: use smallint value as index in LitT table.
                ECompactTermType.Literal => new BigInteger(bytes, true, true),
                //Tag=Atom: use smallint value MINUS 1 as index in the atom table. 0 smallint means NIL [].
                ECompactTermType.Atom => new BigInteger(bytes, true, true) - 1,
                //Tag=Label: use as label index, or 0 means invalid value.
                ECompactTermType.Label => new BigInteger(bytes, true, true),
                //Tag=XRegister: use as register index.
                ECompactTermType.XRegister => new BigInteger(bytes, true, true),
                //Tag=YRegister: use as register index.
                ECompactTermType.YRegister => new BigInteger(bytes, true, true),
                //Tag=Character (an Unicode symbol): use val as unsigned.
                ECompactTermType.Char => new BigInteger(bytes, true, true),
                ECompactTermType.Extended => throw new NotImplementedException(),
                ECompactTermType.ExtFloat => throw new NotImplementedException(),
                ECompactTermType.ExtList => throw new NotImplementedException(),
                ECompactTermType.ExtFpRegister => throw new NotImplementedException(),
                ECompactTermType.ExtAllocationList => throw new NotImplementedException(),
                ECompactTermType.ExtLiteral => throw new NotImplementedException(),
            };
            return new CompactTerm()
            {
                Type = type,
                Value = value
            };
        }

        private static (ECompactTermType type, byte[] value) ReadTypeAndBytes(EndianBinaryReader reader)
        {
            var head = reader.ReadByte();
            var type = (ECompactTermType) (head & 0b0000_0111);
            byte[] value;

            //read standard types
            if (type != ECompactTermType.Extended)
            {
                //  7 6 5 4 | 3 | 2 1 0
                // ---------+---+-------
                // Value >> | 0 | Tag >>
                if ((head & 0b0000_1_000) == 0b0000_0_000)
                {
                    value = new[] {(byte) (head >> 4)};
                }
                // 7 6 5 | 4 3 | 2 1 0
                // ------+-----+-------
                // Value | 0 1 | Tag >> +1b_val
                else if ((head & 0b000_11_000) == 0b000_01_000)
                {
                    value = new byte[2];
                    value[0] = (byte) (head >> 5);
                    value[1] = reader.ReadByte();
                }
                // 7 6 5 4 3 | 2 1 0
                // ----------+------ Followed by nested encoded int(Size - 9)
                // 1 1 1 1 1 | Tag >>
                else if ((head & 0b11111_000) == 0b11111_000)
                {
                    var valLen = reader.ReadByte() + 9;
                    value = reader.ReadBytes(valLen);
                }
                //     7 6 5 | 4 3 | 2 1 0
                // ----------+-----+------
                // Bytes - 2 | 1 1 | Tag >>
                else if ((head & 0b000_11_000) == 0b000_11_000)
                {
                    var valLen = ((head & 0b111_00000) >> 5) + 2;
                    value = reader.ReadBytes(valLen);
                }
                else
                {
                    throw new Exception("I dont know how you get this...");
                }
            }
            else
            {
                type += (byte) ECompactTermType.Extended;
                throw new NotImplementedException(
                    $"Extended compact types not implemented and not planning, use {nameof(BeamRawChunk)} instead");
            }

            return (type, value);
        }

        public void Write(EndianBinaryWriter writer)
        {
            if (Type == ECompactTermType.Extended)
                throw new NotImplementedException(
                    $"Extended compact types not implemented and not planning, use {nameof(BeamRawChunk)} instead");
            var valBytes = Type switch
            {
                ECompactTermType.Integer => Value.ToByteArray(false, true),
                ECompactTermType.Literal => Value.ToByteArray(true, true),
                ECompactTermType.Atom => (Value + 1).ToByteArray(true, true),
                ECompactTermType.Label => Value.ToByteArray(true, true),
                ECompactTermType.XRegister => Value.ToByteArray(true, true),
                ECompactTermType.YRegister => Value.ToByteArray(true, true),
                ECompactTermType.Char => Value.ToByteArray(true, true),
                _ => throw new ArgumentOutOfRangeException()
            };

            byte[] bytes;
            var head = (byte) Type;
            if (valBytes.Length == 1 && valBytes[0] <= 0b1111)
            {
                head |= (byte)(0b0000_0_000 | (valBytes[0] << 4));
                bytes = new byte[0];
            }
            else if (valBytes.Length == 1)
            {
                head |= 0b000_01_000;
                bytes = new[] { valBytes[0] };
            }
            else if (valBytes.Length == 2 && valBytes[0] <= 0b111)
            {
                head |= (byte)(0b000_01_000 | (valBytes[0] << 5));
                bytes = new[] { valBytes[1] };
            }
            else if (valBytes.Length <= 8)
            {
                head |= (byte)(0b000_11_000 | ((valBytes.Length - 2) << 5));
                bytes = valBytes;
            }
            else if (valBytes.Length <= byte.MaxValue + 9)
            {
                head |= 0b11111_000;
                bytes = new byte[1 + valBytes.Length];
                bytes[0] = (byte) (valBytes.Length - 9);
                Array.Copy(valBytes, 0, bytes, 1, valBytes.Length);
            }
            else
            {
                throw new WriteBytesCountException(valBytes.Length, byte.MaxValue + 9);
            }

            writer.Write(head);
            writer.Write(bytes);
        }
    }
}