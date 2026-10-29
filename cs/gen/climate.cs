// Generated from EPA SWMM 5.2.4 (climate.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int MAXCLIMATEVARS = 4;
    public static int MAXDAYSPERMONTH = 32;
    public static CPtr[] ClimateVarWords = new CPtr[] { S.L22, S.L23, S.L7, S.L24, S.L25, CPtr.Null };
    public static CPtr[] TempUnitsWords = new CPtr[] { S.L653, S.L654, S.L655, CPtr.Null };
    public static double Tmin = 0.0;
    public static double Tmax = 0.0;
    public static double Trng = 0.0;
    public static double Trng1 = 0.0;
    public static double Tave = 0.0;
    public static double Hrsr = 0.0;
    public static double Hrss = 0.0;
    public static double Hrday = 0.0;
    public static double Dhrdy = 0.0;
    public static double Dydif = 0.0;
    public static double LastDay = 0.0;
    public static TMovAve Tma = new TMovAve();
    public static double NextEvapDate = 0.0;
    public static double NextEvapRate = 0.0;
    public static int FileFormat = 0;
    public static int FileYear = 0;
    public static int FileMonth = 0;
    public static int FileDay = 0;
    public static int FileLastDay = 0;
    public static int FileElapsedDays = 0;
    public static double[] FileValue = new double[4];
    public static double[][] FileData = R.Jagged<double>(4, 32);
    public static CPtr FileLine = R.Buf(1025);
    public static int[] FileFieldPos = new int[4];
    public static int FileDateFieldPos = 0;
    public static int FileWindType = 0;
    public static int FileTempUnits = 0;

    // climate.c:153
    public static int climate_readParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        double[] x = new double[6];
        double y = 0.0;
        double aDate = 0.0;
        CPtr fname = R.Buf(260);
        k = S.findmatch(tok[0], S.TempKeyWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[0]);
        }
        switch (k)
        {
            case 0:
                if (ntoks < 2)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                i = S.project_findObject(S.TSERIES, tok[1]);
                if (i < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[1]);
                }
                S.Temp.dataSource = S.TSERIES_TEMP;
                S.Temp.tSeries = i;
                S.Tseries[i].refersTo = S.TSERIES_TEMP;
                break;
            case 1:
                if (ntoks < 2)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                S.Temp.dataSource = S.FILE_TEMP;
                S.Fclimate.mode = S.USE_FILE;
                S.sstrncpy(fname, tok[1], unchecked((ulong)(259)));
                S.sstrncpy(S.Fclimate.name, S.addAbsolutePath(fname), unchecked((ulong)(259)));
                S.Temp.fileStartDate = -693594;
                if (ntoks > 2)
                {
                    if (tok[2][0] != '*')
                    {
                        if (!((S.datetime_strToDate(tok[2], ref aDate) != 0)))
                        {
                            return S.error_setInpError(S.ERR_DATETIME, tok[2]);
                        }
                        S.Temp.fileStartDate = aDate;
                    }
                }
                S.FileTempUnits = S.DEG_F;
                if (S.UnitSystem == S.SI)
                {
                    S.FileTempUnits = S.DEG_C;
                }
                if (ntoks > 3)
                {
                    i = S.findmatch(tok[3], S.TempUnitsWords);
                    if (i < 0)
                    {
                        return S.error_setInpError(S.ERR_KEYWORD, tok[3]);
                    }
                    S.FileTempUnits = i;
                }
                break;
            case 2:
                if ((S.strcomp(tok[1], S.L1) != 0))
                {
                    S.Wind.type = S.FILE_WIND;
                }
                else
                {
                    if (ntoks < 14)
                    {
                        return S.error_setInpError(S.ERR_ITEMS, S.L0);
                    }
                    S.Wind.type = S.MONTHLY_WIND;
                    for (i = 0; i < 12; i++)
                    {
                        if (!((S.getDouble(tok[i + 2], ref y) != 0)))
                        {
                            return S.error_setInpError(S.ERR_NUMBER, tok[i + 2]);
                        }
                        S.Wind.aws[i] = y;
                    }
                }
                break;
            case 3:
                if (ntoks < 7)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                for (i = 1; i < 7; i++)
                {
                    if (!((S.getDouble(tok[i], ref x[i - 1]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                    }
                }
                if (S.UnitSystem == S.SI)
                {
                    x[0] = 9.0 / 5.0 * x[0] + 32.0;
                }
                S.Snow.snotmp = x[0];
                S.Snow.tipm = x[1];
                S.Snow.rnm = x[2];
                S.Temp.elev = x[3] / S.UCF(S.LENGTH);
                S.Temp.anglat = x[4];
                S.Temp.dtlong = x[5] / 60.0;
                break;
            case 4:
                if (ntoks < 12)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                if ((S.match(tok[1], S.L2) != 0))
                {
                    i = 0;
                }
                else if ((S.match(tok[1], S.L3) != 0))
                {
                    i = 1;
                }
                else
                {
                    return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
                }
                for (j = 0; j < 10; j++)
                {
                    if (((!((S.getDouble(tok[j + 2], ref y) != 0))) || (y < 0.0)) || (y > 1.0))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[j + 2]);
                    }
                    S.Snow.adc[i][j] = y;
                }
                break;
        }
        return 0;
    }

    // climate.c:285
    public static int climate_readEvapParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int k = 0;
        double x = 0.0;
        k = S.findmatch(tok[0], S.EvapTypeWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[0]);
        }
        if (k == S.RECOVERY)
        {
            if (ntoks < 2)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            i = S.project_findObject(S.TIMEPATTERN, tok[1]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[1]);
            }
            S.Evap.recoveryPattern = i;
            return 0;
        }
        if (k == S.DRYONLY)
        {
            if (ntoks < 2)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if ((S.strcomp(tok[1], S.L4) != 0))
            {
                S.Evap.dryOnly = 0;
            }
            else if ((S.strcomp(tok[1], S.L5) != 0))
            {
                S.Evap.dryOnly = 1;
            }
            else
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
            }
            return 0;
        }
        S.Evap.type = k;
        if ((k != S.TEMPERATURE_EVAP) && (ntoks < 2))
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        switch (k)
        {
            case S.CONSTANT_EVAP:
                if (!((S.getDouble(tok[1], ref x) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[1]);
                }
                for (i = 0; i < 12; i++)
                {
                    S.Evap.monthlyEvap[i] = x;
                }
                break;
            case S.MONTHLY_EVAP:
                if (ntoks < 13)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                for (i = 0; i < 12; i++)
                {
                    if (!((S.getDouble(tok[i + 1], ref S.Evap.monthlyEvap[i]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[i + 1]);
                    }
                }
                break;
            case S.TIMESERIES_EVAP:
                i = S.project_findObject(S.TSERIES, tok[1]);
                if (i < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[1]);
                }
                S.Evap.tSeries = i;
                S.Tseries[i].refersTo = S.TIMESERIES_EVAP;
                break;
            case S.FILE_EVAP:
                if (ntoks > 1)
                {
                    if (ntoks < 13)
                    {
                        return S.error_setInpError(S.ERR_ITEMS, S.L0);
                    }
                    for (i = 0; i < 12; i++)
                    {
                        if (!((S.getDouble(tok[i + 1], ref S.Evap.panCoeff[i]) != 0)))
                        {
                            return S.error_setInpError(S.ERR_NUMBER, tok[i + 1]);
                        }
                    }
                }
                break;
        }
        return 0;
    }

    // climate.c:377
    public static int climate_readAdjustments(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        if (ntoks == 1)
        {
            return 0;
        }
        if ((S.match(tok[0], S.L6) != 0))
        {
            if (ntoks < 13)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            for (i = 1; i < 13; i++)
            {
                if (!((S.getDouble(tok[i], ref S.Adjust.temp[i - 1]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                }
            }
            return 0;
        }
        if ((S.match(tok[0], S.L7) != 0))
        {
            if (ntoks < 13)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            for (i = 1; i < 13; i++)
            {
                if (!((S.getDouble(tok[i], ref S.Adjust.evap[i - 1]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                }
            }
            return 0;
        }
        if ((S.match(tok[0], S.L8) != 0))
        {
            if (ntoks < 13)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            for (i = 1; i < 13; i++)
            {
                if (!((S.getDouble(tok[i], ref S.Adjust.rain[i - 1]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                }
            }
            return 0;
        }
        if ((S.match(tok[0], S.L9) != 0))
        {
            if (ntoks < 13)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            for (i = 1; i < 13; i++)
            {
                if (!((S.getDouble(tok[i], ref S.Adjust.hydcon[i - 1]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                }
                if (S.Adjust.hydcon[i - 1] <= 0.0)
                {
                    S.Adjust.hydcon[i - 1] = 1.0;
                }
            }
            return 0;
        }
        if ((S.match(tok[0], S.L10) != 0))
        {
            if (ntoks < 3)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            i = S.project_findObject(S.SUBCATCH, tok[1]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[1]);
            }
            j = S.project_findObject(S.TIMEPATTERN, tok[2]);
            if (j < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[2]);
            }
            S.Subcatch[i].nPervPattern = j;
            return 0;
        }
        if ((S.match(tok[0], S.L11) != 0))
        {
            if (ntoks < 3)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            i = S.project_findObject(S.SUBCATCH, tok[1]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[1]);
            }
            j = S.project_findObject(S.TIMEPATTERN, tok[2]);
            if (j < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[2]);
            }
            S.Subcatch[i].dStorePattern = j;
            return 0;
        }
        if ((S.match(tok[0], S.L12) != 0))
        {
            if (ntoks < 3)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            i = S.project_findObject(S.SUBCATCH, tok[1]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[1]);
            }
            j = S.project_findObject(S.TIMEPATTERN, tok[2]);
            if (j < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[2]);
            }
            S.Subcatch[i].infilPattern = j;
            return 0;
        }
        return S.error_setInpError(S.ERR_KEYWORD, tok[0]);
    }

    // climate.c:480
    public static void climate_validate()
    {
        int i = 0;
        double a = 0.0;
        double z = 0.0;
        double pa = 0.0;
        if (((S.Wind.type == S.FILE_WIND) || (S.Evap.type == S.FILE_EVAP)) || (S.Evap.type == S.TEMPERATURE_EVAP))
        {
            if (S.Fclimate.mode == S.NO_FILE)
            {
                S.report_writeErrorMsg(S.ERR_NO_CLIMATE_FILE, S.L0);
            }
        }
        if (S.Fclimate.mode == S.USE_FILE)
        {
            S.climate_openFile();
        }
        if ((((S.Snow.tipm < 0.0) || (S.Snow.tipm > 1.0)) || (S.Snow.rnm < 0.0)) || (S.Snow.rnm > 1.0))
        {
            S.report_writeErrorMsg(S.ERR_SNOWMELT_PARAMS, S.L0);
        }
        a = S.Temp.anglat;
        if ((a <= -89.99) || (a >= 89.99))
        {
            S.report_writeErrorMsg(S.ERR_SNOWMELT_PARAMS, S.L0);
        }
        else
        {
            S.Temp.tanAnglat = Math.Tan(a * 3.141592654 / 180.0);
        }
        z = S.Temp.elev / 1000.0;
        if (z <= 0.0)
        {
            pa = 29.9;
        }
        else
        {
            pa = 29.9 - 1.02 * z + 0.0032 * Math.Pow(z, 2.4);
        }
        S.Temp.gamma = 0.000359 * pa;
        for (i = 0; i < 12; i++)
        {
            if (S.UnitSystem == S.SI)
            {
                S.Adjust.temp[i] *= 9.0 / 5.0;
            }
            S.Adjust.evap[i] /= S.UCF(S.EVAPRATE);
        }
    }

    // climate.c:531
    public static void climate_openFile()
    {
        int i = 0;
        int m = 0;
        int y = 0;
        if ((S.Fclimate.file = R.fopen(S.Fclimate.name, S.L13)) == null)
        {
            S.report_writeErrorMsg(S.ERR_CLIMATE_FILE_OPEN, S.Fclimate.name);
            return;
        }
        S.FileValue[S.TMIN] = S.Temp.ta;
        S.FileValue[S.TMAX] = S.Temp.ta;
        S.FileValue[S.EVAP] = 0.0;
        S.FileValue[S.WIND] = 0.0;
        S.FileFormat = S.getFileFormat();
        if (S.FileFormat == S.UNKNOWN_FORMAT)
        {
            S.report_writeErrorMsg(S.ERR_CLIMATE_FILE_READ, S.Fclimate.name);
            return;
        }
        R.rewind(S.Fclimate.file);
        S.sstrncpy(S.FileLine, S.L0, unchecked((ulong)(0)));
        if (S.Temp.fileStartDate == -693594)
        {
            S.datetime_decodeDate(S.StartDate, ref S.FileYear, ref S.FileMonth, ref S.FileDay);
        }
        else
        {
            S.datetime_decodeDate(S.Temp.fileStartDate, ref S.FileYear, ref S.FileMonth, ref S.FileDay);
        }
        while (!((R.feof(S.Fclimate.file) != 0)))
        {
            S.sstrncpy(S.FileLine, S.L0, unchecked((ulong)(0)));
            S.readFileLine(ref y, ref m);
            if ((y == S.FileYear) && (m == S.FileMonth))
            {
                break;
            }
        }
        if ((R.feof(S.Fclimate.file) != 0))
        {
            S.report_writeErrorMsg(S.ERR_CLIMATE_END_OF_FILE, S.Fclimate.name);
            return;
        }
        if (!(S.ErrorCode != 0))
        {
            S.FileElapsedDays = 0;
            S.FileLastDay = S.datetime_daysPerMonth(S.FileYear, S.FileMonth);
            S.readFileValues();
            for (i = S.TMIN; i <= S.WIND; i++)
            {
                if (S.FileData[i][S.FileDay] == -10000000000.0)
                {
                    continue;
                }
                S.FileValue[i] = S.FileData[i][S.FileDay];
            }
        }
    }

    // climate.c:598
    public static void climate_initState()
    {
        S.LastDay = -693594;
        S.Temp.tmax = -10000000000.0;
        S.Snow.removed = 0.0;
        S.NextEvapDate = S.StartDate;
        S.NextEvapRate = 0.0;
        if ((S.Evap.type == S.TIMESERIES_EVAP) && (S.Evap.tSeries >= 0))
        {
            S.table_getFirstEntry(S.Tseries[S.Evap.tSeries], ref S.NextEvapDate, ref S.NextEvapRate);
            if (S.NextEvapDate < S.StartDate)
            {
                S.setNextEvapDate(S.StartDate);
            }
            S.Evap.rate = S.NextEvapRate / S.UCF(S.EVAPRATE);
            S.setNextEvapDate(S.NextEvapDate);
        }
        if (S.Evap.type == S.TEMPERATURE_EVAP)
        {
            S.Tma.maxCount = (int)(56 / 8);
            S.Tma.count = 0;
            S.Tma.front = 0;
            S.Tma.tAve = 0.0;
            S.Tma.tRng = 0.0;
        }
    }

    // climate.c:641
    public static void climate_setState(double theDate)
    {
        if (S.Fclimate.mode == S.USE_FILE)
        {
            S.updateFileValues(theDate);
        }
        if (S.Temp.dataSource != S.NO_TEMP)
        {
            S.setTemp(theDate);
        }
        S.setEvap(theDate);
        S.setWind(theDate);
        S.Adjust.rainFactor = S.Adjust.rain[S.datetime_monthOfYear(theDate) - 1];
        S.Adjust.hydconFactor = S.Adjust.hydcon[S.datetime_monthOfYear(theDate) - 1];
        S.setNextEvapDate(theDate);
    }

    // climate.c:659
    public static double climate_getNextEvapDate()
    {
        return S.NextEvapDate;
    }

    // climate.c:671
    public static void setNextEvapDate(double theDate)
    {
        int yr = 0;
        int mon = 0;
        int day = 0;
        int k = 0;
        double d = 0.0;
        double e = 0.0;
        if (S.NextEvapDate > theDate)
        {
            return;
        }
        switch (S.Evap.type)
        {
            case S.CONSTANT_EVAP:
                S.NextEvapDate = theDate + 365.0;
                break;
            case S.MONTHLY_EVAP:
                S.datetime_decodeDate(theDate, ref yr, ref mon, ref day);
                if (mon == 12)
                {
                    mon = 1;
                    yr++;
                }
                else
                {
                    mon++;
                }
                S.NextEvapDate = S.datetime_encodeDate(yr, mon, 1);
                break;
            case S.TIMESERIES_EVAP:
                k = S.Evap.tSeries;
                if (k >= 0)
                {
                    S.NextEvapDate = theDate + 365.0;
                    while (((S.table_getNextEntry(S.Tseries[k], ref d, ref e) != 0)) && (d <= S.EndDateTime))
                    {
                        if (d >= theDate)
                        {
                            S.NextEvapDate = d;
                            S.NextEvapRate = e;
                            break;
                        }
                    }
                }
                break;
            case S.FILE_EVAP:
                S.NextEvapDate = Math.Floor(theDate) + 1.0;
                break;
            default:
                S.NextEvapDate = theDate + 365.0;
                break;
        }
    }

    // climate.c:734
    public static void updateFileValues(double theDate)
    {
        int i = 0;
        int deltaDays = 0;
        deltaDays = R.D2I((Math.Floor(theDate) - Math.Floor(S.StartDateTime)));
        if (deltaDays > S.FileElapsedDays)
        {
            S.FileElapsedDays++;
            S.FileDay++;
            if (S.FileDay > S.FileLastDay)
            {
                S.FileMonth++;
                if (S.FileMonth > 12)
                {
                    S.FileMonth = 1;
                    S.FileYear++;
                }
                S.readFileValues();
                S.FileDay = 1;
                S.FileLastDay = S.datetime_daysPerMonth(S.FileYear, S.FileMonth);
            }
            for (i = S.TMIN; i <= S.WIND; i++)
            {
                if (S.FileData[i][S.FileDay] == -10000000000.0)
                {
                    continue;
                }
                S.FileValue[i] = S.FileData[i][S.FileDay];
            }
        }
    }

    // climate.c:782
    public static void setTemp(double theDate)
    {
        int j = 0;
        int k = 0;
        int mon = 0;
        int day = 0;
        double theDay = 0.0;
        double hour = 0.0;
        double tmp = 0.0;
        mon = S.datetime_monthOfYear(theDate);
        theDay = Math.Floor(theDate);
        if (theDay > S.LastDay)
        {
            day = S.datetime_dayOfYear(theDate);
            if (S.Temp.dataSource == S.FILE_TEMP)
            {
                S.Tmin = S.FileValue[S.TMIN] + S.Adjust.temp[mon - 1];
                S.Tmax = S.FileValue[S.TMAX] + S.Adjust.temp[mon - 1];
                if (S.Tmin > S.Tmax)
                {
                    tmp = S.Tmin;
                    S.Tmin = S.Tmax;
                    S.Tmax = tmp;
                }
                S.updateTempTimes(day);
                if (S.Evap.type == S.TEMPERATURE_EVAP)
                {
                    S.updateTempMoveAve(S.Tmin, S.Tmax);
                    S.FileValue[S.EVAP] = S.getTempEvap(day, S.Tma.tAve, S.Tma.tRng);
                }
            }
            S.Snow.season = Math.Sin(0.0172615 * (day - 81.0));
            for (j = 0; j < S.Nobjects[S.SNOWMELT]; j++)
            {
                S.snow_setMeltCoeffs(j, S.Snow.season);
            }
            S.LastDay = theDate;
        }
        if (S.Temp.dataSource == S.FILE_TEMP)
        {
            hour = (theDate - theDay) * 24.0;
            if (hour < S.Hrsr)
            {
                S.Temp.ta = S.Tmin + S.Trng1 / 2.0 * Math.Sin(3.141592654 / S.Dydif * (S.Hrsr - hour));
            }
            else if ((hour >= S.Hrsr) && (hour <= S.Hrss))
            {
                S.Temp.ta = S.Tave + S.Trng * Math.Sin(3.141592654 / S.Dhrdy * (S.Hrday - hour));
            }
            else
            {
                S.Temp.ta = S.Tmax - S.Trng * Math.Sin(3.141592654 / S.Dydif * (hour - S.Hrss));
            }
        }
        if (S.Temp.dataSource == S.TSERIES_TEMP)
        {
            k = S.Temp.tSeries;
            if (k >= 0)
            {
                S.Temp.ta = S.table_tseriesLookup(S.Tseries[k], theDate, 1);
                if (S.UnitSystem == S.SI)
                {
                    S.Temp.ta = (9.0 / 5.0) * S.Temp.ta + 32.0;
                }
                S.Temp.ta += S.Adjust.temp[mon - 1];
            }
        }
        S.Temp.ea = 8.1175e6 * Math.Exp(-7701.544 / (S.Temp.ta + 405.0265));
    }

    // climate.c:872
    public static void setEvap(double theDate)
    {
        int k = 0;
        int mon = S.datetime_monthOfYear(theDate);
        switch (S.Evap.type)
        {
            case S.CONSTANT_EVAP:
                S.Evap.rate = S.Evap.monthlyEvap[0] / S.UCF(S.EVAPRATE);
                break;
            case S.MONTHLY_EVAP:
                S.Evap.rate = S.Evap.monthlyEvap[mon - 1] / S.UCF(S.EVAPRATE);
                break;
            case S.TIMESERIES_EVAP:
                if (theDate >= S.NextEvapDate)
                {
                    S.Evap.rate = S.NextEvapRate / S.UCF(S.EVAPRATE);
                }
                break;
            case S.FILE_EVAP:
                S.Evap.rate = S.FileValue[S.EVAP] / S.UCF(S.EVAPRATE);
                S.Evap.rate *= S.Evap.panCoeff[mon - 1];
                break;
            case S.TEMPERATURE_EVAP:
                S.Evap.rate = S.FileValue[S.EVAP] / S.UCF(S.EVAPRATE);
                break;
            default:
                S.Evap.rate = 0.0;
                break;
        }
        S.Evap.rate += S.Adjust.evap[mon - 1];
        S.Evap.recoveryFactor = 1.0;
        k = S.Evap.recoveryPattern;
        if ((k >= 0) && (S.Pattern[k].type == S.MONTHLY_PATTERN))
        {
            S.Evap.recoveryFactor = S.Pattern[k].factor[mon - 1];
        }
    }

    // climate.c:923
    public static void setWind(double theDate)
    {
        int yr = 0;
        int mon = 0;
        int day = 0;
        switch (S.Wind.type)
        {
            case S.MONTHLY_WIND:
                S.datetime_decodeDate(theDate, ref yr, ref mon, ref day);
                S.Wind.ws = S.Wind.aws[mon - 1] / S.UCF(S.WINDSPEED);
                break;
            case S.FILE_WIND:
                S.Wind.ws = S.FileValue[S.WIND];
                break;
            default:
                S.Wind.ws = 0.0;
                break;
        }
    }

    // climate.c:949
    public static void updateTempTimes(int day)
    {
        double decl = 0.0;
        double hrang = 0.0;
        double arg = 0.0;
        decl = 0.40928 * Math.Cos(0.017202 * (172.0 - day));
        arg = -(Math.Tan(decl)) * S.Temp.tanAnglat;
        if (arg <= -1.0)
        {
            arg = 3.141592654;
        }
        else if (arg >= 1.0)
        {
            arg = 0.0;
        }
        else
        {
            arg = Math.Acos(arg);
        }
        hrang = 3.8197 * arg;
        S.Hrsr = 12.0 - hrang + S.Temp.dtlong;
        S.Hrss = 12.0 + hrang + S.Temp.dtlong - 3.0;
        S.Dhrdy = S.Hrsr - S.Hrss;
        S.Dydif = 24.0 + S.Hrsr - S.Hrss;
        S.Hrday = (S.Hrsr + S.Hrss) / 2.0;
        S.Tave = (S.Tmin + S.Tmax) / 2.0;
        S.Trng = (S.Tmax - S.Tmin) / 2.0;
        if (S.Temp.tmax == -10000000000.0)
        {
            S.Trng1 = S.Tmax - S.Tmin;
        }
        else
        {
            S.Trng1 = S.Temp.tmax - S.Tmin;
        }
        S.Temp.tmax = S.Tmax;
    }

    // climate.c:981
    public static double getTempEvap(int day, double tave, double trng)
    {
        double a = 2.0 * 3.141592654 / 365.0;
        double ta = (tave - 32.0) * 5.0 / 9.0;
        double tr = trng * 5.0 / 9.0;
        double lamda = 2.50 - 0.002361 * ta;
        double dr = 1.0 + 0.033 * Math.Cos(a * day);
        double phi = S.Temp.anglat * 2.0 * 3.141592654 / 360.0;
        double del = 0.4093 * Math.Sin(a * (284.0 + (double)(day)));
        double omega = Math.Acos(-(Math.Tan(phi)) * Math.Tan(del));
        double ra = 37.6 * dr * (omega * Math.Sin(phi) * Math.Sin(del) + Math.Cos(phi) * Math.Cos(del) * Math.Sin(omega));
        double e = 0.0023 * ra / lamda * Math.Sqrt(tr) * (ta + 17.8);
        if (e < 0.0)
        {
            e = 0.0;
        }
        if (S.UnitSystem == S.US)
        {
            e /= 25.4;
        }
        return e;
    }

    // climate.c:1010
    public static int getFileFormat()
    {
        CPtr recdType = R.BufFrom(S.L0, 4);
        CPtr elemType = R.BufFrom(S.L0, 4);
        CPtr filler = R.BufFrom(S.L0, 5);
        CPtr staID = R.Buf(80);
        CPtr s = R.Buf(80);
        CPtr line = R.Buf(1024);
        int y = 0;
        int m = 0;
        int d = 0;
        int n = 0;
        if ((R.fgets(line, 1024, S.Fclimate.file)).IsNull)
        {
            return S.UNKNOWN_FORMAT;
        }
        S.sstrncpy(recdType, line, unchecked((ulong)(3)));
        S.sstrncpy(filler, (line + 23), unchecked((ulong)(4)));
        if ((R.strcmp(recdType, S.L14) == 0) && (R.strcmp(filler, S.L15) == 0))
        {
            return S.TD3200;
        }
        if (R.strlen(line) >= unchecked((ulong)(233)))
        {
            S.sstrncpy(elemType, (line + 13), unchecked((ulong)(3)));
            n = R.atoi(elemType);
            if (((n == 1) || (n == 2)) || (n == 151))
            {
                return S.DLY0204;
            }
        }
        n = R.sscanf(line, S.L16, staID, R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v), s);
        if (n == 5)
        {
            return S.USER_PREPARED;
        }
        if ((S.isGhcndFormat(line) != 0))
        {
            return S.GHCND;
        }
        return S.UNKNOWN_FORMAT;
    }

    // climate.c:1055
    public static void readFileLine(ref int y, ref int m)
    {
        while (R.strlen(S.FileLine) == unchecked((ulong)(0)))
        {
            if ((R.fgets(S.FileLine, 1024, S.Fclimate.file)).IsNull)
            {
                return;
            }
            if (S.FileLine[0] == 10)
            {
                S.FileLine[0] = 0;
            }
        }
        switch (S.FileFormat)
        {
            case S.USER_PREPARED:
                S.readUserFileLine(ref y, ref m);
                break;
            case S.TD3200:
                S.readTD3200FileLine(ref y, ref m);
                break;
            case S.DLY0204:
                S.readDLY0204FileLine(ref y, ref m);
                break;
            case S.GHCND:
                S.readGhcndFileLine(ref y, ref m);
                break;
        }
    }

    // climate.c:1082
    public static void readUserFileLine(ref int y, ref int m)
    {
        int n = 0;
        CPtr staID = R.Buf(80);
        n = R.Back(R.sscanf(S.FileLine, S.L17, staID, R.Slot, R.Slot), ref y, ref m);
        if (n < 3)
        {
            S.report_writeErrorMsg(S.ERR_CLIMATE_FILE_READ, S.Fclimate.name);
        }
    }

    // climate.c:1101
    public static void readTD3200FileLine(ref int y, ref int m)
    {
        CPtr recdType = R.BufFrom(S.L0, 4);
        CPtr year = R.BufFrom(S.L0, 5);
        CPtr month = R.BufFrom(S.L0, 3);
        if (R.strlen(S.FileLine) < unchecked((ulong)(30)))
        {
            S.report_writeErrorMsg(S.ERR_CLIMATE_FILE_READ, S.Fclimate.name);
            return;
        }
        S.sstrncpy(recdType, S.FileLine, unchecked((ulong)(3)));
        if (R.strcmp(recdType, S.L14) != 0)
        {
            S.report_writeErrorMsg(S.ERR_CLIMATE_FILE_READ, S.Fclimate.name);
            return;
        }
        S.sstrncpy(year, (S.FileLine + 17), unchecked((ulong)(4)));
        S.sstrncpy(month, (S.FileLine + 21), unchecked((ulong)(2)));
        y = R.atoi(year);
        m = R.atoi(month);
    }

    // climate.c:1137
    public static void readDLY0204FileLine(ref int y, ref int m)
    {
        CPtr year = R.BufFrom(S.L0, 5);
        CPtr month = R.BufFrom(S.L0, 3);
        if (R.strlen(S.FileLine) < unchecked((ulong)(16)))
        {
            S.report_writeErrorMsg(S.ERR_CLIMATE_FILE_READ, S.Fclimate.name);
            return;
        }
        S.sstrncpy(year, (S.FileLine + 7), unchecked((ulong)(4)));
        S.sstrncpy(month, (S.FileLine + 11), unchecked((ulong)(2)));
        y = R.atoi(year);
        m = R.atoi(month);
    }

    // climate.c:1164
    public static void readFileValues()
    {
        int i = 0;
        int j = 0;
        int y = 0;
        int m = 0;
        for (i = 0; i < S.MAXCLIMATEVARS; i++)
        {
            for (j = 0; j < S.MAXDAYSPERMONTH; j++)
            {
                S.FileData[i][j] = -10000000000.0;
            }
        }
        while (!(S.ErrorCode != 0))
        {
            if ((R.feof(S.Fclimate.file) != 0))
            {
                return;
            }
            S.readFileLine(ref y, ref m);
            if ((y > S.FileYear) || (m > S.FileMonth))
            {
                return;
            }
            switch (S.FileFormat)
            {
                case S.USER_PREPARED:
                    S.parseUserFileLine();
                    break;
                case S.TD3200:
                    S.parseTD3200FileLine();
                    break;
                case S.DLY0204:
                    S.parseDLY0204FileLine();
                    break;
                case S.GHCND:
                    S.parseGhcndFileLine();
                    break;
            }
            S.sstrncpy(S.FileLine, S.L0, unchecked((ulong)(0)));
        }
    }

    // climate.c:1201
    public static void parseUserFileLine()
    {
        int n = 0;
        int y = 0;
        int m = 0;
        int d = 0;
        CPtr staID = R.Buf(80);
        CPtr s0 = R.BufFrom(S.L0, 80);
        CPtr s1 = R.BufFrom(S.L0, 80);
        CPtr s2 = R.BufFrom(S.L0, 80);
        CPtr s3 = R.BufFrom(S.L0, 80);
        double x = 0.0;
        n = R.sscanf(S.FileLine, S.L18, staID, R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v), s0, s1, s2, s3);
        if (n < 4)
        {
            return;
        }
        if ((d < 1) || (d > 31))
        {
            return;
        }
        if ((R.strlen(s0) > unchecked((ulong)(0))) && (s0[0] != '*'))
        {
            x = R.atof(s0);
            if (S.UnitSystem == S.SI)
            {
                x = 9.0 / 5.0 * x + 32.0;
            }
            S.FileData[S.TMAX][d] = x;
        }
        if ((R.strlen(s1) > unchecked((ulong)(0))) && (s1[0] != '*'))
        {
            x = R.atof(s1);
            if (S.UnitSystem == S.SI)
            {
                x = 9.0 / 5.0 * x + 32.0;
            }
            S.FileData[S.TMIN][d] = x;
        }
        if ((R.strlen(s2) > unchecked((ulong)(0))) && (s2[0] != '*'))
        {
            S.FileData[S.EVAP][d] = R.atof(s2);
        }
        if ((R.strlen(s3) > unchecked((ulong)(0))) && (s3[0] != '*'))
        {
            S.FileData[S.WIND][d] = R.atof(s3);
        }
    }

    // climate.c:1249
    public static void parseTD3200FileLine()
    {
        int i = 0;
        CPtr param = R.BufFrom(S.L0, 5);
        S.sstrncpy(param, (S.FileLine + 11), unchecked((ulong)(4)));
        for (i = 0; i < S.MAXCLIMATEVARS; i++)
        {
            if (R.strcmp(param, S.ClimateVarWords[i]) == 0)
            {
                S.setTD3200FileValues(i);
            }
        }
    }

    // climate.c:1271
    public static void setTD3200FileValues(int i)
    {
        CPtr valCount = R.BufFrom(S.L0, 4);
        CPtr day = R.BufFrom(S.L0, 3);
        CPtr sign = R.BufFrom(S.L0, 2);
        CPtr value = R.BufFrom(S.L0, 6);
        CPtr flag2 = R.BufFrom(S.L0, 2);
        double x = 0.0;
        int nValues = 0;
        int j = 0;
        int k = 0;
        int d = 0;
        S.sstrncpy(valCount, (S.FileLine + 27), unchecked((ulong)(3)));
        nValues = R.atoi(valCount);
        if ((int)(R.strlen(S.FileLine)) >= 12 * nValues + 30)
        {
            for (j = 0; j < nValues; j++)
            {
                k = 30 + j * 12;
                S.sstrncpy(day, (S.FileLine + k), unchecked((ulong)(2)));
                S.sstrncpy(sign, (S.FileLine + k + 4), unchecked((ulong)(1)));
                S.sstrncpy(value, (S.FileLine + k + 5), unchecked((ulong)(5)));
                S.sstrncpy(flag2, (S.FileLine + k + 11), unchecked((ulong)(1)));
                d = R.atoi(day);
                if ((((R.strcmp(value, S.L19) != 0) && (((flag2[0] == '0') || (flag2[0] == '1')))) && (d > 0)) && (d <= 31))
                {
                    x = R.atof(value);
                    if (sign[0] == '-')
                    {
                        x = -x;
                    }
                    if (i == S.EVAP)
                    {
                        x /= 100.0;
                        if (S.UnitSystem == S.SI)
                        {
                            x *= 25.4;
                        }
                    }
                    if (i == S.WIND)
                    {
                        x /= 24.0;
                    }
                    S.FileData[i][d] = x;
                }
            }
        }
    }

    // climate.c:1336
    public static void parseDLY0204FileLine()
    {
        int j = 0;
        int k = 0;
        int p = 0;
        CPtr param = R.BufFrom(S.L0, 4);
        CPtr sign = R.BufFrom(S.L0, 2);
        CPtr value = R.BufFrom(S.L0, 6);
        CPtr code = R.BufFrom(S.L0, 2);
        double x = 0.0;
        S.sstrncpy(param, (S.FileLine + 13), unchecked((ulong)(3)));
        p = R.atoi(param);
        if (p == 1)
        {
            p = S.TMAX;
        }
        else if (p == 2)
        {
            p = S.TMIN;
        }
        else if (p == 151)
        {
            p = S.EVAP;
        }
        else
        {
            return;
        }
        if (R.strlen(S.FileLine) < unchecked((ulong)(233)))
        {
            return;
        }
        k = 16;
        for (j = 1; j <= 31; j++)
        {
            S.sstrncpy(sign, (S.FileLine + k), unchecked((ulong)(1)));
            S.sstrncpy(value, (S.FileLine + k + 1), unchecked((ulong)(5)));
            S.sstrncpy(code, (S.FileLine + k + 6), unchecked((ulong)(1)));
            k += 7;
            if ((R.strcmp(value, S.L19) != 0) && (R.strcmp(value, S.L20) != 0))
            {
                switch (p)
                {
                    case S.TMAX:
                    case S.TMIN:
                        x = R.atof(value) / 10.0;
                        if (sign[0] == '-')
                        {
                            x = -x;
                        }
                        x = 9.0 / 5.0 * x + 32.0;
                        break;
                    case S.EVAP:
                        x = R.atof(value) / 10.0;
                        if (S.UnitSystem == S.US)
                        {
                            x /= 25.4;
                        }
                        break;
                    default:
                        return;
                }
                S.FileData[p][j] = x;
            }
        }
    }

    // climate.c:1401
    public static int isGhcndFormat(CPtr line)
    {
        int i = 0;
        CPtr ptr = CPtr.Null;
        ptr = R.strstr(line, S.L21);
        if (ptr.IsNull)
        {
            return 0;
        }
        S.FileDateFieldPos = (int)((CPtr.Diff(ptr, line)));
        for (i = S.TMIN; i <= S.WIND; i++)
        {
            S.FileFieldPos[i] = -1;
        }
        ptr = R.strstr(line, S.L22);
        if (!(ptr).IsNull)
        {
            S.FileFieldPos[S.TMIN] = (int)((CPtr.Diff(ptr, line)));
        }
        ptr = R.strstr(line, S.L23);
        if (!(ptr).IsNull)
        {
            S.FileFieldPos[S.TMAX] = (int)((CPtr.Diff(ptr, line)));
        }
        ptr = R.strstr(line, S.L7);
        if (!(ptr).IsNull)
        {
            S.FileFieldPos[S.EVAP] = (int)((CPtr.Diff(ptr, line)));
        }
        S.FileWindType = S.WDMV;
        ptr = R.strstr(line, S.L24);
        if (ptr.IsNull)
        {
            S.FileWindType = S.AWND;
            ptr = R.strstr(line, S.L25);
        }
        if (!(ptr).IsNull)
        {
            S.FileFieldPos[S.WIND] = (int)((CPtr.Diff(ptr, line)));
        }
        for (i = S.TMIN; i <= S.WIND; i++)
        {
            if (S.FileFieldPos[i] >= 0)
            {
                return 1;
            }
        }
        return 0;
    }

    // climate.c:1445
    public static void readGhcndFileLine(ref int y, ref int m)
    {
        int n = R.Back(R.sscanf((S.FileLine + S.FileDateFieldPos), S.L26, R.Slot, R.Slot), ref y, ref m);
        if (n != 2)
        {
            y = -99999;
            m = -99999;
        }
    }

    // climate.c:1463
    public static void parseGhcndFileLine()
    {
        int y = 0;
        int m = 0;
        int d = 0;
        int n = 0;
        int i = 0;
        double v = 0.0;
        n = R.sscanf((S.FileLine + S.FileDateFieldPos), S.L27, R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v));
        if (n < 3)
        {
            return;
        }
        if ((d < 1) || (d > 31))
        {
            return;
        }
        for (i = S.TMIN; i <= S.WIND; i++)
        {
            if (S.FileFieldPos[i] >= 0)
            {
                if (R.sscanf((S.FileLine + S.FileFieldPos[i]), S.L28, R.RefD(__v => v = __v)) > 0)
                {
                    if (Math.Abs(v) < 9999.0)
                    {
                        S.FileData[i][d] = S.convertGhcndValue(i, v);
                    }
                }
            }
        }
    }

    // climate.c:1496
    public static double convertGhcndValue(int var, double v)
    {
        switch (var)
        {
            case S.TMIN:
            case S.TMAX:
                switch (S.FileTempUnits)
                {
                    case S.DEG_C10:
                        return v / 10.0 * 9.0 / 5.0 + 32.0;
                    case S.DEG_C:
                        return v * 9.0 / 5.0 + 32.0;
                    default:
                        return v;
                }
                goto case S.EVAP;
            case S.EVAP:
                switch (S.FileTempUnits)
                {
                    case S.DEG_C10:
                        v /= 10.0;
                        if (S.UnitSystem == S.US)
                        {
                            v /= 25.4;
                        }
                        return v;
                    case S.DEG_C:
                        if (S.UnitSystem == S.US)
                        {
                            v /= 25.4;
                        }
                        return v;
                    default:
                        if (S.UnitSystem == S.SI)
                        {
                            v *= 25.4;
                        }
                        return v;
                }
                goto case S.WIND;
            case S.WIND:
                switch (S.FileTempUnits)
                {
                    case S.DEG_C10:
                        if (S.FileWindType == S.WDMV)
                        {
                            return v * 0.62137 / 24.0;
                        }
                        else
                        {
                            return v / 10.0 / 1000.0 * 0.62137 * 3600.0;
                        }
                    case S.DEG_C:
                        if (S.FileWindType == S.WDMV)
                        {
                            return v * 0.62137 / 24.0;
                        }
                        else
                        {
                            return v / 1000.0 * 0.62137 * 3600.0;
                        }
                    default:
                        if (S.FileWindType == S.WDMV)
                        {
                            return v / 24.0;
                        }
                        else
                        {
                            return v;
                        }
                }
                goto default;
            default:
                return v;
        }
    }

    // climate.c:1569
    public static void updateTempMoveAve(double tmin, double tmax)
    {
        double ta = 0.0;
        double tr = 0.0;
        int kount = S.Tma.count;
        double count = kount;
        ta = (tmin + tmax) / 2.0;
        tr = Math.Abs(tmax - tmin);
        if (kount == S.Tma.maxCount)
        {
            S.Tma.tAve = (S.Tma.tAve * count + ta - S.Tma.ta[S.Tma.front]) / count;
            S.Tma.tRng = (S.Tma.tRng * count + tr - S.Tma.tr[S.Tma.front]) / count;
            S.Tma.ta[S.Tma.front] = ta;
            S.Tma.tr[S.Tma.front] = tr;
            S.Tma.front++;
            if (S.Tma.front == count)
            {
                S.Tma.front = 0;
            }
        }
        else
        {
            S.Tma.tAve = (S.Tma.tAve * count + ta) / (count + 1);
            S.Tma.tRng = (S.Tma.tRng * count + tr) / (count + 1);
            S.Tma.ta[S.Tma.front] = ta;
            S.Tma.tr[S.Tma.front] = tr;
            S.Tma.count++;
            S.Tma.front++;
            if (S.Tma.count == S.Tma.maxCount)
            {
                S.Tma.front = 0;
            }
        }
    }

}
}
