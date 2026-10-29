// Generated from EPA SWMM 5.2.4 (flowrout.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double flowrout_OMEGA = 0.55;
    public static int MAXITER = 10;
    public static double STOPTOL = 0.005;

    // flowrout.c:75
    public static void flowrout_init(int routingModel)
    {
        if (routingModel == S.DW)
        {
            S.validateGeneralLayout();
            S.dynwave_init();
            if (S.Fhotstart1.mode == S.NO_FILE)
            {
                S.initNodeDepths();
                S.initLinkDepths();
            }
        }
        else
        {
            S.validateTreeLayout();
        }
        S.initNodes();
        S.initLinks(routingModel);
    }

    // flowrout.c:107
    public static void flowrout_close(int routingModel)
    {
        if (routingModel == S.DW)
        {
            S.dynwave_close();
        }
    }

    // flowrout.c:119
    public static double flowrout_getRoutingStep(int routingModel, double fixedStep)
    {
        if (routingModel == S.DW)
        {
            return S.dynwave_getRoutingStep(fixedStep);
        }
        return fixedStep;
    }

    // flowrout.c:136
    public static int flowrout_execute(int[] links, int routingModel, double tStep)
    {
        int i = 0;
        int j = 0;
        int n1 = 0;
        double qin = 0.0;
        double qout = 0.0;
        double steps = 0.0;
        if ((S.ErrorCode != 0))
        {
            return 0;
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.Node[j].updated = 0;
            S.Node[j].overflow = 0.0;
            if ((S.Node[j].type != S.STORAGE) && (S.Node[j].newVolume > S.Node[j].fullVolume))
            {
                S.Node[j].overflow = (S.Node[j].newVolume - S.Node[j].fullVolume) / tStep;
            }
        }
        if (routingModel == S.DW)
        {
            return S.dynwave_execute(tStep);
        }
        steps = 0.0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            j = links[i];
            n1 = S.Link[j].node1;
            if (S.Node[n1].type == S.STORAGE)
            {
                S.updateStorageState(n1, i, links, tStep);
            }
            qin = S.getLinkInflow(j, tStep);
            if (routingModel == S.SF)
            {
                steps += S.steadyflow_execute(j, ref qin, ref qout, tStep);
            }
            else
            {
                steps += S.kinwave_execute(j, ref qin, ref qout, tStep);
            }
            S.Link[j].newFlow = qout;
            S.Node[S.Link[j].node1].outflow += qin;
            S.Node[S.Link[j].node2].inflow += qout;
        }
        if (S.Nobjects[S.LINK] > 0)
        {
            steps /= S.Nobjects[S.LINK];
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.setNewNodeState(j, tStep);
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.setNewLinkState(j);
        }
        return R.D2I((steps + 0.5));
    }

    // flowrout.c:203
    public static void validateTreeLayout()
    {
        int j = 0;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            switch (S.Node[j].type)
            {
                case S.DIVIDER:
                    if (S.Node[j].degree > 2)
                    {
                        S.report_writeErrorMsg(S.ERR_DIVIDER, S.Node[j].ID);
                    }
                    break;
                case S.OUTFALL:
                    if (S.Node[j].degree > 0)
                    {
                        S.report_writeErrorMsg(S.ERR_OUTFALL, S.Node[j].ID);
                    }
                    break;
                case S.STORAGE:
                    break;
                default:
                    if (S.Node[j].degree > 1)
                    {
                        S.report_writeErrorMsg(S.ERR_MULTI_OUTLET, S.Node[j].ID);
                    }
                    break;
            }
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            switch (S.Link[j].type)
            {
                case S.CONDUIT:
                    if ((S.Conduit[S.Link[j].subIndex].slope < 0.0) && (S.Link[j].xsect.type != S.DUMMY))
                    {
                        S.report_writeErrorMsg(S.ERR_SLOPE, S.Link[j].ID);
                    }
                    break;
                case S.ORIFICE:
                case S.WEIR:
                case S.OUTLET:
                    if (S.Node[S.Link[j].node1].type != S.STORAGE)
                    {
                        S.report_writeErrorMsg(S.ERR_REGULATOR, S.Link[j].ID);
                    }
                    break;
            }
        }
    }

    // flowrout.c:274
    public static void validateGeneralLayout()
    {
        int i = 0;
        int j = 0;
        int outletCount = 0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.Node[i].inflow = 0.0;
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            i = S.Link[j].node1;
            if (S.Node[i].type != S.OUTFALL)
            {
                i = S.Link[j].node2;
            }
            S.Node[i].inflow += 1.0;
            if ((((S.Link[j].type == S.CONDUIT) && (S.Link[j].xsect.type == S.DUMMY))) || (((S.Link[j].type == S.PUMP) && (S.Pump[S.Link[j].subIndex].type == S.IDEAL_PUMP))))
            {
                i = S.Link[j].node1;
                if (S.Link[j].direction < 0)
                {
                    i = S.Link[j].node2;
                }
                if (S.Node[i].degree > 1)
                {
                    S.report_writeErrorMsg(S.ERR_DUMMY_LINK, S.Node[i].ID);
                }
            }
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (S.Node[i].type == S.OUTFALL)
            {
                if (S.Node[i].degree + R.D2I(S.Node[i].inflow) > 1)
                {
                    S.report_writeErrorMsg(S.ERR_OUTFALL, S.Node[i].ID);
                }
                else
                {
                    outletCount++;
                }
            }
        }
        if (outletCount == 0)
        {
            S.report_writeErrorMsg(S.ERR_NO_OUTLETS, S.L0);
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (S.Node[i].inflow == 0.0)
            {
                S.Node[i].degree = -S.Node[i].degree;
            }
            S.Node[i].inflow = 0.0;
        }
    }

    // flowrout.c:337
    public static void initNodeDepths()
    {
        int i = 0;
        int n = 0;
        double y = 0.0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.Node[i].inflow = 0.0;
            S.Node[i].outflow = 0.0;
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (S.Link[i].newDepth > 0.0001)
            {
                y = S.Link[i].newDepth + S.Link[i].offset1;
            }
            else
            {
                y = 0.0;
            }
            n = S.Link[i].node1;
            S.Node[n].inflow += y;
            S.Node[n].outflow += 1.0;
            n = S.Link[i].node2;
            S.Node[n].inflow += y;
            S.Node[n].outflow += 1.0;
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (S.Node[i].type == S.OUTFALL)
            {
                continue;
            }
            if (S.Node[i].type == S.STORAGE)
            {
                continue;
            }
            if (S.Node[i].initDepth > 0.0)
            {
                continue;
            }
            if (S.Node[i].outflow > 0.0)
            {
                S.Node[i].newDepth = S.Node[i].inflow / S.Node[i].outflow;
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            S.link_setOutfallDepth(i);
        }
    }

    // flowrout.c:389
    public static void initLinkDepths()
    {
        int i = 0;
        double y = 0.0;
        double y1 = 0.0;
        double y2 = 0.0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (S.Link[i].type == S.CONDUIT)
            {
                if (S.Link[i].q0 != 0.0)
                {
                    continue;
                }
                y1 = S.Node[S.Link[i].node1].newDepth - S.Link[i].offset1;
                y1 = ((((y1) >= (0.0)) ? (y1) : (0.0)));
                y1 = ((((y1) <= (S.Link[i].xsect.yFull)) ? (y1) : (S.Link[i].xsect.yFull)));
                y2 = S.Node[S.Link[i].node2].newDepth - S.Link[i].offset2;
                y2 = ((((y2) >= (0.0)) ? (y2) : (0.0)));
                y2 = ((((y2) <= (S.Link[i].xsect.yFull)) ? (y2) : (S.Link[i].xsect.yFull)));
                y = 0.5 * (y1 + y2);
                y = ((((y) >= (0.0001)) ? (y) : (0.0001)));
                S.Link[i].newDepth = y;
            }
        }
    }

    // flowrout.c:425
    public static void initNodes()
    {
        int i = 0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.Node[i].inflow = S.Node[i].newLatFlow;
            S.Node[i].outflow = 0.0;
            S.Node[i].newVolume = 0.0;
            if (((S.AllowPonding != 0) && (S.Node[i].pondedArea > 0.0)) && (S.Node[i].newDepth > S.Node[i].fullDepth))
            {
                S.Node[i].newVolume = S.Node[i].fullVolume + (S.Node[i].newDepth - S.Node[i].fullDepth) * S.Node[i].pondedArea;
            }
            else
            {
                S.Node[i].newVolume = S.node_getVolume(i, S.Node[i].newDepth);
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (S.Link[i].newFlow >= 0.0)
            {
                S.Node[S.Link[i].node1].outflow += S.Link[i].newFlow;
                S.Node[S.Link[i].node2].inflow += S.Link[i].newFlow;
            }
            else
            {
                S.Node[S.Link[i].node1].inflow -= S.Link[i].newFlow;
                S.Node[S.Link[i].node2].outflow -= S.Link[i].newFlow;
            }
        }
    }

    // flowrout.c:472
    public static void initLinks(int routingModel)
    {
        int i = 0;
        int k = 0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (routingModel == S.SF)
            {
                S.Link[i].newFlow = 0.0;
            }
            else if (S.Link[i].type == S.CONDUIT)
            {
                k = S.Link[i].subIndex;
                S.Conduit[k].q1 = S.Link[i].newFlow / S.Conduit[k].barrels;
                S.Conduit[k].q2 = S.Conduit[k].q1;
                S.Conduit[k].a1 = S.xsect_getAofY(S.Link[i].xsect, S.Link[i].newDepth);
                S.Conduit[k].a2 = S.Conduit[k].a1;
                {
                    S.Link[i].newVolume = S.Conduit[k].a1 * S.link_getLength(i) * S.Conduit[k].barrels;
                }
                S.Link[i].oldVolume = S.Link[i].newVolume;
            }
        }
    }

    // flowrout.c:511
    public static double getLinkInflow(int j, double dt)
    {
        int n1 = S.Link[j].node1;
        double q = 0.0;
        if (((S.Link[j].type == S.CONDUIT) || (S.Link[j].type == S.PUMP)) || (S.Node[n1].type == S.STORAGE))
        {
            q = S.link_getInflow(j);
        }
        else
        {
            q = 0.0;
        }
        return S.node_getMaxOutflow(n1, q, dt);
    }

    // flowrout.c:531
    public static void updateStorageState(int i, int j, int[] links, double dt)
    {
        int iter = 0;
        int stopped = 0;
        double vFixed = 0.0;
        double v2 = 0.0;
        double d1 = 0.0;
        double d2 = 0.0;
        if (S.Node[i].type != S.STORAGE)
        {
            return;
        }
        if ((S.Node[i].updated != 0))
        {
            return;
        }
        vFixed = S.Node[i].oldVolume + 0.5 * (S.Node[i].oldNetInflow + S.Node[i].inflow - S.Node[i].outflow) * dt;
        d1 = S.Node[i].newDepth;
        iter = 1;
        stopped = 0;
        while ((iter < S.MAXITER) && (!(stopped != 0)))
        {
            v2 = vFixed - 0.5 * S.getStorageOutflow(i, j, links, dt) * dt;
            v2 = ((((0.0) >= (v2)) ? (0.0) : (v2)));
            S.Node[i].overflow = 0.0;
            if (v2 > S.Node[i].fullVolume)
            {
                S.Node[i].overflow = (v2 - ((((S.Node[i].oldVolume) >= (S.Node[i].fullVolume)) ? (S.Node[i].oldVolume) : (S.Node[i].fullVolume)))) / dt;
                if (S.Node[i].overflow < 0.0001)
                {
                    S.Node[i].overflow = 0.0;
                }
                if ((!(S.AllowPonding != 0)) || (S.Node[i].pondedArea == 0.0))
                {
                    v2 = S.Node[i].fullVolume;
                }
            }
            S.Node[i].newVolume = v2;
            d2 = S.node_getDepth(i, v2);
            S.Node[i].newDepth = d2;
            d2 = (1.0 - S.flowrout_OMEGA) * d1 + S.flowrout_OMEGA * d2;
            if (Math.Abs(d2 - d1) <= S.STOPTOL)
            {
                stopped = 1;
            }
            d1 = d2;
            iter++;
        }
        S.Node[i].updated = 1;
    }

    // flowrout.c:605
    public static double getStorageOutflow(int i, int j, int[] links, double dt)
    {
        int k = 0;
        int m = 0;
        double outflow = 0.0;
        for (k = j; k < S.Nobjects[S.LINK]; k++)
        {
            m = links[k];
            if (S.Link[m].node1 != i)
            {
                break;
            }
            outflow += S.getLinkInflow(m, dt);
        }
        return outflow;
    }

    // flowrout.c:629
    public static void setNewNodeState(int j, double dt)
    {
        int canPond = 0;
        double newNetInflow = 0.0;
        if (S.Node[j].type == S.STORAGE)
        {
            if (S.Node[j].updated == 0)
            {
                S.updateStorageState(j, S.Nobjects[S.LINK], (null), dt);
            }
            return;
        }
        newNetInflow = S.Node[j].inflow - S.Node[j].outflow - S.Node[j].losses;
        S.Node[j].newVolume = S.Node[j].oldVolume + newNetInflow * dt;
        if (S.Node[j].newVolume < 0.0001)
        {
            S.Node[j].newVolume = 0.0;
        }
        S.Node[j].overflow = 0.0;
        canPond = (((S.AllowPonding != 0) && (S.Node[j].pondedArea > 0.0)) ? 1 : 0);
        if (S.Node[j].newVolume > S.Node[j].fullVolume)
        {
            S.Node[j].overflow = (S.Node[j].newVolume - ((((S.Node[j].oldVolume) >= (S.Node[j].fullVolume)) ? (S.Node[j].oldVolume) : (S.Node[j].fullVolume)))) / dt;
            if (S.Node[j].overflow < 0.0001)
            {
                S.Node[j].overflow = 0.0;
            }
            if (!(canPond != 0))
            {
                S.Node[j].newVolume = S.Node[j].fullVolume;
            }
        }
        S.Node[j].newDepth = S.node_getDepth(j, S.Node[j].newVolume);
    }

    // flowrout.c:673
    public static void setNewLinkState(int j)
    {
        int k = 0;
        double a = 0.0;
        double y1 = 0.0;
        double y2 = 0.0;
        S.Link[j].newDepth = 0.0;
        S.Link[j].newVolume = 0.0;
        if (S.Link[j].type == S.CONDUIT)
        {
            k = S.Link[j].subIndex;
            a = 0.5 * (S.Conduit[k].a1 + S.Conduit[k].a2);
            S.Link[j].newVolume = a * S.link_getLength(j) * S.Conduit[k].barrels;
            y1 = S.xsect_getYofA(S.Link[j].xsect, S.Conduit[k].a1);
            y2 = S.xsect_getYofA(S.Link[j].xsect, S.Conduit[k].a2);
            S.Link[j].newDepth = 0.5 * (y1 + y2);
            S.updateNodeDepth(S.Link[j].node1, y1 + S.Link[j].offset1);
            S.updateNodeDepth(S.Link[j].node2, y2 + S.Link[j].offset2);
            if (S.Conduit[k].a1 >= S.Link[j].xsect.aFull)
            {
                S.Conduit[k].capacityLimited = 1;
                S.Conduit[k].fullState = S.ALL_FULL;
            }
            else
            {
                S.Conduit[k].capacityLimited = 0;
                S.Conduit[k].fullState = 0;
            }
        }
    }

    // flowrout.c:717
    public static void updateNodeDepth(int i, double y)
    {
        if (S.Node[i].type == S.STORAGE)
        {
            return;
        }
        if (((S.Node[i].type != S.OUTFALL) && (S.Node[i].degree > 0)) && (S.Node[i].overflow > 0.0))
        {
            y = S.Node[i].fullDepth;
        }
        if (S.Node[i].newDepth < y)
        {
            S.Node[i].newDepth = y;
            if ((S.Node[i].fullDepth > 0.0) && (y > S.Node[i].fullDepth))
            {
                S.Node[i].newDepth = S.Node[i].fullDepth;
            }
        }
    }

    // flowrout.c:748
    public static int steadyflow_execute(int j, ref double qin, ref double qout, double tStep)
    {
        int k = 0;
        double s = 0.0;
        double q = 0.0;
        if (S.Link[j].type == S.CONDUIT)
        {
            k = S.Link[j].subIndex;
            q = (qin) / S.Conduit[k].barrels;
            if (S.Link[j].xsect.type == S.DUMMY)
            {
                S.Conduit[k].a1 = 0.0;
            }
            else
            {
                q -= S.link_getLossRate(j, S.SF, q, tStep);
                if (q > S.Link[j].qFull)
                {
                    q = S.Link[j].qFull;
                    S.Conduit[k].a1 = S.Link[j].xsect.aFull;
                    qin = q * S.Conduit[k].barrels;
                }
                else
                {
                    s = q / S.Conduit[k].beta;
                    S.Conduit[k].a1 = S.xsect_getAofS(S.Link[j].xsect, s);
                }
            }
            S.Conduit[k].a2 = S.Conduit[k].a1;
            S.Conduit[k].q1Old = S.Conduit[k].q1;
            S.Conduit[k].q2Old = S.Conduit[k].q2;
            S.Conduit[k].q1 = q;
            S.Conduit[k].q2 = q;
            qout = q * S.Conduit[k].barrels;
        }
        else
        {
            qout = (qin);
        }
        return 1;
    }

}
}
