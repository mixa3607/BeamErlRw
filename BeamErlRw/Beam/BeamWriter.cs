using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using BeamErlRw.Beam.Chunks;
using BeamErlRw.Beam.Exceptions;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam
{
    public static class BeamWriter
    {
        public static void Write(string filePath, BeamFile file)
        {
            using var stream = File.Open(filePath, FileMode.Create);
            Write(stream, file);
        }

        public static void Write(Stream stream, BeamFile file)
        {
            using var writer = new EndianBinaryWriter(stream);
            Write(writer, file);
        }

        public static void Write(EndianBinaryWriter writer, BeamFile file)
        {
            writer.SeekNext(BeamHeader.HeaderSize);
            var oldPos = writer.Position;
            WriteChunks(file.Chunks, writer);

            var len = writer.Position - oldPos;
            writer.SeekBack((int) len + BeamHeader.HeaderSize);
            file.Header.Size = (uint) len + BeamHeader.IffNameSize;
            WriteHeader(file.Header, writer);
            writer.SeekNext((int) len);
        }

        public static void WriteHeader(BeamHeader header, EndianBinaryWriter writer)
        {
            header.Write(writer);
        }

        public static void WriteChunks(IEnumerable<IBeamChunk> chunks, EndianBinaryWriter writer)
        {
            foreach (var beamChunk in chunks)
            {
                WriteChunk(beamChunk, writer);
            }
        }

        public static void WriteChunk(IBeamChunk chunk, EndianBinaryWriter writer)
        {
            var chunkName = chunk.Type switch
            {
                EChunkType.Raw => ((BeamRawChunk) chunk).Name,
                EChunkType.Atoms => ChunkTypeConverter.GetName(((BeamAtomsChunk) chunk).EncodingType switch
                {
                    EEncodingType.Latin1 => EChunkType.Latin1Atoms,
                    EEncodingType.Utf8 => EChunkType.Utf8Atoms,
                    _ => throw new ArgumentOutOfRangeException(
                        $"Support only {EEncodingType.Latin1} or {EEncodingType.Utf8} but get {((BeamAtomsChunk) chunk).EncodingType}")
                }),
                _ => ChunkTypeConverter.GetName(chunk.Type)
            };
            if (chunkName.Length != BeamHeader.IffNameSize)
                throw new IffNameSizeException(chunkName.Length);

            writer.Write(chunkName, BeamHeader.IffEEncodingType); //4byte name
            writer.SeekNext(sizeof(uint)); //4 byte size skip

            var oldPos = writer.Position;
            chunk.Write(writer); //write chunk
            var len = writer.Position - oldPos;
            writer.SeekBack((int) len + sizeof(uint)); //roll back to size bytes
            writer.Write((uint) len);
            writer.SeekNext((int) len); //skip already written chunk
            writer.Write(new byte[PaddingCalc.Calculate(len)]); //padding to mul of 4
        }
    }
}