using System;
using System.IO;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public class TermAtom : IExtTerm
    {
        public EInternalTermType Type { get; } = EInternalTermType.Atom;
        public EEncodingType EncodingType { get; set; } = EEncodingType.Latin1;
        public string Atom { get; set; }

        public static TermAtom Read(BinaryReader reader, EEncodingType encType, ETermSize size)
        {
            var enc = Enc.Enc.Get(encType);
            var len = size switch
            {
                ETermSize.Small => reader.ReadByte(),
                ETermSize.Large => reader.ReadUInt16(),
                _ => throw new NotSupportedException(),
            };
            var atomBytes = reader.ReadBytes(len);
            var atom = enc.GetString(atomBytes);
            return new TermAtom()
            {
                EncodingType = encType,
                Atom = atom
            };
        }

        public void Write(EndianBinaryWriter writer)
        {
            Write(writer, false);
        }

        public void Write(EndianBinaryWriter writer, bool enableAtom8)
        {
            var enc = Enc.Enc.Get(EncodingType);
            var bytes = enc.GetBytes(Atom);
            if (enableAtom8 && bytes.Length <= byte.MaxValue)
            {
                writer.Write((byte)(EncodingType switch
                {
                    EEncodingType.Latin1 => EExtTermType.AtomLatin18,
                    EEncodingType.Utf8 => EExtTermType.AtomUtf88,
                    _ => throw new ArgumentOutOfRangeException()
                }));
                writer.Write((byte)bytes.Length);
            }
            else if (bytes.Length <= ushort.MaxValue)
            {
                writer.Write((byte)(EncodingType switch
                {
                    EEncodingType.Latin1 => EExtTermType.AtomLatin116,
                    EEncodingType.Utf8 => EExtTermType.AtomUtf816,
                    _ => throw new ArgumentOutOfRangeException()
                }));
                writer.Write((ushort)bytes.Length);
            }
            else
            {
                throw new Exception();
            }
            writer.Write(bytes);
        }


        public override string ToString()
        {
            return Atom;
        }
    }
}