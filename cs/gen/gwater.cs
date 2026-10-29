// Generated from EPA SWMM 5.2.4 (gwater.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double GWTOL = 0.0001;
    public static double XTOL = 0.001;
    public static CPtr[] GWVarWords = new CPtr[] { S.L707, S.L708, S.L709, S.L710, S.L711, S.L712, S.L713, S.L714, S.L715, S.L716, S.L717, CPtr.Null };
    public static double Area = 0.0;
    public static double gwater_Infil = 0.0;
    public static double MaxEvap = 0.0;
    public static double AvailEvap = 0.0;
    public static double UpperEvap = 0.0;
    public static double LowerEvap = 0.0;
    public static double UpperPerc = 0.0;
    public static double LowerLoss = 0.0;
    public static double GWFlow = 0.0;
    public static double MaxUpperPerc = 0.0;
    public static double MaxGWFlowPos = 0.0;
    public static double MaxGWFlowNeg = 0.0;
    public static double FracPerv = 0.0;
    public static double TotalDepth = 0.0;
    public static double Theta = 0.0;
    public static double HydCon = 0.0;
    public static double Hgw = 0.0;
    public static double Hstar = 0.0;
    public static double Hsw = 0.0;
    public static double gwater_Tstep = 0.0;
    public static TAquifer A = new TAquifer();
    public static TGroundwater GW = null;
    public static MathExpr LatFlowExpr = null;
    public static MathExpr DeepFlowExpr = null;

    // gwater.c:119
    public static int gwater_readAquiferParams(int j, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int p = 0;
        double[] x = new double[12];
        CPtr id = CPtr.Null;
        if (ntoks < 13)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.AQUIFER, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        for (i = 0; i < 11; i++)
        {
            x[i] = 0.0;
        }
        for (i = 1; i < 13; i++)
        {
            if (!((S.getDouble(tok[i], ref x[i - 1]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i]);
            }
        }
        p = -1;
        if (ntoks > 13)
        {
            p = S.project_findObject(S.TIMEPATTERN, tok[13]);
            if (p < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[13]);
            }
        }
        S.Aquifer[j].ID = id;
        S.Aquifer[j].porosity = x[0];
        S.Aquifer[j].wiltingPoint = x[1];
        S.Aquifer[j].fieldCapacity = x[2];
        S.Aquifer[j].conductivity = x[3] / S.UCF(S.RAINFALL);
        S.Aquifer[j].conductSlope = x[4];
        S.Aquifer[j].tensionSlope = x[5] / S.UCF(S.LENGTH);
        S.Aquifer[j].upperEvapFrac = x[6];
        S.Aquifer[j].lowerEvapDepth = x[7] / S.UCF(S.LENGTH);
        S.Aquifer[j].lowerLossCoeff = x[8] / S.UCF(S.RAINFALL);
        S.Aquifer[j].bottomElev = x[9] / S.UCF(S.LENGTH);
        S.Aquifer[j].waterTableElev = x[10] / S.UCF(S.LENGTH);
        S.Aquifer[j].upperMoisture = x[11];
        S.Aquifer[j].upperEvapPat = p;
        return 0;
    }

    // gwater.c:179
    public static int gwater_readGroundwaterParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int m = 0;
        int n = 0;
        double[] x = new double[11];
        TGroundwater gw = null;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.SUBCATCH, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        if (ntoks < 11)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        k = S.project_findObject(S.AQUIFER, tok[1]);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        n = S.project_findObject(S.NODE, tok[2]);
        if (n < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        for (i = 0; i < 7; i++)
        {
            if (!((S.getDouble(tok[i + 3], ref x[i]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i + 3]);
            }
        }
        for (i = 7; i < 11; i++)
        {
            x[i] = -10000000000.0;
            m = i + 3;
            if ((ntoks > m) && (tok[m][0] != '*'))
            {
                if (!((S.getDouble(tok[m], ref x[i]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[m]);
                }
                if (i < 10)
                {
                    x[i] /= S.UCF(S.LENGTH);
                }
            }
        }
        if (!(S.Subcatch[j].groundwater != null))
        {
            gw = new TGroundwater();
            if (!(gw != null))
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
            S.Subcatch[j].groundwater = gw;
        }
        else
        {
            gw = S.Subcatch[j].groundwater;
        }
        gw.aquifer = k;
        gw.node = n;
        gw.surfElev = x[0] / S.UCF(S.LENGTH);
        gw.a1 = x[1];
        gw.b1 = x[2];
        gw.a2 = x[3];
        gw.b2 = x[4];
        gw.a3 = x[5];
        gw.fixedDepth = x[6] / S.UCF(S.LENGTH);
        gw.nodeElev = x[7];
        gw.bottomElev = x[8];
        gw.waterTableElev = x[9];
        gw.upperMoisture = x[10];
        return 0;
    }

    // gwater.c:258
    public static int gwater_readFlowExpression(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        CPtr exprStr = R.Buf(1025);
        MathExpr expr = null;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.SUBCATCH, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        k = 1;
        if ((S.match(tok[1], S.L158) != 0))
        {
            k = 1;
        }
        else if ((S.match(tok[1], S.L159) != 0))
        {
            k = 2;
        }
        else
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        S.sstrncpy(exprStr, tok[2], unchecked((ulong)(1024)));
        for (i = 3; i < ntoks; i++)
        {
            S.sstrcat(exprStr, S.L33, unchecked((ulong)(1024 + 1)));
            S.sstrcat(exprStr, tok[i], unchecked((ulong)(1024 + 1)));
        }
        if (k == 1)
        {
            S.mathexpr_delete(S.Subcatch[j].gwLatFlowExpr);
        }
        else
        {
            S.mathexpr_delete(S.Subcatch[j].gwDeepFlowExpr);
        }
        expr = S.mathexpr_create(exprStr, S.gwater_getVariableIndex);
        if (expr == null)
        {
            return S.error_setInpError(S.ERR_MATH_EXPR, S.L0);
        }
        if (k == 1)
        {
            S.Subcatch[j].gwLatFlowExpr = expr;
        }
        else
        {
            S.Subcatch[j].gwDeepFlowExpr = expr;
        }
        return 0;
    }

    // gwater.c:315
    public static void gwater_deleteFlowExpression(int j)
    {
        S.mathexpr_delete(S.Subcatch[j].gwLatFlowExpr);
        S.mathexpr_delete(S.Subcatch[j].gwDeepFlowExpr);
    }

    // gwater.c:328
    public static void gwater_validateAquifer(int j)
    {
        int p = 0;
        if (((((((((((S.Aquifer[j].porosity <= 0.0) || (S.Aquifer[j].fieldCapacity >= S.Aquifer[j].porosity)) || (S.Aquifer[j].wiltingPoint >= S.Aquifer[j].fieldCapacity)) || (S.Aquifer[j].conductivity <= 0.0)) || (S.Aquifer[j].conductSlope < 0.0)) || (S.Aquifer[j].tensionSlope < 0.0)) || (S.Aquifer[j].upperEvapFrac < 0.0)) || (S.Aquifer[j].lowerEvapDepth < 0.0)) || (S.Aquifer[j].waterTableElev < S.Aquifer[j].bottomElev)) || (S.Aquifer[j].upperMoisture > S.Aquifer[j].porosity)) || (S.Aquifer[j].upperMoisture < S.Aquifer[j].wiltingPoint))
        {
            S.report_writeErrorMsg(S.ERR_AQUIFER_PARAMS, S.Aquifer[j].ID);
        }
        p = S.Aquifer[j].upperEvapPat;
        if ((p >= 0) && (S.Pattern[p].type != S.MONTHLY_PATTERN))
        {
            S.report_writeErrorMsg(S.ERR_AQUIFER_PARAMS, S.Aquifer[j].ID);
        }
    }

    // gwater.c:359
    public static void gwater_validate(int j)
    {
        TAquifer a = new TAquifer();
        TGroundwater gw = null;
        gw = S.Subcatch[j].groundwater;
        if ((gw != null))
        {
            a = S.Aquifer[gw.aquifer].Clone();
            if (gw.bottomElev == -10000000000.0)
            {
                gw.bottomElev = a.bottomElev;
            }
            if (gw.waterTableElev == -10000000000.0)
            {
                gw.waterTableElev = a.waterTableElev;
            }
            if (gw.upperMoisture == -10000000000.0)
            {
                gw.upperMoisture = a.upperMoisture;
            }
            if (gw.surfElev < gw.waterTableElev)
            {
                S.report_writeErrorMsg(S.ERR_GROUND_ELEV, S.Subcatch[j].ID);
            }
        }
    }

    // gwater.c:382
    public static void gwater_initState(int j)
    {
        TAquifer a = new TAquifer();
        TGroundwater gw = null;
        gw = S.Subcatch[j].groundwater;
        if ((gw != null))
        {
            a = S.Aquifer[gw.aquifer].Clone();
            gw.theta = gw.upperMoisture;
            if (gw.theta >= a.porosity)
            {
                gw.theta = a.porosity - S.XTOL;
            }
            gw.lowerDepth = gw.waterTableElev - gw.bottomElev;
            if (gw.lowerDepth >= gw.surfElev - gw.bottomElev)
            {
                gw.lowerDepth = gw.surfElev - gw.bottomElev - S.XTOL;
            }
            gw.oldFlow = 0.0;
            gw.newFlow = 0.0;
            gw.evapLoss = 0.0;
            gw.maxInfilVol = (gw.surfElev - gw.waterTableElev) * (a.porosity - gw.theta) / S.subcatch_getFracPerv(j);
        }
    }

    // gwater.c:425
    public static void gwater_getState(int j, double[] x)
    {
        TGroundwater gw = S.Subcatch[j].groundwater;
        x[0] = gw.theta;
        x[1] = gw.bottomElev + gw.lowerDepth;
        x[2] = gw.newFlow;
        x[3] = gw.maxInfilVol;
    }

    // gwater.c:441
    public static void gwater_setState(int j, double[] x)
    {
        TGroundwater gw = S.Subcatch[j].groundwater;
        if (gw == null)
        {
            return;
        }
        gw.theta = x[0];
        gw.lowerDepth = x[1] - gw.bottomElev;
        gw.oldFlow = x[2];
        if (x[3] != -10000000000.0)
        {
            gw.maxInfilVol = x[3];
        }
    }

    // gwater.c:458
    public static double gwater_getVolume(int j)
    {
        TAquifer a = new TAquifer();
        TGroundwater gw = null;
        double upperDepth = 0.0;
        gw = S.Subcatch[j].groundwater;
        if (gw == null)
        {
            return 0.0;
        }
        a = S.Aquifer[gw.aquifer].Clone();
        upperDepth = gw.surfElev - gw.bottomElev - gw.lowerDepth;
        return (upperDepth * gw.theta) + (gw.lowerDepth * a.porosity);
    }

    // gwater.c:477
    public static void gwater_getGroundwater(int j, double evap, double infil, double tStep)
    {
        int n = 0;
        double[] x = new double[2];
        double vUpper = 0.0;
        double nodeFlow = 0.0;
        S.GW = S.Subcatch[j].groundwater;
        if (S.GW == null)
        {
            return;
        }
        S.LatFlowExpr = S.Subcatch[j].gwLatFlowExpr;
        S.DeepFlowExpr = S.Subcatch[j].gwDeepFlowExpr;
        S.A = S.Aquifer[S.GW.aquifer].Clone();
        S.FracPerv = S.subcatch_getFracPerv(j);
        if (S.FracPerv <= 0.0)
        {
            return;
        }
        S.Area = S.Subcatch[j].area;
        infil = infil / S.Area / tStep;
        S.gwater_Infil = infil;
        S.gwater_Tstep = tStep;
        evap = evap / S.Area / tStep;
        S.MaxEvap = S.Evap.rate * S.FracPerv;
        S.AvailEvap = (((((S.MaxEvap - evap)) >= (0.0)) ? ((S.MaxEvap - evap)) : (0.0)));
        S.TotalDepth = S.GW.surfElev - S.GW.bottomElev;
        if (S.TotalDepth <= 0.0)
        {
            return;
        }
        n = S.GW.node;
        if (S.GW.nodeElev != -10000000000.0)
        {
            S.Hstar = S.GW.nodeElev - S.GW.bottomElev;
        }
        else
        {
            S.Hstar = S.Node[n].invertElev - S.GW.bottomElev;
        }
        if (S.GW.fixedDepth > 0.0)
        {
            S.Hsw = S.GW.fixedDepth + S.Node[n].invertElev - S.GW.bottomElev;
        }
        else
        {
            S.Hsw = S.Node[n].newDepth + S.Node[n].invertElev - S.GW.bottomElev;
        }
        x[S.THETA] = S.GW.theta;
        x[S.LOWERDEPTH] = S.GW.lowerDepth;
        vUpper = (S.TotalDepth - x[S.LOWERDEPTH]) * (x[S.THETA] - S.A.fieldCapacity);
        vUpper = ((((0.0) >= (vUpper)) ? (0.0) : (vUpper)));
        S.MaxUpperPerc = vUpper / tStep;
        S.MaxGWFlowPos = x[S.LOWERDEPTH] * S.A.porosity / tStep;
        S.MaxGWFlowNeg = (S.TotalDepth - x[S.LOWERDEPTH]) * (S.A.porosity - x[S.THETA]) / tStep;
        nodeFlow = (S.Node[n].inflow + S.Node[n].newVolume / tStep) / S.Area;
        S.MaxGWFlowNeg = -(((((S.MaxGWFlowNeg) <= (nodeFlow)) ? (S.MaxGWFlowNeg) : (nodeFlow))));
        S.odesolve_integrate(x, 2, 0, tStep, S.GWTOL, tStep, S.getDxDt);
        x[S.THETA] = ((((x[S.THETA]) >= (S.A.wiltingPoint)) ? (x[S.THETA]) : (S.A.wiltingPoint)));
        if (x[S.THETA] >= S.A.porosity)
        {
            x[S.THETA] = S.A.porosity - S.XTOL;
            x[S.LOWERDEPTH] = S.TotalDepth - S.XTOL;
        }
        x[S.LOWERDEPTH] = ((((x[S.LOWERDEPTH]) >= (0.0)) ? (x[S.LOWERDEPTH]) : (0.0)));
        if (x[S.LOWERDEPTH] >= S.TotalDepth)
        {
            x[S.LOWERDEPTH] = S.TotalDepth - S.XTOL;
        }
        S.GW.theta = x[S.THETA];
        S.GW.lowerDepth = x[S.LOWERDEPTH];
        S.getFluxes(S.GW.theta, S.GW.lowerDepth);
        S.GW.oldFlow = S.GW.newFlow;
        S.GW.newFlow = S.GWFlow;
        S.GW.evapLoss = S.UpperEvap + S.LowerEvap;
        S.GW.maxInfilVol = (S.TotalDepth - x[S.LOWERDEPTH]) * (S.A.porosity - x[S.THETA]) / S.FracPerv;
        S.updateMassBal(S.Area, tStep);
        S.stats_updateGwaterStats(j, infil, S.GW.evapLoss, S.GWFlow, S.LowerLoss, S.GW.theta, S.GW.lowerDepth + S.GW.bottomElev, tStep);
    }

    // gwater.c:605
    public static void updateMassBal(double area, double tStep)
    {
        double vInfil = 0.0;
        double vUpperEvap = 0.0;
        double vLowerEvap = 0.0;
        double vLowerPerc = 0.0;
        double vGwater = 0.0;
        double ft2sec = area * tStep;
        vInfil = S.gwater_Infil * ft2sec;
        vUpperEvap = S.UpperEvap * ft2sec;
        vLowerEvap = S.LowerEvap * ft2sec;
        vLowerPerc = S.LowerLoss * ft2sec;
        vGwater = 0.5 * (S.GW.oldFlow + S.GW.newFlow) * ft2sec;
        S.massbal_updateGwaterTotals(vInfil, vUpperEvap, vLowerEvap, vLowerPerc, vGwater);
    }

    // gwater.c:631
    public static void getFluxes(double theta, double lowerDepth)
    {
        double upperDepth = 0.0;
        lowerDepth = ((((lowerDepth) >= (0.0)) ? (lowerDepth) : (0.0)));
        lowerDepth = ((((lowerDepth) <= (S.TotalDepth)) ? (lowerDepth) : (S.TotalDepth)));
        upperDepth = S.TotalDepth - lowerDepth;
        S.Hgw = lowerDepth;
        S.Theta = theta;
        S.gwater_getEvapRates(theta, upperDepth);
        S.UpperPerc = S.getUpperPerc(theta, upperDepth);
        S.UpperPerc = ((((S.UpperPerc) <= (S.MaxUpperPerc)) ? (S.UpperPerc) : (S.MaxUpperPerc)));
        if (S.DeepFlowExpr != null)
        {
            S.LowerLoss = S.mathexpr_eval(S.DeepFlowExpr, S.gwater_getVariableValue) / S.UCF(S.RAINFALL);
        }
        else
        {
            S.LowerLoss = S.A.lowerLossCoeff * lowerDepth / S.TotalDepth;
        }
        S.LowerLoss = ((((S.LowerLoss) <= (lowerDepth / S.gwater_Tstep)) ? (S.LowerLoss) : (lowerDepth / S.gwater_Tstep)));
        S.GWFlow = S.getGWFlow(lowerDepth);
        if (S.LatFlowExpr != null)
        {
            S.GWFlow += S.mathexpr_eval(S.LatFlowExpr, S.gwater_getVariableValue) / S.UCF(S.GWFLOW);
        }
        if (S.GWFlow >= 0.0)
        {
            S.GWFlow = ((((S.GWFlow) <= (S.MaxGWFlowPos)) ? (S.GWFlow) : (S.MaxGWFlowPos)));
        }
        else
        {
            S.GWFlow = ((((S.GWFlow) >= (S.MaxGWFlowNeg)) ? (S.GWFlow) : (S.MaxGWFlowNeg)));
        }
    }

    // gwater.c:677
    public static void getDxDt(double t, double[] x, double[] dxdt)
    {
        double qUpper = 0.0;
        double qLower = 0.0;
        double denom = 0.0;
        S.getFluxes(x[S.THETA], x[S.LOWERDEPTH]);
        qUpper = S.gwater_Infil - S.UpperEvap - S.UpperPerc;
        qLower = S.UpperPerc - S.LowerLoss - S.LowerEvap - S.GWFlow;
        denom = S.TotalDepth - x[S.LOWERDEPTH];
        if (denom > 0.0)
        {
            dxdt[S.THETA] = qUpper / denom;
        }
        else
        {
            dxdt[S.THETA] = 0.0;
        }
        denom = S.A.porosity - x[S.THETA];
        if (denom > 0.0)
        {
            dxdt[S.LOWERDEPTH] = qLower / denom;
        }
        else
        {
            dxdt[S.LOWERDEPTH] = 0.0;
        }
    }

    // gwater.c:713
    public static void gwater_getEvapRates(double theta, double upperDepth)
    {
        int p = 0;
        int month = 0;
        double f = 0.0;
        double lowerFrac = 0.0;
        double upperFrac = 0.0;
        S.UpperEvap = 0.0;
        S.LowerEvap = 0.0;
        if (S.gwater_Infil > 0.0)
        {
            return;
        }
        upperFrac = S.A.upperEvapFrac;
        f = 1.0;
        p = S.A.upperEvapPat;
        if (p >= 0)
        {
            month = S.datetime_monthOfYear(S.getDateTime(S.NewRunoffTime));
            f = S.Pattern[p].factor[month - 1];
        }
        upperFrac *= f;
        if (theta > S.A.wiltingPoint)
        {
            S.UpperEvap = upperFrac * S.MaxEvap;
            S.UpperEvap = ((((S.UpperEvap) <= (S.AvailEvap)) ? (S.UpperEvap) : (S.AvailEvap)));
        }
        if (S.A.lowerEvapDepth > 0.0)
        {
            lowerFrac = (S.A.lowerEvapDepth - upperDepth) / S.A.lowerEvapDepth;
            lowerFrac = ((((0.0) >= (lowerFrac)) ? (0.0) : (lowerFrac)));
            lowerFrac = ((((lowerFrac) <= (1.0)) ? (lowerFrac) : (1.0)));
            S.LowerEvap = lowerFrac * (1.0 - upperFrac) * S.MaxEvap;
            S.LowerEvap = ((((S.LowerEvap) <= ((S.AvailEvap - S.UpperEvap))) ? (S.LowerEvap) : ((S.AvailEvap - S.UpperEvap))));
        }
    }

    // gwater.c:769
    public static double getUpperPerc(double theta, double upperDepth)
    {
        double delta = 0.0;
        double dhdz = 0.0;
        double hydcon = 0.0;
        if ((upperDepth <= 0.0) || (theta <= S.A.fieldCapacity))
        {
            return 0.0;
        }
        delta = theta - S.A.porosity;
        hydcon = S.A.conductivity * Math.Exp(delta * S.A.conductSlope);
        delta = theta - S.A.fieldCapacity;
        dhdz = 1.0 + S.A.tensionSlope * 2.0 * delta / upperDepth;
        S.HydCon = hydcon;
        return hydcon * dhdz;
    }

    // gwater.c:799
    public static double getGWFlow(double lowerDepth)
    {
        double q = 0.0;
        double t1 = 0.0;
        double t2 = 0.0;
        double t3 = 0.0;
        if (lowerDepth <= S.Hstar)
        {
            return 0.0;
        }
        if (S.GW.b1 == 0.0)
        {
            t1 = S.GW.a1;
        }
        else
        {
            t1 = S.GW.a1 * Math.Pow((lowerDepth - S.Hstar) * S.UCF(S.LENGTH), S.GW.b1);
        }
        if (S.GW.b2 == 0.0)
        {
            t2 = S.GW.a2;
        }
        else if (S.Hsw > S.Hstar)
        {
            t2 = S.GW.a2 * Math.Pow((S.Hsw - S.Hstar) * S.UCF(S.LENGTH), S.GW.b2);
        }
        else
        {
            t2 = 0.0;
        }
        t3 = S.GW.a3 * lowerDepth * S.Hsw * S.UCF(S.LENGTH) * S.UCF(S.LENGTH);
        q = (t1 - t2 + t3) / S.UCF(S.GWFLOW);
        if ((q < 0.0) && (S.GW.a3 != 0.0))
        {
            q = 0.0;
        }
        return q;
    }

    // gwater.c:834
    public static int gwater_getVariableIndex(CPtr s)
    {
        int k = 0;
        k = S.findmatch(s, S.GWVarWords);
        if (k >= 0)
        {
            return k;
        }
        return -1;
    }

    // gwater.c:850
    public static double gwater_getVariableValue(int varIndex)
    {
        switch (varIndex)
        {
            case S.gwvHGW:
                return S.Hgw * S.UCF(S.LENGTH);
            case S.gwvHSW:
                return S.Hsw * S.UCF(S.LENGTH);
            case S.gwvHCB:
                return S.Hstar * S.UCF(S.LENGTH);
            case S.gwvHGS:
                return S.TotalDepth * S.UCF(S.LENGTH);
            case S.gwvKS:
                return S.A.conductivity * S.UCF(S.RAINFALL);
            case S.gwvK:
                return S.HydCon * S.UCF(S.RAINFALL);
            case S.gwvTHETA:
                return S.Theta;
            case S.gwvPHI:
                return S.A.porosity;
            case S.gwvFI:
                return S.gwater_Infil * S.UCF(S.RAINFALL);
            case S.gwvFU:
                return S.UpperPerc * S.UCF(S.RAINFALL);
            case S.gwvA:
                return S.Area * S.UCF(S.LANDAREA);
            default:
                return 0.0;
        }
    }

}
}
