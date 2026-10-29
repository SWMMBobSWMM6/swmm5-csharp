// Hand-ported pieces of EPA SWMM 5.2.4 that use C pointer arithmetic the translator does not map:
// odesolve.c (whole file), mempool.c (whole file), and six helpers from swmm5.c / project.c.
// Each keeps the C statement order and floating-point expressions exactly.
// Written by Claude (Anthropic); see AI-USE.md.
using System;
using System.IO;
using System.Collections.Generic;

namespace Swmm5
{
    public delegate void OdeDerivs(double x, double[] y, double[] dydx);

    public static partial class S
    {
        public static CFile stdout = R.stdout_;

        // ------------------------------------------------------------ swmm5.c helpers
        public static ulong sstrncpy(CPtr dest, CPtr src, ulong n)
        {
            int offset = 0;
            if (n > 0)
            {
                while (src[offset] != 0)
                {
                    if ((ulong)offset == n) break;
                    dest[offset] = src[offset];
                    offset++;
                }
            }
            dest[offset] = 0;
            return R.strlen(dest);
        }

        public static ulong sstrcat(CPtr dest, CPtr src, ulong size)
        {
            ulong dest_len = R.strlen(dest);
            ulong offset = dest_len;
            int src_index = 0;
            while (src[src_index] != 0)
            {
                dest[(int)offset] = src[src_index];
                offset++;
                src_index++;
                if (offset == size - 1) break;
            }
            dest[(int)offset] = 0;
            return R.strlen(dest);
        }

        public static void getAbsolutePath(CPtr fname, CPtr absPath, ulong size)
        {
            if (fname.IsNull || R.strlen(fname) == 0) return;
            if (isRelativePath(fname) != 0)
            {
                // realpath(): resolve against the current directory (symlinks resolved like glibc)
                string full;
                try
                {
                    full = Path.GetFullPath(R.Str(fname));
                    var fi = new FileInfo(full);
                    if (fi.Exists && fi.LinkTarget != null) full = fi.ResolveLinkTarget(true)?.FullName ?? full;
                }
                catch (Exception) { return; }
                var bytes = System.Text.Encoding.UTF8.GetBytes(full);
                for (int i = 0; i < bytes.Length && i < absPath.B.Length - absPath.O - 1; i++) absPath[i] = bytes[i];
                absPath[Math.Min(bytes.Length, absPath.B.Length - absPath.O - 1)] = 0;
            }
            else
            {
                sstrncpy(absPath, fname, R.strlen(fname));
            }
            CPtr endOfDir = R.strrchr(absPath, '/');
            if (!endOfDir.IsNull) endOfDir[1] = 0;
        }

        public static CPtr getTempFileName(CPtr fname)
        {
            sstrncpy(fname, R.Lit("swmmXXXXXX"), MAXFNAME);
            R.mkstemp(fname);
            return fname;
        }

        // ------------------------------------------------------------ project.c matrix helpers
        public static double[][] project_createMatrix(int nrows, int ncols)
        {
            if (nrows <= 0 || ncols <= 0) return null;
            var a = new double[nrows][];
            for (int i = 0; i < nrows; i++) a[i] = new double[ncols];
            return a;
        }
        public static void project_freeMatrix(double[][] a) { }

        // ------------------------------------------------------------ mempool.c
        public static object AllocInit() => new object();
        public static CPtr Alloc(long n) => R.Buf((int)n);
        public static object AllocSetPool(object p) => p;
        public static void AllocReset() { }
        public static void AllocFreePool() { }

        // ------------------------------------------------------------ odesolve.c (Cash-Karp Runge-Kutta)

        const double ODE_TINY = 1.0e-30;
        static int nmax;
        static double[] oy, yscal, yerr, ytemp, dydx, ak2, ak3, ak4, ak5, ak6;

