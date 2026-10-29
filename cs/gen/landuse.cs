// Generated from EPA SWMM 5.2.4 (landuse.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // landuse.c:51
    public static int landuse_readParams(int j, CPtr[] tok, int ntoks)
    {
        CPtr id = CPtr.Null;
        if (ntoks < 1)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.LANDUSE, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        S.Landuse[j].ID = id;
        if (ntoks > 1)
        {
            if (ntoks < 4)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (!((S.getDouble(tok[1], ref S.Landuse[j].sweepInterval) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[1]);
            }
            if (!((S.getDouble(tok[2], ref S.Landuse[j].sweepRemoval) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[2]);
            }
            if (!((S.getDouble(tok[3], ref S.Landuse[j].sweepDays0) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[3]);
            }
        }
        else
        {
            S.Landuse[j].sweepInterval = 0.0;
            S.Landuse[j].sweepRemoval = 0.0;
            S.Landuse[j].sweepDays0 = 0.0;
        }
        if ((S.Landuse[j].sweepRemoval < 0.0) || (S.Landuse[j].sweepRemoval > 1.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[2]);
        }
        return 0;
    }

    // landuse.c:92
    public static int landuse_readPollutParams(int j, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int k = 0;
        int coPollut = 0;
        int snowFlag = 0;
        double[] x = new double[4];
        double coFrac = 0.0;
        double cDWF = 0.0;
        double cInit = 0.0;
        CPtr id = CPtr.Null;
        if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.POLLUT, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        k = S.findmatch(tok[1], S.QualUnitsWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        for (i = 2; i <= 4; i++)
        {
            if ((!((S.getDouble(tok[i], ref x[i - 2]) != 0))) || (x[i - 2] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i]);
            }
        }
        if (!((S.getDouble(tok[5], ref x[3]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[5]);
        }
        snowFlag = 0;
        coPollut = -1;
        coFrac = 0.0;
        cDWF = 0.0;
        cInit = 0.0;
        if (ntoks >= 7)
        {
            snowFlag = S.findmatch(tok[6], S.NoYesWords);
            if (snowFlag < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[6]);
            }
        }
        if (ntoks >= 9)
        {
            if (!((S.strcomp(tok[7], S.L275) != 0)))
            {
                coPollut = S.project_findObject(S.POLLUT, tok[7]);
                if (coPollut < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[7]);
                }
                if ((!((S.getDouble(tok[8], ref coFrac) != 0))) || (coFrac < 0.0))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[8]);
                }
            }
        }
        if (ntoks >= 10)
        {
            if ((!((S.getDouble(tok[9], ref cDWF) != 0))) || (cDWF < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[9]);
            }
        }
        if (ntoks >= 11)
        {
            if ((!((S.getDouble(tok[10], ref cInit) != 0))) || (cInit < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[9]);
            }
        }
        S.Pollut[j].ID = id;
        S.Pollut[j].units = k;
        if (S.Pollut[j].units == S.MG)
        {
            S.Pollut[j].mcf = S.UCF(S.MASS);
        }
        else if (S.Pollut[j].units == S.UG)
        {
            S.Pollut[j].mcf = S.UCF(S.MASS) / 1000.0;
        }
        else
        {
            S.Pollut[j].mcf = 1.0;
        }
        S.Pollut[j].pptConcen = x[0];
        S.Pollut[j].gwConcen = x[1];
        S.Pollut[j].rdiiConcen = x[2];
        S.Pollut[j].kDecay = x[3] / 86400;
        S.Pollut[j].snowOnly = snowFlag;
        S.Pollut[j].coPollut = coPollut;
        S.Pollut[j].coFraction = coFrac;
        S.Pollut[j].dwfConcen = cDWF;
        S.Pollut[j].initConcen = cInit;
        return 0;
    }

    // landuse.c:190
    public static int landuse_readBuildupParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int n = 0;
        int p = 0;
        double[] c = new double[] { 0, 0, 0 };
        double tmax = 0.0;
        if (ntoks < 3)
        {
            return 0;
        }
        j = S.project_findObject(S.LANDUSE, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        p = S.project_findObject(S.POLLUT, tok[1]);
        if (p < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        k = S.findmatch(tok[2], S.BuildupTypeWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[2]);
        }
        S.Landuse[j].buildupFunc[p].funcType = k;
        if (k > S.NO_BUILDUP)
        {
            if (ntoks < 7)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (k != S.EXTERNAL_BUILDUP)
            {
                for (i = 0; i < 3; i++)
                {
                    if ((!((S.getDouble(tok[i + 3], ref c[i]) != 0))) || (c[i] < 0.0))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[i + 3]);
                    }
                }
            }
            n = S.findmatch(tok[6], S.NormalizerWords);
            if (n < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[6]);
            }
            S.Landuse[j].buildupFunc[p].normalizer = n;
        }
        switch (S.Landuse[j].buildupFunc[p].funcType)
        {
            case S.POWER_BUILDUP:
                if ((c[2] > 0.0) && (((c[2] < 0.01) || (c[2] > 10.0))))
                {
                    return S.error_setInpError(S.ERR_KEYWORD, tok[5]);
                }
                if (c[1] * c[2] == 0.0)
                {
                    tmax = 0.0;
                }
                else if (Math.Log10(c[0]) / c[2] > 3.5)
                {
                    tmax = 3650.0;
                }
                else
                {
                    tmax = Math.Pow(c[0] / c[1], 1.0 / c[2]);
                }
                break;
            case S.EXPON_BUILDUP:
                if (c[1] == 0.0)
                {
                    tmax = 0.0;
                }
                else
                {
                    tmax = -(Math.Log(0.001)) / c[1];
                }
                break;
            case S.SATUR_BUILDUP:
                tmax = 1000.0 * c[2];
                break;
            case S.EXTERNAL_BUILDUP:
                if ((!((S.getDouble(tok[3], ref c[0]) != 0))) || (c[0] < 0.0))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[3]);
                }
                if ((!((S.getDouble(tok[4], ref c[1]) != 0))) || (c[1] < 0.0))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[3]);
                }
                n = S.project_findObject(S.TSERIES, tok[5]);
                if (n < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[4]);
                }
                S.Tseries[n].refersTo = S.EXTERNAL_BUILDUP;
                c[2] = n;
                tmax = 0.0;
                break;
            default:
                tmax = 0.0;
                break;
        }
        S.Landuse[j].buildupFunc[p].coeff[0] = c[0];
        S.Landuse[j].buildupFunc[p].coeff[1] = c[1];
        S.Landuse[j].buildupFunc[p].coeff[2] = c[2];
        S.Landuse[j].buildupFunc[p].maxDays = tmax;
        return 0;
    }

    // landuse.c:281
    public static int landuse_readWashoffParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int p = 0;
        int func = 0;
        double[] x = new double[4];
        if (ntoks < 3)
        {
            return 0;
        }
        for (i = 0; i < 4; i++)
        {
            x[i] = 0.0;
        }
        func = S.NO_WASHOFF;
        j = S.project_findObject(S.LANDUSE, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        p = S.project_findObject(S.POLLUT, tok[1]);
        if (p < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        if (ntoks > 2)
        {
            func = S.findmatch(tok[2], S.WashoffTypeWords);
            if (func < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[2]);
            }
            if (func != S.NO_WASHOFF)
            {
                if (ntoks < 5)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                if (!((S.getDouble(tok[3], ref x[0]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[3]);
                }
                if (!((S.getDouble(tok[4], ref x[1]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[4]);
                }
                if (ntoks >= 6)
                {
                    if (!((S.getDouble(tok[5], ref x[2]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[5]);
                    }
                }
                if (ntoks >= 7)
                {
                    if (!((S.getDouble(tok[6], ref x[3]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[6]);
                    }
                }
            }
        }
        if (x[0] < 0.0)
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[3]);
        }
        if ((x[1] < -10.0) || (x[1] > 10.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[4]);
        }
        ;
        if ((x[2] < 0.0) || (x[2] > 100.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[5]);
        }
        if ((x[3] < 0.0) || (x[3] > 100.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[6]);
        }
        if (func == S.EXPON_WASHOFF)
        {
            x[0] /= 3600.0;
        }
        if (func == S.RATING_WASHOFF)
        {
            x[0] *= Math.Pow(S.UCF(S.FLOW), x[1]);
        }
        if (func == S.EMC_WASHOFF)
        {
            x[0] *= 28.317;
        }
        S.Landuse[j].washoffFunc[p].funcType = func;
        S.Landuse[j].washoffFunc[p].coeff = x[0];
        S.Landuse[j].washoffFunc[p].expon = x[1];
        S.Landuse[j].washoffFunc[p].sweepEffic = x[2] / 100.0;
        S.Landuse[j].washoffFunc[p].bmpEffic = x[3] / 100.0;
        return 0;
    }

    // landuse.c:355
    public static void landuse_getInitBuildup(TLandFactor[] landFactor, double[] initBuildup, double area, double curb)
    {
        int i = 0;
        int p = 0;
        double startDrySeconds = 0.0;
        double f = 0.0;
        double fArea = 0.0;
        double fCurb = 0.0;
        double buildup = 0.0;
        startDrySeconds = S.StartDryDays * 86400;
        for (i = 0; i < S.Nobjects[S.LANDUSE]; i++)
        {
            landFactor[i].lastSwept = S.StartDateTime - S.Landuse[i].sweepDays0;
            f = landFactor[i].fraction;
            fArea = f * area * S.UCF(S.LANDAREA);
            fCurb = f * curb;
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                buildup = 0.0;
                if (initBuildup[p] > 0.0)
                {
                    buildup = initBuildup[p] * fArea;
                }
                else
                {
                    buildup = S.landuse_getBuildup(i, p, fArea, fCurb, buildup, startDrySeconds);
                }
                landFactor[i].buildup[p] = buildup;
            }
        }
    }

    // landuse.c:411
    public static double landuse_getBuildup(int i, int p, double area, double curb, double buildup, double tStep)
    {
        int n = 0;
        double days = 0.0;
        double perUnit = 0.0;
        if ((S.Landuse[i].buildupFunc[p].funcType == S.NO_BUILDUP) || (tStep == 0.0))
        {
            return buildup;
        }
        n = S.Landuse[i].buildupFunc[p].normalizer;
        perUnit = 1.0;
        if (n == S.PER_AREA)
        {
            perUnit = area;
        }
        if (n == S.PER_CURB)
        {
            perUnit = curb;
        }
        if (perUnit == 0.0)
        {
            return 0.0;
        }
        if (S.Landuse[i].buildupFunc[p].funcType == S.EXTERNAL_BUILDUP)
        {
            return S.landuse_getExternalBuildup(i, p, buildup / perUnit, tStep) * perUnit;
        }
        days = S.landuse_getBuildupDays(i, p, buildup / perUnit);
        days += tStep / 86400;
        return S.landuse_getBuildupMass(i, p, days) * perUnit;
    }

    // landuse.c:458
    public static double landuse_getBuildupDays(int i, int p, double buildup)
    {
        double c0 = S.Landuse[i].buildupFunc[p].coeff[0];
        double c1 = S.Landuse[i].buildupFunc[p].coeff[1];
        double c2 = S.Landuse[i].buildupFunc[p].coeff[2];
        if (buildup == 0.0)
        {
            return 0.0;
        }
        if (buildup >= c0)
        {
            return S.Landuse[i].buildupFunc[p].maxDays;
        }
        switch (S.Landuse[i].buildupFunc[p].funcType)
        {
            case S.POWER_BUILDUP:
                if (c1 * c2 == 0.0)
                {
                    return 0.0;
                }
                else
                {
                    return Math.Pow((buildup / c1), (1.0 / c2));
                }
            case S.EXPON_BUILDUP:
                if (c0 * c1 == 0.0)
                {
                    return 0.0;
                }
                else
                {
                    return -(Math.Log(1.0 - buildup / c0)) / c1;
                }
            case S.SATUR_BUILDUP:
                if (c0 == 0.0)
                {
                    return 0.0;
                }
                else
                {
                    return buildup * c2 / (c0 - buildup);
                }
            default:
                return 0.0;
        }
    }

    // landuse.c:494
    public static double landuse_getBuildupMass(int i, int p, double days)
    {
        double b = 0.0;
        double c0 = S.Landuse[i].buildupFunc[p].coeff[0];
        double c1 = S.Landuse[i].buildupFunc[p].coeff[1];
        double c2 = S.Landuse[i].buildupFunc[p].coeff[2];
        if (days == 0.0)
        {
            return 0.0;
        }
        if (days >= S.Landuse[i].buildupFunc[p].maxDays)
        {
            return c0;
        }
        switch (S.Landuse[i].buildupFunc[p].funcType)
        {
            case S.POWER_BUILDUP:
                b = c1 * Math.Pow(days, c2);
                if (b > c0)
                {
                    b = c0;
                }
                break;
            case S.EXPON_BUILDUP:
                b = c0 * (1.0 - Math.Exp(-days * c1));
                break;
            case S.SATUR_BUILDUP:
                b = days * c0 / (c2 + days);
                break;
            default:
                b = 0.0;
                break;
        }
        return b;
    }

    // landuse.c:532
    public static double landuse_getAvgBmpEffic(int j, int p)
    {
        int i = 0;
        double r = 0.0;
        for (i = 0; i < S.Nobjects[S.LANDUSE]; i++)
        {
            r += S.Subcatch[j].landFactor[i].fraction * S.Landuse[i].washoffFunc[p].bmpEffic;
        }
        return r;
    }

    // landuse.c:553
    public static double landuse_getWashoffLoad(int i, int p, double area, TLandFactor[] landFactor, double runoff, double vOutflow)
    {
        double landuseArea = 0.0;
        double buildup = 0.0;
        double washoffQual = 0.0;
        double washoffLoad = 0.0;
        double bmpRemoval = 0.0;
        buildup = landFactor[i].buildup[p];
        landuseArea = landFactor[i].fraction * area;
        washoffQual = S.landuse_getWashoffQual(i, p, buildup, runoff, landuseArea);
        washoffLoad = washoffQual * vOutflow * landuseArea / area * S.Pollut[p].mcf;
        if ((S.Landuse[i].buildupFunc[p].funcType != S.NO_BUILDUP) || (buildup > washoffLoad))
        {
            washoffLoad = ((((washoffLoad) <= (buildup)) ? (washoffLoad) : (buildup)));
            buildup -= washoffLoad;
            landFactor[i].buildup[p] = buildup;
        }
        else
        {
            S.massbal_updateLoadingTotals(S.BUILDUP_LOAD, p, washoffLoad);
            landFactor[i].buildup[p] = 0.0;
        }
        bmpRemoval = S.Landuse[i].washoffFunc[p].bmpEffic * washoffLoad;
        if (bmpRemoval > 0.0)
        {
            S.massbal_updateLoadingTotals(S.BMP_REMOVAL_LOAD, p, bmpRemoval);
            washoffLoad -= bmpRemoval;
        }
        return washoffLoad / S.Pollut[p].mcf;
    }

    // landuse.c:612
    public static double landuse_getWashoffQual(int i, int p, double buildup, double runoff, double area)
    {
        double cWashoff = 0.0;
        double coeff = S.Landuse[i].washoffFunc[p].coeff;
        double expon = S.Landuse[i].washoffFunc[p].expon;
        int func = S.Landuse[i].washoffFunc[p].funcType;
        if ((func == S.NO_WASHOFF) || (runoff == 0.0))
        {
            return 0.0;
        }
        if ((S.Landuse[i].buildupFunc[p].funcType != S.NO_BUILDUP) && (buildup == 0.0))
        {
            return 0.0;
        }
        if (func == S.EXPON_WASHOFF)
        {
            cWashoff = coeff * Math.Pow(runoff * S.UCF(S.RAINFALL), expon) * buildup / S.Pollut[p].mcf;
            cWashoff /= runoff * area;
        }
        else if (func == S.RATING_WASHOFF)
        {
            cWashoff = coeff * Math.Pow(runoff * area, expon - 1.0);
        }
        else if (func == S.EMC_WASHOFF)
        {
            cWashoff = coeff;
        }
        return cWashoff;
    }

    // landuse.c:665
    public static double landuse_getCoPollutLoad(int p, double[] washoff)
    {
        int k = 0;
        double w = 0.0;
        k = S.Pollut[p].coPollut;
        if (k >= 0)
        {
            w = S.Pollut[p].coFraction * washoff[k];
            S.massbal_updateLoadingTotals(S.BUILDUP_LOAD, p, w * S.Pollut[p].mcf);
            return w;
        }
        return 0.0;
    }

    // landuse.c:693
    public static double landuse_getExternalBuildup(int i, int p, double buildup, double tStep)
    {
        double maxBuildup = S.Landuse[i].buildupFunc[p].coeff[0];
        double sf = S.Landuse[i].buildupFunc[p].coeff[1];
        int ts = R.D2I(Math.Floor(S.Landuse[i].buildupFunc[p].coeff[2]));
        double rate = 0.0;
        if (S.NewRunoffTime == 0.0)
        {
            return 0.0;
        }
        if (ts >= 0)
        {
            rate = sf * S.table_tseriesLookup(S.Tseries[ts], S.getDateTime(S.NewRunoffTime), 0);
        }
        buildup = buildup + rate * tStep / 86400;
        buildup = ((((buildup) <= (maxBuildup)) ? (buildup) : (maxBuildup)));
        return buildup;
    }

}
}
