// Generated from EPA SWMM 5.2.4 (snow.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // snow.c:68
    public static int snow_readMeltParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int m = 0;
        int n = 0;
        double[] x = new double[7];
        if (ntoks < 8)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.SNOWMELT, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        if (S.Snowmelt[j].ID.IsNull)
        {
            S.Snowmelt[j].ID = S.project_findID(S.SNOWMELT, tok[0]);
        }
        k = S.findmatch(tok[1], S.SnowmeltWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        n = 7;
        if (k == S.SNOW_REMOVAL)
        {
            n = 6;
        }
        if (ntoks < n + 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        for (i = 0; i < 7; i++)
        {
            x[i] = 0.0;
        }
        for (i = 0; i < n; i++)
        {
            if (!((S.getDouble(tok[i + 2], ref x[i]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i + 2]);
            }
        }
        if (k == S.SNOW_REMOVAL)
        {
            x[6] = -1.0;
            if (ntoks >= 9)
            {
                m = S.project_findObject(S.SUBCATCH, tok[8]);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[8]);
                }
                x[6] = m;
            }
        }
        S.setMeltParams(j, k, x);
        return 0;
    }

    // snow.c:126
    public static int snow_createSnowpack(int j, int k)
    {
        TSnowpack snowpack = null;
        snowpack = new TSnowpack();
        if (!(snowpack != null))
        {
            return 0;
        }
        S.Subcatch[j].snowpack = snowpack;
        snowpack.snowmeltIndex = k;
        return 1;
    }

    // snow.c:144
    public static void snow_initSnowpack(int j)
    {
        int i = 0;
        int k = 0;
        double f = 0.0;
        double snowDepth = 0.0;
        TSnowpack snowpack = null;
        snowpack = S.Subcatch[j].snowpack;
        if (snowpack == null)
        {
            return;
        }
        k = S.Subcatch[j].snowpack.snowmeltIndex;
        f = S.Snowmelt[k].snn;
        snowpack.fArea[S.SNOW_PLOWABLE] = f * S.Subcatch[j].fracImperv;
        snowpack.fArea[S.SNOW_IMPERV] = (1.0 - f) * S.Subcatch[j].fracImperv;
        snowpack.fArea[S.SNOW_PERV] = 1.0 - S.Subcatch[j].fracImperv;
        for (i = S.SNOW_PLOWABLE; i <= S.SNOW_PERV; i++)
        {
            if (snowpack.fArea[i] > 0.0)
            {
                snowpack.wsnow[i] = S.Snowmelt[k].wsnow[i];
                snowpack.fw[i] = S.Snowmelt[k].fwnow[i];
            }
            else
            {
                snowpack.wsnow[i] = 0.0;
                snowpack.fw[i] = 0.0;
            }
            snowpack.coldc[i] = 0.0;
            snowpack.ati[i] = S.Snowmelt[k].tbase[i];
            snowpack.awe[i] = 1.0;
            snowDepth += snowpack.wsnow[i] * snowpack.fArea[i];
        }
        S.Subcatch[j].newSnowDepth = snowDepth;
    }

    // snow.c:193
    public static void snow_initSnowmelt(int j)
    {
        int i = 0;
        int k = 0;
        for (i = 0; i < 3; i++)
        {
            S.Snowmelt[j].snn = 0.0;
            S.Snowmelt[j].si[i] = 0.0;
            S.Snowmelt[j].dhmin[i] = 0.0;
            S.Snowmelt[j].dhmax[i] = 0.0;
            S.Snowmelt[j].tbase[i] = 0.0;
            S.Snowmelt[j].fwfrac[i] = 0.0;
            S.Snowmelt[j].wsnow[i] = 0.0;
            S.Snowmelt[j].fwnow[i] = 0.0;
            S.Snowmelt[j].weplow = 1.0e6;
            for (k = 0; k < 5; k++)
            {
                S.Snowmelt[j].sfrac[k] = 0.0;
            }
            S.Snowmelt[j].toSubcatch = -1;
        }
    }

    // snow.c:219
    public static void snow_validateSnowmelt(int j)
    {
        int k = 0;
        int err = 0;
        double sum = 0.0;
        for (k = S.SNOW_PLOWABLE; k <= S.SNOW_PERV; k++)
        {
            if (S.Snowmelt[j].dhmin[k] > S.Snowmelt[j].dhmax[k])
            {
                err = 1;
            }
            if ((S.Snowmelt[j].fwfrac[k] < 0.0) || (S.Snowmelt[j].fwfrac[k] > 1.0))
            {
                err = 1;
            }
        }
        if ((S.Snowmelt[j].snn < 0.0) || (S.Snowmelt[j].snn > 1.0))
        {
            err = 1;
        }
        for (k = 0; k < 5; k++)
        {
            sum += S.Snowmelt[j].sfrac[k];
        }
        if (sum > 1.01)
        {
            err = 1;
        }
        if ((err != 0))
        {
            S.report_writeErrorMsg(S.ERR_SNOWPACK_PARAMS, S.Snowmelt[j].ID);
        }
    }

    // snow.c:251
    public static void snow_getState(int i, int j, double[] x)
    {
        TSnowpack snowpack = S.Subcatch[i].snowpack;
        if (snowpack == null)
        {
            return;
        }
        x[0] = snowpack.wsnow[j];
        x[1] = snowpack.fw[j];
        x[2] = snowpack.coldc[j];
        x[3] = snowpack.ati[j];
        x[4] = snowpack.awe[j];
    }

    // snow.c:270
    public static void snow_setState(int i, int j, double[] x)
    {
        TSnowpack snowpack = S.Subcatch[i].snowpack;
        if (snowpack == null)
        {
            return;
        }
        snowpack.wsnow[j] = x[0];
        snowpack.fw[j] = x[1];
        snowpack.coldc[j] = x[2];
        snowpack.ati[j] = x[3];
        snowpack.awe[j] = x[4];
    }

    // snow.c:290
    public static void setMeltParams(int j, int k, double[] x)
    {
        int i = 0;
        if ((k >= S.SNOW_PLOWABLE) && (k <= S.SNOW_PERV))
        {
            S.Snowmelt[j].dhmin[k] = x[0] * S.UCF(S.TEMPERATURE) / S.UCF(S.RAINFALL);
            S.Snowmelt[j].dhmax[k] = x[1] * S.UCF(S.TEMPERATURE) / S.UCF(S.RAINFALL);
            S.Snowmelt[j].tbase[k] = x[2];
            if (S.UnitSystem == S.SI)
            {
                S.Snowmelt[j].tbase[k] = (9.0 / 5.0) * S.Snowmelt[j].tbase[k] + 32.0;
            }
            S.Snowmelt[j].fwfrac[k] = x[3];
            S.Snowmelt[j].wsnow[k] = x[4] / S.UCF(S.RAINDEPTH);
            x[5] = ((((x[5]) <= ((x[3] * x[4]))) ? (x[5]) : ((x[3] * x[4]))));
            S.Snowmelt[j].fwnow[k] = x[5] / S.UCF(S.RAINDEPTH);
            if (k == S.SNOW_PLOWABLE)
            {
                S.Snowmelt[j].snn = x[6];
            }
            else
            {
                S.Snowmelt[j].si[k] = x[6] / S.UCF(S.RAINDEPTH);
            }
        }
        else if (k == S.SNOW_REMOVAL)
        {
            S.Snowmelt[j].weplow = x[0] / S.UCF(S.RAINDEPTH);
            for (i = 0; i <= 4; i++)
            {
                S.Snowmelt[j].sfrac[i] = x[i + 1];
            }
            if (x[6] >= 0.0)
            {
                S.Snowmelt[j].toSubcatch = R.D2I((x[6] + 0.01));
            }
            else
            {
                S.Snowmelt[j].toSubcatch = -1;
            }
        }
    }

    // snow.c:341
    public static void snow_setMeltCoeffs(int j, double s)
    {
        int k = 0;
        for (k = S.SNOW_PLOWABLE; k <= S.SNOW_PERV; k++)
        {
            S.Snowmelt[j].dhm[k] = 0.5 * (S.Snowmelt[j].dhmax[k] * (1.0 + s) + S.Snowmelt[j].dhmin[k] * (1.0 - s));
        }
    }

    // snow.c:360
    public static void snow_plowSnow(int j, double tStep)
    {
        int i = 0;
        int k = 0;
        int m = 0;
        double rainfall = 0.0;
        double snowfall = 0.0;
        double exc = 0.0;
        double f = 0.0;
        double sfracTotal = 0.0;
        TSnowpack snowpack = null;
        snowpack = S.Subcatch[j].snowpack;
        if (!(snowpack != null))
        {
            return;
        }
        S.gage_getPrecip(S.Subcatch[j].gage, ref rainfall, ref snowfall);
        for (i = S.SNOW_PLOWABLE; i <= S.SNOW_PERV; i++)
        {
            if (snowpack.fArea[i] > 0.0)
            {
                snowpack.wsnow[i] += snowfall * tStep;
                snowpack.imelt[i] = 0.0;
            }
        }
        if (snowpack.fArea[S.SNOW_PLOWABLE] > 0.0)
        {
            k = snowpack.snowmeltIndex;
            if (snowpack.wsnow[S.SNOW_PLOWABLE] >= S.Snowmelt[k].weplow)
            {
                exc = snowpack.wsnow[S.SNOW_PLOWABLE];
                f = snowpack.fArea[S.SNOW_PLOWABLE] * (S.Subcatch[j].area - S.Subcatch[j].lidArea);
                S.Snow.removed += S.Snowmelt[k].sfrac[0] * exc * f;
                sfracTotal = S.Snowmelt[k].sfrac[0];
                if (snowpack.fArea[S.SNOW_IMPERV] > 0.0)
                {
                    f = snowpack.fArea[S.SNOW_PLOWABLE] / snowpack.fArea[S.SNOW_IMPERV];
                    snowpack.wsnow[S.SNOW_IMPERV] += S.Snowmelt[k].sfrac[1] * exc * f;
                    sfracTotal += S.Snowmelt[k].sfrac[1];
                }
                if (snowpack.fArea[S.SNOW_PERV] > 0.0)
                {
                    f = snowpack.fArea[S.SNOW_PLOWABLE] / snowpack.fArea[S.SNOW_PERV];
                    snowpack.wsnow[S.SNOW_PERV] += S.Snowmelt[k].sfrac[2] * exc * f;
                    sfracTotal += S.Snowmelt[k].sfrac[2];
                }
                snowpack.imelt[S.SNOW_PLOWABLE] = S.Snowmelt[k].sfrac[3] * exc / tStep;
                sfracTotal += S.Snowmelt[k].sfrac[3];
                if (S.Snowmelt[k].sfrac[4] > 0.0)
                {
                    m = S.Snowmelt[k].toSubcatch;
                    if ((S.Subcatch[m].snowpack != null))
                    {
                        f = S.Subcatch[m].snowpack.fArea[S.SNOW_PERV];
                    }
                    else
                    {
                        f = 0.0;
                    }
                    if (f > 0.0)
                    {
                        f = snowpack.fArea[S.SNOW_PLOWABLE] / f;
                        S.Subcatch[m].snowpack.wsnow[S.SNOW_PERV] += S.Snowmelt[k].sfrac[4] * exc * f;
                        sfracTotal += S.Snowmelt[k].sfrac[4];
                    }
                }
                sfracTotal = ((((sfracTotal) <= (1.0)) ? (sfracTotal) : (1.0)));
                snowpack.wsnow[S.SNOW_PLOWABLE] = exc * (1.0 - sfracTotal);
            }
        }
    }

    // snow.c:458
    public static double snow_getSnowMelt(int j, double rainfall, double snowfall, double tStep, double[] netPrecip)
    {
        int i = 0;
        double rmelt = 0.0;
        double smelt = 0.0;
        double asc = 0.0;
        double snowDepth = 0.0;
        double impervPrecip = 0.0;
        TSnowpack snowpack = null;
        snowpack = S.Subcatch[j].snowpack;
        rmelt = S.getRainmelt(rainfall);
        for (i = S.SNOW_PLOWABLE; i <= S.SNOW_PERV; i++)
        {
            if (snowpack.wsnow[i] <= 0.001 / 12.0)
            {
                asc = 0.0;
                smelt = 0.0;
                snowpack.imelt[i] += (snowpack.wsnow[i] + snowpack.fw[i]) / tStep;
                snowpack.wsnow[i] = 0.0;
                snowpack.fw[i] = 0.0;
                snowpack.coldc[i] = 0.0;
            }
            else
            {
                asc = S.getArealDepletion(snowpack, i, snowfall, tStep);
                smelt = S.meltSnowpack(snowpack, i, rmelt, asc, snowfall, tStep);
                smelt = S.routeSnowmelt(snowpack, i, smelt, asc, rainfall, tStep);
            }
            netPrecip[i] = smelt + snowpack.imelt[i] + rainfall * (1.0 - asc);
            snowDepth += snowpack.wsnow[i] * snowpack.fArea[i];
        }
        if (S.Subcatch[j].fracImperv > 0.0)
        {
            impervPrecip = (netPrecip[S.SNOW_PLOWABLE] * snowpack.fArea[S.SNOW_PLOWABLE] + netPrecip[S.SNOW_IMPERV] * snowpack.fArea[S.SNOW_IMPERV]) / S.Subcatch[j].fracImperv;
            netPrecip[S.IMPERV0] = impervPrecip;
            netPrecip[S.IMPERV1] = impervPrecip;
        }
        return snowDepth;
    }

    // snow.c:532
    public static double snow_getSnowCover(int j)
    {
        int i = 0;
        double snowCover = 0.0;
        TSnowpack snowpack = null;
        snowpack = S.Subcatch[j].snowpack;
        if (!(snowpack != null))
        {
            return 0.0;
        }
        for (i = S.SNOW_PLOWABLE; i <= S.SNOW_PERV; i++)
        {
            snowCover += (snowpack.wsnow[i] + snowpack.fw[i]) * snowpack.fArea[i];
        }
        return snowCover * (S.Subcatch[j].area - S.Subcatch[j].lidArea);
    }

    // snow.c:555
    public static double getArealDepletion(TSnowpack snowpack, int i, double snowfall, double tStep)
    {
        int k = 0;
        double asc = 0.0;
        double si = 0.0;
        double awesi = 0.0;
        double awe = 0.0;
        double sba = 0.0;
        double sbws = 0.0;
        if (i == S.SNOW_PLOWABLE)
        {
            return 1.0;
        }
        k = snowpack.snowmeltIndex;
        si = S.Snowmelt[k].si[i];
        if ((si == 0.0) || (snowpack.wsnow[i] >= si))
        {
            snowpack.awe[i] = 1.0;
            return 1.0;
        }
        if (snowpack.wsnow[i] == 0.0)
        {
            snowpack.awe[i] = 1.0;
            return 0.0;
        }
        if (snowfall > 0.0)
        {
            awe = (snowpack.wsnow[i] - snowfall * tStep) / si;
            awe = ((((awe) >= (0.0)) ? (awe) : (0.0)));
            sba = S.getArealSnowCover(i, awe);
            sbws = awe + (0.75 * snowfall * tStep) / si;
            sbws = ((((sbws) <= (1.0)) ? (sbws) : (1.0)));
            snowpack.awe[i] = awe;
            snowpack.sba[i] = sba;
            snowpack.sbws[i] = sbws;
            return 1.0;
        }
        else
        {
            awe = snowpack.awe[i];
            sba = snowpack.sba[i];
            sbws = snowpack.sbws[i];
            awesi = snowpack.wsnow[i] / si;
            if (awesi < snowpack.awe[i])
            {
                snowpack.awe[i] = 1.0;
                asc = S.getArealSnowCover(i, awesi);
            }
            else if (awesi >= snowpack.sbws[i])
            {
                asc = 1.0;
            }
            else
            {
                asc = sba + (1.0 - sba) / (sbws - awe) * (awesi - awe);
            }
            return asc;
        }
    }

    // snow.c:636
    public static double getArealSnowCover(int i, double awesi)
    {
        int k = 0;
        int m = 0;
        double asc1 = 0.0;
        double asc2 = 0.0;
        if (i == S.SNOW_IMPERV)
        {
            k = 0;
        }
        else if (i == S.SNOW_PERV)
        {
            k = 1;
        }
        else
        {
            return 1.0;
        }
        if (awesi <= 0.0)
        {
            return 0.0;
        }
        if (awesi >= 0.9999)
        {
            return 1.0;
        }
        m = R.D2I((awesi * 10.0 + 0.00001));
        asc1 = S.Snow.adc[k][m];
        if (m >= 9)
        {
            asc2 = 1.0;
        }
        else
        {
            asc2 = S.Snow.adc[k][m + 1];
        }
        return asc1 + (asc2 - asc1) / 0.1 * (awesi - 0.1 * (float)(m));
    }

    // snow.c:674
    public static double meltSnowpack(TSnowpack snowpack, int i, double rmelt, double asc, double snowfall, double tStep)
    {
        int k = 0;
        double smelt = 0.0;
        double ccFactor = 0.0;
        k = snowpack.snowmeltIndex;
        if (rmelt > 0.0)
        {
            smelt = rmelt;
        }
        else if (S.Temp.ta >= S.Snowmelt[k].tbase[i])
        {
            smelt = S.Snowmelt[k].dhm[i] * (S.Temp.ta - S.Snowmelt[k].tbase[i]);
        }
        else
        {
            S.updateColdContent(snowpack, i, asc, snowfall, tStep);
            return 0.0;
        }
        smelt *= asc;
        ccFactor = tStep * S.Snow.rnm * asc;
        smelt = S.reduceColdContent(snowpack, i, smelt, ccFactor);
        snowpack.ati[i] = S.Snowmelt[k].tbase[i];
        return smelt;
    }

    // snow.c:720
    public static double getRainmelt(double rainfall)
    {
        double uadj = 0.0;
        double t1 = 0.0;
        double t2 = 0.0;
        double t3 = 0.0;
        double smelt = 0.0;
        rainfall = rainfall * 43200.0;
        if (rainfall > 0.02)
        {
            uadj = 0.006 * S.Wind.ws;
            t1 = S.Temp.ta - 32.0;
            t2 = 7.5 * S.Temp.gamma * uadj;
            t3 = 8.5 * uadj * (S.Temp.ea - 0.18);
            smelt = t1 * (0.001167 + t2 + 0.007 * rainfall) + t3;
            return smelt / 43200.0;
        }
        else
        {
            return 0.0;
        }
    }

    // snow.c:746
    public static void updateColdContent(TSnowpack snowpack, int i, double asc, double snowfall, double tStep)
    {
        int k = 0;
        double ati = 0.0;
        double cc = 0.0;
        double ccMax = 0.0;
        double tipm = 0.0;
        ati = snowpack.ati[i];
        cc = snowpack.coldc[i];
        if (snowfall * 43200.0 > 0.02)
        {
            ati = S.Temp.ta;
        }
        else
        {
            tipm = 1.0 - Math.Pow(1.0 - S.Snow.tipm, tStep / (6.0 * 3600.0));
            ati += tipm * (S.Temp.ta - ati);
        }
        k = snowpack.snowmeltIndex;
        ati = ((((ati) <= (S.Snowmelt[k].tbase[i])) ? (ati) : (S.Snowmelt[k].tbase[i])));
        cc += S.Snow.rnm * S.Snowmelt[k].dhm[i] * (ati - S.Temp.ta) * tStep * asc;
        cc = ((((cc) >= (0.0)) ? (cc) : (0.0)));
        ccMax = snowpack.wsnow[i] * 0.007 / 12.0 * (S.Snowmelt[k].tbase[i] - ati);
        cc = ((((cc) <= (ccMax)) ? (cc) : (ccMax)));
        snowpack.coldc[i] = cc;
        snowpack.ati[i] = ati;
    }

    // snow.c:799
    public static double reduceColdContent(TSnowpack snowpack, int i, double smelt, double ccFactor)
    {
        double cc = 0.0;
        cc = snowpack.coldc[i];
        if (smelt * ccFactor > cc)
        {
            smelt -= cc / ccFactor;
            cc = 0.0;
        }
        else
        {
            cc -= smelt * ccFactor;
            smelt = 0.0;
        }
        snowpack.coldc[i] = cc;
        return smelt;
    }

    // snow.c:828
    public static double routeSnowmelt(TSnowpack snowpack, int i, double smelt, double asc, double rainfall, double tStep)
    {
        int k = 0;
        double vmelt = 0.0;
        k = snowpack.snowmeltIndex;
        vmelt = smelt * tStep;
        vmelt = ((((vmelt) <= (snowpack.wsnow[i])) ? (vmelt) : (snowpack.wsnow[i])));
        snowpack.wsnow[i] -= vmelt;
        snowpack.fw[i] += vmelt + rainfall * tStep * asc;
        vmelt = snowpack.fw[i] - S.Snowmelt[k].fwfrac[i] * snowpack.wsnow[i];
        vmelt = ((((vmelt) >= (0.0)) ? (vmelt) : (0.0)));
        snowpack.fw[i] -= vmelt;
        return vmelt / tStep;
    }

}
}
