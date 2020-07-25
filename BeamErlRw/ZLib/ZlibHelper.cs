using System.IO;
using Ionic.Zlib;

namespace BeamErlRw.ZLib
{
    public static class ZLibCompressor
    {
        public static byte[] Decompress(byte[] compressed)
        {
            using var ms = new MemoryStream();
            using var compressor = new ZlibStream(ms, CompressionMode.Decompress);
            compressor.Write(compressed, 0, compressed.Length);

            return ms.ToArray();
        }

        public static byte[] Compress(byte[] uncompressed)
        {
            using var ms = new MemoryStream();
            using var compressor = new ZlibStream(ms, CompressionMode.Compress, CompressionLevel.Default);
            compressor.Write(uncompressed, 0, uncompressed.Length);

            return ms.ToArray();
        }
    }
}