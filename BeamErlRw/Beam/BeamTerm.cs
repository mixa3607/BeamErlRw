using System;
using BeamErlRw.Beam.Exceptions;
using BeamErlRw.Beam.Terms;
using BeamErlRw.Beam.Terms.Compact;
using BeamErlRw.Beam.Terms.Ext;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam
{
    public static class BeamTerm
    {
        public const byte TermStartMarker = 131;

        public static void TermToBinary(EndianBinaryWriter writer, IExtTerm term, bool addMarker = true)
        {
            if (addMarker)
                writer.Write(TermStartMarker);

            term.Write(writer);
        }

        public static IExtTerm BinaryToTerm(EndianBinaryReader reader, bool needMarker = true)
        {
            if (needMarker)
            {
                var marker = reader.ReadByte();
                if (marker != TermStartMarker)
                    throw new BadTermMarkerException(marker, TermStartMarker);
            }

            var tag = (EExtTermType)reader.ReadByte();
            IExtTerm value = tag switch
            {
                EExtTermType.List => TermList.Read(reader),
                EExtTermType.Tuple8 => TermTuple.Read(reader, ETermSize.Small),
                EExtTermType.Tuple32 => TermTuple.Read(reader, ETermSize.Large),
                EExtTermType.AtomLatin18 => TermAtom.Read(reader, EEncodingType.Latin1, ETermSize.Small),
                EExtTermType.AtomLatin116 => TermAtom.Read(reader, EEncodingType.Latin1, ETermSize.Large),
                EExtTermType.AtomUtf88 => TermAtom.Read(reader, EEncodingType.Utf8, ETermSize.Small),
                EExtTermType.AtomUtf816 => TermAtom.Read(reader, EEncodingType.Utf8, ETermSize.Large),
                EExtTermType.String => TermString.Read(reader),
                EExtTermType.Nil => TermNil.Read(),
                EExtTermType.FloatString => TermFloatString.Read(reader),
                EExtTermType.Int32 => TermInt32.Read(reader),
                EExtTermType.UInt8 => TermUInt8.Read(reader),
                EExtTermType.Big8 => TermBigNumber.Read(reader, ETermSize.Small),
                EExtTermType.Big32 => TermBigNumber.Read(reader, ETermSize.Large),
                EExtTermType.NewFloat64 => TermNewFloat64.Read(reader),
                EExtTermType.Binary => TermBinary.Read(reader),
                _ => throw new NotImplementedException($"{tag} not supported"),
            };
            return value;
        }

        public static void CompactTermToBinary(EndianBinaryWriter writer, CompactTerm term)
        {
            term.Write(writer);
        }

        public static CompactTerm BinaryToCompactTerm(EndianBinaryReader reader)
        {
            return CompactTerm.Read(reader);
        }
    }
}