// Generated from EPA SWMM 5.2.4 (massbal.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double MAX_RUNOFF_BALANCE_ERR = 10.0;
    public static double MAX_FLOW_BALANCE_ERR = 10.0;
    public static TRunoffTotals RunoffTotals = new TRunoffTotals();
    public static TLoadingTotals[] LoadingTotals = null;
    public static TGwaterTotals GwaterTotals = new TGwaterTotals();
    public static TRoutingTotals FlowTotals = new TRoutingTotals();
    public static TRoutingTotals[] QualTotals = null;
    public static TRoutingTotals StepFlowTotals = new TRoutingTotals();
    public static TRoutingTotals OldStepFlowTotals = new TRoutingTotals();
    public static TRoutingTotals[] StepQualTotals = null;
    public static double[] NodeInflow = null;
    public static double[] NodeOutflow = null;
    public static double TotalArea = 0.0;

    // massbal.c:100
    public static int massbal_open()
    {
        int j = 0;
        int n = 0;
        S.RunoffError = 0.0;
        S.GwaterError = 0.0;
        S.FlowError = 0.0;
        S.QualError = 0.0;
        S.RunoffTotals.rainfall = 0.0;
        S.RunoffTotals.evap = 0.0;
        S.RunoffTotals.infil = 0.0;
        S.RunoffTotals.runoff = 0.0;
        S.RunoffTotals.runon = 0.0;
        S.RunoffTotals.drains = 0.0;
        S.RunoffTotals.snowRemoved = 0.0;
        S.RunoffTotals.initStorage = 0.0;
        S.RunoffTotals.initSnowCover = 0.0;
        S.TotalArea = 0.0;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.RunoffTotals.initStorage += S.subcatch_getStorage(j);
            S.RunoffTotals.initSnowCover += S.snow_getSnowCover(j);
            S.TotalArea += S.Subcatch[j].area;
        }
        S.GwaterTotals.infil = 0.0;
        S.GwaterTotals.upperEvap = 0.0;
        S.GwaterTotals.lowerEvap = 0.0;
        S.GwaterTotals.lowerPerc = 0.0;
        S.GwaterTotals.gwater = 0.0;
        S.GwaterTotals.initStorage = 0.0;
        S.GwaterTotals.finalStorage = 0.0;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.GwaterTotals.initStorage += S.gwater_getVolume(j) * S.Subcatch[j].area;
        }
        S.FlowTotals.dwInflow = 0.0;
        S.FlowTotals.wwInflow = 0.0;
        S.FlowTotals.gwInflow = 0.0;
        S.FlowTotals.iiInflow = 0.0;
        S.FlowTotals.exInflow = 0.0;
        S.FlowTotals.flooding = 0.0;
        S.FlowTotals.outflow = 0.0;
        S.FlowTotals.evapLoss = 0.0;
        S.FlowTotals.seepLoss = 0.0;
        S.FlowTotals.reacted = 0.0;
        S.FlowTotals.initStorage = 0.0;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.FlowTotals.initStorage += S.Node[j].newVolume;
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.FlowTotals.initStorage += S.Link[j].newVolume;
        }
        S.StepFlowTotals = S.FlowTotals.Clone();
        S.LoadingTotals = (null);
        S.QualTotals = (null);
        S.StepQualTotals = (null);
        S.NodeInflow = (null);
        S.NodeOutflow = (null);
        n = S.Nobjects[S.POLLUT];
        if (n > 0)
        {
            S.LoadingTotals = R.NewArray<TLoadingTotals>((int)(unchecked((ulong)(n))));
            if (S.LoadingTotals == null)
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
            for (j = 0; j < n; j++)
            {
                S.LoadingTotals[j].initLoad = S.massbal_getBuildup(j);
                S.LoadingTotals[j].buildup = 0.0;
                S.LoadingTotals[j].deposition = 0.0;
                S.LoadingTotals[j].sweeping = 0.0;
                S.LoadingTotals[j].infil = 0.0;
                S.LoadingTotals[j].bmpRemoval = 0.0;
                S.LoadingTotals[j].runoff = 0.0;
                S.LoadingTotals[j].finalLoad = 0.0;
            }
        }
        if (n > 0)
        {
            S.QualTotals = R.NewArray<TRoutingTotals>((int)(unchecked((ulong)(n))));
            S.StepQualTotals = R.NewArray<TRoutingTotals>((int)(unchecked((ulong)(n))));
            if ((S.QualTotals == null) || (S.StepQualTotals == null))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
        }
        for (j = 0; j < n; j++)
        {
            S.QualTotals[j].dwInflow = 0.0;
            S.QualTotals[j].wwInflow = 0.0;
            S.QualTotals[j].gwInflow = 0.0;
            S.QualTotals[j].exInflow = 0.0;
            S.QualTotals[j].flooding = 0.0;
            S.QualTotals[j].outflow = 0.0;
            S.QualTotals[j].evapLoss = 0.0;
            S.QualTotals[j].seepLoss = 0.0;
            S.QualTotals[j].reacted = 0.0;
            S.QualTotals[j].initStorage = S.massbal_getStoredMass(j);
        }
        S.massbal_initTimeStepTotals();
        if (S.Nobjects[S.NODE] > 0)
        {
            S.NodeInflow = new double[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
            if (S.NodeInflow == null)
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
            S.NodeOutflow = new double[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
            if (S.NodeOutflow == null)
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                S.NodeInflow[j] = S.Node[j].newVolume;
            }
        }
        return S.ErrorCode;
    }

    // massbal.c:246
    public static void massbal_close()
    {
        {
            if ((S.LoadingTotals != null))
            {
                R.free(S.LoadingTotals);
                S.LoadingTotals = (null);
            }
        }
        ;
        {
            if ((S.QualTotals != null))
            {
                R.free(S.QualTotals);
                S.QualTotals = (null);
            }
        }
        ;
        {
            if ((S.StepQualTotals != null))
            {
                R.free(S.StepQualTotals);
                S.StepQualTotals = (null);
            }
        }
        ;
        {
            if ((S.NodeInflow != null))
            {
                R.free(S.NodeInflow);
                S.NodeInflow = (null);
            }
        }
        ;
        {
            if ((S.NodeOutflow != null))
            {
                R.free(S.NodeOutflow);
                S.NodeOutflow = (null);
            }
        }
        ;
    }

    // massbal.c:262
    public static void massbal_report()
    {
        int j = 0;
        double gwArea = 0.0;
        if (S.Nobjects[S.SUBCATCH] > 0)
        {
            if ((S.massbal_getRunoffError() > S.MAX_RUNOFF_BALANCE_ERR) || (S.RptFlags.continuity == 1))
            {
                S.report_writeRunoffError(S.RunoffTotals, S.TotalArea);
            }
            if ((S.Nobjects[S.POLLUT] > 0) && (!(S.IgnoreQuality != 0)))
            {
                if ((S.massbal_getLoadingError() > S.MAX_RUNOFF_BALANCE_ERR) || (S.RptFlags.continuity == 1))
                {
                    S.report_writeLoadingError(S.LoadingTotals);
                }
            }
        }
        if ((S.Nobjects[S.AQUIFER] > 0) && (!(S.IgnoreGwater != 0)))
        {
            if ((S.massbal_getGwaterError() > S.MAX_RUNOFF_BALANCE_ERR) || (S.RptFlags.continuity == 1))
            {
                for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
                {
                    if ((S.Subcatch[j].groundwater != null))
                    {
                        gwArea += S.Subcatch[j].area;
                    }
                }
                if (gwArea > 0.0)
                {
                    S.report_writeGwaterError(S.GwaterTotals, gwArea);
                }
            }
        }
        if ((S.Nobjects[S.NODE] > 0) && (!(S.IgnoreRouting != 0)))
        {
            if ((S.massbal_getFlowError() > S.MAX_FLOW_BALANCE_ERR) || (S.RptFlags.continuity == 1))
            {
                S.report_writeFlowError(S.FlowTotals);
            }
            if ((S.Nobjects[S.POLLUT] > 0) && (!(S.IgnoreQuality != 0)))
            {
                if ((S.massbal_getQualError() > S.MAX_FLOW_BALANCE_ERR) || (S.RptFlags.continuity == 1))
                {
                    S.report_writeQualError(S.QualTotals);
                }
            }
        }
    }

    // massbal.c:316
    public static double massbal_getBuildup(int p)
    {
        int i = 0;
        int j = 0;
        double load = 0.0;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            for (i = 0; i < S.Nobjects[S.LANDUSE]; i++)
            {
                load += S.Subcatch[j].landFactor[i].buildup[p];
            }
            load += S.Subcatch[j].pondedQual[p] * S.Pollut[p].mcf;
        }
        return load;
    }

    // massbal.c:339
    public static void massbal_updateRunoffTotals(int flowType, double v)
    {
        switch (flowType)
        {
            case S.RUNOFF_RAINFALL:
                S.RunoffTotals.rainfall += v;
                break;
            case S.RUNOFF_EVAP:
                S.RunoffTotals.evap += v;
                break;
            case S.RUNOFF_INFIL:
                S.RunoffTotals.infil += v;
                break;
            case S.RUNOFF_RUNOFF:
                S.RunoffTotals.runoff += v;
                break;
            case S.RUNOFF_DRAINS:
                S.RunoffTotals.drains += v;
                break;
            case S.RUNOFF_RUNON:
                S.RunoffTotals.runon += v;
                break;
        }
    }

    // massbal.c:360
    public static void massbal_updateGwaterTotals(double vInfil, double vUpperEvap, double vLowerEvap, double vLowerPerc, double vGwater)
    {
        S.GwaterTotals.infil += vInfil;
        S.GwaterTotals.upperEvap += vUpperEvap;
        S.GwaterTotals.lowerEvap += vLowerEvap;
        S.GwaterTotals.lowerPerc += vLowerPerc;
        S.GwaterTotals.gwater += vGwater;
    }

    // massbal.c:381
    public static void massbal_initTimeStepTotals()
    {
        int j = 0;
        S.OldStepFlowTotals = S.StepFlowTotals.Clone();
        S.StepFlowTotals.dwInflow = 0.0;
        S.StepFlowTotals.wwInflow = 0.0;
        S.StepFlowTotals.gwInflow = 0.0;
        S.StepFlowTotals.iiInflow = 0.0;
        S.StepFlowTotals.exInflow = 0.0;
        S.StepFlowTotals.flooding = 0.0;
        S.StepFlowTotals.outflow = 0.0;
        S.StepFlowTotals.evapLoss = 0.0;
        S.StepFlowTotals.seepLoss = 0.0;
        S.StepFlowTotals.reacted = 0.0;
        for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
        {
            S.StepQualTotals[j].dwInflow = 0.0;
            S.StepQualTotals[j].wwInflow = 0.0;
            S.StepQualTotals[j].gwInflow = 0.0;
            S.StepQualTotals[j].iiInflow = 0.0;
            S.StepQualTotals[j].exInflow = 0.0;
            S.StepQualTotals[j].flooding = 0.0;
            S.StepQualTotals[j].outflow = 0.0;
            S.StepQualTotals[j].reacted = 0.0;
            S.StepQualTotals[j].seepLoss = 0.0;
            S.StepQualTotals[j].initStorage = 0.0;
            S.StepQualTotals[j].finalStorage = 0.0;
        }
    }

    // massbal.c:418
    public static void massbal_addInflowFlow(int type, double q)
    {
        switch (type)
        {
            case S.DRY_WEATHER_INFLOW:
                S.StepFlowTotals.dwInflow += q;
                break;
            case S.WET_WEATHER_INFLOW:
                S.StepFlowTotals.wwInflow += q;
                break;
            case S.GROUNDWATER_INFLOW:
                S.StepFlowTotals.gwInflow += q;
                break;
            case S.RDII_INFLOW:
                S.StepFlowTotals.iiInflow += q;
                break;
            case S.EXTERNAL_INFLOW:
                S.StepFlowTotals.exInflow += q;
                break;
        }
    }

    // massbal.c:438
    public static void massbal_updateLoadingTotals(int type, int p, double w)
    {
        switch (type)
        {
            case S.BUILDUP_LOAD:
                S.LoadingTotals[p].buildup += w;
                break;
            case S.DEPOSITION_LOAD:
                S.LoadingTotals[p].deposition += w;
                break;
            case S.SWEEPING_LOAD:
                S.LoadingTotals[p].sweeping += w;
                break;
            case S.INFIL_LOAD:
                S.LoadingTotals[p].infil += w;
                break;
            case S.BMP_REMOVAL_LOAD:
                S.LoadingTotals[p].bmpRemoval += w;
                break;
            case S.RUNOFF_LOAD:
                S.LoadingTotals[p].runoff += w;
                break;
            case S.FINAL_LOAD:
                S.LoadingTotals[p].finalLoad += w;
                break;
        }
    }

    // massbal.c:461
    public static void massbal_addInflowQual(int type, int p, double w)
    {
        if ((p < 0) || (p >= S.Nobjects[S.POLLUT]))
        {
            return;
        }
        switch (type)
        {
            case S.DRY_WEATHER_INFLOW:
                S.StepQualTotals[p].dwInflow += w;
                break;
            case S.WET_WEATHER_INFLOW:
                S.StepQualTotals[p].wwInflow += w;
                break;
            case S.GROUNDWATER_INFLOW:
                S.StepQualTotals[p].gwInflow += w;
                break;
            case S.EXTERNAL_INFLOW:
                S.StepQualTotals[p].exInflow += w;
                break;
            case S.RDII_INFLOW:
                S.StepQualTotals[p].iiInflow += w;
                break;
        }
    }

    // massbal.c:483
    public static void massbal_addOutflowFlow(double q, int isFlooded)
    {
        if ((isFlooded != 0))
        {
            S.StepFlowTotals.flooding += q;
        }
        else
        {
            S.StepFlowTotals.outflow += q;
        }
    }

    // massbal.c:497
    public static void massbal_addOutflowQual(int p, double w, int isFlooded)
    {
        if ((p < 0) || (p >= S.Nobjects[S.POLLUT]))
        {
            return;
        }
        if (w >= 0.0)
        {
            if ((isFlooded != 0))
            {
                S.StepQualTotals[p].flooding += w;
            }
            else
            {
                S.StepQualTotals[p].outflow += w;
            }
        }
        else
        {
            S.StepQualTotals[p].exInflow -= w;
        }
    }

    // massbal.c:517
    public static void massbal_addReactedMass(int p, double w)
    {
        if ((p < 0) || (p >= S.Nobjects[S.POLLUT]))
        {
            return;
        }
        S.StepQualTotals[p].reacted += w;
    }

    // massbal.c:531
    public static void massbal_addSeepageLoss(int p, double w)
    {
        if ((p < 0) || (p >= S.Nobjects[S.POLLUT]))
        {
            return;
        }
        S.StepQualTotals[p].seepLoss += w;
    }

    // massbal.c:545
    public static void massbal_addToFinalStorage(int p, double w)
    {
        if ((p < 0) || (p >= S.Nobjects[S.POLLUT]))
        {
            return;
        }
        S.StepQualTotals[p].finalStorage += w;
    }

    // massbal.c:559
    public static void massbal_addNodeLosses(double evapLoss, double seepLoss)
    {
        S.StepFlowTotals.evapLoss += evapLoss;
        S.StepFlowTotals.seepLoss += seepLoss;
    }

    // massbal.c:573
    public static void massbal_addLinkLosses(double evapLoss, double seepLoss)
    {
        S.StepFlowTotals.evapLoss += evapLoss;
        S.StepFlowTotals.seepLoss += seepLoss;
    }

    // massbal.c:587
    public static void massbal_updateRoutingTotals(double tStep)
    {
        int j = 0;
        S.FlowTotals.dwInflow += S.StepFlowTotals.dwInflow * tStep;
        S.FlowTotals.wwInflow += S.StepFlowTotals.wwInflow * tStep;
        S.FlowTotals.gwInflow += S.StepFlowTotals.gwInflow * tStep;
        S.FlowTotals.iiInflow += S.StepFlowTotals.iiInflow * tStep;
        S.FlowTotals.exInflow += S.StepFlowTotals.exInflow * tStep;
        S.FlowTotals.flooding += S.StepFlowTotals.flooding * tStep;
        S.FlowTotals.outflow += S.StepFlowTotals.outflow * tStep;
        S.FlowTotals.evapLoss += S.StepFlowTotals.evapLoss * tStep;
        S.FlowTotals.seepLoss += S.StepFlowTotals.seepLoss * tStep;
        for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
        {
            S.QualTotals[j].dwInflow += S.StepQualTotals[j].dwInflow * tStep;
            S.QualTotals[j].wwInflow += S.StepQualTotals[j].wwInflow * tStep;
            S.QualTotals[j].gwInflow += S.StepQualTotals[j].gwInflow * tStep;
            S.QualTotals[j].iiInflow += S.StepQualTotals[j].iiInflow * tStep;
            S.QualTotals[j].exInflow += S.StepQualTotals[j].exInflow * tStep;
            S.QualTotals[j].flooding += S.StepQualTotals[j].flooding * tStep;
            S.QualTotals[j].outflow += S.StepQualTotals[j].outflow * tStep;
            S.QualTotals[j].reacted += S.StepQualTotals[j].reacted * tStep;
            S.QualTotals[j].seepLoss += S.StepQualTotals[j].seepLoss * tStep;
            S.QualTotals[j].finalStorage += S.StepQualTotals[j].finalStorage;
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.NodeInflow[j] += S.Node[j].inflow * tStep;
            if ((S.Node[j].type == S.OUTFALL) || (((S.Node[j].degree == 0) && (S.Node[j].type != S.STORAGE))))
            {
                S.NodeOutflow[j] += S.Node[j].inflow * tStep;
            }
            else
            {
                S.NodeOutflow[j] += S.Node[j].outflow * tStep;
                if (S.Node[j].newVolume <= S.Node[j].fullVolume)
                {
                    S.NodeOutflow[j] += S.Node[j].overflow * tStep;
                }
            }
        }
    }

    // massbal.c:638
    public static double massbal_getStorage(int isFinalStorage)
    {
        int j = 0;
        double totalStorage = 0.0;
        double nodeStorage = 0.0;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            nodeStorage = S.Node[j].newVolume;
            if ((isFinalStorage != 0))
            {
                S.NodeOutflow[j] += nodeStorage;
            }
            totalStorage += nodeStorage;
        }
        if ((isFinalStorage != 0) && (S.RouteModel == S.SF))
        {
            return totalStorage;
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            totalStorage += S.Link[j].newVolume;
        }
        return totalStorage;
    }

    // massbal.c:670
    public static void massbal_getSysFlows(double f, double[] sysFlows)
    {
        double f1 = 1.0 - f;
        sysFlows[S.SYS_DWFLOW] = (f1 * S.OldStepFlowTotals.dwInflow + f * S.StepFlowTotals.dwInflow) * S.UCF(S.FLOW);
        sysFlows[S.SYS_GWFLOW] = (f1 * S.OldStepFlowTotals.gwInflow + f * S.StepFlowTotals.gwInflow) * S.UCF(S.FLOW);
        sysFlows[S.SYS_IIFLOW] = (f1 * S.OldStepFlowTotals.iiInflow + f * S.StepFlowTotals.iiInflow) * S.UCF(S.FLOW);
        sysFlows[S.SYS_EXFLOW] = (f1 * S.OldStepFlowTotals.exInflow + f * S.StepFlowTotals.exInflow) * S.UCF(S.FLOW);
        sysFlows[S.SYS_FLOODING] = (f1 * S.OldStepFlowTotals.flooding + f * S.StepFlowTotals.flooding) * S.UCF(S.FLOW);
        sysFlows[S.SYS_OUTFLOW] = (f1 * S.OldStepFlowTotals.outflow + f * S.StepFlowTotals.outflow) * S.UCF(S.FLOW);
        sysFlows[S.SYS_STORAGE] = (f1 * S.OldStepFlowTotals.finalStorage + f * S.StepFlowTotals.finalStorage) * S.UCF(S.VOLUME);
    }

    // massbal.c:696
    public static double massbal_getRunoffError()
    {
        int j = 0;
        double totalInflow = 0.0;
        double totalOutflow = 0.0;
        S.RunoffTotals.finalStorage = 0.0;
        S.RunoffTotals.finalSnowCover = 0.0;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.RunoffTotals.finalStorage += S.subcatch_getStorage(j);
            S.RunoffTotals.finalSnowCover += S.snow_getSnowCover(j);
        }
        S.RunoffTotals.snowRemoved = S.Snow.removed;
        totalInflow = S.RunoffTotals.rainfall + S.RunoffTotals.runon + S.RunoffTotals.initStorage + S.RunoffTotals.initSnowCover;
        totalOutflow = S.RunoffTotals.evap + S.RunoffTotals.infil + S.RunoffTotals.runoff + S.RunoffTotals.drains + S.RunoffTotals.snowRemoved + S.RunoffTotals.finalStorage + S.RunoffTotals.finalSnowCover;
        S.RunoffTotals.pctError = 0.0;
        if (Math.Abs(totalInflow - totalOutflow) < 1.0)
        {
            S.RunoffTotals.pctError = 1e-06;
        }
        else if (totalInflow > 0.0)
        {
            S.RunoffTotals.pctError = 100.0 * (1.0 - totalOutflow / totalInflow);
        }
        else if (totalOutflow > 0.0)
        {
            S.RunoffTotals.pctError = 100.0 * (totalInflow / totalOutflow - 1.0);
        }
        S.RunoffError = S.RunoffTotals.pctError;
        return S.RunoffTotals.pctError;
    }

    // massbal.c:750
    public static double massbal_getLoadingError()
    {
        int j = 0;
        double loadIn = 0.0;
        double loadOut = 0.0;
        double maxError = 0.0;
        for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
        {
            S.LoadingTotals[j].finalLoad += S.massbal_getBuildup(j);
            loadIn = S.LoadingTotals[j].initLoad + S.LoadingTotals[j].buildup + S.LoadingTotals[j].deposition;
            loadOut = S.LoadingTotals[j].sweeping + S.LoadingTotals[j].infil + S.LoadingTotals[j].bmpRemoval + S.LoadingTotals[j].runoff + S.LoadingTotals[j].finalLoad;
            S.LoadingTotals[j].pctError = 0.0;
            if (Math.Abs(loadIn - loadOut) < 0.001)
            {
                S.LoadingTotals[j].pctError = 1e-06;
            }
            else if (loadIn > 0.0)
            {
                S.LoadingTotals[j].pctError = 100.0 * (1.0 - loadOut / loadIn);
            }
            else if (loadOut > 0.0)
            {
                S.LoadingTotals[j].pctError = 100.0 * (loadIn / loadOut - 1.0);
            }
            maxError = ((((maxError) >= (S.LoadingTotals[j].pctError)) ? (maxError) : (S.LoadingTotals[j].pctError)));
            if (S.Pollut[j].units == S.COUNT)
            {
                S.LoadingTotals[j].initLoad = (((S.LoadingTotals[j].initLoad) > 0.0 ? Math.Log10((S.LoadingTotals[j].initLoad)) : (S.LoadingTotals[j].initLoad)));
                S.LoadingTotals[j].buildup = (((S.LoadingTotals[j].buildup) > 0.0 ? Math.Log10((S.LoadingTotals[j].buildup)) : (S.LoadingTotals[j].buildup)));
                S.LoadingTotals[j].deposition = (((S.LoadingTotals[j].deposition) > 0.0 ? Math.Log10((S.LoadingTotals[j].deposition)) : (S.LoadingTotals[j].deposition)));
                S.LoadingTotals[j].sweeping = (((S.LoadingTotals[j].sweeping) > 0.0 ? Math.Log10((S.LoadingTotals[j].sweeping)) : (S.LoadingTotals[j].sweeping)));
                S.LoadingTotals[j].infil = (((S.LoadingTotals[j].infil) > 0.0 ? Math.Log10((S.LoadingTotals[j].infil)) : (S.LoadingTotals[j].infil)));
                S.LoadingTotals[j].bmpRemoval = (((S.LoadingTotals[j].bmpRemoval) > 0.0 ? Math.Log10((S.LoadingTotals[j].bmpRemoval)) : (S.LoadingTotals[j].bmpRemoval)));
                S.LoadingTotals[j].runoff = (((S.LoadingTotals[j].runoff) > 0.0 ? Math.Log10((S.LoadingTotals[j].runoff)) : (S.LoadingTotals[j].runoff)));
                S.LoadingTotals[j].finalLoad = (((S.LoadingTotals[j].finalLoad) > 0.0 ? Math.Log10((S.LoadingTotals[j].finalLoad)) : (S.LoadingTotals[j].finalLoad)));
            }
        }
        return maxError;
    }

    // massbal.c:813
    public static double massbal_getGwaterError()
    {
        int j = 0;
        double totalInflow = 0.0;
        double totalOutflow = 0.0;
        S.GwaterTotals.finalStorage = 0.0;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.GwaterTotals.finalStorage += S.gwater_getVolume(j) * S.Subcatch[j].area;
        }
        totalInflow = S.GwaterTotals.infil + S.GwaterTotals.initStorage;
        totalOutflow = S.GwaterTotals.upperEvap + S.GwaterTotals.lowerEvap + S.GwaterTotals.lowerPerc + S.GwaterTotals.gwater + S.GwaterTotals.finalStorage;
        S.GwaterTotals.pctError = 0.0;
        if (Math.Abs(totalInflow - totalOutflow) < 1.0)
        {
            S.GwaterTotals.pctError = 1e-06;
        }
        else if (totalInflow > 0.0)
        {
            S.GwaterTotals.pctError = 100.0 * (1.0 - totalOutflow / totalInflow);
        }
        else if (totalOutflow > 0.0)
        {
            S.GwaterTotals.pctError = 100.0 * (totalInflow / totalOutflow - 1.0);
        }
        S.GwaterError = S.GwaterTotals.pctError;
        return S.GwaterTotals.pctError;
    }

    // massbal.c:858
    public static double massbal_getFlowError()
    {
        double totalInflow = 0.0;
        double totalOutflow = 0.0;
        S.FlowTotals.finalStorage = S.massbal_getStorage(1);
        totalInflow = S.FlowTotals.initStorage + S.FlowTotals.wwInflow + S.FlowTotals.iiInflow;
        totalOutflow = S.FlowTotals.finalStorage + S.FlowTotals.flooding + S.FlowTotals.evapLoss + S.FlowTotals.seepLoss + S.FlowTotals.reacted;
        if (S.FlowTotals.dwInflow >= 0.0)
        {
            totalInflow += S.FlowTotals.dwInflow;
        }
        else
        {
            totalOutflow -= S.FlowTotals.dwInflow;
        }
        if (S.FlowTotals.gwInflow >= 0.0)
        {
            totalInflow += S.FlowTotals.gwInflow;
        }
        else
        {
            totalOutflow -= S.FlowTotals.gwInflow;
        }
        if (S.FlowTotals.exInflow >= 0.0)
        {
            totalInflow += S.FlowTotals.exInflow;
        }
        else
        {
            totalOutflow -= S.FlowTotals.exInflow;
        }
        if (S.FlowTotals.outflow >= 0.0)
        {
            totalOutflow += S.FlowTotals.outflow;
        }
        else
        {
            totalInflow -= S.FlowTotals.outflow;
        }
        S.FlowTotals.pctError = 0.0;
        if (Math.Abs(totalInflow - totalOutflow) < 1.0)
        {
            S.FlowTotals.pctError = 1e-06;
        }
        else if (Math.Abs(totalInflow) > 0.0)
        {
            S.FlowTotals.pctError = 100.0 * (1.0 - totalOutflow / totalInflow);
        }
        else if (Math.Abs(totalOutflow) > 0.0)
        {
            S.FlowTotals.pctError = 100.0 * (totalInflow / totalOutflow - 1.0);
        }
        S.FlowError = S.FlowTotals.pctError;
        return S.FlowTotals.pctError;
    }

    // massbal.c:906
    public static double massbal_getQualError()
    {
        int p = 0;
        double maxQualError = 0.0;
        double totalInflow = 0.0;
        double totalOutflow = 0.0;
        double cf = 0.0;
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.QualTotals[p].finalStorage += S.massbal_getStoredMass(p);
            totalInflow = S.QualTotals[p].dwInflow + S.QualTotals[p].wwInflow + S.QualTotals[p].gwInflow + S.QualTotals[p].iiInflow + S.QualTotals[p].exInflow + S.QualTotals[p].initStorage;
            totalOutflow = S.QualTotals[p].flooding + S.QualTotals[p].outflow + S.QualTotals[p].reacted + S.QualTotals[p].seepLoss + S.QualTotals[p].finalStorage;
            S.QualTotals[p].pctError = 0.0;
            if (Math.Abs(totalInflow - totalOutflow) < 0.001)
            {
                S.QualTotals[p].pctError = 1e-06;
            }
            else if (totalInflow > 0.0)
            {
                S.QualTotals[p].pctError = 100.0 * (1.0 - totalOutflow / totalInflow);
            }
            else if (totalOutflow > 0.0)
            {
                S.QualTotals[p].pctError = 100.0 * (totalInflow / totalOutflow - 1.0);
            }
            if (Math.Abs(S.QualTotals[p].pctError) > Math.Abs(maxQualError))
            {
                maxQualError = S.QualTotals[p].pctError;
            }
            cf = 28.317;
            if (S.Pollut[p].units == S.COUNT)
            {
                S.QualTotals[p].dwInflow = (((cf * S.QualTotals[p].dwInflow) > 0.0 ? Math.Log10((cf * S.QualTotals[p].dwInflow)) : (cf * S.QualTotals[p].dwInflow)));
                S.QualTotals[p].wwInflow = (((cf * S.QualTotals[p].wwInflow) > 0.0 ? Math.Log10((cf * S.QualTotals[p].wwInflow)) : (cf * S.QualTotals[p].wwInflow)));
                S.QualTotals[p].gwInflow = (((cf * S.QualTotals[p].gwInflow) > 0.0 ? Math.Log10((cf * S.QualTotals[p].gwInflow)) : (cf * S.QualTotals[p].gwInflow)));
                S.QualTotals[p].iiInflow = (((cf * S.QualTotals[p].iiInflow) > 0.0 ? Math.Log10((cf * S.QualTotals[p].iiInflow)) : (cf * S.QualTotals[p].iiInflow)));
                S.QualTotals[p].exInflow = (((cf * S.QualTotals[p].exInflow) > 0.0 ? Math.Log10((cf * S.QualTotals[p].exInflow)) : (cf * S.QualTotals[p].exInflow)));
                S.QualTotals[p].flooding = (((cf * S.QualTotals[p].flooding) > 0.0 ? Math.Log10((cf * S.QualTotals[p].flooding)) : (cf * S.QualTotals[p].flooding)));
                S.QualTotals[p].outflow = (((cf * S.QualTotals[p].outflow) > 0.0 ? Math.Log10((cf * S.QualTotals[p].outflow)) : (cf * S.QualTotals[p].outflow)));
                S.QualTotals[p].reacted = (((cf * S.QualTotals[p].reacted) > 0.0 ? Math.Log10((cf * S.QualTotals[p].reacted)) : (cf * S.QualTotals[p].reacted)));
                S.QualTotals[p].seepLoss = (((cf * S.QualTotals[p].seepLoss) > 0.0 ? Math.Log10((cf * S.QualTotals[p].seepLoss)) : (cf * S.QualTotals[p].seepLoss)));
                S.QualTotals[p].initStorage = (((cf * S.QualTotals[p].initStorage) > 0.0 ? Math.Log10((cf * S.QualTotals[p].initStorage)) : (cf * S.QualTotals[p].initStorage)));
                S.QualTotals[p].finalStorage = (((cf * S.QualTotals[p].finalStorage) > 0.0 ? Math.Log10((cf * S.QualTotals[p].finalStorage)) : (cf * S.QualTotals[p].finalStorage)));
            }
            else
            {
                cf = cf * S.UCF(S.MASS);
                if (S.Pollut[p].units == S.UG)
                {
                    cf /= 1000.0;
                }
                S.QualTotals[p].dwInflow *= cf;
                S.QualTotals[p].wwInflow *= cf;
                S.QualTotals[p].gwInflow *= cf;
                S.QualTotals[p].iiInflow *= cf;
                S.QualTotals[p].exInflow *= cf;
                S.QualTotals[p].flooding *= cf;
                S.QualTotals[p].outflow *= cf;
                S.QualTotals[p].reacted *= cf;
                S.QualTotals[p].seepLoss *= cf;
                S.QualTotals[p].initStorage *= cf;
                S.QualTotals[p].finalStorage *= cf;
            }
        }
        S.QualError = maxQualError;
        return maxQualError;
    }

    // massbal.c:995
    public static double massbal_getStepFlowError()
    {
        double totalInflow = 0.0;
        double totalOutflow = 0.0;
        totalInflow = S.StepFlowTotals.dwInflow + S.StepFlowTotals.wwInflow + S.StepFlowTotals.gwInflow + S.StepFlowTotals.iiInflow + S.StepFlowTotals.exInflow;
        totalOutflow = S.StepFlowTotals.flooding + S.StepFlowTotals.outflow + S.StepFlowTotals.evapLoss + S.StepFlowTotals.seepLoss + S.StepFlowTotals.reacted;
        if (Math.Abs(totalInflow) > 0.0)
        {
            return 1.0 - totalOutflow / totalInflow;
        }
        else if (Math.Abs(totalOutflow) > 0.0)
        {
            return totalInflow / totalOutflow - 1.0;
        }
        else
        {
            return 0.0;
        }
    }

    // massbal.c:1025
    public static double massbal_getStoredMass(int p)
    {
        int j = 0;
        double storedMass = 0.0;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            storedMass += S.Node[j].newVolume * S.Node[j].newQual[p];
        }
        if (S.RouteModel != S.SF)
        {
            for (j = 0; j < S.Nobjects[S.LINK]; j++)
            {
                storedMass += S.Link[j].newVolume * S.Link[j].newQual[p];
            }
        }
        return storedMass;
    }

}
}
