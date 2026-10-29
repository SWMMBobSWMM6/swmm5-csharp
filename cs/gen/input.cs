// Generated from EPA SWMM 5.2.4 (input.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int MAXERRS = 100;
    public static CPtr[] Tok = new CPtr[40];
    public static int Ntokens = 0;
    public static int[] Mobjects = new int[18];
    public static int[] Mnodes = new int[5];
    public static int[] Mlinks = new int[5];
    public static int Mevents = 0;

    // input.c:69
    public static int input_countObjects()
    {
        CPtr line = R.Buf(1025);
        CPtr wLine = R.Buf(1025);
        CPtr tok = CPtr.Null;
        int sect = -1;
        int newsect = 0;
        int errcode = 0;
        int errsum = 0;
        int i = 0;
        long lineCount = 0;
        if ((S.ErrorCode != 0))
        {
            return S.ErrorCode;
        }
        S.error_setInpError(0, S.L0);
        for (i = 0; i < S.MAX_OBJ_TYPES; i++)
        {
            S.Nobjects[i] = 0;
        }
        for (i = 0; i < 5; i++)
        {
            S.Nnodes[i] = 0;
        }
        for (i = 0; i < 5; i++)
        {
            S.Nlinks[i] = 0;
        }
        S.controls_init();
        while (!(R.fgets(line, 1024, S.Finp.file)).IsNull)
        {
            lineCount++;
            S.sstrncpy(wLine, line, unchecked((ulong)(1024)));
            tok = R.strtok(wLine, S.L184);
            if (tok.IsNull)
            {
                continue;
            }
            if (tok[0] == ';')
            {
                continue;
            }
            if (tok[0] == '[')
            {
                newsect = S.findmatch(tok, S.SectWords);
                if (newsect >= 0)
                {
                    sect = newsect;
                    continue;
                }
                else
                {
                    sect = -1;
                    errcode = S.ERR_KEYWORD;
                }
            }
            if (sect == S.s_OPTION)
            {
                errcode = S.readOption(line);
            }
            else if (sect >= 0)
            {
                errcode = S.addObject(sect, tok);
            }
            if ((errcode != 0))
            {
                S.report_writeInputErrorMsg(errcode, sect, line, lineCount);
                errsum++;
                if (errsum >= S.MAXERRS)
                {
                    break;
                }
            }
        }
        if (errsum > 0)
        {
            S.ErrorCode = S.ERR_INPUT;
        }
        return S.ErrorCode;
    }

    // input.c:141
    public static int input_readData()
    {
        CPtr line = R.Buf(1025);
        CPtr wLine = R.Buf(1025);
        CPtr comment = CPtr.Null;
        int sect = 0;
        int newsect = 0;
        int inperr = 0;
        int errsum = 0;
        int lineLength = 0;
        int i = 0;
        long lineCount = 0;
        if ((S.ErrorCode != 0))
        {
            return S.ErrorCode;
        }
        S.error_setInpError(0, S.L0);
        for (i = 0; i < S.MAX_OBJ_TYPES; i++)
        {
            S.Mobjects[i] = 0;
        }
        for (i = 0; i < 5; i++)
        {
            S.Mnodes[i] = 0;
        }
        for (i = 0; i < 5; i++)
        {
            S.Mlinks[i] = 0;
        }
        S.Mevents = 0;
        for (i = 0; i < S.Nobjects[S.TSERIES]; i++)
        {
            S.Tseries[i].lastDate = S.StartDate + S.StartTime;
        }
        sect = 0;
        errsum = 0;
        R.rewind(S.Finp.file);
        while (!(R.fgets(line, 1024, S.Finp.file)).IsNull)
        {
            lineCount++;
            S.sstrncpy(wLine, line, unchecked((ulong)(1024)));
            S.Ntokens = S.getTokens(wLine);
            if (S.Ntokens == 0)
            {
                continue;
            }
            if (S.Tok[0][0] == ';')
            {
                continue;
            }
            lineLength = (int)(R.strlen(line));
            if (lineLength >= 1024)
            {
                comment = R.strchr(line, ';');
                if (!(comment).IsNull)
                {
                    lineLength = (int)((CPtr.Diff(comment, line)));
                }
                if (lineLength >= 1024)
                {
                    inperr = S.ERR_LINE_LENGTH;
                    S.report_writeInputErrorMsg(inperr, sect, line, lineCount);
                    errsum++;
                }
            }
            if (S.Tok[0][0] == '[')
            {
                newsect = S.findmatch(S.Tok[0], S.SectWords);
                if (newsect >= 0)
                {
                    if (sect == S.s_TRANSECT)
                    {
                        S.transect_validate(S.Nobjects[S.TRANSECT] - 1);
                    }
                    sect = newsect;
                    continue;
                }
                else
                {
                    inperr = S.error_setInpError(S.ERR_KEYWORD, S.Tok[0]);
                    S.report_writeInputErrorMsg(inperr, sect, line, lineCount);
                    errsum++;
                    break;
                }
            }
            else
            {
                inperr = S.parseLine(sect, line);
                if (inperr > 0)
                {
                    errsum++;
                    if (errsum > S.MAXERRS)
                    {
                        S.report_writeLine(S.L201);
                    }
                    else
                    {
                        S.report_writeInputErrorMsg(inperr, sect, line, lineCount);
                    }
                }
            }
            if (errsum > S.MAXERRS)
            {
                break;
            }
        }
        if (errsum > 0)
        {
            S.ErrorCode = S.ERR_INPUT;
        }
        return S.ErrorCode;
    }

    // input.c:251
    public static int addObject(int objType, CPtr id)
    {
        int errcode = 0;
        switch (objType)
        {
            case S.s_RAINGAGE:
                if (!((S.project_addObject(S.GAGE, id, S.Nobjects[S.GAGE]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.GAGE]++;
                break;
            case S.s_SUBCATCH:
                if (!((S.project_addObject(S.SUBCATCH, id, S.Nobjects[S.SUBCATCH]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.SUBCATCH]++;
                break;
            case S.s_AQUIFER:
                if (!((S.project_addObject(S.AQUIFER, id, S.Nobjects[S.AQUIFER]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.AQUIFER]++;
                break;
            case S.s_UNITHYD:
                if (S.project_findObject(S.UNITHYD, id) < 0)
                {
                    if (!((S.project_addObject(S.UNITHYD, id, S.Nobjects[S.UNITHYD]) != 0)))
                    {
                        errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                    }
                    S.Nobjects[S.UNITHYD]++;
                }
                break;
            case S.s_SNOWMELT:
                if (S.project_findObject(S.SNOWMELT, id) < 0)
                {
                    if (!((S.project_addObject(S.SNOWMELT, id, S.Nobjects[S.SNOWMELT]) != 0)))
                    {
                        errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                    }
                    S.Nobjects[S.SNOWMELT]++;
                }
                break;
            case S.s_JUNCTION:
                if (!((S.project_addObject(S.NODE, id, S.Nobjects[S.NODE]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.NODE]++;
                S.Nnodes[S.JUNCTION]++;
                break;
            case S.s_OUTFALL:
                if (!((S.project_addObject(S.NODE, id, S.Nobjects[S.NODE]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.NODE]++;
                S.Nnodes[S.OUTFALL]++;
                break;
            case S.s_STORAGE:
                if (!((S.project_addObject(S.NODE, id, S.Nobjects[S.NODE]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.NODE]++;
                S.Nnodes[S.STORAGE]++;
                break;
            case S.s_DIVIDER:
                if (!((S.project_addObject(S.NODE, id, S.Nobjects[S.NODE]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.NODE]++;
                S.Nnodes[S.DIVIDER]++;
                break;
            case S.s_CONDUIT:
                if (!((S.project_addObject(S.LINK, id, S.Nobjects[S.LINK]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.LINK]++;
                S.Nlinks[S.CONDUIT]++;
                break;
            case S.s_PUMP:
                if (!((S.project_addObject(S.LINK, id, S.Nobjects[S.LINK]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.LINK]++;
                S.Nlinks[S.PUMP]++;
                break;
            case S.s_ORIFICE:
                if (!((S.project_addObject(S.LINK, id, S.Nobjects[S.LINK]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.LINK]++;
                S.Nlinks[S.ORIFICE]++;
                break;
            case S.s_WEIR:
                if (!((S.project_addObject(S.LINK, id, S.Nobjects[S.LINK]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.LINK]++;
                S.Nlinks[S.WEIR]++;
                break;
            case S.s_OUTLET:
                if (!((S.project_addObject(S.LINK, id, S.Nobjects[S.LINK]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.LINK]++;
                S.Nlinks[S.OUTLET]++;
                break;
            case S.s_POLLUTANT:
                if (!((S.project_addObject(S.POLLUT, id, S.Nobjects[S.POLLUT]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.POLLUT]++;
                break;
            case S.s_LANDUSE:
                if (!((S.project_addObject(S.LANDUSE, id, S.Nobjects[S.LANDUSE]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.LANDUSE]++;
                break;
            case S.s_PATTERN:
                if (S.project_findObject(S.TIMEPATTERN, id) < 0)
                {
                    if (!((S.project_addObject(S.TIMEPATTERN, id, S.Nobjects[S.TIMEPATTERN]) != 0)))
                    {
                        errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                    }
                    S.Nobjects[S.TIMEPATTERN]++;
                }
                break;
            case S.s_CURVE:
                if (S.project_findObject(S.CURVE, id) < 0)
                {
                    if (!((S.project_addObject(S.CURVE, id, S.Nobjects[S.CURVE]) != 0)))
                    {
                        errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                    }
                    S.Nobjects[S.CURVE]++;
                    id = R.strtok(CPtr.Null, S.L184);
                    if (S.findmatch(id, S.CurveTypeWords) == S.SHAPE_CURVE)
                    {
                        S.Nobjects[S.SHAPE]++;
                    }
                }
                break;
            case S.s_TIMESERIES:
                if (S.project_findObject(S.TSERIES, id) < 0)
                {
                    if (!((S.project_addObject(S.TSERIES, id, S.Nobjects[S.TSERIES]) != 0)))
                    {
                        errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                    }
                    S.Nobjects[S.TSERIES]++;
                }
                break;
            case S.s_CONTROL:
                if ((S.match(id, S.L202) != 0))
                {
                    S.Nobjects[S.CONTROL]++;
                }
                else
                {
                    S.controls_addToCount(id);
                }
                break;
            case S.s_TRANSECT:
                if ((S.match(id, S.L203) != 0))
                {
                    id = R.strtok(CPtr.Null, S.L184);
                    if (!(id).IsNull)
                    {
                        if (!((S.project_addObject(S.TRANSECT, id, S.Nobjects[S.TRANSECT]) != 0)))
                        {
                            errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                        }
                        S.Nobjects[S.TRANSECT]++;
                    }
                }
                break;
            case S.s_LID_CONTROL:
                if (S.project_findObject(S.LID, id) < 0)
                {
                    if (!((S.project_addObject(S.LID, id, S.Nobjects[S.LID]) != 0)))
                    {
                        errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                    }
                    S.Nobjects[S.LID]++;
                }
                break;
            case S.s_EVENT:
                S.NumEvents++;
                break;
            case S.s_STREET:
                if (!((S.project_addObject(S.STREET, id, S.Nobjects[S.STREET]) != 0)))
                {
                    errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                }
                S.Nobjects[S.STREET]++;
                break;
            case S.s_INLET:
                if (S.project_findObject(S.INLET, id) < 0)
                {
                    if (!((S.project_addObject(S.INLET, id, S.Nobjects[S.INLET]) != 0)))
                    {
                        errcode = S.error_setInpError(S.ERR_DUP_NAME, id);
                    }
                    S.Nobjects[S.INLET]++;
                }
                break;
        }
        return errcode;
    }

    // input.c:464
    public static int parseLine(int sect, CPtr line)
    {
        int j = 0;
        int err = 0;
        switch (sect)
        {
            case S.s_TITLE:
                return S.readTitle(line);
            case S.s_RAINGAGE:
                j = S.Mobjects[S.GAGE];
                err = S.gage_readParams(j, S.Tok, S.Ntokens);
                S.Mobjects[S.GAGE]++;
                return err;
            case S.s_TEMP:
                return S.climate_readParams(S.Tok, S.Ntokens);
            case S.s_EVAP:
                return S.climate_readEvapParams(S.Tok, S.Ntokens);
            case S.s_ADJUST:
                return S.climate_readAdjustments(S.Tok, S.Ntokens);
            case S.s_SUBCATCH:
                j = S.Mobjects[S.SUBCATCH];
                err = S.subcatch_readParams(j, S.Tok, S.Ntokens);
                S.Mobjects[S.SUBCATCH]++;
                return err;
            case S.s_SUBAREA:
                return S.subcatch_readSubareaParams(S.Tok, S.Ntokens);
            case S.s_INFIL:
                return S.infil_readParams(S.InfilModel, S.Tok, S.Ntokens);
            case S.s_AQUIFER:
                j = S.Mobjects[S.AQUIFER];
                err = S.gwater_readAquiferParams(j, S.Tok, S.Ntokens);
                S.Mobjects[S.AQUIFER]++;
                return err;
            case S.s_GROUNDWATER:
                return S.gwater_readGroundwaterParams(S.Tok, S.Ntokens);
            case S.s_GWF:
                return S.gwater_readFlowExpression(S.Tok, S.Ntokens);
            case S.s_SNOWMELT:
                return S.snow_readMeltParams(S.Tok, S.Ntokens);
            case S.s_JUNCTION:
                return S.readNode(S.JUNCTION);
            case S.s_OUTFALL:
                return S.readNode(S.OUTFALL);
            case S.s_STORAGE:
                return S.readNode(S.STORAGE);
            case S.s_DIVIDER:
                return S.readNode(S.DIVIDER);
            case S.s_CONDUIT:
                return S.readLink(S.CONDUIT);
            case S.s_PUMP:
                return S.readLink(S.PUMP);
            case S.s_ORIFICE:
                return S.readLink(S.ORIFICE);
            case S.s_WEIR:
                return S.readLink(S.WEIR);
            case S.s_OUTLET:
                return S.readLink(S.OUTLET);
            case S.s_XSECTION:
                return S.link_readXsectParams(S.Tok, S.Ntokens);
            case S.s_TRANSECT:
                return S.transect_readParams(ref S.Mobjects[S.TRANSECT], S.Tok, S.Ntokens);
            case S.s_LOSSES:
                return S.link_readLossParams(S.Tok, S.Ntokens);
            case S.s_POLLUTANT:
                j = S.Mobjects[S.POLLUT];
                err = S.landuse_readPollutParams(j, S.Tok, S.Ntokens);
                S.Mobjects[S.POLLUT]++;
                return err;
            case S.s_LANDUSE:
                j = S.Mobjects[S.LANDUSE];
                err = S.landuse_readParams(j, S.Tok, S.Ntokens);
                S.Mobjects[S.LANDUSE]++;
                return err;
            case S.s_BUILDUP:
                return S.landuse_readBuildupParams(S.Tok, S.Ntokens);
            case S.s_WASHOFF:
                return S.landuse_readWashoffParams(S.Tok, S.Ntokens);
            case S.s_COVERAGE:
                return S.subcatch_readLanduseParams(S.Tok, S.Ntokens);
            case S.s_INFLOW:
                return S.inflow_readExtInflow(S.Tok, S.Ntokens);
            case S.s_DWF:
                return S.inflow_readDwfInflow(S.Tok, S.Ntokens);
            case S.s_PATTERN:
                return S.inflow_readDwfPattern(S.Tok, S.Ntokens);
            case S.s_RDII:
                return S.rdii_readRdiiInflow(S.Tok, S.Ntokens);
            case S.s_UNITHYD:
                return S.rdii_readUnitHydParams(S.Tok, S.Ntokens);
            case S.s_LOADING:
                return S.subcatch_readInitBuildup(S.Tok, S.Ntokens);
            case S.s_TREATMENT:
                return S.treatmnt_readExpression(S.Tok, S.Ntokens);
            case S.s_CURVE:
                return S.table_readCurve(S.Tok, S.Ntokens);
            case S.s_TIMESERIES:
                return S.table_readTimeseries(S.Tok, S.Ntokens);
            case S.s_CONTROL:
                return S.readControl(S.Tok, S.Ntokens);
            case S.s_REPORT:
                return S.report_readOptions(S.Tok, S.Ntokens);
            case S.s_FILE:
                return S.iface_readFileParams(S.Tok, S.Ntokens);
            case S.s_LID_CONTROL:
                return S.lid_readProcParams(S.Tok, S.Ntokens);
            case S.s_LID_USAGE:
                return S.lid_readGroupParams(S.Tok, S.Ntokens);
            case S.s_EVENT:
                return S.readEvent(S.Tok, S.Ntokens);
            case S.s_STREET:
                return S.street_readParams(S.Tok, S.Ntokens);
            case S.s_INLET:
                return S.inlet_readDesignParams(S.Tok, S.Ntokens);
            case S.s_INLET_USAGE:
                return S.inlet_readUsageParams(S.Tok, S.Ntokens);
            default:
                return 0;
        }
    }

    // input.c:637
    public static int readControl(CPtr[] tok, int ntoks)
    {
        int index = 0;
        int keyword = 0;
        if (ntoks < 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        if ((S.match(tok[0], S.L29) != 0))
        {
            return S.controls_addVariable(tok, ntoks);
        }
        if ((S.match(tok[0], S.L30) != 0))
        {
            return S.controls_addExpression(tok, ntoks);
        }
        keyword = S.findmatch(tok[0], S.RuleKeyWords);
        if (keyword < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[0]);
        }
        if (keyword == 0)
        {
            if (!((S.project_addObject(S.CONTROL, tok[1], S.Mobjects[S.CONTROL]) != 0)))
            {
                return S.error_setInpError(S.ERR_DUP_NAME, S.Tok[1]);
            }
            S.Mobjects[S.CONTROL]++;
        }
        index = S.Mobjects[S.CONTROL] - 1;
        if (index < 0)
        {
            return S.error_setInpError(S.ERR_RULE, S.L0);
        }
        return S.controls_addRuleClause(index, keyword, S.Tok, S.Ntokens);
    }

    // input.c:680
    public static int readOption(CPtr line)
    {
        S.Ntokens = S.getTokens(line);
        if (S.Ntokens < 2)
        {
            return 0;
        }
        return S.project_readOption(S.Tok[0], S.Tok[1]);
    }

    // input.c:694
    public static int readTitle(CPtr line)
    {
        int i = 0;
        int n = 0;
        for (i = 0; i < 3; i++)
        {
            if (R.strlen(S.Title[i]) == unchecked((ulong)(0)))
            {
                n = (int)(R.strlen(line));
                if (line[n - 1] == 10)
                {
                    line[n - 1] = ' ';
                }
                S.sstrncpy(S.Title[i], line, unchecked((ulong)(1024)));
                break;
            }
        }
        return 0;
    }

    // input.c:721
    public static int readNode(int type)
    {
        int j = S.Mobjects[S.NODE];
        int k = S.Mnodes[type];
        int err = S.node_readParams(j, type, k, S.Tok, S.Ntokens);
        S.Mobjects[S.NODE]++;
        S.Mnodes[type]++;
        return err;
    }

    // input.c:738
    public static int readLink(int type)
    {
        int j = S.Mobjects[S.LINK];
        int k = S.Mlinks[type];
        int err = S.link_readParams(j, type, k, S.Tok, S.Ntokens);
        S.Mobjects[S.LINK]++;
        S.Mlinks[type]++;
        return err;
    }

    // input.c:755
    public static int readEvent(CPtr[] tok, int ntoks)
    {
        double[] x = new double[4];
        if (ntoks < 4)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        if (!((S.datetime_strToDate(tok[0], ref x[0]) != 0)))
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[0]);
        }
        if (!((S.datetime_strToTime(tok[1], ref x[1]) != 0)))
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[1]);
        }
        if (!((S.datetime_strToDate(tok[2], ref x[2]) != 0)))
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[2]);
        }
        if (!((S.datetime_strToTime(tok[3], ref x[3]) != 0)))
        {
            return S.error_setInpError(S.ERR_DATETIME, tok[3]);
        }
        S.Event[S.Mevents].start = x[0] + x[1];
        S.Event[S.Mevents].end = x[2] + x[3];
        if (S.Event[S.Mevents].start >= S.Event[S.Mevents].end)
        {
            return S.error_setInpError(S.ERR_DATETIME, S.L204);
        }
        S.Mevents++;
        return 0;
    }

    // input.c:779
    public static int findmatch(CPtr s, CPtr[] keyword)
    {
        int i = 0;
        while (!keyword[i].IsNull)
        {
            if ((S.match(s, keyword[i]) != 0))
            {
                return (i);
            }
            i++;
        }
        return (-1);
    }

    // input.c:798
    public static int match(CPtr str, CPtr substr)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        if (!(substr[0] != 0))
        {
            return (0);
        }
        for (k = 0; (str[k] != 0); k++)
        {
            if (str[k] != ' ')
            {
                break;
            }
        }
        for (i = k, j = 0; (substr[j] != 0); i++, j++)
        {
            if ((!(str[i] != 0)) || ((((((str[i]) >= 'a') && ((str[i]) <= 'z')) ? ((str[i]) & ~32) : (str[i]))) != (((((substr[j]) >= 'a') && ((substr[j]) <= 'z')) ? ((substr[j]) & ~32) : (substr[j])))))
            {
                return (0);
            }
        }
        return (1);
    }

    // input.c:828
    public static int getInt(CPtr s, ref int y)
    {
        double x = 0.0;
        if ((S.getDouble(s, ref x) != 0))
        {
            if (x < 0.0)
            {
                x -= 0.01;
            }
            else
            {
                x += 0.01;
            }
            y = R.D2I(x);
            return 1;
        }
        y = 0;
        return 0;
    }

    // input.c:850
    public static int getFloat(CPtr s, ref float y)
    {
        CPtr endptr = CPtr.Null;
        y = (float)(R.strtod(s, ref endptr));
        if (endptr[0] > 0)
        {
            return (0);
        }
        return (1);
    }

    // input.c:866
    public static int getDouble(CPtr s, ref double y)
    {
        CPtr endptr = CPtr.Null;
        y = R.strtod(s, ref endptr);
        if (endptr[0] > 0)
        {
            return (0);
        }
        return (1);
    }

    // input.c:882
    public static int getTokens(CPtr s)
    {
        int len = 0;
        int n = 0;
        int m = 0;
        CPtr c = CPtr.Null;
        for (n = 0; n < 40; n++)
        {
            S.Tok[n] = CPtr.Null;
        }
        n = 0;
        c = R.strchr(s, ';');
        if (!(c).IsNull)
        {
            c[0] = 0;
        }
        len = (int)(R.strlen(s));
        while ((len > 0) && (n < 40))
        {
            m = (int)(R.strcspn(s, S.L184));
            if (m == 0)
            {
                s++;
            }
            else
            {
                if (s[0] == '"')
                {
                    s++;
                    len--;
                    m = (int)(R.strcspn(s, S.L205));
                }
                s[m] = 0;
                S.Tok[n] = s;
                n++;
                s += m + 1;
            }
            len -= m + 1;
        }
        return n;
    }

}
}
