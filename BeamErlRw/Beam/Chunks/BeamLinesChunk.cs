using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using BeamErlRw.Beam.Exceptions;
using BeamErlRw.Beam.Terms.Compact;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Chunks
{
    public class BeamLinesChunk : IBeamChunk
    {
        public const EEncodingType FileNamesEncodingType = EEncodingType.Utf8;

        public static bool ThrowOnBadFileId = false;

        public EChunkType Type { get; } = EChunkType.Lines;
        public uint Version { get; set; }
        public uint Flags { get; set; }
        public uint LineInstructionsCount { get; set; }
        public string[] FileNames { get; set; }
        public Dictionary<BigInteger, List<BigInteger>> FileLineReferences { get; set; } //file_name_idx with line_num[]

        public static BeamLinesChunk Read(EndianBinaryReader reader, uint size)
        {
            var ver = reader.ReadUInt32();
            var flags = reader.ReadUInt32();
            var lineInstrCount = reader.ReadUInt32(); //??
            var numLineRefs = reader.ReadUInt32();
            var numFileNames = reader.ReadUInt32();

            var lineRefs = new Dictionary<BigInteger, List<BigInteger>>(); //file with line[]
            BigInteger fileNameIndex = -1;
            var fileNames = new string[numFileNames];
            for (int i = 0; i < numLineRefs + numFileNames; i++)
            {
                var term = BeamTerm.BinaryToCompactTerm(reader);
                if (term.Type == ECompactTermType.Integer)
                {
                    if (fileNameIndex < 0)
                        if (ThrowOnBadFileId)
                            throw new FileIndexOverException((int)numFileNames, (int)term.Value);
                        //else
                        //    Console.WriteLine("Warn! Bad file id " + fileNameIndex);
                    if (!lineRefs.ContainsKey(fileNameIndex))
                        lineRefs.Add(fileNameIndex, new List<BigInteger>());
                    lineRefs[fileNameIndex].Add(term.Value);
                }
                else if (term.Type == ECompactTermType.Atom)
                {
                    if (term.Value > numFileNames)
                        throw new FileIndexOverException((int)numFileNames, (int)term.Value);
                    fileNameIndex = term.Value;
                }
                else
                {
                    throw new InvalidOperationException($"Expected only {ECompactTermType.Atom} or {ECompactTermType.Integer} but get {term.Type}");
                }
            }

            for (int i = 0; i < numFileNames; i++)
            {
                var strLen = reader.ReadUInt16();
                fileNames[i] = reader.ReadString(strLen, FileNamesEncodingType);
            }

            return new BeamLinesChunk()
            {
                Version = ver,
                Flags = flags,
                FileNames = fileNames,
                FileLineReferences = lineRefs,
                LineInstructionsCount = lineInstrCount
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(Flags);
            writer.Write(LineInstructionsCount);
            writer.Write((uint)FileLineReferences.Sum(x=>x.Value.Count));
            writer.Write((uint)FileNames.LongLength);
            foreach (var (fileIdx, refs)  in FileLineReferences)
            {
                BeamTerm.CompactTermToBinary(writer, new CompactTerm(ECompactTermType.Atom, fileIdx));
                foreach (var lineRef in refs)
                {
                    BeamTerm.CompactTermToBinary(writer, new CompactTerm(ECompactTermType.Integer, lineRef));
                }
            }

            var enc = Enc.Enc.Get(FileNamesEncodingType);
            foreach (var fileName in FileNames)
            {
                var bytes = enc.GetBytes(fileName);
                writer.Write((ushort) bytes.Length);
                writer.Write(bytes);
            }
        }
    }
}