using System;

namespace BeamErlRw.Beam.Exceptions
{
    public class IffNameSizeException : Exception
    {
        public int ActualSize { get; set; }

        public IffNameSizeException(int actualSize, string message) : base(message)
        {
            ActualSize = actualSize;
        }

        public IffNameSizeException(int actualSize) 
            : this(actualSize,$"Expected iff name size is {BeamHeader.IffNameSize} but read {actualSize} bytes")
        { }
    }
}