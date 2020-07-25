using System.Collections.Generic;
using System.Linq;

namespace BeamErlRw.Beam.Chunks
{
    public static class ChunkTypeConverter
    {
        private static readonly Dictionary<string, EChunkType> StrTypes = new Dictionary<string, EChunkType>()
        {
            {"Atom", EChunkType.Latin1Atoms},
            {"AtU8", EChunkType.Utf8Atoms},
            {"Code", EChunkType.Code},
            {"StrT", EChunkType.Strings},
            {"ImpT", EChunkType.Imports},
            {"ExpT", EChunkType.Exports},
            {"LitT", EChunkType.Literals},
            {"LocT", EChunkType.LocalFuncs},
            {"Attr", EChunkType.Attributes},
            {"CInf", EChunkType.CompilationInfo},
            {"Abst", EChunkType.AbstractCode},
            {"Line", EChunkType.Lines},
        };

        public static EChunkType GetType(string strType)
        {
            if (StrTypes.ContainsKey(strType))
            {
                return StrTypes[strType];
            }
            else
            {
                return EChunkType.Raw;
            }
        }

        public static string GetName(EChunkType type)
        {
            return StrTypes.First(x => x.Value == type).Key;
        }
    }
}