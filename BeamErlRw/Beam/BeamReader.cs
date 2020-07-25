using System;
using System.Collections.Generic;
using System.IO;
using BeamErlRw.Beam.Chunks;
using BeamErlRw.Beam.Exceptions;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam
{
    public static class BeamReader
    {
        public static BeamFile Read(string filePath, BeamReaderOptions options)
        {
            using var stream = File.OpenRead(filePath);
            return Read(stream, options);
        }

        public static BeamFile Read(Stream stream, BeamReaderOptions options)
        {
            using var reader = new EndianBinaryReader(stream);
            return Read(reader, options);
        }

        public static BeamFile Read(EndianBinaryReader reader, BeamReaderOptions options)
        {
            var beamHeader = ReadHeader(reader);
            var chunks = ReadChunks(reader, options);
            return new BeamFile()
            {
                Header = beamHeader,
                Chunks = chunks
            };
        }

        public static BeamHeader ReadHeader(EndianBinaryReader reader) => BeamHeader.Read(reader);

        public static IBeamChunk[] ReadChunks(EndianBinaryReader reader, IBeamReaderOptions options)
        {
            var chunks = new List<IBeamChunk>();
            while (reader.BaseStream.Length > reader.BaseStream.Position)
            {
                chunks.Add(ReadChunk(reader, options));
            }

            return chunks.ToArray();
        }

        public static IBeamChunk ReadChunk(EndianBinaryReader reader, IBeamReaderOptions options)
        {
            var name = reader.ReadString(BeamHeader.IffNameSize, BeamHeader.IffEEncodingType);
            var size = reader.ReadUInt32();
            var oldPos = reader.Position;
            var chunkType = ChunkTypeConverter.GetType(name);
            if (options.ChunkTypesAsRaw.Contains(chunkType))
                chunkType = EChunkType.Raw;

            var chunk = chunkType switch
            {
                EChunkType.Latin1Atoms => options.UseUnifiedAtomsChunk
                    ? (IBeamChunk) BeamAtomsChunk.Read(reader, EEncodingType.Latin1)
                    : (IBeamChunk) BeamLatin1AtomsChunk.Read(reader, size),
                EChunkType.Utf8Atoms => options.UseUnifiedAtomsChunk
                    ? (IBeamChunk) BeamAtomsChunk.Read(reader, EEncodingType.Utf8)
                    : (IBeamChunk) BeamUtf8AtomsChunk.Read(reader, size),
                EChunkType.Exports => BeamExportsChunk.Read(reader, size),
                EChunkType.Code => BeamCodeChunk.Read(reader, size),
                EChunkType.Strings => BeamStringsChunk.Read(reader, size),
                EChunkType.Imports => BeamImportsChunk.Read(reader, size),
                EChunkType.Literals => BeamLiteralsChunk.Read(reader, size),
                EChunkType.LocalFuncs => BeamLocalFuncsChunk.Read(reader, size),
                EChunkType.Attributes => BeamAttributesChunk.Read(reader, size),
                EChunkType.CompilationInfo => BeamCompileInfoChunk.Read(reader, size),
                EChunkType.AbstractCode => BeamAbstractCodeChunk.Read(reader, size),
                EChunkType.Lines => BeamLinesChunk.Read(reader, size),
                EChunkType.Raw => BeamRawChunk.Read(reader, size, name),
                _ => throw new NotImplementedException($"Read {name} not implemented"),
            };
            var newPos = reader.Position;
            if (newPos - oldPos != size)
                throw new ReadBytesCountException((int) (oldPos - newPos), (int) size);

            reader.ReadBytes((int) PaddingCalc.Calculate(size)); //skip padding

            return chunk;
        }
    }
}