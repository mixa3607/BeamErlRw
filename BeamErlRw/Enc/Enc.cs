using System;
using System.Text;

namespace BeamErlRw.Enc
{
    public static class Enc
    {
        private static readonly Encoding AsciiEncoding = Encoding.ASCII;
        private static readonly Encoding Utf8Encoding = Encoding.UTF8;
        private static readonly Encoding Latin1Encoding = Encoding.GetEncoding("iso-8859-1");
        public static Encoding Get(EEncodingType type)
        {
            return type switch
            {
                EEncodingType.Ascii => AsciiEncoding,
                EEncodingType.Latin1 => Latin1Encoding,
                EEncodingType.Utf8 => Utf8Encoding,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}