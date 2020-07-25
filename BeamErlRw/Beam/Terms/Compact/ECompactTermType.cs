namespace BeamErlRw.Beam.Terms.Compact
{
    public enum ECompactTermType : byte
    {
        Literal = 0b000,    //-define(tag_u, 0).
        Integer = 0b001,    //-define(tag_i, 1).
        Atom = 0b010,       //-define(tag_a, 2).
        XRegister = 0b011,  //-define(tag_x, 3).
        YRegister = 0b100,  //-define(tag_y, 4).
        Label = 0b101,      //-define(tag_f, 5).
        Char = 0b110,       //-define(tag_h, 6).
        Extended = 0b111,   //-define(tag_z, 7). extended mark
        ExtFloat = 0b00010_111,
        ExtList = 0b00100_111,
        ExtFpRegister = 0b00110_111,
        ExtAllocationList = 0b01000_111,
        ExtLiteral = 0b01010_111,
    }
}