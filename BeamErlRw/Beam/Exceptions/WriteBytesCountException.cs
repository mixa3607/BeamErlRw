using System;

namespace BeamErlRw.Beam.Exceptions
{
    public class WriteBytesCountException : Exception
    {
        public int ExpectedWrite { get; set; }
        public int ActualWrite { get; set; }

        public WriteBytesCountException(int actualWrite, int expectedWrite, string message) : base(message)
        {
            ActualWrite = actualWrite;
            ExpectedWrite = expectedWrite;
        }

        public WriteBytesCountException(int actualWrite, int expectedWrite)
            : this(actualWrite, expectedWrite, $"Read {actualWrite} bytes but expected maximum {expectedWrite}")
        { }
    }
}