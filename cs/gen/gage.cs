// Generated from EPA SWMM 5.2.4 (gage.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double OneSecond = 1.1574074e-5;

    // gage.c:58
    public static int gage_readParams(int j, CPtr[] tok, int ntoks)
    {
        int k = 0;
        int err = 0;
        CPtr id = CPtr.Null;
        CPtr fname = R.Buf(260);
        CPtr staID = R.Buf(1025);
        double[] x = new double[7];
        if (ntoks < 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        id = S.project_findID(S.GAGE, tok[0]);
        if (id.IsNull)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        x[0] = -1.0;
        x[1] = 1.0;
        x[2] = 3600.0;
        x[3] = 1.0;
        x[4] = -693594;
        x[5] = -693594;
        x[6] = 0.0;
        fname[0] = 0;
        staID[0] = 0;
        if (ntoks < 5)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        k = S.findmatch(tok[4], S.GageDataWords);
        if (k == S.RAIN_TSERIES)
        {
            err = S.readGageSeriesFormat(tok, ntoks, x);
        }
        else if (k == S.RAIN_FILE)
        {
            if (ntoks < 8)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            S.sstrncpy(fname, tok[5], unchecked((ulong)(259)));
            S.sstrncpy(staID, tok[6], unchecked((ulong)(1024)));
            err = S.readGageFileFormat(tok, ntoks, x);
        }
        else
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[4]);
        }
        if (err > 0)
        {
            return err;
        }
        S.Gage[j].ID = id;
        S.Gage[j].tSeries = R.D2I(x[0]);
        S.Gage[j].rainType = R.D2I(x[1]);
        S.Gage[j].rainInterval = R.D2I(x[2]);
        S.Gage[j].snowFactor = x[3];
        S.Gage[j].rainUnits = R.D2I(x[6]);
        if (S.Gage[j].tSeries >= 0)
        {
            S.Gage[j].dataSource = S.RAIN_TSERIES;
        }
        else
        {
            S.Gage[j].dataSource = S.RAIN_FILE;
        }
        if (S.Gage[j].dataSource == S.RAIN_FILE)
        {
            S.sstrncpy(S.Gage[j].fname, S.addAbsolutePath(fname), unchecked((ulong)(259)));
            S.sstrncpy(S.Gage[j].staID, staID, unchecked((ulong)(1024)));
            S.Gage[j].startFileDate = x[4];
            S.Gage[j].endFileDate = x[5];
        }
        S.Gage[j].unitsFactor = 1.0;
        S.Gage[j].coGage = -1;
        S.Gage[j].isUsed = 0;
        return 0;
    }

    // gage.c:133
    public static int readGageSeriesFormat(CPtr[] tok, int ntoks, double[] x)
    {
        int m = 0;
        int ts = 0;
        double aTime = 0.0;
        if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        m = S.findmatch(tok[1], S.RainTypeWords);
        if (m < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        x[1] = (double)(m);
        if ((S.getDouble(tok[2], ref x[2]) != 0))
        {
            x[2] = Math.Floor(x[2] * 3600 + 0.5);
        }
        else if ((S.datetime_strToTime(tok[2], ref aTime) != 0))
        {
            x[2] = Math.Floor(aTime * 86400 + 0.5);
        }
        else
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[2]);
        }
        if (x[2] <= 0.0)
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[2]);
        }
        if (!((S.getDouble(tok[3], ref x[3]) != 0)))
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[3]);
        }
        ;
        ts = S.project_findObject(S.TSERIES, tok[5]);
        if (ts < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[5]);
        }
        x[0] = (double)(ts);
        S.sstrncpy(tok[2], S.L0, unchecked((ulong)(0)));
        return 0;
    }

    // gage.c:168
    public static int readGageFileFormat(CPtr[] tok, int ntoks, double[] x)
    {
        int m = 0;
        int u = 0;
        double aDate = 0.0;
        double aTime = 0.0;
        m = S.findmatch(tok[1], S.RainTypeWords);
        if (m < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        x[1] = (double)(m);
        if ((S.getDouble(tok[2], ref x[2]) != 0))
        {
            x[2] *= 3600;
        }
        else if ((S.datetime_strToTime(tok[2], ref aTime) != 0))
        {
            x[2] = Math.Floor(aTime * 86400 + 0.5);
        }
        else
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[2]);
        }
        if (x[2] <= 0.0)
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[2]);
        }
        if (!((S.getDouble(tok[3], ref x[3]) != 0)))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[3]);
        }
        u = S.findmatch(tok[7], S.RainUnitsWords);
        if (u < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[7]);
        }
        x[6] = (double)(u);
        if ((ntoks > 8) && (tok[8][0] != '*'))
        {
            if (!((S.datetime_strToDate(tok[8], ref aDate) != 0)))
            {
                return S.error_setInpError(S.ERR_DATETIME, tok[8]);
            }
            x[4] = (float)(aDate);
        }
        return 0;
    }

    // gage.c:209
    public static void gage_validate(int j)
    {
        int i = 0;
        int k = 0;
        int gageInterval = 0;
        if (S.Gage[j].dataSource == S.RAIN_TSERIES)
        {
            if (!(S.Gage[j].isUsed != 0))
            {
                return;
            }
            k = S.Gage[j].tSeries;
            for (i = 0; i < j; i++)
            {
                if (((S.Gage[i].dataSource == S.RAIN_TSERIES) && (S.Gage[i].tSeries == k)) && (S.Gage[i].isUsed != 0))
                {
                    S.Gage[j].coGage = i;
                    if (S.Gage[j].rainType != S.Gage[i].rainType)
                    {
                        S.report_writeErrorMsg(S.ERR_RAIN_GAGE_FORMAT, S.Gage[j].ID);
                    }
                    return;
                }
            }
            if (S.Tseries[k].refersTo >= 0)
            {
                S.report_writeErrorMsg(S.ERR_RAIN_GAGE_TSERIES, S.Gage[j].ID);
            }
            gageInterval = R.D2I((Math.Floor(S.Tseries[k].dxMin * 86400 + 0.5)));
            if ((gageInterval > 0) && (S.Gage[j].rainInterval > gageInterval))
            {
                S.report_writeErrorMsg(S.ERR_RAIN_GAGE_INTERVAL, S.Gage[j].ID);
            }
            if (S.Gage[j].rainInterval < gageInterval)
            {
                S.report_writeWarningMsg(S.L156, S.Gage[j].ID);
            }
            if (S.Gage[j].rainInterval < S.WetStep)
            {
                S.report_writeWarningMsg(S.L157, S.Gage[j].ID);
                S.WetStep = S.Gage[j].rainInterval;
            }
        }
    }

    // gage.c:270
    public static void gage_initState(int j)
    {
        S.Gage[j].rainfall = 0.0;
        S.Gage[j].apiRainfall = -10000000000.0;
        S.Gage[j].reportRainfall = 0.0;
        if ((S.IgnoreRainfall != 0))
        {
            return;
        }
        if (S.Gage[j].dataSource == S.RAIN_FILE)
        {
            S.Gage[j].currentFilePos = S.Gage[j].startFilePos;
            if (S.UnitSystem == S.SI)
            {
                S.Gage[j].unitsFactor = 25.4;
            }
        }
        if ((S.getFirstRainfall(j) != 0))
        {
            S.Gage[j].endDate = S.datetime_addSeconds(S.Gage[j].startDate, S.Gage[j].rainInterval);
            if (S.Gage[j].startDate > S.StartDateTime)
            {
                S.Gage[j].nextDate = S.Gage[j].startDate;
                S.Gage[j].nextRainfall = S.Gage[j].rainfall;
                S.Gage[j].startDate = S.StartDateTime;
                S.Gage[j].endDate = S.Gage[j].nextDate;
                S.Gage[j].rainfall = 0.0;
            }
            else if (!((S.getNextRainfall(j) != 0)))
            {
                S.Gage[j].nextDate = -693594;
            }
        }
        else
        {
            S.Gage[j].startDate = -693594;
        }
        S.initPastRain(j);
    }

    // gage.c:323
    public static void gage_setState(int j, double t)
    {
        if (S.Gage[j].isUsed == 0)
        {
            return;
        }
        if ((S.IgnoreRainfall != 0))
        {
            S.Gage[j].rainfall = 0.0;
            return;
        }
        if (S.Gage[j].coGage >= 0)
        {
            S.Gage[j].rainfall = S.Gage[S.Gage[j].coGage].rainfall;
            return;
        }
        if (S.Gage[j].apiRainfall != -10000000000.0)
        {
            S.Gage[j].rainfall = S.Gage[j].apiRainfall;
            return;
        }
        t += S.OneSecond;
        for (; ; )
        {
            if (S.Gage[j].startDate == -693594)
            {
                S.Gage[j].rainfall = 0.0;
                return;
            }
            if (t < S.Gage[j].startDate)
            {
                S.Gage[j].rainfall = 0.0;
                return;
            }
            if (t < S.Gage[j].endDate)
            {
                return;
            }
            if (S.Gage[j].nextDate == -693594)
            {
                S.Gage[j].rainfall = 0.0;
                return;
            }
            if (t < S.Gage[j].nextDate)
            {
                S.Gage[j].rainfall = 0.0;
                return;
            }
            S.Gage[j].startDate = S.Gage[j].nextDate;
            S.Gage[j].endDate = S.datetime_addSeconds(S.Gage[j].startDate, S.Gage[j].rainInterval);
            S.Gage[j].rainfall = S.Gage[j].nextRainfall;
            if (!((S.getNextRainfall(j) != 0)))
            {
                S.Gage[j].nextDate = -693594;
            }
        }
    }

    // gage.c:406
    public static void initPastRain(int j)
    {
        int i = 0;
        for (i = 0; i <= 48; i++)
        {
            S.Gage[j].pastRain[i] = 0.0;
        }
        S.Gage[j].pastInterval = 0;
    }

    // gage.c:417
    public static void gage_updatePastRain(int j, int tStep)
    {
        int i = 0;
        int t = 0;
        double r = 0.0;
        r = S.Gage[j].rainfall / 3600.0;
        while (tStep > 0)
        {
            t = 3600 - S.Gage[j].pastInterval;
            if (tStep > t)
            {
                S.Gage[j].pastRain[0] += t * r;
                for (i = 48; i > 0; i--)
                {
                    S.Gage[j].pastRain[i] = S.Gage[j].pastRain[i - 1];
                }
                S.Gage[j].pastInterval = 0;
                S.Gage[j].pastRain[0] = 0.0;
                tStep -= t;
            }
            else
            {
                S.Gage[j].pastRain[0] += tStep * r;
                S.Gage[j].pastInterval += tStep;
                tStep = 0;
            }
        }
    }

    // gage.c:468
    public static double gage_getPastRain(int j, int n)
    {
        int i = 0;
        double result = 0.0;
        if ((n < 1) || (n > 48))
        {
            return 0.0;
        }
        for (i = 1; i <= n; i++)
        {
            result += S.Gage[j].pastRain[i];
        }
        return result;
    }

    // gage.c:486
    public static double gage_getNextRainDate(int j, double aDate)
    {
        if (S.Gage[j].isUsed == 0)
        {
            return aDate;
        }
        aDate += S.OneSecond;
        if (aDate < S.Gage[j].startDate)
        {
            return S.Gage[j].startDate;
        }
        if (aDate < S.Gage[j].endDate)
        {
            return S.Gage[j].endDate;
        }
        return S.Gage[j].nextDate;
    }

    // gage.c:503
    public static double gage_getPrecip(int j, ref double rainfall, ref double snowfall)
    {
        rainfall = 0.0;
        snowfall = 0.0;
        if ((!(S.IgnoreSnowmelt != 0)) && (S.Temp.ta <= S.Snow.snotmp))
        {
            snowfall = S.Gage[j].rainfall * S.Gage[j].snowFactor / S.UCF(S.RAINFALL);
        }
        else
        {
            rainfall = S.Gage[j].rainfall / S.UCF(S.RAINFALL);
        }
        return (rainfall) + (snowfall);
    }

    // gage.c:524
    public static void gage_setReportRainfall(int j, double reportDate)
    {
        double result = 0.0;
        if (S.Gage[j].coGage >= 0)
        {
            S.Gage[j].reportRainfall = S.Gage[S.Gage[j].coGage].reportRainfall;
            return;
        }
        if (S.Gage[j].apiRainfall != -10000000000.0)
        {
            S.Gage[j].reportRainfall = S.Gage[j].apiRainfall;
            return;
        }
        reportDate += S.OneSecond;
        if (reportDate < S.Gage[j].endDate)
        {
            result = S.Gage[j].rainfall;
        }
        else if (reportDate < S.Gage[j].nextDate)
        {
            result = 0.0;
        }
        else
        {
            result = S.Gage[j].nextRainfall;
        }
        S.Gage[j].reportRainfall = result;
    }

    // gage.c:567
    public static int getFirstRainfall(int j)
    {
        int k = 0;
        float vFirst = 0.0f;
        double rFirst = 0.0;
        S.Gage[j].startDate = -693594;
        S.Gage[j].rainfall = 0.0;
        S.Gage[j].rainAccum = 0;
        if (S.Gage[j].dataSource == S.RAIN_FILE)
        {
            if ((S.Frain.file != null) && (S.Gage[j].endFilePos > S.Gage[j].startFilePos))
            {
                R.fseek(S.Frain.file, S.Gage[j].startFilePos, 0);
                R.freadD1(ref S.Gage[j].startDate, S.Frain.file);
                R.freadF1(ref vFirst, S.Frain.file);
                S.Gage[j].currentFilePos = R.ftell(S.Frain.file);
                S.Gage[j].rainfall = S.convertRainfall(j, vFirst);
                return 1;
            }
            return 0;
        }
        else
        {
            k = S.Gage[j].tSeries;
            if (k >= 0)
            {
                if ((S.table_getFirstEntry(S.Tseries[k], ref S.Gage[j].startDate, ref rFirst) != 0))
                {
                    S.Gage[j].rainfall = S.convertRainfall(j, rFirst);
                    return 1;
                }
            }
            return 0;
        }
    }

    // gage.c:624
    public static int getNextRainfall(int j)
    {
        int k = 0;
        float vNext = 0.0f;
        double rNext = 0.0;
        S.Gage[j].nextRainfall = 0.0;
        do
        {
            if (S.Gage[j].dataSource == S.RAIN_FILE)
            {
                if ((S.Frain.file != null) && (S.Gage[j].currentFilePos < S.Gage[j].endFilePos))
                {
                    R.fseek(S.Frain.file, S.Gage[j].currentFilePos, 0);
                    R.freadD1(ref S.Gage[j].nextDate, S.Frain.file);
                    R.freadF1(ref vNext, S.Frain.file);
                    S.Gage[j].currentFilePos = R.ftell(S.Frain.file);
                    rNext = S.convertRainfall(j, vNext);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                k = S.Gage[j].tSeries;
                if (k >= 0)
                {
                    if (!((S.table_getNextEntry(S.Tseries[k], ref S.Gage[j].nextDate, ref rNext) != 0)))
                    {
                        return 0;
                    }
                    rNext = S.convertRainfall(j, rNext);
                }
                else
                {
                    return 0;
                }
            }
        }
        while (rNext == 0.0);
        S.Gage[j].nextRainfall = rNext;
        return 1;
    }

    // gage.c:674
    public static double convertRainfall(int j, double r)
    {
        double r1 = 0.0;
        switch (S.Gage[j].rainType)
        {
            case S.RAINFALL_INTENSITY:
                r1 = r;
                break;
            case S.RAINFALL_VOLUME:
                r1 = r / S.Gage[j].rainInterval * 3600.0;
                break;
            case S.CUMULATIVE_RAINFALL:
                if (r < S.Gage[j].rainAccum)
                {
                    r1 = r / S.Gage[j].rainInterval * 3600.0;
                }
                else
                {
                    r1 = (r - S.Gage[j].rainAccum) / S.Gage[j].rainInterval * 3600.0;
                }
                S.Gage[j].rainAccum = r;
                break;
            default:
                r1 = r;
                break;
        }
        return r1 * S.Gage[j].unitsFactor * S.Adjust.rainFactor;
    }

}
}
