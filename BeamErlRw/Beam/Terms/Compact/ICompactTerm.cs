using System.Numerics;
using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Compact
{
    public interface ICompactTerm
    {
        ECompactTermType Type { get; set; }
        BigInteger Value { get; set; }

        void Write(EndianBinaryWriter writer);
    }
}