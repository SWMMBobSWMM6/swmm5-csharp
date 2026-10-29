// Generated from EPA SWMM 5.2.4 (output.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static long IDStartPos = 0L;
    public static long InputStartPos = 0L;
    public static long OutputStartPos = 0L;
    public static long BytesPerPeriod = 0L;
    public static int NumSubcatchVars = 0;
    public static int NumNodeVars = 0;
    public static int NumLinkVars = 0;
    public static int NumSubcatch = 0;
    public static int NumNodes = 0;
    public static int NumLinks = 0;
    public static int NumPolluts = 0;
    public static float[] SysResults = new float[15];
    public static TAvgResults[] AvgLinkResults = null;
    public static TAvgResults[] AvgNodeResults = null;
    public static int output_Nsteps = 0;
    public static float[] SubcatchResults = null;
    public static float[] NodeResults = null;
    public static float[] LinkResults = null;

    // output.c:121
    public static int output_open()
    {
        int j = 0;
        int m = 0;
        int k = 0;
        float x = 0.0f;
        double z = 0.0;
        long numResults = 0L;
        S.output_openOutFile();
        if ((S.ErrorCode != 0))
        {
            return S.ErrorCode;
        }
        if ((S.IgnoreQuality != 0))
        {
            S.NumPolluts = 0;
        }
        else
        {
            S.NumPolluts = S.Nobjects[S.POLLUT];
        }
        S.NumSubcatchVars = 9 - 1 + S.NumPolluts;
        S.NumNodeVars = 7 - 1 + S.NumPolluts;
        S.NumLinkVars = 6 - 1 + S.NumPolluts;
        S.NumSubcatch = 0;
        S.NumNodes = 0;
        S.NumLinks = 0;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            if ((S.Subcatch[j].rptFlag != 0))
            {
                S.NumSubcatch++;
            }
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if ((S.Node[j].rptFlag != 0))
            {
                S.NumNodes++;
            }
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if ((S.Link[j].rptFlag != 0))
            {
                S.NumLinks++;
            }
        }
        numResults = (S.NumSubcatch * S.NumSubcatchVars) + (S.NumNodes * S.NumNodeVars) + (S.NumLinks * S.NumLinkVars) + 15;
        S.BytesPerPeriod = (long)(8 + (unchecked((ulong)(numResults)) * 4));
        S.Nperiods = 0;
        S.SubcatchResults = (null);
        S.NodeResults = (null);
        S.LinkResults = (null);
        S.SubcatchResults = new float[(int)(unchecked((ulong)(S.NumSubcatchVars)))];
        S.NodeResults = new float[(int)(unchecked((ulong)(S.NumNodeVars)))];
        S.LinkResults = new float[(int)(unchecked((ulong)(S.NumLinkVars)))];
        if (((!(S.SubcatchResults != null)) || (!(S.NodeResults != null))) || (!(S.LinkResults != null)))
        {
            S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
            return S.ErrorCode;
        }
        S.AvgNodeResults = (null);
        S.AvgLinkResults = (null);
        if ((S.RptFlags.averages != 0) && (!((S.output_openAvgResults() != 0))))
        {
            S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
            return S.ErrorCode;
        }
        R.fseek(S.Fout.file, 0, 0);
        k = 516114522;
        R.fwriteI1(k, S.Fout.file);
        k = 52004;
        R.fwriteI1(k, S.Fout.file);
        k = S.FlowUnits;
        R.fwriteI1(k, S.Fout.file);
        k = S.NumSubcatch;
        R.fwriteI1(k, S.Fout.file);
        k = S.NumNodes;
        R.fwriteI1(k, S.Fout.file);
        k = S.NumLinks;
        R.fwriteI1(k, S.Fout.file);
        k = S.NumPolluts;
        R.fwriteI1(k, S.Fout.file);
        S.IDStartPos = R.ftell(S.Fout.file);
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            if ((S.Subcatch[j].rptFlag != 0))
            {
                S.output_saveID(S.Subcatch[j].ID, S.Fout.file);
            }
        }
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if ((S.Node[j].rptFlag != 0))
            {
                S.output_saveID(S.Node[j].ID, S.Fout.file);
            }
        }
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if ((S.Link[j].rptFlag != 0))
            {
                S.output_saveID(S.Link[j].ID, S.Fout.file);
            }
        }
        for (j = 0; j < S.NumPolluts; j++)
        {
            S.output_saveID(S.Pollut[j].ID, S.Fout.file);
        }
        for (j = 0; j < S.NumPolluts; j++)
        {
            k = S.Pollut[j].units;
            R.fwriteI1(k, S.Fout.file);
        }
        S.InputStartPos = R.ftell(S.Fout.file);
        k = 1;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_AREA;
        R.fwriteI1(k, S.Fout.file);
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            if (!(S.Subcatch[j].rptFlag != 0))
            {
                continue;
            }
            S.SubcatchResults[0] = (float)((S.Subcatch[j].area * S.UCF(S.LANDAREA)));
            R.fwriteF(S.SubcatchResults, 0, (int)(unchecked((ulong)(1))), S.Fout.file);
        }
        k = 3;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_TYPE_CODE;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_INVERT;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_MAX_DEPTH;
        R.fwriteI1(k, S.Fout.file);
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if (!(S.Node[j].rptFlag != 0))
            {
                continue;
            }
            k = S.Node[j].type;
            S.NodeResults[0] = (float)((S.Node[j].invertElev * S.UCF(S.LENGTH)));
            S.NodeResults[1] = (float)((S.Node[j].fullDepth * S.UCF(S.LENGTH)));
            R.fwriteI1(k, S.Fout.file);
            R.fwriteF(S.NodeResults, 0, (int)(unchecked((ulong)(2))), S.Fout.file);
        }
        k = 5;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_TYPE_CODE;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_OFFSET;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_OFFSET;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_MAX_DEPTH;
        R.fwriteI1(k, S.Fout.file);
        k = S.INPUT_LENGTH;
        R.fwriteI1(k, S.Fout.file);
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if (!(S.Link[j].rptFlag != 0))
            {
                continue;
            }
            k = S.Link[j].type;
            if (k == S.PUMP)
            {
                for (m = 0; m < 4; m++)
                {
                    S.LinkResults[m] = 0.0f;
                }
            }
            else
            {
                S.LinkResults[0] = (float)((S.Link[j].offset1 * S.UCF(S.LENGTH)));
                S.LinkResults[1] = (float)((S.Link[j].offset2 * S.UCF(S.LENGTH)));
                if (S.Link[j].direction < 0)
                {
                    x = S.LinkResults[0];
                    S.LinkResults[0] = S.LinkResults[1];
                    S.LinkResults[1] = x;
                }
                if (k == S.OUTLET)
                {
                    S.LinkResults[2] = 0.0f;
                }
                else
                {
                    S.LinkResults[2] = (float)((S.Link[j].xsect.yFull * S.UCF(S.LENGTH)));
                }
                if (k == S.CONDUIT)
                {
                    m = S.Link[j].subIndex;
                    S.LinkResults[3] = (float)((S.Conduit[m].length * S.UCF(S.LENGTH)));
                }
                else
                {
                    S.LinkResults[3] = 0.0f;
                }
            }
            R.fwriteI1(k, S.Fout.file);
            R.fwriteF(S.LinkResults, 0, (int)(unchecked((ulong)(4))), S.Fout.file);
        }
        k = S.NumSubcatchVars;
        R.fwriteI1(k, S.Fout.file);
        k = S.SUBCATCH_RAINFALL;
        R.fwriteI1(k, S.Fout.file);
        k = S.SUBCATCH_SNOWDEPTH;
        R.fwriteI1(k, S.Fout.file);
        k = S.SUBCATCH_EVAP;
        R.fwriteI1(k, S.Fout.file);
        k = S.SUBCATCH_INFIL;
        R.fwriteI1(k, S.Fout.file);
        k = S.SUBCATCH_RUNOFF;
        R.fwriteI1(k, S.Fout.file);
        k = S.SUBCATCH_GW_FLOW;
        R.fwriteI1(k, S.Fout.file);
        k = S.SUBCATCH_GW_ELEV;
        R.fwriteI1(k, S.Fout.file);
        k = S.SUBCATCH_SOIL_MOIST;
        R.fwriteI1(k, S.Fout.file);
        for (j = 0; j < S.NumPolluts; j++)
        {
            k = S.SUBCATCH_WASHOFF + j;
            R.fwriteI1(k, S.Fout.file);
        }
        k = S.NumNodeVars;
        R.fwriteI1(k, S.Fout.file);
        k = S.NODE_DEPTH;
        R.fwriteI1(k, S.Fout.file);
        k = S.NODE_HEAD;
        R.fwriteI1(k, S.Fout.file);
        k = S.NODE_VOLUME;
        R.fwriteI1(k, S.Fout.file);
        k = S.NODE_LATFLOW;
        R.fwriteI1(k, S.Fout.file);
        k = S.NODE_INFLOW;
        R.fwriteI1(k, S.Fout.file);
        k = S.NODE_OVERFLOW;
        R.fwriteI1(k, S.Fout.file);
        for (j = 0; j < S.NumPolluts; j++)
        {
            k = S.NODE_QUAL + j;
            R.fwriteI1(k, S.Fout.file);
        }
        k = S.NumLinkVars;
        R.fwriteI1(k, S.Fout.file);
        k = S.LINK_FLOW;
        R.fwriteI1(k, S.Fout.file);
        k = S.LINK_DEPTH;
        R.fwriteI1(k, S.Fout.file);
        k = S.LINK_VELOCITY;
        R.fwriteI1(k, S.Fout.file);
        k = S.LINK_VOLUME;
        R.fwriteI1(k, S.Fout.file);
        k = S.LINK_CAPACITY;
        R.fwriteI1(k, S.Fout.file);
        for (j = 0; j < S.NumPolluts; j++)
        {
            k = S.LINK_QUAL + j;
            R.fwriteI1(k, S.Fout.file);
        }
        k = 15;
        R.fwriteI1(k, S.Fout.file);
        for (k = 0; k < 15; k++)
        {
            R.fwriteI1(k, S.Fout.file);
        }
        z = (double)(S.ReportStep) / 86400.0;
        if (S.StartDateTime + z > S.ReportStart)
        {
            z = S.StartDateTime;
        }
        else
        {
            z = Math.Floor((S.ReportStart - S.StartDateTime) / z) - 1.0;
            z = S.StartDateTime + z * (double)(S.ReportStep) / 86400.0;
        }
        R.fwriteD1(z, S.Fout.file);
        k = S.ReportStep;
        if (R.fwriteI1(k, S.Fout.file) < unchecked((ulong)(1)))
        {
            S.report_writeErrorMsg(S.ERR_OUT_WRITE, S.L0);
            return S.ErrorCode;
        }
        S.OutputStartPos = R.ftell(S.Fout.file);
        return S.ErrorCode;
    }

    // output.c:427
    public static void output_openOutFile()
    {
        if (S.Fout.file != null)
        {
            R.fclose(S.Fout.file);
        }
        else if (R.strlen(S.Fout.name) != unchecked((ulong)(0)))
        {
            S.Fout.mode = S.SAVE_FILE;
        }
        else
        {
            S.Fout.mode = S.SCRATCH_FILE;
            S.getTempFileName(S.Fout.name);
        }
        if ((S.Fout.file = R.fopen(S.Fout.name, S.L166)) == null)
        {
            S.writecon(S.L318);
            S.ErrorCode = S.ERR_OUT_FILE;
        }
    }

    // output.c:457
    public static void output_saveResults(double reportTime)
    {
        int i = 0;
        double reportDate = S.getDateTime(reportTime);
        double date = 0.0;
        if (reportDate < S.ReportStart)
        {
            return;
        }
        for (i = 0; i < 15; i++)
        {
            S.SysResults[i] = 0.0f;
        }
        date = reportDate;
        R.fwriteD1(date, S.Fout.file);
        if (S.Nobjects[S.SUBCATCH] > 0)
        {
            S.output_saveSubcatchResults(reportTime, S.Fout.file);
        }
        if ((S.RptFlags.averages != 0))
        {
            S.output_saveAvgResults(S.Fout.file);
        }
        else
        {
            if (S.Nobjects[S.NODE] > 0)
            {
                S.output_saveNodeResults(reportTime, S.Fout.file);
            }
            if (S.Nobjects[S.LINK] > 0)
            {
                S.output_saveLinkResults(reportTime, S.Fout.file);
            }
        }
        S.SysResults[S.SYS_FLOODING] = (float)((S.StepFlowTotals.flooding * S.UCF(S.FLOW)));
        S.SysResults[S.SYS_OUTFLOW] = (float)((S.StepFlowTotals.outflow * S.UCF(S.FLOW)));
        S.SysResults[S.SYS_DWFLOW] = (float)((S.StepFlowTotals.dwInflow * S.UCF(S.FLOW)));
        S.SysResults[S.SYS_GWFLOW] = (float)((S.StepFlowTotals.gwInflow * S.UCF(S.FLOW)));
        S.SysResults[S.SYS_IIFLOW] = (float)((S.StepFlowTotals.iiInflow * S.UCF(S.FLOW)));
        S.SysResults[S.SYS_EXFLOW] = (float)((S.StepFlowTotals.exInflow * S.UCF(S.FLOW)));
        S.SysResults[S.SYS_INFLOW] = S.SysResults[S.SYS_RUNOFF] + S.SysResults[S.SYS_DWFLOW] + S.SysResults[S.SYS_GWFLOW] + S.SysResults[S.SYS_IIFLOW] + S.SysResults[S.SYS_EXFLOW];
        R.fwriteF(S.SysResults, 0, (int)(unchecked((ulong)(15))), S.Fout.file);
        if ((S.Foutflows.mode == S.SAVE_FILE) && (!(S.IgnoreRouting != 0)))
        {
            S.iface_saveOutletResults(reportDate, S.Foutflows.file);
        }
        S.Nperiods++;
    }

    // output.c:515
    public static void output_end()
    {
        int k = 0;
        R.fwriteN1((long)(S.IDStartPos), 4, S.Fout.file);
        R.fwriteN1((long)(S.InputStartPos), 4, S.Fout.file);
        R.fwriteN1((long)(S.OutputStartPos), 4, S.Fout.file);
        k = (int)(S.Nperiods);
        R.fwriteI1(k, S.Fout.file);
        k = S.ErrorCode;
        R.fwriteI1(k, S.Fout.file);
        k = 516114522;
        if (R.fwriteI1(k, S.Fout.file) < unchecked((ulong)(1)))
        {
            S.report_writeErrorMsg(S.ERR_OUT_WRITE, S.L0);
        }
    }

    // output.c:539
    public static void output_close()
    {
        {
            if ((S.SubcatchResults != null))
            {
                R.free(S.SubcatchResults);
                S.SubcatchResults = (null);
            }
        }
        ;
        {
            if ((S.NodeResults != null))
            {
                R.free(S.NodeResults);
                S.NodeResults = (null);
            }
        }
        ;
        {
            if ((S.LinkResults != null))
            {
                R.free(S.LinkResults);
                S.LinkResults = (null);
            }
        }
        ;
        S.output_closeAvgResults();
    }

    // output.c:554
    public static void output_saveID(CPtr id, CFile file)
    {
        int n = (int)(R.strlen(id));
        R.fwriteI1(n, file);
        R.fwriteC(id, 0, (int)(unchecked((ulong)(n))), file);
    }

    // output.c:569
    public static void output_saveSubcatchResults(double reportTime, CFile file)
    {
        int j = 0;
        double f = 0.0;
        double area = 0.0;
        float totalArea = 0.0f;
        double reportDate = S.getDateTime(reportTime);
        for (j = 0; j < S.Nobjects[S.GAGE]; j++)
        {
            S.gage_setReportRainfall(j, reportDate);
        }
        f = (reportTime - S.OldRunoffTime) / (S.NewRunoffTime - S.OldRunoffTime);
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.subcatch_getResults(j, f, S.SubcatchResults);
            if ((S.Subcatch[j].rptFlag != 0))
            {
                R.fwriteF(S.SubcatchResults, 0, (int)(unchecked((ulong)(S.NumSubcatchVars))), file);
            }
            area = S.Subcatch[j].area * S.UCF(S.LANDAREA);
            totalArea += (float)(area);
            S.SysResults[S.SYS_RAINFALL] += (float)((S.SubcatchResults[S.SUBCATCH_RAINFALL] * area));
            S.SysResults[S.SYS_SNOWDEPTH] += (float)((S.SubcatchResults[S.SUBCATCH_SNOWDEPTH] * area));
            S.SysResults[S.SYS_EVAP] += (float)((S.SubcatchResults[S.SUBCATCH_EVAP] * area));
            if ((S.Subcatch[j].groundwater != null))
            {
                S.SysResults[S.SYS_EVAP] += (float)((S.Subcatch[j].groundwater.evapLoss * S.UCF(S.EVAPRATE) * area));
            }
            S.SysResults[S.SYS_INFIL] += (float)((S.SubcatchResults[S.SUBCATCH_INFIL] * area));
            S.SysResults[S.SYS_RUNOFF] += (float)(S.SubcatchResults[S.SUBCATCH_RUNOFF]);
        }
        if (totalArea > 0.0)
        {
            S.SysResults[S.SYS_EVAP] /= totalArea;
            S.SysResults[S.SYS_RAINFALL] /= totalArea;
            S.SysResults[S.SYS_SNOWDEPTH] /= totalArea;
            S.SysResults[S.SYS_INFIL] /= totalArea;
        }
        if (S.UnitSystem == S.SI)
        {
            f = (5.0 / 9.0) * (S.Temp.ta - 32.0);
        }
        else
        {
            f = S.Temp.ta;
        }
        S.SysResults[S.SYS_TEMPERATURE] = (float)(f);
        f = S.Evap.rate * S.UCF(S.EVAPRATE);
        S.SysResults[S.SYS_PET] = (float)(f);
    }

    // output.c:636
    public static void output_saveNodeResults(double reportTime, CFile file)
    {
        int j = 0;
        double f = (reportTime - S.OldRoutingTime) / (S.NewRoutingTime - S.OldRoutingTime);
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            S.node_getResults(j, f, S.NodeResults);
            if ((S.Node[j].rptFlag != 0))
            {
                R.fwriteF(S.NodeResults, 0, (int)(unchecked((ulong)(S.NumNodeVars))), file);
            }
            S.stats_updateMaxNodeDepth(j, S.NodeResults[S.NODE_DEPTH]);
            S.SysResults[S.SYS_STORAGE] += S.NodeResults[S.NODE_VOLUME];
        }
    }

    // output.c:666
    public static void output_saveLinkResults(double reportTime, CFile file)
    {
        int j = 0;
        double f = 0.0;
        double z = 0.0;
        f = (reportTime - S.OldRoutingTime) / (S.NewRoutingTime - S.OldRoutingTime);
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            if ((S.Link[j].rptFlag != 0))
            {
                S.link_getResults(j, f, S.LinkResults);
                R.fwriteF(S.LinkResults, 0, (int)(unchecked((ulong)(S.NumLinkVars))), file);
            }
            z = ((1.0 - f) * S.Link[j].oldVolume + f * S.Link[j].newVolume) * S.UCF(S.VOLUME);
            S.SysResults[S.SYS_STORAGE] += (float)(z);
        }
    }

    // output.c:699
    public static void output_readDateTime(long period, ref double days)
    {
        long p = period;
        long bytePos = S.OutputStartPos + (p - 1) * S.BytesPerPeriod;
        R.fseek(S.Fout.file, bytePos, 0);
        days = -693594;
        R.freadD1(ref days, S.Fout.file);
    }

    // output.c:716
    public static void output_readSubcatchResults(long period, int index)
    {
        long offset = index * S.NumSubcatchVars;
        long p = period;
        long bytePos = (long)(unchecked((ulong)(S.OutputStartPos + (p - 1) * S.BytesPerPeriod)) + 8 + unchecked((ulong)(offset)) * 4);
        R.fseek(S.Fout.file, bytePos, 0);
        R.freadF(S.SubcatchResults, 0, (int)(unchecked((ulong)(S.NumSubcatchVars))), S.Fout.file);
    }

    // output.c:735
    public static void output_readNodeResults(long period, int index)
    {
        long offset = S.NumSubcatch * S.NumSubcatchVars + index * S.NumNodeVars;
        long p = period;
        long bytePos = (long)(unchecked((ulong)(S.OutputStartPos + (p - 1) * S.BytesPerPeriod)) + 8 + unchecked((ulong)(offset)) * 4);
        R.fseek(S.Fout.file, bytePos, 0);
        R.freadF(S.NodeResults, 0, (int)(unchecked((ulong)(S.NumNodeVars))), S.Fout.file);
    }

    // output.c:753
    public static void output_readLinkResults(long period, int index)
    {
        long offset = (S.NumSubcatch * S.NumSubcatchVars + S.NumNodes * S.NumNodeVars + index * S.NumLinkVars);
        long p = period;
        long bytePos = (long)(unchecked((ulong)(S.OutputStartPos + (p - 1) * S.BytesPerPeriod)) + 8 + unchecked((ulong)(offset)) * 4);
        R.fseek(S.Fout.file, bytePos, 0);
        R.freadF(S.LinkResults, 0, (int)(unchecked((ulong)(S.NumLinkVars))), S.Fout.file);
        R.freadF(S.SysResults, 0, (int)(unchecked((ulong)(15))), S.Fout.file);
    }

    // output.c:774
    public static int output_openAvgResults()
    {
        int i = 0;
        S.AvgNodeResults = R.NewArray<TAvgResults>((int)(unchecked((ulong)(S.NumNodes))));
        if (S.AvgNodeResults == null)
        {
            return 0;
        }
        for (i = 0; i < S.NumNodes; i++)
        {
            S.AvgNodeResults[i].xAvg = (null);
        }
        S.AvgLinkResults = R.NewArray<TAvgResults>((int)(unchecked((ulong)(S.NumLinks))));
        if (S.AvgLinkResults == null)
        {
            S.output_closeAvgResults();
            return 0;
        }
        for (i = 0; i < S.NumLinks; i++)
        {
            S.AvgLinkResults[i].xAvg = (null);
        }
        for (i = 0; i < S.NumNodes; i++)
        {
            S.AvgNodeResults[i].xAvg = new float[(int)(unchecked((ulong)(S.NumNodeVars)))];
            if (S.AvgNodeResults[i].xAvg == null)
            {
                S.output_closeAvgResults();
                return 0;
            }
        }
        for (i = 0; i < S.NumLinks; i++)
        {
            S.AvgLinkResults[i].xAvg = new float[(int)(unchecked((ulong)(S.NumLinkVars)))];
            if (S.AvgLinkResults[i].xAvg == null)
            {
                S.output_closeAvgResults();
                return 0;
            }
        }
        return 1;
    }

    // output.c:820
    public static void output_closeAvgResults()
    {
        int i = 0;
        if ((S.AvgNodeResults != null))
        {
            for (i = 0; i < S.NumNodes; i++)
            {
                if ((S.AvgNodeResults[i].xAvg != null))
                {
                    R.free(S.AvgNodeResults[i].xAvg);
                    S.AvgNodeResults[i].xAvg = (null);
                }
            }
            ;
            {
                if ((S.AvgNodeResults != null))
                {
                    R.free(S.AvgNodeResults);
                    S.AvgNodeResults = (null);
                }
            }
            ;
        }
        if ((S.AvgLinkResults != null))
        {
            for (i = 0; i < S.NumLinks; i++)
            {
                if ((S.AvgLinkResults[i].xAvg != null))
                {
                    R.free(S.AvgLinkResults[i].xAvg);
                    S.AvgLinkResults[i].xAvg = (null);
                }
            }
            ;
            {
                if ((S.AvgLinkResults != null))
                {
                    R.free(S.AvgLinkResults);
                    S.AvgLinkResults = (null);
                }
            }
            ;
        }
    }

    // output.c:839
    public static void output_initAvgResults()
    {
        int i = 0;
        int j = 0;
        S.output_Nsteps = 0;
        for (i = 0; i < S.NumNodes; i++)
        {
            for (j = 0; j < S.NumNodeVars; j++)
            {
                S.AvgNodeResults[i].xAvg[j] = (float)(0.0);
            }
        }
        for (i = 0; i < S.NumLinks; i++)
        {
            for (j = 0; j < S.NumLinkVars; j++)
            {
                S.AvgLinkResults[i].xAvg[j] = (float)(0.0);
            }
        }
    }

    // output.c:857
    public static void output_updateAvgResults()
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int sign = 0;
        k = 0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (!(S.Node[i].rptFlag != 0))
            {
                continue;
            }
            S.node_getResults(i, 1.0, S.NodeResults);
            for (j = 0; j < S.NumNodeVars; j++)
            {
                S.AvgNodeResults[k].xAvg[j] += S.NodeResults[j];
            }
            k++;
        }
        k = 0;
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (!(S.Link[i].rptFlag != 0))
            {
                continue;
            }
            S.link_getResults(i, 1.0, S.LinkResults);
            sign = ((((S.LinkResults[S.LINK_FLOW]) < (float)(0)) ? (-1) : (1)));
            for (j = 0; j < S.NumLinkVars; j++)
            {
                if (j == S.LINK_CAPACITY)
                {
                    if (S.Link[i].type == S.CONDUIT)
                    {
                        S.AvgLinkResults[k].xAvg[j] += S.LinkResults[j];
                    }
                    else
                    {
                        S.AvgLinkResults[k].xAvg[j] = S.LinkResults[j] * (float)((S.output_Nsteps + 1));
                    }
                }
                else
                {
                    S.AvgLinkResults[k].xAvg[j] += S.LinkResults[j];
                }
            }
            k++;
        }
        S.output_Nsteps++;
    }

    // output.c:911
    public static void output_saveAvgResults(CFile file)
    {
        int i = 0;
        int j = 0;
        for (i = 0; i < S.NumNodes; i++)
        {
            for (j = 0; j < S.NumNodeVars; j++)
            {
                S.NodeResults[j] = S.AvgNodeResults[i].xAvg[j] / (float)(S.output_Nsteps);
            }
            R.fwriteF(S.NodeResults, 0, (int)(unchecked((ulong)(S.NumNodeVars))), file);
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            S.stats_updateMaxNodeDepth(i, S.Node[i].newDepth * S.UCF(S.LENGTH));
            S.SysResults[S.SYS_STORAGE] += (float)((S.Node[i].newVolume * S.UCF(S.VOLUME)));
        }
        for (i = 0; i < S.NumLinks; i++)
        {
            for (j = 0; j < S.NumLinkVars; j++)
            {
                S.LinkResults[j] = S.AvgLinkResults[i].xAvg[j] / (float)(S.output_Nsteps);
            }
            R.fwriteF(S.LinkResults, 0, (int)(unchecked((ulong)(S.NumLinkVars))), file);
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            S.SysResults[S.SYS_STORAGE] += (float)((S.Link[i].newVolume * S.UCF(S.VOLUME)));
        }
        S.output_initAvgResults();
    }

}
}
