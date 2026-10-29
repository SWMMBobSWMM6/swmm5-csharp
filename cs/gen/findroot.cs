// Generated from EPA SWMM 5.2.4 (findroot.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // findroot.c:20
    public static int findroot_Newton(double x1, double x2, ref double rts, double xacc, FP0 func, object p)
    {
        int j = 0;
        int n = 0;
        double df = 0.0;
        double dx = 0.0;
        double dxold = 0.0;
        double f = 0.0;
        double x = 0.0;
        double temp = 0.0;
        double xhi = 0.0;
        double xlo = 0.0;
        x = rts;
        xlo = x1;
        xhi = x2;
        dxold = Math.Abs(x2 - x1);
        dx = dxold;
        func(x, ref f, ref df, p);
        n++;
        for (j = 1; j <= 60; j++)
        {
            if (((((x - xhi) * df - f) * ((x - xlo) * df - f) >= 0.0) || ((Math.Abs(2.0 * f) > Math.Abs(dxold * df)))))
            {
                dxold = dx;
                dx = 0.5 * (xhi - xlo);
                x = xlo + dx;
                if (xlo == x)
                {
                    break;
                }
            }
            else
            {
                dxold = dx;
                dx = f / df;
                temp = x;
                x -= dx;
                if (temp == x)
                {
                    break;
                }
            }
            if (Math.Abs(dx) < xacc)
            {
                break;
            }
            func(x, ref f, ref df, p);
            n++;
            if (f < 0.0)
            {
                xlo = x;
            }
            else
            {
                xhi = x;
            }
        }
        rts = x;
        if (n <= 60)
        {
            return n;
        }
        else
        {
            return 0;
        }
    }

    // findroot.c:91
    public static double findroot_Ridder(double x1, double x2, double xacc, FP1 func, object p)
    {
        int j = 0;
        double ans = 0.0;
        double fhi = 0.0;
        double flo = 0.0;
        double fm = 0.0;
        double fnew = 0.0;
        double s = 0.0;
        double xhi = 0.0;
        double xlo = 0.0;
        double xm = 0.0;
        double xnew = 0.0;
        flo = func(x1, p);
        fhi = func(x2, p);
        if (flo == 0.0)
        {
            return x1;
        }
        if (fhi == 0.0)
        {
            return x2;
        }
        ans = 0.5 * (x1 + x2);
        if ((((flo > 0.0) && (fhi < 0.0))) || (((flo < 0.0) && (fhi > 0.0))))
        {
            xlo = x1;
            xhi = x2;
            for (j = 1; j <= 60; j++)
            {
                xm = 0.5 * (xlo + xhi);
                fm = func(xm, p);
                s = Math.Sqrt(fm * fm - flo * fhi);
                if (s == 0.0)
                {
                    return ans;
                }
                xnew = xm + (xm - xlo) * (((flo >= fhi ? 1.0 : -1.0)) * fm / s);
                if (Math.Abs(xnew - ans) <= xacc)
                {
                    break;
                }
                ans = xnew;
                fnew = func(ans, p);
                if ((((fnew) >= 0.0 ? Math.Abs(fm) : -(Math.Abs(fm)))) != fm)
                {
                    xlo = xm;
                    flo = fm;
                    xhi = ans;
                    fhi = fnew;
                }
                else if ((((fnew) >= 0.0 ? Math.Abs(flo) : -(Math.Abs(flo)))) != flo)
                {
                    xhi = ans;
                    fhi = fnew;
                }
                else if ((((fnew) >= 0.0 ? Math.Abs(fhi) : -(Math.Abs(fhi)))) != fhi)
                {
                    xlo = ans;
                    flo = fnew;
                }
                else
                {
                    return ans;
                }
                if (Math.Abs(xhi - xlo) <= xacc)
                {
                    return ans;
                }
            }
            return ans;
        }
        return -1.0e20;
    }

}
}
