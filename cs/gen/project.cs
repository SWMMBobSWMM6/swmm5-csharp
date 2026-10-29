// Generated from EPA SWMM 5.2.4 (project.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static HTentry[][] Htable = new HTentry[18][];
    public static int MemPoolAllocated = 0;

    // project.c:70
    public static int omp_get_max_threads()
    {
        return 1;
    }

    // project.c:113
    public static void project_open(CPtr f1, CPtr f2, CPtr f3)
    {
        S.initPointers();
        S.setDefaults();
        S.openFiles(f1, f2, f3);
    }

    // project.c:129
    public static void project_readInput()
    {
        S.createHashTables();
        S.input_countObjects();
        S.createObjects();
        S.input_readData();
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.StartDateTime = S.StartDate + S.StartTime;
        S.EndDateTime = S.EndDate + S.EndTime;
        S.ReportStart = S.ReportStartDate + S.ReportStartTime;
        S.ReportStart = ((((S.ReportStart) >= (S.StartDateTime)) ? (S.ReportStart) : (S.StartDateTime)));
        if (S.EndDateTime <= S.StartDateTime)
        {
            S.report_writeErrorMsg(S.ERR_START_DATE, S.L0);
        }
        else if (S.EndDateTime <= S.ReportStart)
        {
            S.report_writeErrorMsg(S.ERR_REPORT_DATE, S.L0);
        }
        else
        {
            S.TotalDuration = Math.Floor((S.EndDateTime - S.StartDateTime) * 86400);
            if ((double)(S.ReportStep) > S.TotalDuration)
            {
                S.ReportStep = R.D2I((S.TotalDuration));
            }
            if ((double)(S.ReportStep) < S.RouteStep)
            {
                S.report_writeErrorMsg(S.ERR_REPORT_STEP, S.L0);
            }
            S.TotalDuration *= 1000.0;
        }
    }

    // project.c:186
    public static void project_validate()
    {
        int i = 0;
        int j = 0;
        int err = 0;
        for (i = 0; i < S.Nobjects[S.CURVE]; i++)
        {
            err = S.table_validate(S.Curve[i]);
            if ((err != 0))
            {
                S.report_writeErrorMsg(S.ERR_CURVE_SEQUENCE, S.Curve[i].ID);
            }
        }
        for (i = 0; i < S.Nobjects[S.TSERIES]; i++)
        {
            err = S.table_validate(S.Tseries[i]);
            if ((err != 0))
            {
                S.report_writeTseriesErrorMsg(err, S.Tseries[i]);
            }
        }
        S.climate_validate();
        S.lid_validate();
        if (S.Nobjects[S.SNOWMELT] == 0)
        {
            S.IgnoreSnowmelt = 1;
        }
        if (S.Nobjects[S.AQUIFER] == 0)
        {
            S.IgnoreGwater = 1;
        }
        for (i = 0; i < S.Nobjects[S.AQUIFER]; i++)
        {
            S.gwater_validateAquifer(i);
        }
        for (i = 0; i < S.Nobjects[S.SUBCATCH]; i++)
        {
            S.subcatch_validate(i);
        }
        for (i = 0; i < S.Nobjects[S.GAGE]; i++)
        {
            S.gage_validate(i);
        }
        for (i = 0; i < S.Nobjects[S.SNOWMELT]; i++)
        {
            S.snow_validateSnowmelt(i);
        }
        j = 0;
        for (i = 0; i < S.Nobjects[S.CURVE]; i++)
        {
            if (S.Curve[i].curveType == S.SHAPE_CURVE)
            {
                S.Curve[i].refersTo = j;
                S.Shape[j].curve = i;
                if (!((S.shape_validate(S.Shape[j], S.Curve[i]) != 0)))
                {
                    S.report_writeErrorMsg(S.ERR_CURVE_SEQUENCE, S.Curve[i].ID);
                }
                j++;
            }
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.Node[i].oldDepth = S.Node[i].fullDepth;
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            S.link_validate(i);
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.node_validate(i);
        }
        if (S.DryStep < S.WetStep)
        {
            S.report_writeWarningMsg(S.L319, S.L0);
            S.DryStep = S.WetStep;
        }
        if (S.RouteStep > (double)(S.WetStep))
        {
            S.report_writeWarningMsg(S.L320, S.L0);
            S.RouteStep = S.WetStep;
        }
        if (S.RptFlags.subcatchments == S.ALL)
        {
            for (i = 0; i < S.Nobjects[S.SUBCATCH]; i++)
            {
                S.Subcatch[i].rptFlag = 1;
            }
        }
        if (S.RptFlags.nodes == S.ALL)
        {
            for (i = 0; i < S.Nobjects[S.NODE]; i++)
            {
                S.Node[i].rptFlag = 1;
            }
        }
        if (S.RptFlags.links == S.ALL)
        {
            for (i = 0; i < S.Nobjects[S.LINK]; i++)
            {
                S.Link[i].rptFlag = 1;
            }
        }
        if (S.RouteModel == S.DW)
        {
            S.dynwave_validate();
        }
        S.inlet_validate();
        if (S.NumThreads == 0)
        {
            S.NumThreads = S.omp_get_max_threads();
        }
        else
        {
            S.NumThreads = ((((S.NumThreads) <= (S.omp_get_max_threads())) ? (S.NumThreads) : (S.omp_get_max_threads())));
        }
        if (S.Nobjects[S.LINK] < 4 * S.NumThreads)
        {
            S.NumThreads = 1;
        }
    }

    // project.c:274
    public static void project_close()
    {
        S.deleteObjects();
        S.deleteHashTables();
    }

    // project.c:287
    public static int project_init()
    {
        int j = 0;
        int k = 0;
        S.climate_initState();
        S.lid_initState();
        for (j = 0; j < S.Nobjects[S.TSERIES]; j++)
        {
            S.table_tseriesInit(S.Tseries[j]);
        }
        for (j = 0; j < S.Nobjects[S.GAGE]; j++)
        {
            S.gage_initState(j);
        }
        k = 1;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.subcatch_initState(j);
            if (S.Subcatch[j].rptFlag > 0)
            {
                S.Subcatch[j].rptFlag = k;
                k++;
            }
        }
        k = 1;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.node_initState(j);
            if (S.Node[j].rptFlag > 0)
            {
                S.Node[j].rptFlag = k;
                k++;
            }
        }
        k = 1;
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.link_initState(j);
            if (S.Link[j].rptFlag > 0)
            {
                S.Link[j].rptFlag = k;
                k++;
            }
        }
        return S.ErrorCode;
    }

    // project.c:334
    public static int project_addObject(int type, CPtr id, int n)
    {
        int result = 0;
        long len = 0L;
        CPtr newID = CPtr.Null;
        if (S.project_findObject(type, id) >= 0)
        {
            return 0;
        }
        len = (long)(R.strlen(id));
        newID = S.Alloc((long)(unchecked((ulong)((len + 1))) * 1));
        S.sstrncpy(newID, id, unchecked((ulong)(len)));
        result = S.HTinsert(S.Htable[type], newID, n);
        if (result == 0)
        {
            result = -1;
        }
        return result;
    }

    // project.c:364
    public static int project_findObject(int type, CPtr id)
    {
        return S.HTfind(S.Htable[type], id);
    }

    // project.c:377
    public static CPtr project_findID(int type, CPtr id)
    {
        return S.HTfindKey(S.Htable[type], id);
    }

    // project.c:445
    public static int project_readOption(CPtr s1, CPtr s2)
    {
        int k = 0;
        int m = 0;
        int h = 0;
        int s = 0;
        double tStep = 0.0;
        CPtr strDate = R.Buf(25);
        double aTime = 0.0;
        double aDate = 0.0;
        k = S.findmatch(s1, S.OptionWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, s1);
        }
        switch (k)
        {
            case S.FLOW_UNITS:
                m = S.findmatch(s2, S.FlowUnitWords);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                S.FlowUnits = m;
                if (S.FlowUnits <= S.MGD)
                {
                    S.UnitSystem = S.US;
                }
                else
                {
                    S.UnitSystem = S.SI;
                }
                break;
            case S.INFIL_MODEL:
                m = S.findmatch(s2, S.InfilModelWords);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                S.InfilModel = m;
                break;
            case S.ROUTE_MODEL:
                m = S.findmatch(s2, S.RouteModelWords);
                if (m < 0)
                {
                    m = S.findmatch(s2, S.OldRouteModelWords);
                }
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                if (m == S.NO_ROUTING)
                {
                    S.IgnoreRouting = 1;
                }
                else
                {
                    S.RouteModel = m;
                }
                if (S.RouteModel == S.EKW)
                {
                    S.RouteModel = S.KW;
                }
                break;
            case S.START_DATE:
                if (!((S.datetime_strToDate(s2, ref S.StartDate) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, s2);
                }
                break;
            case S.START_TIME:
                if (!((S.datetime_strToTime(s2, ref S.StartTime) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, s2);
                }
                break;
            case S.END_DATE:
                if (!((S.datetime_strToDate(s2, ref S.EndDate) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, s2);
                }
                break;
            case S.END_TIME:
                if (!((S.datetime_strToTime(s2, ref S.EndTime) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, s2);
                }
                break;
            case S.REPORT_START_DATE:
                if (!((S.datetime_strToDate(s2, ref S.ReportStartDate) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, s2);
                }
                break;
            case S.REPORT_START_TIME:
                if (!((S.datetime_strToTime(s2, ref S.ReportStartTime) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, s2);
                }
                break;
            case S.SWEEP_START:
            case S.SWEEP_END:
                S.sstrncpy(strDate, s2, unchecked((ulong)(24)));
                S.sstrcat(strDate, S.L35, unchecked((ulong)(25)));
                if (!((S.datetime_strToDate(strDate, ref aDate) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, s2);
                }
                m = S.datetime_dayOfYear(aDate);
                if (k == S.SWEEP_START)
                {
                    S.SweepStart = m;
                }
                else
                {
                    S.SweepEnd = m;
                }
                break;
            case S.START_DRY_DAYS:
                S.StartDryDays = R.atof(s2);
                if (S.StartDryDays < 0.0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                break;
            case S.WET_STEP:
            case S.DRY_STEP:
            case S.REPORT_STEP:
            case S.RULE_STEP:
                if (!((S.datetime_strToTime(s2, ref aTime) != 0)))
                {
                    return S.error_setInpError(S.ERR_DATETIME, s2);
                }
                S.datetime_decodeTime(aTime, ref h, ref m, ref s);
                h += 24 * R.D2I(aTime);
                s = s + 60 * m + 3600 * h;
                if (k == S.RULE_STEP)
                {
                    if (s < 0)
                    {
                        return S.error_setInpError(S.ERR_NUMBER, s2);
                    }
                }
                else if (s <= 0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                switch (k)
                {
                    case S.WET_STEP:
                        S.WetStep = s;
                        break;
                    case S.DRY_STEP:
                        S.DryStep = s;
                        break;
                    case S.REPORT_STEP:
                        S.ReportStep = s;
                        break;
                    case S.RULE_STEP:
                        S.RuleStep = s;
                        break;
                }
                break;
            case S.INERT_DAMPING:
                m = S.findmatch(s2, S.InertDampingWords);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                else
                {
                    S.InertDamping = m;
                }
                break;
            case S.ALLOW_PONDING:
            case S.SLOPE_WEIGHTING:
            case S.SKIP_STEADY_STATE:
            case S.IGNORE_RAINFALL:
            case S.IGNORE_SNOWMELT:
            case S.IGNORE_GWATER:
            case S.IGNORE_ROUTING:
            case S.IGNORE_QUALITY:
            case S.IGNORE_RDII:
                m = S.findmatch(s2, S.NoYesWords);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                switch (k)
                {
                    case S.ALLOW_PONDING:
                        S.AllowPonding = m;
                        break;
                    case S.SLOPE_WEIGHTING:
                        S.SlopeWeighting = m;
                        break;
                    case S.SKIP_STEADY_STATE:
                        S.SkipSteadyState = m;
                        break;
                    case S.IGNORE_RAINFALL:
                        S.IgnoreRainfall = m;
                        break;
                    case S.IGNORE_SNOWMELT:
                        S.IgnoreSnowmelt = m;
                        break;
                    case S.IGNORE_GWATER:
                        S.IgnoreGwater = m;
                        break;
                    case S.IGNORE_ROUTING:
                        S.IgnoreRouting = m;
                        break;
                    case S.IGNORE_QUALITY:
                        S.IgnoreQuality = m;
                        break;
                    case S.IGNORE_RDII:
                        S.IgnoreRDII = m;
                        break;
                }
                break;
            case S.NORMAL_FLOW_LTD:
                m = S.findmatch(s2, S.NormalFlowWords);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                S.NormalFlowLtd = m;
                break;
            case S.FORCE_MAIN_EQN:
                m = S.findmatch(s2, S.ForceMainEqnWords);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                S.ForceMainEqn = m;
                break;
            case S.LINK_OFFSETS:
                m = S.findmatch(s2, S.LinkOffsetWords);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                S.LinkOffsets = m;
                break;
            case S.COMPATIBILITY:
                if ((S.strcomp(s2, S.L321) != 0))
                {
                    S.Compatibility = S.SWMM3;
                }
                else if ((S.strcomp(s2, S.L322) != 0))
                {
                    S.Compatibility = S.SWMM4;
                }
                else if ((S.strcomp(s2, S.L323) != 0))
                {
                    S.Compatibility = S.SWMM5;
                }
                else
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                break;
            case S.ROUTE_STEP:
            case S.LENGTHENING_STEP:
                if (!((S.getDouble(s2, ref tStep) != 0)))
                {
                    if (!((S.datetime_strToTime(s2, ref aTime) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, s2);
                    }
                    else
                    {
                        S.datetime_decodeTime(aTime, ref h, ref m, ref s);
                        h += 24 * R.D2I(aTime);
                        s = s + 60 * m + 3600 * h;
                        tStep = s;
                    }
                }
                if (k == S.ROUTE_STEP)
                {
                    if (tStep <= 0.0)
                    {
                        return S.error_setInpError(S.ERR_NUMBER, s2);
                    }
                    S.RouteStep = tStep;
                }
                else
                {
                    S.LengtheningStep = ((((0.0) >= (tStep)) ? (0.0) : (tStep)));
                }
                break;
            case S.MIN_ROUTE_STEP:
                if ((!((S.getDouble(s2, ref S.MinRouteStep) != 0))) || (S.MinRouteStep < 0.0))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                break;
            case S.NUM_THREADS:
                m = R.atoi(s2);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                S.NumThreads = m;
                break;
            case S.VARIABLE_STEP:
                if (!((S.getDouble(s2, ref S.CourantFactor) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                if ((S.CourantFactor < 0.0) || (S.CourantFactor > 2.0))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                break;
            case S.MIN_SURFAREA:
                if (!((S.getDouble(s2, ref S.MinSurfArea) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                if (S.MinSurfArea < 0.0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                break;
            case S.MIN_SLOPE:
                if (!((S.getDouble(s2, ref S.MinSlope) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                if ((S.MinSlope < 0.0) || (S.MinSlope >= 100))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                S.MinSlope /= 100.0;
                break;
            case S.MAX_TRIALS:
                m = R.atoi(s2);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                S.MaxTrials = m;
                break;
            case S.HEAD_TOL:
                if (!((S.getDouble(s2, ref S.HeadTol) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                break;
            case S.SYS_FLOW_TOL:
                if (!((S.getDouble(s2, ref S.SysFlowTol) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                S.SysFlowTol /= 100.0;
                break;
            case S.LAT_FLOW_TOL:
                if (!((S.getDouble(s2, ref S.LatFlowTol) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, s2);
                }
                S.LatFlowTol /= 100.0;
                break;
            case S.SURCHARGE_METHOD:
                m = S.findmatch(s2, S.SurchargeWords);
                if (m < 0)
                {
                    return S.error_setInpError(S.ERR_KEYWORD, s2);
                }
                S.SurchargeMethod = m;
                break;
            case S.TEMPDIR:
                S.sstrncpy(S.TempDir, s2, unchecked((ulong)(259)));
                break;
        }
        return 0;
    }

    // project.c:773
    public static void initPointers()
    {
        S.Gage = (null);
        S.Subcatch = (null);
        S.Node = (null);
        S.Outfall = (null);
        S.Divider = (null);
        S.Storage = (null);
        S.Link = (null);
        S.Conduit = (null);
        S.Pump = (null);
        S.Orifice = (null);
        S.Weir = (null);
        S.Outlet = (null);
        S.Pollut = (null);
        S.Landuse = (null);
        S.Pattern = (null);
        S.Curve = (null);
        S.Tseries = (null);
        S.Transect = (null);
        S.Shape = (null);
        S.Aquifer = (null);
        S.UnitHyd = (null);
        S.Snowmelt = (null);
        S.Event = (null);
        S.MemPoolAllocated = 0;
    }

    // project.c:808
    public static void setDefaults()
    {
        int i = 0;
        int j = 0;
        for (i = 0; i < 3; i++)
        {
            S.sstrncpy(S.Title[i], S.L0, unchecked((ulong)(0)));
        }
        S.sstrncpy(S.TempDir, S.L0, unchecked((ulong)(0)));
        S.Frain.mode = S.SCRATCH_FILE;
        S.Fclimate.mode = S.NO_FILE;
        S.Frunoff.mode = S.NO_FILE;
        S.Frdii.mode = S.NO_FILE;
        S.Fhotstart1.mode = S.NO_FILE;
        S.Fhotstart2.mode = S.NO_FILE;
        S.Finflows.mode = S.NO_FILE;
        S.Foutflows.mode = S.NO_FILE;
        S.Frain.file = (null);
        S.Fclimate.file = (null);
        S.Frunoff.file = (null);
        S.Frdii.file = (null);
        S.Fhotstart1.file = (null);
        S.Fhotstart2.file = (null);
        S.Finflows.file = (null);
        S.Foutflows.file = (null);
        S.Fout.file = (null);
        S.Fout.mode = S.NO_FILE;
        S.UnitSystem = S.US;
        S.FlowUnits = S.CFS;
        S.InfilModel = S.HORTON;
        S.RouteModel = S.DW;
        S.SurchargeMethod = S.EXTRAN;
        S.CrownCutoff = 0.96;
        S.AllowPonding = 0;
        S.InertDamping = S.PARTIAL_DAMPING;
        S.NormalFlowLtd = S.BOTH;
        S.ForceMainEqn = S.H_W;
        S.LinkOffsets = S.DEPTH_OFFSET;
        S.LengtheningStep = 0;
        S.CourantFactor = 0.75;
        S.MinSurfArea = 0.0;
        S.MinSlope = 0.0;
        S.SkipSteadyState = 0;
        S.IgnoreRainfall = 0;
        S.IgnoreRDII = 0;
        S.IgnoreSnowmelt = 0;
        S.IgnoreGwater = 0;
        S.IgnoreRouting = 0;
        S.IgnoreQuality = 0;
        S.WetStep = 300;
        S.DryStep = 3600;
        S.RuleStep = 0;
        S.RouteStep = 20;
        S.MinRouteStep = 0.5;
        S.ReportStep = 900;
        S.StartDryDays = 0.0;
        S.MaxTrials = 0;
        S.HeadTol = 0.0;
        S.SysFlowTol = 0.05;
        S.LatFlowTol = 0.05;
        S.NumThreads = 1;
        S.NumEvents = 0;
        S.SlopeWeighting = 1;
        S.Compatibility = S.SWMM4;
        S.StartDate = S.datetime_encodeDate(2004, 1, 1);
        S.StartTime = S.datetime_encodeTime(0, 0, 0);
        S.StartDateTime = S.StartDate + S.StartTime;
        S.EndDate = S.StartDate;
        S.EndTime = 0.0;
        S.ReportStartDate = -693594;
        S.ReportStartTime = -693594;
        S.SweepStart = 1;
        S.SweepEnd = 365;
        S.RptFlags.disabled = 0;
        S.RptFlags.input = 0;
        S.RptFlags.continuity = 1;
        S.RptFlags.flowStats = 1;
        S.RptFlags.controls = 0;
        S.RptFlags.subcatchments = 0;
        S.RptFlags.nodes = 0;
        S.RptFlags.links = 0;
        S.RptFlags.averages = 0;
        S.Temp.dataSource = S.NO_TEMP;
        S.Temp.tSeries = -1;
        S.Temp.ta = 70.0;
        S.Temp.elev = 0.0;
        S.Temp.anglat = 40.0;
        S.Temp.dtlong = 0.0;
        S.Temp.tmax = -10000000000.0;
        S.Wind.type = S.MONTHLY_WIND;
        for (i = 0; i < 12; i++)
        {
            S.Wind.aws[i] = 0.0;
        }
        S.Snow.snotmp = 34.0;
        S.Snow.tipm = 0.5;
        S.Snow.rnm = 0.6;
        for (i = 0; i < 2; i++)
        {
            for (j = 0; j < 10; j++)
            {
                S.Snow.adc[i][j] = 1.0;
            }
        }
        S.Evap.type = S.CONSTANT_EVAP;
        for (i = 0; i < 12; i++)
        {
            S.Evap.monthlyEvap[i] = 0.0;
            S.Evap.panCoeff[i] = 1.0;
        }
        S.Evap.recoveryPattern = -1;
        S.Evap.recoveryFactor = 1.0;
        S.Evap.tSeries = -1;
        S.Evap.dryOnly = 0;
        for (i = 0; i < 12; i++)
        {
            S.Adjust.temp[i] = 0.0;
            S.Adjust.evap[i] = 0.0;
            S.Adjust.rain[i] = 1.0;
            S.Adjust.hydcon[i] = 1.0;
        }
        S.Adjust.rainFactor = 1.0;
        S.Adjust.hydconFactor = 1.0;
    }

    // project.c:954
    public static void openFiles(CPtr f1, CPtr f2, CPtr f3)
    {
        S.Finp.file = (null);
        S.Frpt.file = (null);
        S.Fout.file = (null);
        S.sstrncpy(S.Finp.name, f1, unchecked((ulong)(259)));
        S.sstrncpy(S.Frpt.name, f2, unchecked((ulong)(259)));
        S.sstrncpy(S.Fout.name, f3, unchecked((ulong)(259)));
        if ((((S.strcomp(f1, f2) != 0)) || ((S.strcomp(f1, f3) != 0))) || ((S.strcomp(f2, f3) != 0)))
        {
            S.writecon(S.L324);
            S.ErrorCode = S.ERR_FILE_NAME;
            return;
        }
        if ((S.Finp.file = R.fopen(f1, S.L13)) == null)
        {
            S.writecon(S.L325);
            S.writecon(f1);
            S.ErrorCode = S.ERR_INP_FILE;
            return;
        }
        if ((S.Frpt.file = R.fopen(f2, S.L171)) == null)
        {
            S.writecon(S.L326);
            S.ErrorCode = S.ERR_RPT_FILE;
            return;
        }
    }

    // project.c:999
    public static void createObjects()
    {
        int j = 0;
        int k = 0;
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.Gage = R.NewArray<TGage>((int)(unchecked((ulong)(S.Nobjects[S.GAGE]))));
        S.Subcatch = R.NewArray<TSubcatch>((int)(unchecked((ulong)(S.Nobjects[S.SUBCATCH]))));
        S.Node = R.NewArray<TNode>((int)(unchecked((ulong)(S.Nobjects[S.NODE]))));
        S.Outfall = R.NewArray<TOutfall>((int)(unchecked((ulong)(S.Nnodes[S.OUTFALL]))));
        S.Divider = R.NewArray<TDivider>((int)(unchecked((ulong)(S.Nnodes[S.DIVIDER]))));
        S.Storage = R.NewArray<TStorage>((int)(unchecked((ulong)(S.Nnodes[S.STORAGE]))));
        S.Link = R.NewArray<TLink>((int)(unchecked((ulong)(S.Nobjects[S.LINK]))));
        S.Conduit = R.NewArray<TConduit>((int)(unchecked((ulong)(S.Nlinks[S.CONDUIT]))));
        S.Pump = R.NewArray<TPump>((int)(unchecked((ulong)(S.Nlinks[S.PUMP]))));
        S.Orifice = R.NewArray<TOrifice>((int)(unchecked((ulong)(S.Nlinks[S.ORIFICE]))));
        S.Weir = R.NewArray<TWeir>((int)(unchecked((ulong)(S.Nlinks[S.WEIR]))));
        S.Outlet = R.NewArray<TOutlet>((int)(unchecked((ulong)(S.Nlinks[S.OUTLET]))));
        S.Pollut = R.NewArray<TPollut>((int)(unchecked((ulong)(S.Nobjects[S.POLLUT]))));
        S.Landuse = R.NewArray<TLanduse>((int)(unchecked((ulong)(S.Nobjects[S.LANDUSE]))));
        S.Pattern = R.NewArray<TPattern>((int)(unchecked((ulong)(S.Nobjects[S.TIMEPATTERN]))));
        S.Curve = R.NewArray<TTable>((int)(unchecked((ulong)(S.Nobjects[S.CURVE]))));
        S.Tseries = R.NewArray<TTable>((int)(unchecked((ulong)(S.Nobjects[S.TSERIES]))));
        S.Aquifer = R.NewArray<TAquifer>((int)(unchecked((ulong)(S.Nobjects[S.AQUIFER]))));
        S.UnitHyd = R.NewArray<TUnitHyd>((int)(unchecked((ulong)(S.Nobjects[S.UNITHYD]))));
        S.Snowmelt = R.NewArray<TSnowmelt>((int)(unchecked((ulong)(S.Nobjects[S.SNOWMELT]))));
        S.Shape = R.NewArray<TShape>((int)(unchecked((ulong)(S.Nobjects[S.SHAPE]))));
        S.Event = R.NewArray<TEvent>((int)(unchecked((ulong)(S.NumEvents)) + unchecked((ulong)(1))));
        S.Event[S.NumEvents].start = 10000000000.0;
        S.Event[S.NumEvents].end = 10000000000.0 + 1.0;
        S.lid_create(S.Nobjects[S.LID], S.Nobjects[S.SUBCATCH]);
        S.ErrorCode = S.controls_create(S.Nobjects[S.CONTROL]);
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.ErrorCode = S.transect_create(S.Nobjects[S.TRANSECT]);
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.ErrorCode = S.street_create(S.Nobjects[S.STREET]);
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.ErrorCode = S.inlet_create(S.Nobjects[S.INLET]);
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.infil_create(S.Nobjects[S.SUBCATCH]);
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.Subcatch[j].initBuildup = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Subcatch[j].oldQual = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Subcatch[j].newQual = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Subcatch[j].pondedQual = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Subcatch[j].totalLoad = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.Node[j].oldQual = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Node[j].newQual = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Node[j].extInflow = (null);
            S.Node[j].dwfInflow = (null);
            S.Node[j].rdiiInflow = (null);
            S.Node[j].treatment = (null);
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.Link[j].inlet = (null);
            S.Link[j].oldQual = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Link[j].newQual = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            S.Link[j].totalLoad = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
        }
        for (j = 0; j < S.Nobjects[S.LANDUSE]; j++)
        {
            S.Landuse[j].buildupFunc = R.NewArray<TBuildup>((int)(unchecked((ulong)(S.Nobjects[S.POLLUT]))));
            S.Landuse[j].washoffFunc = R.NewArray<TWashoff>((int)(unchecked((ulong)(S.Nobjects[S.POLLUT]))));
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.Subcatch[j].landFactor = R.NewArray<TLandFactor>((int)(unchecked((ulong)(S.Nobjects[S.LANDUSE]))));
            for (k = 0; k < S.Nobjects[S.LANDUSE]; k++)
            {
                S.Subcatch[j].landFactor[k].buildup = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            }
        }
        for (j = 0; j < S.Nobjects[S.LANDUSE]; j++)
        {
            for (k = 0; k < S.Nobjects[S.POLLUT]; k++)
            {
                S.Landuse[j].buildupFunc[k].funcType = S.NO_BUILDUP;
                S.Landuse[j].buildupFunc[k].normalizer = S.PER_AREA;
                S.Landuse[j].washoffFunc[k].funcType = S.NO_WASHOFF;
            }
        }
        for (j = 0; j < S.Nobjects[S.GAGE]; j++)
        {
            S.Gage[j].tSeries = -1;
            S.sstrncpy(S.Gage[j].fname, S.L0, unchecked((ulong)(0)));
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.Subcatch[j].outSubcatch = -1;
            S.Subcatch[j].outNode = -1;
            S.Subcatch[j].infil = -1;
            S.Subcatch[j].groundwater = (null);
            S.Subcatch[j].gwLatFlowExpr = (null);
            S.Subcatch[j].gwDeepFlowExpr = (null);
            S.Subcatch[j].snowpack = (null);
            S.Subcatch[j].lidArea = 0.0;
            for (k = 0; k < S.Nobjects[S.POLLUT]; k++)
            {
                S.Subcatch[j].initBuildup[k] = 0.0;
            }
        }
        for (j = 0; j < S.Nobjects[S.UNITHYD]; j++)
        {
            S.rdii_initUnitHyd(j);
        }
        for (j = 0; j < S.Nobjects[S.SNOWMELT]; j++)
        {
            S.snow_initSnowmelt(j);
        }
        for (j = 0; j < S.Nnodes[S.STORAGE]; j++)
        {
            S.Storage[j].exfil = (null);
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.Link[j].xsect.type = -1;
            S.Link[j].cLossInlet = 0.0;
            S.Link[j].cLossOutlet = 0.0;
            S.Link[j].cLossAvg = 0.0;
            S.Link[j].hasFlapGate = 0;
        }
        for (j = 0; j < S.Nlinks[S.PUMP]; j++)
        {
            S.Pump[j].pumpCurve = -1;
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.Subcatch[j].rptFlag = 0;
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.Node[j].rptFlag = 0;
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            S.Link[j].rptFlag = 0;
        }
        for (j = 0; j < S.Nobjects[S.CURVE]; j++)
        {
            S.table_init(S.Curve[j]);
        }
        for (j = 0; j < S.Nobjects[S.TSERIES]; j++)
        {
            S.table_init(S.Tseries[j]);
        }
        for (j = 0; j < S.Nobjects[S.TIMEPATTERN]; j++)
        {
            S.inflow_initDwfPattern(j);
        }
    }

    // project.c:1176
    public static void deleteObjects()
    {
        int j = 0;
        int k = 0;
        if ((S.Subcatch != null))
        {
            for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
            {
                for (k = 0; k < S.Nobjects[S.LANDUSE]; k++)
                {
                    {
                        if ((S.Subcatch[j].landFactor[k].buildup != null))
                        {
                            R.free(S.Subcatch[j].landFactor[k].buildup);
                            S.Subcatch[j].landFactor[k].buildup = (null);
                        }
                    }
                    ;
                }
                {
                    if ((S.Subcatch[j].landFactor != null))
                    {
                        R.free(S.Subcatch[j].landFactor);
                        S.Subcatch[j].landFactor = (null);
                    }
                }
                ;
                {
                    if ((S.Subcatch[j].groundwater != null))
                    {
                        R.free(S.Subcatch[j].groundwater);
                        S.Subcatch[j].groundwater = (null);
                    }
                }
                ;
                S.gwater_deleteFlowExpression(j);
                {
                    if ((S.Subcatch[j].snowpack != null))
                    {
                        R.free(S.Subcatch[j].snowpack);
                        S.Subcatch[j].snowpack = (null);
                    }
                }
                ;
            }
        }
        if ((S.Landuse != null))
        {
            for (j = 0; j < S.Nobjects[S.LANDUSE]; j++)
            {
                {
                    if ((S.Landuse[j].buildupFunc != null))
                    {
                        R.free(S.Landuse[j].buildupFunc);
                        S.Landuse[j].buildupFunc = (null);
                    }
                }
                ;
                {
                    if ((S.Landuse[j].washoffFunc != null))
                    {
                        R.free(S.Landuse[j].washoffFunc);
                        S.Landuse[j].washoffFunc = (null);
                    }
                }
            }
        }
        if ((S.Subcatch != null))
        {
            for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
            {
                {
                    if ((S.Subcatch[j].initBuildup != null))
                    {
                        R.free(S.Subcatch[j].initBuildup);
                        S.Subcatch[j].initBuildup = (null);
                    }
                }
                ;
                {
                    if ((S.Subcatch[j].oldQual != null))
                    {
                        R.free(S.Subcatch[j].oldQual);
                        S.Subcatch[j].oldQual = (null);
                    }
                }
                ;
                {
                    if ((S.Subcatch[j].newQual != null))
                    {
                        R.free(S.Subcatch[j].newQual);
                        S.Subcatch[j].newQual = (null);
                    }
                }
                ;
                {
                    if ((S.Subcatch[j].pondedQual != null))
                    {
                        R.free(S.Subcatch[j].pondedQual);
                        S.Subcatch[j].pondedQual = (null);
                    }
                }
                ;
                {
                    if ((S.Subcatch[j].totalLoad != null))
                    {
                        R.free(S.Subcatch[j].totalLoad);
                        S.Subcatch[j].totalLoad = (null);
                    }
                }
                ;
            }
        }
        if ((S.Node != null))
        {
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                {
                    if ((S.Node[j].oldQual != null))
                    {
                        R.free(S.Node[j].oldQual);
                        S.Node[j].oldQual = (null);
                    }
                }
                ;
                {
                    if ((S.Node[j].newQual != null))
                    {
                        R.free(S.Node[j].newQual);
                        S.Node[j].newQual = (null);
                    }
                }
                ;
            }
        }
        if ((S.Link != null))
        {
            for (j = 0; j < S.Nobjects[S.LINK]; j++)
            {
                {
                    if ((S.Link[j].oldQual != null))
                    {
                        R.free(S.Link[j].oldQual);
                        S.Link[j].oldQual = (null);
                    }
                }
                ;
                {
                    if ((S.Link[j].newQual != null))
                    {
                        R.free(S.Link[j].newQual);
                        S.Link[j].newQual = (null);
                    }
                }
                ;
                {
                    if ((S.Link[j].totalLoad != null))
                    {
                        R.free(S.Link[j].totalLoad);
                        S.Link[j].totalLoad = (null);
                    }
                }
                ;
            }
        }
        S.infil_delete();
        if ((S.Node != null))
        {
            for (j = 0; j < S.Nnodes[S.STORAGE]; j++)
            {
                if ((S.Storage[j].exfil != null))
                {
                    {
                        if ((S.Storage[j].exfil.btmExfil != null))
                        {
                            R.free(S.Storage[j].exfil.btmExfil);
                            S.Storage[j].exfil.btmExfil = (null);
                        }
                    }
                    ;
                    {
                        if ((S.Storage[j].exfil.bankExfil != null))
                        {
                            R.free(S.Storage[j].exfil.bankExfil);
                            S.Storage[j].exfil.bankExfil = (null);
                        }
                    }
                    ;
                    {
                        if ((S.Storage[j].exfil != null))
                        {
                            R.free(S.Storage[j].exfil);
                            S.Storage[j].exfil = (null);
                        }
                    }
                    ;
                }
            }
        }
        if ((S.Node != null))
        {
            for (j = 0; j < S.Nnodes[S.OUTFALL]; j++)
            {
                if ((S.Outfall[j].wRouted != null))
                {
                    R.free(S.Outfall[j].wRouted);
                    S.Outfall[j].wRouted = (null);
                }
            }
        }
        ;
        if ((S.Node != null))
        {
            for (j = 0; j < S.Nobjects[S.NODE]; j++)
            {
                S.inflow_deleteExtInflows(j);
                S.inflow_deleteDwfInflows(j);
                S.rdii_deleteRdiiInflow(j);
                S.treatmnt_delete(j);
            }
        }
        if ((S.Tseries != null))
        {
            for (j = 0; j < S.Nobjects[S.TSERIES]; j++)
            {
                S.table_deleteEntries(S.Tseries[j]);
            }
        }
        if ((S.Curve != null))
        {
            for (j = 0; j < S.Nobjects[S.CURVE]; j++)
            {
                S.table_deleteEntries(S.Curve[j]);
            }
        }
        S.transect_delete();
        S.street_delete();
        S.inlet_delete();
        S.controls_delete();
        S.lid_delete();
        {
            if ((S.Gage != null))
            {
                R.free(S.Gage);
                S.Gage = (null);
            }
        }
        ;
        {
            if ((S.Subcatch != null))
            {
                R.free(S.Subcatch);
                S.Subcatch = (null);
            }
        }
        ;
        {
            if ((S.Node != null))
            {
                R.free(S.Node);
                S.Node = (null);
            }
        }
        ;
        {
            if ((S.Outfall != null))
            {
                R.free(S.Outfall);
                S.Outfall = (null);
            }
        }
        ;
        {
            if ((S.Divider != null))
            {
                R.free(S.Divider);
                S.Divider = (null);
            }
        }
        ;
        {
            if ((S.Storage != null))
            {
                R.free(S.Storage);
                S.Storage = (null);
            }
        }
        ;
        {
            if ((S.Link != null))
            {
                R.free(S.Link);
                S.Link = (null);
            }
        }
        ;
        {
            if ((S.Conduit != null))
            {
                R.free(S.Conduit);
                S.Conduit = (null);
            }
        }
        ;
        {
            if ((S.Pump != null))
            {
                R.free(S.Pump);
                S.Pump = (null);
            }
        }
        ;
        {
            if ((S.Orifice != null))
            {
                R.free(S.Orifice);
                S.Orifice = (null);
            }
        }
        ;
        {
            if ((S.Weir != null))
            {
                R.free(S.Weir);
                S.Weir = (null);
            }
        }
        ;
        {
            if ((S.Outlet != null))
            {
                R.free(S.Outlet);
                S.Outlet = (null);
            }
        }
        ;
        {
            if ((S.Pollut != null))
            {
                R.free(S.Pollut);
                S.Pollut = (null);
            }
        }
        ;
        {
            if ((S.Landuse != null))
            {
                R.free(S.Landuse);
                S.Landuse = (null);
            }
        }
        ;
        {
            if ((S.Pattern != null))
            {
                R.free(S.Pattern);
                S.Pattern = (null);
            }
        }
        ;
        {
            if ((S.Curve != null))
            {
                R.free(S.Curve);
                S.Curve = (null);
            }
        }
        ;
        {
            if ((S.Tseries != null))
            {
                R.free(S.Tseries);
                S.Tseries = (null);
            }
        }
        ;
        {
            if ((S.Aquifer != null))
            {
                R.free(S.Aquifer);
                S.Aquifer = (null);
            }
        }
        ;
        {
            if ((S.UnitHyd != null))
            {
                R.free(S.UnitHyd);
                S.UnitHyd = (null);
            }
        }
        ;
        {
            if ((S.Snowmelt != null))
            {
                R.free(S.Snowmelt);
                S.Snowmelt = (null);
            }
        }
        ;
        {
            if ((S.Shape != null))
            {
                R.free(S.Shape);
                S.Shape = (null);
            }
        }
        ;
        {
            if ((S.Event != null))
            {
                R.free(S.Event);
                S.Event = (null);
            }
        }
        ;
    }

    // project.c:1304
    public static void createHashTables()
    {
        int j = 0;
        S.MemPoolAllocated = 0;
        for (j = 0; j < S.MAX_OBJ_TYPES; j++)
        {
            S.Htable[j] = S.HTcreate();
            if (S.Htable[j] == null)
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
            }
        }
        if (S.AllocInit() == null)
        {
            S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
        }
        else
        {
            S.MemPoolAllocated = 1;
        }
    }

    // project.c:1325
    public static void deleteHashTables()
    {
        int j = 0;
        for (j = 0; j < S.MAX_OBJ_TYPES; j++)
        {
            if (S.Htable[j] != null)
            {
                S.HTfree(S.Htable[j]);
            }
        }
        if ((S.MemPoolAllocated != 0))
        {
            S.AllocFreePool();
        }
    }

}
}
