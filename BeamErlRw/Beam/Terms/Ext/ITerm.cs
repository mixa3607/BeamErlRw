using BeamErlRw.Endian;

namespace BeamErlRw.Beam.Terms.Ext
{
    public interface IExtTerm
    {
        EInternalTermType Type { get; }
        void Write(EndianBinaryWriter writer);
    }
}