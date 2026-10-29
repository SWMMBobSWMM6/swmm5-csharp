// Generated from EPA SWMM 5.2.4 (node.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // node.c:102
    public static int node_readParams(int j, int type, int k, CPtr[] tok, int ntoks)
    {
        switch (type)
        {
            case S.JUNCTION:
                return S.junc_readParams(j, k, tok, ntoks);
            case S.OUTFALL:
                return S.outfall_readParams(j, k, tok, ntoks);
            case S.STORAGE:
                return S.storage_readParams(j, k, tok, ntoks);
            case S.DIVIDER:
                return S.divider_readParams(j, k, tok, ntoks);
            default:
                return 0;
        }
    }

    // node.c:125
    public static void node_setParams(int j, int type, int k, double[] x)
    {
        S.Node[j].type = type;
        S.Node[j].subIndex = k;
        S.Node[j].invertElev = x[0] / S.UCF(S.LENGTH);
        S.Node[j].crownElev = S.Node[j].invertElev;
        S.Node[j].initDepth = 0.0;
        S.Node[j].newVolume = 0.0;
        S.Node[j].fullVolume = 0.0;
        S.Node[j].fullDepth = 0.0;
        S.Node[j].surDepth = 0.0;
        S.Node[j].pondedArea = 0.0;
        S.Node[j].degree = 0;
        S.Node[j].inlet = S.NO_INLET;
        switch (type)
        {
            case S.JUNCTION:
                S.Node[j].fullDepth = x[1] / S.UCF(S.LENGTH);
                S.Node[j].initDepth = x[2] / S.UCF(S.LENGTH);
                S.Node[j].surDepth = x[3] / S.UCF(S.LENGTH);
                S.Node[j].pondedArea = x[4] / (S.UCF(S.LENGTH) * S.UCF(S.LENGTH));
                break;
            case S.OUTFALL:
                S.Outfall[k].type = R.D2I(x[1]);
                S.Outfall[k].fixedStage = x[2] / S.UCF(S.LENGTH);
                S.Outfall[k].tideCurve = R.D2I(x[3]);
                S.Outfall[k].stageSeries = R.D2I(x[4]);
                S.Outfall[k].hasFlapGate = R.D2I(x[5]);
                S.Outfall[k].routeTo = R.D2I(x[6]);
                S.Outfall[k].wRouted = (null);
                if (S.Outfall[k].routeTo >= 0)
                {
                    S.Outfall[k].wRouted = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
                }
                break;
            case S.STORAGE:
                S.Node[j].fullDepth = x[1] / S.UCF(S.LENGTH);
                S.Node[j].initDepth = x[2] / S.UCF(S.LENGTH);
                S.Storage[k].shape = R.D2I(x[3]);
                S.Storage[k].a1 = x[4];
                S.Storage[k].a2 = x[5];
                S.Storage[k].a0 = x[6];
                S.Storage[k].aCurve = R.D2I(x[7]);
                S.Node[j].surDepth = x[8] / S.UCF(S.LENGTH);
                S.Storage[k].fEvap = x[9];
                break;
            case S.DIVIDER:
                S.Divider[k].link = R.D2I(x[1]);
                S.Divider[k].type = R.D2I(x[2]);
                S.Divider[k].flowCurve = R.D2I(x[3]);
                S.Divider[k].qMin = x[4] / S.UCF(S.FLOW);
                S.Divider[k].dhMax = x[5];
                S.Divider[k].cWeir = x[6];
                S.Node[j].fullDepth = x[7] / S.UCF(S.LENGTH);
                S.Node[j].initDepth = x[8] / S.UCF(S.LENGTH);
                S.Node[j].surDepth = x[9] / S.UCF(S.LENGTH);
                S.Node[j].pondedArea = x[10] / (S.UCF(S.LENGTH) * S.UCF(S.LENGTH));
                break;
        }
    }

    // node.c:200
    public static void node_validate(int j)
    {
        TDwfInflow inflow = null;
        if ((S.Node[j].fullDepth > S.Node[j].oldDepth) && (S.Node[j].oldDepth > 0.0))
        {
            S.report_writeWarningMsg(S.L317, S.Node[j].ID);
        }
        if (S.Node[j].initDepth > S.Node[j].fullDepth + S.Node[j].surDepth)
        {
            S.report_writeErrorMsg(S.ERR_NODE_DEPTH, S.Node[j].ID);
        }
        if (S.Node[j].type == S.STORAGE)
        {
            if (S.node_getVolume(j, S.Node[j].fullDepth) < 0.0)
            {
                S.report_writeErrorMsg(S.ERR_STORAGE_VOLUME, S.Node[j].ID);
            }
        }
        if (S.Node[j].type == S.DIVIDER)
        {
            S.divider_validate(j);
        }
        inflow = S.Node[j].dwfInflow;
        while ((inflow != null))
        {
            S.inflow_initDwfInflow(inflow);
            inflow = inflow.next;
        }
    }

    // node.c:237
    public static void node_initState(int j)
    {
        int p = 0;
        int k = 0;
        S.Node[j].oldDepth = S.Node[j].initDepth;
        S.Node[j].newDepth = S.Node[j].oldDepth;
        S.Node[j].crownElev = S.Node[j].invertElev;
        S.Node[j].fullVolume = S.node_getVolume(j, S.Node[j].fullDepth);
        S.Node[j].oldVolume = S.node_getVolume(j, S.Node[j].oldDepth);
        S.Node[j].newVolume = S.Node[j].oldVolume;
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.Node[j].oldQual[p] = 0.0;
            S.Node[j].newQual[p] = 0.0;
        }
        S.Node[j].oldLatFlow = 0.0;
        S.Node[j].newLatFlow = 0.0;
        S.Node[j].apiExtInflow = 0.0;
        S.Node[j].losses = 0.0;
        if (S.Node[j].type == S.STORAGE)
        {
            k = S.Node[j].subIndex;
            S.Storage[k].hrt = 0.0;
            if ((S.Storage[k].exfil != null))
            {
                S.exfil_initState(k);
            }
        }
        if (S.Node[j].type == S.OUTFALL)
        {
            k = S.Node[j].subIndex;
            if (S.Outfall[k].routeTo >= 0)
            {
                S.Outfall[k].vRouted = 0.0;
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    S.Outfall[k].wRouted[p] = 0.0;
                }
            }
        }
    }

    // node.c:293
    public static void node_setOldHydState(int j)
    {
        S.Node[j].oldDepth = S.Node[j].newDepth;
        S.Node[j].oldVolume = S.Node[j].newVolume;
        S.Node[j].oldFlowInflow = S.Node[j].inflow;
        S.Node[j].oldNetInflow = S.Node[j].inflow - S.Node[j].outflow;
    }

    // node.c:308
    public static void node_setOldQualState(int j)
    {
        int p = 0;
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.Node[j].oldQual[p] = S.Node[j].newQual[p];
            S.Node[j].newQual[p] = 0.0;
        }
    }

    // node.c:325
    public static void node_initFlows(int j, double tStep)
    {
        S.Node[j].inflow = S.Node[j].newLatFlow;
        S.Node[j].outflow = S.Node[j].losses;
        if (S.Node[j].newVolume > S.Node[j].fullVolume)
        {
            S.Node[j].overflow = (S.Node[j].newVolume - S.Node[j].fullVolume) / tStep;
        }
        else
        {
            S.Node[j].overflow = 0.0;
        }
    }

    // node.c:345
    public static double node_getDepth(int j, double v)
    {
        switch (S.Node[j].type)
        {
            case S.STORAGE:
                return S.storage_getDepth(j, v);
            default:
                return 0.0;
        }
    }

    // node.c:362
    public static double node_getVolume(int j, double d)
    {
        switch (S.Node[j].type)
        {
            case S.STORAGE:
                return S.storage_getVolume(j, d);
            default:
                if (S.Node[j].fullDepth > 0.0)
                {
                    return S.Node[j].fullVolume * (d / S.Node[j].fullDepth);
                }
                else
                {
                    return 0.0;
                }
        }
    }

    // node.c:383
    public static double node_getSurfArea(int j, double d)
    {
        switch (S.Node[j].type)
        {
            case S.STORAGE:
                return S.storage_getSurfArea(j, d);
            default:
                return 0.0;
        }
    }

    // node.c:400
    public static double node_getOutflow(int j, int k)
    {
        switch (S.Node[j].type)
        {
            case S.DIVIDER:
                return S.divider_getOutflow(j, k);
            case S.STORAGE:
                return S.storage_getOutflow(j, k);
            default:
                return S.Node[j].inflow + S.Node[j].overflow;
        }
    }

    // node.c:418
    public static double node_getMaxOutflow(int j, double q, double tStep)
    {
        double qMax = 0.0;
        if (S.Node[j].fullVolume > 0.0)
        {
            qMax = S.Node[j].inflow + S.Node[j].oldVolume / tStep;
            if (q > qMax)
            {
                q = qMax;
            }
        }
        return ((((0.0) >= (q)) ? (0.0) : (q)));
    }

    // node.c:438
    public static double node_getSystemOutflow(int j, ref int isFlooded)
    {
        double outflow = 0.0;
        ;
        isFlooded = 0;
        if (S.Node[j].type == S.OUTFALL)
        {
            if (S.Node[j].outflow == 0.0)
            {
                outflow = S.Node[j].inflow;
            }
            else
            {
                if (S.Node[j].inflow == 0.0)
                {
                    outflow = -S.Node[j].outflow;
                    S.Node[j].inflow = Math.Abs(outflow);
                }
            }
            S.Node[j].overflow = 0.0;
            S.Node[j].newVolume = 0.0;
        }
        else if (((S.RouteModel != S.DW) && (S.Node[j].degree == 0)) && (S.Node[j].type != S.STORAGE))
        {
            if (S.Node[j].outflow == 0.0)
            {
                outflow = S.Node[j].inflow;
            }
            S.Node[j].overflow = 0.0;
            S.Node[j].newVolume = 0.0;
        }
        else
        {
            if (S.Node[j].newVolume <= S.Node[j].fullVolume)
            {
                outflow = S.Node[j].overflow;
            }
            if (outflow > 0.0)
            {
                isFlooded = 1;
            }
        }
        return outflow;
    }

    // node.c:497
    public static void node_getResults(int j, double f, float[] x)
    {
        int p = 0;
        double z = 0.0;
        double f1 = 1.0 - f;
        z = (f1 * S.Node[j].oldDepth + f * S.Node[j].newDepth) * S.UCF(S.LENGTH);
        x[S.NODE_DEPTH] = (float)(z);
        z = S.Node[j].invertElev * S.UCF(S.LENGTH);
        x[S.NODE_HEAD] = x[S.NODE_DEPTH] + (float)(z);
        z = (f1 * S.Node[j].oldVolume + f * S.Node[j].newVolume) * S.UCF(S.VOLUME);
        x[S.NODE_VOLUME] = (float)(z);
        z = (f1 * S.Node[j].oldLatFlow + f * S.Node[j].newLatFlow) * S.UCF(S.FLOW);
        x[S.NODE_LATFLOW] = (float)(z);
        z = (f1 * S.Node[j].oldFlowInflow + f * S.Node[j].inflow) * S.UCF(S.FLOW);
        x[S.NODE_INFLOW] = (float)(z);
        z = S.Node[j].overflow * S.UCF(S.FLOW);
        x[S.NODE_OVERFLOW] = (float)(z);
        if (!(S.IgnoreQuality != 0))
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                z = f1 * S.Node[j].oldQual[p] + f * S.Node[j].newQual[p];
                x[S.NODE_QUAL + p] = (float)(z);
            }
        }
    }

    // node.c:532
    public static void node_setOutletDepth(int j, double yNorm, double yCrit, double z)
    {
        switch (S.Node[j].type)
        {
            case S.STORAGE:
                return;
            case S.OUTFALL:
                S.outfall_setOutletDepth(j, yNorm, yCrit, z);
                break;
            default:
                if (z > 0.0)
                {
                    S.Node[j].newDepth = 0.0;
                }
                else
                {
                    S.Node[j].newDepth = ((((yNorm) <= (yCrit)) ? (yNorm) : (yCrit)));
                }
                break;
        }
    }

    // node.c:562
    public static double node_getPondedArea(int j, double d)
    {
        double a = 0.0;
        if ((d <= S.Node[j].fullDepth) || (S.Node[j].pondedArea == 0.0))
        {
            return S.node_getSurfArea(j, d);
        }
        d = d - S.Node[j].fullDepth;
        a = S.Node[j].pondedArea;
        if (a <= 0.0)
        {
            a = S.node_getSurfArea(j, S.Node[j].fullDepth);
        }
        return a;
    }

    // node.c:589
    public static double node_getLosses(int j, double tStep)
    {
        if (S.Node[j].type == S.STORAGE)
        {
            return S.storage_getLosses(j, tStep);
        }
        else
        {
            return 0.0;
        }
    }

    // node.c:606
    public static int junc_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int i = 0;
        double[] x = new double[6];
        CPtr id = CPtr.Null;
        if (ntoks < 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.NODE, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        for (i = 1; i <= 5; i++)
        {
            x[i - 1] = 0.0;
            if (i < ntoks)
            {
                if (!((S.getDouble(tok[i], ref x[i - 1]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                }
            }
        }
        for (i = 1; i <= 4; i++)
        {
            if (x[i] < 0.0)
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i + 1]);
            }
        }
        S.Node[j].ID = id;
        S.node_setParams(j, S.JUNCTION, k, x);
        return 0;
    }

    // node.c:654
    public static int storage_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int m = 0;
        int n = 0;
        double[] x = new double[10];
        double[] y = new double[3];
        double A = 0.0;
        double B = 0.0;
        double L = 0.0;
        double W = 0.0;
        double Z = 0.0;
        CPtr id = CPtr.Null;
        if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.NODE, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        for (i = 1; i <= 3; i++)
        {
            if (!((S.getDouble(tok[i], ref x[i - 1]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i]);
            }
        }
        m = S.findmatch(tok[4], S.RelationWords);
        if (m < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[4]);
        }
        x[3] = m;
        x[4] = 0.0;
        x[5] = 0.0;
        x[6] = 0.0;
        x[7] = -1.0;
        x[8] = 0.0;
        x[9] = 0.0;
        if (m == S.TABULAR)
        {
            i = S.project_findObject(S.CURVE, tok[5]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[5]);
            }
            x[7] = i;
            n = 6;
        }
        else
        {
            if (ntoks < 8)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            for (i = 5; i <= 7; i++)
            {
                if (!((S.getDouble(tok[i], ref y[i - 5]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                }
            }
            n = 8;
        }
        switch (m)
        {
            case S.FUNCTIONAL:
                if (y[2] < 0.0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[7]);
                }
                break;
            case S.CYLINDRICAL:
            case S.CONICAL:
            case S.PARABOLOID:
            case S.PYRAMIDAL:
                if (y[0] <= 0.0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[5]);
                }
                if (y[1] <= 0.0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[6]);
                }
                if (y[2] < 0.0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[7]);
                }
                break;
        }
        if ((m == S.PARABOLOID) && (y[2] == 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[7]);
        }
        switch (m)
        {
            case S.FUNCTIONAL:
                x[4] = y[0];
                x[5] = y[1];
                x[6] = y[2];
                break;
            case S.CYLINDRICAL:
                A = y[0] / 2.0;
                B = y[1] / 2.0;
                x[4] = 0.0;
                x[5] = 0.0;
                x[6] = 3.141592654 * A * B;
                break;
            case S.CONICAL:
                A = y[0] / 2.0;
                B = y[1] / 2.0;
                Z = y[2];
                x[4] = 2.0 * 3.141592654 * B * Z;
                x[5] = 3.141592654 * B / A * Z * Z;
                x[6] = 3.141592654 * A * B;
                break;
            case S.PARABOLOID:
                A = y[0] / 2.0;
                B = y[1] / 2.0;
                Z = y[2];
                x[4] = 3.141592654 * A * B / Z;
                x[5] = 0.0;
                x[6] = 0.0;
                break;
            case S.PYRAMIDAL:
                L = y[0];
                W = y[1];
                Z = y[2];
                x[4] = 2.0 * (L + W) * Z;
                x[5] = 4.0 * Z * Z;
                x[6] = L * W;
                break;
        }
        if (ntoks > n)
        {
            if (!((S.getDouble(tok[n], ref x[8]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[n]);
            }
            n++;
        }
        if (ntoks > n)
        {
            if (!((S.getDouble(tok[n], ref x[9]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[n]);
            }
            n++;
        }
        S.Node[j].ID = id;
        S.node_setParams(j, S.STORAGE, k, x);
        if (ntoks > n)
        {
            return S.exfil_readStorageParams(k, tok, ntoks, n);
        }
        return 0;
    }

    // node.c:813
    public static double storage_getDepth(int j, double v)
    {
        int k = S.Node[j].subIndex;
        int i = S.Storage[k].aCurve;
        int shape = S.Storage[k].shape;
        double a0 = S.Storage[k].a0;
        double a1 = S.Storage[k].a1;
        double a2 = S.Storage[k].a2;
        double d = 0.0;
        double e = 0.0;
        TStorageVol storageVol = new TStorageVol();
        if ((S.Node[j].fullVolume > 0.0) && (v >= S.Node[j].fullVolume))
        {
            return S.Node[j].fullDepth;
        }
        if (v == 0.0)
        {
            return 0.0;
        }
        v *= S.UCF(S.VOLUME);
        storageVol.k = k;
        storageVol.v = v;
        switch (shape)
        {
            case S.TABULAR:
                i = S.Storage[k].aCurve;
                if (i >= 0)
                {
                    d = S.table_getStorageDepth(S.Curve[i], v);
                }
                else
                {
                    d = 0.0;
                }
                break;
            case S.CYLINDRICAL:
                d = v / a0;
                break;
            case S.PARABOLOID:
                d = Math.Sqrt(2.0 * v / a1);
                break;
            case S.FUNCTIONAL:
                if (a2 == 0.0)
                {
                    d = v / (a0 + a1);
                }
                else if (a0 == 0.0)
                {
                    e = 1.0 / (a2 + 1.0);
                    d = Math.Pow(v / (a1 * e), e);
                }
                else if ((a2 == 1.0) && (a1 > 0.0))
                {
                    d = (Math.Sqrt(a0 * a0 + 2.0 * a1 * v) - a0) / a1;
                }
                else
                {
                    d = v / (a0 + a1);
                    S.findroot_Newton(0.0, S.Node[j].fullDepth * S.UCF(S.LENGTH), ref d, 0.001, S.storage_getVolDiff, storageVol);
                }
                break;
            case S.CONICAL:
            case S.PYRAMIDAL:
                d = v / a0;
                S.findroot_Newton(0.0, S.Node[j].fullDepth * S.UCF(S.LENGTH), ref d, 0.001, S.storage_getVolDiff, storageVol);
                break;
            default:
                d = 0.0;
                break;
        }
        d /= S.UCF(S.LENGTH);
        if (d > S.Node[j].fullDepth)
        {
            d = S.Node[j].fullDepth;
        }
        return d;
    }

    // node.c:906
    public static void storage_getVolDiff(double y, ref double f, ref double df, object p)
    {
        int k = 0;
        TStorageVol storageVol = null;
        storageVol = ((TStorageVol)p);
        k = storageVol.k;
        f = S.storage_getVolume(k, y) - storageVol.v;
        df = S.storage_getSurfArea(k, y);
    }

    // node.c:930
    public static double storage_getVolume(int j, double d)
    {
        int k = S.Node[j].subIndex;
        int i = 0;
        double n = 0.0;
        double v = 0.0;
        if (d == 0.0)
        {
            return 0.0;
        }
        if ((d >= S.Node[j].fullDepth) && (S.Node[j].fullVolume > 0.0))
        {
            return S.Node[j].fullVolume;
        }
        switch (S.Storage[k].shape)
        {
            case S.TABULAR:
                i = S.Storage[k].aCurve;
                if (i >= 0)
                {
                    return S.table_getStorageVolume(S.Curve[i], d * S.UCF(S.LENGTH)) / S.UCF(S.VOLUME);
                }
                else
                {
                    return 0.0;
                }
            case S.FUNCTIONAL:
                d *= S.UCF(S.LENGTH);
                n = S.Storage[k].a2 + 1.0;
                v = (S.Storage[k].a0 * d) + S.Storage[k].a1 / n * Math.Pow(d, n);
                return v / S.UCF(S.VOLUME);
            case S.CYLINDRICAL:
            case S.CONICAL:
            case S.PARABOLOID:
            case S.PYRAMIDAL:
                d *= S.UCF(S.LENGTH);
                v = d * (S.Storage[k].a0 + d * (S.Storage[k].a1 / 2.0 + d * S.Storage[k].a2 / 3.0));
                return v / S.UCF(S.VOLUME);
            default:
                return 0.0;
        }
    }

    // node.c:979
    public static double storage_getSurfArea(int j, double d)
    {
        double area = 0.0;
        int k = S.Node[j].subIndex;
        int i = 0;
        switch (S.Storage[k].shape)
        {
            case S.TABULAR:
                i = S.Storage[k].aCurve;
                if (i >= 0)
                {
                    area = S.table_lookupEx(S.Curve[i], d * S.UCF(S.LENGTH));
                }
                break;
            case S.FUNCTIONAL:
                area = S.Storage[k].a0 + S.Storage[k].a1 * Math.Pow(d * S.UCF(S.LENGTH), S.Storage[k].a2);
                break;
            case S.CYLINDRICAL:
            case S.CONICAL:
            case S.PARABOLOID:
            case S.PYRAMIDAL:
                d *= S.UCF(S.LENGTH);
                area = S.Storage[k].a0 + d * (S.Storage[k].a1 + d * S.Storage[k].a2);
                break;
            default:
                return 0.0;
        }
        return area / S.UCF(S.LENGTH) / S.UCF(S.LENGTH);
    }

    // node.c:1022
    public static double storage_getOutflow(int j, int i)
    {
        int k = 0;
        double a = 0.0;
        double y = 0.0;
        if (S.Link[i].type != S.CONDUIT)
        {
            return 0.0;
        }
        y = S.Node[j].newDepth - S.Link[i].offset1;
        if (y <= 0.0)
        {
            return 0.0;
        }
        if (y >= S.Link[i].xsect.yFull)
        {
            return S.Link[i].qFull;
        }
        k = S.Link[i].subIndex;
        a = S.xsect_getAofY(S.Link[i].xsect, y);
        return S.Conduit[k].beta * S.xsect_getSofA(S.Link[i].xsect, a);
    }

    // node.c:1052
    public static double storage_getLosses(int j, double tStep)
    {
        int k = 0;
        double depth = 0.0;
        double area = 0.0;
        double evapRate = 0.0;
        double exfilRate = 0.0;
        double totalLoss = 0.0;
        double lossRatio = 0.0;
        TExfil exfil = null;
        k = S.Node[j].subIndex;
        evapRate = S.Evap.rate * S.Storage[k].fEvap;
        exfil = S.Storage[k].exfil;
        if ((evapRate > 0.0) || (exfil != null))
        {
            depth = S.Node[j].newDepth;
            area = S.storage_getSurfArea(j, depth);
            if (S.Node[j].newVolume > 0.0001)
            {
                evapRate = area * evapRate;
            }
            if (exfil != null)
            {
                exfilRate = S.exfil_getLoss(exfil, tStep, depth, area);
            }
            totalLoss = (evapRate + exfilRate) * tStep;
            if (totalLoss > S.Node[j].newVolume)
            {
                lossRatio = S.Node[j].newVolume / totalLoss;
                evapRate *= lossRatio;
                exfilRate *= lossRatio;
            }
        }
        S.Storage[S.Node[j].subIndex].evapLoss = evapRate * tStep;
        S.Storage[S.Node[j].subIndex].exfilLoss = exfilRate * tStep;
        return evapRate + exfilRate;
    }

    // node.c:1113
    public static int divider_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int m = 0;
        int m1 = 0;
        int m2 = 0;
        int n = 0;
        double[] x = new double[11];
        CPtr id = CPtr.Null;
        if (ntoks < 4)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.NODE, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        if (!((S.getDouble(tok[1], ref x[0]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[1]);
        }
        for (i = 1; i < 11; i++)
        {
            x[i] = 0.0;
        }
        if ((R.strlen(tok[2]) == unchecked((ulong)(0))) || (R.strcmp(tok[2], S.L275) == 0))
        {
            x[1] = -1.0;
        }
        else
        {
            m1 = S.project_findObject(S.LINK, tok[2]);
            if (m1 < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[2]);
            }
            x[1] = m1;
        }
        n = 4;
        m1 = S.findmatch(tok[3], S.DividerTypeWords);
        if (m1 < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
        }
        x[2] = m1;
        x[3] = -1;
        if (m1 == S.TABULAR_DIVIDER)
        {
            if (ntoks < 5)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            m2 = S.project_findObject(S.CURVE, tok[4]);
            if (m2 < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[4]);
            }
            x[3] = m2;
            n = 5;
        }
        if (m1 == S.CUTOFF_DIVIDER)
        {
            if (ntoks < 5)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (!((S.getDouble(tok[4], ref x[4]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[4]);
            }
            n = 5;
        }
        if (m1 == S.WEIR_DIVIDER)
        {
            if (ntoks < 7)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            for (i = 4; i < 7; i++)
            {
                if (!((S.getDouble(tok[i], ref x[i]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                }
            }
            n = 7;
        }
        if (m1 == S.OVERFLOW_DIVIDER)
        {
            n = 4;
        }
        m = 7;
        for (i = n; (i < ntoks) && (m < 11); i++)
        {
            if (!((S.getDouble(tok[i], ref x[m]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i]);
            }
            m++;
        }
        S.Node[j].ID = id;
        S.node_setParams(j, S.DIVIDER, k, x);
        return 0;
    }

    // node.c:1216
    public static void divider_validate(int j)
    {
        int i = 0;
        int k = 0;
        k = S.Node[j].subIndex;
        i = S.Divider[k].link;
        if ((i < 0) || (((S.Link[i].node1 != j) && (S.Link[i].node2 != j))))
        {
            S.report_writeErrorMsg(S.ERR_DIVIDER_LINK, S.Node[j].ID);
        }
        if (S.Divider[k].type == S.WEIR_DIVIDER)
        {
            if ((S.Divider[k].dhMax <= 0.0) || (S.Divider[k].cWeir <= 0.0))
            {
                S.report_writeErrorMsg(S.ERR_WEIR_DIVIDER, S.Node[j].ID);
            }
            else
            {
                S.Divider[k].qMax = S.Divider[k].cWeir * Math.Pow(S.Divider[k].dhMax, 1.5) / S.UCF(S.FLOW);
                if (S.Divider[k].qMin > S.Divider[k].qMax)
                {
                    S.report_writeErrorMsg(S.ERR_WEIR_DIVIDER, S.Node[j].ID);
                }
            }
        }
    }

    // node.c:1251
    public static double divider_getOutflow(int j, int k)
    {
        int i = 0;
        int m = 0;
        double qIn = 0.0;
        double qOut = 0.0;
        double f = 0.0;
        qIn = S.Node[j].inflow + S.Node[j].overflow;
        i = S.Node[j].subIndex;
        switch (S.Divider[i].type)
        {
            case S.CUTOFF_DIVIDER:
                if (qIn <= S.Divider[i].qMin)
                {
                    qOut = 0.0;
                }
                else
                {
                    qOut = qIn - S.Divider[i].qMin;
                }
                break;
            case S.OVERFLOW_DIVIDER:
                if (k != S.Divider[i].link)
                {
                    qOut = qIn;
                }
                else
                {
                    qOut = qIn - S.Node[j].outflow;
                }
                if (qOut < 1e-05)
                {
                    qOut = 0.0;
                }
                return qOut;
            case S.WEIR_DIVIDER:
                if (qIn <= S.Divider[i].qMin)
                {
                    qOut = 0.0;
                }
                else
                {
                    f = (qIn - S.Divider[i].qMin) / (S.Divider[i].qMax - S.Divider[i].qMin);
                    if (f > 1.0)
                    {
                        qOut = S.Divider[i].qMax * Math.Sqrt(f);
                    }
                    else
                    {
                        qOut = S.Divider[i].cWeir * Math.Pow(f * S.Divider[i].dhMax, 1.5) / S.UCF(S.FLOW);
                    }
                }
                break;
            case S.TABULAR_DIVIDER:
                m = S.Divider[i].flowCurve;
                if (m >= 0)
                {
                    qOut = S.table_lookup(S.Curve[m], qIn * S.UCF(S.FLOW)) / S.UCF(S.FLOW);
                }
                else
                {
                    qOut = 0.0;
                }
                break;
            default:
                qOut = 0.0;
                break;
        }
        if (qOut > qIn)
        {
            qOut = qIn;
        }
        if (k != S.Divider[i].link)
        {
            qOut = qIn - qOut;
        }
        return qOut;
    }

    // node.c:1333
    public static int outfall_readParams(int j, int k, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int m = 0;
        int n = 0;
        double[] x = new double[7];
        CPtr id = CPtr.Null;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.NODE, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        if (!((S.getDouble(tok[1], ref x[0]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[1]);
        }
        i = S.findmatch(tok[2], S.OutfallTypeWords);
        if (i < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[2]);
        }
        x[1] = i;
        x[2] = 0.0;
        x[3] = -1.0;
        x[4] = -1.0;
        x[5] = 0.0;
        x[6] = -1.0;
        n = 4;
        if (i >= S.FIXED_OUTFALL)
        {
            if (ntoks < 4)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            n = 5;
            switch (i)
            {
                case S.FIXED_OUTFALL:
                    if (!((S.getDouble(tok[3], ref x[2]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[3]);
                    }
                    break;
                case S.TIDAL_OUTFALL:
                    m = S.project_findObject(S.CURVE, tok[3]);
                    if (m < 0)
                    {
                        return S.error_setInpError(S.ERR_NAME, tok[3]);
                    }
                    x[3] = m;
                    break;
                case S.TIMESERIES_OUTFALL:
                    m = S.project_findObject(S.TSERIES, tok[3]);
                    if (m < 0)
                    {
                        return S.error_setInpError(S.ERR_NAME, tok[3]);
                    }
                    x[4] = m;
                    S.Tseries[m].refersTo = S.TIMESERIES_OUTFALL;
                    break;
            }
        }
        if (ntoks == n)
        {
            m = S.findmatch(tok[n - 1], S.NoYesWords);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[n - 1]);
            }
            x[5] = m;
        }
        if (ntoks == n + 1)
        {
            m = S.project_findObject(S.SUBCATCH, tok[n]);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[n]);
            }
            x[6] = m;
        }
        S.Node[j].ID = id;
        S.node_setParams(j, S.OUTFALL, k, x);
        return 0;
    }

    // node.c:1413
    public static void outfall_setOutletDepth(int j, double yNorm, double yCrit, double z)
    {
        double x = 0.0;
        double y = 0.0;
        double yNew = 0.0;
        double stage = 0.0;
        int k = 0;
        int i = S.Node[j].subIndex;
        double currentDate = 0.0;
        switch (S.Outfall[i].type)
        {
            case S.FREE_OUTFALL:
                if (z > 0.0)
                {
                    S.Node[j].newDepth = 0.0;
                }
                else
                {
                    S.Node[j].newDepth = ((((yNorm) <= (yCrit)) ? (yNorm) : (yCrit)));
                }
                return;
            case S.NORMAL_OUTFALL:
                if (z > 0.0)
                {
                    S.Node[j].newDepth = 0.0;
                }
                else
                {
                    S.Node[j].newDepth = yNorm;
                }
                return;
            case S.FIXED_OUTFALL:
                stage = S.Outfall[i].fixedStage;
                break;
            case S.TIDAL_OUTFALL:
                k = S.Outfall[i].tideCurve;
                S.table_getFirstEntry(S.Curve[k], ref x, ref y);
                currentDate = S.NewRoutingTime / 86400000.0;
                x += (currentDate - Math.Floor(currentDate)) * 24.0;
                stage = S.table_lookup(S.Curve[k], x) / S.UCF(S.LENGTH);
                break;
            case S.TIMESERIES_OUTFALL:
                k = S.Outfall[i].stageSeries;
                currentDate = S.StartDateTime + S.NewRoutingTime / 86400000.0;
                stage = S.table_tseriesLookup(S.Tseries[k], currentDate, 1) / S.UCF(S.LENGTH);
                break;
            default:
                stage = S.Node[j].invertElev;
                break;
        }
        yCrit = ((((yCrit) <= (yNorm)) ? (yCrit) : (yNorm)));
        if (yCrit + z + S.Node[j].invertElev < stage)
        {
            yNew = stage - S.Node[j].invertElev;
        }
        else if (z > 0.0)
        {
            if (stage < S.Node[j].invertElev + z)
            {
                yNew = ((((0.0) >= ((stage - S.Node[j].invertElev))) ? (0.0) : ((stage - S.Node[j].invertElev))));
            }
            else
            {
                yNew = z + yCrit;
            }
        }
        else
        {
            yNew = yCrit;
        }
        S.Node[j].newDepth = yNew;
    }

}
}
