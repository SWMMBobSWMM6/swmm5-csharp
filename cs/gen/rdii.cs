// Generated from EPA SWMM 5.2.4 (rdii.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double ZERO_RDII = 0.0001;
    public static CPtr FileStamp = R.BufFrom(S.L352, 11);
    public static TUHGroup[] UHGroup = null;
    public static int RdiiStep = 0;
    public static int NumRdiiNodes = 0;
    public static int[] RdiiNodeIndex = null;
    public static float[] RdiiNodeFlow = null;
    public static int RdiiFlowUnits = 0;
    public static double RdiiStartDate = 0.0;
    public static double RdiiEndDate = 0.0;
    public static double TotalRainVol = 0.0;
    public static double TotalRdiiVol = 0.0;
    public static int RdiiFileType = 0;

    // rdii.c:152
    public static int rdii_readRdiiInflow(CPtr[] tok, int ntoks)
    {
        int j = 0;
        int k = 0;
        double a = 0.0;
        TRdiiInflow inflow = null;
        if (ntoks < 3)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.NODE, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        k = S.project_findObject(S.UNITHYD, tok[1]);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[1]);
        }
        if ((!((S.getDouble(tok[2], ref a) != 0))) || (a < 0.0))
        {
            return S.error_setInpError(S.ERR_NUMBER, tok[2]);
        }
        inflow = S.Node[j].rdiiInflow;
        if (inflow == null)
        {
            inflow = new TRdiiInflow();
            if (!(inflow != null))
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
        }
        inflow.unitHyd = k;
        inflow.area = a / S.UCF(S.LANDAREA);
        S.Node[j].rdiiInflow = inflow;
        return 0;
    }

    // rdii.c:198
    public static void rdii_initUnitHyd(int j)
    {
        int i = 0;
        int m = 0;
        for (m = 0; m < 12; m++)
        {
            for (i = 0; i < 3; i++)
            {
                S.UnitHyd[j].iaMax[m][i] = 0.0;
                S.UnitHyd[j].iaRecov[m][i] = 0.0;
                S.UnitHyd[j].iaInit[m][i] = 0.0;
                S.UnitHyd[j].r[m][i] = 0.0;
                S.UnitHyd[j].tPeak[m][i] = 0;
                S.UnitHyd[j].tBase[m][i] = 0;
            }
        }
    }

    // rdii.c:224
    public static int rdii_readUnitHydParams(CPtr[] tok, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int m = 0;
        int g = 0;
        double[] x = new double[6];
        j = S.project_findObject(S.UNITHYD, tok[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, tok[0]);
        }
        if (S.UnitHyd[j].ID.IsNull)
        {
            S.UnitHyd[j].ID = S.project_findID(S.UNITHYD, tok[0]);
        }
        if (ntoks == 2)
        {
            g = S.project_findObject(S.GAGE, tok[1]);
            if (g < 0)
            {
                return S.error_setInpError(S.ERR_NAME, tok[1]);
            }
            S.UnitHyd[j].rainGage = g;
            S.Gage[g].isUsed = 1;
            return 0;
        }
        else if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        m = S.datetime_findMonth(tok[1]);
        if (m == 0)
        {
            if (!((S.match(tok[1], S.L351) != 0)))
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
            }
        }
        k = S.findmatch(tok[2], S.UHTypeWords);
        if (k < 0)
        {
            return S.readOldUHFormat(j, m, tok, ntoks);
        }
        for (i = 0; i < 3; i++)
        {
            if (!((S.getDouble(tok[i + 3], ref x[i]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i + 3]);
            }
        }
        for (i = 3; i < 6; i++)
        {
            x[i] = 0.0;
            if (ntoks > i + 3)
            {
                if (!((S.getDouble(tok[i + 3], ref x[i]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i + 2]);
                }
            }
        }
        S.setUnitHydParams(j, k, m, x);
        return 0;
    }

    // rdii.c:293
    public static int readOldUHFormat(int j, int m, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int k = 0;
        double[] p = new double[9];
        double[] x = new double[6];
        if (ntoks < 11)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        for (i = 0; i < 9; i++)
        {
            if (!((S.getDouble(tok[i + 2], ref p[i]) != 0)))
            {
                return S.error_setInpError(S.ERR_NUMBER, tok[i + 2]);
            }
        }
        for (i = 0; i < 3; i++)
        {
            x[i + 3] = 0.0;
            if (ntoks > i + 11)
            {
                if (!((S.getDouble(tok[i + 11], ref x[i + 3]) != 0)))
                {
                    return S.error_setInpError(S.ERR_NUMBER, tok[i + 11]);
                }
            }
        }
        for (k = 0; k < 3; k++)
        {
            for (i = 0; i < 3; i++)
            {
                x[i] = p[3 * k + i];
                S.setUnitHydParams(j, k, m, x);
            }
        }
        return 0;
    }

    // rdii.c:342
    public static void setUnitHydParams(int j, int i, int m, double[] x)
    {
        int m1 = 0;
        int m2 = 0;
        double t = 0.0;
        double k = 0.0;
        double tBase = 0.0;
        if (m == 0)
        {
            m1 = 0;
            m2 = 11;
        }
        else
        {
            m1 = m - 1;
            m2 = m1;
        }
        for (m = m1; m <= m2; m++)
        {
            S.UnitHyd[j].r[m][i] = x[0];
            t = x[1];
            k = x[2];
            tBase = t * (1.0 + k);
            S.UnitHyd[j].tPeak[m][i] = R.D2L((t * 3600.0));
            S.UnitHyd[j].tBase[m][i] = R.D2L((tBase * 3600.0));
            S.UnitHyd[j].iaMax[m][i] = x[3];
            S.UnitHyd[j].iaRecov[m][i] = x[4];
            S.UnitHyd[j].iaInit[m][i] = x[5];
        }
    }

    // rdii.c:389
    public static void rdii_deleteRdiiInflow(int j)
    {
        if ((S.Node[j].rdiiInflow != null))
        {
            R.free(S.Node[j].rdiiInflow);
            S.Node[j].rdiiInflow = (null);
        }
    }

    // rdii.c:408
    public static void rdii_openRdii()
    {
        CPtr fStamp = R.BufFrom(S.L352, 11);
        S.RdiiNodeIndex = (null);
        S.RdiiNodeFlow = (null);
        S.NumRdiiNodes = 0;
        S.RdiiStartDate = -693594;
        if ((S.IgnoreRDII != 0))
        {
            return;
        }
        if (S.Frdii.mode != S.USE_FILE)
        {
            S.createRdiiFile();
        }
        if ((S.Frdii.mode == S.NO_FILE) || (S.ErrorCode != 0))
        {
            return;
        }
        S.Frdii.file = R.fopen(S.Frdii.name, S.L353);
        if (S.Frdii.file == null)
        {
            if (S.Frdii.mode == S.SCRATCH_FILE)
            {
                S.report_writeErrorMsg(S.ERR_RDII_FILE_SCRATCH, S.L0);
            }
            else
            {
                S.report_writeErrorMsg(S.ERR_RDII_FILE_OPEN, S.Frdii.name);
            }
            return;
        }
        R.freadC(fStamp, 0, (int)(R.strlen(S.FileStamp)), S.Frdii.file);
        if (R.strcmp(fStamp, S.FileStamp) == 0)
        {
            S.RdiiFileType = S.BINARY;
            S.ErrorCode = S.readRdiiFileHeader();
        }
        else
        {
            R.fclose(S.Frdii.file);
            S.RdiiFileType = S.TEXT;
            S.openRdiiTextFile();
        }
        if ((S.ErrorCode != 0))
        {
            S.report_writeErrorMsg(S.ErrorCode, S.Frdii.name);
        }
        else
        {
            S.readRdiiFlows();
        }
    }

    // rdii.c:470
    public static void openRdiiTextFile()
    {
        S.Frdii.file = R.fopen(S.Frdii.name, S.L13);
        if (S.Frdii.file == null)
        {
            if (S.Frdii.mode == S.SCRATCH_FILE)
            {
                S.report_writeErrorMsg(S.ERR_RDII_FILE_SCRATCH, S.L0);
            }
            else
            {
                S.report_writeErrorMsg(S.ERR_RDII_FILE_OPEN, S.Frdii.name);
            }
            return;
        }
        S.ErrorCode = S.readRdiiTextFileHeader();
        if ((S.ErrorCode != 0))
        {
            S.report_writeErrorMsg(S.ErrorCode, S.Frdii.name);
        }
    }

    // rdii.c:497
    public static void rdii_closeRdii()
    {
        if ((S.Frdii.file != null))
        {
            R.fclose(S.Frdii.file);
        }
        if (S.Frdii.mode == S.SCRATCH_FILE)
        {
            R.remove(S.Frdii.name);
        }
        {
            if ((S.RdiiNodeIndex != null))
            {
                R.free(S.RdiiNodeIndex);
                S.RdiiNodeIndex = (null);
            }
        }
        ;
        {
            if ((S.RdiiNodeFlow != null))
            {
                R.free(S.RdiiNodeFlow);
                S.RdiiNodeFlow = (null);
            }
        }
        ;
    }

    // rdii.c:512
    public static int rdii_getNumRdiiFlows(double aDate)
    {
        if (S.NumRdiiNodes == 0)
        {
            return 0;
        }
        if (!(S.Frdii.file != null))
        {
            return 0;
        }
        while (!((R.feof(S.Frdii.file) != 0)))
        {
            if (S.RdiiStartDate == -693594)
            {
                return 0;
            }
            if (aDate < S.RdiiStartDate)
            {
                return 0;
            }
            if (aDate < S.RdiiEndDate)
            {
                return S.NumRdiiNodes;
            }
            else
            {
                S.readRdiiFlows();
            }
        }
        return 0;
    }

    // rdii.c:542
    public static void rdii_getRdiiFlow(int i, ref int j, ref double q)
    {
        if ((i >= 0) && (i < S.NumRdiiNodes))
        {
            j = S.RdiiNodeIndex[i];
            q = S.RdiiNodeFlow[i];
        }
    }

    // rdii.c:560
    public static int readRdiiFileHeader()
    {
        int i = 0;
        int j = 0;
        R.freadI1(ref S.RdiiStep, S.Frdii.file);
        if (S.RdiiStep <= 0)
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        R.freadI1(ref S.NumRdiiNodes, S.Frdii.file);
        if (S.NumRdiiNodes <= 0)
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        S.RdiiNodeIndex = new int[(int)(unchecked((ulong)(S.NumRdiiNodes)))];
        if (!(S.RdiiNodeIndex != null))
        {
            return S.ERR_MEMORY;
        }
        S.RdiiNodeFlow = new float[(int)(unchecked((ulong)(S.NumRdiiNodes)))];
        if (!(S.RdiiNodeFlow != null))
        {
            return S.ERR_MEMORY;
        }
        if ((R.feof(S.Frdii.file) != 0))
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        R.freadI(S.RdiiNodeIndex, 0, (int)(unchecked((ulong)(S.NumRdiiNodes))), S.Frdii.file);
        for (i = 0; i < S.NumRdiiNodes; i++)
        {
            j = S.RdiiNodeIndex[i];
            if (S.Node[j].rdiiInflow == null)
            {
                return S.ERR_RDII_FILE_FORMAT;
            }
        }
        if ((R.feof(S.Frdii.file) != 0))
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        return 0;
    }

    // rdii.c:595
    public static int readRdiiTextFileHeader()
    {
        int i = 0;
        int j = 0;
        CPtr line = R.Buf(1025);
        CPtr s1 = R.BufFrom(S.L0, 1025);
        CPtr s2 = R.Buf(1025);
        R.fgets(line, 1024, S.Frdii.file);
        if ((!((R.sscanf(line, S.L169, s1) != 0))) || (R.strcmp(s1, S.L181) != 0))
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        R.fgets(line, 1024, S.Frdii.file);
        S.RdiiStep = 0;
        R.fgets(line, 1024, S.Frdii.file);
        if ((!((R.sscanf(line, S.L182, R.RefI(__v => S.RdiiStep = __v)) != 0))) || (S.RdiiStep <= 0))
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        R.fgets(line, 1024, S.Frdii.file);
        R.fgets(line, 1024, S.Frdii.file);
        if (R.sscanf(line, S.L44, s1, s2) < 2)
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        S.RdiiFlowUnits = S.findmatch(s2, S.FlowUnitWords);
        if (S.RdiiFlowUnits < 0)
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        R.fgets(line, 1024, S.Frdii.file);
        if ((R.sscanf(line, S.L182, R.RefI(__v => S.NumRdiiNodes = __v)) < 1) || (S.NumRdiiNodes <= 0))
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        S.RdiiNodeIndex = new int[(int)(unchecked((ulong)(S.NumRdiiNodes)))];
        if (!(S.RdiiNodeIndex != null))
        {
            return S.ERR_MEMORY;
        }
        S.RdiiNodeFlow = new float[(int)(unchecked((ulong)(S.NumRdiiNodes)))];
        if (!(S.RdiiNodeFlow != null))
        {
            return S.ERR_MEMORY;
        }
        for (i = 0; i < S.NumRdiiNodes; i++)
        {
            if ((R.feof(S.Frdii.file) != 0))
            {
                return S.ERR_RDII_FILE_FORMAT;
            }
            R.fgets(line, 1024, S.Frdii.file);
            if (!((R.sscanf(line, S.L169, s1) != 0)))
            {
                return S.ERR_RDII_FILE_FORMAT;
            }
            j = S.project_findObject(S.NODE, s1);
            if (j < 0)
            {
                return S.ERR_RDII_FILE_FORMAT;
            }
            S.RdiiNodeIndex[i] = j;
        }
        if ((R.feof(S.Frdii.file) != 0))
        {
            return S.ERR_RDII_FILE_FORMAT;
        }
        R.fgets(line, 1024, S.Frdii.file);
        return 0;
    }

    // rdii.c:663
    public static void readRdiiFlows()
    {
        if (S.RdiiFileType == S.TEXT)
        {
            S.readRdiiTextFlows();
        }
        else
        {
            S.RdiiStartDate = -693594;
            S.RdiiEndDate = -693594;
            if ((R.feof(S.Frdii.file) != 0))
            {
                return;
            }
            R.freadD1(ref S.RdiiStartDate, S.Frdii.file);
            if (S.RdiiStartDate == -693594)
            {
                return;
            }
            if (R.freadF(S.RdiiNodeFlow, 0, (int)(unchecked((ulong)(S.NumRdiiNodes))), S.Frdii.file) < unchecked((ulong)(S.NumRdiiNodes)))
            {
                S.RdiiStartDate = -693594;
            }
            else
            {
                S.RdiiEndDate = S.datetime_addSeconds(S.RdiiStartDate, S.RdiiStep);
            }
        }
    }

    // rdii.c:686
    public static void readRdiiTextFlows()
    {
        int i = 0;
        int n = 0;
        int yr = 0;
        int mon = 0;
        int day = 0;
        int hr = 0;
        int min = 0;
        int sec = 0;
        double x = 0.0;
        CPtr line = R.Buf(1025);
        CPtr s = R.Buf(1025);
        S.RdiiStartDate = -693594;
        for (i = 0; i < S.NumRdiiNodes; i++)
        {
            if ((R.feof(S.Frdii.file) != 0))
            {
                return;
            }
            R.fgets(line, 1024, S.Frdii.file);
            n = R.sscanf(line, S.L354, s, R.RefI(__v => yr = __v), R.RefI(__v => mon = __v), R.RefI(__v => day = __v), R.RefI(__v => hr = __v), R.RefI(__v => min = __v), R.RefI(__v => sec = __v), R.RefD(__v => x = __v));
            if (n < 8)
            {
                return;
            }
            S.RdiiNodeFlow[i] = (float)((x / S.Qcf[S.RdiiFlowUnits]));
        }
        S.RdiiStartDate = S.datetime_encodeDate(yr, mon, day) + S.datetime_encodeTime(hr, min, sec);
        S.RdiiEndDate = S.datetime_addSeconds(S.RdiiStartDate, S.RdiiStep);
    }

    // rdii.c:720
    public static void createRdiiFile()
    {
        int hasRdii = 0;
        double elapsedTime = 0.0;
        double duration = 0.0;
        double currentDate = 0.0;
        S.RdiiStep = S.WetStep;
        S.NumRdiiNodes = S.getNumRdiiNodes();
        if (S.NumRdiiNodes == 0)
        {
            S.Frdii.mode = S.NO_FILE;
            return;
        }
        else if (S.Frdii.mode == S.NO_FILE)
        {
            S.Frdii.mode = S.SCRATCH_FILE;
        }
        S.validateRdii();
        S.initGageData();
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.openRdiiProcessor();
        if (!(S.ErrorCode != 0))
        {
            S.initUnitHydData();
            duration = S.TotalDuration / 1000.0;
            elapsedTime = 0.0;
            while ((elapsedTime <= duration) && (!(S.ErrorCode != 0)))
            {
                currentDate = S.StartDateTime + elapsedTime / 86400;
                S.getRainfall(currentDate);
                S.getUnitHydRdii(currentDate);
                hasRdii = S.getNodeRdii();
                if ((hasRdii != 0))
                {
                    S.saveRdiiFlows(currentDate);
                }
                elapsedTime += S.RdiiStep;
            }
        }
        S.closeRdiiProcessor();
    }

    // rdii.c:793
    public static int getNumRdiiNodes()
    {
        int j = 0;
        int n = 0;
        n = 0;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if ((S.Node[j].rdiiInflow != null))
            {
                n++;
            }
        }
        return n;
    }

    // rdii.c:813
    public static void validateRdii()
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int m = 0;
        double rsum = 0.0;
        for (j = 0; j < S.Nobjects[S.UNITHYD]; j++)
        {
            for (m = 0; m < 12; m++)
            {
                rsum = 0.0;
                for (k = 0; k < 3; k++)
                {
                    if (S.UnitHyd[j].tBase[m][k] == 0)
                    {
                        continue;
                    }
                    if (S.UnitHyd[j].tPeak[m][k] < 0.0)
                    {
                        S.report_writeErrorMsg(S.ERR_UNITHYD_TIMES, S.UnitHyd[j].ID);
                    }
                    if (S.UnitHyd[j].r[m][k] < 0.0)
                    {
                        S.report_writeErrorMsg(S.ERR_UNITHYD_RATIOS, S.UnitHyd[j].ID);
                    }
                    else
                    {
                        rsum += S.UnitHyd[j].r[m][k];
                    }
                }
                if (rsum > 1.01)
                {
                    S.report_writeErrorMsg(S.ERR_UNITHYD_RATIOS, S.UnitHyd[j].ID);
                }
            }
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if ((S.Node[i].rdiiInflow != null))
            {
                if (S.Node[i].rdiiInflow.area < 0.0)
                {
                    S.report_writeErrorMsg(S.ERR_RDII_AREA, S.Node[i].ID);
                }
            }
        }
    }

    // rdii.c:877
    public static void openRdiiProcessor()
    {
        int j = 0;
        int n = 0;
        S.UHGroup = (null);
        S.RdiiNodeIndex = (null);
        S.RdiiNodeFlow = (null);
        S.TotalRainVol = 0.0;
        S.TotalRdiiVol = 0.0;
        if (!((S.allocRdiiMemory() != 0)))
        {
            S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
            return;
        }
        if (!((S.openNewRdiiFile() != 0)))
        {
            S.report_writeErrorMsg(S.ERR_RDII_FILE_SCRATCH, S.L0);
            return;
        }
        n = 0;
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if ((S.Node[j].rdiiInflow != null) && (S.RdiiNodeIndex != null))
            {
                S.RdiiNodeIndex[n] = j;
                n++;
            }
        }
    }

    // rdii.c:922
    public static int allocRdiiMemory()
    {
        int i = 0;
        int k = 0;
        int n = 0;
        S.UHGroup = R.NewArray<TUHGroup>((int)(unchecked((ulong)(S.Nobjects[S.UNITHYD]))));
        if (!(S.UHGroup != null))
        {
            return 0;
        }
        for (i = 0; i < S.Nobjects[S.UNITHYD]; i++)
        {
            S.UHGroup[i].rainInterval = S.getRainInterval(i);
            for (k = 0; k < 3; k++)
            {
                S.UHGroup[i].uh[k].pastRain = (null);
                S.UHGroup[i].uh[k].pastMonth = CPtr.Null;
                S.UHGroup[i].uh[k].maxPeriods = S.getMaxPeriods(i, k);
                n = S.UHGroup[i].uh[k].maxPeriods;
                if (n > 0)
                {
                    S.UHGroup[i].uh[k].pastRain = new double[(int)(unchecked((ulong)(n)))];
                    if (!(S.UHGroup[i].uh[k].pastRain != null))
                    {
                        return 0;
                    }
                    S.UHGroup[i].uh[k].pastMonth = R.Buf((int)(unchecked((ulong)(n))));
                    if (!(!(S.UHGroup[i].uh[k].pastMonth).IsNull))
                    {
                        return 0;
                    }
                }
            }
        }
        S.RdiiNodeIndex = new int[(int)(unchecked((ulong)(S.NumRdiiNodes)))];
        if (!(S.RdiiNodeIndex != null))
        {
            return 0;
        }
        S.RdiiNodeFlow = new float[(int)(unchecked((ulong)(S.NumRdiiNodes)))];
        if (!(S.RdiiNodeFlow != null))
        {
            return 0;
        }
        return 1;
    }

    // rdii.c:970
    public static int getRainInterval(int i)
    {
        int ri = 0;
        int tLimb = 0;
        int k = 0;
        int m = 0;
        ri = S.WetStep;
        for (m = 0; m < 12; m++)
        {
            for (k = 0; k < 3; k++)
            {
                if (S.UnitHyd[i].tPeak[m][k] > 0)
                {
                    tLimb = (int)(S.UnitHyd[i].tPeak[m][k]);
                    ri = ((((ri) <= (tLimb)) ? (ri) : (tLimb)));
                    tLimb = (int)(S.UnitHyd[i].tBase[m][k] - tLimb);
                    if (tLimb > 0)
                    {
                        ri = ((((ri) <= (tLimb)) ? (ri) : (tLimb)));
                    }
                }
            }
        }
        return ri;
    }

    // rdii.c:1005
    public static int getMaxPeriods(int i, int k)
    {
        int m = 0;
        int n = 0;
        int nMax = 0;
        int rainInterval = 0;
        rainInterval = S.UHGroup[i].rainInterval;
        nMax = 0;
        for (m = 0; m < 12; m++)
        {
            n = (int)((S.UnitHyd[i].tBase[m][k] / rainInterval) + 1);
            nMax = ((((n) >= (nMax)) ? (n) : (nMax)));
        }
        return nMax;
    }

    // rdii.c:1034
    public static void initGageData()
    {
        int i = 0;
        int g = 0;
        for (g = 0; g < S.Nobjects[S.GAGE]; g++)
        {
            if (S.Gage[g].tSeries >= 0)
            {
                S.table_tseriesInit(S.Tseries[S.Gage[g].tSeries]);
            }
            S.gage_initState(g);
        }
        for (i = 0; i < S.Nobjects[S.UNITHYD]; i++)
        {
            g = S.UnitHyd[i].rainGage;
            if (g >= 0)
            {
                if (S.Gage[g].coGage >= 0)
                {
                    S.UnitHyd[i].rainGage = S.Gage[g].coGage;
                }
            }
        }
    }

    // rdii.c:1072
    public static void initUnitHydData()
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int n = 0;
        int month = 0;
        month = S.datetime_monthOfYear(S.StartDateTime) - 1;
        for (i = 0; i < S.Nobjects[S.UNITHYD]; i++)
        {
            for (k = 0; k < 3; k++)
            {
                S.UHGroup[i].uh[k].drySeconds = (S.UHGroup[i].uh[k].maxPeriods * S.UHGroup[i].rainInterval) + 1;
                S.UHGroup[i].uh[k].period = S.UHGroup[i].uh[k].maxPeriods + 1;
                S.UHGroup[i].uh[k].hasPastRain = 0;
                S.UHGroup[i].uh[k].iaUsed = S.UnitHyd[i].iaInit[month][k];
            }
            S.UHGroup[i].gageDate = S.StartDateTime;
            S.UHGroup[i].area = 0.0;
            S.UHGroup[i].rdii = 0.0;
        }
        for (i = 0; i < S.Nobjects[S.UNITHYD]; i++)
        {
            S.UHGroup[i].isUsed = 0;
        }
        for (n = 0; n < S.NumRdiiNodes; n++)
        {
            j = S.RdiiNodeIndex[n];
            i = S.Node[j].rdiiInflow.unitHyd;
            S.UHGroup[i].isUsed = 1;
            S.UHGroup[i].lastDate = S.StartDateTime;
            S.UHGroup[i].area += S.Node[j].rdiiInflow.area;
        }
    }

    // rdii.c:1128
    public static int openNewRdiiFile()
    {
        int j = 0;
        if (S.Frdii.mode == S.SCRATCH_FILE)
        {
            S.getTempFileName(S.Frdii.name);
        }
        S.Frdii.file = R.fopen(S.Frdii.name, S.L166);
        if (S.Frdii.file == null)
        {
            return 0;
        }
        R.fwriteC(S.FileStamp, 0, (int)(R.strlen(S.FileStamp)), S.Frdii.file);
        R.fwriteI1(S.RdiiStep, S.Frdii.file);
        R.fwriteI1(S.NumRdiiNodes, S.Frdii.file);
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            if ((S.Node[j].rdiiInflow != null))
            {
                R.fwriteI1(j, S.Frdii.file);
            }
        }
        return 1;
    }

    // rdii.c:1163
    public static void getRainfall(double currentDate)
    {
        int j = 0;
        int k = 0;
        int g = 0;
        int i = 0;
        int month = 0;
        int rainInterval = 0;
        double rainDepth = 0.0;
        double excessDepth = 0.0;
        double gageDate = 0.0;
        month = S.datetime_monthOfYear(currentDate) - 1;
        for (g = 0; g < S.Nobjects[S.GAGE]; g++)
        {
            S.Gage[g].isCurrent = 0;
        }
        for (j = 0; j < S.Nobjects[S.UNITHYD]; j++)
        {
            g = S.UnitHyd[j].rainGage;
            rainInterval = S.UHGroup[j].rainInterval;
            while (S.UHGroup[j].gageDate < currentDate)
            {
                gageDate = S.UHGroup[j].gageDate;
                S.Adjust.rainFactor = S.Adjust.rain[S.datetime_monthOfYear(gageDate) - 1];
                if (!(S.Gage[g].isCurrent != 0))
                {
                    S.gage_setState(g, gageDate);
                    S.Gage[g].isCurrent = 1;
                }
                rainDepth = S.Gage[g].rainfall * (double)(rainInterval) / 3600.0;
                S.TotalRainVol += rainDepth / S.UCF(S.RAINDEPTH) * S.UHGroup[j].area;
                for (k = 0; k < 3; k++)
                {
                    excessDepth = S.applyIA(j, k, gageDate, rainInterval, rainDepth);
                    S.updateDryPeriod(j, k, excessDepth, rainInterval);
                    i = S.UHGroup[j].uh[k].period;
                    if (i >= S.UHGroup[j].uh[k].maxPeriods)
                    {
                        i = 0;
                    }
                    S.UHGroup[j].uh[k].pastRain[i] = excessDepth;
                    S.UHGroup[j].uh[k].pastMonth[i] = month;
                    S.UHGroup[j].uh[k].period = i + 1;
                }
                S.UHGroup[j].gageDate = S.datetime_addSeconds(gageDate, rainInterval);
            }
        }
    }

    // rdii.c:1231
    public static double applyIA(int j, int k, double aDate, double dt, double rainDepth)
    {
        int m = 0;
        double ia = 0.0;
        double netRainDepth = 0.0;
        m = S.datetime_monthOfYear(aDate) - 1;
        ia = S.UnitHyd[j].iaMax[m][k] - S.UHGroup[j].uh[k].iaUsed;
        ia = ((((ia) >= (0.0)) ? (ia) : (0.0)));
        if (rainDepth > 0.0)
        {
            netRainDepth = rainDepth - ia;
            netRainDepth = ((((netRainDepth) >= (0.0)) ? (netRainDepth) : (0.0)));
            ia = rainDepth - netRainDepth;
            S.UHGroup[j].uh[k].iaUsed += ia;
        }
        else
        {
            S.UHGroup[j].uh[k].iaUsed -= dt / 86400.0 * S.UnitHyd[j].iaRecov[m][k];
            S.UHGroup[j].uh[k].iaUsed = ((((S.UHGroup[j].uh[k].iaUsed) >= (0.0)) ? (S.UHGroup[j].uh[k].iaUsed) : (0.0)));
            netRainDepth = 0.0;
        }
        return netRainDepth;
    }

    // rdii.c:1276
    public static void updateDryPeriod(int j, int k, double rainDepth, int rainInterval)
    {
        int i = 0;
        if (rainDepth > 0.0)
        {
            if (S.UHGroup[j].uh[k].drySeconds >= rainInterval * S.UHGroup[j].uh[k].maxPeriods)
            {
                for (i = 0; i < S.UHGroup[j].uh[k].maxPeriods; i++)
                {
                    S.UHGroup[j].uh[k].pastRain[i] = 0.0;
                }
                S.UHGroup[j].uh[k].period = 0;
            }
            S.UHGroup[j].uh[k].drySeconds = 0;
            S.UHGroup[j].uh[k].hasPastRain = 1;
        }
        else
        {
            S.UHGroup[j].uh[k].drySeconds += rainInterval;
            if (S.UHGroup[j].uh[k].drySeconds >= rainInterval * S.UHGroup[j].uh[k].maxPeriods)
            {
                S.UHGroup[j].uh[k].hasPastRain = 0;
            }
            else
            {
                S.UHGroup[j].uh[k].hasPastRain = 1;
            }
        }
    }

    // rdii.c:1321
    public static void getUnitHydRdii(double currentDate)
    {
        int j = 0;
        int k = 0;
        int rainInterval = 0;
        for (j = 0; j < S.Nobjects[S.UNITHYD]; j++)
        {
            if (!(S.UHGroup[j].isUsed != 0))
            {
                continue;
            }
            if (currentDate < S.UHGroup[j].lastDate)
            {
                continue;
            }
            S.UHGroup[j].lastDate = S.UHGroup[j].gageDate;
            rainInterval = S.UHGroup[j].rainInterval;
            S.UHGroup[j].rdii = 0.0;
            for (k = 0; k < 3; k++)
            {
                if ((S.UHGroup[j].uh[k].hasPastRain != 0))
                {
                    S.UHGroup[j].rdii += S.getUnitHydConvol(j, k, rainInterval);
                }
            }
        }
    }

    // rdii.c:1358
    public static double getUnitHydConvol(int j, int k, int rainInterval)
    {
        int i = 0;
        int m = 0;
        int p = 0;
        int pMax = 0;
        double t = 0.0;
        double u = 0.0;
        double v = 0.0;
        double rdii = 0.0;
        TUHData uh = null;
        rdii = 0.0;
        uh = S.UHGroup[j].uh[k];
        i = uh.period - 1;
        if (i < 0)
        {
            i = uh.maxPeriods - 1;
        }
        pMax = uh.maxPeriods;
        p = 1;
        while (p < pMax)
        {
            v = uh.pastRain[i];
            m = uh.pastMonth[i];
            if (v > 0.0)
            {
                t = ((double)((p)) - 0.5) * (double)(rainInterval);
                u = S.getUnitHydOrd(j, m, k, t) * S.UnitHyd[j].r[m][k];
                rdii += u * v;
            }
            p = p + 1;
            i = i - 1;
            if (i < 0)
            {
                i = uh.maxPeriods - 1;
            }
        }
        return rdii;
    }

    // rdii.c:1411
    public static double getUnitHydOrd(int h, int m, int k, double t)
    {
        double qPeak = 0.0;
        double f = 0.0;
        double t1 = 0.0;
        double t2 = 0.0;
        double tBase = 0.0;
        tBase = S.UnitHyd[h].tBase[m][k];
        if (t >= tBase)
        {
            return 0.0;
        }
        qPeak = 2.0 / tBase * 3600.0;
        t1 = S.UnitHyd[h].tPeak[m][k];
        t2 = tBase - t1;
        if (t <= t1)
        {
            f = t / t1;
        }
        else
        {
            f = 1.0 - (t - t1) / t2;
        }
        return ((((f) >= (0.0)) ? (f) : (0.0))) * qPeak;
    }

    // rdii.c:1446
    public static int getNodeRdii()
    {
        int hasRdii = 0;
        int i = 0;
        int j = 0;
        int n = 0;
        double rdii = 0.0;
        for (n = 0; n < S.NumRdiiNodes; n++)
        {
            j = S.RdiiNodeIndex[n];
            i = S.Node[j].rdiiInflow.unitHyd;
            rdii = S.UHGroup[i].rdii * S.Node[j].rdiiInflow.area / S.UCF(S.RAINFALL);
            if (rdii < S.ZERO_RDII)
            {
                rdii = 0.0;
            }
            else
            {
                hasRdii = 1;
            }
            S.RdiiNodeFlow[n] = (float)(rdii);
            if (rdii > 0.0)
            {
                S.TotalRdiiVol += rdii * (double)(S.RdiiStep);
            }
        }
        return hasRdii;
    }

    // rdii.c:1483
    public static void saveRdiiFlows(double currentDate)
    {
        R.fwriteD1(currentDate, S.Frdii.file);
        R.fwriteF(S.RdiiNodeFlow, 0, (int)(unchecked((ulong)(S.NumRdiiNodes))), S.Frdii.file);
    }

    // rdii.c:1496
    public static void closeRdiiProcessor()
    {
        if (!(S.ErrorCode != 0))
        {
            S.report_writeRdiiStats(S.TotalRainVol, S.TotalRdiiVol);
        }
        S.freeRdiiMemory();
        if ((S.Frdii.file != null))
        {
            R.fclose(S.Frdii.file);
        }
    }

    // rdii.c:1516
    public static void freeRdiiMemory()
    {
        int i = 0;
        int k = 0;
        if ((S.UHGroup != null))
        {
            for (i = 0; i < S.Nobjects[S.UNITHYD]; i++)
            {
                for (k = 0; k < 3; k++)
                {
                    {
                        if ((S.UHGroup[i].uh[k].pastRain != null))
                        {
                            R.free(S.UHGroup[i].uh[k].pastRain);
                            S.UHGroup[i].uh[k].pastRain = (null);
                        }
                    }
                    ;
                    {
                        if (!(S.UHGroup[i].uh[k].pastMonth).IsNull)
                        {
                            R.free(S.UHGroup[i].uh[k].pastMonth);
                            S.UHGroup[i].uh[k].pastMonth = CPtr.Null;
                        }
                    }
                    ;
                }
            }
            {
                if ((S.UHGroup != null))
                {
                    R.free(S.UHGroup);
                    S.UHGroup = (null);
                }
            }
            ;
        }
        {
            if ((S.RdiiNodeIndex != null))
            {
                R.free(S.RdiiNodeIndex);
                S.RdiiNodeIndex = (null);
            }
        }
        ;
        {
            if ((S.RdiiNodeFlow != null))
            {
                R.free(S.RdiiNodeFlow);
                S.RdiiNodeFlow = (null);
            }
        }
        ;
    }

}
}
