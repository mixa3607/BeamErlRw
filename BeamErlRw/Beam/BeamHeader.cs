using BeamErlRw.Beam.Exceptions;
using BeamErlRw.Enc;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam
{
    public class BeamHeader
    {
        public const int IffNameSize = 4;
        public const int HeaderSize = IffNameSize + 4 + IffNameSize; // BEAM + size uint + FOR1
        public const EEncodingType IffEEncodingType = EEncodingType.Ascii;

        //Interchange File Format
        private string _iffHeader = "BEAM";
        public string IffHeader
        {
            get => _iffHeader;
            set
            {
                if (value.Length != IffNameSize)
                    throw new IffNameSizeException(value.Length);
                _iffHeader = value;
            }
        }

        public uint Size { get; set; }

        private string _formType = "FOR1";
        public string FormType
        {
            get => _formType;
            set
            {
                if (value.Length != IffNameSize)
                    throw new IffNameSizeException(value.Length);
                _formType = value;
            }
        }

        public static BeamHeader Read(EndianBinaryReader reader)
        {
            var beamHeader = new BeamHeader
            {
                IffHeader = reader.ReadString(IffNameSize, IffEEncodingType),
                Size = reader.ReadUInt32(),
                FormType = reader.ReadString(IffNameSize, IffEEncodingType)
            };
            //if (reader.BaseStream.Length - 4 - 4 != beamHeader.Size && ValidateLen)
            //{
            //    throw new FormatException(
            //        $"Len readed from header is {beamHeader.Size} but stream len (-8) is {reader.BaseStream.Length - 8}");
            //}
            //else if (reader.BaseStream.Length - 4 - 4 != beamHeader.Size)
            //{
            //    Console.WriteLine(
            //        $"Len readed from header is {beamHeader.Size} but stream len (-8) is {reader.BaseStream.Length - 8}");
            //}
            return beamHeader;
        }

        public void Write(EndianBinaryWriter writer)
        {
            writer.Write(IffHeader, IffEEncodingType);
            writer.Write(Size);
            writer.Write(FormType, IffEEncodingType);
        }

        public override string ToString()
        {
            return $"Header: {IffHeader}\nSize: {Size} bytes\nFrom: {FormType}";
        }
    }
}