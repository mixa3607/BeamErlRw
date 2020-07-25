using System;
using System.IO;

namespace BeamErlRw.Endian
{
    public class EndianBinaryWriter : BinaryWriter
    {
        public EEndianType Endian { get; set; }

        public EndianBinaryWriter(Stream stream, EEndianType endian = EEndianType.BigEndian) : base(stream)
        {
            Endian = endian;
        }

        public long Position
        {
            get => BaseStream.Position;
            set => BaseStream.Position = value;
        }

        public void SeekBack(int offset)
        {
            base.Seek(offset * -1, SeekOrigin.Current);
        }
        public void SeekNext(int offset)
        {
            base.Seek(offset, SeekOrigin.Current);
        }
        public override void Write(int num)
        {
            var buffer = BitConverter.GetBytes(num);
            if (Endian == EEndianType.BigEndian)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        public override void Write(uint num)
        {
            var buffer = BitConverter.GetBytes(num);
            if (Endian == EEndianType.BigEndian)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        public override void Write(long num)
        {
            var buffer = BitConverter.GetBytes(num);
            if (Endian == EEndianType.BigEndian)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        public override void Write(ulong num)
        {
            var buffer = BitConverter.GetBytes(num);
            if (Endian == EEndianType.BigEndian)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        public override void Write(short num)
        {
            var buffer = BitConverter.GetBytes(num);
            if (Endian == EEndianType.BigEndian)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        public override void Write(ushort num)
        {
            var buffer = BitConverter.GetBytes(num);
            if (Endian == EEndianType.BigEndian)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        public override void Write(float num)
        {
            var buffer = BitConverter.GetBytes(num);
            if (Endian == EEndianType.BigEndian)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        public override void Write(double num)
        {
            var buffer = BitConverter.GetBytes(num);
            if (Endian == EEndianType.BigEndian)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        public int Write(string str, bool nullTerminated = true, string encoding = "UTF-8")
        {
            var buffer = System.Text.Encoding.GetEncoding(encoding).GetBytes(str);
            if (nullTerminated)
            {
                Array.Resize(ref buffer, buffer.Length + 1);
            }
            base.Write(buffer);
            return buffer.Length;
        }
    }
}