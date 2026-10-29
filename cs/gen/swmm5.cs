// Generated from EPA SWMM 5.2.4 (swmm5.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static TFile Finp = new TFile();
    public static TFile Fout = new TFile();
    public static TFile Frpt = new TFile();
    public static TFile Fclimate = new TFile();
    public static TFile Frain = new TFile();
    public static TFile Frunoff = new TFile();
    public static TFile Frdii = new TFile();
    public static TFile Fhotstart1 = new TFile();
    public static TFile Fhotstart2 = new TFile();
    public static TFile Finflows = new TFile();
    public static TFile Foutflows = new TFile();
    public static long Nperiods = 0L;
    public static long TotalStepCount = 0L;
    public static long ReportStepCount = 0L;
    public static long NonConvergeCount = 0L;
    public static CPtr Msg = R.Buf(1025);
    public static CPtr ErrorMsg = R.Buf(1025);
    public static CPtr[] Title = R.NewBufs(3, 1025);
    public static CPtr TempDir = R.Buf(260);
    public static CPtr InpDir = R.Buf(260);
    public static TRptFlags RptFlags = new TRptFlags();
    public static int[] Nobjects = new int[18];
    public static int[] Nnodes = new int[5];
    public static int[] Nlinks = new int[5];
    public static int UnitSystem = 0;
    public static int FlowUnits = 0;
    public static int InfilModel = 0;
    public static int RouteModel = 0;
    public static int ForceMainEqn = 0;
    public static int LinkOffsets = 0;
    public static int SurchargeMethod = 0;
    public static int AllowPonding = 0;
    public static int InertDamping = 0;
    public static int NormalFlowLtd = 0;
    public static int SlopeWeighting = 0;
    public static int Compatibility = 0;
    public static int SkipSteadyState = 0;
    public static int IgnoreRainfall = 0;
    public static int IgnoreRDII = 0;
    public static int IgnoreSnowmelt = 0;
    public static int IgnoreGwater = 0;
    public static int IgnoreRouting = 0;
    public static int IgnoreQuality = 0;
    public static int ErrorCode = 0;
    public static int Warnings = 0;
    public static int WetStep = 0;
    public static int DryStep = 0;
    public static int ReportStep = 0;
    public static int RuleStep = 0;
    public static int SweepStart = 0;
    public static int SweepEnd = 0;
    public static int MaxTrials = 0;
    public static int NumThreads = 0;
    public static int NumEvents = 0;
    public static double RouteStep = 0.0;
    public static double MinRouteStep = 0.0;
    public static double LengtheningStep = 0.0;
    public static double StartDryDays = 0.0;
    public static double CourantFactor = 0.0;
    public static double MinSurfArea = 0.0;
    public static double MinSlope = 0.0;
    public static double RunoffError = 0.0;
    public static double GwaterError = 0.0;
    public static double FlowError = 0.0;
    public static double QualError = 0.0;
    public static double HeadTol = 0.0;
    public static double SysFlowTol = 0.0;
    public static double LatFlowTol = 0.0;
    public static double CrownCutoff = 0.0;
    public static double StartDate = 0.0;
    public static double StartTime = 0.0;
    public static double StartDateTime = 0.0;
    public static double EndDate = 0.0;
    public static double EndTime = 0.0;
    public static double EndDateTime = 0.0;
    public static double ReportStartDate = 0.0;
    public static double ReportStartTime = 0.0;
    public static double ReportStart = 0.0;
    public static double ReportTime = 0.0;
    public static double OldRunoffTime = 0.0;
    public static double NewRunoffTime = 0.0;
    public static double OldRoutingTime = 0.0;
    public static double NewRoutingTime = 0.0;
    public static double TotalDuration = 0.0;
    public static double ElapsedTime = 0.0;
    public static TTemp Temp = new TTemp();
    public static TEvap Evap = new TEvap();
    public static TWind Wind = new TWind();
    public static TSnow Snow = new TSnow();
    public static TAdjust Adjust = new TAdjust();
    public static TSnowmelt[] Snowmelt = null;
    public static TGage[] Gage = null;
    public static TSubcatch[] Subcatch = null;
    public static TAquifer[] Aquifer = null;
    public static TUnitHyd[] UnitHyd = null;
    public static TNode[] Node = null;
    public static TOutfall[] Outfall = null;
    public static TDivider[] Divider = null;
    public static TStorage[] Storage = null;
    public static TLink[] Link = null;
    public static TConduit[] Conduit = null;
    public static TPump[] Pump = null;
    public static TOrifice[] Orifice = null;
    public static TWeir[] Weir = null;
    public static TOutlet[] Outlet = null;
    public static TPollut[] Pollut = null;
    public static TLanduse[] Landuse = null;
    public static TPattern[] Pattern = null;
    public static TTable[] Curve = null;
    public static TTable[] Tseries = null;
    public static TTransect[] Transect = null;
    public static TStreet[] Street = null;
    public static TShape[] Shape = null;
    public static TEvent[] Event = null;
    public static double[][] Ucf = new double[][] { new double[] { 43200.0, 1097280.0 }, new double[] { 12.0, 304.8 }, new double[] { 1036800.0, 26334720.0 }, new double[] { 1.0, 0.3048 }, new double[] { 2.2956e-5, 0.92903e-5 }, new double[] { 1.0, 0.02832 }, new double[] { 1.0, 1.608 }, new double[] { 1.0, 1.8 }, new double[] { 2.203e-6, 1.0e-6 }, new double[] { 43560.0, 3048.0 } };
    public static double[] Qcf = new double[] { 1.0, 448.831, 0.64632, 0.02832, 28.317, 2.4466 };
    public static int IsOpenFlag = 0;
    public static int IsStartedFlag = 0;
    public static int SaveResultsFlag = 0;
    public static int ExceptionCount = 0;
    public static int DoRunoff = 0;
    public static int DoRouting = 0;
    public static double RoutingDuration = 0.0;

    // swmm5.c:186
    public static int swmm_run(CPtr f1, CPtr f2, CPtr f3)
    {
        long newHour = 0L;
        long oldHour = 0;
        long theDay = 0L;
        long theHour = 0L;
        double elapsedTime = 0.0;
        S.IsOpenFlag = 0;
        S.IsStartedFlag = 0;
        S.SaveResultsFlag = 1;
        S.ErrorCode = 0;
        S.writecon(S.L642);
        S.swmm_open(f1, f2, f3);
        if (!(S.ErrorCode != 0))
        {
            S.swmm_start(1);
            if (!(S.ErrorCode != 0))
            {
                S.writecon(S.L643);
                do
                {
                    S.swmm_step(ref elapsedTime);
                    newHour = R.D2L((elapsedTime * 24.0));
                    if (newHour > oldHour)
                    {
                        theDay = R.D2L(elapsedTime);
                        theHour = R.D2L(((elapsedTime - Math.Floor(elapsedTime)) * 24.0));
                        S.writecon(S.L644);
                        R.snprintf(S.Msg, unchecked((ulong)(1024)), S.L645, theDay, theHour);
                        S.writecon(S.Msg);
                        oldHour = newHour;
                    }
                }
                while ((elapsedTime > 0.0) && (!(S.ErrorCode != 0)));
                S.writecon(S.L646);
                S.writecon(S.L647);
            }
            S.swmm_end();
        }
        if ((!(S.ErrorCode != 0)) && (S.Fout.mode == S.SCRATCH_FILE))
        {
            S.writecon(S.L648);
            S.swmm_report();
        }
        S.swmm_close();
        return S.ErrorCode;
    }

    // swmm5.c:256
    public static int swmm_open(CPtr f1, CPtr f2, CPtr f3)
    {
        {
            S.datetime_setDateFormat(1);
            S.ErrorCode = 0;
            S.ErrorMsg[0] = 0;
            S.Warnings = 0;
            S.IsOpenFlag = 0;
            S.IsStartedFlag = 0;
            S.ExceptionCount = 0;
            R.strcpy(S.InpDir, S.L0);
            S.project_open(f1, f2, f3);
            S.getAbsolutePath(f1, S.InpDir, 260);
            if ((S.ErrorCode != 0))
            {
                return S.ErrorCode;
            }
            S.IsOpenFlag = 1;
            S.report_writeLogo();
            S.project_readInput();
            if ((S.ErrorCode != 0))
            {
                return S.ErrorCode;
            }
            S.report_writeTitle();
            S.project_validate();
        }
        return S.ErrorCode;
    }

    // swmm5.c:314
    public static int swmm_start(int saveResults)
    {
        if ((S.ErrorCode != 0))
        {
            return S.ErrorCode;
        }
        if (!(S.IsOpenFlag != 0))
        {
            return (S.ErrorCode = S.ERR_API_NOT_OPEN);
        }
        if ((S.IsStartedFlag != 0))
        {
            return (S.ErrorCode = S.ERR_API_NOT_ENDED);
        }
        if (!(S.RptFlags.disabled != 0))
        {
            if ((S.RptFlags.input != 0))
            {
                S.inputrpt_writeInput();
            }
            S.report_writeOptions();
        }
        S.SaveResultsFlag = saveResults;
        S.ExceptionCount = 0;
        {
            S.ElapsedTime = 0.0;
            S.RoutingDuration = S.TotalDuration;
            S.NewRunoffTime = 0.0;
            S.NewRoutingTime = 0.0;
            S.ReportTime = 1000 * (double)(S.ReportStep);
            S.TotalStepCount = 0;
            S.ReportStepCount = 0;
            S.NonConvergeCount = 0;
            S.IsStartedFlag = 1;
            S.RunoffError = 0.0;
            S.GwaterError = 0.0;
            S.FlowError = 0.0;
            S.QualError = 0.0;
            if (!(S.IgnoreRainfall != 0))
            {
                S.rain_open();
            }
            if ((S.ErrorCode != 0))
            {
                return S.ErrorCode;
            }
            S.project_init();
            if (S.Nobjects[S.SUBCATCH] > 0)
            {
                S.DoRunoff = 1;
            }
            else
            {
                S.DoRunoff = 0;
            }
            if ((S.Nobjects[S.NODE] > 0) && (!(S.IgnoreRouting != 0)))
            {
                S.DoRouting = 1;
            }
            else
            {
                S.DoRouting = 0;
            }
            S.output_open();
            if ((S.DoRunoff != 0))
            {
                S.runoff_open();
            }
            if (!((S.hotstart_open() != 0)))
            {
                return S.ErrorCode;
            }
            if ((S.DoRouting != 0))
            {
                S.routing_open();
            }
            S.massbal_open();
            S.stats_open();
            if ((!(S.RptFlags.disabled != 0)) && (S.RptFlags.controls != 0))
            {
                S.report_writeControlActionsHeading();
            }
        }
        return S.ErrorCode;
    }

    // swmm5.c:410
    public static int swmm_step(ref double elapsedTime)
    {
        elapsedTime = 0.0;
        if ((S.ErrorCode != 0))
        {
            return S.ErrorCode;
        }
        if (!(S.IsOpenFlag != 0))
        {
            return (S.ErrorCode = S.ERR_API_NOT_OPEN);
        }
        if (!(S.IsStartedFlag != 0))
        {
            return (S.ErrorCode = S.ERR_API_NOT_STARTED);
        }
        {
            if (S.NewRoutingTime < S.RoutingDuration)
            {
                S.execRouting();
            }
            if ((S.SaveResultsFlag != 0))
            {
                S.saveResults();
            }
            if (S.NewRoutingTime < S.RoutingDuration)
            {
                S.ElapsedTime = S.NewRoutingTime / 86400000.0;
            }
            else
            {
                S.ElapsedTime = 0.0;
            }
            elapsedTime = S.ElapsedTime;
        }
        return S.ErrorCode;
    }

    // swmm5.c:466
    public static int swmm_stride(int strideStep, ref double elapsedTime)
    {
        double realRouteStep = S.RouteStep;
        elapsedTime = 0.0;
        if ((S.ErrorCode != 0))
        {
            return S.ErrorCode;
        }
        if (!(S.IsOpenFlag != 0))
        {
            return (S.ErrorCode = S.ERR_API_NOT_OPEN);
        }
        if (!(S.IsStartedFlag != 0))
        {
            return (S.ErrorCode = S.ERR_API_NOT_STARTED);
        }
        S.RoutingDuration = S.NewRoutingTime + 1000.0 * strideStep;
        S.RoutingDuration = ((((S.TotalDuration) <= (S.RoutingDuration)) ? (S.TotalDuration) : (S.RoutingDuration)));
        if (strideStep < S.RouteStep)
        {
            S.RouteStep = strideStep;
        }
        do
        {
            S.swmm_step(ref elapsedTime);
        }
        while ((elapsedTime > 0.0) && (!(S.ErrorCode != 0)));
        S.RouteStep = realRouteStep;
        S.RoutingDuration = S.TotalDuration;
        if (S.NewRoutingTime < S.TotalDuration)
        {
            S.ElapsedTime = S.NewRoutingTime / 86400000.0;
        }
        else
        {
            S.ElapsedTime = 0.0;
        }
        elapsedTime = S.ElapsedTime;
        return S.ErrorCode;
    }

    // swmm5.c:514
    public static void execRouting()
    {
        double nextRoutingTime = 0.0;
        double routingStep = 0.0;
        {
            S.TotalStepCount++;
            if (!(S.DoRouting != 0))
            {
                routingStep = ((((S.WetStep) <= (S.ReportStep)) ? (S.WetStep) : (S.ReportStep)));
            }
            else
            {
                routingStep = S.routing_getRoutingStep(S.RouteModel, S.RouteStep);
            }
            if (routingStep <= 0.0)
            {
                S.ErrorCode = S.ERR_TIMESTEP;
                return;
            }
            nextRoutingTime = S.NewRoutingTime + 1000.0 * routingStep;
            if (nextRoutingTime > S.RoutingDuration)
            {
                routingStep = (S.RoutingDuration - S.NewRoutingTime) / 1000.0;
                routingStep = ((((routingStep) >= (1.0 / 1000.0)) ? (routingStep) : (1.0 / 1000.0)));
                nextRoutingTime = S.RoutingDuration;
            }
            if ((S.DoRunoff != 0))
            {
                while (S.NewRunoffTime < nextRoutingTime)
                {
                    S.runoff_execute();
                    if ((S.ErrorCode != 0))
                    {
                        return;
                    }
                }
            }
            else
            {
                S.climate_setState(S.getDateTime(S.NewRoutingTime));
            }
            if ((S.DoRouting != 0))
            {
                S.routing_execute(S.RouteModel, routingStep);
            }
            else
            {
                S.NewRoutingTime = nextRoutingTime;
            }
        }
    }

    // swmm5.c:579
    public static void saveResults()
    {
        if (S.NewRoutingTime >= S.ReportTime)
        {
            if ((S.RptFlags.averages != 0))
            {
                if (S.NewRoutingTime == S.ReportTime)
                {
                    S.output_updateAvgResults();
                }
                S.output_saveResults(S.ReportTime);
                if (S.NewRoutingTime > S.ReportTime)
                {
                    S.output_updateAvgResults();
                }
            }
            else
            {
                S.output_saveResults(S.ReportTime);
            }
            S.ReportTime = S.ReportTime + 1000 * (double)(S.ReportStep);
        }
        else if ((S.RptFlags.averages != 0))
        {
            S.output_updateAvgResults();
        }
    }

    // swmm5.c:618
    public static int swmm_end()
    {
        if (!(S.IsOpenFlag != 0))
        {
            return (S.ErrorCode = S.ERR_API_NOT_OPEN);
        }
        if ((S.IsStartedFlag != 0))
        {
            if ((S.Fout.file != null))
            {
                S.output_end();
            }
            if ((!(S.ErrorCode != 0)) && (S.RptFlags.disabled == 0))
            {
                S.massbal_report();
                S.stats_report();
            }
            S.stats_close();
            S.massbal_close();
            if (!(S.IgnoreRainfall != 0))
            {
                S.rain_close();
            }
            if ((S.DoRunoff != 0))
            {
                S.runoff_close();
            }
            if ((S.DoRouting != 0))
            {
                S.routing_close(S.RouteModel);
            }
            S.hotstart_close();
            S.IsStartedFlag = 0;
        }
        return S.ErrorCode;
    }

    // swmm5.c:655
    public static int swmm_report()
    {
        if (!(S.ErrorCode != 0))
        {
            S.report_writeReport();
        }
        return S.ErrorCode;
    }

    // swmm5.c:669
    public static void swmm_writeLine(CPtr line)
    {
        if ((S.IsOpenFlag != 0))
        {
            S.report_writeLine(line);
        }
    }

    // swmm5.c:682
    public static int swmm_close()
    {
        if ((S.Fout.file != null))
        {
            S.output_close();
        }
        if ((S.IsOpenFlag != 0))
        {
            S.project_close();
        }
        S.report_writeSysTime();
        if (S.Finp.file != null)
        {
            R.fclose(S.Finp.file);
        }
        if (S.Frpt.file != null)
        {
            R.fclose(S.Frpt.file);
        }
        if (S.Fout.file != null)
        {
            R.fclose(S.Fout.file);
            if (S.Fout.mode == S.SCRATCH_FILE)
            {
                R.remove(S.Fout.name);
            }
        }
        S.IsOpenFlag = 0;
        S.IsStartedFlag = 0;
        return 0;
    }

    // swmm5.c:708
    public static int swmm_getMassBalErr(ref float runoffErr, ref float flowErr, ref float qualErr)
    {
        runoffErr = (float)(0.0);
        flowErr = (float)(0.0);
        qualErr = (float)(0.0);
        if ((S.IsOpenFlag != 0) && (!(S.IsStartedFlag != 0)))
        {
            runoffErr = (float)(S.RunoffError);
            flowErr = (float)(S.FlowError);
            qualErr = (float)(S.QualError);
        }
        return 0;
    }

    // swmm5.c:734
    public static int swmm_getVersion()
    {
        return 52004;
    }

    // swmm5.c:749
    public static int swmm_getWarnings()
    {
        return S.Warnings;
    }

    // swmm5.c:760
    public static int swmm_getError(CPtr errMsg, int msgLen)
    {
        if ((S.ErrorCode > 0) && (R.strlen(S.ErrorMsg) == unchecked((ulong)(0))))
        {
            S.error_getMsg(S.ErrorCode, S.ErrorMsg);
        }
        S.sstrncpy(errMsg, S.ErrorMsg, unchecked((ulong)(msgLen)));
        if ((msgLen > 0) && (errMsg[0] == 10))
        {
            errMsg[0] = ' ';
        }
        return S.ErrorCode;
    }

    // swmm5.c:780
    public static int swmm_getCount(int objType)
    {
        if (!(S.IsOpenFlag != 0))
        {
            return 0;
        }
        if ((objType < S.swmm_GAGE) || (objType > S.swmm_LINK))
        {
            return 0;
        }
        return S.Nobjects[objType];
    }

    // swmm5.c:795
    public static void swmm_getName(int objType, int index, CPtr name, int size)
    {
        CPtr idName = CPtr.Null;
        name[0] = 0;
        if (!(S.IsOpenFlag != 0))
        {
            return;
        }
        if ((objType < S.swmm_GAGE) || (objType > S.swmm_LINK))
        {
            return;
        }
        if ((index < 0) || (index >= S.Nobjects[objType]))
        {
            return;
        }
        switch (objType)
        {
            case S.GAGE:
                idName = S.Gage[index].ID;
                break;
            case S.SUBCATCH:
                idName = S.Subcatch[index].ID;
                break;
            case S.NODE:
                idName = S.Node[index].ID;
                break;
            case S.LINK:
                idName = S.Link[index].ID;
                break;
        }
        if (!(idName).IsNull)
        {
            S.sstrncpy(name, idName, unchecked((ulong)(size)));
        }
    }

    // swmm5.c:826
    public static int swmm_getIndex(int objType, CPtr name)
    {
        if (!(S.IsOpenFlag != 0))
        {
            return -1;
        }
        if ((objType < S.swmm_GAGE) || (objType > S.swmm_LINK))
        {
            return -1;
        }
        return S.project_findObject(objType, name);
    }

    // swmm5.c:842
    public static double swmm_getValue(int property, int index)
    {
        if (!(S.IsOpenFlag != 0))
        {
            return 0;
        }
        if (property < 100)
        {
            return S.getSystemValue(property);
        }
        if (property < 200)
        {
            return S.getGageValue(property, index);
        }
        if (property < 300)
        {
            return S.getSubcatchValue(property, index);
        }
        if (property < 400)
        {
            return S.getNodeValue(property, index);
        }
        if (property < 500)
        {
            return S.getLinkValue(property, index);
        }
        return 0;
    }

    // swmm5.c:867
    public static void swmm_setValue(int property, int index, double value)
    {
        if (!(S.IsOpenFlag != 0))
        {
            return;
        }
        switch (property)
        {
            case S.swmm_GAGE_RAINFALL:
                if ((index < 0) || (index >= S.Nobjects[S.GAGE]))
                {
                    return;
                }
                if (value >= 0.0)
                {
                    S.Gage[index].apiRainfall = value;
                }
                return;
            case S.swmm_SUBCATCH_RPTFLAG:
                if (((!(S.IsStartedFlag != 0)) && (index >= 0)) && (index < S.Nobjects[S.SUBCATCH]))
                {
                    S.Subcatch[index].rptFlag = ((value > 0.0) ? 1 : 0);
                }
                return;
            case S.swmm_NODE_LATFLOW:
                S.setNodeLatFlow(index, value);
                return;
            case S.swmm_NODE_HEAD:
                S.setOutfallStage(index, value);
                return;
            case S.swmm_NODE_RPTFLAG:
                if (((!(S.IsStartedFlag != 0)) && (index >= 0)) && (index < S.Nobjects[S.NODE]))
                {
                    S.Node[index].rptFlag = ((value > 0.0) ? 1 : 0);
                }
                return;
            case S.swmm_LINK_SETTING:
                S.setLinkSetting(index, value);
                return;
            case S.swmm_LINK_RPTFLAG:
                if (((!(S.IsStartedFlag != 0)) && (index >= 0)) && (index < S.Nobjects[S.LINK]))
                {
                    S.Link[index].rptFlag = ((value > 0.0) ? 1 : 0);
                }
                return;
            case S.swmm_ROUTESTEP:
                S.setRoutingStep(value);
                return;
            case S.swmm_REPORTSTEP:
                if ((!(S.IsStartedFlag != 0)) && (value > 0))
                {
                    S.ReportStep = R.D2I(value);
                }
                return;
            case S.swmm_NOREPORT:
                if (!(S.IsStartedFlag != 0))
                {
                    S.RptFlags.disabled = ((value > 0.0) ? 1 : 0);
                }
                return;
        }
    }

    // swmm5.c:922
    public static double swmm_getSavedValue(int property, int index, int period)
    {
        if (!(S.IsOpenFlag != 0))
        {
            return 0;
        }
        if ((S.IsStartedFlag != 0))
        {
            return 0;
        }
        if ((period < 1) || (period > S.Nperiods))
        {
            return 0;
        }
        if (property == S.swmm_CURRENTDATE)
        {
            return S.getSavedDate(period);
        }
        if ((property >= 200) && (property < 300))
        {
            return S.getSavedSubcatchValue(property, index, period);
        }
        if (property < 400)
        {
            return S.getSavedNodeValue(property, index, period);
        }
        if (property < 500)
        {
            return S.getSavedLinkValue(property, index, period);
        }
        return 0;
    }

    // swmm5.c:949
    public static void swmm_decodeDate(double date, ref int year, ref int month, ref int day, ref int hour, ref int minute, ref int second, ref int dayOfWeek)
    {
        S.datetime_decodeDate(date, ref year, ref month, ref day);
        S.datetime_decodeTime(date, ref hour, ref minute, ref second);
        dayOfWeek = S.datetime_dayOfWeek(date);
    }

    // swmm5.c:966
    public static double getGageValue(int property, int index)
    {
        if ((index < 0) || (index >= S.Nobjects[S.GAGE]))
        {
            return 0;
        }
        if (property == S.swmm_GAGE_RAINFALL)
        {
            return S.Gage[index].reportRainfall;
        }
        return 0;
    }

    // swmm5.c:982
    public static double getSubcatchValue(int property, int index)
    {
        TSubcatch subcatch = null;
        if ((index < 0) || (index >= S.Nobjects[S.SUBCATCH]))
        {
            return 0;
        }
        subcatch = S.Subcatch[index];
        switch (property)
        {
            case S.swmm_SUBCATCH_AREA:
                return subcatch.area * S.UCF(S.LANDAREA);
            case S.swmm_SUBCATCH_RAINGAGE:
                return subcatch.gage;
            case S.swmm_SUBCATCH_RAINFALL:
                if (subcatch.gage >= 0)
                {
                    return S.Gage[subcatch.gage].reportRainfall;
                }
                else
                {
                    return 0.0;
                }
            case S.swmm_SUBCATCH_EVAP:
                return subcatch.evapLoss * S.UCF(S.EVAPRATE);
            case S.swmm_SUBCATCH_INFIL:
                return subcatch.infilLoss * S.UCF(S.RAINFALL);
            case S.swmm_SUBCATCH_RUNOFF:
                return subcatch.newRunoff * S.UCF(S.FLOW);
            case S.swmm_SUBCATCH_RPTFLAG:
                return ((subcatch.rptFlag > 0) ? 1 : 0);
            default:
                return 0;
        }
    }

    // swmm5.c:1019
    public static double getNodeValue(int property, int index)
    {
        TNode node = null;
        if ((index < 0) || (index >= S.Nobjects[S.NODE]))
        {
            return 0;
        }
        node = S.Node[index];
        switch (property)
        {
            case S.swmm_NODE_TYPE:
                return node.type;
            case S.swmm_NODE_ELEV:
                return node.invertElev * S.UCF(S.LENGTH);
            case S.swmm_NODE_MAXDEPTH:
                return node.fullDepth * S.UCF(S.LENGTH);
            case S.swmm_NODE_DEPTH:
                return node.newDepth * S.UCF(S.LENGTH);
            case S.swmm_NODE_HEAD:
                return (node.newDepth + node.invertElev) * S.UCF(S.LENGTH);
            case S.swmm_NODE_VOLUME:
                return node.newVolume * S.UCF(S.VOLUME);
            case S.swmm_NODE_LATFLOW:
                return node.newLatFlow * S.UCF(S.FLOW);
            case S.swmm_NODE_INFLOW:
                return node.inflow * S.UCF(S.FLOW);
            case S.swmm_NODE_OVERFLOW:
                return node.overflow * S.UCF(S.FLOW);
            case S.swmm_NODE_RPTFLAG:
                return ((node.rptFlag > 0) ? 1 : 0);
            default:
                return 0;
        }
    }

    // swmm5.c:1059
    public static double getLinkValue(int property, int index)
    {
        TLink link = null;
        if ((index < 0) || (index >= S.Nobjects[S.LINK]))
        {
            return 0;
        }
        link = S.Link[index];
        switch (property)
        {
            case S.swmm_LINK_TYPE:
                return link.type;
            case S.swmm_LINK_NODE1:
                return link.node1;
            case S.swmm_LINK_NODE2:
                return link.node2;
            case S.swmm_LINK_LENGTH:
                if (link.type == S.CONDUIT)
                {
                    return S.Conduit[link.subIndex].length * S.UCF(S.LENGTH);
                }
                else
                {
                    return 0;
                }
            case S.swmm_LINK_SLOPE:
                if (link.type == S.CONDUIT)
                {
                    return S.Conduit[link.subIndex].slope;
                }
                else
                {
                    return 0;
                }
                break;
            case S.swmm_LINK_FULLDEPTH:
                return link.xsect.yFull * S.UCF(S.LENGTH);
            case S.swmm_LINK_FULLFLOW:
                return link.qFull * S.UCF(S.FLOW);
            case S.swmm_LINK_FLOW:
                return link.newFlow * S.UCF(S.FLOW) * (double)(link.direction);
            case S.swmm_LINK_VELOCITY:
                return S.link_getVelocity(index, Math.Abs(link.newFlow), link.newDepth) * S.UCF(S.LENGTH);
            case S.swmm_LINK_DEPTH:
                return link.newDepth * S.UCF(S.LENGTH);
            case S.swmm_LINK_TOPWIDTH:
                if (link.type == S.CONDUIT)
                {
                    return S.xsect_getWofY(link.xsect, link.newDepth) * S.UCF(S.LENGTH);
                }
                else
                {
                    return 0;
                }
            case S.swmm_LINK_SETTING:
                return link.setting;
            case S.swmm_LINK_TIMEOPEN:
                if (link.setting > 0.0)
                {
                    return (S.getDateTime(S.NewRoutingTime) - link.timeLastSet) * 24.0;
                }
                else
                {
                    return 0;
                }
            case S.swmm_LINK_TIMECLOSED:
                if (link.setting == 0.0)
                {
                    return (S.getDateTime(S.NewRoutingTime) - link.timeLastSet) * 24.0;
                }
                else
                {
                    return 0;
                }
            case S.swmm_LINK_RPTFLAG:
                return ((link.rptFlag > 0) ? 1 : 0);
            default:
                return 0;
        }
    }

    // swmm5.c:1126
    public static double getSystemValue(int property)
    {
        switch (property)
        {
            case S.swmm_STARTDATE:
                return S.StartDateTime;
            case S.swmm_CURRENTDATE:
                return S.StartDateTime + S.ElapsedTime;
            case S.swmm_ELAPSEDTIME:
                return S.ElapsedTime;
            case S.swmm_ROUTESTEP:
                return S.RouteStep;
            case S.swmm_MAXROUTESTEP:
                return S.getMaxRouteStep();
            case S.swmm_REPORTSTEP:
                return S.ReportStep;
            case S.swmm_TOTALSTEPS:
                return S.Nperiods;
            case S.swmm_NOREPORT:
                return S.RptFlags.disabled;
            case S.swmm_FLOWUNITS:
                return S.FlowUnits;
            default:
                return 0;
        }
    }

    // swmm5.c:1159
    public static void setNodeLatFlow(int index, double value)
    {
        if ((index < 0) || (index >= S.Nobjects[S.NODE]))
        {
            return;
        }
        S.Node[index].apiExtInflow = value / S.UCF(S.FLOW);
    }

    // swmm5.c:1173
    public static void setOutfallStage(int index, double value)
    {
        TNode node = null;
        if ((index < 0) || (index >= S.Nobjects[S.NODE]))
        {
            return;
        }
        node = S.Node[index];
        if (node.type != S.OUTFALL)
        {
            return;
        }
        S.Outfall[node.subIndex].fixedStage = value / S.UCF(S.LENGTH);
        S.Outfall[node.subIndex].type = S.FIXED_OUTFALL;
    }

    // swmm5.c:1192
    public static void setLinkSetting(int index, double value)
    {
        TLink link = null;
        if ((index < 0) || (index >= S.Nobjects[S.LINK]))
        {
            return;
        }
        link = S.Link[index];
        if ((value < 0.0) || (link.type == S.CONDUIT))
        {
            return;
        }
        if ((link.type != S.PUMP) && (value > 1.0))
        {
            value = 1.0;
        }
        if (link.targetSetting == value)
        {
            return;
        }
        link.targetSetting = value;
        if (link.targetSetting * link.setting == 0.0)
        {
            link.timeLastSet = S.StartDateTime + S.ElapsedTime;
        }
        S.link_setSetting(index, 0.0);
    }

    // swmm5.c:1217
    public static double getSavedDate(int period)
    {
        double days = 0.0;
        S.output_readDateTime(period, ref days);
        return days;
    }

    // swmm5.c:1230
    public static double getSavedSubcatchValue(int property, int index, int period)
    {
        int outIndex = S.Subcatch[index].rptFlag - 1;
        if (outIndex < 0)
        {
            return 0;
        }
        S.output_readSubcatchResults(period, outIndex);
        switch (property)
        {
            case S.swmm_SUBCATCH_RAINFALL:
                return S.SubcatchResults[S.SUBCATCH_RAINFALL];
            case S.swmm_SUBCATCH_EVAP:
                return S.SubcatchResults[S.SUBCATCH_EVAP];
            case S.swmm_SUBCATCH_INFIL:
                return S.SubcatchResults[S.SUBCATCH_INFIL];
            case S.swmm_SUBCATCH_RUNOFF:
                return S.SubcatchResults[S.SUBCATCH_RUNOFF];
            default:
                return 0;
        }
    }

    // swmm5.c:1265
    public static double getSavedNodeValue(int property, int index, int period)
    {
        int outIndex = S.Node[index].rptFlag - 1;
        if (outIndex < 0)
        {
            return 0;
        }
        S.output_readNodeResults(period, outIndex);
        switch (property)
        {
            case S.swmm_NODE_DEPTH:
                return S.NodeResults[S.NODE_DEPTH];
            case S.swmm_NODE_HEAD:
                return S.NodeResults[S.NODE_HEAD];
            case S.swmm_NODE_VOLUME:
                return S.NodeResults[S.NODE_VOLUME];
            case S.swmm_NODE_LATFLOW:
                return S.NodeResults[S.NODE_LATFLOW];
            case S.swmm_NODE_INFLOW:
                return S.NodeResults[S.NODE_INFLOW];
            case S.swmm_NODE_OVERFLOW:
                return S.NodeResults[S.NODE_OVERFLOW];
            default:
                return 0;
        }
    }

    // swmm5.c:1304
    public static double getSavedLinkValue(int property, int index, int period)
    {
        double y = 0.0;
        double w = 0.0;
        int outIndex = S.Link[index].rptFlag - 1;
        if (outIndex < 0)
        {
            return 0;
        }
        S.output_readLinkResults(period, outIndex);
        switch (property)
        {
            case S.swmm_LINK_FLOW:
                return S.LinkResults[S.LINK_FLOW];
            case S.swmm_LINK_DEPTH:
                return S.LinkResults[S.LINK_DEPTH];
            case S.swmm_LINK_VELOCITY:
                return S.LinkResults[S.LINK_VELOCITY];
            case S.swmm_LINK_TOPWIDTH:
                y = S.LinkResults[S.LINK_DEPTH] / S.UCF(S.LENGTH);
                w = S.xsect_getWofY(S.Link[index].xsect, y);
                return w * S.UCF(S.LENGTH);
            case S.swmm_LINK_SETTING:
                return S.LinkResults[S.LINK_CAPACITY];
            default:
                return 0;
        }
    }

    // swmm5.c:1345
    public static double getMaxRouteStep()
    {
        double tmpCourantFactor = S.CourantFactor;
        double result = S.RouteStep;
        if ((!(S.IsStartedFlag != 0)) || (S.RouteModel != S.DW))
        {
            return result;
        }
        S.CourantFactor = 1.0;
        result = S.routing_getRoutingStep(S.RouteModel, S.MinRouteStep);
        S.CourantFactor = tmpCourantFactor;
        return result;
    }

    // swmm5.c:1360
    public static void setRoutingStep(double value)
    {
        if (value <= 0.0)
        {
            return;
        }
        if (value <= S.MinRouteStep)
        {
            value = S.MinRouteStep;
        }
        S.CourantFactor = 0.0;
        S.RouteStep = value;
    }

    // swmm5.c:1378
    public static double UCF(int u)
    {
        if (u < S.FLOW)
        {
            return S.Ucf[u][S.UnitSystem];
        }
        else
        {
            return S.Qcf[S.FlowUnits];
        }
    }

    // swmm5.c:1453
    public static int strcomp(CPtr s1, CPtr s2)
    {
        int i = 0;
        for (i = 0; (((((s1[i]) >= 'a') && ((s1[i]) <= 'z')) ? ((s1[i]) & ~32) : (s1[i]))) == (((((s2[i]) >= 'a') && ((s2[i]) <= 'z')) ? ((s2[i]) & ~32) : (s2[i]))); i++)
        {
            if ((!(s1[i + 1] != 0)) && (!(s2[i + 1] != 0)))
            {
                return (1);
            }
        }
        return (0);
    }

    // swmm5.c:1518
    public static void getElapsedTime(double aDate, ref int days, ref int hrs, ref int mins)
    {
        double x = 0.0;
        int secs = 0;
        x = aDate - S.ReportStart;
        if (x <= 0.0)
        {
            days = 0;
            hrs = 0;
            mins = 0;
        }
        else
        {
            days = R.D2I(x);
            S.datetime_decodeTime(x, ref hrs, ref mins, ref secs);
        }
    }

    // swmm5.c:1543
    public static double getDateTime(double elapsedMsec)
    {
        return S.datetime_addSeconds(S.StartDateTime, (elapsedMsec + 1) / 1000.0);
    }

    // swmm5.c:1556
    public static int isRelativePath(CPtr fname)
    {
        if (!(R.strchr(fname, ':')).IsNull)
        {
            return 0;
        }
        if (fname[0] == 92)
        {
            return 0;
        }
        if (fname[0] == '/')
        {
            return 0;
        }
        return 1;
    }

    // swmm5.c:1617
    public static CPtr addAbsolutePath(CPtr fname)
    {
        ulong n = 0UL;
        CPtr buffer = R.Buf(259);
        if ((S.isRelativePath(fname) != 0))
        {
            n = unchecked((ulong)(R.snprintf(buffer, unchecked((ulong)(259)), S.L649, S.InpDir, fname)));
            if (n > unchecked((ulong)(0)))
            {
                S.sstrncpy(fname, buffer, unchecked((ulong)(259)));
            }
        }
        return fname;
    }

    // swmm5.c:1638
    public static void writecon(CPtr s)
    {
        R.fprintf(S.stdout, S.L169, s);
        R.fflush(S.stdout);
    }

}
}
