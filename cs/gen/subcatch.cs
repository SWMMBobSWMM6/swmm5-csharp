// Generated from EPA SWMM 5.2.4 (subcatch.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double MCOEFF = 1.49;
    public static double MEXP = 1.6666667;
    public static double ODETOL = 0.0001;
    public static double Vevap = 0.0;
    public static double Vpevap = 0.0;
    public static double Vinfil = 0.0;
    public static double Vinflow = 0.0;
    public static double Voutflow = 0.0;
    public static double VlidIn = 0.0;
    public static double VlidInfil = 0.0;
    public static double VlidOut = 0.0;
    public static double VlidDrain = 0.0;
    public static double VlidReturn = 0.0;
    public static TSubarea theSubarea = null;
    public static double Dstore = 0.0;
    public static double Alpha = 0.0;
    public static CPtr[] RunoffRoutingWords = new CPtr[] { S.L663, S.L2, S.L3, CPtr.Null };

    // subcatch.c:119
    public static int subcatch_readParams(int j, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int k = 0;
        int m = 0;
        CPtr id = CPtr.Null;
        double[] x = new double[9];
        if (ntoks < 8)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.SUBCATCH, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        k = S.project_findObject(S.GAGE, tok[1]);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        x[0] = k;
        m = S.project_findObject(S.NODE, tok[2]);
        x[1] = m;
        m = S.project_findObject(S.SUBCATCH, tok[2]);
        x[2] = m;
        if ((x[1] < 0.0) && (x[2] < 0.0))
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        for (i = 3; i < 8; i++)
        {
            if ((!((S.getDouble(tok[i], ref x[i]) != 0))) || (x[i] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i]);
            }
        }
        x[8] = -1;
        if (ntoks > 8)
        {
            k = S.project_findObject(S.SNOWMELT, tok[8]);
            if (k < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[8]);
            }
            x[8] = k;
        }
        S.Subcatch[j].ID = id;
        S.Subcatch[j].gage = R.D2I(x[0]);
        S.Subcatch[j].outNode = R.D2I(x[1]);
        S.Subcatch[j].outSubcatch = R.D2I(x[2]);
        S.Subcatch[j].area = x[3] / S.UCF(S.LANDAREA);
        S.Subcatch[j].fracImperv = ((((x[4]) <= (100.0)) ? (x[4]) : (100.0))) / 100.0;
        S.Subcatch[j].width = x[5] / S.UCF(S.LENGTH);
        S.Subcatch[j].slope = x[6] / 100.0;
        S.Subcatch[j].curbLength = x[7];
        S.Subcatch[j].nPervPattern = -1;
        S.Subcatch[j].dStorePattern = -1;
        S.Subcatch[j].infilPattern = -1;
        if (x[8] >= 0)
        {
            if (!((S.snow_createSnowpack(j, R.D2I(x[8])) != 0)))
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
        }
        return 0;
    }

    // subcatch.c:196
    public static int subcatch_readSubareaParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int m = 0;
        double[] x = new double[7];
        if (ntoks < 7)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.SUBCATCH, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        for (i = 0; i < 5; i++)
        {
            if ((!((S.getDouble(tok[i + 1], ref x[i]) != 0))) || (x[i] < 0.0))
            {
                return S.error_setInpError(S.ERR_NAME, tok[i + 1]);
            }
        }
        m = S.findmatch(tok[6], S.RunoffRoutingWords);
        if (m < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[6]);
        }
        x[5] = m;
        x[6] = 1.0;
        if (ntoks >= 8)
        {
            if (((!((S.getDouble(tok[7], ref x[6]) != 0))) || (x[6] < 0.0)) || (x[6] > 100.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[7]);
            }
            x[6] /= 100.0;
        }
        S.Subcatch[j].subArea[S.IMPERV0].N = x[0];
        S.Subcatch[j].subArea[S.IMPERV1].N = x[0];
        S.Subcatch[j].subArea[S.PERV].N = x[1];
        S.Subcatch[j].subArea[S.IMPERV0].dStore = 0.0;
        S.Subcatch[j].subArea[S.IMPERV1].dStore = x[2] / S.UCF(S.RAINDEPTH);
        S.Subcatch[j].subArea[S.PERV].dStore = x[3] / S.UCF(S.RAINDEPTH);
        S.Subcatch[j].subArea[S.IMPERV0].fArea = S.Subcatch[j].fracImperv * x[4] / 100.0;
        S.Subcatch[j].subArea[S.IMPERV1].fArea = S.Subcatch[j].fracImperv * (1.0 - x[4] / 100.0);
        S.Subcatch[j].subArea[S.PERV].fArea = (1.0 - S.Subcatch[j].fracImperv);
        for (i = S.IMPERV0; i <= S.PERV; i++)
        {
            S.Subcatch[j].subArea[i].routeTo = S.TO_OUTLET;
            S.Subcatch[j].subArea[i].fOutlet = 1.0;
        }
        k = R.D2I(x[5]);
        if ((S.Subcatch[j].fracImperv == 0.0) || (S.Subcatch[j].fracImperv == 1.0))
        {
            k = S.TO_OUTLET;
        }
        if ((k == S.TO_IMPERV) && (S.Subcatch[j].fracImperv != 0.0))
        {
            S.Subcatch[j].subArea[S.PERV].routeTo = k;
            S.Subcatch[j].subArea[S.PERV].fOutlet = 1.0 - x[6];
        }
        if (k == S.TO_PERV)
        {
            S.Subcatch[j].subArea[S.IMPERV0].routeTo = k;
            S.Subcatch[j].subArea[S.IMPERV1].routeTo = k;
            S.Subcatch[j].subArea[S.IMPERV0].fOutlet = 1.0 - x[6];
            S.Subcatch[j].subArea[S.IMPERV1].fOutlet = 1.0 - x[6];
        }
        return 0;
    }

    // subcatch.c:284
    public static int subcatch_readLanduseParams(CPtr[] tok, int ntoks)
    {
        int j = 0;
        int k = 0;
        int m = 0;
        double f = 0.0;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.SUBCATCH, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        for (k = 2; k <= ntoks; k = k + 2)
        {
            m = S.project_findObject(S.LANDUSE, tok[k - 1]);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[k - 1]);
            }
            if (k + 1 > ntoks)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (!((S.getDouble(tok[k], ref f) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[k]);
            }
            S.Subcatch[j].landFactor[m].fraction = f / 100.0;
        }
        return 0;
    }

    // subcatch.c:324
    public static int subcatch_readInitBuildup(CPtr[] tok, int ntoks)
    {
        int j = 0;
        int k = 0;
        int m = 0;
        double x = 0.0;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.SUBCATCH, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        for (k = 2; k <= ntoks; k = k + 2)
        {
            m = S.project_findObject(S.POLLUT, tok[k - 1]);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[k - 1]);
            }
            if (k + 1 > ntoks)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (!((S.getDouble(tok[k], ref x) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[k]);
            }
            S.Subcatch[j].initBuildup[m] = x;
        }
        return 0;
    }

    // subcatch.c:364
    public static void subcatch_validate(int j)
    {
        int i = 0;
        double area = 0.0;
        double nonLidArea = S.Subcatch[j].area;
        if ((S.Subcatch[j].outNode >= 0) && (S.Subcatch[j].outSubcatch >= 0))
        {
            S.report_writeErrorMsg(S.ERR_SUBCATCH_OUTLET, S.Subcatch[j].ID);
        }
        S.gwater_validate(j);
        nonLidArea -= S.Subcatch[j].lidArea;
        for (i = S.IMPERV0; i <= S.PERV; i++)
        {
            if (i == S.PERV)
            {
                area = (1.0 - S.Subcatch[j].fracImperv) * nonLidArea;
            }
            else
            {
                area = S.Subcatch[j].fracImperv * nonLidArea;
            }
            S.Subcatch[j].subArea[i].alpha = 0.0;
            if ((area > 0.0) && (S.Subcatch[j].subArea[i].N > 0.0))
            {
                S.Subcatch[j].subArea[i].alpha = S.MCOEFF * S.Subcatch[j].width / area * Math.Sqrt(S.Subcatch[j].slope) / S.Subcatch[j].subArea[i].N;
            }
        }
        i = S.Subcatch[j].gage;
        if (i >= 0)
        {
            S.Gage[i].isUsed = 1;
        }
    }

    // subcatch.c:419
    public static void subcatch_initState(int j)
    {
        int i = 0;
        S.Subcatch[j].rainfall = 0.0;
        S.Subcatch[j].oldRunoff = 0.0;
        S.Subcatch[j].newRunoff = 0.0;
        S.Subcatch[j].oldSnowDepth = 0.0;
        S.Subcatch[j].newSnowDepth = 0.0;
        S.Subcatch[j].runon = 0.0;
        S.Subcatch[j].evapLoss = 0.0;
        S.Subcatch[j].infilLoss = 0.0;
        if (S.Subcatch[j].infil == j)
        {
            S.infil_initState(j);
        }
        if ((S.Subcatch[j].groundwater != null))
        {
            S.gwater_initState(j);
        }
        if ((S.Subcatch[j].snowpack != null))
        {
            S.snow_initSnowpack(j);
        }
        for (i = S.IMPERV0; i <= S.PERV; i++)
        {
            S.Subcatch[j].subArea[i].depth = 0.0;
            S.Subcatch[j].subArea[i].inflow = 0.0;
            S.Subcatch[j].subArea[i].runoff = 0.0;
        }
        S.surfqual_initState(j);
    }

    // subcatch.c:457
    public static void subcatch_setOldState(int j)
    {
        int i = 0;
        S.Subcatch[j].oldRunoff = S.Subcatch[j].newRunoff;
        S.Subcatch[j].oldSnowDepth = S.Subcatch[j].newSnowDepth;
        for (i = S.IMPERV0; i <= S.PERV; i++)
        {
            S.Subcatch[j].subArea[i].inflow = 0.0;
        }
        for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
        {
            S.Subcatch[j].oldQual[i] = S.Subcatch[j].newQual[i];
            S.Subcatch[j].newQual[i] = 0.0;
        }
        S.lid_setOldGroupState(j);
    }

    // subcatch.c:481
    public static double subcatch_getFracPerv(int j)
    {
        double fracPerv = 1.0 - S.Subcatch[j].fracImperv;
        if (S.Subcatch[j].lidArea > 0.0)
        {
            fracPerv = (fracPerv * (S.Subcatch[j].area - S.Subcatch[j].lidArea) + S.lid_getPervArea(j)) / S.Subcatch[j].area;
            fracPerv = ((((fracPerv) <= (1.0)) ? (fracPerv) : (1.0)));
        }
        return fracPerv;
    }

    // subcatch.c:502
    public static double subcatch_getStorage(int j)
    {
        int i = 0;
        double v = 0.0;
        for (i = S.IMPERV0; i <= S.PERV; i++)
        {
            v += S.Subcatch[j].subArea[i].depth * S.Subcatch[j].subArea[i].fArea;
        }
        return v * (S.Subcatch[j].area - S.Subcatch[j].lidArea) + S.lid_getStoredVolume(j);
    }

    // subcatch.c:523
    public static void subcatch_getRunon(int j)
    {
        int k = 0;
        int p = 0;
        double q = 0.0;
        double q1 = 0.0;
        double q2 = 0.0;
        double pervArea = 0.0;
        k = S.Subcatch[j].outSubcatch;
        q = S.Subcatch[j].oldRunoff;
        if ((k >= 0) && (k != j))
        {
            S.subcatch_addRunonFlow(k, q);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                S.Subcatch[k].newQual[p] += q * S.Subcatch[j].oldQual[p] * 28.317;
            }
        }
        if (S.Subcatch[j].lidArea > 0.0)
        {
            S.lid_addDrainRunon(j);
        }
        if ((S.Subcatch[j].fracImperv < 1.0) && (S.Subcatch[j].subArea[S.IMPERV0].routeTo == S.TO_PERV))
        {
            q1 = S.Subcatch[j].subArea[S.IMPERV0].runoff * S.Subcatch[j].subArea[S.IMPERV0].fArea;
            q2 = S.Subcatch[j].subArea[S.IMPERV1].runoff * S.Subcatch[j].subArea[S.IMPERV1].fArea;
            q = q1 + q2;
            S.Subcatch[j].subArea[S.PERV].inflow += q * (1.0 - S.Subcatch[j].subArea[S.IMPERV0].fOutlet) / S.Subcatch[j].subArea[S.PERV].fArea;
        }
        if (((S.Subcatch[j].fracImperv > 0.0) && (S.Subcatch[j].subArea[S.PERV].routeTo == S.TO_IMPERV)) && (S.Subcatch[j].subArea[S.IMPERV1].fArea > 0.0))
        {
            q = S.Subcatch[j].subArea[S.PERV].runoff;
            S.Subcatch[j].subArea[S.IMPERV1].inflow += q * (1.0 - S.Subcatch[j].subArea[S.PERV].fOutlet) * S.Subcatch[j].subArea[S.PERV].fArea / S.Subcatch[j].subArea[S.IMPERV1].fArea;
        }
        if ((S.Subcatch[j].lidArea > 0.0) && (S.Subcatch[j].fracImperv < 1.0))
        {
            pervArea = S.Subcatch[j].subArea[S.PERV].fArea * (S.Subcatch[j].area - S.Subcatch[j].lidArea);
            q = S.lid_getFlowToPerv(j);
            if (pervArea > 0.0)
            {
                S.Subcatch[j].subArea[S.PERV].inflow += q / pervArea;
            }
        }
    }

    // subcatch.c:600
    public static void subcatch_addRunonFlow(int k, double q)
    {
        int i = 0;
        double nonLidArea = 0.0;
        if (S.Subcatch[k].area <= 0.0)
        {
            return;
        }
        nonLidArea = S.Subcatch[k].area - S.Subcatch[k].lidArea;
        if (nonLidArea > 0.0)
        {
            q = q / nonLidArea;
        }
        else
        {
            q = q / S.Subcatch[k].area;
        }
        S.Subcatch[k].runon += q;
        for (i = S.IMPERV0; i <= S.PERV; i++)
        {
            S.Subcatch[k].subArea[i].inflow += q;
        }
    }

    // subcatch.c:629
    public static double subcatch_getRunoff(int j, double tStep)
    {
        int i = 0;
        double nonLidArea = 0.0;
        double area = 0.0;
        double[] netPrecip = new double[3];
        double vRain = 0.0;
        double vRunon = 0.0;
        double vOutflow = 0.0;
        double runoff = 0.0;
        double evapRate = 0.0;
        double subAreaRunoff = 0.0;
        double vImpervRunoff = 0.0;
        double vPervRunoff = 0.0;
        S.Vevap = 0.0;
        S.Vpevap = 0.0;
        S.Vinfil = 0.0;
        S.Voutflow = 0.0;
        S.VlidIn = 0.0;
        S.VlidInfil = 0.0;
        S.VlidOut = 0.0;
        S.VlidDrain = 0.0;
        S.VlidReturn = 0.0;
        nonLidArea = S.Subcatch[j].area - S.Subcatch[j].lidArea;
        vRunon = S.Subcatch[j].runon * tStep * nonLidArea;
        S.Vinflow = vRunon + S.subcatch_getDepth(j) * nonLidArea;
        if (nonLidArea == 0.0)
        {
            vRunon = S.Subcatch[j].runon * tStep * S.Subcatch[j].area;
        }
        S.getNetPrecip(j, netPrecip, tStep);
        if ((S.Evap.dryOnly != 0) && (S.Subcatch[j].rainfall > 0.0))
        {
            evapRate = 0.0;
        }
        else
        {
            evapRate = S.Evap.rate;
        }
        S.infil_setInfilFactor(j);
        if (nonLidArea > 0.0)
        {
            for (i = S.IMPERV0; i <= S.PERV; i++)
            {
                area = nonLidArea * S.Subcatch[j].subArea[i].fArea;
                S.Subcatch[j].subArea[i].runoff = S.getSubareaRunoff(j, i, area, netPrecip[i], evapRate, tStep);
                subAreaRunoff = S.Subcatch[j].subArea[i].runoff * area;
                if (i == S.PERV)
                {
                    vPervRunoff = subAreaRunoff * tStep;
                }
                else
                {
                    vImpervRunoff += subAreaRunoff * tStep;
                }
                runoff += subAreaRunoff;
            }
        }
        if (S.Subcatch[j].lidArea > 0.0)
        {
            S.lid_getRunoff(j, tStep);
        }
        if ((!(S.IgnoreGwater != 0)) && (S.Subcatch[j].groundwater != null))
        {
            S.gwater_getGroundwater(j, S.Vpevap, S.Vinfil + S.VlidInfil, tStep);
        }
        area = S.Subcatch[j].area;
        S.Subcatch[j].evapLoss = S.Vevap / tStep / area;
        S.Subcatch[j].infilLoss = (S.Vinfil + S.VlidInfil) / tStep / area;
        vOutflow = S.Voutflow - S.VlidIn + S.VlidOut;
        S.Subcatch[j].newRunoff = vOutflow / tStep;
        vRain = S.Subcatch[j].rainfall * tStep * area;
        S.stats_updateSubcatchStats(j, vRain, vRunon, S.Vevap, S.Vinfil + S.VlidInfil, vImpervRunoff, vPervRunoff, vOutflow + S.VlidDrain, S.Subcatch[j].newRunoff + S.VlidDrain / tStep);
        if ((S.Subcatch[j].outNode == -1) && (S.Subcatch[j].outSubcatch != j))
        {
            vOutflow = 0.0;
        }
        S.massbal_updateRunoffTotals(S.RUNOFF_RAINFALL, vRain);
        S.massbal_updateRunoffTotals(S.RUNOFF_EVAP, S.Vevap);
        S.massbal_updateRunoffTotals(S.RUNOFF_INFIL, S.Vinfil + S.VlidInfil);
        S.massbal_updateRunoffTotals(S.RUNOFF_RUNOFF, vOutflow);
        return runoff / area;
    }

    // subcatch.c:758
    public static void getNetPrecip(int j, double[] netPrecip, double tStep)
    {
        int i = 0;
        int k = 0;
        double rainfall = 0.0;
        double snowfall = 0.0;
        k = S.Subcatch[j].gage;
        if (k >= 0)
        {
            S.gage_getPrecip(k, ref rainfall, ref snowfall);
        }
        S.Subcatch[j].rainfall = rainfall + snowfall;
        if ((S.Subcatch[j].snowpack != null) && (!(S.IgnoreSnowmelt != 0)))
        {
            S.Subcatch[j].newSnowDepth = S.snow_getSnowMelt(j, rainfall, snowfall, tStep, netPrecip);
        }
        else
        {
            for (i = S.IMPERV0; i <= S.PERV; i++)
            {
                netPrecip[i] = rainfall + snowfall;
            }
        }
    }

    // subcatch.c:798
    public static double subcatch_getDepth(int j)
    {
        int i = 0;
        double fArea = 0.0;
        double depth = 0.0;
        for (i = S.IMPERV0; i <= S.PERV; i++)
        {
            fArea = S.Subcatch[j].subArea[i].fArea;
            if (fArea > 0.0)
            {
                depth += S.Subcatch[j].subArea[i].depth * fArea;
            }
        }
        return depth;
    }

    // subcatch.c:820
    public static double subcatch_getWtdOutflow(int j, double f)
    {
        if (S.Subcatch[j].area == 0.0)
        {
            return 0.0;
        }
        return (1.0 - f) * S.Subcatch[j].oldRunoff + f * S.Subcatch[j].newRunoff;
    }

    // subcatch.c:834
    public static void subcatch_getResults(int j, double f, float[] x)
    {
        int p = 0;
        int k = 0;
        double f1 = 1.0 - f;
        double z = 0.0;
        double runoff = 0.0;
        TGroundwater gw = null;
        k = S.Subcatch[j].gage;
        if (k >= 0)
        {
            x[S.SUBCATCH_RAINFALL] = (float)(S.Gage[k].reportRainfall);
        }
        else
        {
            x[S.SUBCATCH_RAINFALL] = 0.0f;
        }
        z = (f1 * S.Subcatch[j].oldSnowDepth + f * S.Subcatch[j].newSnowDepth) * S.UCF(S.RAINDEPTH);
        x[S.SUBCATCH_SNOWDEPTH] = (float)(z);
        x[S.SUBCATCH_EVAP] = (float)((S.Subcatch[j].evapLoss * S.UCF(S.EVAPRATE)));
        x[S.SUBCATCH_INFIL] = (float)((S.Subcatch[j].infilLoss * S.UCF(S.RAINFALL)));
        runoff = f1 * S.Subcatch[j].oldRunoff + f * S.Subcatch[j].newRunoff;
        if (S.Subcatch[j].lidArea > 0.0)
        {
            runoff += f1 * S.lid_getDrainFlow(j, S.PREVIOUS) + f * S.lid_getDrainFlow(j, S.CURRENT);
        }
        if (runoff < 2.31481e-08 * S.Subcatch[j].area)
        {
            runoff = 0.0;
        }
        x[S.SUBCATCH_RUNOFF] = (float)((runoff * S.UCF(S.FLOW)));
        gw = S.Subcatch[j].groundwater;
        if ((gw != null))
        {
            z = (f1 * gw.oldFlow + f * gw.newFlow) * S.Subcatch[j].area * S.UCF(S.FLOW);
            x[S.SUBCATCH_GW_FLOW] = (float)(z);
            z = (gw.bottomElev + gw.lowerDepth) * S.UCF(S.LENGTH);
            x[S.SUBCATCH_GW_ELEV] = (float)(z);
            z = gw.theta;
            x[S.SUBCATCH_SOIL_MOIST] = (float)(z);
        }
        else
        {
            x[S.SUBCATCH_GW_FLOW] = 0.0f;
            x[S.SUBCATCH_GW_ELEV] = 0.0f;
            x[S.SUBCATCH_SOIL_MOIST] = 0.0f;
        }
        if (!(S.IgnoreQuality != 0))
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                if (runoff == 0.0)
                {
                    z = 0.0;
                }
                else
                {
                    z = f1 * S.Subcatch[j].oldQual[p] + f * S.Subcatch[j].newQual[p];
                }
                x[S.SUBCATCH_WASHOFF + p] = (float)(z);
            }
        }
    }

    // subcatch.c:907
    public static double getSubareaRunoff(int j, int i, double area, double precip, double evap, double tStep)
    {
        double tRunoff = 0.0;
        double surfMoisture = 0.0;
        double surfEvap = 0.0;
        double infil = 0.0;
        double runoff = 0.0;
        TSubarea subarea = null;
        if (area == 0.0)
        {
            return 0.0;
        }
        subarea = S.Subcatch[j].subArea[i];
        tRunoff = tStep;
        surfMoisture = subarea.depth / tStep;
        surfEvap = ((((surfMoisture) <= (evap)) ? (surfMoisture) : (evap)));
        if (i == S.PERV)
        {
            infil = S.getSubareaInfil(j, subarea, precip, tStep);
        }
        subarea.inflow += precip;
        surfMoisture += subarea.inflow;
        S.Vinflow += precip * area * tStep;
        S.Vevap += surfEvap * area * tStep;
        if (i == S.PERV)
        {
            S.Vpevap += S.Vevap;
        }
        S.Vinfil += infil * area * tStep;
        S.Alpha = subarea.alpha;
        S.Dstore = subarea.dStore;
        S.adjustSubareaParams(i, j);
        if (surfEvap + infil >= surfMoisture)
        {
            subarea.depth = 0.0;
        }
        else
        {
            subarea.inflow -= surfEvap + infil;
            S.updatePondedDepth(subarea, ref tRunoff);
        }
        runoff = S.findSubareaRunoff(subarea, tRunoff);
        S.Voutflow += subarea.fOutlet * runoff * area * tStep;
        return runoff;
    }

    // subcatch.c:985
    public static double getSubareaInfil(int j, TSubarea subarea, double precip, double tStep)
    {
        double infil = 0.0;
        infil = S.infil_getInfil(j, tStep, precip, subarea.inflow, subarea.depth);
        if ((!(S.IgnoreGwater != 0)) && (S.Subcatch[j].groundwater != null))
        {
            infil = ((((infil) <= (S.Subcatch[j].groundwater.maxInfilVol / tStep)) ? (infil) : (S.Subcatch[j].groundwater.maxInfilVol / tStep)));
        }
        return infil;
    }

    // subcatch.c:1013
    public static double findSubareaRunoff(TSubarea subarea, double tRunoff)
    {
        double xDepth = subarea.depth - S.Dstore;
        double runoff = 0.0;
        if (xDepth > 1e-10)
        {
            if (subarea.N > 0.0)
            {
                runoff = S.Alpha * Math.Pow(xDepth, S.MEXP);
            }
            else
            {
                runoff = xDepth / tRunoff;
                subarea.depth = S.Dstore;
            }
        }
        else
        {
            runoff = 0.0;
        }
        return runoff;
    }

    // subcatch.c:1048
    public static void updatePondedDepth(TSubarea subarea, ref double dt)
    {
        double ix = subarea.inflow;
        double dx = 0.0;
        double tx = dt;
        if (subarea.depth + ix * tx <= S.Dstore)
        {
            subarea.depth += ix * tx;
        }
        else
        {
            dx = S.Dstore - subarea.depth;
            if ((dx > 0.0) && (ix > 0.0))
            {
                tx -= dx / ix;
                subarea.depth = S.Dstore;
            }
            if ((S.Alpha > 0.0) && (tx > 0.0))
            {
                S.theSubarea = subarea;
                R.Arr1(ref subarea.depth, __a => S.odesolve_integrate(__a, 1, 0, tx, S.ODETOL, tx, S.getDdDt));
            }
            else
            {
                if (tx < 0.0)
                {
                    tx = 0.0;
                }
                subarea.depth += ix * tx;
            }
        }
        if (subarea.depth < 0.0)
        {
            subarea.depth = 0.0;
        }
        dt = tx;
    }

    // subcatch.c:1101
    public static void getDdDt(double t, double[] d, double[] dddt)
    {
        double ix = S.theSubarea.inflow;
        double rx = d[0] - S.Dstore;
        if (rx < 0.0)
        {
            rx = 0.0;
        }
        else
        {
            rx = S.Alpha * Math.Pow(rx, S.MEXP);
        }
        dddt[0] = ix - rx;
    }

    // subcatch.c:1125
    public static void adjustSubareaParams(int i, int j)
    {
        int p = 0;
        int m = 0;
        double f = 0.0;
        if (i == S.PERV)
        {
            p = S.Subcatch[j].dStorePattern;
            if ((p >= 0) && (S.Pattern[p].type == S.MONTHLY_PATTERN))
            {
                m = S.datetime_monthOfYear(S.getDateTime(S.OldRunoffTime)) - 1;
                f = S.Pattern[p].factor[m];
                if (f >= 0.0)
                {
                    S.Dstore *= f;
                }
            }
            p = S.Subcatch[j].nPervPattern;
            if ((p >= 0) && (S.Pattern[p].type == S.MONTHLY_PATTERN))
            {
                m = S.datetime_monthOfYear(S.getDateTime(S.OldRunoffTime)) - 1;
                f = S.Pattern[p].factor[m];
                if (f <= 0.0)
                {
                    S.Alpha = 0.0;
                }
                else
                {
                    S.Alpha /= f;
                }
            }
        }
    }

}
}
