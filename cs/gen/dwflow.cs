// Generated from EPA SWMM 5.2.4 (dwflow.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double MAXVELOCITY = 50.0;

    // dwflow.c:57
    public static void dwflow_findConduitFlow(int j, int steps, double omega, double dt)
    {
        int k = 0;
        int n1 = 0;
        int n2 = 0;
        double z1 = 0.0;
        double z2 = 0.0;
        double h1 = 0.0;
        double h2 = 0.0;
        double y1 = 0.0;
        double y2 = 0.0;
        double a1 = 0.0;
        double a2 = 0.0;
        double r1 = 0.0;
        double yMid = 0.0;
        double rMid = 0.0;
        double aMid = 0.0;
        double aWtd = 0.0;
        double rWtd = 0.0;
        double qLast = 0.0;
        double qOld = 0.0;
        double aOld = 0.0;
        double v = 0.0;
        double rho = 0.0;
        double sigma = 0.0;
        double length = 0.0;
        double wSlot = 0.0;
        double dq1 = 0.0;
        double dq2 = 0.0;
        double dq3 = 0.0;
        double dq4 = 0.0;
        double dq5 = 0.0;
        double dq6 = 0.0;
        double denom = 0.0;
        double q = 0.0;
        double barrels = 0.0;
        TXsect xsect = S.Link[j].xsect;
        int isFull = 0;
        int isClosed = 0;
        if (S.Link[j].setting == 0)
        {
            isClosed = 1;
        }
        k = S.Link[j].subIndex;
        barrels = S.Conduit[k].barrels;
        qOld = S.Link[j].oldFlow / barrels;
        qLast = S.Conduit[k].q1;
        S.Conduit[k].evapLossRate = 0.0;
        S.Conduit[k].seepLossRate = 0.0;
        n1 = S.Link[j].node1;
        n2 = S.Link[j].node2;
        z1 = S.Node[n1].invertElev + S.Link[j].offset1;
        z2 = S.Node[n2].invertElev + S.Link[j].offset2;
        h1 = S.Node[n1].newDepth + S.Node[n1].invertElev;
        h2 = S.Node[n2].newDepth + S.Node[n2].invertElev;
        h1 = ((((h1) >= (z1)) ? (h1) : (z1)));
        h2 = ((((h2) >= (z2)) ? (h2) : (z2)));
        y1 = h1 - z1;
        y2 = h2 - z2;
        y1 = ((((y1) >= (0.0001)) ? (y1) : (0.0001)));
        y2 = ((((y2) >= (0.0001)) ? (y2) : (0.0001)));
        if (S.SurchargeMethod != S.SLOT)
        {
            y1 = ((((y1) <= (xsect.yFull)) ? (y1) : (xsect.yFull)));
            y2 = ((((y2) <= (xsect.yFull)) ? (y2) : (xsect.yFull)));
        }
        aOld = S.Conduit[k].a2;
        aOld = ((((aOld) >= (0.0001)) ? (aOld) : (0.0001)));
        length = S.Conduit[k].modLength;
        S.findSurfArea(j, qLast, length, ref h1, ref h2, ref y1, ref y2);
        wSlot = S.getSlotWidth(xsect, y1);
        a1 = S.dwflow_getArea(xsect, y1, wSlot);
        r1 = S.getHydRad(xsect, y1);
        wSlot = S.getSlotWidth(xsect, y2);
        a2 = S.dwflow_getArea(xsect, y2, wSlot);
        yMid = 0.5 * (y1 + y2);
        wSlot = S.getSlotWidth(xsect, yMid);
        aMid = S.dwflow_getArea(xsect, yMid, wSlot);
        rMid = S.getHydRad(xsect, yMid);
        if ((y1 >= xsect.yFull) && (y2 >= xsect.yFull))
        {
            isFull = 1;
        }
        if (((((S.Link[j].flowClass == S.DRY) || (S.Link[j].flowClass == S.UP_DRY)) || (S.Link[j].flowClass == S.DN_DRY)) || (isClosed != 0)) || (aMid <= 0.0001))
        {
            S.Conduit[k].a1 = 0.5 * (a1 + a2);
            S.Conduit[k].q1 = 0.0;
            ;
            S.Conduit[k].q2 = 0.0;
            S.Link[j].dqdh = 32.2 * dt * aMid / length * barrels;
            S.Link[j].froude = 0.0;
            S.Link[j].newDepth = ((((yMid) <= (S.Link[j].xsect.yFull)) ? (yMid) : (S.Link[j].xsect.yFull)));
            S.Link[j].newVolume = S.Conduit[k].a1 * S.link_getLength(j) * barrels;
            S.Link[j].newFlow = 0.0;
            return;
        }
        v = qLast / aMid;
        if (Math.Abs(v) > S.MAXVELOCITY)
        {
            v = S.MAXVELOCITY * ((((qLast) < 0) ? (-1) : (1)));
        }
        S.Link[j].froude = S.link_getFroude(j, v, yMid);
        if ((S.Link[j].flowClass == S.SUBCRITICAL) && (S.Link[j].froude > 1.0))
        {
            S.Link[j].flowClass = S.SUPCRITICAL;
        }
        if (S.Link[j].froude <= 0.5)
        {
            sigma = 1.0;
        }
        else if (S.Link[j].froude >= 1.0)
        {
            sigma = 0.0;
        }
        else
        {
            sigma = 2.0 * (1.0 - S.Link[j].froude);
        }
        rho = 1.0;
        if (((!(isFull != 0)) && (qLast > 0.0)) && (h1 >= h2))
        {
            rho = sigma;
        }
        aWtd = a1 + (aMid - a1) * rho;
        rWtd = r1 + (rMid - r1) * rho;
        if (S.InertDamping == S.NO_DAMPING)
        {
            sigma = 1.0;
        }
        else if (S.InertDamping == S.FULL_DAMPING)
        {
            sigma = 0.0;
        }
        if ((isFull != 0) && (!((S.xsect_isOpen(xsect.type) != 0))))
        {
            sigma = 0.0;
        }
        if ((xsect.type == S.FORCE_MAIN) && (isFull != 0))
        {
            dq1 = dt * S.forcemain_getFricSlope(j, Math.Abs(v), rMid);
        }
        else
        {
            dq1 = dt * S.Conduit[k].roughFactor / Math.Pow(rWtd, 1.33333) * Math.Abs(v);
        }
        dq2 = dt * 32.2 * aWtd * (h2 - h1) / length;
        dq3 = 0.0;
        dq4 = 0.0;
        if (sigma > 0.0)
        {
            dq3 = 2.0 * v * (aMid - aOld) * sigma;
            dq4 = dt * v * v * (a2 - a1) / length * sigma;
        }
        dq5 = 0.0;
        if ((S.Conduit[k].hasLosses != 0))
        {
            dq5 = S.findLocalLosses(j, a1, a2, aMid, qLast) / 2.0 / length * dt;
        }
        dq6 = S.link_getLossRate(j, S.DW, qLast, dt) * 2.5 * dt * v / S.link_getLength(j);
        denom = 1.0 + dq1 + dq5;
        q = (qOld - dq2 + dq3 + dq4 + dq6) / denom;
        S.Link[j].dqdh = 1.0 / denom * 32.2 * dt * aWtd / length * barrels;
        S.Link[j].inletControl = 0;
        S.Link[j].normalFlow = 0;
        if (q > 0.0)
        {
            if ((xsect.culvertCode > 0) && (!(isFull != 0)))
            {
                q = S.culvert_getInflow(j, q, h1);
            }
            else if (((S.NormalFlowLtd != S.NEITHER) && (y1 < S.Link[j].xsect.yFull)) && (((S.Link[j].flowClass == S.SUBCRITICAL) || (S.Link[j].flowClass == S.SUPCRITICAL))))
            {
                q = S.checkNormalFlow(j, q, y1, y2, a1, r1);
            }
        }
        if (steps > 0)
        {
            q = (1.0 - omega) * qLast + omega * q;
            if (q * qLast < 0.0)
            {
                q = 0.001 * ((((q) < 0) ? (-1) : (1)));
            }
        }
        if (S.Link[j].qLimit > 0.0)
        {
            if (Math.Abs(q) > S.Link[j].qLimit)
            {
                q = ((((q) < 0) ? (-1) : (1))) * S.Link[j].qLimit;
            }
        }
        if ((S.link_setFlapGate(j, n1, n2, q) != 0))
        {
            q = 0.0;
        }
        if ((q > 0.0001) && (S.Node[n1].newDepth <= 0.0001))
        {
            q = 0.0001;
        }
        if ((q < -0.0001) && (S.Node[n2].newDepth <= 0.0001))
        {
            q = -0.0001;
        }
        S.Conduit[k].a1 = aMid;
        S.Conduit[k].q1 = q;
        S.Conduit[k].q2 = q;
        S.Link[j].newDepth = ((((yMid) <= (xsect.yFull)) ? (yMid) : (xsect.yFull)));
        aMid = (a1 + a2) / 2.0;
        S.Conduit[k].fullState = S.link_getFullState(a1, a2, xsect.aFull);
        S.Link[j].newVolume = aMid * S.link_getLength(j) * barrels;
        S.Link[j].newFlow = q * barrels;
    }

    // dwflow.c:297
    public static int getFlowClass(int j, double q, double h1, double h2, double y1, double y2, ref double yC, ref double yN, ref double fasnh)
    {
        int n1 = 0;
        int n2 = 0;
        int flowClass = 0;
        double ycMin = 0.0;
        double ycMax = 0.0;
        double z1 = 0.0;
        double z2 = 0.0;
        n1 = S.Link[j].node1;
        n2 = S.Link[j].node2;
        z1 = S.Link[j].offset1;
        z2 = S.Link[j].offset2;
        if (S.Node[n1].type == S.OUTFALL)
        {
            z1 = ((((0.0) >= ((z1 - S.Node[n1].newDepth))) ? (0.0) : ((z1 - S.Node[n1].newDepth))));
        }
        if (S.Node[n2].type == S.OUTFALL)
        {
            z2 = ((((0.0) >= ((z2 - S.Node[n2].newDepth))) ? (0.0) : ((z2 - S.Node[n2].newDepth))));
        }
        flowClass = S.SUBCRITICAL;
        fasnh = 1.0;
        if ((y1 > 0.0001) && (y2 > 0.0001))
        {
            if (q < 0.0)
            {
                if (z1 > 0.0)
                {
                    yN = S.link_getYnorm(j, Math.Abs(q));
                    yC = S.link_getYcrit(j, Math.Abs(q));
                    ycMin = ((((yN) <= (yC)) ? (yN) : (yC)));
                    if (y1 < ycMin)
                    {
                        flowClass = S.UP_CRITICAL;
                    }
                }
            }
            else
            {
                if (z2 > 0.0)
                {
                    yN = S.link_getYnorm(j, Math.Abs(q));
                    yC = S.link_getYcrit(j, Math.Abs(q));
                    ycMin = ((((yN) <= (yC)) ? (yN) : (yC)));
                    ycMax = ((((yN) >= (yC)) ? (yN) : (yC)));
                    if (y2 < ycMin)
                    {
                        flowClass = S.DN_CRITICAL;
                    }
                    else if (y2 < ycMax)
                    {
                        if (ycMax - ycMin < 0.0001)
                        {
                            fasnh = 0.0;
                        }
                        else
                        {
                            fasnh = (ycMax - y2) / (ycMax - ycMin);
                        }
                    }
                }
            }
        }
        else if ((y1 <= 0.0001) && (y2 <= 0.0001))
        {
            flowClass = S.DRY;
        }
        else if (y2 > 0.0001)
        {
            if (h2 < S.Node[n1].invertElev + S.Link[j].offset1)
            {
                flowClass = S.UP_DRY;
            }
            else if (z1 > 0.0)
            {
                yN = S.link_getYnorm(j, Math.Abs(q));
                yC = S.link_getYcrit(j, Math.Abs(q));
                flowClass = S.UP_CRITICAL;
            }
        }
        else
        {
            if (h1 < S.Node[n2].invertElev + S.Link[j].offset2)
            {
                flowClass = S.DN_DRY;
            }
            else if (z2 > 0.0)
            {
                yN = S.link_getYnorm(j, Math.Abs(q));
                yC = S.link_getYcrit(j, Math.Abs(q));
                flowClass = S.DN_CRITICAL;
            }
        }
        return flowClass;
    }

    // dwflow.c:417
    public static void findSurfArea(int j, double q, double length, ref double h1, ref double h2, ref double y1, ref double y2)
    {
        int n1 = 0;
        int n2 = 0;
        double flowDepth1 = 0.0;
        double flowDepth2 = 0.0;
        double flowDepthMid = 0.0;
        double width1 = 0.0;
        double width2 = 0.0;
        double widthMid = 0.0;
        double surfArea1 = 0.0;
        double surfArea2 = 0.0;
        double criticalDepth = 0.0;
        double normalDepth = 0.0;
        double fullDepth = 0.0;
        double fasnh = 1.0;
        TXsect xsect = S.Link[j].xsect;
        n1 = S.Link[j].node1;
        n2 = S.Link[j].node2;
        flowDepth1 = y1;
        flowDepth2 = y2;
        normalDepth = (flowDepth1 + flowDepth2) / 2.0;
        criticalDepth = normalDepth;
        fullDepth = xsect.yFull;
        if ((flowDepth1 >= fullDepth) && (flowDepth2 >= fullDepth))
        {
            S.Link[j].flowClass = S.SUBCRITICAL;
        }
        else
        {
            S.Link[j].flowClass = S.getFlowClass(j, q, h1, h2, y1, y2, ref criticalDepth, ref normalDepth, ref fasnh);
        }
        switch (S.Link[j].flowClass)
        {
            case S.SUBCRITICAL:
                flowDepthMid = 0.5 * (flowDepth1 + flowDepth2);
                if (flowDepthMid < 0.0001)
                {
                    flowDepthMid = 0.0001;
                }
                width1 = S.dwflow_getWidth(xsect, flowDepth1);
                width2 = S.dwflow_getWidth(xsect, flowDepth2);
                widthMid = S.dwflow_getWidth(xsect, flowDepthMid);
                surfArea1 = (width1 + widthMid) * length / 4.0;
                surfArea2 = (widthMid + width2) * length / 4.0 * fasnh;
                break;
            case S.UP_CRITICAL:
                flowDepth1 = criticalDepth;
                if (normalDepth < criticalDepth)
                {
                    flowDepth1 = normalDepth;
                }
                flowDepth1 = ((((flowDepth1) >= (0.0001)) ? (flowDepth1) : (0.0001)));
                h1 = S.Node[n1].invertElev + S.Link[j].offset1 + flowDepth1;
                flowDepthMid = 0.5 * (flowDepth1 + flowDepth2);
                if (flowDepthMid < 0.0001)
                {
                    flowDepthMid = 0.0001;
                }
                width2 = S.dwflow_getWidth(xsect, flowDepth2);
                widthMid = S.dwflow_getWidth(xsect, flowDepthMid);
                surfArea2 = (widthMid + width2) * length * 0.5;
                break;
            case S.DN_CRITICAL:
                flowDepth2 = criticalDepth;
                if (normalDepth < criticalDepth)
                {
                    flowDepth2 = normalDepth;
                }
                flowDepth2 = ((((flowDepth2) >= (0.0001)) ? (flowDepth2) : (0.0001)));
                h2 = S.Node[n2].invertElev + S.Link[j].offset2 + flowDepth2;
                width1 = S.dwflow_getWidth(xsect, flowDepth1);
                flowDepthMid = 0.5 * (flowDepth1 + flowDepth2);
                if (flowDepthMid < 0.0001)
                {
                    flowDepthMid = 0.0001;
                }
                widthMid = S.dwflow_getWidth(xsect, flowDepthMid);
                surfArea1 = (width1 + widthMid) * length * 0.5;
                break;
            case S.UP_DRY:
                flowDepth1 = 0.0001;
                flowDepthMid = 0.5 * (flowDepth1 + flowDepth2);
                if (flowDepthMid < 0.0001)
                {
                    flowDepthMid = 0.0001;
                }
                width1 = S.dwflow_getWidth(xsect, flowDepth1);
                width2 = S.dwflow_getWidth(xsect, flowDepth2);
                widthMid = S.dwflow_getWidth(xsect, flowDepthMid);
                surfArea2 = (widthMid + width2) * length / 4.0;
                if (S.Link[j].offset1 <= 0.0)
                {
                    surfArea1 = (width1 + widthMid) * length / 4.0;
                }
                break;
            case S.DN_DRY:
                flowDepth2 = 0.0001;
                flowDepthMid = 0.5 * (flowDepth1 + flowDepth2);
                if (flowDepthMid < 0.0001)
                {
                    flowDepthMid = 0.0001;
                }
                width1 = S.dwflow_getWidth(xsect, flowDepth1);
                width2 = S.dwflow_getWidth(xsect, flowDepth2);
                widthMid = S.dwflow_getWidth(xsect, flowDepthMid);
                surfArea1 = (widthMid + width1) * length / 4.0;
                if (S.Link[j].offset2 <= 0.0)
                {
                    surfArea2 = (width2 + widthMid) * length / 4.0;
                }
                break;
            case S.DRY:
                surfArea1 = 0.0001 * length / 2.0;
                surfArea2 = surfArea1;
                break;
        }
        S.Link[j].surfArea1 = surfArea1;
        S.Link[j].surfArea2 = surfArea2;
        y1 = flowDepth1;
        y2 = flowDepth2;
    }

    // dwflow.c:554
    public static double findLocalLosses(int j, double a1, double a2, double aMid, double q)
    {
        double losses = 0.0;
        q = Math.Abs(q);
        if (a1 > 0.0001)
        {
            losses += S.Link[j].cLossInlet * (q / a1);
        }
        if (a2 > 0.0001)
        {
            losses += S.Link[j].cLossOutlet * (q / a2);
        }
        if (aMid > 0.0001)
        {
            losses += S.Link[j].cLossAvg * (q / aMid);
        }
        return losses;
    }

    // dwflow.c:575
    public static double getSlotWidth(TXsect xsect, double y)
    {
        double yNorm = y / xsect.yFull;
        if (((S.SurchargeMethod != S.SLOT) || ((S.xsect_isOpen(xsect.type) != 0))) || (yNorm < S.CrownCutoff))
        {
            return 0.0;
        }
        if (yNorm > 1.78)
        {
            return 0.01 * xsect.wMax;
        }
        return xsect.wMax * 0.5423 * Math.Exp(-(Math.Pow(yNorm, 2.4)));
    }

    // dwflow.c:592
    public static double dwflow_getWidth(TXsect xsect, double y)
    {
        double wSlot = S.getSlotWidth(xsect, y);
        if (wSlot > 0.0)
        {
            return wSlot;
        }
        if ((y / xsect.yFull >= S.CrownCutoff) && (!((S.xsect_isOpen(xsect.type) != 0))))
        {
            y = S.CrownCutoff * xsect.yFull;
        }
        return S.xsect_getWofY(xsect, y);
    }

    // dwflow.c:609
    public static double dwflow_getArea(TXsect xsect, double y, double wSlot)
    {
        if (y >= xsect.yFull)
        {
            return xsect.aFull + (y - xsect.yFull) * wSlot;
        }
        return S.xsect_getAofY(xsect, y);
    }

    // dwflow.c:623
    public static double getHydRad(TXsect xsect, double y)
    {
        if (y >= xsect.yFull)
        {
            return xsect.rFull;
        }
        return S.xsect_getRofY(xsect, y);
    }

    // dwflow.c:637
    public static double checkNormalFlow(int j, double q, double y1, double y2, double a1, double r1)
    {
        int check = 0;
        int k = S.Link[j].subIndex;
        int n1 = S.Link[j].node1;
        int n2 = S.Link[j].node2;
        int hasOutfall = (((S.Node[n1].type == S.OUTFALL) || (S.Node[n2].type == S.OUTFALL)) ? 1 : 0);
        double qNorm = 0.0;
        double f1 = 0.0;
        if (((S.NormalFlowLtd == S.SLOPE) || (S.NormalFlowLtd == S.BOTH)) || (hasOutfall != 0))
        {
            if (y1 < y2)
            {
                check = 1;
            }
        }
        if (((!(check != 0)) && (((S.NormalFlowLtd == S.FROUDE) || (S.NormalFlowLtd == S.BOTH)))) && (!(hasOutfall != 0)))
        {
            if ((y1 > 0.0001) && (y2 > 0.0001))
            {
                f1 = S.link_getFroude(j, q / a1, y1);
                if (f1 >= 1.0)
                {
                    check = 1;
                }
            }
        }
        if ((check != 0))
        {
            qNorm = S.Conduit[k].beta * a1 * Math.Pow(r1, 2.0 / 3.0);
            if (qNorm < q)
            {
                S.Link[j].normalFlow = 1;
                return qNorm;
            }
        }
        return q;
    }

}
}
