// Generated from EPA SWMM 5.2.4 (forcmain.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double VISCOS = 1.1E-5;

    // forcmain.c:37
    public static double forcemain_getEquivN(int j, int k)
    {
        TXsect xsect = S.Link[j].xsect.Clone();
        double f = 0.0;
        double d = xsect.yFull;
        switch (S.ForceMainEqn)
        {
            case S.H_W:
                return 1.067 / xsect.rBot * Math.Pow(d / S.Conduit[k].slope, 0.04);
            case S.D_W:
                f = S.forcemain_getFricFactor(xsect.rBot, d / 4.0, 1.0e12);
                return Math.Sqrt(f / 185.0) * Math.Pow(d, (1.0 / 6.0));
        }
        return S.Conduit[k].roughness;
    }

    // forcmain.c:64
    public static double forcemain_getRoughFactor(int j, double lengthFactor)
    {
        TXsect xsect = S.Link[j].xsect.Clone();
        double r = 0.0;
        switch (S.ForceMainEqn)
        {
            case S.H_W:
                r = 1.318 * xsect.rBot * Math.Pow(lengthFactor, 0.54);
                return 32.2 / Math.Pow(r, 1.852);
            case S.D_W:
                return 1.0 / 8.0 / lengthFactor;
        }
        return 0.0;
    }

    // forcmain.c:88
    public static double forcemain_getFricSlope(int j, double v, double hrad)
    {
        double re = 0.0;
        double f = 0.0;
        TXsect xsect = S.Link[j].xsect.Clone();
        switch (S.ForceMainEqn)
        {
            case S.H_W:
                return xsect.sBot * Math.Pow(v, 0.852) / Math.Pow(hrad, 1.1667);
            case S.D_W:
                re = S.forcemain_getReynolds(v, hrad);
                f = S.forcemain_getFricFactor(xsect.rBot, hrad, re);
                return f * xsect.sBot * v / hrad;
        }
        return 0.0;
    }

    // forcmain.c:117
    public static double forcemain_getReynolds(double v, double hrad)
    {
        return 4.0 * hrad * v / S.VISCOS;
    }

    // forcmain.c:130
    public static double forcemain_getFricFactor(double e, double hrad, double re)
    {
        double f = 0.0;
        if (re < 10.0)
        {
            re = 10.0;
        }
        if (re <= 2000.0)
        {
            f = 64.0 / re;
        }
        else if (re < 4000.0)
        {
            f = S.forcemain_getFricFactor(e, hrad, 4000.0);
            f = 0.032 + (f - 0.032) * (re - 2000.0) / 2000.0;
        }
        else
        {
            f = e / 3.7 / (4.0 * hrad);
            if (re < 1.0e10)
            {
                f += 5.74 / Math.Pow(re, 0.9);
            }
            f = Math.Log10(f);
            f = 0.25 / f / f;
        }
        return f;
    }

}
}
