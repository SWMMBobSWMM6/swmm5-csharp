// Generated from EPA SWMM 5.2.4 (stats.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static TTimeStepStats TimeStepStats = new TTimeStepStats();
    public static TMaxStats[] MaxMassBalErrs = R.NewArray<TMaxStats>(5);
    public static TMaxStats[] MaxCourantCrit = R.NewArray<TMaxStats>(5);
    public static TMaxStats[] MaxFlowTurns = R.NewArray<TMaxStats>(5);
    public static TMaxStats[] MaxNonConverged = R.NewArray<TMaxStats>(5);
    public static double SysOutfallFlow = 0.0;
    public static TSubcatchStats[] SubcatchStats = null;
    public static TNodeStats[] NodeStats = null;
    public static TLinkStats[] LinkStats = null;
    public static TStorageStats[] StorageStats = null;
    public static TOutfallStats[] OutfallStats = null;
    public static TPumpStats[] PumpStats = null;
    public static double MaxOutfallFlow = 0.0;
    public static double MaxRunoffFlow = 0.0;
    public static double RoutingTimeSpan = 0.0;

    // stats.c:102
    public static int stats_open()
    {
        int j = 0;
        int k = 0;
        double timeStepDelta = 0.0;
        double logMaxTimeStep = 0.0;
        double logMinTimeStep = 0.0;
        S.NodeStats = (null);
        S.LinkStats = (null);
        S.StorageStats = (null);
        S.OutfallStats = (null);
        S.PumpStats = (null);
        S.SubcatchStats = (null);
        if (S.Nobjects[S.SUBCATCH] > 0)
        {
            S.SubcatchStats = R.NewArray<TSubcatchStats>((int)(unchecked((ulong)(S.Nobjects[S.SUBCATCH]))));
            if (!(S.SubcatchStats != null))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
            for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
            {
                S.SubcatchStats[j].precip = 0.0;
                S.SubcatchStats[j].runon = 0.0;
                S.SubcatchStats[j].evap = 0.0;
                S.SubcatchStats[j].infil = 0.0;
                S.SubcatchStats[j].runoff = 0.0;
                S.SubcatchStats[j].maxFlow = 0.0;
                S.SubcatchStats[j].impervRunoff = 0.0;
                S.SubcatchStats[j].pervRunoff = 0.0;
            }
            for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
            {
                if (S.Subcatch[j].groundwater == null)
                {
                    continue;
                }
                S.Subcatch[j].groundwater.stats.avgUpperMoist = 0.0;
                S.Subcatch[j].groundwater.stats.avgWaterTable = 0.0;
                S.Subcatch[j].groundwater.stats.infil = 0.0;
                S.Subcatch[j].groundwater.stats.latFlow = 0.0;
                S.Subcatch[j].groundwater.stats.deepFlow = 0.0;
                S.Subcatch[j].groundwater.stats.evap = 0.0;
                S.Subcatch[j].groundwater.stats.maxFlow = 0.0;
                S.Subcatch[j].groundwater.stats.finalUpperMoist = 0.0;
                S.Subcatch[j].groundwater.stats.finalWaterTable = 0.0;
            }
        }
        if (S.Nobjects[S.LINK] > 0)
        {
            S.NodeStats = R.NewArray<TNodeStats>((int)(unchecked((ulong)(S.Nobjects[S.NODE]))));
            S.LinkStats = R.NewArray<TLinkStats>((int)(unchecked((ulong)(S.Nobjects[S.LINK]))));
            if ((!(S.NodeStats != null)) || (!(S.LinkStats != null)))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
        }
        if ((S.NodeStats != null))
        {
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                S.NodeStats[j].avgDepth = 0.0;
                S.NodeStats[j].maxDepth = 0.0;
                S.NodeStats[j].maxDepthDate = S.StartDateTime;
                S.NodeStats[j].maxRptDepth = 0.0;
                S.NodeStats[j].volFlooded = 0.0;
                S.NodeStats[j].timeFlooded = 0.0;
                S.NodeStats[j].timeSurcharged = 0.0;
                S.NodeStats[j].timeCourantCritical = 0.0;
                S.NodeStats[j].totLatFlow = 0.0;
                S.NodeStats[j].maxLatFlow = 0.0;
                S.NodeStats[j].maxInflow = 0.0;
                S.NodeStats[j].maxOverflow = 0.0;
                S.NodeStats[j].maxPondedVol = 0.0;
                S.NodeStats[j].nonConvergedCount = 0;
                S.NodeStats[j].maxInflowDate = S.StartDateTime;
                S.NodeStats[j].maxOverflowDate = S.StartDateTime;
            }
        }
        if ((S.LinkStats != null))
        {
            for (j = 0; j < S.Nobjects[S.LINK]; j++)
            {
                S.LinkStats[j].maxFlow = 0.0;
                S.LinkStats[j].maxVeloc = 0.0;
                S.LinkStats[j].maxDepth = 0.0;
                S.LinkStats[j].maxStreetFilled = 0.0;
                S.LinkStats[j].timeSurcharged = 0.0;
                S.LinkStats[j].timeFullUpstream = 0.0;
                S.LinkStats[j].timeFullDnstream = 0.0;
                S.LinkStats[j].timeFullFlow = 0.0;
                S.LinkStats[j].timeCapacityLimited = 0.0;
                S.LinkStats[j].timeCourantCritical = 0.0;
                for (k = 0; k < S.MAX_FLOW_CLASSES; k++)
                {
                    S.LinkStats[j].timeInFlowClass[k] = 0.0;
                }
                S.LinkStats[j].flowTurns = 0;
                S.LinkStats[j].flowTurnSign = 0;
            }
        }
        if (S.Nnodes[S.STORAGE] > 0)
        {
            S.StorageStats = R.NewArray<TStorageStats>((int)(unchecked((ulong)(S.Nnodes[S.STORAGE]))));
            if (!(S.StorageStats != null))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
            else
            {
                for (k = 0; k < S.Nobjects[S.NODE]; k++)
                {
                    if (S.Node[k].type != S.STORAGE)
                    {
                        continue;
                    }
                    j = S.Node[k].subIndex;
                    S.StorageStats[j].initVol = S.Node[k].newVolume;
                    S.StorageStats[j].avgVol = 0.0;
                    S.StorageStats[j].maxVol = 0.0;
                    S.StorageStats[j].maxFlow = 0.0;
                    S.StorageStats[j].evapLosses = 0.0;
                    S.StorageStats[j].exfilLosses = 0.0;
                    S.StorageStats[j].maxVolDate = S.StartDateTime;
                }
            }
        }
        if (S.Nnodes[S.OUTFALL] > 0)
        {
            S.OutfallStats = R.NewArray<TOutfallStats>((int)(unchecked((ulong)(S.Nnodes[S.OUTFALL]))));
            if (!(S.OutfallStats != null))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
            else
            {
                for (j = 0; j < S.Nnodes[S.OUTFALL]; j++)
                {
                    S.OutfallStats[j].avgFlow = 0.0;
                    S.OutfallStats[j].maxFlow = 0.0;
                    S.OutfallStats[j].totalPeriods = 0;
                    if (S.Nobjects[S.POLLUT] > 0)
                    {
                        S.OutfallStats[j].totalLoad = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
                        if (!(S.OutfallStats[j].totalLoad != null))
                        {
                            S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                            return S.ErrorCode;
                        }
                        for (k = 0; k < S.Nobjects[S.POLLUT]; k++)
                        {
                            S.OutfallStats[j].totalLoad[k] = 0.0;
                        }
                    }
                    else
                    {
                        S.OutfallStats[j].totalLoad = (null);
                    }
                }
            }
        }
        if (S.Nlinks[S.PUMP] > 0)
        {
            S.PumpStats = R.NewArray<TPumpStats>((int)(unchecked((ulong)(S.Nlinks[S.PUMP]))));
            if (!(S.PumpStats != null))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
            else
            {
                for (j = 0; j < S.Nlinks[S.PUMP]; j++)
                {
                    S.PumpStats[j].utilized = 0.0;
                    S.PumpStats[j].minFlow = 0.0;
                    S.PumpStats[j].avgFlow = 0.0;
                    S.PumpStats[j].maxFlow = 0.0;
                    S.PumpStats[j].volume = 0.0;
                    S.PumpStats[j].energy = 0.0;
                    S.PumpStats[j].startUps = 0;
                    S.PumpStats[j].offCurveLow = 0.0;
                    S.PumpStats[j].offCurveHigh = 0.0;
                }
            }
        }
        S.MaxRunoffFlow = 0.0;
        S.MaxOutfallFlow = 0.0;
        S.TimeStepStats.maxTimeStep = 0.0;
        S.TimeStepStats.minTimeStep = S.RouteStep;
        S.TimeStepStats.routingTime = 0.0;
        S.TimeStepStats.trialsCount = 0.0;
        S.TimeStepStats.steadyStateTime = 0.0;
        S.TimeStepStats.timeStepCount = 0;
        logMaxTimeStep = Math.Log10(S.RouteStep);
        logMinTimeStep = Math.Log10(S.MinRouteStep);
        timeStepDelta = (logMaxTimeStep - logMinTimeStep) / (double)((6 - 1));
        S.TimeStepStats.timeStepIntervals[0] = S.RouteStep;
        for (j = 1; j < 6; j++)
        {
            S.TimeStepStats.timeStepIntervals[j] = Math.Pow(10.0, logMaxTimeStep - j * timeStepDelta);
            S.TimeStepStats.timeStepCounts[j] = 0;
        }
        S.TimeStepStats.timeStepIntervals[6 - 1] = S.MinRouteStep;
        S.RoutingTimeSpan = 0.0;
        return 0;
    }

    // stats.c:318
    public static void stats_close()
    {
        int j = 0;
        {
            if ((S.SubcatchStats != null))
            {
                R.free(S.SubcatchStats);
                S.SubcatchStats = (null);
            }
        }
        ;
        {
            if ((S.NodeStats != null))
            {
                R.free(S.NodeStats);
                S.NodeStats = (null);
            }
        }
        ;
        {
            if ((S.LinkStats != null))
            {
                R.free(S.LinkStats);
                S.LinkStats = (null);
            }
        }
        ;
        {
            if ((S.StorageStats != null))
            {
                R.free(S.StorageStats);
                S.StorageStats = (null);
            }
        }
        ;
        if ((S.OutfallStats != null))
        {
            for (j = 0; j < S.Nnodes[S.OUTFALL]; j++)
            {
                if ((S.OutfallStats[j].totalLoad != null))
                {
                    R.free(S.OutfallStats[j].totalLoad);
                    S.OutfallStats[j].totalLoad = (null);
                }
            }
            ;
            {
                if ((S.OutfallStats != null))
                {
                    R.free(S.OutfallStats);
                    S.OutfallStats = (null);
                }
            }
            ;
        }
        {
            if ((S.PumpStats != null))
            {
                R.free(S.PumpStats);
                S.PumpStats = (null);
            }
        }
        ;
    }

    // stats.c:342
    public static void stats_report()
    {
        if ((S.Nobjects[S.LINK] > 0) && (S.RouteModel != S.NO_ROUTING))
        {
            S.stats_findMaxStats();
            if ((!(S.RptFlags.disabled != 0)) && (S.RptFlags.flowStats != 0))
            {
                S.report_writeMaxStats(S.MaxMassBalErrs, S.MaxCourantCrit, 5);
                S.report_writeMaxFlowTurns(S.MaxFlowTurns, 5);
                S.report_writeNonconvergedStats(S.MaxNonConverged, 5);
                S.report_writeTimeStepStats(S.TimeStepStats);
            }
        }
        if (!(S.RptFlags.disabled != 0))
        {
            S.statsrpt_writeReport();
        }
    }

    // stats.c:369
    public static void stats_updateSubcatchStats(int j, double rainVol, double runonVol, double evapVol, double infilVol, double impervVol, double pervVol, double runoffVol, double runoff)
    {
        S.SubcatchStats[j].precip += rainVol;
        S.SubcatchStats[j].runon += runonVol;
        S.SubcatchStats[j].evap += evapVol;
        S.SubcatchStats[j].infil += infilVol;
        S.SubcatchStats[j].runoff += runoffVol;
        S.SubcatchStats[j].maxFlow = ((((S.SubcatchStats[j].maxFlow) >= (runoff)) ? (S.SubcatchStats[j].maxFlow) : (runoff)));
        S.SubcatchStats[j].impervRunoff += impervVol;
        S.SubcatchStats[j].pervRunoff += pervVol;
    }

    // stats.c:399
    public static void stats_updateGwaterStats(int j, double infil, double evap, double latFlow, double deepFlow, double theta, double waterTable, double tStep)
    {
        S.Subcatch[j].groundwater.stats.infil += infil * tStep;
        S.Subcatch[j].groundwater.stats.evap += evap * tStep;
        S.Subcatch[j].groundwater.stats.latFlow += latFlow * tStep;
        S.Subcatch[j].groundwater.stats.deepFlow += deepFlow * tStep;
        S.Subcatch[j].groundwater.stats.avgUpperMoist += theta * tStep;
        S.Subcatch[j].groundwater.stats.avgWaterTable += waterTable * tStep;
        S.Subcatch[j].groundwater.stats.finalUpperMoist = theta;
        S.Subcatch[j].groundwater.stats.finalWaterTable = waterTable;
        if (Math.Abs(latFlow) > Math.Abs(S.Subcatch[j].groundwater.stats.maxFlow))
        {
            S.Subcatch[j].groundwater.stats.maxFlow = latFlow;
        }
    }

    // stats.c:419
    public static void stats_updateMaxRunoff()
    {
        int j = 0;
        double sysRunoff = 0.0;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            sysRunoff += S.Subcatch[j].newRunoff;
        }
        S.MaxRunoffFlow = ((((S.MaxRunoffFlow) >= (sysRunoff)) ? (S.MaxRunoffFlow) : (sysRunoff)));
    }

    // stats.c:435
    public static void stats_updateMaxNodeDepth(int j, double depth)
    {
        if (S.NodeStats != null)
        {
            S.NodeStats[j].maxRptDepth = ((((S.NodeStats[j].maxRptDepth) >= (depth)) ? (S.NodeStats[j].maxRptDepth) : (depth)));
        }
    }

    // stats.c:449
    public static void stats_updateFlowStats(double tStep, double aDate)
    {
        int j = 0;
        if (aDate < S.ReportStart)
        {
            return;
        }
        S.SysOutfallFlow = 0.0;
        {
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                S.stats_updateNodeStats(j, tStep, aDate);
            }
            for (j = 0; j < S.Nobjects[S.LINK]; j++)
            {
                S.stats_updateLinkStats(j, tStep, aDate);
            }
        }
        S.ReportStepCount++;
        S.RoutingTimeSpan += tStep;
        S.MaxOutfallFlow = ((((S.MaxOutfallFlow) >= (S.SysOutfallFlow)) ? (S.MaxOutfallFlow) : (S.SysOutfallFlow)));
    }

    // stats.c:484
    public static void stats_updateTimeStepStats(double tStep, int trialsCount, int steadyState)
    {
        int j = 0;
        S.TimeStepStats.steadyStateTime += steadyState * tStep;
        if (steadyState == 0)
        {
            if (S.OldRoutingTime > 0)
            {
                S.TimeStepStats.minTimeStep = ((((S.TimeStepStats.minTimeStep) <= (tStep)) ? (S.TimeStepStats.minTimeStep) : (tStep)));
                for (j = 1; j < 6; j++)
                {
                    if (tStep >= S.TimeStepStats.timeStepIntervals[j])
                    {
                        S.TimeStepStats.timeStepCounts[j]++;
                        break;
                    }
                }
            }
            S.TimeStepStats.maxTimeStep = ((((S.TimeStepStats.maxTimeStep) >= (tStep)) ? (S.TimeStepStats.maxTimeStep) : (tStep)));
            S.TimeStepStats.routingTime += tStep;
            S.TimeStepStats.timeStepCount++;
            S.TimeStepStats.trialsCount += trialsCount;
        }
    }

    // stats.c:522
    public static void stats_updateCriticalTimeCount(int node, int link)
    {
        if (node >= 0)
        {
            S.NodeStats[node].timeCourantCritical += 1.0;
        }
        else if (link >= 0)
        {
            S.LinkStats[link].timeCourantCritical += 1.0;
        }
    }

    // stats.c:536
    public static void stats_updateConvergenceStats(int node, int converged)
    {
        if (converged == 0)
        {
            S.NodeStats[node].nonConvergedCount++;
        }
    }

    // stats.c:543
    public static void stats_updateNodeStats(int j, double tStep, double aDate)
    {
        int k = 0;
        int p = 0;
        double newVolume = S.Node[j].newVolume;
        double newDepth = S.Node[j].newDepth;
        int canPond = (((S.AllowPonding != 0) && (S.Node[j].pondedArea > 0.0)) ? 1 : 0);
        S.NodeStats[j].avgDepth += newDepth;
        if (newDepth > S.NodeStats[j].maxDepth)
        {
            S.NodeStats[j].maxDepth = newDepth;
            S.NodeStats[j].maxDepthDate = aDate;
        }
        if (S.Node[j].type != S.OUTFALL)
        {
            if ((newVolume > S.Node[j].fullVolume) || (S.Node[j].overflow > 0.0))
            {
                S.NodeStats[j].timeFlooded += tStep;
                S.NodeStats[j].volFlooded += S.Node[j].overflow * tStep;
                if ((canPond != 0))
                {
                    S.NodeStats[j].maxPondedVol = ((((S.NodeStats[j].maxPondedVol) >= ((newVolume - S.Node[j].fullVolume))) ? (S.NodeStats[j].maxPondedVol) : ((newVolume - S.Node[j].fullVolume))));
                }
            }
            if (S.RouteModel == S.DW)
            {
                if ((((S.Node[j].type != S.STORAGE) || (S.Node[j].surDepth > 0.0))) && (newDepth + S.Node[j].invertElev + 0.0001 >= S.Node[j].crownElev))
                {
                    S.NodeStats[j].timeSurcharged += tStep;
                }
            }
        }
        if (S.Node[j].type == S.STORAGE)
        {
            k = S.Node[j].subIndex;
            S.StorageStats[k].avgVol += newVolume;
            S.StorageStats[k].evapLosses += S.Storage[S.Node[j].subIndex].evapLoss;
            S.StorageStats[k].exfilLosses += S.Storage[S.Node[j].subIndex].exfilLoss;
            newVolume = ((((newVolume) <= (S.Node[j].fullVolume)) ? (newVolume) : (S.Node[j].fullVolume)));
            if (newVolume > S.StorageStats[k].maxVol)
            {
                S.StorageStats[k].maxVol = newVolume;
                S.StorageStats[k].maxVolDate = aDate;
            }
            S.StorageStats[k].maxFlow = ((((S.StorageStats[k].maxFlow) >= (S.Node[j].outflow)) ? (S.StorageStats[k].maxFlow) : (S.Node[j].outflow)));
        }
        if (S.Node[j].type == S.OUTFALL)
        {
            k = S.Node[j].subIndex;
            if (S.Node[j].inflow >= 0.001)
            {
                S.OutfallStats[k].avgFlow += S.Node[j].inflow;
                S.OutfallStats[k].maxFlow = ((((S.OutfallStats[k].maxFlow) >= (S.Node[j].inflow)) ? (S.OutfallStats[k].maxFlow) : (S.Node[j].inflow)));
                S.OutfallStats[k].totalPeriods++;
            }
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                S.OutfallStats[k].totalLoad[p] += S.Node[j].inflow * S.Node[j].newQual[p] * tStep;
            }
            S.SysOutfallFlow += S.Node[j].inflow;
        }
        S.NodeStats[j].totLatFlow += ((S.Node[j].oldLatFlow + S.Node[j].newLatFlow) * 0.5 * tStep);
        if (Math.Abs(S.Node[j].newLatFlow) > Math.Abs(S.NodeStats[j].maxLatFlow))
        {
            S.NodeStats[j].maxLatFlow = S.Node[j].newLatFlow;
        }
        if (S.Node[j].inflow > S.NodeStats[j].maxInflow)
        {
            S.NodeStats[j].maxInflow = S.Node[j].inflow;
            S.NodeStats[j].maxInflowDate = aDate;
        }
        if (S.Node[j].overflow > S.NodeStats[j].maxOverflow)
        {
            S.NodeStats[j].maxOverflow = S.Node[j].overflow;
            S.NodeStats[j].maxOverflowDate = aDate;
        }
    }

    // stats.c:647
    public static void stats_updateLinkStats(int j, double tStep, double aDate)
    {
        int k = 0;
        double q = 0.0;
        double v = 0.0;
        double dq = 0.0;
        dq = S.Link[j].newFlow - S.Link[j].oldFlow;
        q = Math.Abs(S.Link[j].newFlow);
        if (q > S.LinkStats[j].maxFlow)
        {
            S.LinkStats[j].maxFlow = q;
            S.LinkStats[j].maxFlowDate = aDate;
        }
        v = S.link_getVelocity(j, q, S.Link[j].newDepth);
        if (v > S.LinkStats[j].maxVeloc)
        {
            S.LinkStats[j].maxVeloc = v;
        }
        if (S.Link[j].newDepth > S.LinkStats[j].maxDepth)
        {
            S.LinkStats[j].maxDepth = S.Link[j].newDepth;
        }
        if (S.Link[j].type == S.PUMP)
        {
            if (q >= S.Link[j].qFull)
            {
                S.LinkStats[j].timeFullFlow += tStep;
            }
            if (q > 0.001)
            {
                k = S.Link[j].subIndex;
                S.PumpStats[k].minFlow = ((((S.PumpStats[k].minFlow) <= (q)) ? (S.PumpStats[k].minFlow) : (q)));
                S.PumpStats[k].maxFlow = S.LinkStats[j].maxFlow;
                S.PumpStats[k].avgFlow += q;
                S.PumpStats[k].volume += q * tStep;
                S.PumpStats[k].utilized += tStep;
                S.PumpStats[k].energy += S.link_getPower(j) * tStep / 3600.0;
                if (S.Link[j].flowClass == S.DN_DRY)
                {
                    S.PumpStats[k].offCurveLow += tStep;
                }
                if (S.Link[j].flowClass == S.UP_DRY)
                {
                    S.PumpStats[k].offCurveHigh += tStep;
                }
                if (S.Link[j].oldFlow < 0.001)
                {
                    S.PumpStats[k].startUps++;
                }
                S.PumpStats[k].totalPeriods++;
                S.LinkStats[j].timeSurcharged += tStep;
                S.LinkStats[j].timeFullUpstream += tStep;
                S.LinkStats[j].timeFullDnstream += tStep;
            }
        }
        else if (S.Link[j].type == S.CONDUIT)
        {
            if ((S.Link[j].normalFlow != 0))
            {
                S.LinkStats[j].timeNormalFlow += tStep;
            }
            if ((S.Link[j].inletControl != 0))
            {
                S.LinkStats[j].timeInletControl += tStep;
            }
            k = S.Link[j].flowClass;
            if ((k >= 0) && (k < S.MAX_FLOW_CLASSES))
            {
                S.LinkStats[j].timeInFlowClass[k] += tStep;
            }
            k = S.Link[j].subIndex;
            if (q >= S.Link[j].qFull * (double)(S.Conduit[k].barrels))
            {
                S.LinkStats[j].timeFullFlow += tStep;
            }
            if ((S.Conduit[k].capacityLimited != 0))
            {
                S.LinkStats[j].timeCapacityLimited += tStep;
            }
            switch (S.Conduit[k].fullState)
            {
                case S.ALL_FULL:
                    S.LinkStats[j].timeSurcharged += tStep;
                    S.LinkStats[j].timeFullUpstream += tStep;
                    S.LinkStats[j].timeFullDnstream += tStep;
                    break;
                case S.UP_FULL:
                    S.LinkStats[j].timeFullUpstream += tStep;
                    break;
                case S.DN_FULL:
                    S.LinkStats[j].timeFullDnstream += tStep;
                    break;
            }
            if (S.Link[j].xsect.type == S.STREET_XSECT)
            {
                S.LinkStats[j].maxStreetFilled = ((((S.LinkStats[j].maxStreetFilled) >= (S.street_getExtentFilled(j))) ? (S.LinkStats[j].maxStreetFilled) : (S.street_getExtentFilled(j))));
            }
        }
        k = S.LinkStats[j].flowTurnSign;
        S.LinkStats[j].flowTurnSign = ((((dq) < 0) ? (-1) : (1)));
        if ((Math.Abs(dq) > 0.001) && (k * S.LinkStats[j].flowTurnSign < 0))
        {
            S.LinkStats[j].flowTurns++;
        }
    }

    // stats.c:756
    public static void stats_findMaxStats()
    {
        int j = 0;
        double x = 0.0;
        double z = 0.0;
        double stepCount = 0.0;
        for (j = 0; j < 5; j++)
        {
            S.MaxMassBalErrs[j].objType = S.NODE;
            S.MaxMassBalErrs[j].index = -1;
            S.MaxMassBalErrs[j].value = -1.0;
            S.MaxCourantCrit[j].index = -1;
            S.MaxCourantCrit[j].value = -1.0;
            S.MaxFlowTurns[j].index = -1;
            S.MaxFlowTurns[j].value = -1.0;
            S.MaxNonConverged[j].index = -1;
            S.MaxNonConverged[j].value = 0.0;
        }
        if (S.ReportStepCount > 2)
        {
            stepCount = S.ReportStepCount;
            z = 100.0 / (2.0 / 3.0 * (stepCount - 2.0));
            for (j = 0; j < S.Nobjects[S.LINK]; j++)
            {
                x = S.LinkStats[j].flowTurns * z;
                S.stats_updateMaxStats(S.MaxFlowTurns, S.LINK, j, x);
            }
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if (S.Node[j].degree <= 0)
            {
                continue;
            }
            if (S.NodeInflow[j] <= 0.1)
            {
                continue;
            }
            if (S.NodeInflow[j] > 0.0)
            {
                x = 1.0 - S.NodeOutflow[j] / S.NodeInflow[j];
            }
            else if (S.NodeOutflow[j] > 0.0)
            {
                x = -1.0;
            }
            else
            {
                x = 0.0;
            }
            S.stats_updateMaxStats(S.MaxMassBalErrs, S.NODE, j, 100.0 * x);
        }
        stepCount = S.TimeStepStats.timeStepCount;
        if (S.RouteModel == S.DW)
        {
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                S.stats_updateMaxStats(S.MaxNonConverged, S.NODE, j, S.NodeStats[j].nonConvergedCount / stepCount);
            }
        }
        if ((S.RouteModel != S.DW) || (S.CourantFactor == 0.0))
        {
            return;
        }
        if (stepCount == 0)
        {
            return;
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            x = S.NodeStats[j].timeCourantCritical / stepCount;
            S.stats_updateMaxStats(S.MaxCourantCrit, S.NODE, j, 100.0 * x);
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            x = S.LinkStats[j].timeCourantCritical / stepCount;
            S.stats_updateMaxStats(S.MaxCourantCrit, S.LINK, j, 100.0 * x);
        }
    }

    // stats.c:841
    public static void stats_updateMaxStats(TMaxStats[] maxStats, int i, int j, double x)
    {
        int k = 0;
        TMaxStats maxStats1 = new TMaxStats();
        TMaxStats maxStats2 = new TMaxStats();
        maxStats1.objType = i;
        maxStats1.index = j;
        maxStats1.value = x;
        for (k = 0; k < 5; k++)
        {
            if (Math.Abs(maxStats1.value) > Math.Abs(maxStats[k].value))
            {
                maxStats2 = maxStats[k].Clone();
                maxStats[k] = maxStats1.Clone();
                maxStats1 = maxStats2.Clone();
            }
        }
    }

}
}
