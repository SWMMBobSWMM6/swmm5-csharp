// Generated from EPA SWMM 5.2.4 (datetime.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static CPtr[] MonthTxt = new CPtr[] { S.L695, S.L696, S.L697, S.L698, S.L699, S.L700, S.L701, S.L702, S.L703, S.L704, S.L705, S.L706, CPtr.Null };
    public static int[][] DaysPerMonth = new int[][] { new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 }, new int[] { 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 } };
    public static int DateDelta = 693594;
    public static double SecsPerDay = 86400.0;
    public static int DateFormat = 0;

    // datetime.c:51
    public static void divMod(int n, int d, ref int result, ref int remainder)
    {
        if (d == 0)
        {
            result = 0;
            remainder = 0;
        }
        else
        {
            result = n / d;
            remainder = n - d * (result);
        }
    }

    // datetime.c:74
    public static int isLeapYear(int year)
    {
        if ((year % 4 == 0) && (((year % 100 != 0) || (year % 400 == 0))))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    // datetime.c:89
    public static int datetime_findMonth(CPtr month)
    {
        int i = 0;
        for (i = 0; i < 12; i++)
        {
            if ((((((((month[0]) >= 'a') && ((month[0]) <= 'z')) ? ((month[0]) & ~32) : (month[0]))) == S.MonthTxt[i][0]) && ((((((month[1]) >= 'a') && ((month[1]) <= 'z')) ? ((month[1]) & ~32) : (month[1]))) == S.MonthTxt[i][1])) && ((((((month[2]) >= 'a') && ((month[2]) <= 'z')) ? ((month[2]) & ~32) : (month[2]))) == S.MonthTxt[i][2]))
            {
                return i + 1;
            }
        }
        return 0;
    }

    // datetime.c:108
    public static double datetime_encodeDate(int year, int month, int day)
    {
        int i = 0;
        int j = 0;
        i = S.isLeapYear(year);
        if ((((((year >= 1) && (year <= 9999)) && (month >= 1)) && (month <= 12)) && (day >= 1)) && (day <= S.DaysPerMonth[i][month - 1]))
        {
            for (j = 0; j < month - 1; j++)
            {
                day += S.DaysPerMonth[i][j];
            }
            i = year - 1;
            i = i * 365 + i / 4 - i / 100 + i / 400 + day - S.DateDelta;
            return i;
        }
        else
        {
            return -S.DateDelta;
        }
    }

    // datetime.c:136
    public static double datetime_encodeTime(int hour, int minute, int second)
    {
        int s = 0;
        if (((hour >= 0) && (minute >= 0)) && (second >= 0))
        {
            s = (hour * 3600 + minute * 60 + second);
            return (double)(s) / S.SecsPerDay;
        }
        else
        {
            return 0.0;
        }
    }

    // datetime.c:158
    public static void datetime_decodeDate(double date, ref int year, ref int month, ref int day)
    {
        int D1 = 0;
        int D4 = 0;
        int D100 = 0;
        int D400 = 0;
        int y = 0;
        int m = 0;
        int d = 0;
        int i = 0;
        int k = 0;
        int t = 0;
        D1 = 365;
        D4 = D1 * 4 + 1;
        D100 = D4 * 25 - 1;
        D400 = D100 * 4 + 1;
        t = R.D2I((Math.Floor(date))) + S.DateDelta;
        if (t <= 0)
        {
            year = 0;
            month = 1;
            day = 1;
        }
        else
        {
            t--;
            y = 1;
            while (t >= D400)
            {
                t -= D400;
                y += 400;
            }
            S.divMod(t, D100, ref i, ref d);
            if (i == 4)
            {
                i--;
                d += D100;
            }
            y += i * 100;
            S.divMod(d, D4, ref i, ref d);
            y += i * 4;
            S.divMod(d, D1, ref i, ref d);
            if (i == 4)
            {
                i--;
                d += D1;
            }
            y += i;
            k = S.isLeapYear(y);
            m = 1;
            for (; ; )
            {
                i = S.DaysPerMonth[k][m - 1];
                if (d < i)
                {
                    break;
                }
                d -= i;
                m++;
            }
            year = y;
            month = m;
            day = d + 1;
        }
    }

    // datetime.c:224
    public static void datetime_decodeTime(double time, ref int h, ref int m, ref int s)
    {
        int secs = 0;
        int mins = 0;
        double fracDay = (time - Math.Floor(time)) * S.SecsPerDay;
        secs = R.D2I((Math.Floor(fracDay + 0.5)));
        if (secs >= 86400)
        {
            secs = 86399;
        }
        S.divMod(secs, 60, ref mins, ref s);
        S.divMod(mins, 60, ref h, ref m);
        if (h > 23)
        {
            h = 0;
        }
    }

    // datetime.c:245
    public static void datetime_dateToStr(double date, CPtr s)
    {
        int y = 0;
        int m = 0;
        int d = 0;
        S.datetime_decodeDate(date, ref y, ref m, ref d);
        switch (S.DateFormat)
        {
            case 0:
                R.snprintf(s, unchecked((ulong)(12)), S.L36, y, S.MonthTxt[m - 1], d);
                break;
            case 1:
                R.snprintf(s, unchecked((ulong)(12)), S.L37, m, d, y);
                break;
            default:
                R.snprintf(s, unchecked((ulong)(12)), S.L38, d, S.MonthTxt[m - 1], y);
                break;
        }
    }

    // datetime.c:270
    public static void datetime_timeToStr(double time, CPtr s)
    {
        int hr = 0;
        int min = 0;
        int sec = 0;
        S.datetime_decodeTime(time, ref hr, ref min, ref sec);
        R.snprintf(s, unchecked((ulong)(9)), S.L39, hr, min, sec);
    }

    // datetime.c:284
    public static int datetime_strToDate(CPtr s, ref double d)
    {
        int yr = 0;
        int mon = 0;
        int day = 0;
        int n = 0;
        CPtr month = R.Buf(4);
        int sep1 = 0;
        int sep2 = 0;
        d = -S.DateDelta;
        if ((!(R.strchr(s, '-')).IsNull) || (!(R.strchr(s, '/')).IsNull))
        {
            switch (S.DateFormat)
            {
                case 0:
                    n = R.sscanf(s, S.L40, R.RefI(__v => yr = __v), R.RefI(__v => sep1 = __v), R.RefI(__v => mon = __v), R.RefI(__v => sep2 = __v), R.RefI(__v => day = __v));
                    if (n < 3)
                    {
                        mon = 0;
                        n = R.sscanf(s, S.L41, R.RefI(__v => yr = __v), R.RefI(__v => sep1 = __v), month, R.RefI(__v => sep2 = __v), R.RefI(__v => day = __v));
                        if (n < 3)
                        {
                            return 0;
                        }
                    }
                    break;
                case 2:
                    n = R.sscanf(s, S.L40, R.RefI(__v => day = __v), R.RefI(__v => sep1 = __v), R.RefI(__v => mon = __v), R.RefI(__v => sep2 = __v), R.RefI(__v => yr = __v));
                    if (n < 3)
                    {
                        mon = 0;
                        n = R.sscanf(s, S.L41, R.RefI(__v => day = __v), R.RefI(__v => sep1 = __v), month, R.RefI(__v => sep2 = __v), R.RefI(__v => yr = __v));
                        if (n < 3)
                        {
                            return 0;
                        }
                    }
                    break;
                default:
                    n = R.sscanf(s, S.L40, R.RefI(__v => mon = __v), R.RefI(__v => sep1 = __v), R.RefI(__v => day = __v), R.RefI(__v => sep2 = __v), R.RefI(__v => yr = __v));
                    if (n < 3)
                    {
                        mon = 0;
                        n = R.sscanf(s, S.L42, month, R.RefI(__v => sep1 = __v), R.RefI(__v => day = __v), R.RefI(__v => sep2 = __v), R.RefI(__v => yr = __v));
                        if (n < 3)
                        {
                            return 0;
                        }
                    }
                    break;
            }
            if (mon == 0)
            {
                mon = S.datetime_findMonth(month);
            }
            d = S.datetime_encodeDate(yr, mon, day);
        }
        if (d == -S.DateDelta)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    // datetime.c:338
    public static int datetime_strToTime(CPtr s, ref double t)
    {
        int n = 0;
        int hr = 0;
        int min = 0;
        int sec = 0;
        CPtr endptr = CPtr.Null;
        t = R.strtod(s, ref endptr);
        if (endptr[0] == 0)
        {
            t /= 24.0;
            return 1;
        }
        t = 0.0;
        n = R.sscanf(s, S.L43, R.RefI(__v => hr = __v), R.RefI(__v => min = __v), R.RefI(__v => sec = __v));
        if (n == 0)
        {
            return 0;
        }
        t = S.datetime_encodeTime(hr, min, sec);
        if (((hr >= 0) && (min >= 0)) && (sec >= 0))
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    // datetime.c:369
    public static void datetime_setDateFormat(int fmt)
    {
        if ((fmt >= 0) && (fmt <= 1))
        {
            S.DateFormat = fmt;
        }
    }

    // datetime.c:381
    public static double datetime_addSeconds(double date1, double seconds)
    {
        double d = Math.Floor(date1);
        int h = 0;
        int m = 0;
        int s = 0;
        S.datetime_decodeTime(date1, ref h, ref m, ref s);
        return d + (3600.0 * h + 60.0 * m + s + seconds) / S.SecsPerDay;
    }

    // datetime.c:397
    public static double datetime_addDays(double date1, double date2)
    {
        double d1 = Math.Floor(date1);
        double d2 = Math.Floor(date2);
        int h1 = 0;
        int m1 = 0;
        int s1 = 0;
        int h2 = 0;
        int m2 = 0;
        int s2 = 0;
        S.datetime_decodeTime(date1, ref h1, ref m1, ref s1);
        S.datetime_decodeTime(date2, ref h2, ref m2, ref s2);
        return d1 + d2 + S.datetime_encodeTime(h1 + h2, m1 + m2, s1 + s2);
    }

    // datetime.c:416
    public static long datetime_timeDiff(double date1, double date2)
    {
        double d1 = Math.Floor(date1);
        double d2 = Math.Floor(date2);
        int h = 0;
        int m = 0;
        int s = 0;
        long s1 = 0L;
        long s2 = 0L;
        long secs = 0L;
        S.datetime_decodeTime(date1, ref h, ref m, ref s);
        s1 = 3600 * h + 60 * m + s;
        S.datetime_decodeTime(date2, ref h, ref m, ref s);
        s2 = 3600 * h + 60 * m + s;
        secs = R.D2I((Math.Floor((d1 - d2) * S.SecsPerDay + 0.5)));
        secs += (s1 - s2);
        return secs;
    }

    // datetime.c:439
    public static int datetime_monthOfYear(double date)
    {
        int year = 0;
        int month = 0;
        int day = 0;
        S.datetime_decodeDate(date, ref year, ref month, ref day);
        return month;
    }

    // datetime.c:453
    public static int datetime_dayOfYear(double date)
    {
        int year = 0;
        int month = 0;
        int day = 0;
        double startOfYear = 0.0;
        S.datetime_decodeDate(date, ref year, ref month, ref day);
        startOfYear = S.datetime_encodeDate(year, 1, 1);
        return R.D2I((Math.Floor(date - startOfYear))) + 1;
    }

    // datetime.c:469
    public static int datetime_dayOfWeek(double date)
    {
        int t = R.D2I((Math.Floor(date))) + S.DateDelta;
        return (t % 7) + 1;
    }

    // datetime.c:482
    public static int datetime_hourOfDay(double date)
    {
        int hour = 0;
        int min = 0;
        int sec = 0;
        S.datetime_decodeTime(date, ref hour, ref min, ref sec);
        return hour;
    }

    // datetime.c:496
    public static int datetime_daysPerMonth(int year, int month)
    {
        if ((month < 1) || (month > 12))
        {
            return 0;
        }
        return (S.DaysPerMonth[S.isLeapYear(year)])[month - 1];
    }

    // datetime.c:510
    public static void datetime_getTimeStamp(int fmt, double aDate, int stampSize, CPtr timeStamp)
    {
        CPtr dateStr = R.Buf(12);
        CPtr timeStr = R.Buf(9);
        int oldDateFormat = S.DateFormat;
        if (stampSize < 21)
        {
            return;
        }
        S.datetime_setDateFormat(fmt);
        S.datetime_dateToStr(aDate, dateStr);
        S.DateFormat = oldDateFormat;
        S.datetime_timeToStr(aDate, timeStr);
        R.snprintf(timeStamp, unchecked((ulong)(stampSize)), S.L44, dateStr, timeStr);
    }

}
}
