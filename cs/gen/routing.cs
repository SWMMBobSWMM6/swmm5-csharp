// Generated from EPA SWMM 5.2.4 (routing.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int[] SortedLinks = null;
    public static int NextEvent = 0;
    public static int BetweenEvents = 0;
    public static double NewRuleTime = 0.0;

    // routing.c:93
    public static int routing_open()
    {
        if (!((S.treatmnt_open() != 0)))
        {
            return S.ErrorCode;
        }
        S.SortedLinks = (null);
        if (S.Nobjects[S.LINK] > 0)
        {
            S.SortedLinks = new int[(int)(unchecked((ulong)(S.Nobjects[S.LINK])))];
            if (!(S.SortedLinks != null))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return S.ErrorCode;
            }
            S.toposort_sortLinks(S.SortedLinks);
            if ((S.ErrorCode != 0))
            {
                return S.ErrorCode;
            }
        }
        S.iface_openRoutingFiles();
        S.flowrout_init(S.RouteModel);
        if (S.Fhotstart1.mode == S.NO_FILE)
        {
            S.qualrout_init();
        }
        if (S.NumEvents > 0)
        {
            S.sortEvents();
        }
        S.NextEvent = 0;
        S.BetweenEvents = ((S.NumEvents > 0) ? 1 : 0);
        S.NewRuleTime = 0.0;
        return S.ErrorCode;
    }

    // routing.c:134
    public static void routing_close(int routingModel)
    {
        S.iface_closeRoutingFiles();
        S.flowrout_close(routingModel);
        S.treatmnt_close();
        {
            if ((S.SortedLinks != null))
            {
                R.free(S.SortedLinks);
                S.SortedLinks = (null);
            }
        }
        ;
    }

    // routing.c:152
    public static double routing_getRoutingStep(int routingModel, double fixedStep)
    {
        double date1 = 0.0;
        double date2 = 0.0;
        double nextTime = 0.0;
        double routingStep = 0.0;
        double nextRuleTime = 0.0;
        double nextRoutingTime = 0.0;
        if (S.Nobjects[S.LINK] == 0)
        {
            return fixedStep;
        }
        if ((S.NumEvents > 0) && (S.BetweenEvents != 0))
        {
            nextTime = ((((S.NewRunoffTime) <= (S.ReportTime)) ? (S.NewRunoffTime) : (S.ReportTime)));
            date1 = S.getDateTime(S.NewRoutingTime);
            date2 = S.getDateTime(nextTime);
            if ((date2 > date1) && (date2 < S.Event[S.NextEvent].start))
            {
                routingStep = (nextTime - S.NewRoutingTime) / 1000.0;
            }
            else
            {
                date1 = S.getDateTime(S.NewRoutingTime + 1000.0 * fixedStep);
                if (date1 < S.Event[S.NextEvent].start)
                {
                    return fixedStep;
                }
            }
        }
        if (routingStep == 0.0)
        {
            routingStep = S.flowrout_getRoutingStep(routingModel, fixedStep);
        }
        if (S.RuleStep > 0)
        {
            nextRuleTime = S.NewRuleTime + 1000.0 * S.RuleStep;
            nextRoutingTime = S.NewRoutingTime + 1000.0 * routingStep;
            if (nextRoutingTime >= nextRuleTime)
            {
                routingStep = (nextRuleTime - S.NewRoutingTime) / 1000.0;
            }
        }
        return routingStep;
    }

    // routing.c:203
    public static void routing_execute(int routingModel, double routingStep)
    {
        int trialsCount = 1;
        int actionCount = 0;
        int inSteadyState = 1;
        double currentDate = 0.0;
        double stepFlowError = 0.0;
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.massbal_updateRoutingTotals(routingStep / 2.0);
        currentDate = S.getDateTime(S.NewRoutingTime);
        actionCount = S.evaluateControlRules(currentDate, routingStep);
        stepFlowError = S.massbal_getStepFlowError();
        S.massbal_initTimeStepTotals();
        S.initSystemInflows();
        S.BetweenEvents = S.isBetweenEvents(currentDate);
        if (S.BetweenEvents == 0)
        {
            S.addSystemInflows(currentDate, routingStep);
            S.inlet_findCapturedFlows(routingStep);
            inSteadyState = S.isInSteadyState(actionCount, stepFlowError);
            if (inSteadyState == 0)
            {
                trialsCount = S.routeFlow(routingModel, routingStep);
            }
            if ((S.Nobjects[S.POLLUT] > 0) && (!(S.IgnoreQuality != 0)))
            {
                S.inlet_adjustQualInflows();
                S.qualrout_execute(routingStep);
            }
            S.removeSystemOutflows(routingStep);
            S.inlet_adjustQualOutflows();
            if (S.Nobjects[S.LINK] > 0)
            {
                S.stats_updateFlowStats(routingStep, S.getDateTime(S.NewRoutingTime));
                S.stats_updateTimeStepStats(routingStep, trialsCount, inSteadyState);
            }
        }
        S.massbal_updateRoutingTotals(routingStep / 2.0);
    }

    // routing.c:269
    public static int evaluateControlRules(double currentDate, double routingStep)
    {
        int j = 0;
        int actionCount = 0;
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.link_setTargetSetting(j);
        }
        if ((S.RuleStep == 0) || (Math.Abs(S.NewRoutingTime - S.NewRuleTime) < 1.0))
        {
            S.controls_evaluate(currentDate, currentDate - S.StartDateTime, routingStep / 86400);
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if (S.Link[j].targetSetting != S.Link[j].setting)
            {
                if (S.Link[j].targetSetting * S.Link[j].setting == 0.0)
                {
                    S.Link[j].timeLastSet = currentDate;
                }
                S.link_setSetting(j, routingStep);
                actionCount++;
            }
        }
        S.OldRoutingTime = S.NewRoutingTime;
        S.NewRoutingTime = S.NewRoutingTime + 1000.0 * routingStep;
        if (Math.Abs(S.NewRoutingTime - (S.NewRuleTime + 1000.0 * S.RuleStep)) < 1)
        {
            S.NewRuleTime += 1000.0 * S.RuleStep;
        }
        return actionCount;
    }

    // routing.c:312
    public static void initSystemInflows()
    {
        int j = 0;
        if (S.Nobjects[S.POLLUT] > 0)
        {
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                S.node_setOldQualState(j);
            }
            for (j = 0; j < S.Nobjects[S.LINK]; j++)
            {
                S.link_setOldQualState(j);
            }
        }
        S.infil_setInfilFactor(-1);
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.Node[j].oldLatFlow = S.Node[j].newLatFlow;
            S.Node[j].newLatFlow = 0.0;
        }
    }

    // routing.c:337
    public static int isBetweenEvents(double currentDate)
    {
        if (S.NumEvents == 0)
        {
            return 0;
        }
        if (currentDate > S.Event[S.NextEvent].end)
        {
            S.NextEvent++;
            return 1;
        }
        else if (currentDate >= S.Event[S.NextEvent].start)
        {
            return 0;
        }
        return 1;
    }

    // routing.c:359
    public static void addSystemInflows(double currentDate, double routingStep)
    {
        int j = 0;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.Node[j].losses = S.node_getLosses(j, routingStep);
        }
        S.addExternalInflows(currentDate);
        S.addDryWeatherInflows(currentDate);
        S.addWetWeatherInflows(S.OldRoutingTime);
        S.addGroundwaterInflows(S.OldRoutingTime);
        S.addLidDrainInflows(S.OldRoutingTime);
        S.addRdiiInflows(currentDate);
        S.addIfaceInflows(currentDate);
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.Node[j].qualInflow = ((((0.0) >= (S.Node[j].newLatFlow)) ? (0.0) : (S.Node[j].newLatFlow)));
        }
    }

    // routing.c:383
    public static int isInSteadyState(int actionCount, double stepFlowError)
    {
        if ((S.SkipSteadyState != 0))
        {
            if ((((S.OldRoutingTime == 0.0) || (actionCount > 0)) || (Math.Abs(stepFlowError) > S.SysFlowTol)) || ((S.inflowHasChanged() != 0)))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        return 0;
    }

    // routing.c:399
    public static int routeFlow(int routingModel, double routingStep)
    {
        int j = 0;
        int stepCount = 1;
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.link_setOldHydState(j);
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.node_setOldHydState(j);
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.node_initFlows(j, routingStep);
        }
        if (S.Nobjects[S.LINK] > 0)
        {
            stepCount = S.flowrout_execute(S.SortedLinks, routingModel, routingStep);
        }
        return stepCount;
    }

    // routing.c:425
    public static void removeSystemOutflows(double routingStep)
    {
        S.removeStorageLosses(routingStep);
        S.removeConduitLosses();
        S.removeOutflows(routingStep);
    }

    // routing.c:435
    public static void addExternalInflows(double currentDate)
    {
        int j = 0;
        int p = 0;
        double q = 0.0;
        double w = 0.0;
        TExtInflow inflow = null;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            q = S.Node[j].apiExtInflow;
            inflow = S.Node[j].extInflow;
            while ((inflow != null))
            {
                if (inflow.type == S.FLOW_INFLOW)
                {
                    q += S.inflow_getExtInflow(inflow, currentDate);
                    break;
                }
                else
                {
                    inflow = inflow.next;
                }
            }
            if (Math.Abs(q) < 1e-05)
            {
                q = 0.0;
            }
            S.Node[j].newLatFlow += q;
            if (q >= 0.0)
            {
                S.massbal_addInflowFlow(S.EXTERNAL_INFLOW, q);
            }
            else
            {
                S.massbal_addOutflowFlow(-q, 0);
                continue;
            }
            if ((S.Node[j].type == S.OUTFALL) && (S.Node[j].oldNetInflow < 0.0))
            {
                q = q - S.Node[j].oldNetInflow;
            }
            inflow = S.Node[j].extInflow;
            while ((inflow != null))
            {
                if (inflow.type != S.FLOW_INFLOW)
                {
                    p = inflow.param;
                    w = S.inflow_getExtInflow(inflow, currentDate);
                    if (inflow.type == S.CONCEN_INFLOW)
                    {
                        w *= q;
                    }
                    S.Node[j].newQual[p] += w;
                    S.massbal_addInflowQual(S.EXTERNAL_INFLOW, p, w);
                }
                inflow = inflow.next;
            }
        }
    }

    // routing.c:498
    public static void addDryWeatherInflows(double currentDate)
    {
        int j = 0;
        int p = 0;
        int month = 0;
        int day = 0;
        int hour = 0;
        double q = 0.0;
        double w = 0.0;
        TDwfInflow inflow = null;
        month = S.datetime_monthOfYear(currentDate) - 1;
        day = S.datetime_dayOfWeek(currentDate) - 1;
        hour = S.datetime_hourOfDay(currentDate);
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            inflow = S.Node[j].dwfInflow;
            if (!(inflow != null))
            {
                continue;
            }
            q = 0.0;
            while ((inflow != null))
            {
                if (inflow.param < 0)
                {
                    q = S.inflow_getDwfInflow(inflow, month, day, hour);
                    break;
                }
                inflow = inflow.next;
            }
            if (Math.Abs(q) < 1e-05)
            {
                q = 0.0;
            }
            S.Node[j].newLatFlow += q;
            S.massbal_addInflowFlow(S.DRY_WEATHER_INFLOW, q);
            if (q <= 0.0)
            {
                continue;
            }
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                if (S.Pollut[p].dwfConcen > 0.0)
                {
                    w = q * S.Pollut[p].dwfConcen;
                    S.Node[j].newQual[p] += w;
                    S.massbal_addInflowQual(S.DRY_WEATHER_INFLOW, p, w);
                }
            }
            inflow = S.Node[j].dwfInflow;
            while ((inflow != null))
            {
                if (inflow.param >= 0)
                {
                    p = inflow.param;
                    w = q * S.inflow_getDwfInflow(inflow, month, day, hour);
                    S.Node[j].newQual[p] += w;
                    S.massbal_addInflowQual(S.DRY_WEATHER_INFLOW, p, w);
                    if (S.Pollut[p].dwfConcen > 0.0)
                    {
                        w = q * S.Pollut[p].dwfConcen;
                        S.Node[j].newQual[p] -= w;
                        S.massbal_addInflowQual(S.DRY_WEATHER_INFLOW, p, -w);
                    }
                }
                inflow = inflow.next;
            }
        }
    }

    // routing.c:579
    public static void addWetWeatherInflows(double routingTime)
    {
        int i = 0;
        int j = 0;
        int p = 0;
        double q = 0.0;
        double w = 0.0;
        double f = 0.0;
        if (S.Nobjects[S.SUBCATCH] == 0)
        {
            return;
        }
        f = (routingTime - S.OldRunoffTime) / (S.NewRunoffTime - S.OldRunoffTime);
        if (f < 0.0)
        {
            f = 0.0;
        }
        if (f > 1.0)
        {
            f = 1.0;
        }
        for (i = 0; i < S.Nobjects[S.SUBCATCH]; i++)
        {
            j = S.Subcatch[i].outNode;
            if (j >= 0)
            {
                q = S.subcatch_getWtdOutflow(i, f);
                S.Node[j].newLatFlow += q;
                S.massbal_addInflowFlow(S.WET_WEATHER_INFLOW, q);
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    w = S.surfqual_getWtdWashoff(i, p, f);
                    S.Node[j].newQual[p] += w;
                    S.massbal_addInflowQual(S.WET_WEATHER_INFLOW, p, w);
                }
            }
        }
    }

    // routing.c:621
    public static void addGroundwaterInflows(double routingTime)
    {
        int i = 0;
        int j = 0;
        int p = 0;
        double q = 0.0;
        double w = 0.0;
        double f = 0.0;
        TGroundwater gw = null;
        if (S.Nobjects[S.SUBCATCH] == 0)
        {
            return;
        }
        f = (routingTime - S.OldRunoffTime) / (S.NewRunoffTime - S.OldRunoffTime);
        if (f < 0.0)
        {
            f = 0.0;
        }
        if (f > 1.0)
        {
            f = 1.0;
        }
        for (i = 0; i < S.Nobjects[S.SUBCATCH]; i++)
        {
            gw = S.Subcatch[i].groundwater;
            if ((gw != null))
            {
                j = gw.node;
                if (j >= 0)
                {
                    q = ((1.0 - f) * (gw.oldFlow) + f * (gw.newFlow)) * S.Subcatch[i].area;
                    if (Math.Abs(q) < 1e-05)
                    {
                        continue;
                    }
                    S.Node[j].newLatFlow += q;
                    S.massbal_addInflowFlow(S.GROUNDWATER_INFLOW, q);
                    if (q > 0.0)
                    {
                        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                        {
                            w = q * S.Pollut[p].gwConcen;
                            S.Node[j].newQual[p] += w;
                            S.massbal_addInflowQual(S.GROUNDWATER_INFLOW, p, w);
                        }
                    }
                }
            }
        }
    }

    // routing.c:674
    public static void addLidDrainInflows(double routingTime)
    {
        int j = 0;
        double f = 0.0;
        if (S.Nobjects[S.SUBCATCH] == 0)
        {
            return;
        }
        f = (routingTime - S.OldRunoffTime) / (S.NewRunoffTime - S.OldRunoffTime);
        if (f < 0.0)
        {
            f = 0.0;
        }
        if (f > 1.0)
        {
            f = 1.0;
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            if ((S.Subcatch[j].area > 0.0) && (S.Subcatch[j].lidArea > 0.0))
            {
                S.lid_addDrainInflow(j, f);
            }
        }
    }

    // routing.c:698
    public static void addRdiiInflows(double currentDate)
    {
        int i = 0;
        int j = 0;
        int p = 0;
        double q = 0.0;
        double w = 0.0;
        int numRdiiNodes = 0;
        numRdiiNodes = S.rdii_getNumRdiiFlows(currentDate);
        for (i = 0; i < numRdiiNodes; i++)
        {
            S.rdii_getRdiiFlow(i, ref j, ref q);
            if (j < 0)
            {
                continue;
            }
            if (Math.Abs(q) < 1e-05)
            {
                continue;
            }
            S.Node[j].newLatFlow += q;
            S.massbal_addInflowFlow(S.RDII_INFLOW, q);
            if (q > 0.0)
            {
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    w = q * S.Pollut[p].rdiiConcen;
                    S.Node[j].newQual[p] += w;
                    S.massbal_addInflowQual(S.RDII_INFLOW, p, w);
                }
            }
        }
    }

    // routing.c:736
    public static void addIfaceInflows(double currentDate)
    {
        int i = 0;
        int j = 0;
        int p = 0;
        double q = 0.0;
        double w = 0.0;
        int numIfaceNodes = 0;
        if (S.Finflows.mode != S.USE_FILE)
        {
            return;
        }
        numIfaceNodes = S.iface_getNumIfaceNodes(currentDate);
        for (i = 0; i < numIfaceNodes; i++)
        {
            j = S.iface_getIfaceNode(i);
            if (j < 0)
            {
                continue;
            }
            q = S.iface_getIfaceFlow(i);
            if (Math.Abs(q) < 1e-05)
            {
                continue;
            }
            S.Node[j].newLatFlow += q;
            S.massbal_addInflowFlow(S.EXTERNAL_INFLOW, q);
            if (q > 0.0)
            {
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    w = q * S.iface_getIfaceQual(i, p);
                    S.Node[j].newQual[p] += w;
                    S.massbal_addInflowQual(S.EXTERNAL_INFLOW, p, w);
                }
            }
        }
    }

    // routing.c:776
    public static int inflowHasChanged()
    {
        int j = 0;
        double diff = 0.0;
        double qOld = 0.0;
        double qNew = 0.0;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            qOld = S.Node[j].oldLatFlow;
            qNew = S.Node[j].newLatFlow;
            if (Math.Abs(qOld) > 1e-06)
            {
                diff = (qNew / qOld) - 1.0;
            }
            else if (Math.Abs(qNew) > 1e-06)
            {
                diff = 1.0;
            }
            else
            {
                diff = 0.0;
            }
            if (Math.Abs(diff) > S.LatFlowTol)
            {
                return 1;
            }
            if ((S.Node[j].type == S.OUTFALL) || (S.Node[j].degree == 0))
            {
                qOld = S.Node[j].oldFlowInflow;
                qNew = S.Node[j].inflow;
                if (Math.Abs(qOld) > 1e-06)
                {
                    diff = (qNew / qOld) - 1.0;
                }
                else if (Math.Abs(qNew) > 1e-06)
                {
                    diff = 1.0;
                }
                else
                {
                    diff = 0.0;
                }
                if (Math.Abs(diff) > S.LatFlowTol)
                {
                    return 1;
                }
            }
        }
        return 0;
    }

    // routing.c:812
    public static void removeStorageLosses(double tStep)
    {
        int i = 0;
        double evapLoss = 0.0;
        double exfilLoss = 0.0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (S.Node[i].type == S.STORAGE)
            {
                evapLoss += S.Storage[S.Node[i].subIndex].evapLoss;
                exfilLoss += S.Storage[S.Node[i].subIndex].exfilLoss;
            }
        }
        S.massbal_addNodeLosses(evapLoss / tStep, exfilLoss / tStep);
    }

    // routing.c:841
    public static void removeConduitLosses()
    {
        int i = 0;
        int k = 0;
        double barrels = 0.0;
        double evapLoss = 0.0;
        double seepLoss = 0.0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (S.Link[i].type == S.CONDUIT)
            {
                k = S.Link[i].subIndex;
                barrels = S.Conduit[k].barrels;
                evapLoss += S.Conduit[k].evapLossRate * barrels;
                seepLoss += S.Conduit[k].seepLossRate * barrels;
            }
        }
        S.massbal_addLinkLosses(evapLoss, seepLoss);
    }

    // routing.c:872
    public static void removeOutflows(double tStep)
    {
        int i = 0;
        int p = 0;
        int k = 0;
        int isFlooded = 0;
        double q = 0.0;
        double w = 0.0;
        double v = 0.0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if ((S.Node[i].type == S.OUTFALL) && (S.Node[i].inflow > 0.0))
            {
                k = S.Node[i].subIndex;
                if (S.Outfall[k].routeTo >= 0)
                {
                    v = S.Node[i].inflow * tStep;
                    S.Outfall[k].vRouted += v;
                    for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                    {
                        S.Outfall[k].wRouted[p] += S.Node[i].newQual[p] * v;
                    }
                }
            }
            q = S.node_getSystemOutflow(i, ref isFlooded);
            if (q > 0.0)
            {
                S.massbal_addOutflowFlow(q, isFlooded);
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    w = q * S.Node[i].newQual[p];
                    S.massbal_addOutflowQual(p, w, isFlooded);
                }
            }
            else
            {
                S.massbal_addInflowFlow(S.EXTERNAL_INFLOW, -q);
            }
            q = S.Node[i].newLatFlow;
            if (q < 0.0)
            {
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    w = -q * S.Node[i].newQual[p];
                    S.massbal_addOutflowQual(p, w, 0);
                }
            }
        }
    }

    // routing.c:929
    public static void sortEvents()
    {
        int i = 0;
        int j = 0;
        int gap = 0;
        TEvent temp = new TEvent();
        for (gap = S.NumEvents / 2; gap >= 1; gap /= 2)
        {
            for (i = gap; i < S.NumEvents; i += gap)
            {
                temp = S.Event[i].Clone();
                j = i - gap;
                while ((j >= 0) && (S.Event[j].start > temp.start))
                {
                    S.Event[j + gap] = S.Event[j].Clone();
                    j -= gap;
                }
                if (j != i - gap)
                {
                    S.Event[j + gap] = temp.Clone();
                }
            }
        }
        for (i = 0; i < S.NumEvents - 1; i++)
        {
            if (S.Event[i].end > (S.Event[i + 1]).start)
            {
                S.Event[i].end = (S.Event[i + 1]).start;
            }
        }
    }

}
}
