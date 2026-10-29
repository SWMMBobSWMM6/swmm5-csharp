// runswmm for the C# port: same command line and console messages as EPA's run/main.c.
// Written by Claude (Anthropic); see AI-USE.md.
using System;
namespace Swmm5
{
    public static class Program
    {
        static void P(string s) { var b = System.Text.Encoding.ASCII.GetBytes(s); S.stdout.S.Write(b, 0, b.Length); S.stdout.S.Flush(); }
        public static int Main(string[] argv)
        {
            int version = S.swmm_getVersion();
            int vMajor = version / 10000;
            int vMinor = (version - 10000 * vMajor) / 1000;
            int vRelease = (version - 10000 * vMajor - 1000 * vMinor);
            long start = R.time();
            if (argv.Length == 0) P("\nNot Enough Arguments (See Help --help)\n\n");
            else if (argv.Length == 1)
            {
                string a = argv[0];
                if (a == "--help" || a == "-h")
                    P("\n\nSTORMWATER MANAGEMENT MODEL (SWMM) HELP\n\nCOMMANDS:\n\t--help (-h)       SWMM Help\n\t--version (-v)    Build Version\n\nRUNNING A SIMULATION:\n\t runswmm <input file> <report file> <optional output file>\n\n");
                else if (a == "--version" || a == "-v") P(string.Format("\n{0}.{1}.{2}\n\n", vMajor, vMinor, vRelease));
                else P("\nUnknown Argument (See Help --help)\n\n");
            }
            else
            {
                CPtr inp = R.Lit(argv[0]), rpt = R.Lit(argv[1]), outf = R.Lit(argv.Length > 2 ? argv[2] : "");
                P(string.Format("\n... EPA SWMM {0}.{1} (Build {0}.{1}.{2}) [C# port]\n", vMajor, vMinor, vRelease));
                S.swmm_run(inp, rpt, outf);
                double runTime = R.time() - start;
                P("\n\n... EPA SWMM completed in " + runTime.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + " seconds.");
                CPtr err = R.Buf(128);
                if (S.swmm_getError(err, 127) > 0) P(" There are errors.\n");
                else if (S.swmm_getWarnings() > 0) P(" There are warnings.\n");
                else P("\n");
            }
            S.stdout.S.Flush();
            return 0;
        }
    }
}
