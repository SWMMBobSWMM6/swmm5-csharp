// Generated from EPA SWMM 5.2.4 (inlet.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static TInletDesign[] InletDesigns = null;
    public static int InletDesignCount = 0;
    public static int UsesInlets = 0;
    public static CPtr[] InletTypeWords = new CPtr[] { S.L718, S.L719, S.L0, S.L720, S.L721, S.L722, S.L723, CPtr.Null };
    public static CPtr[] GrateTypeWords = new CPtr[] { S.L724, S.L725, S.L726, S.L727, S.L728, S.L729, S.L730, S.L731, CPtr.Null };
    public static CPtr[] ThroatAngleWords = new CPtr[] { S.L732, S.L733, S.L734, CPtr.Null };
    public static CPtr[] PlacementTypeWords = new CPtr[] { S.L735, S.L736, S.L737, CPtr.Null };
    public static double[][] SplashCoeffs = new double[][] { new double[] { 2.22, 4.03, 0.65, 0.06 }, new double[] { 0.74, 2.44, 0.27, 0.02 }, new double[] { 1.76, 3.12, 0.45, 0.03 }, new double[] { 0.30, 4.85, 1.31, 0.15 }, new double[] { 0.99, 2.64, 0.36, 0.03 }, new double[] { 0.51, 2.34, 0.2, 0.01 }, new double[] { 0.28, 2.28, 0.18, 0.01 } };
    public static double[] GrateOpeningRatios = new double[] { 0.90, 0.80, 0.60, 0.35, 0.17, 0.34, 0.80, 1.00 };
    public static double Sx = 0.0;
    public static double SL = 0.0;
    public static double Sw = 0.0;
    public static double a = 0.0;
    public static double W = 0.0;
    public static double T = 0.0;
    public static double n = 0.0;
    public static int Nsides = 0;
    public static double Tcrown = 0.0;
    public static double Beta = 0.0;
    public static double Qfactor = 0.0;
    public static TXsect xsect = null;
    public static double[] InletFlow = null;
    public static TInlet FirstInlet = null;

    // inlet.c:257
    public static int inlet_create(int numInlets)
    {
        int i = 0;
        S.InletDesigns = (null);
        S.InletFlow = (null);
        S.InletDesignCount = 0;
        S.UsesInlets = 0;
        S.FirstInlet = (null);
        S.InletDesigns = R.NewArray<TInletDesign>((int)(unchecked((ulong)(numInlets))));
        if (S.InletDesigns == null)
        {
            return S.ERR_MEMORY;
        }
        S.InletDesignCount = numInlets;
        S.InletFlow = new double[(int)(unchecked((ulong)(S.Nobjects[S.NODE])))];
        if (S.InletFlow == null)
        {
            return S.ERR_MEMORY;
        }
        for (i = 0; i < S.InletDesignCount; i++)
        {
            S.InletDesigns[i].customCurve = -1;
            S.InletDesigns[i].curbInlet.length = 0.0;
            S.InletDesigns[i].grateInlet.length = 0.0;
            S.InletDesigns[i].slottedInlet.length = 0.0;
            S.InletDesigns[i].type = S.CUSTOM_INLET;
        }
        return 0;
    }

    // inlet.c:291
    public static void inlet_delete()
    {
        TInlet inlet = S.FirstInlet;
        TInlet nextInlet = null;
        while ((inlet != null))
        {
            nextInlet = inlet.nextInlet;
            R.free(inlet);
            inlet = nextInlet;
        }
        S.FirstInlet = (null);
        {
            if ((S.InletFlow != null))
            {
                R.free(S.InletFlow);
                S.InletFlow = (null);
            }
        }
        ;
        {
            if ((S.InletDesigns != null))
            {
                R.free(S.InletDesigns);
                S.InletDesigns = (null);
            }
        }
        ;
    }

    // inlet.c:313
    public static int inlet_readDesignParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        i = S.project_findObject(S.INLET, tok[0]);
        if (i < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        S.InletDesigns[i].ID = S.project_findID(S.INLET, tok[0]);
        S.InletDesigns[i].type = S.findmatch(tok[1], S.InletTypeWords);
        switch (S.InletDesigns[i].type)
        {
            case S.GRATE_INLET:
            case S.DROP_GRATE_INLET:
                return S.readGrateInletParams(i, tok, ntoks);
            case S.CURB_INLET:
            case S.DROP_CURB_INLET:
                return S.readCurbInletParams(i, tok, ntoks);
            case S.SLOTTED_INLET:
                return S.readSlottedInletParams(i, tok, ntoks);
            case S.CUSTOM_INLET:
                return S.readCustomInletParams(i, tok, ntoks);
            default:
                return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        return 0;
    }

    // inlet.c:362
    public static int inlet_readUsageParams(CPtr[] tok, int ntoks)
    {
        int linkIndex = 0;
        int designIndex = 0;
        int nodeIndex = 0;
        int numInlets = 1;
        int placement = S.AUTOMATIC;
        double flowLimit = 0.0;
        double pctClogged = 0.0;
        double aLocal = 0.0;
        double wLocal = 0.0;
        TInlet inlet = null;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        linkIndex = S.project_findObject(S.LINK, tok[0]);
        if (linkIndex < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        designIndex = S.project_findObject(S.INLET, tok[1]);
        if (designIndex < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        nodeIndex = S.project_findObject(S.NODE, tok[2]);
        if (nodeIndex < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        if (ntoks > 3)
        {
            if ((!((S.getInt(tok[3], ref numInlets) != 0))) || (numInlets < 1))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[3]);
            }
        }
        if (ntoks > 4)
        {
            if (((!((S.getDouble(tok[4], ref pctClogged) != 0))) || (pctClogged < 0.0)) || (pctClogged > 99.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[4]);
            }
        }
        if (ntoks > 5)
        {
            if ((!((S.getDouble(tok[5], ref flowLimit) != 0))) || (flowLimit < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[5]);
            }
        }
        if (ntoks > 6)
        {
            if ((!((S.getDouble(tok[6], ref aLocal) != 0))) || (aLocal < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[6]);
            }
        }
        if (ntoks > 7)
        {
            if ((!((S.getDouble(tok[7], ref wLocal) != 0))) || (wLocal < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[7]);
            }
        }
        if (ntoks > 8)
        {
            placement = S.findmatch(tok[8], S.PlacementTypeWords);
            if (placement < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[8]);
            }
        }
        inlet = S.Link[linkIndex].inlet;
        if (inlet == null)
        {
            inlet = new TInlet();
            if (!(inlet != null))
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
            S.Link[linkIndex].inlet = inlet;
            inlet.nextInlet = S.FirstInlet;
            S.FirstInlet = inlet;
        }
        inlet.linkIndex = linkIndex;
        inlet.designIndex = designIndex;
        inlet.nodeIndex = nodeIndex;
        inlet.numInlets = numInlets;
        inlet.placement = placement;
        inlet.clogFactor = 1.0 - (pctClogged / 100.0);
        inlet.flowLimit = flowLimit / S.UCF(S.FLOW);
        inlet.localDepress = aLocal / S.UCF(S.LENGTH);
        inlet.localWidth = wLocal / S.UCF(S.LENGTH);
        inlet.flowFactor = 0.0;
        inlet.backflowRatio = 0.0;
        S.initInletStats(inlet);
        S.UsesInlets = 1;
        return 0;
    }

    // inlet.c:464
    public static void inlet_validate()
    {
        int i = 0;
        int j = 0;
        int inletType = 0;
        int inletValid = 0;
        TInlet inlet = null;
        TInlet prevInlet = null;
        if (!(S.UsesInlets != 0))
        {
            return;
        }
        prevInlet = S.FirstInlet;
        inlet = S.FirstInlet;
        while ((inlet != null))
        {
            inletValid = 0;
            i = inlet.linkIndex;
            S.xsect = S.Link[i].xsect;
            inletType = S.InletDesigns[inlet.designIndex].type;
            if (inletType == S.CUSTOM_INLET)
            {
                j = S.InletDesigns[inlet.designIndex].customCurve;
                if (j >= 0)
                {
                    if ((S.Curve[j].curveType == S.DIVERSION_CURVE) || (S.Curve[j].curveType == S.RATING_CURVE))
                    {
                        inletValid = 1;
                    }
                }
            }
            else if ((((S.xsect.type == S.TRAPEZOIDAL) || (S.xsect.type == S.RECT_OPEN))) && (((inletType == S.DROP_GRATE_INLET) || (inletType == S.DROP_CURB_INLET))))
            {
                inletValid = 1;
            }
            else if (((S.xsect.type == S.STREET_XSECT) && (inletType != S.DROP_GRATE_INLET)) && (inletType != S.DROP_CURB_INLET))
            {
                inletValid = 1;
            }
            if ((inletValid != 0))
            {
                S.Node[S.Link[i].node2].inlet = S.BYPASS;
                S.Node[inlet.nodeIndex].inlet = S.CAPTURE;
                inlet.backflow = 0.0;
                S.getConduitGeometry(inlet);
                inlet.flowFactor = (0.56 / S.n) * Math.Pow(S.SL, 0.5) * Math.Pow(S.Sx, 1.67);
                prevInlet = inlet;
                inlet = inlet.nextInlet;
            }
            else
            {
                S.report_writeWarningMsg(S.L187, S.Link[i].ID);
                if (inlet == S.FirstInlet)
                {
                    S.FirstInlet = inlet.nextInlet;
                    prevInlet = S.FirstInlet;
                    R.free(inlet);
                    inlet = S.FirstInlet;
                }
                else
                {
                    prevInlet.nextInlet = inlet.nextInlet;
                    R.free(inlet);
                    inlet = prevInlet.nextInlet;
                }
                S.Link[i].inlet = (null);
            }
        }
        S.getBackflowRatios();
    }

    // inlet.c:555
    public static void inlet_findCapturedFlows(double tStep)
    {
        int i = 0;
        int j = 0;
        int m = 0;
        int placement = 0;
        double q = 0.0;
        TInlet inlet = null;
        if (!(S.UsesInlets != 0))
        {
            return;
        }
        R.memset(S.InletFlow, 0, unchecked((ulong)(S.Nobjects[S.NODE])) * 8);
        if (S.RouteModel != S.DW)
        {
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                S.Node[j].inflow = ((((0.0) >= (S.Node[j].newLatFlow)) ? (0.0) : (S.Node[j].newLatFlow)));
            }
            for (i = 0; i < S.Nobjects[S.LINK]; i++)
            {
                S.Node[S.Link[i].node2].inflow += ((((0.0) >= (S.Link[i].newFlow)) ? (0.0) : (S.Link[i].newFlow)));
            }
        }
        for (inlet = S.FirstInlet; inlet != null; inlet = inlet.nextInlet)
        {
            i = inlet.linkIndex;
            j = S.Link[i].node2;
            m = inlet.nodeIndex;
            placement = S.getInletPlacement(inlet, j);
            if (S.InletDesigns[inlet.designIndex].type == S.CUSTOM_INLET)
            {
                q = Math.Abs(S.Link[i].newFlow);
                inlet.flowCapture = S.getCustomCapturedFlow(inlet, q, S.Node[j].newDepth);
            }
            else if (placement == S.ON_GRADE)
            {
                q = Math.Abs(S.Link[i].newFlow);
                inlet.flowCapture = S.getOnGradeCapturedFlow(inlet, q, S.Node[j].newDepth);
            }
            else
            {
                q = S.Node[j].inflow;
                inlet.flowCapture = S.getOnSagCapturedFlow(inlet, q, S.Node[j].newDepth);
            }
            if (Math.Abs(inlet.flowCapture) < 0.0001)
            {
                inlet.flowCapture = 0.0;
            }
            S.InletFlow[j] += inlet.flowCapture;
            inlet.backflow = S.Node[m].overflow * inlet.backflowRatio;
            if (Math.Abs(inlet.backflow) < 0.0001)
            {
                inlet.backflow = 0.0;
            }
        }
        for (inlet = S.FirstInlet; inlet != null; inlet = inlet.nextInlet)
        {
            i = inlet.linkIndex;
            j = S.Link[i].node2;
            m = inlet.nodeIndex;
            if ((S.RouteModel != S.DW) && (S.getInletPlacement(inlet, j) == S.ON_SAG))
            {
                q = S.Node[j].newVolume / tStep;
                q += ((((S.Node[j].inflow) >= (0.0)) ? (S.Node[j].inflow) : (0.0)));
                if (S.InletFlow[j] > q)
                {
                    inlet.flowCapture *= q / S.InletFlow[j];
                }
            }
            S.Node[j].newLatFlow -= (inlet.flowCapture - inlet.backflow);
            S.Node[m].newLatFlow += inlet.flowCapture;
            if (S.getDateTime(S.NewRoutingTime) > S.ReportStart)
            {
                S.updateInletStats(inlet, Math.Abs(S.Link[i].newFlow));
            }
        }
    }

    // inlet.c:655
    public static void inlet_adjustQualInflows()
    {
        int i = 0;
        int j = 0;
        int m = 0;
        int p = 0;
        double qNet = 0.0;
        TInlet inlet = null;
        if (!(S.UsesInlets != 0))
        {
            return;
        }
        if ((S.IgnoreQuality != 0) || (S.Nobjects[S.POLLUT] == 0))
        {
            return;
        }
        for (inlet = S.FirstInlet; inlet != null; inlet = inlet.nextInlet)
        {
            i = inlet.linkIndex;
            j = S.Link[i].node2;
            m = inlet.nodeIndex;
            qNet = inlet.flowCapture - inlet.backflow;
            if (qNet > 0.0)
            {
                S.Node[m].qualInflow += qNet;
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    S.Node[m].newQual[p] += qNet * S.Node[j].oldQual[p];
                }
            }
            else
            {
                qNet = -qNet;
                S.Node[j].qualInflow += qNet;
                for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                {
                    S.Node[j].newQual[p] += qNet * S.Node[m].oldQual[p];
                }
            }
        }
    }

    // inlet.c:706
    public static void inlet_adjustQualOutflows()
    {
        int j = 0;
        int p = 0;
        double q = 0.0;
        double w = 0.0;
        TInlet inlet = null;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if (S.Node[j].inlet == S.CAPTURE)
            {
                q = S.Node[j].overflow;
                if (q > 0.0)
                {
                    S.StepFlowTotals.flooding -= q;
                    if (!(S.IgnoreQuality != 0))
                    {
                        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                        {
                            w = q * S.Node[j].newQual[p];
                            S.StepQualTotals[p].flooding -= w;
                        }
                    }
                }
            }
        }
        if ((!(S.IgnoreQuality != 0)) && (S.Nobjects[S.POLLUT] > 0))
        {
            for (inlet = S.FirstInlet; inlet != null; inlet = inlet.nextInlet)
            {
                j = S.Link[inlet.linkIndex].node2;
                q = inlet.flowCapture - inlet.backflow;
                if ((q > 0.0) && (S.Node[j].newLatFlow < 0.0))
                {
                    for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                    {
                        w = q * S.Node[j].newQual[p];
                        S.StepQualTotals[p].outflow -= w;
                    }
                }
            }
        }
    }

    // inlet.c:774
    public static void inlet_writeStatsReport()
    {
        int j = 0;
        int header = 0;
        if (S.Nobjects[S.STREET] == 0)
        {
            return;
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if (S.Link[j].xsect.type == S.STREET_XSECT)
            {
                if (!(header != 0))
                {
                    S.writeStreetStatsHeader();
                    header = 1;
                }
                S.writeStreetStats(j);
            }
        }
        S.report_writeLine(S.L0);
    }

    // inlet.c:801
    public static double inlet_capturedFlow(int i)
    {
        if ((S.Link[i].inlet != null))
        {
            return S.Link[i].inlet.flowCapture;
        }
        return 0.0;
    }

    // inlet.c:814
    public static int readGrateInletParams(int i, CPtr[] tok, int ntoks)
    {
        int grateType = 0;
        double width = 0.0;
        double length = 0.0;
        double areaRatio = 0.0;
        double vSplash = 0.0;
        if (ntoks < 5)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        if ((!((S.getDouble(tok[2], ref length) != 0))) || (length <= 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[2]);
        }
        if ((!((S.getDouble(tok[3], ref width) != 0))) || (width <= 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[3]);
        }
        grateType = S.findmatch(tok[4], S.GrateTypeWords);
        if (grateType < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[4]);
        }
        if (grateType == S.GENERIC)
        {
            if (ntoks < 6)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (((!((S.getDouble(tok[5], ref areaRatio) != 0))) || (areaRatio <= 0.0)) || (areaRatio > 1.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[5]);
            }
            if (ntoks > 6)
            {
                if ((!((S.getDouble(tok[6], ref vSplash) != 0))) || (vSplash < 0.0))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[6]);
                }
            }
        }
        S.InletDesigns[i].grateInlet.length = length / S.UCF(S.LENGTH);
        S.InletDesigns[i].grateInlet.width = width / S.UCF(S.LENGTH);
        S.InletDesigns[i].grateInlet.type = grateType;
        S.InletDesigns[i].grateInlet.fracOpenArea = areaRatio;
        S.InletDesigns[i].grateInlet.splashVeloc = vSplash / S.UCF(S.LENGTH);
        if ((S.InletDesigns[i].type == S.GRATE_INLET) && (S.InletDesigns[i].curbInlet.length > 0.0))
        {
            S.InletDesigns[i].type = S.COMBO_INLET;
        }
        return 0;
    }

    // inlet.c:868
    public static int readCurbInletParams(int i, CPtr[] tok, int ntoks)
    {
        int throatAngle = 0;
        double height = 0.0;
        double length = 0.0;
        if (ntoks < 4)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        if ((!((S.getDouble(tok[2], ref length) != 0))) || (length <= 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[2]);
        }
        if ((!((S.getDouble(tok[3], ref height) != 0))) || (height <= 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[3]);
        }
        throatAngle = S.VERTICAL_THROAT;
        if ((S.InletDesigns[i].type == S.CURB_INLET) && (ntoks > 4))
        {
            throatAngle = S.findmatch(tok[4], S.ThroatAngleWords);
            if (throatAngle < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[4]);
            }
        }
        S.InletDesigns[i].curbInlet.length = length / S.UCF(S.LENGTH);
        S.InletDesigns[i].curbInlet.height = height / S.UCF(S.LENGTH);
        S.InletDesigns[i].curbInlet.throatAngle = throatAngle;
        if ((S.InletDesigns[i].type == S.CURB_INLET) && (S.InletDesigns[i].grateInlet.length > 0.0))
        {
            S.InletDesigns[i].type = S.COMBO_INLET;
        }
        return 0;
    }

    // inlet.c:911
    public static int readSlottedInletParams(int i, CPtr[] tok, int ntoks)
    {
        double width = 0.0;
        double length = 0.0;
        if (ntoks < 4)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        if ((!((S.getDouble(tok[2], ref length) != 0))) || (length <= 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[2]);
        }
        if ((!((S.getDouble(tok[3], ref width) != 0))) || (width <= 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[3]);
        }
        S.InletDesigns[i].slottedInlet.length = length / S.UCF(S.LENGTH);
        S.InletDesigns[i].slottedInlet.width = width / S.UCF(S.LENGTH);
        return 0;
    }

    // inlet.c:939
    public static int readCustomInletParams(int i, CPtr[] tok, int ntoks)
    {
        int c = 0;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        else
        {
            c = S.project_findObject(S.CURVE, tok[2]);
            if (c < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[2]);
            }
        }
        S.InletDesigns[i].customCurve = c;
        return 0;
    }

    // inlet.c:962
    public static void initInletStats(TInlet inlet)
    {
        if ((inlet != null))
        {
            inlet.flowCapture = 0.0;
            inlet.backflow = 0.0;
            inlet.stats.flowPeriods = 0;
            inlet.stats.capturePeriods = 0;
            inlet.stats.backflowPeriods = 0;
            inlet.stats.peakFlow = 0.0;
            inlet.stats.peakFlowCapture = 0;
            inlet.stats.avgFlowCapture = 0;
            inlet.stats.bypassFreq = 0;
        }
    }

    // inlet.c:985
    public static void updateInletStats(TInlet inlet, double q)
    {
        double qCapture = inlet.flowCapture;
        double qBackflow = inlet.backflow;
        double qNet = qCapture - qBackflow;
        double qBypass = q - qNet;
        double fCapture = 0.0;
        if ((q < 0.001) && (qBackflow <= 0.0))
        {
            return;
        }
        inlet.stats.flowPeriods++;
        if (qNet > 0.0)
        {
            inlet.stats.capturePeriods++;
            fCapture = qNet / q;
            fCapture = ((((fCapture) <= (1.0)) ? (fCapture) : (1.0)));
            inlet.stats.avgFlowCapture += fCapture;
            if (qBypass > 0.001)
            {
                inlet.stats.bypassFreq++;
            }
        }
        else
        {
            inlet.stats.backflowPeriods++;
        }
        if (q > inlet.stats.peakFlow)
        {
            inlet.stats.peakFlow = q;
            inlet.stats.peakFlowCapture = fCapture * 100.0;
        }
    }

    // inlet.c:1026
    public static void writeStreetStatsHeader()
    {
        S.report_writeLine(S.L0);
        S.report_writeLine(S.L188);
        S.report_writeLine(S.L189);
        S.report_writeLine(S.L188);
        S.report_writeLine(S.L0);
        R.fprintf(S.Frpt.file, S.L190);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L191, S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits]);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L192, S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits]);
        }
        R.fprintf(S.Frpt.file, S.L193);
    }

    // inlet.c:1055
    public static void writeStreetStats(int link)
    {
        int k = 0;
        int t = 0;
        int placement = 0;
        double maxSpread = 0.0;
        double maxDepth = 0.0;
        double maxFlow = 0.0;
        double fp = 0.0;
        double cp = 0.0;
        double afc = 0.0;
        double bpf = 0.0;
        TInlet inlet = null;
        k = S.Link[link].subIndex;
        t = S.Link[link].xsect.transect;
        inlet = S.Link[link].inlet;
        maxFlow = S.LinkStats[link].maxFlow;
        maxDepth = S.LinkStats[link].maxDepth;
        maxSpread = S.xsect_getWofY(S.Link[link].xsect, maxDepth) / S.Street[t].sides;
        maxSpread = ((((maxSpread) <= (S.Street[t].width)) ? (maxSpread) : (S.Street[t].width)));
        R.fprintf(S.Frpt.file, S.L194, S.Link[link].ID);
        R.fprintf(S.Frpt.file, S.L195, maxFlow * S.UCF(S.FLOW));
        R.fprintf(S.Frpt.file, S.L195, maxSpread * S.UCF(S.LENGTH));
        R.fprintf(S.Frpt.file, S.L195, maxDepth * S.UCF(S.LENGTH));
        if ((inlet != null))
        {
            R.fprintf(S.Frpt.file, S.L196, S.InletDesigns[inlet.designIndex].ID);
            placement = S.getInletPlacement(inlet, S.Link[inlet.linkIndex].node2);
            if (placement == S.ON_GRADE)
            {
                R.fprintf(S.Frpt.file, S.L197);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L198);
            }
            R.fprintf(S.Frpt.file, S.L199, inlet.numInlets);
            fp = inlet.stats.flowPeriods / 100.0;
            if (fp > 0.0)
            {
                cp = inlet.stats.capturePeriods / 100.0;
                R.fprintf(S.Frpt.file, S.L200, inlet.stats.peakFlowCapture);
                if (cp > 0.0)
                {
                    afc = inlet.stats.avgFlowCapture / cp;
                    bpf = inlet.stats.bypassFreq / cp;
                }
                R.fprintf(S.Frpt.file, S.L200, afc);
                R.fprintf(S.Frpt.file, S.L200, bpf);
                R.fprintf(S.Frpt.file, S.L200, inlet.stats.backflowPeriods / fp);
                R.fprintf(S.Frpt.file, S.L200, (maxFlow / S.Street[t].sides) * S.UCF(S.FLOW) * 0.01 * inlet.stats.peakFlowCapture / inlet.numInlets);
                R.fprintf(S.Frpt.file, S.L200, maxFlow * S.UCF(S.FLOW) * 0.01 * (100.0 - inlet.stats.peakFlowCapture));
            }
        }
    }

    // inlet.c:1129
    public static int getInletPlacement(TInlet inlet, int j)
    {
        if (inlet.placement == S.AUTOMATIC)
        {
            if (S.Node[j].degree > 0)
            {
                return S.ON_GRADE;
            }
            else
            {
                return S.ON_SAG;
            }
        }
        else
        {
            return inlet.placement;
        }
    }

    // inlet.c:1147
    public static void getConduitGeometry(TInlet inlet)
    {
        int linkIndex = inlet.linkIndex;
        int t = 0;
        int k = S.Link[linkIndex].subIndex;
        S.SL = S.Conduit[k].slope;
        S.Beta = S.Conduit[k].beta;
        S.xsect = S.Link[linkIndex].xsect;
        if (S.xsect.type == S.STREET_XSECT)
        {
            t = S.xsect.transect;
            S.Sx = S.Street[t].slope;
            S.a = S.Street[t].gutterDepression;
            S.W = S.Street[t].gutterWidth;
            S.n = S.Street[t].roughness;
            S.Nsides = S.Street[t].sides;
            S.Tcrown = S.Street[t].width;
            S.Qfactor = inlet.flowFactor;
            if ((inlet != null) && (inlet.localDepress * inlet.localWidth > 0))
            {
                S.a += inlet.localDepress;
                S.W = inlet.localWidth;
            }
            if (S.W * S.a > 0.0)
            {
                S.Sw = S.Sx + S.a / S.W;
            }
            else
            {
                S.Sw = S.Sx;
            }
        }
        else
        {
            S.a = 0.0;
            S.W = 0.0;
            S.n = S.Conduit[k].roughness;
            S.Nsides = 1;
            S.Sx = 0.01;
            S.Sw = S.Sx;
        }
    }

    // inlet.c:1200
    public static double getFlowSpread(double Q)
    {
        int iter = 0;
        double f = 0.0;
        double f1 = 0.0;
        double Sr = 0.0;
        double Ts1 = 0.0;
        double Ts2 = 0.0;
        double Tw = 0.0;
        double Qs = 0.0;
        double Eo = 0.0;
        f = S.Qfactor;
        if (S.a == 0.0)
        {
            Ts1 = Math.Pow(Q / f, 0.375);
        }
        else
        {
            f1 = f * Math.Pow((S.a / S.W) / S.Sx, 1.67);
            Tw = Math.Pow(Q / f1, 0.375);
            if (Tw <= S.W)
            {
                Ts1 = Tw;
            }
            else
            {
                Sr = (S.Sx + S.a / S.W) / S.Sx;
                iter = 1;
                Ts1 = Math.Pow(Q / f, 0.375) - S.W;
                if (Ts1 <= 0)
                {
                    Ts1 = Tw - S.W;
                }
                while (iter < 11)
                {
                    Eo = S.getEo(Sr, Ts1, S.W);
                    Qs = (1.0 - Eo) * Q;
                    Ts2 = Math.Pow(Qs / f, 0.375);
                    if (Math.Abs(Ts2 - Ts1) < 0.01)
                    {
                        break;
                    }
                    Ts1 = Ts2;
                    iter++;
                }
                Ts1 = Ts2 + S.W;
            }
        }
        return ((((Ts1) <= (S.Tcrown)) ? (Ts1) : (S.Tcrown)));
    }

    // inlet.c:1248
    public static double getEo(double Sr, double Ts, double w)
    {
        double x = 0.0;
        x = Sr / (Ts / w);
        x = Math.Pow((1.0 + x), 2.67) - 1.0;
        x = 1.0 + Sr / x;
        return 1.0 / x;
    }

    // inlet.c:1267
    public static double getOnGradeCapturedFlow(TInlet inlet, double q, double d)
    {
        int i = 0;
        int linkIndex = 0;
        double qApproach = 0.0;
        double qc = 0.0;
        double qCaptured = 0.0;
        double qBypassed = 0.0;
        double qMax = 0.0;
        if (inlet.numInlets == 0)
        {
            return 0.0;
        }
        linkIndex = inlet.linkIndex;
        qApproach = q;
        if (qApproach < 0.001)
        {
            return 0.0;
        }
        S.getConduitGeometry(inlet);
        qApproach /= S.Nsides;
        qBypassed = qApproach;
        qCaptured = 0.0;
        qMax = 10000000000.0;
        if (inlet.flowLimit > 0.0)
        {
            qMax = inlet.flowLimit;
        }
        for (i = 1; i <= inlet.numInlets; i++)
        {
            qc = S.getOnGradeInletCapture(inlet.designIndex, qBypassed, d) * inlet.clogFactor;
            qc = ((((qc) <= (qMax)) ? (qc) : (qMax)));
            qc = ((((qc) <= (qBypassed)) ? (qc) : (qBypassed)));
            qCaptured += qc;
            qBypassed -= qc;
            if (qBypassed < 0.001)
            {
                break;
            }
        }
        return qCaptured *= S.Nsides;
    }

    // inlet.c:1325
    public static double getOnGradeInletCapture(int i, double Q, double d)
    {
        double Q1 = Q;
        double Qc = 0.0;
        double Lsweep = 0.0;
        double Lcurb = 0.0;
        double Lgrate = 0.0;
        if (S.InletDesigns[i].type == S.DROP_CURB_INLET)
        {
            Qc = S.getOnSagInletCapture(i, d);
            return ((((Qc) <= (Q)) ? (Qc) : (Q)));
        }
        if (S.InletDesigns[i].type == S.DROP_GRATE_INLET)
        {
            Qc = S.getGrateInletCapture(i, Q);
            return ((((Qc) <= (Q)) ? (Qc) : (Q)));
        }
        S.T = S.getFlowSpread(Q);
        if (S.InletDesigns[i].type == S.SLOTTED_INLET)
        {
            Qc = S.getCurbInletCapture(Q, S.InletDesigns[i].slottedInlet.length);
            return ((((Qc) <= (Q)) ? (Qc) : (Q)));
        }
        Lcurb = S.InletDesigns[i].curbInlet.length;
        Lgrate = S.InletDesigns[i].grateInlet.length;
        if (Lcurb > 0.0)
        {
            Lsweep = Lcurb - Lgrate;
            if (Lsweep > 0.0)
            {
                Qc = S.getCurbInletCapture(Q1, Lsweep);
                Q1 -= Qc;
            }
        }
        if ((Lgrate > 0.0) && (Q1 > 0.0))
        {
            if (Q1 != Q)
            {
                S.T = S.getFlowSpread(Q1);
            }
            Qc += S.getGrateInletCapture(i, Q1);
        }
        return Qc;
    }

    // inlet.c:1387
    public static double getGrateInletCapture(int i, double Q)
    {
        int grateType = 0;
        double Lg = 0.0;
        double Wg = 0.0;
        double A = 0.0;
        double Y = 0.0;
        double Eo = 0.0;
        double V = 0.0;
        double Vo = 0.0;
        double Qo = Q;
        double Rf = 1.0;
        double Rs = 0.0;
        Lg = S.InletDesigns[i].grateInlet.length;
        Wg = S.InletDesigns[i].grateInlet.width;
        if ((S.xsect.type == S.TRAPEZOIDAL) || (S.xsect.type == S.RECT_OPEN))
        {
            A = S.xsect_getAofS(S.xsect, Q / S.Beta);
            Y = S.xsect_getYofA(S.xsect, A);
            S.T = S.xsect_getWofY(S.xsect, Y);
            Eo = S.Beta * Math.Pow(Y * Wg, 1.67) / Math.Pow(Wg + 2 * Y, 0.67) / Q;
            if (((Wg > 0.99 * S.xsect.yBot) && (S.xsect.type == S.TRAPEZOIDAL)) && (S.xsect.sBot > 0.0))
            {
                Wg = S.xsect.yBot;
                S.Sx = 1.0 / S.xsect.sBot;
            }
        }
        else if (S.a == 0.0)
        {
            A = S.T * S.T * S.Sx / 2.0;
            Eo = S.getGutterFlowRatio(Wg);
            if (S.T >= S.Tcrown)
            {
                Qo = S.Qfactor * Math.Pow(S.Tcrown, 2.67);
            }
        }
        else
        {
            if (S.T <= S.W)
            {
                A = S.T * S.T * S.Sw / 2.0;
            }
            else
            {
                A = (S.T * S.T * S.Sx + S.a * S.W) / 2.0;
            }
            Eo = S.getGutterFlowRatio(S.W);
            if (Eo < 1.0)
            {
                if (S.T >= S.Tcrown)
                {
                    Qo = S.Qfactor * Math.Pow(S.Tcrown, 2.67) / (1.0 - Eo);
                }
                Eo = Eo * S.getGutterAreaRatio(Wg, A);
            }
        }
        V = Qo / A;
        grateType = S.InletDesigns[i].grateInlet.type;
        if ((grateType < 0) || (grateType == S.GENERIC))
        {
            Vo = S.InletDesigns[i].grateInlet.splashVeloc;
        }
        else
        {
            Vo = S.getSplashOverVelocity(grateType, Lg);
        }
        if (V > Vo)
        {
            Rf = 1.0 - 0.09 * (V - Vo);
        }
        if (Eo < 1.0)
        {
            Rs = 1.0 / (1.0 + (0.15 * Math.Pow(V, 1.8) / S.Sx / Math.Pow(Lg, 2.3)));
        }
        return Q * (Rf * Eo + Rs * (1.0 - Eo));
    }

    // inlet.c:1477
    public static double getCurbInletCapture(double Q, double L)
    {
        double Se = S.Sx;
        double Lt = 0.0;
        double Sr = 0.0;
        double Eo = 0.0;
        double E = 1.0;
        if (S.a > 0.0)
        {
            Sr = S.Sw / S.Sx;
            Eo = S.getEo(Sr, S.T - S.W, S.W);
            Se = S.Sx + (S.a / S.W) * Eo;
        }
        Lt = 0.6 * Math.Pow(Q, 0.42) * Math.Pow(S.SL, 0.3) * Math.Pow(1.0 / (S.n * Se), 0.6);
        if (L < Lt)
        {
            E = 1.0 - (L / Lt);
            E = 1 - Math.Pow(E, 1.8);
        }
        E = ((((E) <= (1.0)) ? (E) : (1.0)));
        E = ((((E) >= (0.0)) ? (E) : (0.0)));
        return E * Q;
    }

    // inlet.c:1518
    public static double getGutterFlowRatio(double w)
    {
        if (S.T <= w)
        {
            return 1.0;
        }
        else if (S.a > 0.0)
        {
            return S.getEo(S.Sw / S.Sx, S.T - w, w);
        }
        else
        {
            return 1.0 - Math.Pow((1.0 - w / S.T), 2.67);
        }
    }

    // inlet.c:1535
    public static double getGutterAreaRatio(double Wg, double A)
    {
        double As = 0.0;
        double Ag = 0.0;
        if (Wg >= S.W)
        {
            return 1.0;
        }
        if (S.T <= Wg)
        {
            return 1.0;
        }
        if (S.T <= S.W)
        {
            return Wg / S.T;
        }
        As = 0.5 * (((S.T - S.W)) * ((S.T - S.W))) * S.Sx;
        Ag = Wg * ((S.T * S.Sx) + S.a - (Wg * S.Sw / 2.0));
        return Ag / (A - As);
    }

    // inlet.c:1557
    public static double getSplashOverVelocity(int grateType, double L)
    {
        return S.SplashCoeffs[grateType][0] + S.SplashCoeffs[grateType][1] * L - S.SplashCoeffs[grateType][2] * L * L + S.SplashCoeffs[grateType][3] * L * L * L;
    }

    // inlet.c:1574
    public static double getOnSagCapturedFlow(TInlet inlet, double q, double d)
    {
        int linkIndex = 0;
        int designIndex = 0;
        int totalInlets = 0;
        double qCaptured = 0.0;
        double qMax = 10000000000.0;
        if (inlet.numInlets == 0)
        {
            return 0.0;
        }
        totalInlets = S.Nsides * inlet.numInlets;
        linkIndex = inlet.linkIndex;
        designIndex = inlet.designIndex;
        S.getConduitGeometry(inlet);
        if (inlet.flowLimit > 0.0)
        {
            qMax = inlet.flowLimit;
        }
        qCaptured = S.getOnSagInletCapture(designIndex, Math.Abs(d));
        qCaptured *= inlet.clogFactor;
        qCaptured = ((((qCaptured) <= (qMax)) ? (qCaptured) : (qMax)));
        qCaptured *= (double)(totalInlets);
        return qCaptured;
    }

    // inlet.c:1610
    public static double getOnSagInletCapture(int i, double d)
    {
        double Lsweep = 0.0;
        double Lcurb = 0.0;
        double Lgrate = 0.0;
        double Qsw = 0.0;
        double Qso = 0.0;
        double Qgw = 0.0;
        double Qgo = 0.0;
        double Qcw = 0.0;
        double Qco = 0.0;
        if (S.InletDesigns[i].slottedInlet.length > 0.0)
        {
            return S.getOnSagSlottedFlow(i, d);
        }
        Lgrate = S.InletDesigns[i].grateInlet.length;
        if (Lgrate > 0.0)
        {
            S.findOnSagGrateFlows(i, d, ref Qgw, ref Qgo);
        }
        Lcurb = S.InletDesigns[i].curbInlet.length;
        if (Lcurb > 0.0)
        {
            Lsweep = Lcurb - Lgrate;
            if (Lsweep > 0.0)
            {
                S.findOnSagCurbFlows(i, d, Lsweep, ref Qsw, ref Qso);
            }
            if (Qgo > 0.0)
            {
                S.findOnSagCurbFlows(i, d, Lgrate, ref Qcw, ref Qco);
            }
        }
        return Qgw + Qgo + Qsw + Qso + Qco;
    }

    // inlet.c:1644
    public static void findOnSagGrateFlows(int i, double d, ref double Qw, ref double Qo)
    {
        int grateType = S.InletDesigns[i].grateInlet.type;
        double Lg = S.InletDesigns[i].grateInlet.length;
        double Wg = S.InletDesigns[i].grateInlet.width;
        double P = 0.0;
        double Ao = 0.0;
        double di = 0.0;
        if (S.InletDesigns[i].type == S.DROP_GRATE_INLET)
        {
            di = d;
            P = 2.0 * (Lg + Wg);
        }
        else
        {
            if (d <= Wg * S.Sw)
            {
                Wg = d / S.Sw;
            }
            di = d - (Wg / 2.0) * S.Sw;
            P = Lg + 2.0 * Wg;
        }
        if (grateType == S.GENERIC)
        {
            Ao = Lg * Wg * S.InletDesigns[i].grateInlet.fracOpenArea;
        }
        else
        {
            Ao = Lg * Wg * S.GrateOpeningRatios[grateType];
        }
        if (d <= 1.79 * Ao / P)
        {
            Qw = 3.0 * P * Math.Pow(di, 1.5);
        }
        else
        {
            Qo = 0.67 * Ao * Math.Sqrt(2.0 * 32.16 * di);
        }
    }

    // inlet.c:1703
    public static void findOnSagCurbFlows(int i, double d, double L, ref double Qw, ref double Qo)
    {
        int throatAngle = S.InletDesigns[i].curbInlet.throatAngle;
        double h = S.InletDesigns[i].curbInlet.height;
        double Qweir = 0.0;
        double Qorif = 0.0;
        double P = 0.0;
        double dweir = 0.0;
        double dorif = 0.0;
        double r = 0.0;
        if (L <= 0.0)
        {
            return;
        }
        if (S.InletDesigns[i].type == S.DROP_CURB_INLET)
        {
            L = L * 4.0;
        }
        dorif = 1.4 * h;
        if (d > dorif)
        {
            Qo = S.getCurbOrificeFlow(d, h, L, throatAngle);
            return;
        }
        if ((S.a == 0.0) || (L > 12.0))
        {
            dweir = h;
            if (d < dweir)
            {
                Qw = 3.0 * L * Math.Pow(d, 1.5);
                return;
            }
            else
            {
                Qweir = 3.0 * L * Math.Pow(dweir, 1.5);
            }
        }
        else
        {
            P = L + 1.8 * S.W;
            dweir = h + S.a;
            if (d < dweir)
            {
                Qw = 2.3 * P * Math.Pow(d, 1.5);
                return;
            }
            else
            {
                Qweir = 2.3 * P * Math.Pow(dweir, 1.5);
            }
        }
        Qorif = S.getCurbOrificeFlow(dorif, h, L, throatAngle);
        r = (d - dweir) / (dorif - dweir);
        Qw = (1.0 - r) * Qweir;
        Qo = r * Qorif;
    }

    // inlet.c:1764
    public static double getCurbOrificeFlow(double di, double h, double L, int throatAngle)
    {
        double d = di;
        if (throatAngle == S.HORIZONTAL_THROAT)
        {
            d = di - h / 2.0;
        }
        else if (throatAngle == S.INCLINED_THROAT)
        {
            d = di - (h / 2.0) * 0.7071;
        }
        return 0.67 * h * L * Math.Sqrt(2.0 * 32.16 * d);
    }

    // inlet.c:1785
    public static double getOnSagSlottedFlow(int i, double d)
    {
        double L = S.InletDesigns[i].slottedInlet.length;
        double w = S.InletDesigns[i].slottedInlet.width;
        if (d <= 2.587 * w)
        {
            return 2.48 * L * Math.Pow(d, 1.5);
        }
        else
        {
            return 0.8 * L * w * Math.Sqrt(64.32 * d);
        }
    }

    // inlet.c:1805
    public static void getBackflowRatios()
    {
        TInlet inlet = null;
        double area = 0.0;
        double f = 0.0;
        int n = 0;
        TInletNode[] inletNodes = R.NewArray<TInletNode>((int)(unchecked((ulong)(S.Nobjects[S.NODE]))));
        if (inletNodes == null)
        {
            return;
        }
        for (inlet = S.FirstInlet; inlet != null; inlet = inlet.nextInlet)
        {
            n = inlet.nodeIndex;
            inletNodes[n].numInletLinks++;
            area = S.getInletArea(inlet);
            if (area > 0.0)
            {
                inletNodes[n].numStdInletLinks++;
                inletNodes[n].totalInletArea += area;
            }
            else
            {
                inletNodes[n].numCustomInlets += inlet.numInlets;
            }
        }
        for (inlet = S.FirstInlet; inlet != null; inlet = inlet.nextInlet)
        {
            n = inlet.nodeIndex;
            f = (double)(inletNodes[n].numStdInletLinks) / (double)(inletNodes[n].numInletLinks);
            area = S.getInletArea(inlet);
            if (area == 0.0)
            {
                inlet.backflowRatio = (double)(inlet.numInlets) / (double)(inletNodes[n].numCustomInlets) * (1.0 - f);
            }
            else
            {
                inlet.backflowRatio = area / inletNodes[n].totalInletArea * f;
            }
        }
        R.free(inletNodes);
    }

    // inlet.c:1870
    public static double getInletArea(TInlet inlet)
    {
        double area = 0.0;
        double curbLength = 0.0;
        int i = inlet.designIndex;
        int grateType = S.InletDesigns[i].grateInlet.type;
        if (S.InletDesigns[i].grateInlet.length > 0.0)
        {
            area = S.InletDesigns[i].grateInlet.length * S.InletDesigns[i].grateInlet.width;
            if (grateType == S.GENERIC)
            {
                area *= S.InletDesigns[i].grateInlet.fracOpenArea;
            }
            else
            {
                area *= S.GrateOpeningRatios[grateType];
            }
        }
        curbLength = S.InletDesigns[i].curbInlet.length - S.InletDesigns[i].grateInlet.length;
        if (curbLength > 0.0)
        {
            area += curbLength * S.InletDesigns[i].curbInlet.height;
        }
        if (S.InletDesigns[i].slottedInlet.length > 0.0)
        {
            area = S.InletDesigns[i].slottedInlet.length * S.InletDesigns[i].slottedInlet.width;
        }
        return area * inlet.numInlets * inlet.clogFactor;
    }

    // inlet.c:1902
    public static double getCustomCapturedFlow(TInlet inlet, double q, double d)
    {
        int i = inlet.designIndex;
        int j = 0;
        int sides = 1;
        int c = 0;
        double qApproach = 0.0;
        double qBypassed = 0.0;
        double qCaptured = 0.0;
        double qIncrement = 0.0;
        double qMax = 10000000000.0;
        if (inlet.numInlets == 0)
        {
            return 0.0;
        }
        qMax = 10000000000.0;
        if (inlet.flowLimit > 0.0)
        {
            qMax = inlet.flowLimit;
        }
        S.xsect = S.Link[inlet.linkIndex].xsect;
        if (S.xsect.type == S.STREET_XSECT)
        {
            sides = S.Street[S.xsect.transect].sides;
        }
        qApproach = q / sides;
        qBypassed = qApproach;
        qCaptured = 0.0;
        c = S.InletDesigns[i].customCurve;
        if (c >= 0)
        {
            if (S.Curve[c].curveType == S.DIVERSION_CURVE)
            {
                for (j = 1; j <= inlet.numInlets; j++)
                {
                    qIncrement = inlet.clogFactor * S.table_lookupEx(S.Curve[c], qBypassed * S.UCF(S.FLOW)) / S.UCF(S.FLOW);
                    qIncrement = ((((qIncrement) <= (qMax)) ? (qIncrement) : (qMax)));
                    qIncrement = ((((qIncrement) <= (qBypassed)) ? (qIncrement) : (qBypassed)));
                    qCaptured += qIncrement;
                    qBypassed -= qIncrement;
                    if (qBypassed < 0.001)
                    {
                        break;
                    }
                }
            }
            else if (S.Curve[c].curveType == S.RATING_CURVE)
            {
                qCaptured = inlet.numInlets * inlet.clogFactor * S.table_lookupEx(S.Curve[c], d * S.UCF(S.LENGTH)) / S.UCF(S.FLOW);
            }
            qCaptured *= sides;
        }
        return qCaptured;
    }

}
}
