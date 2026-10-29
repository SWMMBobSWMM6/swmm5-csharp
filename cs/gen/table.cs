// Generated from EPA SWMM 5.2.4 (table.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // table.c:51
    public static double table_interpolate(double x, double x1, double y1, double x2, double y2)
    {
        double dx = x2 - x1;
        if (Math.Abs(dx) < 1.0e-20)
        {
            return (y1 + y2) / 2.0;
        }
        return y1 + (x - x1) * (y2 - y1) / dx;
    }

    // table.c:67
    public static int table_readCurve(CPtr[] tok, int ntoks)
    {
        int j = 0;
        int m = 0;
        int k = 0;
        int k1 = 1;
        double x = 0.0;
        double y = 0.0;
        if (ntoks < 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.CURVE, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        if (S.Curve[j].ID.IsNull)
        {
            S.Curve[j].ID = S.project_findID(S.CURVE, tok[0]);
            m = S.findmatch(tok[1], S.CurveTypeWords);
            if (m < 0)
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
            }
            S.Curve[j].curveType = m;
            if (ntoks == 2)
            {
                return 0;
            }
            k1 = 2;
        }
        for (k = k1; k < ntoks; k = k + 2)
        {
            if (k + 1 >= ntoks)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (!((S.getDouble(tok[k], ref x) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[k]);
            }
            if (!((S.getDouble(tok[k + 1], ref y) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[k + 1]);
            }
            S.table_addEntry(S.Curve[j], x, y);
        }
        return 0;
    }

    // table.c:113
    public static int table_readTimeseries(CPtr[] tok, int ntoks)
    {
        int j = 0;
        int k = 0;
        int state = 0;
        double x = 0.0;
        double y = 0.0;
        double d = 0.0;
        double t = 0.0;
        CPtr fname = R.Buf(260);
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.TSERIES, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        if (S.Tseries[j].ID.IsNull)
        {
            S.Tseries[j].ID = S.project_findID(S.TSERIES, tok[0]);
        }
        if ((S.strcomp(tok[1], S.L1) != 0))
        {
            S.sstrncpy(fname, tok[2], unchecked((ulong)(259)));
            S.sstrncpy(S.Tseries[j].file.name, S.addAbsolutePath(fname), unchecked((ulong)(259)));
            S.Tseries[j].file.mode = S.USE_FILE;
            return 0;
        }
        x = 0.0;
        k = 1;
        state = 1;
        while (k < ntoks)
        {
            switch (state)
            {
                case 1:
                    if ((S.datetime_strToDate(tok[k], ref d) != 0))
                    {
                        S.Tseries[j].lastDate = d;
                        k++;
                    }
                    state = 2;
                    break;
                case 2:
                    if (k >= ntoks)
                    {
                        return S.error_setInpError(S.ERR_ITEMS, S.L0);
                    }
                    if ((S.getDouble(tok[k], ref t) != 0))
                    {
                        t /= 24.0;
                    }
                    else if (!((S.datetime_strToTime(tok[k], ref t) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[k]);
                    }
                    x = S.Tseries[j].lastDate + t;
                    k++;
                    state = 3;
                    break;
                case 3:
                    if (k >= ntoks)
                    {
                        return S.error_setInpError(S.ERR_ITEMS, S.L0);
                    }
                    if (!((S.getDouble(tok[k], ref y) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[k]);
                    }
                    S.table_addEntry(S.Tseries[j], x, y);
                    k++;
                    state = 1;
                    break;
            }
        }
        return 0;
    }

    // table.c:208
    public static int table_addEntry(TTable table, double x, double y)
    {
        TTableEntry entry = null;
        entry = new TTableEntry();
        if (!(entry != null))
        {
            return 0;
        }
        entry.x = x;
        entry.y = y;
        entry.next = (null);
        if (table.firstEntry == null)
        {
            table.firstEntry = entry;
        }
        else
        {
            table.lastEntry.next = entry;
        }
        table.lastEntry = entry;
        return 1;
    }

    // table.c:231
    public static void table_deleteEntries(TTable table)
    {
        TTableEntry entry = null;
        TTableEntry nextEntry = null;
        entry = table.firstEntry;
        while ((entry != null))
        {
            nextEntry = entry.next;
            R.free(entry);
            entry = nextEntry;
        }
        table.firstEntry = (null);
        table.lastEntry = (null);
        table.thisEntry = (null);
        if ((table.file.file != null))
        {
            R.fclose(table.file.file);
            table.file.file = (null);
        }
    }

    // table.c:260
    public static void table_init(TTable table)
    {
        table.ID = CPtr.Null;
        table.refersTo = -1;
        table.firstEntry = (null);
        table.lastEntry = (null);
        table.thisEntry = table.firstEntry;
        table.lastDate = 0.0;
        table.x1 = 0.0;
        table.x2 = 0.0;
        table.y1 = 0.0;
        table.y2 = 0.0;
        table.dxMin = 0.0;
        table.file.mode = S.NO_FILE;
        table.file.file = (null);
        table.curveType = -1;
    }

    // table.c:285
    public static int table_validate(TTable table)
    {
        int result = 0;
        double x1 = 0.0;
        double x2 = 0.0;
        double y1 = 0.0;
        double y2 = 0.0;
        double dx = 0.0;
        double dxMin = 10000000000.0;
        if (table.file.mode == S.USE_FILE)
        {
            table.file.file = R.fopen(table.file.name, S.L13);
            if (table.file.file == null)
            {
                return S.ERR_TABLE_FILE_OPEN;
            }
        }
        result = S.table_getFirstEntry(table, ref x1, ref y1);
        if ((!(result != 0)) && (table.file.mode == S.USE_FILE))
        {
            return S.ERR_TABLE_FILE_READ;
        }
        while ((S.table_getNextEntry(table, ref x2, ref y2) != 0))
        {
            dx = x2 - x1;
            if (dx <= 0.0)
            {
                table.x2 = x2;
                return S.ERR_CURVE_SEQUENCE;
            }
            dxMin = ((((dxMin) <= (dx)) ? (dxMin) : (dx)));
            x1 = x2;
        }
        table.dxMin = dxMin;
        if ((table.file.mode == S.USE_FILE) && (!((R.feof(table.file.file) != 0))))
        {
            return S.ERR_TABLE_FILE_READ;
        }
        return 0;
    }

    // table.c:331
    public static int table_getFirstEntry(TTable table, ref double x, ref double y)
    {
        TTableEntry entry = null;
        x = 0;
        y = 0.0;
        if (table.file.mode == S.USE_FILE)
        {
            if (table.file.file == null)
            {
                return 0;
            }
            R.rewind(table.file.file);
            return S.table_getNextFileEntry(table, ref x, ref y);
        }
        entry = table.firstEntry;
        if ((entry != null))
        {
            x = entry.x;
            y = entry.y;
            table.thisEntry = entry;
            return 1;
        }
        else
        {
            return 0;
        }
    }

    // table.c:366
    public static int table_getNextEntry(TTable table, ref double x, ref double y)
    {
        TTableEntry entry = null;
        if (table.file.mode == S.USE_FILE)
        {
            return S.table_getNextFileEntry(table, ref x, ref y);
        }
        entry = table.thisEntry.next;
        if ((entry != null))
        {
            x = entry.x;
            y = entry.y;
            table.thisEntry = entry;
            return 1;
        }
        else
        {
            return 0;
        }
    }

    // table.c:395
    public static double table_lookup(TTable table, double x)
    {
        double x1 = 0.0;
        double y1 = 0.0;
        double x2 = 0.0;
        double y2 = 0.0;
        TTableEntry entry = null;
        entry = table.firstEntry;
        if (entry == null)
        {
            return 0.0;
        }
        x1 = entry.x;
        y1 = entry.y;
        if (x <= x1)
        {
            return y1;
        }
        while ((entry.next != null))
        {
            entry = entry.next;
            x2 = entry.x;
            y2 = entry.y;
            if (x <= x2)
            {
                return S.table_interpolate(x, x1, y1, x2, y2);
            }
            x1 = x2;
            y1 = y2;
        }
        return y1;
    }

    // table.c:430
    public static double table_getSlope(TTable table, double x)
    {
        double x1 = 0.0;
        double y1 = 0.0;
        double x2 = 0.0;
        double y2 = 0.0;
        double dx = 0.0;
        TTableEntry entry = null;
        entry = table.firstEntry;
        if (entry == null)
        {
            return 0.0;
        }
        x1 = entry.x;
        y1 = entry.y;
        x2 = x1;
        y2 = y1;
        while ((entry.next != null))
        {
            entry = entry.next;
            x2 = entry.x;
            y2 = entry.y;
            if (x <= x2)
            {
                break;
            }
            x1 = x2;
            y1 = y2;
        }
        dx = x2 - x1;
        if (dx == 0.0)
        {
            return 0.0;
        }
        return (y2 - y1) / dx;
    }

    // table.c:464
    public static double table_lookupEx(TTable table, double x)
    {
        double x1 = 0.0;
        double y1 = 0.0;
        double x2 = 0.0;
        double y2 = 0.0;
        double s = 0.0;
        TTableEntry entry = null;
        entry = table.firstEntry;
        if (entry == null)
        {
            return 0.0;
        }
        x1 = entry.x;
        y1 = entry.y;
        if (x <= x1)
        {
            if (x1 > 0.0)
            {
                return x / x1 * y1;
            }
            else
            {
                return y1;
            }
        }
        while ((entry.next != null))
        {
            entry = entry.next;
            x2 = entry.x;
            y2 = entry.y;
            if (x2 != x1)
            {
                s = (y2 - y1) / (x2 - x1);
            }
            if (x <= x2)
            {
                return S.table_interpolate(x, x1, y1, x2, y2);
            }
            x1 = x2;
            y1 = y2;
        }
        if (s < 0.0)
        {
            s = 0.0;
        }
        return y1 + s * (x - x1);
    }

    // table.c:503
    public static double table_intervalLookup(TTable table, double x)
    {
        TTableEntry entry = null;
        entry = table.firstEntry;
        if (entry == null)
        {
            return 0.0;
        }
        if (x < entry.x)
        {
            return entry.y;
        }
        while ((entry.next != null))
        {
            entry = entry.next;
            if (x < entry.x)
            {
                return entry.y;
            }
        }
        return entry.y;
    }

    // table.c:527
    public static double table_inverseLookup(TTable table, double y)
    {
        double x1 = 0.0;
        double y1 = 0.0;
        double x2 = 0.0;
        double y2 = 0.0;
        TTableEntry entry = null;
        entry = table.firstEntry;
        if (entry == null)
        {
            return 0.0;
        }
        x1 = entry.x;
        y1 = entry.y;
        if (y <= y1)
        {
            return x1;
        }
        while ((entry.next != null))
        {
            entry = entry.next;
            x2 = entry.x;
            y2 = entry.y;
            if (y <= y2)
            {
                return S.table_interpolate(y, y1, x1, y2, x2);
            }
            x1 = x2;
            y1 = y2;
        }
        return x1;
    }

    // table.c:562
    public static double table_getMaxY(TTable table, double x)
    {
        double ymax = 0.0;
        TTableEntry entry = null;
        entry = table.firstEntry;
        ymax = entry.y;
        while ((x > entry.x) && (entry.next != null))
        {
            entry = entry.next;
            if (entry.y < ymax)
            {
                return ymax;
            }
            ymax = entry.y;
        }
        return 0.0;
    }

    // table.c:587
    public static double table_getStorageVolume(TTable table, double x)
    {
        double a = 0.0;
        double a1 = 0.0;
        double x1 = 0.0;
        double v = 0.0;
        double dx = 0.0;
        double dy = 0.0;
        double s = 0.0;
        TTableEntry entry = null;
        v = 0.0;
        entry = table.firstEntry;
        if (entry == null)
        {
            return 0.0;
        }
        x1 = entry.x;
        a1 = entry.y;
        if (x <= x1)
        {
            if (x1 < 1.0e-6)
            {
                return 0.0;
            }
            return (a1 / x1) * x * x / 2.0;
        }
        while ((entry.next != null))
        {
            entry = entry.next;
            if (entry.x >= x)
            {
                a = S.table_interpolate(x, x1, a1, entry.x, entry.y);
                return v + (a1 + a) / 2.0 * (x - x1);
            }
            else
            {
                dx = entry.x - x1;
                dy = entry.y - a1;
                v = v + (a1 + entry.y) / 2.0 * dx;
                x1 = entry.x;
                a1 = entry.y;
            }
        }
        if (dx > 1.0e-6)
        {
            s = dy / dx;
            a = a1 + s * (x - x1);
            if (a < 0.0)
            {
                v = v - a1 * a1 / s / 2.0;
            }
            else
            {
                v = v + (a1 + a) / 2.0 * (x - x1);
            }
        }
        return v;
    }

    // table.c:651
    public static double table_getStorageDepth(TTable table, double v)
    {
        double a1 = 0.0;
        double a2 = 0.0;
        double d1 = 0.0;
        double d2 = 0.0;
        double dd = 0.0;
        double da = 0.0;
        double v1 = 0.0;
        double v2 = 0.0;
        double s = 0.0;
        TTableEntry entry = null;
        if (v == 0.0)
        {
            return 0.0;
        }
        entry = table.firstEntry;
        if (entry == null)
        {
            return 0.0;
        }
        d1 = entry.x;
        a1 = entry.y;
        v1 = a1 * d1 / 2.0;
        if (v <= v1)
        {
            if (a1 > 0.0)
            {
                return Math.Sqrt(2.0 * v * d1 / a1);
            }
            else
            {
                return 0.0;
            }
        }
        while ((entry.next != null))
        {
            entry = entry.next;
            d2 = entry.x;
            a2 = entry.y;
            dd = d2 - d1;
            da = a2 - a1;
            v2 = v1 + (a1 + a2) / 2.0 * dd;
            if (v <= v2)
            {
                if (dd <= 0.0)
                {
                    return d1;
                }
                if (da == 0.0)
                {
                    if (Math.Abs(v2 - v1) < 1.0e-6)
                    {
                        return d1;
                    }
                    else
                    {
                        return d1 + dd * (v - v1) / (v2 - v1);
                    }
                }
                if (da < 0.0)
                {
                    d1 = d2;
                    a1 = a2;
                    v1 = v2;
                }
                s = da / dd;
                return d1 + (Math.Sqrt(a1 * a1 + 2.0 * s * (v - v1)) - a1) / s;
            }
            d1 = d2;
            a1 = a2;
            v1 = v2;
        }
        if ((dd == 0.0) || (da == 0.0))
        {
            if (a1 > 0.0)
            {
                dd = (v - v1) / a1;
            }
            else
            {
                dd = 0.0;
            }
        }
        else
        {
            s = da / dd;
            dd = (Math.Sqrt(a1 * a1 + 2.0 * s * (v - v1)) - a1) / s;
            if (dd < 0.0)
            {
                dd = 0.0;
            }
        }
        return d1 + dd;
    }

    // table.c:730
    public static void table_tseriesInit(TTable table)
    {
        S.table_getFirstEntry(table, ref table.x1, ref table.y1);
        table.x2 = table.x1;
        table.y2 = table.y1;
        S.table_getNextEntry(table, ref table.x2, ref table.y2);
    }

    // table.c:745
    public static double table_tseriesLookup(TTable table, double x, int extend)
    {
        if (((table.x1 <= x) && (table.x2 >= x)) && (table.x1 != table.x2))
        {
            return S.table_interpolate(x, table.x1, table.y1, table.x2, table.y2);
        }
        if ((table.file.mode == S.USE_FILE) && ((R.feof(table.file.file) != 0)))
        {
            if (extend == 1)
            {
                return table.y1;
            }
            else
            {
                return 0;
            }
        }
        if ((table.x1 == table.x2) || (x < table.x1))
        {
            S.table_getFirstEntry(table, ref table.x1, ref table.y1);
            if (x < table.x1)
            {
                if (extend == 1)
                {
                    return table.y1;
                }
                else
                {
                    return 0;
                }
            }
        }
        table.x1 = table.x2;
        table.y1 = table.y2;
        while ((S.table_getNextEntry(table, ref table.x2, ref table.y2) != 0))
        {
            if (x <= table.x2)
            {
                return S.table_interpolate(x, table.x1, table.y1, table.x2, table.y2);
            }
            table.x1 = table.x2;
            table.y1 = table.y2;
        }
        if (extend == 1)
        {
            return table.y1;
        }
        else
        {
            return 0.0;
        }
    }

    // table.c:808
    public static int table_getNextFileEntry(TTable table, ref double x, ref double y)
    {
        CPtr line = R.Buf(1025);
        int code = 0;
        if (table.file.file == null)
        {
            return 0;
        }
        while ((!((R.feof(table.file.file) != 0))) && (!(R.fgets(line, 1024, table.file.file)).IsNull))
        {
            code = S.table_parseFileLine(line, table, ref x, ref y);
            if (code < 0)
            {
                continue;
            }
            return code;
        }
        return 0;
    }

    // table.c:833
    public static int table_parseFileLine(CPtr line, TTable table, ref double x, ref double y)
    {
        int n = 0;
        CPtr s1 = R.Buf(50);
        CPtr s2 = R.Buf(50);
        CPtr s3 = R.Buf(50);
        CPtr tStr = CPtr.Null;
        CPtr yStr = CPtr.Null;
        double yy = 0.0;
        double d = 0.0;
        double t = 0.0;
        n = R.sscanf(line, S.L650, s1, s2, s3);
        tStr = R.strtok(line, S.L184);
        if (tStr.IsNull || (tStr[0] == ';'))
        {
            return -1;
        }
        if (n == 2)
        {
            d = table.lastDate;
            tStr = s1;
            yStr = s2;
        }
        else if (n == 3)
        {
            if (!((S.datetime_strToDate(s1, ref d) != 0)))
            {
                return 0;
            }
            table.lastDate = d;
            tStr = s2;
            yStr = s3;
        }
        else
        {
            return 0;
        }
        if ((S.getDouble(tStr, ref t) != 0))
        {
            t /= 24.0;
        }
        else if (!((S.datetime_strToTime(tStr, ref t) != 0)))
        {
            return 0;
        }
        if (!((S.getDouble(yStr, ref yy) != 0)))
        {
            return 0;
        }
        x = d + t;
        y = yy;
        return 1;
    }

}
}