        public static int odesolve_open(int n)
        {
            nmax = 0;
            oy = new double[n]; yscal = new double[n]; dydx = new double[n]; yerr = new double[n]; ytemp = new double[n];
            ak2 = new double[n]; ak3 = new double[n]; ak4 = new double[n]; ak5 = new double[n]; ak6 = new double[n];
            nmax = n;
            return 1;
        }
        public static void odesolve_close()
        {
            oy = yscal = dydx = yerr = ytemp = ak2 = ak3 = ak4 = ak5 = ak6 = null;
            nmax = 0;
        }
        public static int odesolve_integrate(double[] ystart, int n, double x1, double x2, double eps, double h1, OdeDerivs derivs)
        {
            int i, errcode, nstp;
            double hdid = 0, hnext = 0;
            double x = x1;
            double h = h1;
            if (nmax < n) return 1;
            for (i = 0; i < n; i++) oy[i] = ystart[i];
            for (nstp = 1; nstp <= MAXSTP; nstp++)
            {
                derivs(x, oy, dydx);
                for (i = 0; i < n; i++) yscal[i] = Math.Abs(oy[i]) + Math.Abs(dydx[i] * h) + ODE_TINY;
                if ((x + h - x2) * (x + h - x1) > 0.0) h = x2 - x;
                errcode = rkqs(ref x, n, h, eps, ref hdid, ref hnext, derivs);
                if (errcode != 0) break;
                if ((x - x2) * (x2 - x1) >= 0.0)
                {
                    for (i = 0; i < n; i++) ystart[i] = oy[i];
                    return 0;
                }
                if (Math.Abs(hnext) <= 0.0) return 2;
                h = hnext;
            }
            return 3;
        }
        static int rkqs(ref double x, int n, double htry, double eps, ref double hdid, ref double hnext, OdeDerivs derivs)
        {
            int i;
            double err, errmax, h, htemp, xnew, xold = x;
            h = htry;
            for (; ; )
            {
                rkck(xold, n, h, derivs);
                errmax = 0.0;
                for (i = 0; i < n; i++)
                {
                    err = Math.Abs(yerr[i] / yscal[i]);
                    if (err > errmax) errmax = err;
                }
                errmax /= eps;
                if (errmax > 1.0)
                {
                    htemp = SAFETY * h * Math.Pow(errmax, PSHRNK);
                    if (h >= 0)
                    {
                        if (htemp > 0.1 * h) h = htemp;
                        else h = 0.1 * h;
                    }
                    else
                    {
                        if (htemp < 0.1 * h) h = htemp;
                        else h = 0.1 * h;
                    }
                    xnew = xold + h;
                    if (xnew == xold) return 2;
                    continue;
                }
                else
                {
                    if (errmax > ERRCON) hnext = SAFETY * h * Math.Pow(errmax, PGROW);
                    else hnext = 5.0 * h;
                    x += (hdid = h);
                    for (i = 0; i < n; i++) oy[i] = ytemp[i];
                    return 0;
                }
            }
        }
        static void rkck(double x, int n, double h, OdeDerivs derivs)
        {
            double a2 = 0.2, a3 = 0.3, a4 = 0.6, a5 = 1.0, a6 = 0.875,
                   b21 = 0.2, b31 = 3.0 / 40.0, b32 = 9.0 / 40.0, b41 = 0.3, b42 = -0.9, b43 = 1.2,
                   b51 = -11.0 / 54.0, b52 = 2.5, b53 = -70.0 / 27.0, b54 = 35.0 / 27.0,
                   b61 = 1631.0 / 55296.0, b62 = 175.0 / 512.0, b63 = 575.0 / 13824.0,
                   b64 = 44275.0 / 110592.0, b65 = 253.0 / 4096.0, c1 = 37.0 / 378.0,
                   c3 = 250.0 / 621.0, c4 = 125.0 / 594.0, c6 = 512.0 / 1771.0,
                   dc5 = -277.0 / 14336.0;
            double dc1 = c1 - 2825.0 / 27648.0, dc3 = c3 - 18575.0 / 48384.0,
                   dc4 = c4 - 13525.0 / 55296.0, dc6 = c6 - 0.25;
            int i;
            for (i = 0; i < n; i++) ytemp[i] = oy[i] + b21 * h * dydx[i];
            derivs(x + a2 * h, ytemp, ak2);
            for (i = 0; i < n; i++) ytemp[i] = oy[i] + h * (b31 * dydx[i] + b32 * ak2[i]);
            derivs(x + a3 * h, ytemp, ak3);
            for (i = 0; i < n; i++) ytemp[i] = oy[i] + h * (b41 * dydx[i] + b42 * ak2[i] + b43 * ak3[i]);
            derivs(x + a4 * h, ytemp, ak4);
            for (i = 0; i < n; i++) ytemp[i] = oy[i] + h * (b51 * dydx[i] + b52 * ak2[i] + b53 * ak3[i] + b54 * ak4[i]);
            derivs(x + a5 * h, ytemp, ak5);
            for (i = 0; i < n; i++) ytemp[i] = oy[i] + h * (b61 * dydx[i] + b62 * ak2[i] + b63 * ak3[i] + b64 * ak4[i] + b65 * ak5[i]);
            derivs(x + a6 * h, ytemp, ak6);
            for (i = 0; i < n; i++) ytemp[i] = oy[i] + h * (c1 * dydx[i] + c3 * ak3[i] + c4 * ak4[i] + c6 * ak6[i]);
            for (i = 0; i < n; i++) yerr[i] = h * (dc1 * dydx[i] + dc3 * ak3[i] + dc4 * ak4[i] + dc5 * ak5[i] + dc6 * ak6[i]);
        }
    }

