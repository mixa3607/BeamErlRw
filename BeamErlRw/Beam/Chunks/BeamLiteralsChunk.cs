using System;
using System.IO;
using BeamErlRw.Beam.Exceptions;
using BeamErlRw.Beam.Terms.Ext;
using BeamErlRw.Endian;
using BeamErlRw.ZLib;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamLiteralsChunk : IBeamChunk
    {
        public EChunkType Type { get; } = EChunkType.Literals;
        public IExtTerm[] Literals { get; set; }

        public static BeamLiteralsChunk Read(EndianBinaryReader reader, uint size)
        {
            var decSize = reader.ReadUInt32();
            var encBytes = reader.ReadBytes((int)size - 4); //-decSize size

            var decBytes = ZLibCompressor.Decompress(encBytes);
            if (decBytes.Length != decSize)
                throw new ReadBytesCountException(decBytes.Length, (int)decSize);

            var decReader = new EndianBinaryReader(new MemoryStream(decBytes));
            var litCount = decReader.ReadUInt32();
            var literals = new IExtTerm[litCount];
            for (uint i = 0; i < litCount; i++)
            {
                var litSize = decReader.ReadUInt32();
                var oldPos = decReader.Position;
                literals[i] = BeamTerm.BinaryToTerm(decReader);
                var newPos = decReader.Position;
                if (oldPos + litSize != newPos)
                {
                    throw new Exception();
                }
            }
            return new BeamLiteralsChunk()
            {
                Literals = literals
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            var decMemStream = new MemoryStream();
            var decWriter = new EndianBinaryWriter(decMemStream);
            decWriter.Write(Literals.Length);
            foreach (var literal in Literals)
            {
                decWriter.SeekNext(4); //uint size
                var oldPos = decWriter.Position;
                BeamTerm.TermToBinary(decWriter, literal);
                var len = (uint)(decWriter.Position - oldPos);
                decWriter.SeekBack((int)(len + 4));
                decWriter.Write(len);
                decWriter.SeekNext((int)len);
            }

            decMemStream.Position = 0;
            var decBytes = decMemStream.ToArray();
            writer.Write(decBytes.Length);
            var encBytes = ZLibCompressor.Compress(decBytes);
            writer.Write(encBytes);
        }
    }
}