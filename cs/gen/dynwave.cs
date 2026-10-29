// Generated from EPA SWMM 5.2.4 (dynwave.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double MINTIMESTEP = 0.001;
    public static double dynwave_OMEGA = 0.5;
    public static double DEFAULT_SURFAREA = 12.566;
    public static double DEFAULT_HEADTOL = 0.005;
    public static double EXTRAN_CROWN_CUTOFF = 0.96;
    public static double SLOT_CROWN_CUTOFF = 0.985257;
    public static int DEFAULT_MAXTRIALS = 8;
    public static double VariableStep = 0.0;
    public static TXnode[] Xnode = null;
    public static double Omega = 0.0;
    public static int Steps = 0;

    // dynwave.c:117
    public static void dynwave_init()
    {
        int i = 0;
        int j = 0;
        double z = 0.0;
        S.VariableStep = 0.0;
        S.Xnode = R.NewArray<TXnode>((int)(unchecked((ulong)(S.Nobjects[S.NODE]))));
        if (S.Xnode == null)
        {
            S.report_writeErrorMsg(S.ERR_MEMORY, S.L45);
            return;
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.Xnode[i].newSurfArea = 0.0;
            S.Xnode[i].oldSurfArea = 0.0;
            S.Node[i].crownElev = S.Node[i].invertElev;
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            j = S.Link[i].node1;
            z = S.Node[j].invertElev + S.Link[i].offset1 + S.Link[i].xsect.yFull;
            S.Node[j].crownElev = ((((S.Node[j].crownElev) >= (z)) ? (S.Node[j].crownElev) : (z)));
            j = S.Link[i].node2;
            z = S.Node[j].invertElev + S.Link[i].offset2 + S.Link[i].xsect.yFull;
            S.Node[j].crownElev = ((((S.Node[j].crownElev) >= (z)) ? (S.Node[j].crownElev) : (z)));
            S.Link[i].flowClass = S.DRY;
            S.Link[i].dqdh = 0.0;
        }
        if (S.SurchargeMethod == S.SLOT)
        {
            S.CrownCutoff = S.SLOT_CROWN_CUTOFF;
        }
        else
        {
            S.CrownCutoff = S.EXTRAN_CROWN_CUTOFF;
        }
    }

    // dynwave.c:165
    public static void dynwave_close()
    {
        {
            if ((S.Xnode != null))
            {
                R.free(S.Xnode);
                S.Xnode = (null);
            }
        }
        ;
    }

    // dynwave.c:177
    public static void dynwave_validate()
    {
        if (S.MinRouteStep > S.RouteStep)
        {
            S.MinRouteStep = S.RouteStep;
        }
        if (S.MinRouteStep < S.MINTIMESTEP)
        {
            S.MinRouteStep = S.MINTIMESTEP;
        }
        if (S.MinSurfArea == 0.0)
        {
            S.MinSurfArea = S.DEFAULT_SURFAREA;
        }
        else
        {
            S.MinSurfArea /= S.UCF(S.LENGTH) * S.UCF(S.LENGTH);
        }
        if (S.HeadTol == 0.0)
        {
            S.HeadTol = S.DEFAULT_HEADTOL;
        }
        else
        {
            S.HeadTol /= S.UCF(S.LENGTH);
        }
        if (S.MaxTrials == 0)
        {
            S.MaxTrials = S.DEFAULT_MAXTRIALS;
        }
    }

    // dynwave.c:195
    public static double dynwave_getRoutingStep(double fixedStep)
    {
        if (S.CourantFactor == 0.0)
        {
            return fixedStep;
        }
        if (fixedStep < S.MINTIMESTEP)
        {
            return fixedStep;
        }
        if (S.VariableStep == 0.0)
        {
            S.VariableStep = S.MinRouteStep;
        }
        else
        {
            S.VariableStep = S.getVariableStep(fixedStep);
        }
        S.VariableStep = Math.Floor(1000.0 * S.VariableStep) / 1000.0;
        return S.VariableStep;
    }

    // dynwave.c:224
    public static int dynwave_execute(double tStep)
    {
        int converged = 0;
        if ((S.ErrorCode != 0))
        {
            return 0;
        }
        S.Steps = 0;
        converged = 0;
        S.Omega = S.dynwave_OMEGA;
        S.initRoutingStep();
        while (S.Steps < S.MaxTrials)
        {
            S.initNodeStates();
            S.findLinkFlows(tStep);
            converged = S.findNodeDepths(tStep);
            S.Steps++;
            if (S.Steps > 1)
            {
                if ((converged != 0))
                {
                    break;
                }
                S.findBypassedLinks();
            }
        }
        if (!(converged != 0))
        {
            S.updateConvergenceStats();
        }
        S.findLimitedLinks();
        return S.Steps;
    }

    // dynwave.c:266
    public static void updateConvergenceStats()
    {
        int i = 0;
        S.NonConvergeCount++;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.stats_updateConvergenceStats(i, S.Xnode[i].converged);
        }
    }

    // dynwave.c:276
    public static void initRoutingStep()
    {
        int i = 0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.Xnode[i].converged = 0;
            S.Xnode[i].dYdT = 0.0;
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            S.Link[i].bypassed = 0;
            S.Link[i].surfArea1 = 0.0;
            S.Link[i].surfArea2 = 0.0;
        }
        for (i = 0; i < S.Nlinks[S.CONDUIT]; i++)
        {
            S.Conduit[i].a2 = S.Conduit[i].a1;
        }
    }

    // dynwave.c:297
    public static void initNodeStates()
    {
        int i = 0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if ((S.AllowPonding != 0))
            {
                S.Xnode[i].newSurfArea = S.node_getPondedArea(i, S.Node[i].newDepth);
            }
            else
            {
                S.Xnode[i].newSurfArea = S.node_getSurfArea(i, S.Node[i].newDepth);
            }
            S.Node[i].inflow = 0.0;
            S.Node[i].outflow = S.Node[i].losses;
            if (S.Node[i].newLatFlow >= 0.0)
            {
                S.Node[i].inflow += S.Node[i].newLatFlow;
            }
            else
            {
                S.Node[i].outflow -= S.Node[i].newLatFlow;
            }
            S.Xnode[i].sumdqdh = 0.0;
        }
    }

    // dynwave.c:335
    public static void findBypassedLinks()
    {
        int i = 0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if ((S.Xnode[S.Link[i].node1].converged != 0) && (S.Xnode[S.Link[i].node2].converged != 0))
            {
                S.Link[i].bypassed = 1;
            }
            else
            {
                S.Link[i].bypassed = 0;
            }
        }
    }

    // dynwave.c:349
    public static void findLimitedLinks()
    {
        int j = 0;
        int n1 = 0;
        int n2 = 0;
        int k = 0;
        double h1 = 0.0;
        double h2 = 0.0;
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if (!((S.isTrueConduit(j) != 0)))
            {
                continue;
            }
            k = S.Link[j].subIndex;
            S.Conduit[k].capacityLimited = 0;
            if (S.Conduit[k].a1 >= S.Link[j].xsect.aFull)
            {
                n1 = S.Link[j].node1;
                n2 = S.Link[j].node2;
                h1 = S.Node[n1].newDepth + S.Node[n1].invertElev;
                h2 = S.Node[n2].newDepth + S.Node[n2].invertElev;
                if ((h1 - h2) > Math.Abs(S.Conduit[k].slope) * S.Conduit[k].length)
                {
                    S.Conduit[k].capacityLimited = 1;
                }
            }
        }
    }

    // dynwave.c:382
    public static void findLinkFlows(double dt)
    {
        int i = 0;
        {
            for (i = 0; i < S.Nobjects[S.LINK]; i++)
            {
                if (((S.isTrueConduit(i) != 0)) && (!(S.Link[i].bypassed != 0)))
                {
                    S.dwflow_findConduitFlow(i, S.Steps, S.Omega, dt);
                }
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if ((S.isTrueConduit(i) != 0))
            {
                S.updateNodeFlows(i);
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (!((S.isTrueConduit(i) != 0)))
            {
                if (!(S.Link[i].bypassed != 0))
                {
                    S.findNonConduitFlow(i, dt);
                }
                S.updateNodeFlows(i);
            }
        }
    }

    // dynwave.c:416
    public static int isTrueConduit(int j)
    {
        return (((S.Link[j].type == S.CONDUIT) && (S.Link[j].xsect.type != S.DUMMY)) ? 1 : 0);
    }

    // dynwave.c:423
    public static void findNonConduitFlow(int i, double dt)
    {
        double qLast = 0.0;
        double qNew = 0.0;
        qLast = S.Link[i].newFlow;
        S.Link[i].dqdh = 0.0;
        qNew = S.link_getInflow(i);
        if (S.Link[i].type == S.PUMP)
        {
            qNew = S.getModPumpFlow(i, qNew, dt);
        }
        S.findNonConduitSurfArea(i);
        if ((S.Steps > 0) && (S.Link[i].type != S.PUMP))
        {
            qNew = (1.0 - S.Omega) * qLast + S.Omega * qNew;
            if (qNew * qLast < 0.0)
            {
                qNew = 0.001 * ((((qNew) < 0) ? (-1) : (1)));
            }
        }
        S.Link[i].newFlow = qNew;
    }

    // dynwave.c:458
    public static double getModPumpFlow(int i, double q, double dt)
    {
        int j = S.Link[i].node1;
        int k = S.Link[i].subIndex;
        double newNetInflow = 0.0;
        double netFlowVolume = 0.0;
        double y = 0.0;
        if (q == 0.0)
        {
            return q;
        }
        if (S.Node[j].type == S.STORAGE)
        {
            return S.node_getMaxOutflow(j, q, dt);
        }
        switch (S.Pump[k].type)
        {
            case S.TYPE1_PUMP:
                return S.node_getMaxOutflow(j, q, dt);
            case S.TYPE2_PUMP:
            case S.TYPE4_PUMP:
            case S.TYPE3_PUMP:
                newNetInflow = S.Node[j].inflow - S.Node[j].outflow - q;
                netFlowVolume = 0.5 * (S.Node[j].oldNetInflow + newNetInflow) * dt;
                y = S.Node[j].oldDepth + netFlowVolume / S.Xnode[j].newSurfArea;
                if (y <= 0.0)
                {
                    return S.Node[j].inflow;
                }
                break;
        }
        return q;
    }

    // dynwave.c:503
    public static void findNonConduitSurfArea(int i)
    {
        if (S.Link[i].type == S.ORIFICE)
        {
            S.Link[i].surfArea1 = S.Orifice[S.Link[i].subIndex].surfArea / 2.0;
        }
        else
        {
            S.Link[i].surfArea1 = 0.0;
        }
        S.Link[i].surfArea2 = S.Link[i].surfArea1;
        if ((S.Link[i].flowClass == S.UP_CRITICAL) || (S.Node[S.Link[i].node1].type == S.STORAGE))
        {
            S.Link[i].surfArea1 = 0.0;
        }
        if ((S.Link[i].flowClass == S.DN_CRITICAL) || (S.Node[S.Link[i].node2].type == S.STORAGE))
        {
            S.Link[i].surfArea2 = 0.0;
        }
    }

    // dynwave.c:528
    public static void updateNodeFlows(int i)
    {
        int k = 0;
        int barrels = 1;
        int n1 = S.Link[i].node1;
        int n2 = S.Link[i].node2;
        double q = S.Link[i].newFlow;
        double conduitLossRate = 0.0;
        if (q >= 0.0)
        {
            S.Node[n1].outflow += q;
            S.Node[n2].inflow += q;
        }
        else
        {
            S.Node[n1].inflow -= q;
            S.Node[n2].outflow -= q;
        }
        if (S.Link[i].type == S.CONDUIT)
        {
            k = S.Link[i].subIndex;
            barrels = S.Conduit[k].barrels;
            conduitLossRate = (S.Conduit[k].evapLossRate + S.Conduit[k].seepLossRate) * barrels;
            if (conduitLossRate > 0.0)
            {
                if ((S.Node[n1].type != S.OUTFALL) && (S.Node[n2].type != S.OUTFALL))
                {
                    conduitLossRate /= 2.0;
                }
                if (S.Node[n1].type != S.OUTFALL)
                {
                    S.Node[n1].outflow += conduitLossRate;
                }
                if (S.Node[n2].type != S.OUTFALL)
                {
                    S.Node[n2].outflow += conduitLossRate;
                }
            }
        }
        S.Xnode[S.Link[i].node1].newSurfArea += S.Link[i].surfArea1 * barrels;
        S.Xnode[S.Link[i].node2].newSurfArea += S.Link[i].surfArea2 * barrels;
        S.Xnode[S.Link[i].node1].sumdqdh += S.Link[i].dqdh;
        if (S.Link[i].type == S.PUMP)
        {
            k = S.Link[i].subIndex;
            if (S.Pump[k].type != S.TYPE4_PUMP)
            {
                S.Xnode[n2].sumdqdh += S.Link[i].dqdh;
            }
        }
        else
        {
            S.Xnode[n2].sumdqdh += S.Link[i].dqdh;
        }
    }

    // dynwave.c:593
    public static int findNodeDepths(double dt)
    {
        int i = 0;
        double yOld = 0.0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            S.link_setOutfallDepth(i);
        }
        {
            for (i = 0; i < S.Nobjects[S.NODE]; i++)
            {
                if (S.Node[i].type == S.OUTFALL)
                {
                    continue;
                }
                yOld = S.Node[i].newDepth;
                S.setNodeDepth(i, dt);
                S.Xnode[i].converged = 1;
                if (Math.Abs(yOld - S.Node[i].newDepth) > S.HeadTol)
                {
                    S.Xnode[i].converged = 0;
                }
            }
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (S.Node[i].type == S.OUTFALL)
            {
                continue;
            }
            if (S.Xnode[i].converged == 0)
            {
                return 0;
            }
        }
        return 1;
    }

    // dynwave.c:636
    public static void setNodeDepth(int i, double dt)
    {
        int canPond = 0;
        int isPonded = 0;
        int isSurcharged = 0;
        double dQ = 0.0;
        double dV = 0.0;
        double dy = 0.0;
        double yMax = 0.0;
        double yOld = 0.0;
        double yLast = 0.0;
        double yNew = 0.0;
        double yCrown = 0.0;
        double surfArea = 0.0;
        double denom = 0.0;
        double corr = 0.0;
        double f = 0.0;
        canPond = (((S.AllowPonding != 0) && (S.Node[i].pondedArea > 0.0)) ? 1 : 0);
        isPonded = (((canPond != 0) && (S.Node[i].newDepth > S.Node[i].fullDepth)) ? 1 : 0);
        yCrown = S.Node[i].crownElev - S.Node[i].invertElev;
        yOld = S.Node[i].oldDepth;
        yLast = S.Node[i].newDepth;
        S.Node[i].overflow = 0.0;
        surfArea = S.Xnode[i].newSurfArea;
        surfArea = ((((surfArea) >= (S.MinSurfArea)) ? (surfArea) : (S.MinSurfArea)));
        dQ = S.Node[i].inflow - S.Node[i].outflow;
        dV = 0.5 * (S.Node[i].oldNetInflow + dQ) * dt;
        if (S.SurchargeMethod == S.EXTRAN)
        {
            if ((isPonded != 0))
            {
                isSurcharged = 0;
            }
            else if (S.Node[i].type == S.STORAGE)
            {
                isSurcharged = (((S.Node[i].surDepth > 0.0) && (yLast > S.Node[i].fullDepth)) ? 1 : 0);
            }
            else
            {
                isSurcharged = (((yCrown > 0.0) && (yLast > yCrown)) ? 1 : 0);
            }
        }
        if (!(isSurcharged != 0))
        {
            dy = dV / surfArea;
            yNew = yOld + dy;
            if (!(isPonded != 0))
            {
                S.Xnode[i].oldSurfArea = surfArea;
            }
            if (S.Steps > 0)
            {
                yNew = (1.0 - S.Omega) * yLast + S.Omega * yNew;
            }
            if ((isPonded != 0) && (yNew < S.Node[i].fullDepth))
            {
                yNew = S.Node[i].fullDepth - 0.0001;
            }
        }
        else
        {
            corr = 1.0;
            if (S.Node[i].degree < 0)
            {
                corr = 0.6;
            }
            denom = S.Xnode[i].sumdqdh;
            if (yLast < 1.25 * yCrown)
            {
                f = (yLast - yCrown) / yCrown;
                denom += (S.Xnode[i].oldSurfArea / dt - S.Xnode[i].sumdqdh) * Math.Exp(-15.0 * f);
            }
            if (denom == 0.0)
            {
                dy = 0.0;
            }
            else
            {
                dy = corr * dQ / denom;
            }
            yNew = yLast + dy;
            if (yNew < yCrown)
            {
                yNew = yCrown - 0.0001;
            }
            if ((canPond != 0) && (yNew > S.Node[i].fullDepth))
            {
                yNew = S.Node[i].fullDepth + 0.0001;
            }
        }
        if (yNew < 0)
        {
            yNew = 0.0;
        }
        yMax = S.Node[i].fullDepth;
        if (canPond == 0)
        {
            yMax += S.Node[i].surDepth;
        }
        if (yNew > yMax)
        {
            yNew = S.getFloodedDepth(i, canPond, dV, yNew, yMax, dt);
        }
        else
        {
            S.Node[i].newVolume = S.node_getVolume(i, yNew);
        }
        S.Xnode[i].dYdT = Math.Abs(yNew - yOld) / dt;
        S.Node[i].newDepth = yNew;
    }

    // dynwave.c:766
    public static double getFloodedDepth(int i, int canPond, double dV, double yNew, double yMax, double dt)
    {
        if (canPond == 0)
        {
            S.Node[i].overflow = dV / dt;
            S.Node[i].newVolume = S.Node[i].fullVolume;
            yNew = yMax;
        }
        else
        {
            S.Node[i].newVolume = (((((S.Node[i].oldVolume + dV)) >= (S.Node[i].fullVolume)) ? ((S.Node[i].oldVolume + dV)) : (S.Node[i].fullVolume)));
            S.Node[i].overflow = (S.Node[i].newVolume - ((((S.Node[i].oldVolume) >= (S.Node[i].fullVolume)) ? (S.Node[i].oldVolume) : (S.Node[i].fullVolume)))) / dt;
        }
        if (S.Node[i].overflow < 0.0001)
        {
            S.Node[i].overflow = 0.0;
        }
        return yNew;
    }

    // dynwave.c:799
    public static double getVariableStep(double maxStep)
    {
        int minLink = -1;
        int minNode = -1;
        double tMin = 0.0;
        double tMinLink = 0.0;
        double tMinNode = 0.0;
        tMin = maxStep;
        tMinLink = S.getLinkStep(tMin, ref minLink);
        tMinNode = S.getNodeStep(tMinLink, ref minNode);
        tMin = tMinLink;
        if (tMinNode < tMin)
        {
            tMin = tMinNode;
            minLink = -1;
        }
        S.stats_updateCriticalTimeCount(minNode, minLink);
        if (tMin < S.MinRouteStep)
        {
            tMin = S.MinRouteStep;
        }
        return tMin;
    }

    // dynwave.c:836
    public static double getLinkStep(double tMin, ref int minLink)
    {
        int i = 0;
        int k = 0;
        double q = 0.0;
        double t = 0.0;
        double tLink = tMin;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (S.Link[i].type == S.CONDUIT)
            {
                k = S.Link[i].subIndex;
                q = Math.Abs(S.Link[i].newFlow) / S.Conduit[k].barrels;
                if (((q <= 0.0001) || (S.Conduit[k].a1 <= 0.0001)) || (S.Link[i].froude <= 0.01))
                {
                    continue;
                }
                t = S.Link[i].newVolume / S.Conduit[k].barrels / q;
                t = t * S.Conduit[k].modLength / S.link_getLength(i);
                t = t * S.Link[i].froude / (1.0 + S.Link[i].froude) * S.CourantFactor;
                if (t < tLink)
                {
                    tLink = t;
                    minLink = i;
                }
            }
        }
        return tLink;
    }

    // dynwave.c:881
    public static double getNodeStep(double tMin, ref int minNode)
    {
        int i = 0;
        double maxDepth = 0.0;
        double dYdT = 0.0;
        double t1 = 0.0;
        double tNode = tMin;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (S.Node[i].type == S.OUTFALL)
            {
                continue;
            }
            if (S.Node[i].newDepth <= 0.0001)
            {
                continue;
            }
            if (S.Node[i].newDepth + 0.0001 >= S.Node[i].crownElev - S.Node[i].invertElev)
            {
                continue;
            }
            maxDepth = (S.Node[i].crownElev - S.Node[i].invertElev) * 0.25;
            if (maxDepth < 0.0001)
            {
                continue;
            }
            dYdT = S.Xnode[i].dYdT;
            if (dYdT < 0.0001)
            {
                continue;
            }
            t1 = maxDepth / dYdT;
            if (t1 < tNode)
            {
                tNode = t1;
                minNode = i;
            }
        }
        return tNode;
    }

}
}
