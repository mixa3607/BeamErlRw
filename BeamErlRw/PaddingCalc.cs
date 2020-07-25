namespace BeamErlRw
{
    public class PaddingCalc
    {
        public static long Calculate(long paddingFrom, int paddingBy = 4)
        {
            var padding = 4 - paddingFrom % 4;
            padding = padding == 4 ? 0 : padding;
            return padding;
        }

        public static int Calculate(int paddingFrom, int paddingBy = 4)
        {
            return (int)Calculate((long)paddingBy, paddingFrom);
        }
    }
}