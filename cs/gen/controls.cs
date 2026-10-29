// Generated from EPA SWMM 5.2.4 (controls.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static CPtr[] ObjectWords = new CPtr[] { S.L656, S.L657, S.L658, S.L659, S.L660, S.L661, S.L662, S.L663, S.L32, CPtr.Null };
    public static CPtr[] AttribWords = new CPtr[] { S.L664, S.L665, S.L314, S.L666, S.L667, S.L183, S.L668, S.L669, S.L670, S.L671, S.L672, S.L673, S.L674, S.L675, S.L676, S.L677, S.L21, S.L678, S.L679, S.L680, S.L681, CPtr.Null };
    public static CPtr[] RelOpWords = new CPtr[] { S.L31, S.L682, S.L683, S.L684, S.L685, S.L686, CPtr.Null };
    public static CPtr[] StatusWords = new CPtr[] { S.L687, S.L688, CPtr.Null };
    public static CPtr[] ConduitWords = new CPtr[] { S.L689, S.L690, CPtr.Null };
    public static CPtr[] SettingTypeWords = new CPtr[] { S.L691, S.L692, S.L693, CPtr.Null };
    public static CPtr IntensityWord = S.L694;
    public static TRule[] Rules = null;
    public static TActionList ActionList = null;
    public static int InputState = 0;
    public static int RuleCount = 0;
    public static double ControlValue = 0.0;
    public static double SetPoint = 0.0;
    public static double CurrentDate = 0.0;
    public static double CurrentTime = 0.0;
    public static int VariableCount = 0;
    public static int ExpressionCount = 0;
    public static int CurrentVariable = 0;
    public static int CurrentExpression = 0;
    public static TNamedVariable[] NamedVariable = null;
    public static TExpression[] Expression = null;

    // controls.c:226
    public static void controls_init()
    {
        S.Rules = (null);
        S.NamedVariable = (null);
        S.Expression = (null);
        S.RuleCount = 0;
        S.VariableCount = 0;
        S.ExpressionCount = 0;
    }

    // controls.c:243
    public static void controls_addToCount(CPtr s)
    {
        if ((S.match(s, S.L29) != 0))
        {
            S.VariableCount++;
        }
        else if ((S.match(s, S.L30) != 0))
        {
            S.ExpressionCount++;
        }
    }

    // controls.c:257
    public static int controls_create(int n)
    {
        int r = 0;
        S.ActionList = (null);
        S.InputState = S.r_PRIORITY;
        S.RuleCount = n;
        if (S.RuleCount > 0)
        {
            S.Rules = R.NewArray<TRule>((int)(unchecked((ulong)(S.RuleCount))));
            if (S.Rules == null)
            {
                return S.ERR_MEMORY;
            }
            for (r = 0; r < S.RuleCount; r++)
            {
                S.Rules[r].ID = CPtr.Null;
                S.Rules[r].firstPremise = (null);
                S.Rules[r].lastPremise = (null);
                S.Rules[r].thenActions = (null);
                S.Rules[r].elseActions = (null);
                S.Rules[r].priority = 0.0;
            }
        }
        S.CurrentVariable = -1;
        S.CurrentExpression = -1;
        if (S.VariableCount > 0)
        {
            S.NamedVariable = R.NewArray<TNamedVariable>((int)(unchecked((ulong)(S.VariableCount))));
            if (S.NamedVariable == null)
            {
                return S.ERR_MEMORY;
            }
        }
        if (S.ExpressionCount > 0)
        {
            S.Expression = R.NewArray<TExpression>((int)(unchecked((ulong)(S.ExpressionCount))));
            if (S.Expression == null)
            {
                return S.ERR_MEMORY;
            }
        }
        return 0;
    }

    // controls.c:302
    public static void controls_delete()
    {
        int i = 0;
        for (i = 0; i < S.ExpressionCount; i++)
        {
            S.mathexpr_delete(S.Expression[i].expression);
            S.Expression[i].expression = (null);
        }
        {
            if ((S.Expression != null))
            {
                R.free(S.Expression);
                S.Expression = (null);
            }
        }
        ;
        {
            if ((S.NamedVariable != null))
            {
                R.free(S.NamedVariable);
                S.NamedVariable = (null);
            }
        }
        ;
        if (S.RuleCount == 0)
        {
            return;
        }
        S.deleteActionList();
        S.deleteRules();
    }

    // controls.c:326
    public static int controls_addVariable(CPtr[] tok, int nToks)
    {
        TVariable v1 = new TVariable();
        int k = 0;
        int err = 0;
        S.CurrentVariable++;
        if (nToks < 5)
        {
            return S.ERR_ITEMS;
        }
        if (S.findExactMatch(tok[1], S.AttribWords) >= 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        if (!((S.match(tok[2], S.L31) != 0)))
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[2]);
        }
        if ((!((S.match(tok[3], S.L32) != 0))) && (nToks < 6))
        {
            return S.ERR_ITEMS;
        }
        k = 3;
        err = S.getPremiseVariable(tok, nToks, ref k, v1);
        if (err > 0)
        {
            return err;
        }
        k = S.CurrentVariable;
        S.NamedVariable[k].variable = v1.Clone();
        S.sstrncpy(S.NamedVariable[k].name, tok[1], unchecked((ulong)(32)));
        return 0;
    }

    // controls.c:357
    public static int controls_addExpression(CPtr[] tok, int nToks)
    {
        int i = 0;
        int k = 0;
        CPtr s = R.Buf(1025);
        MathExpr expr = null;
        S.CurrentExpression++;
        if (nToks < 4)
        {
            return S.ERR_ITEMS;
        }
        k = S.CurrentExpression;
        S.Expression[k].expression = (null);
        S.sstrncpy(S.Expression[k].name, tok[1], unchecked((ulong)(32)));
        S.sstrncpy(s, tok[3], unchecked((ulong)(1024)));
        for (i = 4; i < nToks; i++)
        {
            S.sstrcat(s, S.L33, unchecked((ulong)(1024)));
            S.sstrcat(s, tok[i], unchecked((ulong)(1024)));
        }
        expr = S.mathexpr_create(s, S.getVariableIndex);
        if (expr == null)
        {
            return S.error_setInpError(S.ERR_MATH_EXPR, S.L0);
        }
        S.Expression[k].expression = expr;
        return 0;
    }

    // controls.c:393
    public static int getVariableIndex(CPtr varName)
    {
        int i = 0;
        for (i = 0; i < S.VariableCount; i++)
        {
            if ((S.match(varName, S.NamedVariable[i].name) != 0))
            {
                return i;
            }
        }
        return -1;
    }

    // controls.c:410
    public static double getNamedVariableValue(int varIndex)
    {
        return S.getVariableValue(S.NamedVariable[varIndex].variable);
    }

    // controls.c:422
    public static int getExpressionIndex(CPtr exprName)
    {
        int i = 0;
        for (i = 0; i < S.ExpressionCount; i++)
        {
            if ((S.match(exprName, S.Expression[i].name) != 0))
            {
                return i;
            }
        }
        return -1;
    }

    // controls.c:439
    public static int controls_addRuleClause(int r, int keyword, CPtr[] tok, int nToks)
    {
        switch (keyword)
        {
            case S.r_RULE:
                if (S.Rules[r].ID.IsNull)
                {
                    S.Rules[r].ID = S.project_findID(S.CONTROL, tok[1]);
                }
                S.InputState = S.r_RULE;
                if (nToks > 2)
                {
                    return S.ERR_RULE;
                }
                return 0;
            case S.r_IF:
                if (S.InputState != S.r_RULE)
                {
                    return S.ERR_RULE;
                }
                S.InputState = S.r_IF;
                return S.addPremise(r, S.r_AND, tok, nToks);
            case S.r_AND:
                if (S.InputState == S.r_IF)
                {
                    return S.addPremise(r, S.r_AND, tok, nToks);
                }
                else if ((S.InputState == S.r_THEN) || (S.InputState == S.r_ELSE))
                {
                    return S.addAction(r, tok, nToks);
                }
                else
                {
                    return S.ERR_RULE;
                }
            case S.r_OR:
                if (S.InputState != S.r_IF)
                {
                    return S.ERR_RULE;
                }
                return S.addPremise(r, S.r_OR, tok, nToks);
            case S.r_THEN:
                if (S.InputState != S.r_IF)
                {
                    return S.ERR_RULE;
                }
                S.InputState = S.r_THEN;
                return S.addAction(r, tok, nToks);
            case S.r_ELSE:
                if (S.InputState != S.r_THEN)
                {
                    return S.ERR_RULE;
                }
                S.InputState = S.r_ELSE;
                return S.addAction(r, tok, nToks);
            case S.r_PRIORITY:
                if ((S.InputState != S.r_THEN) && (S.InputState != S.r_ELSE))
                {
                    return S.ERR_RULE;
                }
                S.InputState = S.r_PRIORITY;
                if (!((S.getDouble(tok[1], ref S.Rules[r].priority) != 0)))
                {
                    return S.ERR_NUMBER;
                }
                if (nToks > 2)
                {
                    return S.ERR_RULE;
                }
                return 0;
        }
        return 0;
    }

    // controls.c:495
    public static int controls_evaluate(double currentTime, double elapsedTime, double tStep)
    {
        int r = 0;
        int result = 0;
        TPremise p = null;
        TAction a = null;
        S.CurrentDate = Math.Floor(currentTime);
        S.CurrentTime = currentTime - Math.Floor(currentTime);
        S.ElapsedTime = elapsedTime;
        if (S.RuleCount == 0)
        {
            return 0;
        }
        S.clearActionList();
        for (r = 0; r < S.RuleCount; r++)
        {
            result = 1;
            p = S.Rules[r].firstPremise;
            while ((p != null))
            {
                if (p.type == S.r_OR)
                {
                    if (result == 0)
                    {
                        result = S.evaluatePremise(p, tStep);
                    }
                }
                else
                {
                    if (result == 0)
                    {
                        break;
                    }
                    result = S.evaluatePremise(p, tStep);
                }
                p = p.next;
            }
            if (result == 1)
            {
                a = S.Rules[r].thenActions;
            }
            else
            {
                a = S.Rules[r].elseActions;
            }
            while ((a != null))
            {
                S.updateActionValue(a, currentTime, tStep);
                S.updateActionList(a);
                a = a.next;
            }
        }
        if ((S.ActionList != null))
        {
            return S.executeActionList(currentTime);
        }
        else
        {
            return 0;
        }
    }

    // controls.c:556
    public static int addPremise(int r, int type, CPtr[] tok, int nToks)
    {
        int relation = 0;
        int n = 0;
        int err = 0;
        double value = -10000000000.0;
        TPremise p = null;
        TVariable v1 = new TVariable();
        TVariable v2 = new TVariable();
        int obj = 0;
        int exprIndex = 0;
        int varIndex = -1;
        if (nToks < 4)
        {
            return S.ERR_ITEMS;
        }
        v1.attribute = -1;
        v1.@object = -1;
        v1.index = -1;
        n = 1;
        exprIndex = S.getExpressionIndex(tok[1]);
        if (exprIndex < 0)
        {
            varIndex = S.getVariableIndex(tok[n]);
            if (varIndex >= 0)
            {
                v1 = S.NamedVariable[varIndex].variable.Clone();
            }
            else
            {
                err = S.getPremiseVariable(tok, nToks, ref n, v1);
                if (err > 0)
                {
                    return err;
                }
            }
        }
        n++;
        if (n >= nToks)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        relation = S.findExactMatch(tok[n], S.RelOpWords);
        if (relation < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
        }
        v2.attribute = -1;
        v2.@object = -1;
        v2.index = -1;
        n++;
        if (n >= nToks)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        varIndex = S.getVariableIndex(tok[n]);
        if (varIndex >= 0)
        {
            v2 = S.NamedVariable[varIndex].variable.Clone();
        }
        else
        {
            obj = S.findmatch(tok[n], S.ObjectWords);
            if (obj >= 0)
            {
                err = S.getPremiseVariable(tok, nToks, ref n, v2);
                if (err > 0)
                {
                    return S.ERR_RULE;
                }
                if ((exprIndex < 0) && (v1.attribute != v2.attribute))
                {
                    S.report_writeWarningMsg(S.L34, S.Rules[r].ID);
                }
            }
            else
            {
                err = S.getPremiseValue(tok[n], v1.attribute, ref value);
                if (err > 0)
                {
                    return err;
                }
            }
        }
        n++;
        if ((n < nToks) && (S.findmatch(tok[n], S.RuleKeyWords) >= 0))
        {
            return S.ERR_RULE;
        }
        p = new TPremise();
        if (!(p != null))
        {
            return S.ERR_MEMORY;
        }
        p.type = type;
        p.exprIndex = exprIndex;
        p.lhsVar = v1.Clone();
        p.rhsVar = v2.Clone();
        p.relation = relation;
        p.value = value;
        p.next = (null);
        if (S.Rules[r].firstPremise == null)
        {
            S.Rules[r].firstPremise = p;
        }
        else
        {
            S.Rules[r].lastPremise.next = p;
        }
        S.Rules[r].lastPremise = p;
        return 0;
    }

    // controls.c:668
    public static int getPremiseVariable(CPtr[] tok, int nToks, ref int k, TVariable v)
    {
        int n = k;
        int @object = -1;
        int index = -1;
        int obj = 0;
        int attrib = 0;
        obj = S.findmatch(tok[n], S.ObjectWords);
        if (obj < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
        }
        n++;
        if (n >= nToks)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        switch (obj)
        {
            case S.r_GAGE:
                index = S.project_findObject(S.GAGE, tok[n]);
                if (index < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[n]);
                }
                @object = S.r_GAGE;
                break;
            case S.r_NODE:
                index = S.project_findObject(S.NODE, tok[n]);
                if (index < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[n]);
                }
                @object = S.r_NODE;
                break;
            case S.r_LINK:
            case S.r_CONDUIT:
            case S.r_PUMP:
            case S.r_ORIFICE:
            case S.r_WEIR:
            case S.r_OUTLET:
                index = S.project_findObject(S.LINK, tok[n]);
                if (index < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[n]);
                }
                @object = S.r_LINK;
                break;
            default:
                n--;
                break;
        }
        n++;
        if (n >= nToks)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        if (@object == S.r_GAGE)
        {
            attrib = S.getGageAttrib(tok[n]);
        }
        else
        {
            attrib = S.findmatch(tok[n], S.AttribWords);
        }
        if (attrib < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
        }
        if (obj == S.r_GAGE)
        {
        }
        else if (obj == S.r_NODE)
        {
            switch (attrib)
            {
                case S.r_DEPTH:
                case S.r_MAXDEPTH:
                case S.r_HEAD:
                case S.r_VOLUME:
                case S.r_INFLOW:
                    break;
                default:
                    return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
            }
        }
        else if (((@object == S.r_LINK) && (index >= 0)) && ((((attrib == S.r_TIMEOPEN) || (attrib == S.r_TIMECLOSED)))))
        {
        }
        else if ((obj == S.r_LINK) || (obj == S.r_CONDUIT))
        {
            switch (attrib)
            {
                case S.r_STATUS:
                case S.r_DEPTH:
                case S.r_FULLFLOW:
                case S.r_FULLDEPTH:
                case S.r_FLOW:
                case S.r_LENGTH:
                case S.r_SLOPE:
                case S.r_VELOCITY:
                    break;
                default:
                    return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
            }
        }
        else if (obj == S.r_PUMP)
        {
            switch (attrib)
            {
                case S.r_FLOW:
                case S.r_SETTING:
                case S.r_STATUS:
                    break;
                default:
                    return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
            }
        }
        else if (((obj == S.r_ORIFICE) || (obj == S.r_WEIR)) || (obj == S.r_OUTLET))
        {
            switch (attrib)
            {
                case S.r_FLOW:
                case S.r_SETTING:
                    break;
                default:
                    return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
            }
        }
        else
        {
            switch (attrib)
            {
                case S.r_TIME:
                case S.r_DATE:
                case S.r_CLOCKTIME:
                case S.r_DAY:
                case S.r_MONTH:
                case S.r_DAYOFYEAR:
                    break;
                default:
                    return S.error_setInpError(S.ERR_KEYWORD, tok[n]);
            }
        }
        v.@object = @object;
        v.index = index;
        v.attribute = attrib;
        k = n;
        return 0;
    }

    // controls.c:797
    public static int getGageAttrib(CPtr token)
    {
        int attrib = 0;
        if ((S.match(token, S.IntensityWord) != 0))
        {
            return 0;
        }
        attrib = R.atoi(token);
        if ((attrib < 1) || (attrib > 48))
        {
            return -1;
        }
        return attrib;
    }

    // controls.c:824
    public static int getPremiseValue(CPtr token, int attrib, ref double value)
    {
        CPtr strDate = R.Buf(25);
        switch (attrib)
        {
            case S.r_STATUS:
                value = S.findmatch(token, S.StatusWords);
                if (value < 0.0)
                {
                    value = S.findmatch(token, S.ConduitWords);
                }
                if (value < 0.0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, token);
                }
                break;
            case S.r_TIME:
            case S.r_CLOCKTIME:
            case S.r_TIMEOPEN:
            case S.r_TIMECLOSED:
                if (!((S.datetime_strToTime(token, ref value) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, token);
                }
                break;
            case S.r_DATE:
                if (!((S.datetime_strToDate(token, ref value) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, token);
                }
                break;
            case S.r_DAY:
                if (!((S.getDouble(token, ref value) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, token);
                }
                if ((value < 1.0) || (value > 7.0))
                {
                    return S.error_setInpError(S.ERR_DATETIME, token);
                }
                break;
            case S.r_MONTH:
                if (!((S.getDouble(token, ref value) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, token);
                }
                if ((value < 1.0) || (value > 12.0))
                {
                    return S.error_setInpError(S.ERR_DATETIME, token);
                }
                break;
            case S.r_DAYOFYEAR:
                S.sstrncpy(strDate, token, unchecked((ulong)(6)));
                S.sstrcat(strDate, S.L35, unchecked((ulong)(25)));
                if ((S.datetime_strToDate(strDate, ref value) != 0))
                {
                    value = S.datetime_dayOfYear(value);
                }
                else if (((!((S.getDouble(token, ref value) != 0))) || (value < 1)) || (value > 365))
                {
                    return S.error_setInpError(S.ERR_DATETIME, token);
                }
                break;
            default:
                if (!((S.getDouble(token, ref value) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, token);
                }
                break;
        }
        return 0;
    }

    // controls.c:889
    public static int addAction(int r, CPtr[] tok, int nToks)
    {
        int obj = 0;
        int link = 0;
        int attrib = 0;
        int curve = -1;
        int tseries = -1;
        int n = 0;
        int err = 0;
        double[] values = new double[] { 1.0, 0.0, 0.0 };
        TAction a = null;
        if (nToks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        obj = S.findmatch(tok[1], S.ObjectWords);
        if ((((((obj != S.r_LINK) && (obj != S.r_CONDUIT)) && (obj != S.r_PUMP)) && (obj != S.r_ORIFICE)) && (obj != S.r_WEIR)) && (obj != S.r_OUTLET))
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        link = S.project_findObject(S.LINK, tok[2]);
        if (link < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[2]);
        }
        switch (obj)
        {
            case S.r_CONDUIT:
                if (S.Link[link].type != S.CONDUIT)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[2]);
                }
                break;
            case S.r_PUMP:
                if (S.Link[link].type != S.PUMP)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[2]);
                }
                break;
            case S.r_ORIFICE:
                if (S.Link[link].type != S.ORIFICE)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[2]);
                }
                break;
            case S.r_WEIR:
                if (S.Link[link].type != S.WEIR)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[2]);
                }
                break;
            case S.r_OUTLET:
                if (S.Link[link].type != S.OUTLET)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[2]);
                }
                break;
        }
        attrib = S.findmatch(tok[3], S.AttribWords);
        if (attrib < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
        }
        if (obj == S.r_CONDUIT)
        {
            if (attrib == S.r_STATUS)
            {
                values[0] = S.findmatch(tok[5], S.ConduitWords);
                if (values[0] < 0.0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, tok[5]);
                }
            }
            else
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
            }
        }
        else if (obj == S.r_PUMP)
        {
            if (attrib == S.r_STATUS)
            {
                values[0] = S.findmatch(tok[5], S.StatusWords);
                if (values[0] < 0.0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, tok[5]);
                }
            }
            else if (attrib == S.r_SETTING)
            {
                err = S.setActionSetting(tok, nToks, ref curve, ref tseries, ref attrib, values);
                if (err > 0)
                {
                    return err;
                }
            }
            else
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
            }
        }
        else if (((obj == S.r_ORIFICE) || (obj == S.r_WEIR)) || (obj == S.r_OUTLET))
        {
            if (attrib == S.r_SETTING)
            {
                err = S.setActionSetting(tok, nToks, ref curve, ref tseries, ref attrib, values);
                if (err > 0)
                {
                    return err;
                }
                if ((attrib == S.r_SETTING) && (((values[0] < 0.0) || (values[0] > 1.0))))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[5]);
                }
            }
            else
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
            }
        }
        else
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        n = 6;
        if ((curve >= 0) || (tseries >= 0))
        {
            n = 7;
        }
        if (attrib == S.r_PID)
        {
            n = 9;
        }
        if ((n < nToks) && (S.findmatch(tok[n], S.RuleKeyWords) >= 0))
        {
            return S.ERR_RULE;
        }
        a = new TAction();
        if (!(a != null))
        {
            return S.ERR_MEMORY;
        }
        a.rule = r;
        a.link = link;
        a.attribute = attrib;
        a.curve = curve;
        a.tseries = tseries;
        a.value = values[0];
        if (attrib == S.r_PID)
        {
            a.kp = values[0];
            a.ki = values[1];
            a.kd = values[2];
            a.e1 = 0.0;
            a.e2 = 0.0;
        }
        if (S.InputState == S.r_THEN)
        {
            a.next = S.Rules[r].thenActions;
            S.Rules[r].thenActions = a;
        }
        else
        {
            a.next = S.Rules[r].elseActions;
            S.Rules[r].elseActions = a;
        }
        return 0;
    }

    // controls.c:1028
    public static int setActionSetting(CPtr[] tok, int nToks, ref int curve, ref int tseries, ref int attrib, double[] values)
    {
        int k = 0;
        int m = 0;
        if (nToks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        k = S.findmatch(tok[5], S.SettingTypeWords);
        if ((k >= 0) && (nToks < 7))
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        switch (k)
        {
            case S.r_CURVE:
                m = S.project_findObject(S.CURVE, tok[6]);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[6]);
                }
                curve = m;
                break;
            case S.r_TIMESERIES:
                m = S.project_findObject(S.TSERIES, tok[6]);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[6]);
                }
                tseries = m;
                S.Tseries[m].refersTo = S.CONTROL;
                break;
            case S.r_PID:
                if (nToks < 9)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                for (m = 6; m <= 8; m++)
                {
                    if (!((S.getDouble(tok[m], ref values[m - 6]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[m]);
                    }
                }
                attrib = S.r_PID;
                break;
            default:
                if (!((S.getDouble(tok[5], ref values[0]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[5]);
                }
                break;
        }
        return 0;
    }

    // controls.c:1086
    public static void updateActionValue(TAction a, double currentTime, double dt)
    {
        if (a.curve >= 0)
        {
            a.value = S.table_lookup(S.Curve[a.curve], S.ControlValue);
        }
        else if (a.tseries >= 0)
        {
            a.value = S.table_tseriesLookup(S.Tseries[a.tseries], currentTime, 1);
        }
        else if (a.attribute == S.r_PID)
        {
            a.value = S.getPIDSetting(a, dt);
        }
    }

    // controls.c:1111
    public static double getPIDSetting(TAction a, double dt)
    {
        double e0 = 0.0;
        double setting = 0.0;
        double p = 0.0;
        double i = 0.0;
        double d = 0.0;
        double update = 0.0;
        double tolerance = 0.0001;
        dt *= 1440.0;
        e0 = S.SetPoint - S.ControlValue;
        if (Math.Abs(e0) > 1e-06)
        {
            if (S.SetPoint != 0.0)
            {
                e0 = e0 / S.SetPoint;
            }
            else
            {
                e0 = e0 / S.ControlValue;
            }
        }
        if (Math.Abs(e0 - a.e1) < tolerance)
        {
            a.e2 = 0.0;
            a.e1 = 0.0;
        }
        p = (e0 - a.e1);
        if (a.ki == 0.0)
        {
            i = 0.0;
        }
        else
        {
            i = e0 * dt / a.ki;
        }
        d = a.kd * (e0 - 2.0 * a.e1 + a.e2) / dt;
        update = a.kp * (p + i + d);
        if (Math.Abs(update) < tolerance)
        {
            update = 0.0;
        }
        setting = S.Link[a.link].targetSetting + update;
        a.e2 = a.e1;
        a.e1 = e0;
        if (setting < 0.0)
        {
            setting = 0.0;
        }
        if ((S.Link[a.link].type != S.PUMP) && (setting > 1.0))
        {
            setting = 1.0;
        }
        return setting;
    }

    // controls.c:1168
    public static void updateActionList(TAction a)
    {
        TActionList listItem = null;
        TAction a1 = null;
        double priority = S.Rules[a.rule].priority;
        listItem = S.ActionList;
        while ((listItem != null))
        {
            a1 = listItem.action;
            if (!(a1 != null))
            {
                break;
            }
            if (a1.link == a.link)
            {
                if (priority > S.Rules[a1.rule].priority)
                {
                    listItem.action = a;
                }
                return;
            }
            listItem = listItem.next;
        }
        if (!(listItem != null))
        {
            listItem = new TActionList();
            listItem.next = S.ActionList;
            S.ActionList = listItem;
        }
        listItem.action = a;
    }

    // controls.c:1206
    public static int executeActionList(double currentTime)
    {
        TActionList listItem = null;
        TActionList nextItem = null;
        TAction a1 = null;
        int count = 0;
        listItem = S.ActionList;
        while ((listItem != null))
        {
            a1 = listItem.action;
            if (!(a1 != null))
            {
                break;
            }
            if (a1.link >= 0)
            {
                if (S.Link[a1.link].targetSetting != a1.value)
                {
                    S.Link[a1.link].targetSetting = a1.value;
                    if ((((S.RptFlags.controls != 0) && (a1.curve < 0)) && (a1.tseries < 0)) && (a1.attribute != S.r_PID))
                    {
                        S.report_writeControlAction(currentTime, S.Link[a1.link].ID, a1.value, S.Rules[a1.rule].ID);
                    }
                    count++;
                }
            }
            nextItem = listItem.next;
            listItem = nextItem;
        }
        return count;
    }

    // controls.c:1243
    public static int evaluatePremise(TPremise p, double tStep)
    {
        double lhsValue = 0.0;
        double rhsValue = 0.0;
        int result = 0;
        if (p.exprIndex >= 0)
        {
            lhsValue = S.mathexpr_eval(S.Expression[p.exprIndex].expression, S.getNamedVariableValue);
        }
        else
        {
            lhsValue = S.getVariableValue(p.lhsVar);
        }
        if (p.value == -10000000000.0)
        {
            rhsValue = S.getVariableValue(p.rhsVar);
        }
        else
        {
            rhsValue = p.value;
        }
        if ((lhsValue == -10000000000.0) || (rhsValue == -10000000000.0))
        {
            return 0;
        }
        switch (p.lhsVar.attribute)
        {
            case S.r_TIME:
            case S.r_CLOCKTIME:
                return S.compareTimes(lhsValue, p.relation, rhsValue, tStep / 2.0);
            case S.r_TIMEOPEN:
            case S.r_TIMECLOSED:
                result = S.compareTimes(lhsValue, p.relation, rhsValue, tStep / 2.0);
                S.ControlValue = lhsValue * 24.0;
                return result;
            default:
                return S.compareValues(lhsValue, p.relation, rhsValue);
        }
    }

    // controls.c:1286
    public static double getVariableValue(TVariable v)
    {
        int i = -1;
        int j = -1;
        if (v.@object == S.r_GAGE)
        {
            return S.getRainValue(v);
        }
        if (v.@object == S.r_NODE)
        {
            i = v.index;
        }
        if (v.@object == S.r_LINK)
        {
            j = v.index;
        }
        switch (v.attribute)
        {
            case S.r_TIME:
                return S.ElapsedTime;
            case S.r_DATE:
                return S.CurrentDate;
            case S.r_CLOCKTIME:
                return S.CurrentTime;
            case S.r_DAY:
                return S.datetime_dayOfWeek(S.CurrentDate);
            case S.r_MONTH:
                return S.datetime_monthOfYear(S.CurrentDate);
            case S.r_DAYOFYEAR:
                return S.datetime_dayOfYear(S.CurrentDate);
            case S.r_STATUS:
                if ((j < 0) || (((S.Link[j].type != S.CONDUIT) && (S.Link[j].type != S.PUMP))))
                {
                    return -10000000000.0;
                }
                else
                {
                    return S.Link[j].setting;
                }
            case S.r_SETTING:
                if ((j < 0) || ((((S.Link[j].type != S.PUMP) && (S.Link[j].type != S.ORIFICE)) && (S.Link[j].type != S.WEIR))))
                {
                    return -10000000000.0;
                }
                else
                {
                    return S.Link[j].setting;
                }
            case S.r_FLOW:
                if (j < 0)
                {
                    return -10000000000.0;
                }
                else
                {
                    return S.Link[j].direction * S.Link[j].newFlow * S.UCF(S.FLOW);
                }
            case S.r_FULLFLOW:
            case S.r_FULLDEPTH:
            case S.r_VELOCITY:
            case S.r_LENGTH:
            case S.r_SLOPE:
                if (j < 0)
                {
                    return -10000000000.0;
                }
                else if (S.Link[j].type != S.CONDUIT)
                {
                    return -10000000000.0;
                }
                switch (v.attribute)
                {
                    case S.r_FULLFLOW:
                        return S.Link[j].qFull * S.UCF(S.FLOW);
                    case S.r_FULLDEPTH:
                        return S.Link[j].xsect.yFull * S.UCF(S.LENGTH);
                    case S.r_VELOCITY:
                        return S.link_getVelocity(j, S.Link[j].newFlow, S.Link[j].newDepth) * S.UCF(S.LENGTH);
                    case S.r_LENGTH:
                        return S.Conduit[S.Link[j].subIndex].length * S.UCF(S.LENGTH);
                    case S.r_SLOPE:
                        return S.Conduit[S.Link[j].subIndex].slope;
                    default:
                        return -10000000000.0;
                }
                goto case S.r_DEPTH;
            case S.r_DEPTH:
                if (j >= 0)
                {
                    return S.Link[j].newDepth * S.UCF(S.LENGTH);
                }
                else if (i >= 0)
                {
                    return S.Node[i].newDepth * S.UCF(S.LENGTH);
                }
                else
                {
                    return -10000000000.0;
                }
            case S.r_MAXDEPTH:
                if (i >= 0)
                {
                    return S.Node[i].fullDepth * S.UCF(S.LENGTH);
                }
                else
                {
                    return -10000000000.0;
                }
            case S.r_HEAD:
                if (i < 0)
                {
                    return -10000000000.0;
                }
                return (S.Node[i].newDepth + S.Node[i].invertElev) * S.UCF(S.LENGTH);
            case S.r_VOLUME:
                if (i < 0)
                {
                    return -10000000000.0;
                }
                return (S.Node[i].newVolume * S.UCF(S.VOLUME));
            case S.r_INFLOW:
                if (i < 0)
                {
                    return -10000000000.0;
                }
                else
                {
                    return S.Node[i].newLatFlow * S.UCF(S.FLOW);
                }
            case S.r_TIMEOPEN:
                if (j < 0)
                {
                    return -10000000000.0;
                }
                if (S.Link[j].setting <= 0.0)
                {
                    return -10000000000.0;
                }
                return S.CurrentDate + S.CurrentTime - S.Link[j].timeLastSet;
            case S.r_TIMECLOSED:
                if (j < 0)
                {
                    return -10000000000.0;
                }
                if (S.Link[j].setting > 0.0)
                {
                    return -10000000000.0;
                }
                return S.CurrentDate + S.CurrentTime - S.Link[j].timeLastSet;
            default:
                return -10000000000.0;
        }
    }

    // controls.c:1388
    public static double getRainValue(TVariable v)
    {
        if (v.index < 0)
        {
            return -10000000000.0;
        }
        else if (S.Gage[v.index].isUsed == 0)
        {
            return 0.0;
        }
        else if (v.attribute == 0)
        {
            return S.Gage[v.index].rainfall;
        }
        else
        {
            return S.gage_getPastRain(v.index, v.attribute);
        }
    }

    // controls.c:1405
    public static int compareTimes(double lhsValue, int relation, double rhsValue, double halfStep)
    {
        if (relation == S.EQ)
        {
            if ((lhsValue >= rhsValue - halfStep) && (lhsValue < rhsValue + halfStep))
            {
                return 1;
            }
            return 0;
        }
        else if (relation == S.NE)
        {
            if ((lhsValue < rhsValue - halfStep) || (lhsValue >= rhsValue + halfStep))
            {
                return 1;
            }
            return 0;
        }
        else
        {
            return S.compareValues(lhsValue, relation, rhsValue);
        }
    }

    // controls.c:1432
    public static int compareValues(double lhsValue, int relation, double rhsValue)
    {
        S.SetPoint = rhsValue;
        S.ControlValue = lhsValue;
        switch (relation)
        {
            case S.EQ:
                if (lhsValue == rhsValue)
                {
                    return 1;
                }
                break;
            case S.NE:
                if (lhsValue != rhsValue)
                {
                    return 1;
                }
                break;
            case S.LT:
                if (lhsValue < rhsValue)
                {
                    return 1;
                }
                break;
            case S.LE:
                if (lhsValue <= rhsValue)
                {
                    return 1;
                }
                break;
            case S.GT:
                if (lhsValue > rhsValue)
                {
                    return 1;
                }
                break;
            case S.GE:
                if (lhsValue >= rhsValue)
                {
                    return 1;
                }
                break;
        }
        return 0;
    }

    // controls.c:1455
    public static void clearActionList()
    {
        TActionList listItem = null;
        listItem = S.ActionList;
        while ((listItem != null))
        {
            listItem.action = (null);
            listItem = listItem.next;
        }
    }

    // controls.c:1473
    public static void deleteActionList()
    {
        TActionList listItem = null;
        TActionList nextItem = null;
        listItem = S.ActionList;
        while ((listItem != null))
        {
            nextItem = listItem.next;
            R.free(listItem);
            listItem = nextItem;
        }
        S.ActionList = (null);
    }

    // controls.c:1494
    public static void deleteRules()
    {
        TPremise p = null;
        TPremise pnext = null;
        TAction a = null;
        TAction anext = null;
        int r = 0;
        for (r = 0; r < S.RuleCount; r++)
        {
            p = S.Rules[r].firstPremise;
            while ((p != null))
            {
                pnext = p.next;
                R.free(p);
                p = pnext;
            }
            a = S.Rules[r].thenActions;
            while ((a != null))
            {
                anext = a.next;
                R.free(a);
                a = anext;
            }
            a = S.Rules[r].elseActions;
            while ((a != null))
            {
                anext = a.next;
                R.free(a);
                a = anext;
            }
        }
        {
            if ((S.Rules != null))
            {
                R.free(S.Rules);
                S.Rules = (null);
            }
        }
        ;
        S.RuleCount = 0;
    }

    // controls.c:1536
    public static int findExactMatch(CPtr s, CPtr[] keyword)
    {
        int i = 0;
        while (!keyword[i].IsNull)
        {
            if ((S.strcomp(s, keyword[i]) != 0))
            {
                return (i);
            }
            i++;
        }
        return (-1);
    }

}
}
