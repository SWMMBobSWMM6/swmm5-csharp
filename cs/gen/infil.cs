// Generated from EPA SWMM 5.2.4 (infil.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static TInfil[] Infil = null;
    public static double Fumax = 0.0;
    public static double InfilFactor = 0.0;

    // infil.c:101
    public static void infil_create(int n)
    {
        S.Infil = R.NewArray<TInfil>((int)(unchecked((ulong)(n))));
        if (S.Infil == null)
        {
            S.ErrorCode = S.ERR_MEMORY;
        }
        S.InfilFactor = 1.0;
        return;
    }

    // infil.c:116
    public static void infil_delete()
    {
        {
            if ((S.Infil != null))
            {
                R.free(S.Infil);
                S.Infil = (null);
            }
        }
        ;
    }

    // infil.c:128
    public static int infil_readParams(int m, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int n = 0;
        int status = 0;
        double[] x = new double[5];
        j = S.project_findObject(S.SUBCATCH, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        i = S.findmatch(tok[ntoks - 1], S.InfilModelWords);
        if (i >= 0)
        {
            m = i;
            --ntoks;
        }
        if (m == S.HORTON)
        {
            n = 5;
        }
        else if (m == S.MOD_HORTON)
        {
            n = 5;
        }
        else if (m == S.GREEN_AMPT)
        {
            n = 4;
        }
        else if (m == S.MOD_GREEN_AMPT)
        {
            n = 4;
        }
        else if (m == S.CURVE_NUMBER)
        {
            n = 4;
        }
        else
        {
            return 0;
        }
        if (ntoks < n)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        for (i = 0; i < 5; i++)
        {
            x[i] = 0.0;
        }
        for (i = 1; i < n; i++)
        {
            if (!((S.getDouble(tok[i], ref x[i - 1]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i]);
            }
        }
        if ((((m == S.HORTON) || (m == S.MOD_HORTON))) && (ntoks > n))
        {
            if (!((S.getDouble(tok[n], ref x[n - 1]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[n]);
            }
        }
        S.Subcatch[j].infil = j;
        S.Subcatch[j].infilModel = m;
        switch (m)
        {
            case S.HORTON:
            case S.MOD_HORTON:
                status = S.horton_setParams(S.Infil[j].horton, x);
                break;
            case S.GREEN_AMPT:
            case S.MOD_GREEN_AMPT:
                status = S.grnampt_setParams(S.Infil[j].grnAmpt, x);
                break;
            case S.CURVE_NUMBER:
                status = S.curvenum_setParams(S.Infil[j].curveNum, x);
                break;
            default:
                status = 1;
                break;
        }
        if (!(status != 0))
        {
            return S.error_setInpError(S.ERR_INFIL_PARAMS, S.L0);
        }
        return 0;
    }

    // infil.c:201
    public static void infil_initState(int j)
    {
        switch (S.Subcatch[j].infilModel)
        {
            case S.HORTON:
            case S.MOD_HORTON:
                S.horton_initState(S.Infil[j].horton);
                break;
            case S.GREEN_AMPT:
            case S.MOD_GREEN_AMPT:
                S.grnampt_initState(S.Infil[j].grnAmpt);
                break;
            case S.CURVE_NUMBER:
                S.curvenum_initState(S.Infil[j].curveNum);
                break;
        }
    }

    // infil.c:221
    public static void infil_getState(int j, double[] x)
    {
        switch (S.Subcatch[j].infilModel)
        {
            case S.HORTON:
            case S.MOD_HORTON:
                S.horton_getState(S.Infil[j].horton, x);
                break;
            case S.GREEN_AMPT:
            case S.MOD_GREEN_AMPT:
                S.grnampt_getState(S.Infil[j].grnAmpt, x);
                break;
            case S.CURVE_NUMBER:
                S.curvenum_getState(S.Infil[j].curveNum, x);
                break;
        }
    }

    // infil.c:241
    public static void infil_setState(int j, double[] x)
    {
        switch (S.Subcatch[j].infilModel)
        {
            case S.HORTON:
            case S.MOD_HORTON:
                S.horton_setState(S.Infil[j].horton, x);
                break;
            case S.GREEN_AMPT:
            case S.MOD_GREEN_AMPT:
                S.grnampt_setState(S.Infil[j].grnAmpt, x);
                break;
            case S.CURVE_NUMBER:
                S.curvenum_setState(S.Infil[j].curveNum, x);
                break;
        }
    }

    // infil.c:262
    public static void infil_setInfilFactor(int j)
    {
        int m = 0;
        int p = 0;
        S.InfilFactor = S.Adjust.hydconFactor;
        if (j >= 0)
        {
            p = S.Subcatch[j].infilPattern;
            if ((p >= 0) && (S.Pattern[p].type == S.MONTHLY_PATTERN))
            {
                m = S.datetime_monthOfYear(S.getDateTime(S.OldRunoffTime)) - 1;
                S.InfilFactor = S.Pattern[p].factor[m];
            }
        }
    }

    // infil.c:288
    public static double infil_getInfil(int j, double tstep, double rainfall, double runon, double depth)
    {
        switch (S.Subcatch[j].infilModel)
        {
            case S.HORTON:
                return S.horton_getInfil(S.Infil[j].horton, tstep, rainfall + runon, depth);
            case S.MOD_HORTON:
                return S.modHorton_getInfil(S.Infil[j].horton, tstep, rainfall + runon, depth);
            case S.GREEN_AMPT:
            case S.MOD_GREEN_AMPT:
                return S.grnampt_getInfil(S.Infil[j].grnAmpt, tstep, rainfall + runon, depth, S.Subcatch[j].infilModel);
            case S.CURVE_NUMBER:
                depth += runon * tstep;
                return S.curvenum_getInfil(S.Infil[j].curveNum, tstep, rainfall, depth);
            default:
                return 0.0;
        }
    }

    // infil.c:325
    public static int horton_setParams(THorton infil, double[] p)
    {
        int k = 0;
        for (k = 0; k < 5; k++)
        {
            if (p[k] < 0.0)
            {
                return 0;
            }
        }
        infil.f0 = p[0] / S.UCF(S.RAINFALL);
        infil.fmin = p[1] / S.UCF(S.RAINFALL);
        infil.decay = p[2] / 3600.0;
        if (p[3] == 0.0)
        {
            p[3] = 1e-06;
        }
        infil.regen = -(Math.Log(1.0 - 0.98)) / p[3] / 86400;
        infil.Fmax = p[4] / S.UCF(S.RAINDEPTH);
        if (infil.f0 < infil.fmin)
        {
            return 0;
        }
        return 1;
    }

    // infil.c:357
    public static void horton_initState(THorton infil)
    {
        infil.tp = 0.0;
        infil.Fe = 0.0;
    }

    // infil.c:370
    public static void horton_getState(THorton infil, double[] x)
    {
        x[0] = infil.tp;
        x[1] = infil.Fe;
    }

    // infil.c:376
    public static void horton_setState(THorton infil, double[] x)
    {
        infil.tp = x[0];
        infil.Fe = x[1];
    }

    // infil.c:384
    public static double horton_getInfil(THorton infil, double tstep, double irate, double depth)
    {
        int iter = 0;
        double fa = 0.0;
        double fp = 0.0;
        double Fp = 0.0;
        double F1 = 0.0;
        double t1 = 0.0;
        double tlim = 0.0;
        double ex = 0.0;
        double kt = 0.0;
        double FF = 0.0;
        double FF1 = 0.0;
        double r = 0.0;
        double f0 = infil.f0 * S.InfilFactor;
        double fmin = infil.fmin * S.InfilFactor;
        double Fmax = infil.Fmax;
        double tp = infil.tp;
        double df = f0 - fmin;
        double kd = infil.decay;
        double kr = infil.regen * S.Evap.recoveryFactor;
        if (((df < 0.0) || (kd < 0.0)) || (kr < 0.0))
        {
            return 0.0;
        }
        if ((df == 0.0) || (kd == 0.0))
        {
            fp = f0;
            fa = irate + depth / tstep;
            if (fp > fa)
            {
                fp = fa;
            }
            return ((((0.0) >= (fp)) ? (0.0) : (fp)));
        }
        fa = irate + depth / tstep;
        if (fa > 1e-10)
        {
            t1 = tp + tstep;
            tlim = 16.0 / kd;
            if (tp >= tlim)
            {
                Fp = fmin * tp + df / kd;
                F1 = Fp + fmin * tstep;
            }
            else
            {
                Fp = fmin * tp + df / kd * (1.0 - Math.Exp(-kd * tp));
                F1 = fmin * t1 + df / kd * (1.0 - Math.Exp(-kd * t1));
            }
            fp = (F1 - Fp) / tstep;
            fp = ((((fp) >= (fmin)) ? (fp) : (fmin)));
            if (fp > fa)
            {
                fp = fa;
            }
            if (t1 > tlim)
            {
                tp = t1;
            }
            else if (fp < fa)
            {
                tp = t1;
            }
            else
            {
                F1 = Fp + fp * tstep;
                tp = tp + tstep / 2.0;
                for (iter = 1; iter <= 20; iter++)
                {
                    kt = ((((60.0) <= (kd * tp)) ? (60.0) : (kd * tp)));
                    ex = Math.Exp(-kt);
                    FF = fmin * tp + df / kd * (1.0 - ex) - F1;
                    FF1 = fmin + df * ex;
                    r = FF / FF1;
                    tp = tp - r;
                    if (Math.Abs(r) <= 0.001 * tstep)
                    {
                        break;
                    }
                }
            }
            if (Fmax > 0.0)
            {
                if (infil.Fe + fp * tstep > Fmax)
                {
                    fp = (Fmax - infil.Fe) / tstep;
                }
                fp = ((((fp) >= (0.0)) ? (fp) : (0.0)));
                infil.Fe += fp * tstep;
            }
        }
        else if (kr > 0.0)
        {
            r = Math.Exp(-kr * tstep);
            tp = 1.0 - Math.Exp(-kd * tp);
            tp = -(Math.Log(1.0 - r * tp)) / kd;
            if (Fmax > 0.0)
            {
                infil.Fe = fmin * tp + (df / kd) * (1.0 - Math.Exp(-kd * tp));
            }
        }
        infil.tp = tp;
        return fp;
    }

    // infil.c:496
    public static double modHorton_getInfil(THorton infil, double tstep, double irate, double depth)
    {
        double f = 0.0;
        double fp = 0.0;
        double fa = 0.0;
        double f0 = infil.f0 * S.InfilFactor;
        double fmin = infil.fmin * S.InfilFactor;
        double df = f0 - fmin;
        double kd = infil.decay;
        double kr = infil.regen * S.Evap.recoveryFactor;
        if (((df < 0.0) || (kd < 0.0)) || (kr < 0.0))
        {
            return 0.0;
        }
        if ((df == 0.0) || (kd == 0.0))
        {
            fp = f0;
            fa = irate + depth / tstep;
            if (fp > fa)
            {
                fp = fa;
            }
            return ((((0.0) >= (fp)) ? (0.0) : (fp)));
        }
        fa = irate + depth / tstep;
        if (fa > 1e-10)
        {
            if ((infil.Fmax > 0.0) && (infil.Fe >= infil.Fmax))
            {
                return 0.0;
            }
            fp = f0 - kd * infil.Fe;
            fp = ((((fp) >= (fmin)) ? (fp) : (fmin)));
            f = ((((fa) <= (fp)) ? (fa) : (fp)));
            infil.Fe += (((((f - fmin)) >= (0.0)) ? ((f - fmin)) : (0.0))) * tstep;
            if (infil.Fmax > 0.0)
            {
                infil.Fe = ((((infil.Fe) >= (infil.Fmax)) ? (infil.Fe) : (infil.Fmax)));
            }
        }
        else if (kr > 0.0)
        {
            infil.Fe *= Math.Exp(-kr * tstep);
            infil.Fe = ((((infil.Fe) >= (0.0)) ? (infil.Fe) : (0.0)));
        }
        return f;
    }

    // infil.c:559
    public static void grnampt_getParams(int j, double[] p)
    {
        p[0] = S.Infil[j].grnAmpt.S_ * S.UCF(S.RAINDEPTH);
        p[1] = S.Infil[j].grnAmpt.Ks * S.UCF(S.RAINFALL);
        p[2] = S.Infil[j].grnAmpt.IMDmax;
    }

    // infil.c:574
    public static int grnampt_setParams(TGrnAmpt infil, double[] p)
    {
        double ksat = 0.0;
        if ((((p[0] < 0.0) || (p[1] <= 0.0)) || (p[2] < 0.0)) || (p[2] > 1.0))
        {
            return 0;
        }
        infil.S_ = p[0] / S.UCF(S.RAINDEPTH);
        infil.Ks = p[1] / S.UCF(S.RAINFALL);
        infil.IMDmax = p[2];
        ksat = infil.Ks * 12.0 * 3600.0;
        infil.Lu = 4.0 * Math.Sqrt(ksat) / 12.0;
        return 1;
    }

    // infil.c:597
    public static void grnampt_initState(TGrnAmpt infil)
    {
        if (infil == null)
        {
            return;
        }
        infil.IMD = infil.IMDmax;
        infil.Fu = 0.0;
        infil.F = 0.0;
        infil.Sat = 0;
        infil.T = 0.0;
    }

    // infil.c:612
    public static void grnampt_getState(TGrnAmpt infil, double[] x)
    {
        x[0] = infil.IMD;
        x[1] = infil.F;
        x[2] = infil.Fu;
        x[3] = infil.Sat;
        x[4] = infil.T;
    }

    // infil.c:621
    public static void grnampt_setState(TGrnAmpt infil, double[] x)
    {
        infil.IMD = x[0];
        infil.F = x[1];
        infil.Fu = x[2];
        infil.Sat = R.D2I(x[3]);
        infil.T = x[4];
    }

    // infil.c:632
    public static double grnampt_getInfil(TGrnAmpt infil, double tstep, double irate, double depth, int modelType)
    {
        S.Fumax = infil.IMDmax * infil.Lu * Math.Sqrt(S.InfilFactor);
        infil.T -= tstep;
        if ((infil.Sat != 0))
        {
            return S.grnampt_getSatInfil(infil, tstep, irate, depth);
        }
        else
        {
            return S.grnampt_getUnsatInfil(infil, tstep, irate, depth, modelType);
        }
    }

    // infil.c:660
    public static double grnampt_getUnsatInfil(TGrnAmpt infil, double tstep, double irate, double depth, int modelType)
    {
        double ia = 0.0;
        double c1 = 0.0;
        double F2 = 0.0;
        double dF = 0.0;
        double Fs = 0.0;
        double kr = 0.0;
        double ts = 0.0;
        double ks = infil.Ks * S.InfilFactor;
        double lu = infil.Lu * Math.Sqrt(S.InfilFactor);
        ia = irate + depth / tstep;
        if (ia < 1e-10)
        {
            ia = 0.0;
        }
        if (ia == 0.0)
        {
            if (infil.Fu <= 0.0)
            {
                return 0.0;
            }
            kr = lu / 90000.0 * S.Evap.recoveryFactor;
            dF = kr * S.Fumax * tstep;
            infil.F -= dF;
            infil.Fu -= dF;
            if (infil.Fu <= 0.0)
            {
                infil.Fu = 0.0;
                infil.F = 0.0;
                infil.IMD = infil.IMDmax;
                return 0.0;
            }
            if (infil.T <= 0.0)
            {
                infil.IMD = (S.Fumax - infil.Fu) / lu;
                infil.F = 0.0;
            }
            return 0.0;
        }
        if (ia <= ks)
        {
            dF = ia * tstep;
            infil.F += dF;
            infil.Fu += dF;
            infil.Fu = ((((infil.Fu) <= (S.Fumax)) ? (infil.Fu) : (S.Fumax)));
            if ((modelType == S.GREEN_AMPT) && (infil.T <= 0.0))
            {
                infil.IMD = (S.Fumax - infil.Fu) / lu;
                infil.F = 0.0;
            }
            return ia;
        }
        infil.T = 5400.0 / lu / S.Evap.recoveryFactor;
        Fs = ks * (infil.S_ + depth) * infil.IMD / (ia - ks);
        if (infil.F > Fs)
        {
            infil.Sat = 1;
            return S.grnampt_getSatInfil(infil, tstep, irate, depth);
        }
        if (infil.F + ia * tstep < Fs)
        {
            dF = ia * tstep;
            infil.F += dF;
            infil.Fu += dF;
            infil.Fu = ((((infil.Fu) <= (S.Fumax)) ? (infil.Fu) : (S.Fumax)));
            return ia;
        }
        ts = tstep - (Fs - infil.F) / ia;
        if (ts <= 0.0)
        {
            ts = 0.0;
        }
        c1 = (infil.S_ + depth) * infil.IMD;
        F2 = S.grnampt_getF2(Fs, c1, ks, ts);
        if (F2 > Fs + ia * ts)
        {
            F2 = Fs + ia * ts;
        }
        dF = F2 - infil.F;
        infil.F = F2;
        infil.Fu += dF;
        infil.Fu = ((((infil.Fu) <= (S.Fumax)) ? (infil.Fu) : (S.Fumax)));
        infil.Sat = 1;
        return dF / tstep;
    }

    // infil.c:767
    public static double grnampt_getSatInfil(TGrnAmpt infil, double tstep, double irate, double depth)
    {
        double ia = 0.0;
        double c1 = 0.0;
        double dF = 0.0;
        double F2 = 0.0;
        double ks = infil.Ks * S.InfilFactor;
        double lu = infil.Lu * Math.Sqrt(S.InfilFactor);
        ia = irate + depth / tstep;
        if (ia < 1e-10)
        {
            return 0.0;
        }
        infil.T = 5400.0 / lu / S.Evap.recoveryFactor;
        c1 = (infil.S_ + depth) * infil.IMD;
        F2 = S.grnampt_getF2(infil.F, c1, ks, tstep);
        dF = F2 - infil.F;
        if (dF > ia * tstep)
        {
            dF = ia * tstep;
            infil.Sat = 0;
        }
        infil.F += dF;
        infil.Fu += dF;
        infil.Fu = ((((infil.Fu) <= (S.Fumax)) ? (infil.Fu) : (S.Fumax)));
        return dF / tstep;
    }

    // infil.c:813
    public static double grnampt_getF2(double f1, double c1, double ks, double ts)
    {
        int i = 0;
        double f2 = f1;
        double f2min = 0.0;
        double df2 = 0.0;
        double c2 = 0.0;
        f2min = f1 + ks * ts;
        if (c1 == 0.0)
        {
            return f2min;
        }
        if ((ts < 10.0) && (f1 > 0.01 * c1))
        {
            f2 = f1 + ks * (1.0 + c1 / f1) * ts;
            return ((((f2) >= (f2min)) ? (f2) : (f2min)));
        }
        c2 = c1 * Math.Log(f1 + c1) - ks * ts;
        for (i = 1; i <= 20; i++)
        {
            df2 = (f2 - f1 - c1 * Math.Log(f2 + c1) + c2) / (1.0 - c1 / (f2 + c1));
            if (Math.Abs(df2) < 0.00001)
            {
                return ((((f2) >= (f2min)) ? (f2) : (f2min)));
            }
            f2 -= df2;
        }
        return f2min;
    }

    // infil.c:861
    public static int curvenum_setParams(TCurveNum infil, double[] p)
    {
        if (p[0] < 10.0)
        {
            p[0] = 10.0;
        }
        if (p[0] > 99.0)
        {
            p[0] = 99.0;
        }
        infil.Smax = (1000.0 / p[0] - 10.0) / 12.0;
        if (infil.Smax < 0.0)
        {
            return 0;
        }
        if (p[2] > 0.0)
        {
            infil.regen = 1.0 / (p[2] * 86400);
        }
        else
        {
            return 0;
        }
        infil.Tmax = 0.06 / infil.regen;
        return 1;
    }

    // infil.c:888
    public static void curvenum_initState(TCurveNum infil)
    {
        infil.S_ = infil.Smax;
        infil.P = 0.0;
        infil.F = 0.0;
        infil.T = 0.0;
        infil.Se = infil.Smax;
        infil.f = 0.0;
    }

    // infil.c:903
    public static void curvenum_getState(TCurveNum infil, double[] x)
    {
        x[0] = infil.S_;
        x[1] = infil.P;
        x[2] = infil.F;
        x[3] = infil.T;
        x[4] = infil.Se;
        x[5] = infil.f;
    }

    // infil.c:913
    public static void curvenum_setState(TCurveNum infil, double[] x)
    {
        infil.S_ = x[0];
        infil.P = x[1];
        infil.F = x[2];
        infil.T = x[3];
        infil.Se = x[4];
        infil.f = x[5];
    }

    // infil.c:925
    public static double curvenum_getInfil(TCurveNum infil, double tstep, double irate, double depth)
    {
        double F1 = 0.0;
        double f1 = 0.0;
        double fa = irate + depth / tstep;
        if (irate > 1e-10)
        {
            if (infil.T >= infil.Tmax)
            {
                infil.P = 0.0;
                infil.F = 0.0;
                infil.f = 0.0;
                infil.Se = infil.S_;
            }
            infil.T = 0.0;
            infil.P += irate * tstep;
            F1 = infil.P * (1.0 - infil.P / (infil.P + infil.Se));
            f1 = (F1 - infil.F) / tstep;
            if ((f1 < 0.0) || (infil.S_ <= 0.0))
            {
                f1 = 0.0;
            }
        }
        else
        {
            if ((depth > 0.004167) && (infil.S_ > 0.0))
            {
                f1 = infil.f;
                if (f1 * tstep > infil.S_)
                {
                    f1 = infil.S_ / tstep;
                }
            }
            else
            {
                infil.T += tstep;
            }
        }
        if (f1 > 0.0)
        {
            f1 = ((((f1) <= (fa)) ? (f1) : (fa)));
            f1 = ((((f1) >= (0.0)) ? (f1) : (0.0)));
            infil.F += f1 * tstep;
            if (infil.regen > 0.0)
            {
                infil.S_ -= f1 * tstep;
                if (infil.S_ < 0.0)
                {
                    infil.S_ = 0.0;
                }
            }
        }
        else
        {
            infil.S_ += infil.regen * infil.Smax * tstep * S.Evap.recoveryFactor;
            if (infil.S_ > infil.Smax)
            {
                infil.S_ = infil.Smax;
            }
        }
        infil.f = f1;
        return f1;
    }

}
}
