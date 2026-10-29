// Generated from EPA SWMM 5.2.4 (kinwave.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double WX = 0.6;
    public static double WT = 0.6;
    public static double EPSIL = 0.001;
    public static double Beta1 = 0.0;
    public static double C1 = 0.0;
    public static double C2 = 0.0;
    public static double Afull = 0.0;
    public static double Qfull = 0.0;
    public static TXsect pXsect = null;

    // kinwave.c:57
    public static int kinwave_execute(int j, ref double qinflow, ref double qoutflow, double tStep)
    {
        int k = 0;
        int result = 1;
        double dxdt = 0.0;
        double dq = 0.0;
        double ain = 0.0;
        double aout = 0.0;
        double qin = 0.0;
        double qout = 0.0;
        double a1 = 0.0;
        double a2 = 0.0;
        double q1 = 0.0;
        double q2 = 0.0;
        double q3 = 0.0;
        qoutflow = (qinflow);
        if (S.Link[j].type != S.CONDUIT)
        {
            return result;
        }
        if (S.Link[j].xsect.type == S.DUMMY)
        {
            return result;
        }
        S.pXsect = S.Link[j].xsect;
        S.Qfull = S.Link[j].qFull;
        S.Afull = S.Link[j].xsect.aFull;
        k = S.Link[j].subIndex;
        S.Beta1 = S.Conduit[k].beta / S.Qfull;
        q1 = S.Conduit[k].q1 / S.Qfull;
        q2 = S.Conduit[k].q2 / S.Qfull;
        qin = (qinflow) / S.Conduit[k].barrels / S.Qfull;
        q3 = S.link_getLossRate(j, S.KW, qin * S.Qfull, tStep) / S.Qfull;
        a1 = S.Conduit[k].a1 / S.Afull;
        a2 = S.Conduit[k].a2 / S.Afull;
        if (qin >= 1.0)
        {
            ain = 1.0;
        }
        else
        {
            ain = S.xsect_getAofS(S.pXsect, qin / S.Beta1) / S.Afull;
        }
        if ((qin <= 1e-06) && (q2 <= 1e-06))
        {
            qout = 0.0;
            aout = 0.0;
        }
        else
        {
            dxdt = S.link_getLength(j) / tStep * S.Afull / S.Qfull;
            dq = q2 - q1;
            S.C1 = dxdt * S.WT / S.WX;
            S.C2 = (1.0 - S.WT) * (ain - a1);
            S.C2 = S.C2 - S.WT * a2;
            S.C2 = S.C2 * dxdt / S.WX;
            S.C2 = S.C2 + (1.0 - S.WX) / S.WX * dq - qin;
            S.C2 = S.C2 + q3 / S.WX;
            aout = a2;
            result = S.solveContinuity(qin, ain, ref aout);
            if (result == -1)
            {
                S.report_writeErrorMsg(S.ERR_KINWAVE, S.Link[j].ID);
                return 1;
            }
            if (result <= 0)
            {
                result = 1;
            }
            qout = S.Beta1 * S.xsect_getSofA(S.pXsect, aout * S.Afull);
            if (qin > 1.0)
            {
                qin = 1.0;
            }
        }
        S.Conduit[k].q1 = qin * S.Qfull;
        S.Conduit[k].a1 = ain * S.Afull;
        S.Conduit[k].q2 = qout * S.Qfull;
        S.Conduit[k].a2 = aout * S.Afull;
        S.Conduit[k].fullState = S.link_getFullState(S.Conduit[k].a1, S.Conduit[k].a2, S.Afull);
        qinflow = S.Conduit[k].q1 * S.Conduit[k].barrels;
        qoutflow = S.Conduit[k].q2 * S.Conduit[k].barrels;
        return result;
    }

    // kinwave.c:170
    public static int solveContinuity(double qin, double ain, ref double aout)
    {
        int n = 0;
        double aLo = 0.0;
        double aHi = 0.0;
        double aTmp = 0.0;
        double fLo = 0.0;
        double fHi = 0.0;
        double tol = S.EPSIL;
        aHi = 1.0;
        fHi = 1.0 + S.C1 + S.C2;
        aLo = S.xsect_getAmax(S.pXsect) / S.Afull;
        if (aLo < aHi)
        {
            fLo = (S.Beta1 * S.pXsect.sMax) + (S.C1 * aLo) + S.C2;
        }
        else
        {
            fLo = fHi;
        }
        if (fHi * fLo > 0.0)
        {
            aHi = aLo;
            fHi = fLo;
            aLo = 0.0;
            fLo = S.C2;
        }
        if (fHi * fLo <= 0.0)
        {
            if ((aout < aLo) || (aout > aHi))
            {
                aout = 0.5 * (aLo + aHi);
            }
            if (fLo > fHi)
            {
                aTmp = aLo;
                aLo = aHi;
                aHi = aTmp;
            }
            n = S.findroot_Newton(aLo, aHi, ref aout, tol, S.evalContinuity, (null));
            if (n <= 0)
            {
                n = -1;
            }
        }
        else if (fLo < 0.0)
        {
            if (qin > 1.0)
            {
                aout = ain;
            }
            else
            {
                aout = 1.0;
            }
            n = -2;
        }
        else if (fLo > 0)
        {
            aout = 0.0;
            n = -3;
        }
        else
        {
            n = -1;
        }
        return n;
    }

    // kinwave.c:261
    public static void evalContinuity(double a, ref double f, ref double df, object p)
    {
        f = (S.Beta1 * S.xsect_getSofA(S.pXsect, a * S.Afull)) + (S.C1 * a) + S.C2;
        df = (S.Beta1 * S.Afull * S.xsect_getdSdA(S.pXsect, a * S.Afull)) + S.C1;
    }

}
}