    public static partial class R
    {
        /// <summary>C passes &scalar where an array is expected: run f on a one-element array, copy back.</summary>
        public static T Arr1<T>(ref double v, Func<double[], T> f)
        {
            var a = new double[] { v };
            T r = f(a);
            v = a[0];
            return r;
        }
    }
}

namespace Swmm5
{
    public static partial class R
    {
        /// <summary>C passes &charVar where a char* is expected: run f on a 2-byte buffer, copy the char back.</summary>
        public static T Chr1<T>(ref int c, Func<CPtr, T> f)
        {
            var p = Buf(2);
            p[0] = c;
            T r = f(p);
            c = p[0];
            return r;
        }
    }
}

namespace Swmm5
{
    public static partial class R
    {
        /// <summary>fwrite(&wide, n, 1, f) with n smaller than the variable: write its low n bytes (little-endian)</summary>
        public static ulong fwriteN1(long v, int n, CFile f) { var b = BitConverter.GetBytes(v); f.S.Write(b, 0, n); return 1; }
        /// <summary>fread into the low n bytes of a wider variable, upper bytes unchanged (as C does)</summary>
        public static ulong freadN1(ref long v, int n, CFile f)
        {
            var b = BitConverter.GetBytes(v);
            var t = new byte[n];
            int got = f.S.Read(t, 0, n);
            if (got < n) { f.Eof = true; return 0; }
            Array.Copy(t, b, n);
            v = BitConverter.ToInt64(b, 0);
            return 1;
        }
    }
}

namespace Swmm5
{
    public sealed class ScanSlot { }
    public static partial class R
    {
        /// <summary>sscanf target that is a C pointer parameter (ref in C#): the value is stashed, then R.Back copies it.</summary>
        public static readonly ScanSlot Slot = new ScanSlot();
        [ThreadStatic] static List<object> stash;
        internal static void StashReset() { (stash ??= new List<object>()).Clear(); }
        internal static void Stash(object v) { stash.Add(v); }
        static void Take<T>(int i, ref T a) { if (stash != null && i < stash.Count && stash[i] != null) a = (T)Convert.ChangeType(stash[i], typeof(T)); }
        public static int Back<T1>(int n, ref T1 a) { Take(0, ref a); return n; }
        public static int Back<T1, T2>(int n, ref T1 a, ref T2 b) { Take(0, ref a); Take(1, ref b); return n; }
        public static int Back<T1, T2, T3>(int n, ref T1 a, ref T2 b, ref T3 c) { Take(0, ref a); Take(1, ref b); Take(2, ref c); return n; }
        public static int Back<T1, T2, T3, T4>(int n, ref T1 a, ref T2 b, ref T3 c, ref T4 d) { Take(0, ref a); Take(1, ref b); Take(2, ref c); Take(3, ref d); return n; }
        public static int Back<T1, T2, T3, T4, T5>(int n, ref T1 a, ref T2 b, ref T3 c, ref T4 d, ref T5 e) { Take(0, ref a); Take(1, ref b); Take(2, ref c); Take(3, ref d); Take(4, ref e); return n; }
        public static int Back<T1, T2, T3, T4, T5, T6>(int n, ref T1 a, ref T2 b, ref T3 c, ref T4 d, ref T5 e, ref T6 f) { Take(0, ref a); Take(1, ref b); Take(2, ref c); Take(3, ref d); Take(4, ref e); Take(5, ref f); return n; }
    }
}
