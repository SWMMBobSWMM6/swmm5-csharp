// Generated from EPA SWMM 5.2.4 (exfil.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // exfil.c:34
    public static int exfil_readStorageParams(int k, CPtr[] tok, int ntoks, int n)
    {
        int i = 0;
        double[] x = new double[3];
        if (ntoks == n + 1)
        {
            if (!((S.getDouble(tok[n], ref x[1]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[n]);
            }
            x[0] = 0.0;
            x[2] = 0.0;
        }
        else if (ntoks < n + 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        else
        {
            for (i = 0; i < 3; i++)
            {
                if (!((S.getDouble(tok[n + i], ref x[i]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[n + i]);
                }
            }
        }
        if (x[1] == 0.0)
        {
            return 0;
        }
        return S.createStorageExfil(k, x);
    }

    // exfil.c:74
    public static void exfil_initState(int k)
    {
        int i = 0;
        double a = 0.0;
        double alast = 0.0;
        double d = 0.0;
        TTable aCurve = null;
        TExfil exfil = S.Storage[k].exfil;
        if (exfil != null)
        {
            S.grnampt_initState(exfil.btmExfil);
            S.grnampt_initState(exfil.bankExfil);
            switch (S.Storage[k].shape)
            {
                case S.TABULAR:
                    i = S.Storage[k].aCurve;
                    exfil.btmArea = 0.0;
                    exfil.bankMinDepth = 0.0;
                    exfil.bankMaxDepth = 0.0;
                    exfil.bankMaxArea = 0.0;
                    if (i >= 0)
                    {
                        aCurve = S.Curve[i];
                        S.Storage[k].exfil.btmArea = S.table_lookupEx(aCurve, 0.0);
                        S.table_getFirstEntry(aCurve, ref d, ref a);
                        alast = a;
                        while ((S.table_getNextEntry(aCurve, ref d, ref a) != 0))
                        {
                            if (a < alast)
                            {
                                break;
                            }
                            else if (a > alast)
                            {
                                exfil.bankMaxArea = a;
                                exfil.bankMaxDepth = d;
                            }
                            else if (exfil.bankMaxArea == 0.0)
                            {
                                exfil.bankMinDepth = d;
                            }
                            else
                            {
                                break;
                            }
                            alast = a;
                        }
                        exfil.btmArea /= S.UCF(S.LENGTH) * S.UCF(S.LENGTH);
                        exfil.bankMaxArea /= S.UCF(S.LENGTH) * S.UCF(S.LENGTH);
                        exfil.bankMinDepth /= S.UCF(S.LENGTH);
                        exfil.bankMaxDepth /= S.UCF(S.LENGTH);
                    }
                    break;
                case S.FUNCTIONAL:
                    exfil.btmArea = S.Storage[k].a0;
                    if (S.Storage[k].a2 == 0.0)
                    {
                        exfil.btmArea += S.Storage[k].a1;
                    }
                    exfil.bankMinDepth = 0.0;
                    exfil.bankMaxDepth = 10000000000.0;
                    exfil.bankMaxArea = 10000000000.0;
                    break;
                case S.CYLINDRICAL:
                case S.CONICAL:
                case S.PYRAMIDAL:
                    exfil.btmArea = S.Storage[k].a0;
                    exfil.bankMinDepth = 0.0;
                    exfil.bankMaxDepth = 10000000000.0;
                    exfil.bankMaxArea = 10000000000.0;
                    break;
            }
        }
    }

    // exfil.c:158
    public static double exfil_getLoss(TExfil exfil, double tStep, double depth, double area)
    {
        double exfilRate = 0.0;
        if (exfil.btmExfil.IMDmax == 0.0)
        {
            exfilRate = exfil.btmExfil.Ks * S.Adjust.hydconFactor;
        }
        else
        {
            exfilRate = S.grnampt_getInfil(exfil.btmExfil, tStep, 0.0, depth, S.MOD_GREEN_AMPT);
        }
        exfilRate *= exfil.btmArea;
        if (depth > exfil.bankMinDepth)
        {
            area = ((((area) <= (exfil.bankMaxArea)) ? (area) : (exfil.bankMaxArea))) - exfil.btmArea;
            if (area > 0.0)
            {
                if (exfil.btmExfil.IMDmax == 0.0)
                {
                    exfilRate += area * exfil.btmExfil.Ks * S.Adjust.hydconFactor;
                }
                else
                {
                    if (depth > exfil.bankMaxDepth)
                    {
                        depth = depth - exfil.bankMaxDepth + (exfil.bankMaxDepth - exfil.bankMinDepth) / 2.0;
                    }
                    else
                    {
                        depth = (depth - exfil.bankMinDepth) / 2.0;
                    }
                    exfilRate += area * S.grnampt_getInfil(exfil.bankExfil, tStep, 0.0, depth, S.MOD_GREEN_AMPT);
                }
            }
        }
        return exfilRate;
    }

    // exfil.c:218
    public static int createStorageExfil(int k, double[] x)
    {
        TExfil exfil = null;
        exfil = S.Storage[k].exfil;
        if (exfil == null)
        {
            exfil = new TExfil();
            if (exfil == null)
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
            S.Storage[k].exfil = exfil;
            exfil.btmExfil = (null);
            exfil.bankExfil = (null);
            exfil.btmExfil = new TGrnAmpt();
            if (exfil.btmExfil == null)
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
            exfil.bankExfil = new TGrnAmpt();
            if (exfil.bankExfil == null)
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
        }
        if (!((S.grnampt_setParams(exfil.btmExfil, x) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, S.L0);
        }
        S.grnampt_setParams(exfil.bankExfil, x);
        return 0;
    }

}
}
