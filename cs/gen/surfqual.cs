// Generated from EPA SWMM 5.2.4 (surfqual.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // surfqual.c:65
    public static void surfqual_initState(int j)
    {
        int p = 0;
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.Subcatch[j].oldQual[p] = 0.0;
            S.Subcatch[j].newQual[p] = 0.0;
            S.Subcatch[j].pondedQual[p] = 0.0;
        }
        S.landuse_getInitBuildup(S.Subcatch[j].landFactor, S.Subcatch[j].initBuildup, S.Subcatch[j].area, S.Subcatch[j].curbLength);
    }

    // surfqual.c:89
    public static void surfqual_getBuildup(int j, double tStep)
    {
        int i = 0;
        int p = 0;
        double f = 0.0;
        double area = 0.0;
        double curb = 0.0;
        double oldBuildup = 0.0;
        double newBuildup = 0.0;
        for (i = 0; i < S.Nobjects[S.LANDUSE]; i++)
        {
            f = S.Subcatch[j].landFactor[i].fraction;
            if (f == 0.0)
            {
                continue;
            }
            area = f * S.Subcatch[j].area * S.UCF(S.LANDAREA);
            curb = f * S.Subcatch[j].curbLength;
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                if ((S.Pollut[p].snowOnly != 0) && (S.Subcatch[j].newSnowDepth < 0.001 / 12.0))
                {
                    continue;
                }
                oldBuildup = S.Subcatch[j].landFactor[i].buildup[p];
                newBuildup = S.landuse_getBuildup(i, p, area, curb, oldBuildup, tStep);
                newBuildup = ((((newBuildup) >= (oldBuildup)) ? (newBuildup) : (oldBuildup)));
                S.Subcatch[j].landFactor[i].buildup[p] = newBuildup;
                S.massbal_updateLoadingTotals(S.BUILDUP_LOAD, p, (newBuildup - oldBuildup));
            }
        }
    }

    // surfqual.c:137
    public static void surfqual_sweepBuildup(int j, double aDate)
    {
        int i = 0;
        int p = 0;
        double oldBuildup = 0.0;
        double newBuildup = 0.0;
        if ((S.Subcatch[j].snowpack != null) && (S.Subcatch[j].snowpack.wsnow[S.IMPERV0] > 0.004167))
        {
            return;
        }
        for (i = 0; i < S.Nobjects[S.LANDUSE]; i++)
        {
            if (S.Subcatch[j].landFactor[i].fraction == 0.0)
            {
                continue;
            }
            if (S.Landuse[i].sweepInterval == 0.0)
            {
                continue;
            }
            if (aDate - S.Subcatch[j].landFactor[i].lastSwept >= S.Landuse[i].sweepInterval)
            {
                S.Subcatch[j].landFactor[i].lastSwept = aDate;
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    oldBuildup = S.Subcatch[j].landFactor[i].buildup[p];
                    newBuildup = oldBuildup * (1.0 - S.Landuse[i].sweepRemoval * S.Landuse[i].washoffFunc[p].sweepEffic);
                    newBuildup = ((((oldBuildup) <= (newBuildup)) ? (oldBuildup) : (newBuildup)));
                    newBuildup = ((((0.0) >= (newBuildup)) ? (0.0) : (newBuildup)));
                    S.Subcatch[j].landFactor[i].buildup[p] = newBuildup;
                    S.massbal_updateLoadingTotals(S.SWEEPING_LOAD, p, oldBuildup - newBuildup);
                }
            }
        }
    }

    // surfqual.c:192
    public static void surfqual_getWashoff(int j, double runoff, double tStep)
    {
        int p = 0;
        int hasOutflow = 0;
        double cOut = 0.0;
        double massLoad = 0.0;
        double vLidRain = 0.0;
        double vLidRunon = 0.0;
        double vSurfOut = 0.0;
        double vOut1 = 0.0;
        double vOut2 = 0.0;
        double area = 0.0;
        area = S.Subcatch[j].area;
        if ((S.Nobjects[S.POLLUT] == 0) || (area == 0.0))
        {
            return;
        }
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.OutflowLoad[p] = 0.0;
        }
        S.findWashoffLoads(j, runoff);
        S.findPondedLoads(j, tStep);
        S.findLidLoads(j, tStep);
        vLidRain = S.Subcatch[j].rainfall * S.Subcatch[j].lidArea * tStep;
        vLidRunon = 0.0;
        if (area == S.Subcatch[j].lidArea)
        {
            vLidRunon = S.Subcatch[j].runon * area * tStep;
        }
        vOut1 = S.Voutflow + vLidRain + vLidRunon;
        vSurfOut = S.Subcatch[j].newRunoff * tStep;
        vOut2 = vSurfOut + S.VlidDrain;
        hasOutflow = ((vOut2 > 2.31481e-08 * area * tStep) ? 1 : 0);
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            cOut = 0.0;
            if ((vOut1 > 0.0) && (hasOutflow != 0))
            {
                cOut = S.OutflowLoad[p] / vOut1;
            }
            if (S.Subcatch[j].lidArea > 0.0)
            {
                massLoad = cOut * (vOut1 - vOut2) * S.Pollut[p].mcf;
                if (massLoad > 0.0)
                {
                    S.massbal_updateLoadingTotals(S.BMP_REMOVAL_LOAD, p, massLoad);
                }
            }
            massLoad = cOut * vOut2 * S.Pollut[p].mcf;
            S.Subcatch[j].totalLoad[p] += massLoad;
            if (((S.Subcatch[j].outNode >= 0) || (S.Subcatch[j].outSubcatch == j)))
            {
                massLoad = cOut * vSurfOut * S.Pollut[p].mcf;
                S.massbal_updateLoadingTotals(S.RUNOFF_LOAD, p, massLoad);
            }
            S.Subcatch[j].newQual[p] = cOut / 28.317;
        }
        if (S.Subcatch[j].lidArea > 0.0)
        {
            S.lid_addDrainLoads(j, S.Subcatch[j].newQual, tStep);
        }
    }

    // surfqual.c:298
    public static double surfqual_getWtdWashoff(int j, int p, double f)
    {
        return (1.0 - f) * S.Subcatch[j].oldRunoff * S.Subcatch[j].oldQual[p] + f * S.Subcatch[j].newRunoff * S.Subcatch[j].newQual[p];
    }

    // surfqual.c:313
    public static void findPondedLoads(int j, double tStep)
    {
        int p = 0;
        double cPonded = 0.0;
        double wPonded = 0.0;
        double bmpRemoval = 0.0;
        double vRain = 0.0;
        double wRain = 0.0;
        double wRunon = 0.0;
        double wInfil = 0.0;
        double wOutflow = 0.0;
        double fullArea = 0.0;
        double nonLidArea = 0.0;
        if (S.Subcatch[j].area == S.Subcatch[j].lidArea)
        {
            return;
        }
        fullArea = S.Subcatch[j].area;
        nonLidArea = fullArea - S.Subcatch[j].lidArea;
        vRain = S.Subcatch[j].rainfall * nonLidArea * tStep;
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            wRain = S.Pollut[p].pptConcen * 28.317 * vRain;
            S.massbal_updateLoadingTotals(S.DEPOSITION_LOAD, p, wRain * S.Pollut[p].mcf);
            if (S.Vinflow == 0.0)
            {
                S.massbal_updateLoadingTotals(S.FINAL_LOAD, p, S.Subcatch[j].pondedQual[p] * S.Pollut[p].mcf);
                S.Subcatch[j].pondedQual[p] = 0.0;
            }
            else
            {
                wRunon = S.Subcatch[j].newQual[p] * tStep;
                wPonded = S.Subcatch[j].pondedQual[p] + wRain + wRunon;
                cPonded = wPonded / S.Vinflow;
                wInfil = cPonded * S.Vinfil;
                wInfil = ((((wInfil) <= (wPonded)) ? (wInfil) : (wPonded)));
                S.massbal_updateLoadingTotals(S.INFIL_LOAD, p, wInfil * S.Pollut[p].mcf);
                wPonded -= wInfil;
                wOutflow = cPonded * S.Voutflow;
                wOutflow = ((((wOutflow) <= (wPonded)) ? (wOutflow) : (wPonded)));
                wPonded -= wOutflow;
                bmpRemoval = S.landuse_getAvgBmpEffic(j, p) * wOutflow;
                S.massbal_updateLoadingTotals(S.BMP_REMOVAL_LOAD, p, bmpRemoval * S.Pollut[p].mcf);
                wOutflow -= bmpRemoval;
                S.Subcatch[j].pondedQual[p] = cPonded * S.subcatch_getDepth(j) * nonLidArea;
                S.OutflowLoad[p] += wOutflow;
            }
        }
    }

    // surfqual.c:390
    public static void findWashoffLoads(int j, double runoff)
    {
        int i = 0;
        int p = 0;
        int k = 0;
        double w = 0.0;
        double area = S.Subcatch[j].area;
        if (runoff < 2.31481e-08)
        {
            return;
        }
        for (i = 0; i < S.Nobjects[S.LANDUSE]; i++)
        {
            if (S.Subcatch[j].landFactor[i].fraction > 0.0)
            {
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    S.OutflowLoad[p] += S.landuse_getWashoffLoad(i, p, area, S.Subcatch[j].landFactor, runoff, S.Voutflow);
                }
            }
        }
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            k = S.Pollut[p].coPollut;
            if (k >= 0)
            {
                w = S.Pollut[p].coFraction * S.OutflowLoad[k];
                S.massbal_updateLoadingTotals(S.BUILDUP_LOAD, p, w * S.Pollut[p].mcf);
                S.OutflowLoad[p] += w;
            }
        }
    }

    // surfqual.c:443
    public static void findLidLoads(int j, double tStep)
    {
        int p = 0;
        int useRunon = 0;
        double lidArea = 0.0;
        double vLidRain = 0.0;
        double wLidRain = 0.0;
        double wLidRunon = 0.0;
        lidArea = S.Subcatch[j].lidArea;
        if (lidArea == 0.0)
        {
            return;
        }
        vLidRain = S.Subcatch[j].rainfall * lidArea * tStep;
        useRunon = ((lidArea == S.Subcatch[j].area) ? 1 : 0);
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            wLidRain = S.Pollut[p].pptConcen * vLidRain * 28.317;
            S.massbal_updateLoadingTotals(S.DEPOSITION_LOAD, p, wLidRain * S.Pollut[p].mcf);
            if ((useRunon != 0))
            {
                wLidRunon = S.Subcatch[j].newQual[p] * tStep;
            }
            else
            {
                wLidRunon = 0.0;
            }
            S.OutflowLoad[p] += wLidRain + wLidRunon;
        }
    }

}
}
