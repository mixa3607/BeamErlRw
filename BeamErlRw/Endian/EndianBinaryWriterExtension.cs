using System;
using BeamErlRw.Enc;

namespace BeamErlRw.Endian
{
    public static class EndianBinaryWriterExtension
    {
        public static int Write(this EndianBinaryWriter writer,string str, EEncodingType encodingType, bool nullTerminated = false)
        {
            var buffer = Enc.Enc.Get(encodingType).GetBytes(str);
            if (nullTerminated)
            {
                Array.Resize(ref buffer, buffer.Length + 1);
            }
            writer.Write(buffer);
            return buffer.Length;
        }
    }
}