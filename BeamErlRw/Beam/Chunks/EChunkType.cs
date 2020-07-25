namespace BeamErlRw.Beam.Chunks
{
    public enum EChunkType
    {
        Latin1Atoms,
        Utf8Atoms,
        Code,
        Strings,
        Imports,
        Exports,
        Literals,
        LocalFuncs,
        Attributes,
        CompilationInfo,
        AbstractCode,
        Lines,

        // no representation in beam, used only in code
        Atoms,
        Raw,
    }
}