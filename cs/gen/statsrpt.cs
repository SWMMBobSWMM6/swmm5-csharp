// Generated from EPA SWMM 5.2.4 (statsrpt.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static CPtr FlowFmt = R.Buf(6);
    public static double Vcf = 0.0;

    // statsrpt.c:84
    public static void statsrpt_writeReport()
    {
        if ((S.FlowUnits == S.MGD) || (S.FlowUnits == S.CMS))
        {
            S.sstrncpy(S.FlowFmt, S.L534, unchecked((ulong)(5)));
        }
        else
        {
            S.sstrncpy(S.FlowFmt, S.L535, unchecked((ulong)(5)));
        }
        if (S.UnitSystem == S.US)
        {
            S.Vcf = 7.48 / 1.0e6;
        }
        else
        {
            S.Vcf = 28.317 / 1.0e6;
        }
        if (S.Nobjects[S.SUBCATCH] > 0)
        {
            if (((!(S.IgnoreRainfall != 0)) || (((S.Nobjects[S.SNOWMELT] > 0) && (!(S.IgnoreSnowmelt != 0))))) || (((S.Nobjects[S.AQUIFER] > 0) && (!(S.IgnoreGwater != 0)))))
            {
                S.writeSubcatchRunoff();
                S.lid_writeWaterBalance();
                if (!(S.IgnoreGwater != 0))
                {
                    S.writeGroundwater();
                }
                if ((S.Nobjects[S.POLLUT] > 0) && (!(S.IgnoreQuality != 0)))
                {
                    S.writeSubcatchLoads();
                }
            }
        }
        if ((S.Nobjects[S.LINK] > 0) && (!(S.IgnoreRouting != 0)))
        {
            S.writeNodeDepths();
            S.writeNodeFlows();
            if (S.RouteModel == S.DW)
            {
                S.writeNodeSurcharge();
            }
            S.writeNodeFlooding();
            S.writeStorageVolumes();
            S.writeOutfallLoads();
            S.inlet_writeStatsReport();
            S.writeLinkFlows();
            S.writeFlowClass();
            S.writeLinkSurcharge();
            S.writePumpFlows();
            if ((S.Nobjects[S.POLLUT] > 0) && (!(S.IgnoreQuality != 0)))
            {
                S.writeLinkLoads();
            }
        }
    }

    // statsrpt.c:133
    public static void writeSubcatchRunoff()
    {
        int j = 0;
        double a = 0.0;
        double x = 0.0;
        double r = 0.0;
        if (S.Nobjects[S.SUBCATCH] == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L473));
        S.report_writeLine((S.L536));
        S.report_writeLine((S.L473));
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L537);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L538, S.VolUnitsWords[S.UnitSystem], S.FlowUnitWords[S.FlowUnits]);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L539, S.VolUnitsWords[S.UnitSystem], S.FlowUnitWords[S.FlowUnits]);
        }
        R.fprintf(S.Frpt.file, S.L540);
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            a = S.Subcatch[j].area;
            if (a == 0.0)
            {
                continue;
            }
            R.fprintf(S.Frpt.file, S.L541, S.Subcatch[j].ID);
            x = S.SubcatchStats[j].precip * S.UCF(S.RAINDEPTH);
            R.fprintf(S.Frpt.file, S.L542, x / a);
            x = S.SubcatchStats[j].runon * S.UCF(S.RAINDEPTH);
            R.fprintf(S.Frpt.file, S.L542, x / a);
            x = S.SubcatchStats[j].evap * S.UCF(S.RAINDEPTH);
            R.fprintf(S.Frpt.file, S.L542, x / a);
            x = S.SubcatchStats[j].infil * S.UCF(S.RAINDEPTH);
            R.fprintf(S.Frpt.file, S.L542, x / a);
            x = S.SubcatchStats[j].impervRunoff * S.UCF(S.RAINDEPTH);
            R.fprintf(S.Frpt.file, S.L542, x / a);
            x = S.SubcatchStats[j].pervRunoff * S.UCF(S.RAINDEPTH);
            R.fprintf(S.Frpt.file, S.L542, x / a);
            x = S.SubcatchStats[j].runoff * S.UCF(S.RAINDEPTH);
            R.fprintf(S.Frpt.file, S.L542, x / a);
            x = S.SubcatchStats[j].runoff * S.Vcf;
            R.fprintf(S.Frpt.file, S.L543, x);
            x = S.SubcatchStats[j].maxFlow * S.UCF(S.FLOW);
            R.fprintf(S.Frpt.file, S.L544, x);
            r = S.SubcatchStats[j].precip + S.SubcatchStats[j].runon;
            if (r > 0.0)
            {
                r = S.SubcatchStats[j].runoff / r;
            }
            R.fprintf(S.Frpt.file, S.L545, r);
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:190
    public static void writeGroundwater()
    {
        int i = 0;
        int j = 0;
        int count = 0;
        double totalSeconds = S.NewRunoffTime / 1000.0;
        double[] x = new double[9];
        if (S.Nobjects[S.SUBCATCH] == 0)
        {
            return;
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            if (S.Subcatch[j].groundwater != null)
            {
                count++;
            }
        }
        if (count == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L188));
        S.report_writeLine((S.L546));
        S.report_writeLine((S.L188));
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L547);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L548, S.FlowUnitWords[S.FlowUnits]);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L549, S.FlowUnitWords[S.FlowUnits]);
        }
        R.fprintf(S.Frpt.file, S.L550);
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            if ((S.Subcatch[j].area == 0.0) || (S.Subcatch[j].groundwater == null))
            {
                continue;
            }
            R.fprintf(S.Frpt.file, S.L541, S.Subcatch[j].ID);
            x[0] = S.Subcatch[j].groundwater.stats.infil * S.UCF(S.RAINDEPTH);
            x[1] = S.Subcatch[j].groundwater.stats.evap * S.UCF(S.RAINDEPTH);
            x[2] = S.Subcatch[j].groundwater.stats.deepFlow * S.UCF(S.RAINDEPTH);
            x[3] = S.Subcatch[j].groundwater.stats.latFlow * S.UCF(S.RAINDEPTH);
            x[4] = S.Subcatch[j].groundwater.stats.maxFlow * S.UCF(S.FLOW) * S.Subcatch[j].area;
            x[5] = S.Subcatch[j].groundwater.stats.avgUpperMoist / totalSeconds;
            x[6] = S.Subcatch[j].groundwater.stats.avgWaterTable * S.UCF(S.LENGTH) / totalSeconds;
            x[7] = S.Subcatch[j].groundwater.stats.finalUpperMoist;
            x[8] = S.Subcatch[j].groundwater.stats.finalWaterTable * S.UCF(S.LENGTH);
            for (i = 0; i < 9; i++)
            {
                R.fprintf(S.Frpt.file, S.L544, x[i]);
            }
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:245
    public static void writeSubcatchLoads()
    {
        int i = 0;
        int j = 0;
        int p = 0;
        double x = 0.0;
        double[] totals = null;
        CPtr units = R.Buf(15);
        CPtr subcatchLine = R.BufFrom(S.L551, 21);
        CPtr pollutLine = R.BufFrom(S.L552, 15);
        totals = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
        if ((totals != null))
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L553));
            S.report_writeLine((S.L554));
            S.report_writeLine((S.L553));
            S.report_writeLine((S.L0));
            R.fprintf(S.Frpt.file, S.L356, subcatchLine);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L169, pollutLine);
            }
            R.fprintf(S.Frpt.file, S.L555);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L423, S.Pollut[p].ID);
            }
            R.fprintf(S.Frpt.file, S.L556);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                i = S.UnitSystem;
                if (S.Pollut[p].units == S.COUNT)
                {
                    i = 2;
                }
                S.sstrncpy(units, S.LoadUnitsWords[i], unchecked((ulong)(14)));
                R.fprintf(S.Frpt.file, S.L423, units);
                totals[p] = 0.0;
            }
            R.fprintf(S.Frpt.file, S.L356, subcatchLine);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L169, pollutLine);
            }
            for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
            {
                R.fprintf(S.Frpt.file, S.L541, S.Subcatch[j].ID);
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    x = S.Subcatch[j].totalLoad[p];
                    totals[p] += x;
                    if (S.Pollut[p].units == S.COUNT)
                    {
                        x = (((x) > 0.0 ? Math.Log10((x)) : (x)));
                    }
                    R.fprintf(S.Frpt.file, S.L427, x);
                }
            }
            R.fprintf(S.Frpt.file, S.L356, subcatchLine);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L169, pollutLine);
            }
            R.fprintf(S.Frpt.file, S.L557);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                x = totals[p];
                if (S.Pollut[p].units == S.COUNT)
                {
                    x = (((x) > 0.0 ? Math.Log10((x)) : (x)));
                }
                R.fprintf(S.Frpt.file, S.L427, x);
            }
            R.free(totals);
            S.report_writeLine((S.L0));
        }
    }

    // statsrpt.c:310
    public static void writeNodeDepths()
    {
        int j = 0;
        int days = 0;
        int hrs = 0;
        int mins = 0;
        if (S.Nobjects[S.LINK] == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L558));
        S.report_writeLine((S.L559));
        S.report_writeLine((S.L558));
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L560);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L561);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L562);
        }
        R.fprintf(S.Frpt.file, S.L563);
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            R.fprintf(S.Frpt.file, S.L541, S.Node[j].ID);
            R.fprintf(S.Frpt.file, S.L564, S.NodeTypeWords[S.Node[j].type]);
            S.getElapsedTime(S.NodeStats[j].maxDepthDate, ref days, ref hrs, ref mins);
            R.fprintf(S.Frpt.file, S.L565, S.NodeStats[j].avgDepth / S.ReportStepCount * S.UCF(S.LENGTH), S.NodeStats[j].maxDepth * S.UCF(S.LENGTH), (S.NodeStats[j].maxDepth + S.Node[j].invertElev) * S.UCF(S.LENGTH), days, hrs, mins, S.NodeStats[j].maxRptDepth);
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:353
    public static void writeNodeFlows()
    {
        int j = 0;
        int days1 = 0;
        int hrs1 = 0;
        int mins1 = 0;
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L188));
        S.report_writeLine((S.L566));
        S.report_writeLine((S.L188));
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L567, S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits], S.VolUnitsWords[S.UnitSystem], S.VolUnitsWords[S.UnitSystem]);
        R.fprintf(S.Frpt.file, S.L568);
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            R.fprintf(S.Frpt.file, S.L541, S.Node[j].ID);
            R.fprintf(S.Frpt.file, S.L569, S.NodeTypeWords[S.Node[j].type]);
            S.getElapsedTime(S.NodeStats[j].maxInflowDate, ref days1, ref hrs1, ref mins1);
            R.fprintf(S.Frpt.file, S.FlowFmt, S.NodeStats[j].maxLatFlow * S.UCF(S.FLOW));
            R.fprintf(S.Frpt.file, S.FlowFmt, S.NodeStats[j].maxInflow * S.UCF(S.FLOW));
            R.fprintf(S.Frpt.file, S.L570, days1, hrs1, mins1);
            R.fprintf(S.Frpt.file, S.L571, S.NodeStats[j].totLatFlow * S.Vcf);
            R.fprintf(S.Frpt.file, S.L571, S.NodeInflow[j] * S.Vcf);
            if (Math.Abs(S.NodeOutflow[j]) < 1.0)
            {
                R.fprintf(S.Frpt.file, S.L572, (S.NodeInflow[j] - S.NodeOutflow[j]) * S.Vcf * 1.0e6, S.VolUnitsWords2[S.UnitSystem]);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L573, (S.NodeInflow[j] - S.NodeOutflow[j]) / S.NodeOutflow[j] * 100.0);
            }
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:403
    public static void writeNodeSurcharge()
    {
        int j = 0;
        int n = 0;
        double t = 0.0;
        double d1 = 0.0;
        double d2 = 0.0;
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L574));
        S.report_writeLine((S.L575));
        S.report_writeLine((S.L574));
        S.report_writeLine((S.L0));
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if (S.Node[j].type == S.OUTFALL)
            {
                continue;
            }
            if (S.NodeStats[j].timeSurcharged == 0.0)
            {
                continue;
            }
            t = ((((0.01) >= ((S.NodeStats[j].timeSurcharged / 3600.0))) ? (0.01) : ((S.NodeStats[j].timeSurcharged / 3600.0))));
            if (n == 0)
            {
                S.report_writeLine((S.L576));
                R.fprintf(S.Frpt.file, S.L577);
                if (S.UnitSystem == S.US)
                {
                    R.fprintf(S.Frpt.file, S.L578);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L579);
                }
                R.fprintf(S.Frpt.file, S.L580);
                n = 1;
            }
            R.fprintf(S.Frpt.file, S.L541, S.Node[j].ID);
            R.fprintf(S.Frpt.file, S.L569, S.NodeTypeWords[S.Node[j].type]);
            d1 = S.NodeStats[j].maxDepth + S.Node[j].invertElev - S.Node[j].crownElev;
            if (d1 < 0.0)
            {
                d1 = 0.0;
            }
            d2 = S.Node[j].fullDepth - S.NodeStats[j].maxDepth;
            if (d2 < 0.0)
            {
                d2 = 0.0;
            }
            R.fprintf(S.Frpt.file, S.L581, t, d1 * S.UCF(S.LENGTH), d2 * S.UCF(S.LENGTH));
        }
        if (n == 0)
        {
            S.report_writeLine((S.L582));
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:449
    public static void writeNodeFlooding()
    {
        int j = 0;
        int n = 0;
        int days = 0;
        int hrs = 0;
        int mins = 0;
        double t = 0.0;
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L255));
        S.report_writeLine((S.L583));
        S.report_writeLine((S.L255));
        S.report_writeLine((S.L0));
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if (S.Node[j].type == S.OUTFALL)
            {
                continue;
            }
            if (S.NodeStats[j].timeFlooded == 0.0)
            {
                continue;
            }
            t = ((((0.01) >= ((S.NodeStats[j].timeFlooded / 3600.0))) ? (0.01) : ((S.NodeStats[j].timeFlooded / 3600.0))));
            if (n == 0)
            {
                S.report_writeLine((S.L584));
                R.fprintf(S.Frpt.file, S.L585);
                if (S.RouteModel == S.DW)
                {
                    R.fprintf(S.Frpt.file, S.L586);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L587);
                }
                R.fprintf(S.Frpt.file, S.L588, S.FlowUnitWords[S.FlowUnits], S.VolUnitsWords[S.UnitSystem]);
                if (S.RouteModel == S.DW)
                {
                    R.fprintf(S.Frpt.file, S.L589, S.PondingUnitsWords[S.UnitSystem]);
                }
                else if (S.UnitSystem == S.US)
                {
                    R.fprintf(S.Frpt.file, S.L590);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L591);
                }
                R.fprintf(S.Frpt.file, S.L592);
                n = 1;
            }
            R.fprintf(S.Frpt.file, S.L541, S.Node[j].ID);
            R.fprintf(S.Frpt.file, S.L593, t);
            R.fprintf(S.Frpt.file, S.FlowFmt, S.NodeStats[j].maxOverflow * S.UCF(S.FLOW));
            S.getElapsedTime(S.NodeStats[j].maxOverflowDate, ref days, ref hrs, ref mins);
            R.fprintf(S.Frpt.file, S.L594, days, hrs, mins);
            R.fprintf(S.Frpt.file, S.L573, S.NodeStats[j].volFlooded * S.Vcf);
            if (S.RouteModel == S.DW)
            {
                R.fprintf(S.Frpt.file, S.L195, (S.NodeStats[j].maxDepth - S.Node[j].fullDepth) * S.UCF(S.LENGTH));
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L195, S.NodeStats[j].maxPondedVol / 1000.0 * S.UCF(S.VOLUME));
            }
        }
        if (n == 0)
        {
            S.report_writeLine((S.L595));
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:508
    public static void writeStorageVolumes()
    {
        int j = 0;
        int k = 0;
        int days = 0;
        int hrs = 0;
        int mins = 0;
        double avgVol = 0.0;
        double maxVol = 0.0;
        double pctAvgVol = 0.0;
        double pctMaxVol = 0.0;
        double pctEvapLoss = 0.0;
        double pctSeepLoss = 0.0;
        if (S.Nnodes[S.STORAGE] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L574));
            S.report_writeLine((S.L596));
            S.report_writeLine((S.L574));
            S.report_writeLine((S.L0));
            R.fprintf(S.Frpt.file, S.L597);
            if (S.UnitSystem == S.US)
            {
                R.fprintf(S.Frpt.file, S.L598);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L599);
            }
            R.fprintf(S.Frpt.file, S.L600, S.FlowUnitWords[S.FlowUnits]);
            R.fprintf(S.Frpt.file, S.L601);
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                if (S.Node[j].type != S.STORAGE)
                {
                    continue;
                }
                k = S.Node[j].subIndex;
                R.fprintf(S.Frpt.file, S.L541, S.Node[j].ID);
                avgVol = S.StorageStats[k].avgVol / (double)(S.ReportStepCount);
                maxVol = S.StorageStats[k].maxVol;
                pctMaxVol = 0.0;
                pctAvgVol = 0.0;
                if (S.Node[j].fullVolume > 0.0)
                {
                    pctAvgVol = avgVol / S.Node[j].fullVolume * 100.0;
                    pctMaxVol = maxVol / S.Node[j].fullVolume * 100.0;
                }
                pctEvapLoss = 0.0;
                pctSeepLoss = 0.0;
                if (S.NodeInflow[j] > 0.0)
                {
                    pctEvapLoss = S.StorageStats[k].evapLosses / S.NodeInflow[j] * 100.0;
                    pctSeepLoss = S.StorageStats[k].exfilLosses / S.NodeInflow[j] * 100.0;
                }
                R.fprintf(S.Frpt.file, S.L602, avgVol * S.UCF(S.VOLUME) / 1000.0, pctAvgVol, pctEvapLoss, pctSeepLoss, maxVol * S.UCF(S.VOLUME) / 1000.0, pctMaxVol);
                S.getElapsedTime(S.StorageStats[k].maxVolDate, ref days, ref hrs, ref mins);
                R.fprintf(S.Frpt.file, S.L603, days, hrs, mins);
                R.fprintf(S.Frpt.file, S.FlowFmt, S.StorageStats[k].maxFlow * S.UCF(S.FLOW));
            }
            S.report_writeLine((S.L0));
        }
    }

    // statsrpt.c:575
    public static void writeOutfallLoads()
    {
        CPtr units = R.Buf(15);
        int i = 0;
        int j = 0;
        int k = 0;
        int p = 0;
        double x = 0.0;
        double outfallCount = 0.0;
        double flowCount = 0.0;
        double flowSum = 0.0;
        double freqSum = 0.0;
        double volSum = 0.0;
        double[] totals = null;
        if (S.Nnodes[S.OUTFALL] > 0)
        {
            totals = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                totals[p] = 0.0;
            }
            flowSum = 0.0;
            freqSum = 0.0;
            volSum = 0.0;
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L604));
            S.report_writeLine((S.L605));
            S.report_writeLine((S.L604));
            S.report_writeLine((S.L0));
            R.fprintf(S.Frpt.file, S.L606);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L552);
            }
            R.fprintf(S.Frpt.file, S.L607);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L608);
            }
            R.fprintf(S.Frpt.file, S.L609);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L423, S.Pollut[p].ID);
            }
            R.fprintf(S.Frpt.file, S.L610, S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits], S.VolUnitsWords[S.UnitSystem]);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                i = S.UnitSystem;
                if (S.Pollut[p].units == S.COUNT)
                {
                    i = 2;
                }
                S.sstrncpy(units, S.LoadUnitsWords[i], unchecked((ulong)(14)));
                R.fprintf(S.Frpt.file, S.L423, units);
            }
            R.fprintf(S.Frpt.file, S.L606);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L552);
            }
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                if (S.Node[j].type != S.OUTFALL)
                {
                    continue;
                }
                k = S.Node[j].subIndex;
                flowCount = S.OutfallStats[k].totalPeriods;
                R.fprintf(S.Frpt.file, S.L541, S.Node[j].ID);
                x = 100.0 * flowCount / (double)(S.ReportStepCount);
                R.fprintf(S.Frpt.file, S.L611, x);
                freqSum += x;
                if (flowCount > 0)
                {
                    x = S.OutfallStats[k].avgFlow * S.UCF(S.FLOW) / flowCount;
                }
                else
                {
                    x = 0.0;
                }
                flowSum += x;
                R.fprintf(S.Frpt.file, S.L33);
                R.fprintf(S.Frpt.file, S.FlowFmt, x);
                R.fprintf(S.Frpt.file, S.L33);
                R.fprintf(S.Frpt.file, S.FlowFmt, S.OutfallStats[k].maxFlow * S.UCF(S.FLOW));
                R.fprintf(S.Frpt.file, S.L573, S.NodeInflow[j] * S.Vcf);
                volSum += S.NodeInflow[j];
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    x = S.OutfallStats[k].totalLoad[p] * 28.317 * S.Pollut[p].mcf;
                    totals[p] += x;
                    if (S.Pollut[p].units == S.COUNT)
                    {
                        x = (((x) > 0.0 ? Math.Log10((x)) : (x)));
                    }
                    R.fprintf(S.Frpt.file, S.L427, x);
                }
            }
            outfallCount = S.Nnodes[S.OUTFALL];
            R.fprintf(S.Frpt.file, S.L606);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(S.Frpt.file, S.L552);
            }
            R.fprintf(S.Frpt.file, S.L612, freqSum / outfallCount);
            R.fprintf(S.Frpt.file, S.FlowFmt, flowSum);
            R.fprintf(S.Frpt.file, S.L33);
            R.fprintf(S.Frpt.file, S.FlowFmt, S.MaxOutfallFlow * S.UCF(S.FLOW));
            R.fprintf(S.Frpt.file, S.L573, volSum * S.Vcf);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                x = totals[p];
                if (S.Pollut[p].units == S.COUNT)
                {
                    x = (((x) > 0.0 ? Math.Log10((x)) : (x)));
                }
                R.fprintf(S.Frpt.file, S.L427, x);
            }
            S.report_writeLine((S.L0));
            R.free(totals);
        }
    }

    // statsrpt.c:691
    public static void writeLinkFlows()
    {
        int j = 0;
        int k = 0;
        int days = 0;
        int hrs = 0;
        int mins = 0;
        double v = 0.0;
        double fullDepth = 0.0;
        if (S.Nobjects[S.LINK] == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L235));
        S.report_writeLine((S.L613));
        S.report_writeLine((S.L235));
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L614);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L615, S.FlowUnitWords[S.FlowUnits]);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L616, S.FlowUnitWords[S.FlowUnits]);
        }
        R.fprintf(S.Frpt.file, S.L617);
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            k = S.Link[j].subIndex;
            R.fprintf(S.Frpt.file, S.L541, S.Link[j].ID);
            if (S.Link[j].xsect.type == S.DUMMY)
            {
                R.fprintf(S.Frpt.file, S.L618);
            }
            else if (S.Link[j].xsect.type == S.IRREGULAR)
            {
                R.fprintf(S.Frpt.file, S.L619);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L620, S.LinkTypeWords[S.Link[j].type]);
            }
            S.getElapsedTime(S.LinkStats[j].maxFlowDate, ref days, ref hrs, ref mins);
            R.fprintf(S.Frpt.file, S.FlowFmt, S.LinkStats[j].maxFlow * S.UCF(S.FLOW));
            R.fprintf(S.Frpt.file, S.L570, days, hrs, mins);
            if ((S.Link[j].type == S.PUMP) && (S.Link[j].qFull > 0.0))
            {
                R.fprintf(S.Frpt.file, S.L621);
                R.fprintf(S.Frpt.file, S.L622, S.LinkStats[j].maxFlow / S.Link[j].qFull);
                continue;
            }
            if (S.Link[j].xsect.type == S.DUMMY)
            {
                continue;
            }
            if (S.Link[j].type == S.OUTLET)
            {
                continue;
            }
            if (S.Link[j].type == S.CONDUIT)
            {
                v = S.LinkStats[j].maxVeloc * S.UCF(S.LENGTH);
                if (v > 50.0)
                {
                    R.fprintf(S.Frpt.file, S.L623);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L624, v);
                }
                R.fprintf(S.Frpt.file, S.L622, S.LinkStats[j].maxFlow / S.Link[j].qFull / (double)(S.Conduit[k].barrels));
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L625);
            }
            fullDepth = S.Link[j].xsect.yFull;
            if ((S.Link[j].type == S.ORIFICE) && (S.Orifice[k].type == S.BOTTOM_ORIFICE))
            {
                fullDepth = 0.0;
            }
            if (fullDepth > 0.0)
            {
                R.fprintf(S.Frpt.file, S.L622, S.LinkStats[j].maxDepth / fullDepth);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L626);
            }
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:778
    public static void writeFlowClass()
    {
        int i = 0;
        int j = 0;
        int k = 0;
        double totalSeconds = S.RoutingTimeSpan;
        if (S.RouteModel != S.DW)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L473));
        S.report_writeLine((S.L627));
        S.report_writeLine((S.L473));
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L628);
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if (S.Link[j].type != S.CONDUIT)
            {
                continue;
            }
            if (S.Link[j].xsect.type == S.DUMMY)
            {
                continue;
            }
            k = S.Link[j].subIndex;
            R.fprintf(S.Frpt.file, S.L541, S.Link[j].ID);
            R.fprintf(S.Frpt.file, S.L629, S.Conduit[k].modLength / S.Conduit[k].length);
            for (i = 0; i < S.MAX_FLOW_CLASSES; i++)
            {
                R.fprintf(S.Frpt.file, S.L630, S.LinkStats[j].timeInFlowClass[i] /= totalSeconds);
            }
            R.fprintf(S.Frpt.file, S.L630, S.LinkStats[j].timeNormalFlow / totalSeconds);
            R.fprintf(S.Frpt.file, S.L630, S.LinkStats[j].timeInletControl / totalSeconds);
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:821
    public static void writeLinkSurcharge()
    {
        int i = 0;
        int j = 0;
        int n = 0;
        double[] t = new double[5];
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L470));
        S.report_writeLine((S.L631));
        S.report_writeLine((S.L470));
        S.report_writeLine((S.L0));
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if ((S.Link[j].type != S.CONDUIT) || (S.Link[j].xsect.type == S.DUMMY))
            {
                continue;
            }
            t[0] = S.LinkStats[j].timeSurcharged / 3600.0;
            t[1] = S.LinkStats[j].timeFullUpstream / 3600.0;
            t[2] = S.LinkStats[j].timeFullDnstream / 3600.0;
            t[3] = S.LinkStats[j].timeFullFlow / 3600.0;
            if (t[0] + t[1] + t[2] + t[3] == 0.0)
            {
                continue;
            }
            t[4] = S.LinkStats[j].timeCapacityLimited / 3600.0;
            for (i = 0; i < 5; i++)
            {
                t[i] = ((((0.01) >= (t[i])) ? (0.01) : (t[i])));
            }
            if (n == 0)
            {
                R.fprintf(S.Frpt.file, S.L632);
                n = 1;
            }
            R.fprintf(S.Frpt.file, S.L541, S.Link[j].ID);
            R.fprintf(S.Frpt.file, S.L633, t[0], t[1], t[2], t[3], t[4]);
        }
        if (n == 0)
        {
            S.report_writeLine((S.L634));
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:862
    public static void writePumpFlows()
    {
        int j = 0;
        int k = 0;
        double avgFlow = 0.0;
        double pctUtilized = 0.0;
        double pctOffCurve1 = 0.0;
        double pctOffCurve2 = 0.0;
        double totalSeconds = S.RoutingTimeSpan;
        if (S.Nlinks[S.PUMP] == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L221));
        S.report_writeLine((S.L635));
        S.report_writeLine((S.L221));
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L636, S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits], S.VolUnitsWords[S.UnitSystem]);
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if (S.Link[j].type != S.PUMP)
            {
                continue;
            }
            k = S.Link[j].subIndex;
            R.fprintf(S.Frpt.file, S.L541, S.Link[j].ID);
            pctUtilized = S.PumpStats[k].utilized / totalSeconds * 100.0;
            avgFlow = S.PumpStats[k].avgFlow;
            if (S.PumpStats[k].totalPeriods > 0)
            {
                avgFlow /= S.PumpStats[k].totalPeriods;
            }
            R.fprintf(S.Frpt.file, S.L637, pctUtilized, S.PumpStats[k].startUps, S.PumpStats[k].minFlow * S.UCF(S.FLOW), avgFlow * S.UCF(S.FLOW), S.PumpStats[k].maxFlow * S.UCF(S.FLOW), S.PumpStats[k].volume * S.Vcf, S.PumpStats[k].energy);
            pctOffCurve1 = S.PumpStats[k].offCurveLow;
            pctOffCurve2 = S.PumpStats[k].offCurveHigh;
            if (S.PumpStats[k].utilized > 0.0)
            {
                pctOffCurve1 = pctOffCurve1 / S.PumpStats[k].utilized * 100.0;
                pctOffCurve2 = pctOffCurve2 / S.PumpStats[k].utilized * 100.0;
            }
            R.fprintf(S.Frpt.file, S.L638, pctOffCurve1, pctOffCurve2);
        }
        S.report_writeLine((S.L0));
    }

    // statsrpt.c:916
    public static void writeLinkLoads()
    {
        int i = 0;
        int j = 0;
        int p = 0;
        double x = 0.0;
        CPtr units = R.Buf(15);
        CPtr linkLine = R.BufFrom(S.L551, 21);
        CPtr pollutLine = R.BufFrom(S.L552, 15);
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L473));
        S.report_writeLine((S.L639));
        S.report_writeLine((S.L473));
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L356, linkLine);
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            R.fprintf(S.Frpt.file, S.L169, pollutLine);
        }
        R.fprintf(S.Frpt.file, S.L555);
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            R.fprintf(S.Frpt.file, S.L423, S.Pollut[p].ID);
        }
        R.fprintf(S.Frpt.file, S.L640);
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            i = S.UnitSystem;
            if (S.Pollut[p].units == S.COUNT)
            {
                i = 2;
            }
            S.sstrncpy(units, S.LoadUnitsWords[i], unchecked((ulong)(14)));
            R.fprintf(S.Frpt.file, S.L423, units);
        }
        R.fprintf(S.Frpt.file, S.L356, linkLine);
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            R.fprintf(S.Frpt.file, S.L169, pollutLine);
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            R.fprintf(S.Frpt.file, S.L541, S.Link[j].ID);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                x = S.Link[j].totalLoad[p] * 28.317 * S.Pollut[p].mcf;
                if (S.Pollut[p].units == S.COUNT)
                {
                    x = (((x) > 0.0 ? Math.Log10((x)) : (x)));
                }
                if (x < 10000.0)
                {
                    R.fprintf(S.Frpt.file, S.L427, x);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L641, x);
                }
            }
        }
        S.report_writeLine((S.L0));
    }

}
}
