using System;

namespace BeamErlRw.Beam.Exceptions
{
    public class FileIndexOverException : Exception
    {
        public int FilesCount { get; set; }
        public int WrongIndex { get; set; }

        public FileIndexOverException(int filesCount, int wrongIdx, string message) : base(message)
        {
            FilesCount = filesCount;
            WrongIndex = wrongIdx;
        }

        public FileIndexOverException(int filesCount, int wrongIdx)
            : this(filesCount, wrongIdx, $"Read index {wrongIdx} out of files count {filesCount}")
        { }
    }
}