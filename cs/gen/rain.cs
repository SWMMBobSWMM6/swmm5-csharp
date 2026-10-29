// Generated from EPA SWMM 5.2.4 (rain.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static TRainStats RainStats = new TRainStats();
    public static int Condition = 0;
    public static int TimeOffset = 0;
    public static int DataOffset = 0;
    public static int ValueOffset = 0;
    public static int RainType = 0;
    public static int Interval = 0;
    public static double UnitsFactor = 0.0;
    public static float RainAccum = 0.0f;
    public static CPtr StationID = CPtr.Null;
    public static double AccumStartDate = 0.0;
    public static double PreviousDate = 0.0;
    public static int GageIndex = 0;
    public static int hasStationName = 0;

    // rain.c:118
    public static void rain_open()
    {
        int i = 0;
        int count = 0;
        count = 0;
        for (i = 0; i < S.Nobjects[S.GAGE]; i++)
        {
            if (S.Gage[i].dataSource == S.RAIN_FILE)
            {
                count++;
            }
        }
        S.Frain.file = (null);
        if (count == 0)
        {
            S.Frain.mode = S.NO_FILE;
        }
        else
        {
            switch (S.Frain.mode)
            {
                case S.SCRATCH_FILE:
                    S.getTempFileName(S.Frain.name);
                    if ((S.Frain.file = R.fopen(S.Frain.name, S.L166)) == null)
                    {
                        S.report_writeErrorMsg(S.ERR_RAIN_FILE_SCRATCH, S.L0);
                        return;
                    }
                    break;
                case S.USE_FILE:
                    if ((S.Frain.file = R.fopen(S.Frain.name, S.L165)) == null)
                    {
                        S.report_writeErrorMsg(S.ERR_RAIN_FILE_OPEN, S.Frain.name);
                        return;
                    }
                    break;
                case S.SAVE_FILE:
                    if ((S.Frain.file = R.fopen(S.Frain.name, S.L166)) == null)
                    {
                        S.report_writeErrorMsg(S.ERR_RAIN_FILE_OPEN, S.Frain.name);
                        return;
                    }
                    break;
            }
        }
        if ((S.Frain.mode == S.SCRATCH_FILE) || (S.Frain.mode == S.SAVE_FILE))
        {
            S.createRainFile(count);
        }
        if (S.Frain.mode != S.NO_FILE)
        {
            S.initRainFile();
        }
        S.rdii_openRdii();
    }

    // rain.c:184
    public static void rain_close()
    {
        if ((S.Frain.file != null))
        {
            R.fclose(S.Frain.file);
            if (S.Frain.mode == S.SCRATCH_FILE)
            {
                R.remove(S.Frain.name);
            }
        }
        S.Frain.file = (null);
        S.rdii_closeRdii();
    }

    // rain.c:202
    public static void createRainFile(int count)
    {
        int i = 0;
        int k = 0;
        int kount = count;
        int filePos1 = 0;
        int filePos2 = 0;
        int filePos3 = 0;
        int interval = 0;
        int dummy = -1;
        CPtr staID = R.Buf(1025);
        CPtr fileStamp = R.BufFrom(S.L327, 11);
        if ((S.ErrorCode != 0) || (!(S.Frain.file != null)))
        {
            return;
        }
        R.fwriteC(fileStamp, 0, (int)(R.strlen(fileStamp)), S.Frain.file);
        R.fwriteI1(kount, S.Frain.file);
        filePos1 = (int)(R.ftell(S.Frain.file));
        if (count > 0)
        {
            S.report_writeRainStats(-1, S.RainStats);
        }
        R.strcpy(staID, S.L33);
        for (i = 0; i < count; i++)
        {
            R.fwriteC(staID, 0, (int)(unchecked((ulong)(1024 + 1))), S.Frain.file);
            for (k = 1; k <= 3; k++)
            {
                R.fwriteI1(dummy, S.Frain.file);
            }
        }
        filePos2 = (int)(R.ftell(S.Frain.file));
        for (i = 0; i < S.Nobjects[S.GAGE]; i++)
        {
            if ((S.ErrorCode != 0) || (S.Gage[i].dataSource != S.RAIN_FILE))
            {
                continue;
            }
            if ((S.rainFileConflict(i) != 0))
            {
                break;
            }
            R.fseek(S.Frain.file, filePos2, 0);
            if ((S.addGageToRainFile(i) != 0))
            {
                filePos3 = (int)(R.ftell(S.Frain.file));
                R.fseek(S.Frain.file, filePos1, 0);
                S.sstrncpy(staID, S.Gage[i].staID, unchecked((ulong)(1024)));
                interval = S.Interval;
                R.fwriteC(staID, 0, (int)(unchecked((ulong)(1024 + 1))), S.Frain.file);
                R.fwriteI1(interval, S.Frain.file);
                R.fwriteI1(filePos2, S.Frain.file);
                R.fwriteI1(filePos3, S.Frain.file);
                filePos1 = (int)(R.ftell(S.Frain.file));
                filePos2 = filePos3;
                S.report_writeRainStats(i, S.RainStats);
            }
        }
        if ((S.ErrorCode != 0))
        {
            R.fclose(S.Frain.file);
            S.Frain.file = (null);
            R.remove(S.Frain.name);
        }
    }

    // rain.c:278
    public static int rainFileConflict(int i)
    {
        int j = 0;
        CPtr staID = S.Gage[i].staID;
        CPtr fname = S.Gage[i].fname;
        for (j = 0; j < i; j++)
        {
            if (((S.strcomp(S.Gage[j].staID, staID) != 0)) && (!((S.strcomp(S.Gage[j].fname, fname) != 0))))
            {
                S.report_writeErrorMsg(S.ERR_RAIN_FILE_CONFLICT, S.Gage[i].ID);
                return 1;
            }
        }
        return 0;
    }

    // rain.c:302
    public static int addGageToRainFile(int i)
    {
        CFile f = null;
        int fileFormat = 0;
        int hdrLines = 0;
        S.StationID = CPtr.Null;
        if ((f = R.fopen(S.Gage[i].fname, S.L13)) == null)
        {
            S.report_writeErrorMsg(S.ERR_RAIN_FILE_DATA, S.Gage[i].fname);
        }
        else
        {
            fileFormat = S.findFileFormat(f, i, ref hdrLines);
            if (fileFormat == S.UNKNOWN_FORMAT)
            {
                S.report_writeErrorMsg(S.ERR_RAIN_FILE_FORMAT, S.Gage[i].fname);
            }
            else
            {
                S.GageIndex = i;
                S.readFile(f, fileFormat, hdrLines, S.Gage[i].startFileDate, S.Gage[i].endFileDate);
            }
            R.fclose(f);
        }
        if ((S.ErrorCode != 0))
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    // rain.c:341
    public static void initRainFile()
    {
        CPtr fileStamp = R.BufFrom(S.L327, 11);
        CPtr fStamp = R.BufFrom(S.L327, 11);
        int i = 0;
        int kount = 0;
        long filePos = 0L;
        if ((S.ErrorCode != 0) || (!(S.Frain.file != null)))
        {
            return;
        }
        R.rewind(S.Frain.file);
        R.freadC(fStamp, 0, (int)(R.strlen(fileStamp)), S.Frain.file);
        if (R.strcmp(fStamp, fileStamp) != 0)
        {
            S.report_writeErrorMsg(S.ERR_RAIN_IFACE_FORMAT, S.L0);
            return;
        }
        R.freadI1(ref kount, S.Frain.file);
        filePos = R.ftell(S.Frain.file);
        for (i = 0; i < S.Nobjects[S.GAGE]; i++)
        {
            if ((S.ErrorCode != 0) || (S.Gage[i].dataSource != S.RAIN_FILE))
            {
                continue;
            }
            R.fseek(S.Frain.file, filePos, 0);
            if ((!((S.findGageInFile(i, kount) != 0))) || (S.Gage[i].startFilePos == S.Gage[i].endFilePos))
            {
                S.report_writeErrorMsg(S.ERR_RAIN_FILE_GAGE, S.Gage[i].ID);
            }
        }
    }

    // rain.c:385
    public static int findGageInFile(int i, int kount)
    {
        int k = 0;
        int interval = 0;
        int filePos1 = 0;
        int filePos2 = 0;
        CPtr staID = R.BufFrom(S.L0, 1025);
        for (k = 1; k <= kount; k++)
        {
            R.freadC(staID, 0, (int)(unchecked((ulong)(1024 + 1))), S.Frain.file);
            R.freadI1(ref interval, S.Frain.file);
            R.freadI1(ref filePos1, S.Frain.file);
            R.freadI1(ref filePos2, S.Frain.file);
            if (R.strcmp(staID, S.Gage[i].staID) == 0)
            {
                S.Gage[i].rainType = S.RAINFALL_VOLUME;
                S.Gage[i].rainInterval = interval;
                S.Gage[i].startFilePos = filePos1;
                S.Gage[i].endFilePos = filePos2;
                S.Gage[i].currentFilePos = S.Gage[i].startFilePos;
                return 1;
            }
        }
        return 0;
    }

    // rain.c:420
    public static int findFileFormat(CFile f, int i, ref int hdrLines)
    {
        int fileFormat = 0;
        int lineCount = 0;
        int maxCount = 5;
        int n = 0;
        int div = 0;
        long sn2 = 0L;
        CPtr recdType = R.Buf(4);
        CPtr elemType = R.Buf(5);
        CPtr coopID = R.BufFrom(S.L0, 6);
        CPtr line = R.Buf(1024);
        int year = 0;
        int month = 0;
        int day = 0;
        int hour = 0;
        int minute = 0;
        int elem = 0;
        float x = 0.0f;
        fileFormat = S.UNKNOWN_FORMAT;
        S.hasStationName = 0;
        S.UnitsFactor = 1.0;
        S.Interval = 0;
        hdrLines = 0;
        for (lineCount = 1; lineCount <= maxCount; lineCount++)
        {
            if ((R.fgets(line, 1024, f)).IsNull)
            {
                return fileFormat;
            }
            n = R.sscanf(line, S.L328, R.RefL(__v => sn2 = __v), R.RefI(__v => div = __v), elemType);
            if (n == 3)
            {
                S.Interval = S.getNWSInterval(elemType);
                S.TimeOffset = S.Interval;
                if (S.Interval > 0)
                {
                    fileFormat = S.NWS_SPACE_DELIMITED;
                    break;
                }
            }
            n = R.sscanf((line + 37), S.L329, R.RefI(__v => div = __v), elemType, recdType, R.RefI(__v => year = __v));
            if (n == 4)
            {
                S.Interval = S.getNWSInterval(elemType);
                S.TimeOffset = S.Interval;
                if (S.Interval > 0)
                {
                    fileFormat = S.NWS_SPACE_DELIMITED;
                    S.hasStationName = 1;
                    break;
                }
            }
            n = R.sscanf(line, S.L330, R.RefL(__v => sn2 = __v), R.RefI(__v => div = __v), elemType);
            if (n == 3)
            {
                S.Interval = S.getNWSInterval(elemType);
                S.TimeOffset = S.Interval;
                if (S.Interval > 0)
                {
                    fileFormat = S.NWS_COMMA_DELIMITED;
                    break;
                }
            }
            n = R.sscanf((line + 37), S.L331, R.RefI(__v => div = __v), elemType, recdType, R.RefI(__v => year = __v));
            if (n == 4)
            {
                S.Interval = S.getNWSInterval(elemType);
                S.TimeOffset = S.Interval;
                if (S.Interval > 0)
                {
                    fileFormat = S.NWS_COMMA_DELIMITED;
                    S.hasStationName = 1;
                    break;
                }
            }
            n = R.sscanf(line, S.L332, recdType, R.RefL(__v => sn2 = __v), R.RefI(__v => div = __v), elemType);
            if (n == 4)
            {
                S.Interval = S.getNWSInterval(elemType);
                S.TimeOffset = S.Interval;
                if (S.Interval > 0)
                {
                    fileFormat = S.NWS_TAPE;
                    break;
                }
            }
            n = R.sscanf(line, S.L333, coopID, R.RefL(__v => sn2 = __v));
            if ((n == 2) && (R.strcmp(coopID, S.L334) == 0))
            {
                fileFormat = S.findNWSOnlineFormat(f, line);
                break;
            }
            n = R.sscanf(line, S.L335, R.RefL(__v => sn2 = __v), R.RefI(__v => year = __v), R.RefI(__v => month = __v), R.RefI(__v => day = __v), R.RefI(__v => elem = __v));
            if (n == 5)
            {
                if ((elem == 123) && (R.strlen(line) >= unchecked((ulong)(185))))
                {
                    fileFormat = S.AES_HLY;
                    S.Interval = 3600;
                    S.TimeOffset = S.Interval;
                    S.UnitsFactor = 1.0 / 25.4;
                    break;
                }
            }
            n = R.sscanf(line, S.L336, R.RefL(__v => sn2 = __v), R.RefI(__v => year = __v), R.RefI(__v => month = __v), R.RefI(__v => day = __v), R.RefI(__v => elem = __v));
            if (n == 5)
            {
                if ((elem == 159) && (R.strlen(line) >= unchecked((ulong)(691))))
                {
                    fileFormat = S.CMC_FIF;
                    S.Interval = 900;
                }
                else if ((elem == 123) && (R.strlen(line) >= unchecked((ulong)(186))))
                {
                    fileFormat = S.CMC_HLY;
                    S.Interval = 3600;
                }
                if ((fileFormat == S.CMC_FIF) || (fileFormat == S.CMC_HLY))
                {
                    S.TimeOffset = S.Interval;
                    S.UnitsFactor = 1.0 / 25.4;
                    break;
                }
            }
            if ((S.parseStdLine(line, ref year, ref month, ref day, ref hour, ref minute, ref x) != 0))
            {
                fileFormat = S.STD_SPACE_DELIMITED;
                S.RainType = S.Gage[i].rainType;
                S.Interval = S.Gage[i].rainInterval;
                if (S.Gage[i].rainUnits == S.SI)
                {
                    S.UnitsFactor = 1.0 / 25.4;
                }
                S.TimeOffset = 0;
                S.StationID = S.Gage[i].staID;
                break;
            }
            (hdrLines)++;
        }
        if (fileFormat != S.UNKNOWN_FORMAT)
        {
            S.Gage[i].rainInterval = S.Interval;
        }
        return fileFormat;
    }

    // rain.c:584
    public static int findNWSOnlineFormat(CFile f, CPtr line)
    {
        int n = 0;
        int fileFormat = S.UNKNOWN_FORMAT;
        CPtr str = CPtr.Null;
        R.rewind(f);
        R.fgets(line, 1024, f);
        if (!((str = R.strstr(line, S.L337))).IsNull)
        {
            S.Interval = 3600;
            S.TimeOffset = S.Interval;
            S.ValueOffset = (int)((CPtr.Diff(str, line)));
            fileFormat = S.NWS_ONLINE_60;
        }
        else if (!((str = R.strstr(line, S.L338))).IsNull)
        {
            S.Interval = 900;
            S.TimeOffset = S.Interval;
            S.ValueOffset = (int)((CPtr.Diff(str, line)));
            fileFormat = S.NWS_ONLINE_15;
        }
        else
        {
            return S.UNKNOWN_FORMAT;
        }
        for (n = 1; n <= 5; n++)
        {
            if ((R.fgets(line, 1024, f)).IsNull)
            {
                return S.UNKNOWN_FORMAT;
            }
            if ((R.strstr(line, S.L334)).IsNull)
            {
                continue;
            }
            str = R.strrchr(line, ':');
            if (str.IsNull)
            {
                return S.UNKNOWN_FORMAT;
            }
            n = (int)((CPtr.Diff(str, line)));
            S.DataOffset = n - 11;
            return fileFormat;
        }
        return S.UNKNOWN_FORMAT;
    }

    // rain.c:641
    public static int getNWSInterval(CPtr elemType)
    {
        if (R.strcmp(elemType, S.L337) == 0)
        {
            return 3600;
        }
        else if (R.strcmp(elemType, S.L338) == 0)
        {
            return 900;
        }
        else if (R.strcmp(elemType, S.L339) == 0)
        {
            return 900;
        }
        else
        {
            return 0;
        }
    }

    // rain.c:656
    public static void readFile(CFile f, int fileFormat, int hdrLines, double day1, double day2)
    {
        CPtr line = R.Buf(1024);
        int i = 0;
        int n = 0;
        R.rewind(f);
        S.RainStats.startDate = -693594;
        S.RainStats.endDate = -693594;
        S.RainStats.periodsRain = 0;
        S.RainStats.periodsMissing = 0;
        S.RainStats.periodsMalfunc = 0;
        S.RainAccum = (float)(0.0);
        S.AccumStartDate = -693594;
        S.PreviousDate = -693594;
        for (i = 1; i <= hdrLines; i++)
        {
            if ((R.fgets(line, 1024, f)).IsNull)
            {
                return;
            }
        }
        while (!(R.fgets(line, 1024, f)).IsNull)
        {
            switch (fileFormat)
            {
                case S.STD_SPACE_DELIMITED:
                    n = S.readStdLine(line, day1, day2);
                    break;
                case S.NWS_TAPE:
                case S.NWS_SPACE_DELIMITED:
                case S.NWS_COMMA_DELIMITED:
                case S.NWS_ONLINE_60:
                case S.NWS_ONLINE_15:
                    n = S.readNWSLine(line, fileFormat, day1, day2);
                    break;
                case S.AES_HLY:
                case S.CMC_FIF:
                case S.CMC_HLY:
                    n = S.readCMCLine(line, fileFormat, day1, day2);
                    break;
                default:
                    n = -1;
                    break;
            }
            if (n < 0)
            {
                break;
            }
        }
    }

    // rain.c:717
    public static int readNWSLine(CPtr line, int fileFormat, double day1, double day2)
    {
        int flag1 = 0;
        int flag2 = 0;
        int isMissing = 0;
        double date1 = 0.0;
        long result = 1;
        int k = 0;
        int y = 0;
        int m = 0;
        int d = 0;
        int n = 0;
        int hour = 0;
        int minute = 0;
        long v = 0L;
        float x = 0.0f;
        int lineLength = (int)(R.strlen(line)) - 1;
        int nameLength = 0;
        switch (fileFormat)
        {
            case S.NWS_TAPE:
                if (lineLength <= 30)
                {
                    return 0;
                }
                if (R.sscanf((line + 17), S.L340, R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v), R.RefI(__v => n = __v)) < 4)
                {
                    return 0;
                }
                k = 30;
                break;
            case S.NWS_SPACE_DELIMITED:
                if ((S.hasStationName != 0))
                {
                    nameLength = 31;
                }
                if (lineLength <= 28 + nameLength)
                {
                    return 0;
                }
                k = 18 + nameLength;
                if (R.sscanf((line + k), S.L341, R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v)) < 3)
                {
                    return 0;
                }
                k = k + 10;
                break;
            case S.NWS_COMMA_DELIMITED:
                if (lineLength <= 28)
                {
                    return 0;
                }
                if (R.sscanf((line + 18), S.L342, R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v)) < 3)
                {
                    return 0;
                }
                k = 28;
                break;
            case S.NWS_ONLINE_60:
            case S.NWS_ONLINE_15:
                if (lineLength <= S.DataOffset + 23)
                {
                    return 0;
                }
                if (R.sscanf((line + S.DataOffset), S.L27, R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v)) < 3)
                {
                    return 0;
                }
                k = S.DataOffset + 8;
                break;
            default:
                return 0;
        }
        date1 = S.datetime_encodeDate(y, m, d);
        if ((day1 != -693594) && (date1 < day1))
        {
            return 0;
        }
        if ((day2 != -693594) && (date1 > day2))
        {
            return -1;
        }
        while (k < lineLength)
        {
            flag1 = 0;
            flag2 = 0;
            v = 99999;
            hour = 25;
            minute = 0;
            switch (fileFormat)
            {
                case S.NWS_TAPE:
                    n = R.sscanf((line + k), S.L343, R.RefI(__v => hour = __v), R.RefI(__v => minute = __v), R.RefL(__v => v = __v), R.RefI(__v => flag1 = __v), R.RefI(__v => flag2 = __v));
                    k += 12;
                    break;
                case S.NWS_SPACE_DELIMITED:
                    n = R.sscanf((line + k), S.L344, R.RefI(__v => hour = __v), R.RefI(__v => minute = __v), R.RefL(__v => v = __v), R.RefI(__v => flag1 = __v), R.RefI(__v => flag2 = __v));
                    k += 16;
                    break;
                case S.NWS_COMMA_DELIMITED:
                    n = R.sscanf((line + k), S.L345, R.RefI(__v => hour = __v), R.RefI(__v => minute = __v), R.RefL(__v => v = __v), R.RefI(__v => flag1 = __v), R.RefI(__v => flag2 = __v));
                    k += 16;
                    break;
                case S.NWS_ONLINE_60:
                case S.NWS_ONLINE_15:
                    n = R.sscanf((line + k), S.L346, R.RefI(__v => hour = __v), R.RefI(__v => minute = __v));
                    n += R.Chr1(ref flag1, __c => S.readNwsOnlineValue((line + S.ValueOffset), ref v, __c));
                    if (hour == 0)
                    {
                        hour = 24;
                        date1 -= 1.0;
                    }
                    k += lineLength;
                    break;
                default:
                    n = 0;
                    break;
            }
            if ((n < 3) || (hour >= 25))
            {
                break;
            }
            S.setCondition(flag1);
            if (((S.Condition == S.DELETED_PERIOD) || (S.Condition == S.MISSING_PERIOD)) || (flag1 == 'M'))
            {
                isMissing = 1;
            }
            else if (v >= 9999)
            {
                isMissing = 1;
            }
            else
            {
                isMissing = 0;
            }
            if (flag1 == 'a')
            {
                S.AccumStartDate = date1 + S.datetime_encodeTime(hour, minute, 0);
            }
            else if (flag1 == 'A')
            {
                S.saveAccumRainfall(date1, hour, minute, v);
            }
            else
            {
                x = (float)(v) / 100.0f;
                if ((x > (float)(0)) || (isMissing != 0))
                {
                    S.saveRainfall(date1, hour, minute, x, isMissing);
                }
            }
            if (((flag1 == 'A') || (flag1 == '}')) || (flag1 == ']'))
            {
                S.Condition = 0;
            }
        }
        return (int)(result);
    }

    // rain.c:862
    public static int readNwsOnlineValue(CPtr s, ref long v, CPtr flag)
    {
        int n = 0;
        float x = 99.99f;
        if (!(R.strchr(s, '.')).IsNull)
        {
            n = R.sscanf(s, S.L347, R.RefF(__v => x = __v), flag);
            v = R.D2L((100.0f * x + 0.5f));
        }
        else
        {
            n = R.Back(R.sscanf(s, S.L348, R.Slot, flag), ref v);
        }
        return n;
    }

    // rain.c:891
    public static void setCondition(int flag)
    {
        switch (flag)
        {
            case 'a':
            case 'A':
                S.Condition = S.ACCUMULATED_PERIOD;
                break;
            case '{':
            case '}':
                S.Condition = S.DELETED_PERIOD;
                break;
            case '[':
            case ']':
                S.Condition = S.MISSING_PERIOD;
                break;
            default:
                S.Condition = S.NO_CONDITION;
                break;
        }
    }

    // rain.c:914
    public static int readCMCLine(CPtr line, int fileFormat, double day1, double day2)
    {
        int flag = 0;
        int isMissing = 0;
        double date1 = 0.0;
        long sn = 0L;
        long v = 0L;
        int col = 0;
        int j = 0;
        int jMax = 0;
        int elem = 0;
        int y = 0;
        int m = 0;
        int d = 0;
        int hour = 0;
        int minute = 0;
        float x = 0.0f;
        if (fileFormat == S.AES_HLY)
        {
            if (R.sscanf(line, S.L335, R.RefL(__v => sn = __v), R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v), R.RefI(__v => elem = __v)) < 5)
            {
                return 0;
            }
            if (y < 100)
            {
                y = y + 2000;
            }
            else
            {
                y = y + 1000;
            }
            col = 17;
        }
        else
        {
            if (R.sscanf(line, S.L336, R.RefL(__v => sn = __v), R.RefI(__v => y = __v), R.RefI(__v => m = __v), R.RefI(__v => d = __v), R.RefI(__v => elem = __v)) < 5)
            {
                return 0;
            }
            col = 18;
        }
        date1 = S.datetime_encodeDate(y, m, d);
        if ((day1 != -693594) && (date1 < day1))
        {
            return 0;
        }
        if ((day2 != -693594) && (date1 > day2))
        {
            return -1;
        }
        if ((fileFormat == S.AES_HLY) && (elem != 123))
        {
            return 0;
        }
        else if ((fileFormat == S.CMC_FIF) && (elem != 159))
        {
            return 0;
        }
        else if ((fileFormat == S.CMC_HLY) && (elem != 123))
        {
            return 0;
        }
        hour = 0;
        minute = 0;
        jMax = 24;
        if (fileFormat == S.CMC_FIF)
        {
            jMax = 96;
        }
        for (j = 1; j <= jMax; j++)
        {
            if (R.sscanf((line + col), S.L349, R.RefL(__v => v = __v), R.RefI(__v => flag = __v)) < 2)
            {
                return 0;
            }
            col += 7;
            if (v == -99999)
            {
                isMissing = 1;
            }
            else
            {
                isMissing = 0;
            }
            x = (float)(((double)(v) / 10.0 / 25.4));
            if ((x > (float)(0)) || (isMissing != 0))
            {
                S.saveRainfall(date1, hour, minute, x, isMissing);
            }
            if (fileFormat == S.CMC_FIF)
            {
                minute += 15;
                if (minute == 60)
                {
                    minute = 0;
                    hour++;
                }
            }
            else
            {
                hour++;
            }
        }
        return 1;
    }

    // rain.c:994
    public static int readStdLine(CPtr line, double day1, double day2)
    {
        double date1 = 0.0;
        double date2 = 0.0;
        int year = 0;
        int month = 0;
        int day = 0;
        int hour = 0;
        int minute = 0;
        float x = 0.0f;
        if (!((S.parseStdLine(line, ref year, ref month, ref day, ref hour, ref minute, ref x) != 0)))
        {
            return 0;
        }
        date1 = S.datetime_encodeDate(year, month, day);
        if ((day1 != -693594) && (date1 < day1))
        {
            return 0;
        }
        if ((day2 != -693594) && (date1 > day2))
        {
            return -1;
        }
        date2 = date1 + S.datetime_encodeTime(hour, minute, 0);
        if (date2 <= S.PreviousDate)
        {
            S.report_writeErrorMsg(S.ERR_RAIN_FILE_SEQUENCE, S.Gage[S.GageIndex].fname);
            S.report_writeLine(line);
            return -1;
        }
        S.PreviousDate = date2;
        switch (S.RainType)
        {
            case S.RAINFALL_INTENSITY:
                x = x * (float)(S.Interval) / 3600.0f;
                break;
            case S.CUMULATIVE_RAINFALL:
                if (x >= S.RainAccum)
                {
                    x = x - S.RainAccum;
                    S.RainAccum += x;
                }
                else
                {
                    S.RainAccum = x;
                }
                break;
        }
        x *= (float)(S.UnitsFactor);
        S.saveRainfall(date1, hour, minute, x, 0);
        return 1;
    }

    // rain.c:1052
    public static int parseStdLine(CPtr line, ref int year, ref int month, ref int day, ref int hour, ref int minute, ref float value)
    {
        int n = 0;
        CPtr token = R.Buf(1024);
        n = R.Back(R.sscanf(line, S.L350, token, R.Slot, R.Slot, R.Slot, R.Slot, R.Slot, R.Slot), ref year, ref month, ref day, ref hour, ref minute, ref value);
        if (n < 7)
        {
            return 0;
        }
        if ((!S.StationID.IsNull) && (!((S.strcomp(token, S.StationID) != 0))))
        {
            return 0;
        }
        return 1;
    }

    // rain.c:1078
    public static void saveAccumRainfall(double date1, int hour, int minute, long v)
    {
        double date2 = 0.0;
        int n = 0;
        int j = 0;
        float x = 0.0f;
        if (S.AccumStartDate == -693594)
        {
            return;
        }
        date2 = date1 + S.datetime_encodeTime(hour, minute, 0);
        n = (int)((S.datetime_timeDiff(date2, S.AccumStartDate) / S.Interval) + 1);
        if (v == 99999)
        {
            S.RainStats.periodsMissing += n;
            return;
        }
        S.RainStats.periodsRain += n;
        x = (float)(v) / (float)(n) / 100.0f;
        if (x > 0.0f)
        {
            date2 = S.datetime_addSeconds(S.AccumStartDate, -S.TimeOffset);
            if (S.RainStats.startDate == -693594)
            {
                S.RainStats.startDate = date2;
            }
            for (j = 0; j < n; j++)
            {
                R.fwriteD1(date2, S.Frain.file);
                R.fwriteF1(x, S.Frain.file);
                date2 = S.datetime_addSeconds(date2, S.Interval);
                S.RainStats.endDate = date2;
            }
        }
        S.AccumStartDate = -693594;
    }

    // rain.c:1133
    public static void saveRainfall(double date1, int hour, int minute, float x, int isMissing)
    {
        double date2 = 0.0;
        int seconds = 0;
        if ((isMissing != 0))
        {
            S.RainStats.periodsMissing++;
        }
        else
        {
            S.RainStats.periodsRain++;
        }
        if (!(isMissing != 0))
        {
            seconds = 3600 * hour + 60 * minute - S.TimeOffset;
            date2 = S.datetime_addSeconds(date1, seconds);
            R.fwriteD1(date2, S.Frain.file);
            R.fwriteF1(x, S.Frain.file);
            if (S.RainStats.startDate == -693594)
            {
                S.RainStats.startDate = date2;
            }
            S.RainStats.endDate = date2;
        }
    }

}
}
