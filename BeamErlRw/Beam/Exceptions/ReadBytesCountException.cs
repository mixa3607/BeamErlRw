using System;

namespace BeamErlRw.Beam.Exceptions
{
    public class ReadBytesCountException : Exception
    {
        public int ExpectedRead { get; set; }
        public int ActualRead { get; set; }

        public ReadBytesCountException(int actualRead, int expectedRead, string message) : base(message)
        {
            ActualRead = actualRead;
            ExpectedRead = expectedRead;
        }

        public ReadBytesCountException(int actualRead, int expectedRead)
            : this(actualRead, expectedRead, $"Read {actualRead} bytes but expected {expectedRead}")
        { }
    }
}