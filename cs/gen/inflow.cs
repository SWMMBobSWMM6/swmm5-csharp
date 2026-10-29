// Generated from EPA SWMM 5.2.4 (inflow.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // inflow.c:41
    public static int inflow_readExtInflow(CPtr[] tok, int ntoks)
    {
        int j = 0;
        int param = 0;
        int type = S.CONCEN_INFLOW;
        int tseries = -1;
        int basePat = -1;
        double cf = 1.0;
        double sf = 1.0;
        double baseline = 0.0;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.NODE, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        param = S.project_findObject(S.POLLUT, tok[1]);
        if (param < 0)
        {
            if ((S.match(tok[1], S.L183) != 0))
            {
                param = -1;
            }
            else
            {
                return S.error_setInpError(S.ERR_NAME, tok[1]);
            }
        }
        if (R.strlen(tok[2]) > unchecked((ulong)(0)))
        {
            tseries = S.project_findObject(S.TSERIES, tok[2]);
            if (tseries < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[2]);
            }
            S.Tseries[tseries].refersTo = S.EXTERNAL_INFLOW;
        }
        if (param == -1)
        {
            type = S.FLOW_INFLOW;
            cf = 1.0 / S.UCF(S.FLOW);
        }
        if ((ntoks >= 4) && (param > -1))
        {
            if ((S.match(tok[3], S.L185) != 0))
            {
                type = S.CONCEN_INFLOW;
            }
            else if ((S.match(tok[3], S.L186) != 0))
            {
                type = S.MASS_INFLOW;
            }
            else
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
            }
            if ((ntoks >= 5) && (type == S.MASS_INFLOW))
            {
                if (!((S.getDouble(tok[4], ref cf) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[4]);
                }
                if (cf <= 0.0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[4]);
                }
            }
        }
        if (ntoks >= 6)
        {
            if (!((S.getDouble(tok[5], ref sf) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[5]);
            }
        }
        if (ntoks >= 7)
        {
            if (!((S.getDouble(tok[6], ref baseline) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[6]);
            }
        }
        if (ntoks >= 8)
        {
            basePat = S.project_findObject(S.TIMEPATTERN, tok[7]);
            if (basePat < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[7]);
            }
        }
        if (type == S.MASS_INFLOW)
        {
            cf /= 28.317;
        }
        return (S.inflow_setExtInflow(j, param, type, tseries, basePat, cf, baseline, sf));
    }

    // inflow.c:138
    public static int inflow_setExtInflow(int j, int param, int type, int tseries, int basePat, double cf, double baseline, double sf)
    {
        TExtInflow inflow = null;
        inflow = S.Node[j].extInflow;
        while ((inflow != null))
        {
            if (inflow.param == param)
            {
                break;
            }
            inflow = inflow.next;
        }
        if (inflow == null)
        {
            inflow = new TExtInflow();
            if (inflow == null)
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
            inflow.next = S.Node[j].extInflow;
            S.Node[j].extInflow = inflow;
        }
        inflow.param = param;
        inflow.type = type;
        inflow.tSeries = tseries;
        inflow.cFactor = cf;
        inflow.sFactor = sf;
        inflow.baseline = baseline;
        inflow.basePat = basePat;
        return 0;
    }

    // inflow.c:187
    public static void inflow_deleteExtInflows(int j)
    {
        TExtInflow inflow1 = null;
        TExtInflow inflow2 = null;
        inflow1 = S.Node[j].extInflow;
        while ((inflow1 != null))
        {
            inflow2 = inflow1.next;
            R.free(inflow1);
            inflow1 = inflow2;
        }
    }

    // inflow.c:207
    public static double inflow_getExtInflow(TExtInflow inflow, double aDate)
    {
        int month = 0;
        int day = 0;
        int hour = 0;
        int p = inflow.basePat;
        int k = inflow.tSeries;
        double cf = inflow.cFactor;
        double sf = inflow.sFactor;
        double blv = inflow.baseline;
        double tsv = 0.0;
        if (p >= 0)
        {
            month = S.datetime_monthOfYear(aDate) - 1;
            day = S.datetime_dayOfWeek(aDate) - 1;
            hour = S.datetime_hourOfDay(aDate);
            blv *= S.getPatternFactor(p, month, day, hour);
        }
        if (k >= 0)
        {
            tsv = S.table_tseriesLookup(S.Tseries[k], aDate, 0) * sf;
        }
        return cf * (tsv + blv);
    }

    // inflow.c:237
    public static int inflow_readDwfInflow(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int m = 0;
        int[] pats = new int[4];
        double x = 0.0;
        TDwfInflow inflow = null;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.NODE, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        k = S.project_findObject(S.POLLUT, tok[1]);
        if (k < 0)
        {
            if ((S.match(tok[1], S.L183) != 0))
            {
                k = -1;
            }
            else
            {
                return S.error_setInpError(S.ERR_NAME, tok[1]);
            }
        }
        if (!((S.getDouble(tok[2], ref x) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[2]);
        }
        if (k == -1)
        {
            x /= S.UCF(S.FLOW);
        }
        for (i = 0; i < 4; i++)
        {
            pats[i] = -1;
        }
        for (i = 3; i < 7; i++)
        {
            if (i >= ntoks)
            {
                break;
            }
            if (R.strlen(tok[i]) == unchecked((ulong)(0)))
            {
                continue;
            }
            m = S.project_findObject(S.TIMEPATTERN, tok[i]);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[i]);
            }
            pats[i - 3] = m;
        }
        inflow = S.Node[j].dwfInflow;
        while ((inflow != null))
        {
            if (inflow.param == k)
            {
                break;
            }
            inflow = inflow.next;
        }
        if (inflow == null)
        {
            inflow = new TDwfInflow();
            if (inflow == null)
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
            inflow.next = S.Node[j].dwfInflow;
            S.Node[j].dwfInflow = inflow;
        }
        inflow.param = k;
        inflow.avgValue = x;
        for (i = 0; i < 4; i++)
        {
            inflow.patterns[i] = pats[i];
        }
        return 0;
    }

    // inflow.c:311
    public static void inflow_deleteDwfInflows(int j)
    {
        TDwfInflow inflow1 = null;
        TDwfInflow inflow2 = null;
        inflow1 = S.Node[j].dwfInflow;
        while ((inflow1 != null))
        {
            inflow2 = inflow1.next;
            R.free(inflow1);
            inflow1 = inflow2;
        }
    }

    // inflow.c:331
    public static void inflow_initDwfInflow(TDwfInflow inflow)
    {
        int i = 0;
        int p = 0;
        int[] tmpPattern = new int[4];
        for (i = 0; i < 4; i++)
        {
            tmpPattern[i] = -1;
        }
        for (i = 0; i < 4; i++)
        {
            p = inflow.patterns[i];
            if (p >= 0)
            {
                tmpPattern[S.Pattern[p].type] = p;
            }
        }
        for (i = 0; i < 4; i++)
        {
            inflow.patterns[i] = tmpPattern[i];
        }
    }

    // inflow.c:361
    public static double inflow_getDwfInflow(TDwfInflow inflow, int month, int day, int hour)
    {
        int p1 = 0;
        int p2 = 0;
        double f = 1.0;
        p1 = inflow.patterns[S.MONTHLY_PATTERN];
        if (p1 >= 0)
        {
            f *= S.getPatternFactor(p1, month, day, hour);
        }
        p1 = inflow.patterns[S.DAILY_PATTERN];
        if (p1 >= 0)
        {
            f *= S.getPatternFactor(p1, month, day, hour);
        }
        p1 = inflow.patterns[S.HOURLY_PATTERN];
        p2 = inflow.patterns[S.WEEKEND_PATTERN];
        if (p2 >= 0)
        {
            if ((day == 0) || (day == 6))
            {
                f *= S.getPatternFactor(p2, month, day, hour);
            }
            else if (p1 >= 0)
            {
                f *= S.getPatternFactor(p1, month, day, hour);
            }
        }
        else if (p1 >= 0)
        {
            f *= S.getPatternFactor(p1, month, day, hour);
        }
        return f * inflow.avgValue;
    }

    // inflow.c:394
    public static void inflow_initDwfPattern(int j)
    {
        int i = 0;
        for (i = 0; i < 24; i++)
        {
            S.Pattern[j].factor[i] = 1.0;
        }
        S.Pattern[j].count = 0;
        S.Pattern[j].type = -1;
        S.Pattern[j].ID = CPtr.Null;
    }

    // inflow.c:410
    public static int inflow_readDwfPattern(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int n = 1;
        if (ntoks < 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.TIMEPATTERN, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        if (S.Pattern[j].ID.IsNull)
        {
            S.Pattern[j].ID = S.project_findID(S.TIMEPATTERN, tok[0]);
            k = S.findmatch(tok[1], S.PatternTypeWords);
            if (k < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
            }
            S.Pattern[j].type = k;
            n = 2;
        }
        while ((ntoks > n) && (S.Pattern[j].count < 24))
        {
            i = S.Pattern[j].count;
            if (!((S.getDouble(tok[n], ref S.Pattern[j].factor[i]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[n]);
            }
            S.Pattern[j].count++;
            n++;
        }
        return 0;
    }

    // inflow.c:456
    public static double getPatternFactor(int p, int month, int day, int hour)
    {
        switch (S.Pattern[p].type)
        {
            case S.MONTHLY_PATTERN:
                if ((month >= 0) && (month < 12))
                {
                    return S.Pattern[p].factor[month];
                }
                break;
            case S.DAILY_PATTERN:
                if ((day >= 0) && (day < 7))
                {
                    return S.Pattern[p].factor[day];
                }
                break;
            case S.HOURLY_PATTERN:
                if ((hour >= 0) && (hour < 24))
                {
                    return S.Pattern[p].factor[hour];
                }
                break;
            case S.WEEKEND_PATTERN:
                if ((day == 0) || (day == 6))
                {
                    if ((hour >= 0) && (hour < 24))
                    {
                        return S.Pattern[p].factor[hour];
                    }
                }
                break;
        }
        return 1.0;
    }

}
}
