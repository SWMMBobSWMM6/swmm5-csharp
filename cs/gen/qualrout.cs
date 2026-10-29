// Generated from EPA SWMM 5.2.4 (qualrout.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double ZeroVolume = 0.0353147;
    public static double ZeroDepth = 0.003281;

    // qualrout.c:63
    public static void qualrout_init()
    {
        int i = 0;
        int p = 0;
        int isWet = 0;
        double c = 0.0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            isWet = ((S.Node[i].newDepth > S.ZeroDepth) ? 1 : 0);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                c = 0.0;
                if ((isWet != 0))
                {
                    c = S.Pollut[p].initConcen;
                }
                S.Node[i].oldQual[p] = c;
                S.Node[i].newQual[p] = c;
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            isWet = ((S.Link[i].newDepth > S.ZeroDepth) ? 1 : 0);
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                c = 0.0;
                if ((isWet != 0))
                {
                    c = S.Pollut[p].initConcen;
                }
                S.Link[i].oldQual[p] = c;
                S.Link[i].newQual[p] = c;
            }
        }
    }

    // qualrout.c:100
    public static void qualrout_execute(double tStep)
    {
        int i = 0;
        int j = 0;
        double qIn = 0.0;
        double vAvg = 0.0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            S.findLinkMassFlow(i, tStep);
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.Node[j].qualInflow = S.Node[j].inflow;
            qIn = S.Node[j].qualInflow;
            vAvg = (S.Node[j].oldVolume + S.Node[j].newVolume) / 2.0;
            if ((S.Node[j].treatment != null))
            {
                if (qIn < 1e-10)
                {
                    qIn = 0.0;
                }
                S.treatmnt_setInflow(qIn, S.Node[j].newQual);
            }
            if ((S.Node[j].type == S.STORAGE) || (S.Node[j].oldVolume > S.ZeroVolume))
            {
                S.findStorageQual(j, tStep);
            }
            else
            {
                S.findNodeQual(j);
            }
            if ((S.Node[j].treatment != null))
            {
                S.treatmnt_treat(j, qIn, vAvg, tStep);
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            S.findLinkQual(i, tStep);
        }
    }

    // qualrout.c:146
    public static double getMixedQual(double c, double v1, double wIn, double qIn, double tStep)
    {
        double vIn = 0.0;
        double cIn = 0.0;
        double cMax = 0.0;
        if (qIn <= 1e-10)
        {
            return c;
        }
        vIn = qIn * tStep;
        cIn = wIn * tStep / vIn;
        cMax = ((((c) >= (cIn)) ? (c) : (cIn)));
        c = (c * v1 + wIn * tStep) / (v1 + vIn);
        c = ((((c) <= (cMax)) ? (c) : (cMax)));
        c = ((((c) >= (0.0)) ? (c) : (0.0)));
        return c;
    }

    // qualrout.c:179
    public static void findLinkMassFlow(int i, double tStep)
    {
        int j = 0;
        int p = 0;
        double qLink = 0.0;
        double w = 0.0;
        qLink = S.Link[i].newFlow;
        j = S.Link[i].node2;
        if (qLink < 0.0)
        {
            j = S.Link[i].node1;
        }
        qLink = Math.Abs(qLink);
        if (S.RouteModel != S.DW)
        {
            qLink -= S.inlet_capturedFlow(i);
        }
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            w = qLink * S.Link[i].oldQual[p];
            S.Node[j].newQual[p] += w;
            S.Link[i].totalLoad[p] += w * tStep;
        }
        S.Node[j].qualInflow += qLink;
    }

    // qualrout.c:221
    public static void findNodeQual(int j)
    {
        int p = 0;
        double qNode = 0.0;
        qNode = S.Node[j].qualInflow;
        if (qNode > 1e-10)
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                S.Node[j].newQual[p] /= qNode;
            }
        }
        else
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                if (S.Node[j].newDepth > S.ZeroDepth)
                {
                    S.Node[j].newQual[p] = S.Node[j].oldQual[p];
                }
                else
                {
                    S.Node[j].newQual[p] = 0.0;
                }
            }
        }
    }

    // qualrout.c:253
    public static void findLinkQual(int i, double tStep)
    {
        int j = 0;
        int k = 0;
        int p = 0;
        double wIn = 0.0;
        double qIn = 0.0;
        double qSeep = 0.0;
        double v1 = 0.0;
        double v2 = 0.0;
        double c1 = 0.0;
        double c2 = 0.0;
        double vEvap = 0.0;
        double vLosses = 0.0;
        double fEvap = 0.0;
        double barrels = 0.0;
        j = S.Link[i].node1;
        if (S.Link[i].newFlow < 0.0)
        {
            j = S.Link[i].node2;
        }
        if ((S.Link[i].type != S.CONDUIT) || (S.Link[i].xsect.type == S.DUMMY))
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                S.Link[i].newQual[p] = S.Node[j].newQual[p];
            }
            return;
        }
        k = S.Link[i].subIndex;
        barrels = S.Conduit[k].barrels;
        qIn = Math.Abs(S.Conduit[k].q1) * barrels;
        qSeep = S.Conduit[k].seepLossRate * barrels;
        vEvap = S.Conduit[k].evapLossRate * barrels * tStep;
        v1 = S.Link[i].oldVolume;
        v2 = S.Link[i].newVolume;
        vLosses = qSeep * tStep + vEvap;
        fEvap = 1.0;
        if ((vEvap > 0.0) && (v1 > S.ZeroVolume))
        {
            fEvap += vEvap / v1;
        }
        if (S.RouteModel == S.SF)
        {
            S.findSFLinkQual(i, qSeep, fEvap, tStep);
            return;
        }
        if (S.RouteModel == S.DW)
        {
            qIn = qIn + (v2 + vLosses - v1) / tStep;
            qIn = ((((qIn) >= (0.0)) ? (qIn) : (0.0)));
        }
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            c1 = S.Link[i].oldQual[p];
            S.massbal_addSeepageLoss(p, qSeep * c1);
            c1 *= fEvap;
            c2 = S.getReactedQual(p, c1, v1, tStep);
            wIn = S.Node[j].newQual[p] * qIn;
            c2 = S.getMixedQual(c2, v1, wIn, qIn, tStep);
            if ((v2 < S.ZeroVolume) || (S.Link[i].newDepth <= S.ZeroDepth))
            {
                S.massbal_addToFinalStorage(p, c2 * v2);
                c2 = 0.0;
            }
            S.Link[i].newQual[p] = c2;
        }
    }

    // qualrout.c:357
    public static void findSFLinkQual(int i, double qSeep, double fEvap, double tStep)
    {
        int j = S.Link[i].node1;
        int p = 0;
        double c1 = 0.0;
        double c2 = 0.0;
        double lossRate = 0.0;
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            c1 = S.Node[j].newQual[p];
            S.massbal_addSeepageLoss(p, qSeep * c1);
            c1 *= fEvap;
            c2 = c1;
            if (S.Pollut[p].kDecay > 0.0)
            {
                c2 = c1 * Math.Exp(-S.Pollut[p].kDecay * tStep);
                c2 = ((((0.0) >= (c2)) ? (0.0) : (c2)));
                lossRate = (c1 - c2) * S.Link[i].newFlow;
                S.massbal_addReactedMass(p, lossRate);
            }
            S.Link[i].newQual[p] = c2;
        }
    }

    // qualrout.c:398
    public static void findStorageQual(int j, double tStep)
    {
        int p = 0;
        int k = 0;
        double qIn = 0.0;
        double wIn = 0.0;
        double v1 = 0.0;
        double c1 = 0.0;
        double c2 = 0.0;
        double qExfil = 0.0;
        double vEvap = 0.0;
        double fEvap = 1.0;
        qIn = S.Node[j].qualInflow;
        v1 = S.Node[j].oldVolume;
        if (S.Node[j].type == S.STORAGE)
        {
            S.updateHRT(j, S.Node[j].oldVolume, qIn, tStep);
            k = S.Node[j].subIndex;
            qExfil = S.Storage[k].exfilLoss / tStep;
            vEvap = S.Storage[k].evapLoss;
            if ((vEvap > 0.0) && (v1 > S.ZeroVolume))
            {
                fEvap += vEvap / v1;
            }
        }
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            c1 = S.Node[j].oldQual[p];
            S.massbal_addSeepageLoss(p, qExfil * c1);
            c1 *= fEvap;
            if ((S.Node[j].treatment == null) || (S.Node[j].treatment[p].equation == null))
            {
                c1 = S.getReactedQual(p, c1, v1, tStep);
            }
            wIn = S.Node[j].newQual[p];
            c2 = S.getMixedQual(c1, v1, wIn, qIn, tStep);
            if ((((S.Node[j].newVolume <= S.ZeroVolume) || (S.Node[j].newDepth <= S.ZeroDepth))) && (qIn <= 1e-10))
            {
                S.massbal_addToFinalStorage(p, c2 * S.Node[j].newVolume);
                c2 = 0.0;
            }
            S.Node[j].newQual[p] = c2;
        }
    }

    // qualrout.c:478
    public static void updateHRT(int j, double v, double q, double tStep)
    {
        int k = S.Node[j].subIndex;
        double hrt = S.Storage[k].hrt;
        if (v < 1e-10)
        {
            hrt = 0.0;
        }
        else
        {
            hrt = (hrt + tStep) * v / (v + q * tStep);
        }
        S.Storage[k].hrt = ((((hrt) >= (0.0)) ? (hrt) : (0.0)));
    }

    // qualrout.c:498
    public static double getReactedQual(int p, double c, double v1, double tStep)
    {
        double c2 = 0.0;
        double lossRate = 0.0;
        double kDecay = S.Pollut[p].kDecay;
        if (kDecay == 0.0)
        {
            return c;
        }
        c2 = c * (1.0 - kDecay * tStep);
        c2 = ((((0.0) >= (c2)) ? (0.0) : (c2)));
        lossRate = (c - c2) * v1 / tStep;
        S.massbal_addReactedMass(p, lossRate);
        return c2;
    }

}
}
