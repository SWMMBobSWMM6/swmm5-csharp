// Generated from EPA SWMM 5.2.4 (iface.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int IfaceFlowUnits = 0;
    public static int IfaceStep = 0;
    public static int NumIfacePolluts = 0;
    public static int[] IfacePolluts = null;
    public static int NumIfaceNodes = 0;
    public static int[] IfaceNodes = null;
    public static double[][] OldIfaceValues = null;
    public static double[][] NewIfaceValues = null;
    public static double IfaceFrac = 0.0;
    public static double OldIfaceDate = 0.0;
    public static double NewIfaceDate = 0.0;

    // iface.c:66
    public static int iface_readFileParams(CPtr[] tok, int ntoks)
    {
        int k = 0;
        int j = 0;
        CPtr fname = R.Buf(260);
        if (ntoks < 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        k = S.findmatch(tok[0], S.FileModeWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[0]);
        }
        j = S.findmatch(tok[1], S.FileTypeWords);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
        }
        if (ntoks < 3)
        {
            return 0;
        }
        S.sstrncpy(fname, tok[2], unchecked((ulong)(259)));
        switch (j)
        {
            case S.RAINFALL_FILE:
                S.Frain.mode = k;
                S.sstrncpy(S.Frain.name, S.addAbsolutePath(fname), unchecked((ulong)(259)));
                break;
            case S.RUNOFF_FILE:
                S.Frunoff.mode = k;
                S.sstrncpy(S.Frunoff.name, S.addAbsolutePath(fname), unchecked((ulong)(259)));
                break;
            case S.HOTSTART_FILE:
                if (k == S.USE_FILE)
                {
                    S.Fhotstart1.mode = k;
                    S.sstrncpy(S.Fhotstart1.name, S.addAbsolutePath(fname), unchecked((ulong)(259)));
                }
                else if (k == S.SAVE_FILE)
                {
                    S.Fhotstart2.mode = k;
                    S.sstrncpy(S.Fhotstart2.name, S.addAbsolutePath(fname), unchecked((ulong)(259)));
                }
                break;
            case S.RDII_FILE:
                S.Frdii.mode = k;
                S.sstrncpy(S.Frdii.name, fname, unchecked((ulong)(259)));
                break;
            case S.INFLOWS_FILE:
                if (k != S.USE_FILE)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                S.Finflows.mode = k;
                S.sstrncpy(S.Finflows.name, S.addAbsolutePath(fname), unchecked((ulong)(259)));
                break;
            case S.OUTFLOWS_FILE:
                if (k != S.SAVE_FILE)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                S.Foutflows.mode = k;
                S.sstrncpy(S.Foutflows.name, S.addAbsolutePath(fname), unchecked((ulong)(259)));
                break;
        }
        return 0;
    }

    // iface.c:138
    public static void iface_openRoutingFiles()
    {
        S.NumIfacePolluts = 0;
        S.IfacePolluts = (null);
        S.NumIfaceNodes = 0;
        S.IfaceNodes = (null);
        S.OldIfaceValues = (null);
        S.NewIfaceValues = (null);
        if ((S.Foutflows.mode != S.NO_FILE) && (S.Finflows.mode != S.NO_FILE))
        {
            if ((S.strcomp(S.Foutflows.name, S.Finflows.name) != 0))
            {
                S.report_writeErrorMsg(S.ERR_ROUTING_FILE_NAMES, S.L0);
                return;
            }
        }
        if (S.Foutflows.mode == S.SAVE_FILE)
        {
            S.openFileForOutput();
        }
        if (S.Finflows.mode == S.USE_FILE)
        {
            S.openFileForInput();
        }
    }

    // iface.c:170
    public static void iface_closeRoutingFiles()
    {
        {
            if ((S.IfacePolluts != null))
            {
                R.free(S.IfacePolluts);
                S.IfacePolluts = (null);
            }
        }
        ;
        {
            if ((S.IfaceNodes != null))
            {
                R.free(S.IfaceNodes);
                S.IfaceNodes = (null);
            }
        }
        ;
        if (S.OldIfaceValues != null)
        {
            S.project_freeMatrix(S.OldIfaceValues);
        }
        if (S.NewIfaceValues != null)
        {
            S.project_freeMatrix(S.NewIfaceValues);
        }
        if ((S.Finflows.file != null))
        {
            R.fclose(S.Finflows.file);
        }
        if ((S.Foutflows.file != null))
        {
            R.fclose(S.Foutflows.file);
        }
    }

    // iface.c:187
    public static int iface_getNumIfaceNodes(double currentDate)
    {
        if (S.OldIfaceDate > currentDate)
        {
            return 0;
        }
        while ((S.NewIfaceDate < currentDate) && (S.NewIfaceDate != -693594))
        {
            S.setOldIfaceValues();
            S.readNewIfaceValues();
        }
        if (S.NewIfaceDate == -693594)
        {
            return 0;
        }
        S.IfaceFrac = (currentDate - S.OldIfaceDate) / (S.NewIfaceDate - S.OldIfaceDate);
        S.IfaceFrac = ((((0.0) >= (S.IfaceFrac)) ? (0.0) : (S.IfaceFrac)));
        S.IfaceFrac = ((((S.IfaceFrac) <= (1.0)) ? (S.IfaceFrac) : (1.0)));
        return S.NumIfaceNodes;
    }

    // iface.c:219
    public static int iface_getIfaceNode(int index)
    {
        if ((index >= 0) && (index < S.NumIfaceNodes))
        {
            return S.IfaceNodes[index];
        }
        else
        {
            return -1;
        }
    }

    // iface.c:232
    public static double iface_getIfaceFlow(int index)
    {
        double q1 = 0.0;
        double q2 = 0.0;
        if ((index >= 0) && (index < S.NumIfaceNodes))
        {
            q1 = S.OldIfaceValues[index][0];
            q2 = S.NewIfaceValues[index][0];
            return (1.0 - S.IfaceFrac) * q1 + S.IfaceFrac * q2;
        }
        else
        {
            return 0.0;
        }
    }

    // iface.c:253
    public static double iface_getIfaceQual(int index, int pollut)
    {
        int j = 0;
        double c1 = 0.0;
        double c2 = 0.0;
        if ((index >= 0) && (index < S.NumIfaceNodes))
        {
            j = S.IfacePolluts[pollut];
            if (j < 0)
            {
                return 0.0;
            }
            c1 = S.OldIfaceValues[index][j + 1];
            c2 = S.NewIfaceValues[index][j + 1];
            return (1.0 - S.IfaceFrac) * c1 + S.IfaceFrac * c2;
        }
        else
        {
            return 0.0;
        }
    }

    // iface.c:281
    public static void iface_saveOutletResults(double reportDate, CFile file)
    {
        int i = 0;
        int p = 0;
        int yr = 0;
        int mon = 0;
        int day = 0;
        int hr = 0;
        int min = 0;
        int sec = 0;
        CPtr theDate = R.Buf(26);
        S.datetime_decodeDate(reportDate, ref yr, ref mon, ref day);
        S.datetime_decodeTime(reportDate, ref hr, ref min, ref sec);
        R.snprintf(theDate, unchecked((ulong)(26)), S.L167, yr, mon, day, hr, min, sec);
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (!((S.isOutletNode(i) != 0)))
            {
                continue;
            }
            R.fprintf(file, S.L168, S.Node[i].ID);
            R.fprintf(file, S.L169, theDate);
            R.fprintf(file, S.L170, S.Node[i].inflow * S.UCF(S.FLOW));
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                R.fprintf(file, S.L170, S.Node[i].newQual[p]);
            }
        }
    }

    // iface.c:313
    public static void openFileForOutput()
    {
        int i = 0;
        int n = 0;
        S.Foutflows.file = R.fopen(S.Foutflows.name, S.L171);
        if (S.Foutflows.file == null)
        {
            S.report_writeErrorMsg(S.ERR_ROUTING_FILE_OPEN, S.Foutflows.name);
            return;
        }
        R.fprintf(S.Foutflows.file, S.L172);
        R.fprintf(S.Foutflows.file, S.L173, S.Title[0]);
        R.fprintf(S.Foutflows.file, S.L174, S.ReportStep);
        R.fprintf(S.Foutflows.file, S.L175, S.Nobjects[S.POLLUT] + 1);
        R.fprintf(S.Foutflows.file, S.L176, S.FlowUnitWords[S.FlowUnits]);
        for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
        {
            R.fprintf(S.Foutflows.file, S.L177, S.Pollut[i].ID, S.QualUnitsWords[S.Pollut[i].units]);
        }
        n = 0;
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if ((S.isOutletNode(i) != 0))
            {
                n++;
            }
        }
        R.fprintf(S.Foutflows.file, S.L178, n);
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if ((S.isOutletNode(i) != 0))
            {
                R.fprintf(S.Foutflows.file, S.L173, S.Node[i].ID);
            }
        }
        R.fprintf(S.Foutflows.file, S.L179);
        for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
        {
            R.fprintf(S.Foutflows.file, S.L180, S.Pollut[i].ID);
        }
        if (S.ReportStart == S.StartDateTime)
        {
            S.iface_saveOutletResults(S.ReportStart, S.Foutflows.file);
        }
    }

    // iface.c:377
    public static void openFileForInput()
    {
        int err = 0;
        CPtr line = R.Buf(1025);
        CPtr s = R.Buf(1025);
        S.Finflows.file = R.fopen(S.Finflows.name, S.L13);
        if (S.Finflows.file == null)
        {
            S.report_writeErrorMsg(S.ERR_ROUTING_FILE_OPEN, S.Finflows.name);
            return;
        }
        R.fgets(line, 1024, S.Finflows.file);
        if ((!((R.sscanf(line, S.L169, s) != 0))) || (!((S.strcomp(s, S.L181) != 0))))
        {
            S.report_writeErrorMsg(S.ERR_ROUTING_FILE_FORMAT, S.Finflows.name);
            return;
        }
        R.fgets(line, 1024, S.Finflows.file);
        S.IfaceStep = 0;
        R.fgets(line, 1024, S.Finflows.file);
        if ((!((R.sscanf(line, S.L182, R.RefI(__v => S.IfaceStep = __v)) != 0))) || (S.IfaceStep <= 0))
        {
            S.report_writeErrorMsg(S.ERR_ROUTING_FILE_FORMAT, S.Finflows.name);
            return;
        }
        err = S.getIfaceFilePolluts();
        if (err > 0)
        {
            S.report_writeErrorMsg(err, S.Finflows.name);
            return;
        }
        err = S.getIfaceFileNodes();
        if (err > 0)
        {
            S.report_writeErrorMsg(err, S.Finflows.name);
            return;
        }
        S.OldIfaceValues = S.project_createMatrix(S.NumIfaceNodes, 1 + S.NumIfacePolluts);
        S.NewIfaceValues = S.project_createMatrix(S.NumIfaceNodes, 1 + S.NumIfacePolluts);
        if ((S.OldIfaceValues == null) || (S.NewIfaceValues == null))
        {
            S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
            return;
        }
        S.readNewIfaceValues();
        S.OldIfaceDate = S.NewIfaceDate;
    }

    // iface.c:450
    public static int getIfaceFilePolluts()
    {
        int i = 0;
        int j = 0;
        CPtr line = R.Buf(1025);
        CPtr s1 = R.Buf(1025);
        CPtr s2 = R.Buf(1025);
        R.fgets(line, 1024, S.Finflows.file);
        S.NumIfacePolluts = -1;
        if ((R.sscanf(line, S.L182, R.RefI(__v => S.NumIfacePolluts = __v)) != 0))
        {
            S.NumIfacePolluts--;
        }
        if (S.NumIfacePolluts < 0)
        {
            return S.ERR_ROUTING_FILE_FORMAT;
        }
        R.fgets(line, 1024, S.Finflows.file);
        if (R.sscanf(line, S.L44, s1, s2) < 2)
        {
            return S.ERR_ROUTING_FILE_FORMAT;
        }
        if (!((S.strcomp(s1, S.L183) != 0)))
        {
            return S.ERR_ROUTING_FILE_FORMAT;
        }
        S.IfaceFlowUnits = S.findmatch(s2, S.FlowUnitWords);
        if (S.IfaceFlowUnits < 0)
        {
            return S.ERR_ROUTING_FILE_FORMAT;
        }
        if (S.Nobjects[S.POLLUT] > 0)
        {
            S.IfacePolluts = new int[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            if (!(S.IfacePolluts != null))
            {
                return S.ERR_MEMORY;
            }
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                S.IfacePolluts[i] = -1;
            }
        }
        if ((S.NumIfacePolluts > 0) && (S.Nobjects[S.POLLUT] > 0))
        {
            for (i = 0; i < S.NumIfacePolluts; i++)
            {
                if ((R.feof(S.Finflows.file) != 0))
                {
                    return S.ERR_ROUTING_FILE_FORMAT;
                }
                R.fgets(line, 1024, S.Finflows.file);
                if (R.sscanf(line, S.L44, s1, s2) < 2)
                {
                    return S.ERR_ROUTING_FILE_FORMAT;
                }
                j = S.project_findObject(S.POLLUT, s1);
                if (j < 0)
                {
                    continue;
                }
                if (!((S.strcomp(s2, S.QualUnitsWords[S.Pollut[j].units]) != 0)))
                {
                    return S.ERR_ROUTING_FILE_NOMATCH;
                }
                S.IfacePolluts[j] = i;
            }
        }
        return 0;
    }

    // iface.c:510
    public static int getIfaceFileNodes()
    {
        int i = 0;
        CPtr line = R.Buf(1025);
        CPtr s = R.Buf(1025);
        R.fgets(line, 1024, S.Finflows.file);
        if ((!((R.sscanf(line, S.L182, R.RefI(__v => S.NumIfaceNodes = __v)) != 0))) || (S.NumIfaceNodes <= 0))
        {
            return S.ERR_ROUTING_FILE_FORMAT;
        }
        S.IfaceNodes = new int[(int)(unchecked((ulong)(S.NumIfaceNodes)))];
        if (!(S.IfaceNodes != null))
        {
            return S.ERR_MEMORY;
        }
        for (i = 0; i < S.NumIfaceNodes; i++)
        {
            if ((R.feof(S.Finflows.file) != 0))
            {
                return S.ERR_ROUTING_FILE_FORMAT;
            }
            S.IfaceNodes[i] = 0;
            R.fgets(line, 1024, S.Finflows.file);
            if (!((R.sscanf(line, S.L169, s) != 0)))
            {
                return S.ERR_ROUTING_FILE_FORMAT;
            }
            S.IfaceNodes[i] = S.project_findObject(S.NODE, s);
        }
        if ((R.feof(S.Finflows.file) != 0))
        {
            return S.ERR_ROUTING_FILE_FORMAT;
        }
        R.fgets(line, 1024, S.Finflows.file);
        return 0;
    }

    // iface.c:550
    public static void readNewIfaceValues()
    {
        int i = 0;
        int j = 0;
        CPtr s = CPtr.Null;
        int yr = 0;
        int mon = 0;
        int day = 0;
        int hr = 0;
        int min = 0;
        int sec = 0;
        CPtr line = R.Buf(1025);
        S.NewIfaceDate = -693594;
        for (i = 0; i < S.NumIfaceNodes; i++)
        {
            if ((R.feof(S.Finflows.file) != 0))
            {
                return;
            }
            R.fgets(line, 1024, S.Finflows.file);
            if ((R.strtok(line, S.L184)).IsNull)
            {
                return;
            }
            s = R.strtok(CPtr.Null, S.L184);
            if (s.IsNull)
            {
                return;
            }
            yr = R.atoi(s);
            s = R.strtok(CPtr.Null, S.L184);
            if (s.IsNull)
            {
                return;
            }
            mon = R.atoi(s);
            s = R.strtok(CPtr.Null, S.L184);
            if (s.IsNull)
            {
                return;
            }
            day = R.atoi(s);
            s = R.strtok(CPtr.Null, S.L184);
            if (s.IsNull)
            {
                return;
            }
            hr = R.atoi(s);
            s = R.strtok(CPtr.Null, S.L184);
            if (s.IsNull)
            {
                return;
            }
            min = R.atoi(s);
            s = R.strtok(CPtr.Null, S.L184);
            if (s.IsNull)
            {
                return;
            }
            sec = R.atoi(s);
            s = R.strtok(CPtr.Null, S.L184);
            if (s.IsNull)
            {
                return;
            }
            S.NewIfaceValues[i][0] = R.atof(s) / S.Qcf[S.IfaceFlowUnits];
            for (j = 1; j <= S.NumIfacePolluts; j++)
            {
                s = R.strtok(CPtr.Null, S.L184);
                if (s.IsNull)
                {
                    return;
                }
                S.NewIfaceValues[i][j] = R.atof(s);
            }
        }
        S.NewIfaceDate = S.datetime_encodeDate(yr, mon, day) + S.datetime_encodeTime(hr, min, sec);
    }

    // iface.c:613
    public static void setOldIfaceValues()
    {
        int i = 0;
        int j = 0;
        S.OldIfaceDate = S.NewIfaceDate;
        for (i = 0; i < S.NumIfaceNodes; i++)
        {
            for (j = 0; j < S.NumIfacePolluts + 1; j++)
            {
                S.OldIfaceValues[i][j] = S.NewIfaceValues[i][j];
            }
        }
    }

    // iface.c:633
    public static int isOutletNode(int i)
    {
        if (S.RouteModel == S.DW)
        {
            return ((S.Node[i].type == S.OUTFALL) ? 1 : 0);
        }
        else
        {
            return ((S.Node[i].degree == 0) ? 1 : 0);
        }
    }

}
}
