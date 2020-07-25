using System;

namespace BeamErlRw.Beam.Exceptions
{
    public class BadTermMarkerException : Exception
    {
        public byte ReadMarker { get; set; }
        public byte ExpectedMarker { get; set; }

        public BadTermMarkerException(byte readMarker, byte expectedMarker, string message) : base(message)
        {
            ReadMarker = readMarker;
            ExpectedMarker = expectedMarker;
        }

        public BadTermMarkerException(byte readMarker, byte expectedMarker)
            : this(readMarker, expectedMarker, $"Expected term marker is {expectedMarker} but read {readMarker}")
        { }
    }
}