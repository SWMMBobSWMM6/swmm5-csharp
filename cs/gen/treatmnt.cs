// Generated from EPA SWMM 5.2.4 (treatmnt.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int PVMAX = 5;
    public static int ErrCode = 0;
    public static int J = 0;
    public static double Dt = 0.0;
    public static double Q = 0.0;
    public static double V = 0.0;
    public static double[] R_ = null;
    public static double[] Cin = null;

    // treatmnt.c:66
    public static int treatmnt_open()
    {
        S.R_ = (null);
        S.Cin = (null);
        if (S.Nobjects[S.POLLUT] > 0)
        {
            S.R_ = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Cin = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            if ((S.R_ == null) || (S.Cin == null))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
                return 0;
            }
        }
        return 1;
    }

    // treatmnt.c:90
    public static void treatmnt_close()
    {
        {
            if ((S.R_ != null))
            {
                R.free(S.R_);
                S.R_ = (null);
            }
        }
        ;
        {
            if ((S.Cin != null))
            {
                R.free(S.Cin);
                S.Cin = (null);
            }
        }
        ;
    }

    // treatmnt.c:103
    public static int treatmnt_readExpression(CPtr[] tok, int ntoks)
    {
        CPtr s = R.Buf(1025);
        CPtr expr = CPtr.Null;
        int i = 0;
        int j = 0;
        int k = 0;
        int p = 0;
        MathExpr equation = null;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.NODE, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        p = S.project_findObject(S.POLLUT, tok[1]);
        if (p < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        S.sstrncpy(s, tok[2], unchecked((ulong)(1024)));
        for (i = 3; i < ntoks; i++)
        {
            S.sstrcat(s, S.L33, unchecked((ulong)(1024)));
            S.sstrcat(s, tok[i], unchecked((ulong)(1024)));
        }
        if ((((((s[0]) >= 'a') && ((s[0]) <= 'z')) ? ((s[0]) & ~32) : (s[0]))) == 'R')
        {
            k = 0;
        }
        else if ((((((s[0]) >= 'a') && ((s[0]) <= 'z')) ? ((s[0]) & ~32) : (s[0]))) == 'C')
        {
            k = 1;
        }
        else
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[2]);
        }
        expr = R.strchr(s, '=');
        if (expr.IsNull)
        {
            return S.error_setInpError(S.ERR_KEYWORD, S.L0);
        }
        else
        {
            expr++;
        }
        if (S.Node[j].treatment == null)
        {
            if (!((S.createTreatment(j) != 0)))
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
        }
        equation = S.mathexpr_create(expr, S.treatmnt_getVariableIndex);
        if (equation == null)
        {
            return S.error_setInpError(S.ERR_MATH_EXPR, S.L0);
        }
        if (S.Node[j].treatment != null)
        {
            S.Node[j].treatment[p].treatType = k;
            S.Node[j].treatment[p].equation = equation;
        }
        return 0;
    }

    // treatmnt.c:165
    public static void treatmnt_delete(int j)
    {
        int p = 0;
        if ((S.Node[j].treatment != null))
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                S.mathexpr_delete(S.Node[j].treatment[p].equation);
            }
            R.free(S.Node[j].treatment);
        }
        S.Node[j].treatment = (null);
    }

    // treatmnt.c:184
    public static void treatmnt_setInflow(double qIn, double[] wIn)
    {
        int p = 0;
        if (qIn > 0.0)
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                S.Cin[p] = wIn[p] / qIn;
            }
        }
        else
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                S.Cin[p] = 0.0;
            }
        }
    }

    // treatmnt.c:202
    public static void treatmnt_treat(int j, double q, double v, double tStep)
    {
        int p = 0;
        double cOut = 0.0;
        double massLost = 0.0;
        TTreatment treatment = null;
        if (S.Node[j].treatment == null)
        {
            return;
        }
        S.ErrCode = 0;
        S.J = j;
        S.Dt = tStep;
        S.Q = q;
        S.V = v;
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.R_[p] = -1.0;
        }
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            treatment = S.Node[j].treatment[p];
            if (treatment.equation == null)
            {
                S.R_[p] = 0.0;
            }
            else if ((treatment.treatType == S.REMOVAL) && (q <= 1e-10))
            {
                S.R_[p] = 0.0;
            }
            else
            {
                S.getRemoval(p);
            }
        }
        if (S.ErrCode == S.ERR_CYCLIC_TREATMENT)
        {
            S.report_writeErrorMsg(S.ERR_CYCLIC_TREATMENT, S.Node[S.J].ID);
        }
        else
        {
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                if (S.R_[p] == 0.0)
                {
                    continue;
                }
                treatment = S.Node[j].treatment[p];
                if (treatment.treatType == S.REMOVAL)
                {
                    if (S.Cin[p] == 0.0)
                    {
                        cOut = S.Node[j].newQual[p];
                    }
                    else
                    {
                        cOut = (1.0 - S.R_[p]) * S.Cin[p];
                    }
                    cOut = ((((cOut) <= (S.Node[j].newQual[p])) ? (cOut) : (S.Node[j].newQual[p])));
                }
                else
                {
                    cOut = (1.0 - S.R_[p]) * S.Node[j].newQual[p];
                }
                massLost = (S.Cin[p] * q * tStep + S.Node[j].oldQual[p] * S.Node[j].oldVolume - cOut * (q * tStep + S.Node[j].oldVolume)) / tStep;
                massLost = ((((0.0) >= (massLost)) ? (0.0) : (massLost)));
                S.massbal_addReactedMass(p, massLost);
                S.Node[j].newQual[p] = cOut;
            }
        }
    }

    // treatmnt.c:288
    public static int createTreatment(int j)
    {
        int p = 0;
        S.Node[j].treatment = R.NewArray<TTreatment>((int)(unchecked((ulong)(S.Nobjects[S.POLLUT]))));
        if (S.Node[j].treatment == null)
        {
            return 0;
        }
        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
        {
            S.Node[j].treatment[p].equation = (null);
        }
        return 1;
    }

    // treatmnt.c:311
    public static int treatmnt_getVariableIndex(CPtr s)
    {
        int k = 0;
        int m = S.PVMAX;
        k = S.findmatch(s, S.ProcessVarWords);
        if (k >= 0)
        {
            return k;
        }
        k = S.project_findObject(S.POLLUT, s);
        if (k >= 0)
        {
            return (k + m);
        }
        if (((((((s[0]) >= 'a') && ((s[0]) <= 'z')) ? ((s[0]) & ~32) : (s[0]))) == 'R') && (s[1] == '_'))
        {
            k = S.project_findObject(S.POLLUT, (s + 2));
            if (k >= 0)
            {
                return (S.Nobjects[S.POLLUT] + k + m);
            }
        }
        return -1;
    }

    // treatmnt.c:340
    public static double treatmnt_getVariableValue(int varCode)
    {
        int p = 0;
        double a1 = 0.0;
        double a2 = 0.0;
        double y = 0.0;
        TTreatment treatment = null;
        if (varCode < S.PVMAX)
        {
            switch (varCode)
            {
                case S.pvHRT:
                    if (S.Node[S.J].type == S.STORAGE)
                    {
                        return S.Storage[S.Node[S.J].subIndex].hrt / 3600.0;
                    }
                    else
                    {
                        return 0.0;
                    }
                case S.pvDT:
                    return S.Dt;
                case S.pvFLOW:
                    return S.Q * S.UCF(S.FLOW);
                case S.pvDEPTH:
                    y = (S.Node[S.J].oldDepth + S.Node[S.J].newDepth) / 2.0;
                    return y * S.UCF(S.LENGTH);
                case S.pvAREA:
                    a1 = S.node_getSurfArea(S.J, S.Node[S.J].oldDepth);
                    a2 = S.node_getSurfArea(S.J, S.Node[S.J].newDepth);
                    return (a1 + a2) / 2.0 * S.UCF(S.LENGTH) * S.UCF(S.LENGTH);
                default:
                    return 0.0;
            }
        }
        else if (varCode < S.PVMAX + S.Nobjects[S.POLLUT])
        {
            p = varCode - S.PVMAX;
            treatment = S.Node[S.J].treatment[p];
            if (treatment.treatType == S.REMOVAL)
            {
                return S.Cin[p];
            }
            return S.Node[S.J].newQual[p];
        }
        else
        {
            p = varCode - S.PVMAX - S.Nobjects[S.POLLUT];
            if (p >= S.Nobjects[S.POLLUT])
            {
                return 0.0;
            }
            return S.getRemoval(p);
        }
    }

    // treatmnt.c:404
    public static double getRemoval(int p)
    {
        double c0 = S.Node[S.J].newQual[p];
        double r = 0.0;
        TTreatment treatment = null;
        if ((S.R_[p] > 1.0) || (S.ErrCode != 0))
        {
            S.ErrCode = 1;
            return 0.0;
        }
        if ((S.R_[p] >= 0.0) && (S.R_[p] <= 1.0))
        {
            return S.R_[p];
        }
        S.R_[p] = 10.0;
        if (c0 == 0.0)
        {
            S.R_[p] = 0.0;
            return 0.0;
        }
        treatment = S.Node[S.J].treatment[p];
        r = S.mathexpr_eval(treatment.equation, S.treatmnt_getVariableValue);
        r = ((((0.0) >= (r)) ? (0.0) : (r)));
        if (treatment.treatType == S.REMOVAL)
        {
            r = ((((1.0) <= (r)) ? (1.0) : (r)));
            S.R_[p] = r;
        }
        else
        {
            r = ((((c0) <= (r)) ? (c0) : (r)));
            S.R_[p] = 1.0 - r / c0;
        }
        return S.R_[p];
    }

}
}
