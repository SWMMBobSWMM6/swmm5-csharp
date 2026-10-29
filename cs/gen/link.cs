// Generated from EPA SWMM 5.2.4 (link.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double MIN_DELTA_Z = 0.001;

    // link.c:137
    public static int link_readParams(int j, int type, int k, CPtr[] tok, int ntoks)
    {
        switch (type)
        {
            case S.CONDUIT:
                return S.conduit_readParams(j, k, tok, ntoks);
            case S.PUMP:
                return S.pump_readParams(j, k, tok, ntoks);
            case S.ORIFICE:
                return S.orifice_readParams(j, k, tok, ntoks);
            case S.WEIR:
                return S.weir_readParams(j, k, tok, ntoks);
            case S.OUTLET:
                return S.outlet_readParams(j, k, tok, ntoks);
            default:
                return 0;
        }
    }

    // link.c:162
    public static int link_readXsectParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        double[] x = new double[4];
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.LINK, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        k = S.findmatch(tok[1], S.XsectTypeWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        if (S.Link[j].type == S.CONDUIT)
        {
            S.Conduit[S.Link[j].subIndex].barrels = 1;
        }
        S.Link[j].xsect.culvertCode = 0;
        if (k == S.IRREGULAR)
        {
            i = S.project_findObject(S.TRANSECT, tok[2]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[2]);
            }
            S.Link[j].xsect.type = k;
            S.Link[j].xsect.transect = i;
            return 0;
        }
        else if (k == S.STREET_XSECT)
        {
            i = S.project_findObject(S.STREET, tok[2]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[2]);
            }
            S.Link[j].xsect.type = k;
            S.Link[j].xsect.transect = i;
            return 0;
        }
        else
        {
            if (ntoks < 6)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (k == S.CUSTOM)
            {
                if ((!((S.getDouble(tok[2], ref x[0]) != 0))) || (x[0] <= 0.0))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[2]);
                }
                i = S.project_findObject(S.CURVE, tok[3]);
                if (i < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[3]);
                }
                S.Link[j].xsect.type = k;
                S.Link[j].xsect.transect = i;
                S.Link[j].xsect.yFull = x[0] / S.UCF(S.LENGTH);
            }
            else
            {
                for (i = 2; i <= 5; i++)
                {
                    if (!((S.getDouble(tok[i], ref x[i - 2]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                    }
                }
            }
            if ((S.Link[j].type != S.CONDUIT) && (k == S.RECT_OPEN))
            {
                x[2] = 0.0;
                x[3] = 0.0;
            }
            if (!((S.xsect_setParams(S.Link[j].xsect, k, x, S.UCF(S.LENGTH)) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, S.L0);
            }
            if ((S.Link[j].type == S.CONDUIT) && (ntoks >= 7))
            {
                i = R.atoi(tok[6]);
                if (i <= 0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[6]);
                }
                else
                {
                    S.Conduit[S.Link[j].subIndex].barrels = i;
                }
            }
            if ((S.Link[j].type == S.CONDUIT) && (ntoks >= 8))
            {
                i = R.atoi(tok[7]);
                if (i < 0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[7]);
                }
                else
                {
                    S.Link[j].xsect.culvertCode = i;
                }
            }
        }
        return 0;
    }

    // link.c:271
    public static int link_readLossParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        double[] x = new double[3];
        double seepRate = 0.0;
        if (ntoks < 4)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.LINK, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        for (i = 1; i <= 3; i++)
        {
            if ((!((S.getDouble(tok[i], ref x[i - 1]) != 0))) || (x[i - 1] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i]);
            }
        }
        k = 0;
        if (ntoks >= 5)
        {
            k = S.findmatch(tok[4], S.NoYesWords);
            if (k < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[4]);
            }
        }
        if (ntoks >= 6)
        {
            if (!((S.getDouble(tok[5], ref seepRate) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[5]);
            }
        }
        S.Link[j].cLossInlet = x[0];
        S.Link[j].cLossOutlet = x[1];
        S.Link[j].cLossAvg = x[2];
        S.Link[j].hasFlapGate = k;
        S.Link[j].seepRate = seepRate / S.UCF(S.RAINFALL);
        return 0;
    }

    // link.c:315
    public static void link_setParams(int j, int type, int n1, int n2, int k, double[] x)
    {
        S.Link[j].node1 = n1;
        S.Link[j].node2 = n2;
        S.Link[j].type = type;
        S.Link[j].subIndex = k;
        S.Link[j].offset1 = 0.0;
        S.Link[j].offset2 = 0.0;
        S.Link[j].q0 = 0.0;
        S.Link[j].qFull = 0.0;
        S.Link[j].setting = 1.0;
        S.Link[j].targetSetting = 1.0;
        S.Link[j].hasFlapGate = 0;
        S.Link[j].qLimit = 0.0;
        S.Link[j].direction = 1;
        switch (type)
        {
            case S.CONDUIT:
                S.Conduit[k].length = x[0] / S.UCF(S.LENGTH);
                S.Conduit[k].modLength = S.Conduit[k].length;
                S.Conduit[k].roughness = x[1];
                S.Link[j].offset1 = x[2] / S.UCF(S.LENGTH);
                S.Link[j].offset2 = x[3] / S.UCF(S.LENGTH);
                S.Link[j].q0 = x[4] / S.UCF(S.FLOW);
                S.Link[j].qLimit = x[5] / S.UCF(S.FLOW);
                break;
            case S.PUMP:
                S.Pump[k].pumpCurve = R.D2I(x[0]);
                S.Link[j].hasFlapGate = 0;
                S.Pump[k].initSetting = x[1];
                S.Pump[k].yOn = x[2] / S.UCF(S.LENGTH);
                S.Pump[k].yOff = x[3] / S.UCF(S.LENGTH);
                S.Pump[k].xMin = 0.0;
                S.Pump[k].xMax = 0.0;
                break;
            case S.ORIFICE:
                S.Orifice[k].type = R.D2I(x[0]);
                S.Link[j].offset1 = x[1] / S.UCF(S.LENGTH);
                S.Link[j].offset2 = S.Link[j].offset1;
                S.Orifice[k].cDisch = x[2];
                S.Link[j].hasFlapGate = ((x[3] > 0.0) ? 1 : 0);
                S.Orifice[k].orate = x[4] * 3600.0;
                break;
            case S.WEIR:
                S.Weir[k].type = R.D2I(x[0]);
                S.Link[j].offset1 = x[1] / S.UCF(S.LENGTH);
                S.Link[j].offset2 = S.Link[j].offset1;
                S.Weir[k].cDisch1 = x[2];
                S.Link[j].hasFlapGate = ((x[3] > 0.0) ? 1 : 0);
                S.Weir[k].endCon = x[4];
                S.Weir[k].cDisch2 = x[5];
                S.Weir[k].canSurcharge = R.D2I(x[6]);
                S.Weir[k].roadWidth = x[7] / S.UCF(S.LENGTH);
                S.Weir[k].roadSurface = R.D2I(x[8]);
                S.Weir[k].cdCurve = R.D2I(x[9]);
                break;
            case S.OUTLET:
                S.Link[j].offset1 = x[0] / S.UCF(S.LENGTH);
                S.Link[j].offset2 = S.Link[j].offset1;
                S.Outlet[k].qCoeff = x[1];
                S.Outlet[k].qExpon = x[2];
                S.Outlet[k].qCurve = R.D2I(x[3]);
                S.Link[j].hasFlapGate = ((x[4] > 0.0) ? 1 : 0);
                S.Outlet[k].curveType = R.D2I(x[5]);
                S.xsect_setParams(S.Link[j].xsect, S.DUMMY, (null), 0.0);
                break;
        }
    }

    // link.c:403
    public static void link_validate(int j)
    {
        int n = 0;
        if (S.LinkOffsets == S.ELEV_OFFSET)
        {
            S.link_convertOffsets(j);
        }
        switch (S.Link[j].type)
        {
            case S.CONDUIT:
                S.conduit_validate(j, S.Link[j].subIndex);
                break;
            case S.PUMP:
                S.pump_validate(j, S.Link[j].subIndex);
                break;
            case S.ORIFICE:
                S.orifice_validate(j, S.Link[j].subIndex);
                break;
            case S.WEIR:
                S.weir_validate(j, S.Link[j].subIndex);
                break;
        }
        switch (S.Link[j].type)
        {
            case S.ORIFICE:
            case S.WEIR:
            case S.OUTLET:
                if (S.Node[S.Link[j].node1].invertElev + S.Link[j].offset1 < S.Node[S.Link[j].node2].invertElev)
                {
                    if (S.RouteModel == S.DW)
                    {
                        S.Link[j].offset1 = S.Node[S.Link[j].node2].invertElev - S.Node[S.Link[j].node1].invertElev;
                        S.report_writeWarningMsg(S.L305, S.Link[j].ID);
                    }
                    else
                    {
                        S.report_writeWarningMsg(S.L306, S.Link[j].ID);
                    }
                }
                break;
        }
        if ((S.Link[j].type == S.PUMP) || (((S.Link[j].type == S.ORIFICE) && (S.Orifice[S.Link[j].subIndex].type == S.BOTTOM_ORIFICE))))
        {
            return;
        }
        n = S.Link[j].node1;
        if ((S.Node[n].type != S.STORAGE) || (S.Node[n].surDepth > 0.0))
        {
            S.Node[n].fullDepth = ((((S.Node[n].fullDepth) >= (S.Link[j].offset1 + S.Link[j].xsect.yFull)) ? (S.Node[n].fullDepth) : (S.Link[j].offset1 + S.Link[j].xsect.yFull)));
        }
        n = S.Link[j].node2;
        if ((((S.Node[n].type != S.STORAGE) || (S.Node[n].surDepth > 0.0))) && (S.Link[j].type == S.CONDUIT))
        {
            S.Node[n].fullDepth = ((((S.Node[n].fullDepth) >= (S.Link[j].offset2 + S.Link[j].xsect.yFull)) ? (S.Node[n].fullDepth) : (S.Link[j].offset2 + S.Link[j].xsect.yFull)));
        }
    }

    // link.c:468
    public static void link_convertOffsets(int j)
    {
        double elev = 0.0;
        elev = S.Node[S.Link[j].node1].invertElev;
        S.Link[j].offset1 = S.link_getOffsetHeight(j, S.Link[j].offset1, elev);
        if (S.Link[j].type == S.CONDUIT)
        {
            elev = S.Node[S.Link[j].node2].invertElev;
            S.Link[j].offset2 = S.link_getOffsetHeight(j, S.Link[j].offset2, elev);
        }
        else
        {
            S.Link[j].offset2 = S.Link[j].offset1;
        }
    }

    // link.c:489
    public static double link_getOffsetHeight(int j, double offset, double elev)
    {
        if ((offset <= -10000000000.0) || (S.Link[j].type == S.PUMP))
        {
            return 0.0;
        }
        offset -= elev;
        if (offset >= 0.0)
        {
            return offset;
        }
        if (offset >= -S.MIN_DELTA_Z)
        {
            return 0.0;
        }
        S.report_writeWarningMsg(S.L307, S.Link[j].ID);
        return 0.0;
    }

    // link.c:508
    public static void link_initState(int j)
    {
        int p = 0;
        S.Link[j].oldFlow = S.Link[j].q0;
        S.Link[j].newFlow = S.Link[j].q0;
        S.Link[j].oldDepth = 0.0;
        S.Link[j].newDepth = 0.0;
        S.Link[j].oldVolume = 0.0;
        S.Link[j].newVolume = 0.0;
        S.Link[j].setting = 1.0;
        S.Link[j].targetSetting = 1.0;
        S.Link[j].timeLastSet = S.StartDate;
        S.Link[j].inletControl = 0;
        S.Link[j].normalFlow = 0;
        if (S.Link[j].type == S.CONDUIT)
        {
            S.conduit_initState(j, S.Link[j].subIndex);
        }
        if (S.Link[j].type == S.PUMP)
        {
            S.pump_initState(j, S.Link[j].subIndex);
        }
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.Link[j].oldQual[p] = 0.0;
            S.Link[j].newQual[p] = 0.0;
            S.Link[j].totalLoad[p] = 0.0;
        }
    }

    // link.c:543
    public static double link_getInflow(int j)
    {
        if (S.Link[j].setting == 0)
        {
            return 0.0;
        }
        switch (S.Link[j].type)
        {
            case S.CONDUIT:
                return S.conduit_getInflow(j);
            case S.PUMP:
                return S.pump_getInflow(j);
            case S.ORIFICE:
                return S.orifice_getInflow(j);
            case S.WEIR:
                return S.weir_getInflow(j);
            case S.OUTLET:
                return S.outlet_getInflow(j);
            default:
                return S.node_getOutflow(S.Link[j].node1, j);
        }
    }

    // link.c:564
    public static void link_setOldHydState(int j)
    {
        int k = 0;
        S.Link[j].oldDepth = S.Link[j].newDepth;
        S.Link[j].oldFlow = S.Link[j].newFlow;
        S.Link[j].oldVolume = S.Link[j].newVolume;
        if (S.Link[j].type == S.CONDUIT)
        {
            k = S.Link[j].subIndex;
            S.Conduit[k].q1Old = S.Conduit[k].q1;
            S.Conduit[k].q2Old = S.Conduit[k].q2;
        }
    }

    // link.c:587
    public static void link_setOldQualState(int j)
    {
        int p = 0;
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.Link[j].oldQual[p] = S.Link[j].newQual[p];
            S.Link[j].newQual[p] = 0.0;
        }
    }

    // link.c:604
    public static void link_setTargetSetting(int j)
    {
        int k = 0;
        int n1 = 0;
        if (S.Link[j].type == S.PUMP)
        {
            k = S.Link[j].subIndex;
            n1 = S.Link[j].node1;
            S.Link[j].targetSetting = S.Link[j].setting;
            if (((S.Pump[k].yOff > 0.0) && (S.Link[j].setting > 0.0)) && (S.Node[n1].newDepth < S.Pump[k].yOff))
            {
                S.Link[j].targetSetting = 0.0;
            }
            if (((S.Pump[k].yOn > 0.0) && (S.Link[j].setting == 0.0)) && (S.Node[n1].newDepth > S.Pump[k].yOn))
            {
                S.Link[j].targetSetting = 1.0;
            }
        }
    }

    // link.c:628
    public static void link_setSetting(int j, double tstep)
    {
        if (S.Link[j].type == S.ORIFICE)
        {
            S.orifice_setSetting(j, tstep);
        }
        else if (S.Link[j].type == S.WEIR)
        {
            S.weir_setSetting(j);
        }
        else
        {
            S.Link[j].setting = S.Link[j].targetSetting;
        }
    }

    // link.c:643
    public static int link_setFlapGate(int j, int n1, int n2, double q)
    {
        int n = -1;
        if ((S.Link[j].hasFlapGate != 0))
        {
            if (q * (double)(S.Link[j].direction) < 0.0)
            {
                return 1;
            }
        }
        if (q < 0.0)
        {
            n = n2;
        }
        if (q > 0.0)
        {
            n = n1;
        }
        if (((n >= 0) && (S.Node[n].type == S.OUTFALL)) && (S.Outfall[S.Node[n].subIndex].hasFlapGate != 0))
        {
            return 1;
        }
        return 0;
    }

    // link.c:674
    public static void link_getResults(int j, double f, float[] x)
    {
        int p = 0;
        double y = 0.0;
        double q = 0.0;
        double u = 0.0;
        double v = 0.0;
        double c = 0.0;
        double f1 = 1.0 - f;
        y = f1 * S.Link[j].oldDepth + f * S.Link[j].newDepth;
        q = f1 * S.Link[j].oldFlow + f * S.Link[j].newFlow;
        v = f1 * S.Link[j].oldVolume + f * S.Link[j].newVolume;
        u = S.link_getVelocity(j, q, y);
        c = 0.0;
        if (S.Link[j].type == S.CONDUIT)
        {
            if (S.Link[j].xsect.type != S.DUMMY)
            {
                c = S.xsect_getAofY(S.Link[j].xsect, y) / S.Link[j].xsect.aFull;
            }
        }
        else
        {
            c = S.Link[j].setting;
        }
        if ((S.Link[j].type == S.PUMP) && (S.Link[j].oldFlow * S.Link[j].newFlow == 0.0))
        {
            if (f >= f1)
            {
                q = S.Link[j].newFlow;
            }
            else
            {
                q = S.Link[j].oldFlow;
            }
        }
        y *= S.UCF(S.LENGTH);
        v *= S.UCF(S.VOLUME);
        q *= S.UCF(S.FLOW) * (double)(S.Link[j].direction);
        u *= S.UCF(S.LENGTH) * (double)(S.Link[j].direction);
        x[S.LINK_DEPTH] = (float)(y);
        x[S.LINK_FLOW] = (float)(q);
        x[S.LINK_VELOCITY] = (float)(u);
        x[S.LINK_VOLUME] = (float)(v);
        x[S.LINK_CAPACITY] = (float)(c);
        if (!(S.IgnoreQuality != 0))
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                c = f1 * S.Link[j].oldQual[p] + f * S.Link[j].newQual[p];
                x[S.LINK_QUAL + p] = (float)(c);
            }
        }
    }

    // link.c:728
    public static void link_setOutfallDepth(int j)
    {
        int k = 0;
        int n = 0;
        double z = 0.0;
        double q = 0.0;
        double yCrit = 0.0;
        double yNorm = 0.0;
        if (S.Node[S.Link[j].node2].type == S.OUTFALL)
        {
            n = S.Link[j].node2;
            z = S.Link[j].offset2;
        }
        else if (S.Node[S.Link[j].node1].type == S.OUTFALL)
        {
            n = S.Link[j].node1;
            z = S.Link[j].offset1;
        }
        else
        {
            return;
        }
        if (S.Link[j].type == S.CONDUIT)
        {
            k = S.Link[j].subIndex;
            q = Math.Abs(S.Link[j].newFlow / S.Conduit[k].barrels);
            yNorm = S.link_getYnorm(j, q);
            yCrit = S.link_getYcrit(j, q);
        }
        S.node_setOutletDepth(n, yNorm, yCrit, z);
    }

    // link.c:770
    public static double link_getYcrit(int j, double q)
    {
        return S.xsect_getYcrit(S.Link[j].xsect, q);
    }

    // link.c:783
    public static double link_getYnorm(int j, double q)
    {
        int k = 0;
        double s = 0.0;
        double a = 0.0;
        double y = 0.0;
        if (S.Link[j].type != S.CONDUIT)
        {
            return 0.0;
        }
        if (S.Link[j].xsect.type == S.DUMMY)
        {
            return 0.0;
        }
        q = Math.Abs(q);
        k = S.Link[j].subIndex;
        if (q > S.Conduit[k].qMax)
        {
            q = S.Conduit[k].qMax;
        }
        if (q <= 0.0)
        {
            return 0.0;
        }
        s = q / S.Conduit[k].beta;
        a = S.xsect_getAofS(S.Link[j].xsect, s);
        y = S.xsect_getYofA(S.Link[j].xsect, a);
        return y;
    }

    // link.c:808
    public static double link_getLength(int j)
    {
        if (S.Link[j].type == S.CONDUIT)
        {
            return S.conduit_getLength(j);
        }
        return 0.0;
    }

    // link.c:821
    public static double link_getVelocity(int j, double flow, double depth)
    {
        double area = 0.0;
        double veloc = 0.0;
        int k = 0;
        if (depth <= 0.01)
        {
            return 0.0;
        }
        if (S.Link[j].type == S.CONDUIT)
        {
            k = S.Link[j].subIndex;
            flow /= S.Conduit[k].barrels;
            area = S.xsect_getAofY(S.Link[j].xsect, depth);
            if (area > 0.0001)
            {
                veloc = flow / area;
            }
        }
        return veloc;
    }

    // link.c:847
    public static double link_getFroude(int j, double v, double y)
    {
        TXsect xsect = S.Link[j].xsect;
        if (S.Link[j].type != S.CONDUIT)
        {
            return 0.0;
        }
        if (y <= 0.0001)
        {
            return 0.0;
        }
        if ((!((S.xsect_isOpen(xsect.type) != 0))) && (xsect.yFull - y <= 0.0001))
        {
            return 0.0;
        }
        y = S.xsect_getAofY(xsect, y) / S.xsect_getWofY(xsect, y);
        return Math.Abs(v) / Math.Sqrt(32.2 * y);
    }

    // link.c:875
    public static double link_getPower(int j)
    {
        int n1 = S.Link[j].node1;
        int n2 = S.Link[j].node2;
        double dh = (S.Node[n1].invertElev + S.Node[n1].newDepth) - (S.Node[n2].invertElev + S.Node[n2].newDepth);
        double q = Math.Abs(S.Link[j].newFlow);
        return Math.Abs(dh) * q / 8.814 * 0.7457;
    }

    // link.c:893
    public static double link_getLossRate(int j, int routeModel, double q, double tstep)
    {
        if (S.Link[j].type == S.CONDUIT)
        {
            return S.conduit_getLossRate(j, routeModel, q, tstep);
        }
        else
        {
            return 0.0;
        }
    }

    // link.c:911
    public static int link_getFullState(double a1, double a2, double aFull)
    {
        if (a1 >= aFull)
        {
            if (a2 >= aFull)
            {
                return S.ALL_FULL;
            }
            else
            {
                return S.UP_FULL;
            }
        }
        if (a2 >= aFull)
        {
            return S.DN_FULL;
        }
        return 0;
    }

    // link.c:933
    public static int conduit_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int n1 = 0;
        int n2 = 0;
        double[] x = new double[6];
        CPtr id = CPtr.Null;
        if (ntoks < 7)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.LINK, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        n1 = S.project_findObject(S.NODE, tok[1]);
        if (n1 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        n2 = S.project_findObject(S.NODE, tok[2]);
        if (n2 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        if (!((S.getDouble(tok[3], ref x[0]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[3]);
        }
        if (!((S.getDouble(tok[4], ref x[1]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[4]);
        }
        if ((S.LinkOffsets == S.ELEV_OFFSET) && (tok[5][0] == '*'))
        {
            x[2] = -10000000000.0;
        }
        else if (!((S.getDouble(tok[5], ref x[2]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[5]);
        }
        if ((S.LinkOffsets == S.ELEV_OFFSET) && (tok[6][0] == '*'))
        {
            x[3] = -10000000000.0;
        }
        else if (!((S.getDouble(tok[6], ref x[3]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[6]);
        }
        x[4] = 0.0;
        if (ntoks >= 8)
        {
            if (!((S.getDouble(tok[7], ref x[4]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[7]);
            }
        }
        x[5] = 0.0;
        if (ntoks >= 9)
        {
            if (!((S.getDouble(tok[8], ref x[5]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[8]);
            }
        }
        S.Link[j].ID = id;
        S.link_setParams(j, S.CONDUIT, n1, n2, k, x);
        return 0;
    }

    // link.c:992
    public static void conduit_validate(int j, int k)
    {
        double aa = 0.0;
        double lengthFactor = 0.0;
        double roughness = 0.0;
        double slope = 0.0;
        if ((S.Link[j].xsect.type == S.DUMMY) && (S.RouteModel == S.DW))
        {
            if (S.Node[S.Link[j].node1].type == S.STORAGE)
            {
                S.report_writeErrorMsg(S.ERR_DUMMY_LINK, S.Node[S.Link[j].node1].ID);
                return;
            }
        }
        if (S.Link[j].xsect.type == S.CUSTOM)
        {
            S.xsect_setCustomXsectParams(S.Link[j].xsect);
        }
        if (S.Link[j].xsect.type == S.IRREGULAR)
        {
            S.xsect_setIrregXsectParams(S.Link[j].xsect);
            S.Conduit[k].roughness = S.Transect[S.Link[j].xsect.transect].roughness;
        }
        if (S.Link[j].xsect.type == S.STREET_XSECT)
        {
            S.xsect_setStreetXsectParams(S.Link[j].xsect);
            S.Conduit[k].roughness = S.Street[S.Link[j].xsect.transect].roughness;
        }
        if (S.Link[j].xsect.type == S.FORCE_MAIN)
        {
            if (S.ForceMainEqn == S.D_W)
            {
                S.Link[j].xsect.rBot /= S.UCF(S.RAINDEPTH);
            }
            if (S.Link[j].xsect.rBot <= 0.0)
            {
                S.report_writeErrorMsg(S.ERR_XSECT, S.Link[j].ID);
            }
        }
        if (S.Conduit[k].length <= 0.0)
        {
            S.report_writeErrorMsg(S.ERR_LENGTH, S.Link[j].ID);
        }
        if (S.Conduit[k].roughness <= 0.0)
        {
            S.report_writeErrorMsg(S.ERR_ROUGHNESS, S.Link[j].ID);
        }
        if (S.Conduit[k].barrels <= 0)
        {
            S.report_writeErrorMsg(S.ERR_BARRELS, S.Link[j].ID);
        }
        if (S.Link[j].xsect.type != S.DUMMY)
        {
            if (S.Link[j].xsect.type < 0)
            {
                S.report_writeErrorMsg(S.ERR_NO_XSECT, S.Link[j].ID);
            }
            else if (S.Link[j].xsect.aFull <= 0.0)
            {
                S.report_writeErrorMsg(S.ERR_XSECT, S.Link[j].ID);
            }
        }
        if ((S.ErrorCode != 0))
        {
            return;
        }
        if (S.Link[j].offset1 < 0.0)
        {
            S.report_writeWarningMsg(S.L307, S.Link[j].ID);
            S.Link[j].offset1 = 0.0;
        }
        if (S.Link[j].offset2 < 0.0)
        {
            S.report_writeWarningMsg(S.L307, S.Link[j].ID);
            S.Link[j].offset2 = 0.0;
        }
        if (S.Link[j].xsect.type == S.FILLED_CIRCULAR)
        {
            S.Link[j].offset1 += S.Link[j].xsect.yBot;
            S.Link[j].offset2 += S.Link[j].xsect.yBot;
        }
        slope = S.conduit_getSlope(j);
        S.Conduit[k].slope = slope;
        if (((S.RouteModel == S.DW) && (slope < 0.0)) && (S.Link[j].xsect.type != S.DUMMY))
        {
            S.conduit_reverse(j, k);
        }
        roughness = S.Conduit[k].roughness;
        if ((S.RouteModel == S.DW) && (S.Link[j].xsect.type == S.FORCE_MAIN))
        {
            roughness = S.forcemain_getEquivN(j, k);
        }
        if (S.Link[j].xsect.type == S.IRREGULAR)
        {
            lengthFactor = S.Transect[S.Link[j].xsect.transect].lengthFactor;
            roughness *= Math.Sqrt(lengthFactor);
        }
        lengthFactor = 1.0;
        if (((S.RouteModel == S.DW) && (S.LengtheningStep > 0.0)) && (S.Link[j].xsect.type != S.DUMMY))
        {
            lengthFactor = S.conduit_getLengthFactor(j, k, roughness);
        }
        if (lengthFactor != 1.0)
        {
            S.Conduit[k].modLength = lengthFactor * S.conduit_getLength(j);
            slope /= lengthFactor;
            roughness = roughness / Math.Sqrt(lengthFactor);
        }
        if ((S.RouteModel == S.DW) && (S.Link[j].xsect.type == S.FORCE_MAIN))
        {
            S.Link[j].xsect.sBot = S.forcemain_getRoughFactor(j, lengthFactor);
        }
        S.Conduit[k].roughFactor = 32.2 * ((roughness / 1.486) * (roughness / 1.486));
        if (S.Link[j].xsect.type == S.DUMMY)
        {
            S.Conduit[k].beta = 0.0;
        }
        else
        {
            S.Conduit[k].beta = 1.486 * Math.Sqrt(Math.Abs(slope)) / roughness;
        }
        S.Link[j].qFull = S.Link[j].xsect.sFull * S.Conduit[k].beta;
        S.Conduit[k].qMax = S.Link[j].xsect.sMax * S.Conduit[k].beta;
        aa = S.Conduit[k].beta / Math.Sqrt(32.2) * Math.Pow(S.Link[j].xsect.yFull, 0.1666667) * 0.3;
        if (aa >= 1.0)
        {
            S.Conduit[k].superCritical = 1;
        }
        else
        {
            S.Conduit[k].superCritical = 0;
        }
        if (((S.Link[j].cLossInlet == 0.0) && (S.Link[j].cLossOutlet == 0.0)) && (S.Link[j].cLossAvg == 0.0))
        {
            S.Conduit[k].hasLosses = 0;
        }
        else
        {
            S.Conduit[k].hasLosses = 1;
        }
    }

    // link.c:1158
    public static void conduit_reverse(int j, int k)
    {
        int i = 0;
        double z = 0.0;
        double cLoss = 0.0;
        i = S.Link[j].node1;
        S.Link[j].node1 = S.Link[j].node2;
        S.Link[j].node2 = i;
        z = S.Link[j].offset1;
        S.Link[j].offset1 = S.Link[j].offset2;
        S.Link[j].offset2 = z;
        cLoss = S.Link[j].cLossInlet;
        S.Link[j].cLossInlet = S.Link[j].cLossOutlet;
        S.Link[j].cLossOutlet = cLoss;
        S.Conduit[k].slope = -S.Conduit[k].slope;
        S.Link[j].direction *= -1;
        S.Link[j].q0 = -S.Link[j].q0;
    }

    // link.c:1195
    public static double conduit_getLength(int j)
    {
        int k = S.Link[j].subIndex;
        int t = 0;
        if (S.Link[j].xsect.type != S.IRREGULAR)
        {
            return S.Conduit[k].length;
        }
        t = S.Link[j].xsect.transect;
        if ((t < 0) || (t >= S.Nobjects[S.TRANSECT]))
        {
            return S.Conduit[k].length;
        }
        return S.Conduit[k].length / S.Transect[t].lengthFactor;
    }

    // link.c:1217
    public static double conduit_getLengthFactor(int j, int k, double roughness)
    {
        double ratio = 0.0;
        double yFull = 0.0;
        double vFull = 0.0;
        double tStep = 0.0;
        yFull = S.Link[j].xsect.yFull;
        if ((S.xsect_isOpen(S.Link[j].xsect.type) != 0))
        {
            yFull = S.Link[j].xsect.aFull / S.xsect_getWofY(S.Link[j].xsect, yFull);
        }
        vFull = 1.486 / roughness * S.Link[j].xsect.sFull * Math.Sqrt(Math.Abs(S.Conduit[k].slope)) / S.Link[j].xsect.aFull;
        if (S.LengtheningStep == 0.0)
        {
            tStep = S.RouteStep;
        }
        else
        {
            tStep = ((((S.RouteStep) <= (S.LengtheningStep)) ? (S.RouteStep) : (S.LengtheningStep)));
        }
        ratio = (Math.Sqrt(32.2 * yFull) + vFull) * tStep / S.conduit_getLength(j);
        if (ratio > 1.0)
        {
            return ratio;
        }
        else
        {
            return 1.0;
        }
    }

    // link.c:1258
    public static double conduit_getSlope(int j)
    {
        double elev1 = 0.0;
        double elev2 = 0.0;
        double delta = 0.0;
        double slope = 0.0;
        double length = S.conduit_getLength(j);
        elev1 = S.Link[j].offset1 + S.Node[S.Link[j].node1].invertElev;
        elev2 = S.Link[j].offset2 + S.Node[S.Link[j].node2].invertElev;
        delta = Math.Abs(elev1 - elev2);
        if (delta < S.MIN_DELTA_Z)
        {
            S.report_writeWarningMsg(S.L308, S.Link[j].ID);
            delta = S.MIN_DELTA_Z;
        }
        if (delta >= length)
        {
            S.report_writeWarningMsg(S.L309, S.Link[j].ID);
            slope = delta / length;
        }
        else
        {
            slope = delta / Math.Sqrt(((length) * (length)) - ((delta) * (delta)));
        }
        if ((S.MinSlope > 0.0) && (slope < S.MinSlope))
        {
            S.report_writeWarningMsg(S.L310, S.Link[j].ID);
            slope = S.MinSlope;
            if ((S.RouteModel == S.SF) || (S.RouteModel == S.KW))
            {
                return slope;
            }
        }
        if (elev1 < elev2)
        {
            slope = -slope;
        }
        return slope;
    }

    // link.c:1304
    public static void conduit_initState(int j, int k)
    {
        S.Link[j].newDepth = S.link_getYnorm(j, S.Link[j].q0 / S.Conduit[k].barrels);
        S.Link[j].oldDepth = S.Link[j].newDepth;
        S.Conduit[k].evapLossRate = 0.0;
        S.Conduit[k].seepLossRate = 0.0;
    }

    // link.c:1320
    public static double conduit_getInflow(int j)
    {
        double qIn = S.node_getOutflow(S.Link[j].node1, j);
        if (S.Link[j].qLimit > 0.0)
        {
            qIn = ((((qIn) <= (S.Link[j].qLimit)) ? (qIn) : (S.Link[j].qLimit)));
        }
        return qIn;
    }

    // link.c:1334
    public static double conduit_getLossRate(int j, int routeModel, double q, double tstep)
    {
        TXsect xsect = null;
        double depth = 0.5 * (S.Link[j].oldDepth + S.Link[j].newDepth);
        double length = 0.0;
        double width = 0.0;
        double topWidth = 0.0;
        double evapLossRate = 0.0;
        double seepLossRate = 0.0;
        double totalLossRate = 0.0;
        if (depth > 0.0001)
        {
            xsect = S.Link[j].xsect;
            length = S.conduit_getLength(j);
            if (((S.xsect_isOpen(xsect.type) != 0)) && (S.Evap.rate > 0.0))
            {
                topWidth = S.xsect_getWofY(xsect, depth);
                evapLossRate = topWidth * length * S.Evap.rate;
            }
            if (S.Link[j].seepRate > 0.0)
            {
                if (xsect.type == S.RECT_CLOSED)
                {
                    width = xsect.wMax;
                }
                else
                {
                    if (depth >= xsect.ywMax)
                    {
                        depth = xsect.ywMax;
                    }
                    width = S.xsect_getWofY(xsect, depth);
                }
                seepLossRate = S.Link[j].seepRate * width * length;
                seepLossRate *= S.Adjust.hydconFactor;
            }
            totalLossRate = evapLossRate + seepLossRate;
            if (routeModel == S.DW)
            {
                q = S.Link[j].newVolume / tstep;
            }
            else
            {
                q = ((((q) < 0) ? -(q) : (q)));
            }
            if (totalLossRate > q)
            {
                evapLossRate = evapLossRate * q / totalLossRate;
                seepLossRate = seepLossRate * q / totalLossRate;
                totalLossRate = q;
            }
        }
        S.Conduit[S.Link[j].subIndex].evapLossRate = evapLossRate;
        S.Conduit[S.Link[j].subIndex].seepLossRate = seepLossRate;
        return totalLossRate;
    }

    // link.c:1406
    public static int pump_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int m = 0;
        int n1 = 0;
        int n2 = 0;
        double[] x = new double[4];
        CPtr id = CPtr.Null;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.LINK, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        n1 = S.project_findObject(S.NODE, tok[1]);
        if (n1 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        n2 = S.project_findObject(S.NODE, tok[2]);
        if (n2 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        x[0] = -1.0;
        if (ntoks >= 4)
        {
            if (!((S.strcomp(tok[3], S.L275) != 0)))
            {
                m = S.project_findObject(S.CURVE, tok[3]);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[3]);
                }
                x[0] = m;
            }
        }
        x[1] = 1.0;
        if (ntoks >= 5)
        {
            m = S.findmatch(tok[4], S.OffOnWords);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[4]);
            }
            x[1] = m;
        }
        x[2] = 0.0;
        if (ntoks >= 6)
        {
            if ((!((S.getDouble(tok[5], ref x[2]) != 0))) || (x[2] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[5]);
            }
        }
        x[3] = 0.0;
        if (ntoks >= 7)
        {
            if ((!((S.getDouble(tok[6], ref x[3]) != 0))) || (x[3] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[6]);
            }
        }
        S.Link[j].ID = id;
        S.link_setParams(j, S.PUMP, n1, n2, k, x);
        return 0;
    }

    // link.c:1473
    public static void pump_validate(int j, int k)
    {
        int m = 0;
        int n1 = 0;
        double x = 0.0;
        double y = 0.0;
        S.Link[j].xsect.yFull = 0.0;
        m = S.Pump[k].pumpCurve;
        if (m < 0)
        {
            S.Pump[k].type = S.IDEAL_PUMP;
        }
        else
        {
            if ((S.Curve[m].curveType < S.PUMP1_CURVE) || (S.Curve[m].curveType > S.PUMP5_CURVE))
            {
                S.report_writeErrorMsg(S.ERR_NO_CURVE, S.Link[j].ID);
            }
            else
            {
                S.Pump[k].type = S.Curve[m].curveType - S.PUMP1_CURVE;
                if ((S.table_getFirstEntry(S.Curve[m], ref x, ref y) != 0))
                {
                    S.Link[j].qFull = y;
                    S.Pump[k].xMin = x;
                    S.Pump[k].xMax = x;
                    while ((S.table_getNextEntry(S.Curve[m], ref x, ref y) != 0))
                    {
                        S.Link[j].qFull = ((((y) >= (S.Link[j].qFull)) ? (y) : (S.Link[j].qFull)));
                        S.Pump[k].xMax = x;
                    }
                }
                S.Link[j].qFull /= S.UCF(S.FLOW);
            }
        }
        if ((S.Pump[k].yOn > 0.0) && (S.Pump[k].yOn <= S.Pump[k].yOff))
        {
            S.report_writeErrorMsg(S.ERR_PUMP_LIMITS, S.Link[j].ID);
        }
        if (S.Pump[k].type == S.TYPE1_PUMP)
        {
            n1 = S.Link[j].node1;
            if (S.Node[n1].type != S.STORAGE)
            {
                S.Node[n1].fullVolume = ((((S.Node[n1].fullVolume) >= (S.Pump[k].xMax / S.UCF(S.VOLUME))) ? (S.Node[n1].fullVolume) : (S.Pump[k].xMax / S.UCF(S.VOLUME))));
            }
        }
    }

    // link.c:1534
    public static void pump_initState(int j, int k)
    {
        S.Link[j].setting = S.Pump[k].initSetting;
        S.Link[j].targetSetting = S.Pump[k].initSetting;
    }

    // link.c:1548
    public static double pump_getInflow(int j)
    {
        int k = 0;
        int m = 0;
        int n1 = 0;
        int n2 = 0;
        double vol = 0.0;
        double depth = 0.0;
        double head = 0.0;
        double qIn = 0.0;
        double qIn1 = 0.0;
        double dh = 0.001;
        double s = 1.0;
        k = S.Link[j].subIndex;
        m = S.Pump[k].pumpCurve;
        n1 = S.Link[j].node1;
        n2 = S.Link[j].node2;
        S.Link[j].flowClass = S.NO;
        S.Link[j].setting = S.Link[j].targetSetting;
        if (S.Link[j].setting == 0.0)
        {
            return 0.0;
        }
        if (S.Pump[k].type == S.IDEAL_PUMP)
        {
            qIn = S.Node[n1].inflow + S.Node[n1].overflow;
        }
        else
        {
            switch (S.Curve[m].curveType)
            {
                case S.PUMP1_CURVE:
                    vol = S.Node[n1].newVolume * S.UCF(S.VOLUME);
                    qIn = S.table_intervalLookup(S.Curve[m], vol) / S.UCF(S.FLOW);
                    if ((vol < S.Pump[k].xMin) || (vol > S.Pump[k].xMax))
                    {
                        S.Link[j].flowClass = S.YES;
                    }
                    break;
                case S.PUMP2_CURVE:
                    depth = S.Node[n1].newDepth * S.UCF(S.LENGTH);
                    qIn = S.table_intervalLookup(S.Curve[m], depth) / S.UCF(S.FLOW);
                    if ((depth < S.Pump[k].xMin) || (depth > S.Pump[k].xMax))
                    {
                        S.Link[j].flowClass = S.YES;
                    }
                    break;
                case S.PUMP3_CURVE:
                case S.PUMP5_CURVE:
                    if (S.Curve[m].curveType == S.PUMP5_CURVE)
                    {
                        s = S.Link[j].setting;
                    }
                    head = ((S.Node[n2].newDepth + S.Node[n2].invertElev) - (S.Node[n1].newDepth + S.Node[n1].invertElev)) / s / s;
                    head = ((((head) >= (0.0)) ? (head) : (0.0))) * S.UCF(S.LENGTH);
                    qIn = S.table_lookup(S.Curve[m], head) / S.UCF(S.FLOW);
                    S.Link[j].dqdh = -(S.table_getSlope(S.Curve[m], head)) * S.UCF(S.LENGTH) / S.UCF(S.FLOW) / s;
                    if ((head < S.Pump[k].xMin) || (head > S.Pump[k].xMax))
                    {
                        S.Link[j].flowClass = S.YES;
                    }
                    break;
                case S.PUMP4_CURVE:
                    depth = S.Node[n1].newDepth;
                    qIn = S.table_lookup(S.Curve[m], depth * S.UCF(S.LENGTH)) / S.UCF(S.FLOW);
                    qIn1 = S.table_lookup(S.Curve[m], (depth + dh) * S.UCF(S.LENGTH)) / S.UCF(S.FLOW);
                    S.Link[j].dqdh = (qIn1 - qIn) / dh;
                    depth *= S.UCF(S.LENGTH);
                    if (depth < S.Pump[k].xMin)
                    {
                        S.Link[j].flowClass = S.DN_DRY;
                    }
                    if (depth > S.Pump[k].xMax)
                    {
                        S.Link[j].flowClass = S.UP_DRY;
                    }
                    break;
                default:
                    qIn = 0.0;
                    break;
            }
        }
        if (qIn < 0.0)
        {
            qIn = 0.0;
        }
        return qIn * S.Link[j].setting;
    }

    // link.c:1641
    public static int orifice_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int m = 0;
        int n1 = 0;
        int n2 = 0;
        double[] x = new double[5];
        CPtr id = CPtr.Null;
        if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.LINK, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        n1 = S.project_findObject(S.NODE, tok[1]);
        if (n1 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        n2 = S.project_findObject(S.NODE, tok[2]);
        if (n2 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        m = S.findmatch(tok[3], S.OrificeTypeWords);
        if (m < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
        }
        x[0] = m;
        if ((S.LinkOffsets == S.ELEV_OFFSET) && (tok[4][0] == '*'))
        {
            x[1] = -10000000000.0;
        }
        else if (!((S.getDouble(tok[4], ref x[1]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[4]);
        }
        if ((!((S.getDouble(tok[5], ref x[2]) != 0))) || (x[2] < 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[5]);
        }
        x[3] = 0.0;
        if (ntoks >= 7)
        {
            m = S.findmatch(tok[6], S.NoYesWords);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[6]);
            }
            x[3] = m;
        }
        x[4] = 0.0;
        if (ntoks >= 8)
        {
            if ((!((S.getDouble(tok[7], ref x[4]) != 0))) || (x[4] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[7]);
            }
        }
        S.Link[j].ID = id;
        S.link_setParams(j, S.ORIFICE, n1, n2, k, x);
        return 0;
    }

    // link.c:1696
    public static void orifice_validate(int j, int k)
    {
        int err = 0;
        if ((S.Link[j].xsect.type != S.RECT_CLOSED) && (S.Link[j].xsect.type != S.CIRCULAR))
        {
            err = S.ERR_REGULATOR_SHAPE;
        }
        if (err > 0)
        {
            S.report_writeErrorMsg(err, S.Link[j].ID);
            return;
        }
        if (S.Link[j].offset1 < 0.0)
        {
            S.Link[j].offset1 = 0.0;
        }
        S.orifice_setSetting(j, 0.0);
        S.Orifice[k].length = 2.0 * S.RouteStep * Math.Sqrt(32.2 * S.Link[j].xsect.yFull);
        S.Orifice[k].length = ((((200.0) >= (S.Orifice[k].length)) ? (200.0) : (S.Orifice[k].length)));
        S.Orifice[k].surfArea = 0.0;
    }

    // link.c:1729
    public static void orifice_setSetting(int j, double tstep)
    {
        int k = S.Link[j].subIndex;
        double delta = 0.0;
        double step = 0.0;
        double h = 0.0;
        double f = 0.0;
        if ((S.Orifice[k].orate == 0.0) || (tstep == 0.0))
        {
            S.Link[j].setting = S.Link[j].targetSetting;
        }
        else
        {
            delta = S.Link[j].targetSetting - S.Link[j].setting;
            step = tstep / S.Orifice[k].orate;
            if (step + 0.001 >= Math.Abs(delta))
            {
                S.Link[j].setting = S.Link[j].targetSetting;
            }
            else
            {
                S.Link[j].setting += ((((delta) < 0) ? (-1) : (1))) * step;
            }
        }
        h = S.Link[j].setting * S.Link[j].xsect.yFull;
        f = S.xsect_getAofY(S.Link[j].xsect, h) * Math.Sqrt(2.0 * 32.2);
        S.Orifice[k].cOrif = S.Orifice[k].cDisch * f;
        S.Orifice[k].cWeir = S.orifice_getWeirCoeff(j, k, h) * f;
    }

    // link.c:1766
    public static double orifice_getWeirCoeff(int j, int k, double h)
    {
        double w = 0.0;
        double aOverL = 0.0;
        if (S.Orifice[k].type == S.BOTTOM_ORIFICE)
        {
            if (S.Link[j].xsect.type == S.CIRCULAR)
            {
                aOverL = h / 4.0;
            }
            else
            {
                w = S.Link[j].xsect.wMax;
                aOverL = (h * w) / (2.0 * (h + w));
            }
            h = S.Orifice[k].cDisch / 0.414 * aOverL;
            S.Orifice[k].hCrit = h;
        }
        else
        {
            S.Orifice[k].hCrit = h;
            h = h / 2.0;
        }
        return S.Orifice[k].cDisch * Math.Sqrt(h);
    }

    // link.c:1812
    public static double orifice_getInflow(int j)
    {
        int k = 0;
        int n1 = 0;
        int n2 = 0;
        double head = 0.0;
        double h1 = 0.0;
        double h2 = 0.0;
        double y1 = 0.0;
        double dir = 0.0;
        double f = 0.0;
        double hcrest = 0.0;
        double hcrown = 0.0;
        double hmidpt = 0.0;
        double q = 0.0;
        double ratio = 0.0;
        n1 = S.Link[j].node1;
        n2 = S.Link[j].node2;
        k = S.Link[j].subIndex;
        if (S.RouteModel == S.DW)
        {
            h1 = S.Node[n1].newDepth + S.Node[n1].invertElev;
            h2 = S.Node[n2].newDepth + S.Node[n2].invertElev;
        }
        else
        {
            h1 = S.Node[n1].newDepth + S.Node[n1].invertElev;
            h2 = S.Node[n1].invertElev;
        }
        dir = ((h1 >= h2) ? 1.0 : -1.0);
        y1 = S.Node[n1].newDepth;
        if (dir < 0.0)
        {
            head = h1;
            h1 = h2;
            h2 = head;
            y1 = S.Node[n2].newDepth;
        }
        if (S.Orifice[k].type == S.BOTTOM_ORIFICE)
        {
            hcrest = S.Node[n1].invertElev + S.Link[j].offset1;
            if (h1 < hcrest)
            {
                head = 0.0;
            }
            else if (h2 > hcrest)
            {
                head = h1 - h2;
            }
            else
            {
                head = h1 - hcrest;
            }
            f = head / S.Orifice[k].hCrit;
            f = ((((f) <= (1.0)) ? (f) : (1.0)));
        }
        else
        {
            hcrest = S.Node[n1].invertElev + S.Link[j].offset1;
            hcrown = hcrest + S.Link[j].xsect.yFull * S.Link[j].setting;
            hmidpt = (hcrest + hcrown) / 2.0;
            if ((h1 < hcrown) && (hcrown > hcrest))
            {
                f = (h1 - hcrest) / (hcrown - hcrest);
            }
            else
            {
                f = 1.0;
            }
            if (f < 1.0)
            {
                head = h1 - hcrest;
            }
            else if (h2 < hmidpt)
            {
                head = h1 - hmidpt;
            }
            else
            {
                head = h1 - h2;
            }
        }
        if (((head <= 0.0001) || (y1 <= 0.0001)) || ((S.link_setFlapGate(j, n1, n2, dir) != 0)))
        {
            S.Link[j].newDepth = 0.0;
            S.Link[j].flowClass = S.DRY;
            S.Orifice[k].surfArea = 0.0001 * S.Orifice[k].length;
            S.Link[j].dqdh = 0.0;
            return 0.0;
        }
        S.Link[j].flowClass = S.SUBCRITICAL;
        if (hcrest > h2)
        {
            if (dir == 1.0)
            {
                S.Link[j].flowClass = S.DN_CRITICAL;
            }
            else
            {
                S.Link[j].flowClass = S.UP_CRITICAL;
            }
        }
        y1 = S.Link[j].xsect.yFull * S.Link[j].setting;
        if (S.Orifice[k].type == S.SIDE_ORIFICE)
        {
            S.Link[j].newDepth = y1 * f;
            S.Orifice[k].surfArea = S.xsect_getWofY(S.Link[j].xsect, S.Link[j].newDepth) * S.Orifice[k].length;
        }
        else
        {
            S.Link[j].newDepth = y1;
            S.Orifice[k].surfArea = S.xsect_getAofY(S.Link[j].xsect, y1);
        }
        q = dir * S.orifice_getFlow(j, k, head, f, S.Link[j].hasFlapGate);
        if ((f < 1.0) && (h2 > hcrest))
        {
            ratio = (h2 - hcrest) / (h1 - hcrest);
            q *= Math.Pow((1.0 - Math.Pow(ratio, 1.5)), 0.385);
        }
        return q;
    }

    // link.c:1938
    public static double orifice_getFlow(int j, int k, double head, double f, int hasFlapGate)
    {
        double area = 0.0;
        double q = 0.0;
        double veloc = 0.0;
        double hLoss = 0.0;
        if ((head == 0.0) || (f <= 0.0))
        {
            S.Link[j].dqdh = 0.0;
            return 0.0;
        }
        else if (f < 1.0)
        {
            q = S.Orifice[k].cWeir * Math.Pow(f, 1.5);
            S.Link[j].dqdh = 1.5 * q / (f * S.Orifice[k].hCrit);
        }
        else
        {
            q = S.Orifice[k].cOrif * Math.Sqrt(head);
            S.Link[j].dqdh = q / (2.0 * head);
        }
        if ((hasFlapGate != 0))
        {
            area = S.xsect_getAofY(S.Link[j].xsect, S.Link[j].setting * S.Link[j].xsect.yFull);
            veloc = q / area;
            hLoss = (4.0 / 32.2) * veloc * veloc * Math.Exp(-1.15 * veloc / Math.Sqrt(head));
            if (f < 1.0)
            {
                f = f - hLoss / S.Orifice[k].hCrit;
                if (f < 0.0)
                {
                    f = 0.0;
                }
            }
            else
            {
                head = head - hLoss;
                if (head < 0.0)
                {
                    head = 0.0;
                }
            }
            q = S.orifice_getFlow(j, k, head, f, 0);
        }
        return q;
    }

    // link.c:2010
    public static int weir_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int m = 0;
        int n1 = 0;
        int n2 = 0;
        double[] x = new double[10];
        CPtr id = CPtr.Null;
        if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.LINK, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        n1 = S.project_findObject(S.NODE, tok[1]);
        if (n1 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        n2 = S.project_findObject(S.NODE, tok[2]);
        if (n2 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        m = S.findmatch(tok[3], S.WeirTypeWords);
        if (m < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
        }
        x[0] = m;
        if ((S.LinkOffsets == S.ELEV_OFFSET) && (tok[4][0] == '*'))
        {
            x[1] = -10000000000.0;
        }
        else if (!((S.getDouble(tok[4], ref x[1]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[4]);
        }
        if ((!((S.getDouble(tok[5], ref x[2]) != 0))) || (x[2] < 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[5]);
        }
        x[3] = 0.0;
        x[4] = 0.0;
        x[5] = 0.0;
        x[6] = 1.0;
        x[7] = 0.0;
        x[8] = 0.0;
        x[9] = -1.0;
        if ((ntoks >= 7) && (tok[6][0] != '*'))
        {
            m = S.findmatch(tok[6], S.NoYesWords);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[6]);
            }
            x[3] = m;
        }
        if ((ntoks >= 8) && (tok[7][0] != '*'))
        {
            if ((!((S.getDouble(tok[7], ref x[4]) != 0))) || (x[4] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[7]);
            }
        }
        if ((ntoks >= 9) && (tok[8][0] != '*'))
        {
            if ((!((S.getDouble(tok[8], ref x[5]) != 0))) || (x[5] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[8]);
            }
        }
        if ((ntoks >= 10) && (tok[9][0] != '*'))
        {
            m = S.findmatch(tok[9], S.NoYesWords);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[9]);
            }
            x[6] = m;
        }
        if ((m = R.D2I(x[0])) == S.ROADWAY_WEIR)
        {
            if (ntoks >= 11)
            {
                if ((!((S.getDouble(tok[10], ref x[7]) != 0))) || (x[7] < 0.0))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[10]);
                }
            }
            if (ntoks >= 12)
            {
                if ((S.strcomp(tok[11], S.L311) != 0))
                {
                    x[8] = 1.0;
                }
                else if ((S.strcomp(tok[11], S.L312) != 0))
                {
                    x[8] = 2.0;
                }
            }
        }
        if ((ntoks >= 13) && (tok[12][0] != '*'))
        {
            m = S.project_findObject(S.CURVE, tok[12]);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[12]);
            }
            x[9] = m;
        }
        S.Link[j].ID = id;
        S.link_setParams(j, S.WEIR, n1, n2, k, x);
        return 0;
    }

    // link.c:2103
    public static void weir_validate(int j, int k)
    {
        int err = 0;
        double q = 0.0;
        double q1 = 0.0;
        double q2 = 0.0;
        double head = 0.0;
        switch (S.Weir[k].type)
        {
            case S.TRANSVERSE_WEIR:
            case S.SIDEFLOW_WEIR:
            case S.ROADWAY_WEIR:
                if (S.Link[j].xsect.type != S.RECT_OPEN)
                {
                    err = S.ERR_REGULATOR_SHAPE;
                }
                S.Weir[k].slope = 0.0;
                break;
            case S.VNOTCH_WEIR:
                if (S.Link[j].xsect.type != S.TRIANGULAR)
                {
                    err = S.ERR_REGULATOR_SHAPE;
                }
                else
                {
                    S.Weir[k].slope = S.Link[j].xsect.sBot;
                }
                break;
            case S.TRAPEZOIDAL_WEIR:
                if (S.Link[j].xsect.type != S.TRAPEZOIDAL)
                {
                    err = S.ERR_REGULATOR_SHAPE;
                }
                else
                {
                    S.Weir[k].slope = S.Link[j].xsect.sBot;
                }
                break;
        }
        if (err > 0)
        {
            S.report_writeErrorMsg(err, S.Link[j].ID);
            return;
        }
        if (S.Link[j].offset1 < 0.0)
        {
            S.Link[j].offset1 = 0.0;
        }
        S.Weir[k].length = 2.0 * S.RouteStep * Math.Sqrt(32.2 * S.Link[j].xsect.yFull);
        S.Weir[k].length = ((((200.0) >= (S.Weir[k].length)) ? (200.0) : (S.Weir[k].length)));
        S.Weir[k].surfArea = 0.0;
        head = S.Link[j].xsect.yFull;
        S.weir_getFlow(j, k, head, 1.0, 0, ref q1, ref q2);
        q = q1 + q2;
        head = head / 2.0;
        S.Weir[k].cSurcharge = q / Math.Sqrt(head);
    }

    // link.c:2166
    public static void weir_setSetting(int j)
    {
        int k = S.Link[j].subIndex;
        double h = 0.0;
        double q = 0.0;
        double q1 = 0.0;
        double q2 = 0.0;
        S.Link[j].setting = S.Link[j].targetSetting;
        if (!(S.Weir[k].canSurcharge != 0))
        {
            return;
        }
        if (S.Weir[k].type == S.ROADWAY_WEIR)
        {
            return;
        }
        if (S.Link[j].setting == 0.0)
        {
            S.Weir[k].cSurcharge = 0.0;
        }
        else
        {
            h = S.Link[j].setting * S.Link[j].xsect.yFull;
            S.weir_getFlow(j, k, h, 1.0, 0, ref q1, ref q2);
            q = q1 + q2;
            h = h / 2.0;
            S.Weir[k].cSurcharge = q / Math.Sqrt(h);
        }
    }

    // link.c:2198
    public static double weir_getInflow(int j)
    {
        int n1 = 0;
        int n2 = 0;
        int k = 0;
        double q1 = 0.0;
        double q2 = 0.0;
        double head = 0.0;
        double h1 = 0.0;
        double h2 = 0.0;
        double hcrest = 0.0;
        double hcrown = 0.0;
        double y = 0.0;
        double dir = 0.0;
        double ratio = 0.0;
        double[] weirPower = new double[] { 1.5, 5.0 / 3.0, 2.5, 1.5 };
        n1 = S.Link[j].node1;
        n2 = S.Link[j].node2;
        k = S.Link[j].subIndex;
        if (S.RouteModel == S.DW)
        {
            h1 = S.Node[n1].newDepth + S.Node[n1].invertElev;
            h2 = S.Node[n2].newDepth + S.Node[n2].invertElev;
        }
        else
        {
            h1 = S.Node[n1].newDepth + S.Node[n1].invertElev;
            h2 = S.Node[n1].invertElev;
        }
        dir = ((h1 > h2) ? 1.0 : -1.0);
        if (dir < 0.0)
        {
            head = h1;
            h1 = h2;
            h2 = head;
        }
        hcrest = S.Node[n1].invertElev + S.Link[j].offset1;
        hcrown = hcrest + S.Link[j].xsect.yFull;
        if (S.Weir[k].type == S.ROADWAY_WEIR)
        {
            return S.roadway_getInflow(j, dir, hcrest, h1, h2);
        }
        hcrest += (1.0 - S.Link[j].setting) * S.Link[j].xsect.yFull;
        head = h1 - hcrest;
        S.Link[j].dqdh = 0.0;
        if (((head <= 0.0001) || (hcrest >= hcrown)) || ((S.link_setFlapGate(j, n1, n2, dir) != 0)))
        {
            S.Link[j].newDepth = 0.0;
            S.Link[j].flowClass = S.DRY;
            return 0.0;
        }
        S.Link[j].flowClass = S.SUBCRITICAL;
        if (hcrest > h2)
        {
            if (dir == 1.0)
            {
                S.Link[j].flowClass = S.DN_CRITICAL;
            }
            else
            {
                S.Link[j].flowClass = S.UP_CRITICAL;
            }
        }
        y = S.Link[j].xsect.yFull - (hcrown - ((((h1) <= (hcrown)) ? (h1) : (hcrown))));
        S.Weir[k].surfArea = S.xsect_getWofY(S.Link[j].xsect, y) * S.Weir[k].length;
        if (h1 >= hcrown)
        {
            if ((S.Weir[k].canSurcharge != 0))
            {
                y = (hcrest + hcrown) / 2.0;
                if (h2 < y)
                {
                    head = h1 - y;
                }
                else
                {
                    head = h1 - h2;
                }
                y = hcrown - hcrest;
                q1 = S.weir_getOrificeFlow(j, head, y, S.Weir[k].cSurcharge);
                S.Link[j].newDepth = y;
                return dir * q1;
            }
            else
            {
                head = hcrown - hcrest;
            }
        }
        S.weir_getFlow(j, k, head, dir, S.Link[j].hasFlapGate, ref q1, ref q2);
        if (h2 > hcrest)
        {
            ratio = (h2 - hcrest) / (h1 - hcrest);
            q1 *= Math.Pow((1.0 - Math.Pow(ratio, weirPower[S.Weir[k].type])), 0.385);
            if (q2 > 0.0)
            {
                q2 *= Math.Pow((1.0 - Math.Pow(ratio, weirPower[S.VNOTCH_WEIR])), 0.385);
            }
        }
        S.Link[j].newDepth = (((((h1 - hcrest)) <= (S.Link[j].xsect.yFull)) ? ((h1 - hcrest)) : (S.Link[j].xsect.yFull)));
        return dir * (q1 + q2);
    }

    // link.c:2321
    public static void weir_getFlow(int j, int k, double head, double dir, int hasFlapGate, ref double q1, ref double q2)
    {
        double length = 0.0;
        double h = 0.0;
        double y = 0.0;
        double hLoss = 0.0;
        double area = 0.0;
        double veloc = 0.0;
        int wType = 0;
        int cdCurve = S.Weir[k].cdCurve;
        double cDisch1 = S.Weir[k].cDisch1;
        q1 = 0.0;
        q2 = 0.0;
        S.Link[j].dqdh = 0.0;
        if (head <= 0.0)
        {
            return;
        }
        length = S.Link[j].xsect.wMax * S.UCF(S.LENGTH);
        h = head * S.UCF(S.LENGTH);
        if (cdCurve >= 0)
        {
            cDisch1 = S.table_lookup(S.Curve[cdCurve], h);
        }
        wType = S.Weir[k].type;
        if ((wType == S.VNOTCH_WEIR) && (S.Link[j].setting < 1.0))
        {
            wType = S.TRAPEZOIDAL_WEIR;
        }
        switch (wType)
        {
            case S.TRANSVERSE_WEIR:
                length -= 0.1 * S.Weir[k].endCon * h;
                length = ((((length) >= (0.0)) ? (length) : (0.0)));
                q1 = cDisch1 * length * Math.Pow(h, 1.5);
                break;
            case S.SIDEFLOW_WEIR:
                length -= 0.1 * S.Weir[k].endCon * h;
                length = ((((length) >= (0.0)) ? (length) : (0.0)));
                if (dir < 0.0)
                {
                    q1 = cDisch1 * length * Math.Pow(h, 1.5);
                }
                else
                {
                    q1 = cDisch1 * Math.Pow(length, 0.83) * Math.Pow(h, 1.67);
                }
                break;
            case S.VNOTCH_WEIR:
                q1 = cDisch1 * S.Weir[k].slope * Math.Pow(h, 2.5);
                break;
            case S.TRAPEZOIDAL_WEIR:
                y = (1.0 - S.Link[j].setting) * S.Link[j].xsect.yFull;
                length = S.xsect_getWofY(S.Link[j].xsect, y) * S.UCF(S.LENGTH);
                q1 = cDisch1 * length * Math.Pow(h, 1.5);
                q2 = S.Weir[k].cDisch2 * S.Weir[k].slope * Math.Pow(h, 2.5);
                break;
        }
        if (S.UnitSystem == S.SI)
        {
            q1 /= 0.028317;
            q2 /= 0.028317;
        }
        if ((hasFlapGate != 0))
        {
            area = S.weir_getOpenArea(j, head);
            if (area > 1e-06)
            {
                veloc = (q1 + q2) / area;
                hLoss = (4.0 / 32.2) * veloc * veloc * Math.Exp(-1.15 * veloc / Math.Sqrt(head));
                head = head - hLoss;
                if (head < 0.0)
                {
                    head = 0.0;
                }
                S.weir_getFlow(j, k, head, dir, 0, ref q1, ref q2);
            }
        }
        S.Link[j].dqdh = S.weir_getdqdh(k, dir, head, q1, q2);
    }

    // link.c:2432
    public static double weir_getOrificeFlow(int j, double head, double y, double cOrif)
    {
        double a = 0.0;
        double q = 0.0;
        double v = 0.0;
        double hloss = 0.0;
        q = cOrif * Math.Sqrt(head);
        if ((S.Link[j].hasFlapGate != 0))
        {
            a = S.weir_getOpenArea(j, y);
            if (a > 0.0)
            {
                v = q / a;
                hloss = (4.0 / 32.2) * v * v * Math.Exp(-1.15 * v / Math.Sqrt(y));
                head -= hloss;
                head = ((((head) >= (0.0)) ? (head) : (0.0)));
                q = cOrif * Math.Sqrt(head);
            }
        }
        if (head > 0.0)
        {
            S.Link[j].dqdh = q / (2.0 * head);
        }
        else
        {
            S.Link[j].dqdh = 0.0;
        }
        return q;
    }

    // link.c:2467
    public static double weir_getOpenArea(int j, double y)
    {
        double z = 0.0;
        double zy = 0.0;
        z = (1.0 - S.Link[j].setting) * S.Link[j].xsect.yFull;
        zy = z + y;
        zy = ((((zy) <= (S.Link[j].xsect.yFull)) ? (zy) : (S.Link[j].xsect.yFull)));
        return S.xsect_getAofY(S.Link[j].xsect, zy) - S.xsect_getAofY(S.Link[j].xsect, z);
    }

    // link.c:2492
    public static double weir_getdqdh(int k, double dir, double h, double q1, double q2)
    {
        double q1h = 0.0;
        double q2h = 0.0;
        if (Math.Abs(h) < 0.0001)
        {
            return 0.0;
        }
        q1h = Math.Abs(q1 / h);
        q2h = Math.Abs(q2 / h);
        switch (S.Weir[k].type)
        {
            case S.TRANSVERSE_WEIR:
                return 1.5 * q1h;
            case S.SIDEFLOW_WEIR:
                if (dir < 0.0)
                {
                    return 1.5 * q1h;
                }
                else
                {
                    return 1.67 * q1h;
                }
            case S.VNOTCH_WEIR:
                if (q2h == 0.0)
                {
                    return 2.5 * q1h;
                }
                else
                {
                    return 1.5 * q1h + 2.5 * q2h;
                }
            case S.TRAPEZOIDAL_WEIR:
                return 1.5 * q1h + 2.5 * q2h;
        }
        return 0.0;
    }

    // link.c:2524
    public static int outlet_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int m = 0;
        int n = 0;
        int n1 = 0;
        int n2 = 0;
        double[] x = new double[6];
        CPtr id = CPtr.Null;
        CPtr s = CPtr.Null;
        if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.LINK, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        n1 = S.project_findObject(S.NODE, tok[1]);
        if (n1 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        n2 = S.project_findObject(S.NODE, tok[2]);
        if (n2 < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        if ((S.LinkOffsets == S.ELEV_OFFSET) && (tok[3][0] == '*'))
        {
            x[0] = -10000000000.0;
        }
        else
        {
            if (!((S.getDouble(tok[3], ref x[0]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[3]);
            }
            if ((S.LinkOffsets == S.DEPTH_OFFSET) && (x[0] < 0.0))
            {
                x[0] = 0.0;
            }
        }
        m = S.findmatch(tok[4], S.RelationWords);
        if (m < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[4]);
        }
        x[1] = 0.0;
        x[2] = 0.0;
        x[3] = -1.0;
        x[4] = 0.0;
        x[5] = S.NODE_DEPTH;
        s = R.strtok(tok[4], S.L313);
        s = R.strtok(CPtr.Null, S.L313);
        if ((S.strcomp(s, S.L314) != 0))
        {
            x[5] = S.NODE_HEAD;
        }
        if (m == S.FUNCTIONAL)
        {
            if (ntoks < 7)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (!((S.getDouble(tok[5], ref x[1]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[5]);
            }
            if (!((S.getDouble(tok[6], ref x[2]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[6]);
            }
            n = 7;
        }
        else
        {
            i = S.project_findObject(S.CURVE, tok[5]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[5]);
            }
            x[3] = i;
            n = 6;
        }
        if (ntoks > n)
        {
            i = S.findmatch(tok[n], S.NoYesWords);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
            }
            x[4] = i;
        }
        S.Link[j].ID = id;
        S.link_setParams(j, S.OUTLET, n1, n2, k, x);
        return 0;
    }

    // link.c:2608
    public static double outlet_getInflow(int j)
    {
        int k = 0;
        int n1 = 0;
        int n2 = 0;
        double head = 0.0;
        double hcrest = 0.0;
        double h1 = 0.0;
        double h2 = 0.0;
        double y1 = 0.0;
        double dir = 0.0;
        n1 = S.Link[j].node1;
        n2 = S.Link[j].node2;
        k = S.Link[j].subIndex;
        if (S.RouteModel == S.DW)
        {
            h1 = S.Node[n1].newDepth + S.Node[n1].invertElev;
            h2 = S.Node[n2].newDepth + S.Node[n2].invertElev;
        }
        else
        {
            h1 = S.Node[n1].newDepth + S.Node[n1].invertElev;
            h2 = S.Node[n1].invertElev;
        }
        dir = ((h1 >= h2) ? 1.0 : -1.0);
        y1 = S.Node[n1].newDepth;
        if (dir < 0.0)
        {
            y1 = h1;
            h1 = h2;
            h2 = y1;
            y1 = S.Node[n2].newDepth;
        }
        hcrest = S.Node[n1].invertElev + S.Link[j].offset1;
        if ((S.Outlet[k].curveType == S.NODE_HEAD) && (S.RouteModel == S.DW))
        {
            head = h1 - ((((h2) >= (hcrest)) ? (h2) : (hcrest)));
        }
        else
        {
            head = h1 - hcrest;
        }
        if (((head <= 0.0001) || (y1 <= 0.0001)) || ((S.link_setFlapGate(j, n1, n2, dir) != 0)))
        {
            S.Link[j].newDepth = 0.0;
            S.Link[j].flowClass = S.DRY;
            return 0.0;
        }
        S.Link[j].newDepth = head;
        S.Link[j].flowClass = S.SUBCRITICAL;
        return dir * S.Link[j].setting * S.outlet_getFlow(k, head);
    }

    // link.c:2672
    public static double outlet_getFlow(int k, double head)
    {
        int m = 0;
        double h = 0.0;
        h = head * S.UCF(S.LENGTH);
        m = S.Outlet[k].qCurve;
        if (m >= 0)
        {
            return S.table_lookup(S.Curve[m], h) / S.UCF(S.FLOW);
        }
        else
        {
            return S.Outlet[k].qCoeff * Math.Pow(h, S.Outlet[k].qExpon) / S.UCF(S.FLOW);
        }
    }

}
}
