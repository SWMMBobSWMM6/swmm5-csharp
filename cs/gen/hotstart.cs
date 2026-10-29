// Generated from EPA SWMM 5.2.4 (hotstart.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int fileVersion = 0;

    // hotstart.c:72
    public static int hotstart_open()
    {
        if (!((S.openHotstartFile1() != 0)))
        {
            return 0;
        }
        if (!((S.openHotstartFile2() != 0)))
        {
            return 0;
        }
        return 1;
    }

    // hotstart.c:82
    public static void hotstart_close()
    {
        if ((S.Fhotstart2.file != null))
        {
            S.saveRunoff();
            S.saveRouting();
            R.fclose(S.Fhotstart2.file);
        }
    }

    // hotstart.c:94
    public static int openHotstartFile1()
    {
        int nSubcatch = 0;
        int nNodes = 0;
        int nLinks = 0;
        int nPollut = 0;
        int nLandUses = 0;
        int flowUnits = 0;
        CPtr fStamp = R.BufFrom(S.L160, 15);
        CPtr fileStamp = R.BufFrom(S.L160, 15);
        CPtr fStampx = R.BufFrom(S.L161, 16);
        CPtr fileStamp2 = R.BufFrom(S.L162, 16);
        CPtr fileStamp3 = R.BufFrom(S.L163, 16);
        CPtr fileStamp4 = R.BufFrom(S.L164, 16);
        if (S.Fhotstart1.mode != S.USE_FILE)
        {
            return 1;
        }
        if ((S.Fhotstart1.file = R.fopen(S.Fhotstart1.name, S.L165)) == null)
        {
            S.report_writeErrorMsg(S.ERR_HOTSTART_FILE_OPEN, S.Fhotstart1.name);
            return 0;
        }
        R.freadC(fStampx, 0, (int)(R.strlen(fileStamp2)), S.Fhotstart1.file);
        if (R.strcmp(fStampx, fileStamp4) == 0)
        {
            S.fileVersion = 4;
        }
        else if (R.strcmp(fStampx, fileStamp3) == 0)
        {
            S.fileVersion = 3;
        }
        else if (R.strcmp(fStampx, fileStamp2) == 0)
        {
            S.fileVersion = 2;
        }
        else
        {
            R.rewind(S.Fhotstart1.file);
            R.freadC(fStamp, 0, (int)(R.strlen(fileStamp)), S.Fhotstart1.file);
            if (R.strcmp(fStamp, fileStamp) != 0)
            {
                S.report_writeErrorMsg(S.ERR_HOTSTART_FILE_FORMAT, S.L0);
                return 0;
            }
            S.fileVersion = 1;
        }
        nSubcatch = -1;
        nNodes = -1;
        nLinks = -1;
        nPollut = -1;
        nLandUses = -1;
        flowUnits = -1;
        if (S.fileVersion >= 2)
        {
            R.freadI1(ref nSubcatch, S.Fhotstart1.file);
        }
        else
        {
            nSubcatch = S.Nobjects[S.SUBCATCH];
        }
        if (S.fileVersion >= 3)
        {
            R.freadI1(ref nLandUses, S.Fhotstart1.file);
        }
        else
        {
            nLandUses = S.Nobjects[S.LANDUSE];
        }
        R.freadI1(ref nNodes, S.Fhotstart1.file);
        R.freadI1(ref nLinks, S.Fhotstart1.file);
        R.freadI1(ref nPollut, S.Fhotstart1.file);
        R.freadI1(ref flowUnits, S.Fhotstart1.file);
        if ((((((nSubcatch != S.Nobjects[S.SUBCATCH]) || (nLandUses != S.Nobjects[S.LANDUSE])) || (nNodes != S.Nobjects[S.NODE])) || (nLinks != S.Nobjects[S.LINK])) || (nPollut != S.Nobjects[S.POLLUT])) || (flowUnits != S.FlowUnits))
        {
            S.report_writeErrorMsg(S.ERR_HOTSTART_FILE_FORMAT, S.L0);
            return 0;
        }
        if (S.fileVersion >= 3)
        {
            S.readRunoff();
        }
        S.readRouting();
        R.fclose(S.Fhotstart1.file);
        if ((S.ErrorCode != 0))
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    // hotstart.c:180
    public static int openHotstartFile2()
    {
        int nSubcatch = 0;
        int nLandUses = 0;
        int nNodes = 0;
        int nLinks = 0;
        int nPollut = 0;
        int flowUnits = 0;
        CPtr fileStamp = R.BufFrom(S.L164, 16);
        if (S.Fhotstart2.mode != S.SAVE_FILE)
        {
            return 1;
        }
        if ((S.Fhotstart2.file = R.fopen(S.Fhotstart2.name, S.L166)) == null)
        {
            S.report_writeErrorMsg(S.ERR_HOTSTART_FILE_OPEN, S.Fhotstart2.name);
            return 0;
        }
        nSubcatch = S.Nobjects[S.SUBCATCH];
        nLandUses = S.Nobjects[S.LANDUSE];
        nNodes = S.Nobjects[S.NODE];
        nLinks = S.Nobjects[S.LINK];
        nPollut = S.Nobjects[S.POLLUT];
        flowUnits = S.FlowUnits;
        R.fwriteC(fileStamp, 0, (int)(R.strlen(fileStamp)), S.Fhotstart2.file);
        R.fwriteI1(nSubcatch, S.Fhotstart2.file);
        R.fwriteI1(nLandUses, S.Fhotstart2.file);
        R.fwriteI1(nNodes, S.Fhotstart2.file);
        R.fwriteI1(nLinks, S.Fhotstart2.file);
        R.fwriteI1(nPollut, S.Fhotstart2.file);
        R.fwriteI1(flowUnits, S.Fhotstart2.file);
        return 1;
    }

    // hotstart.c:222
    public static void saveRouting()
    {
        int i = 0;
        int j = 0;
        float[] x = new float[3];
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            x[0] = (float)(S.Node[i].newDepth);
            x[1] = (float)(S.Node[i].newLatFlow);
            R.fwriteF(x, 0, (int)(unchecked((ulong)(2))), S.Fhotstart2.file);
            if (S.Node[i].type == S.STORAGE)
            {
                j = S.Node[i].subIndex;
                x[0] = (float)(S.Storage[j].hrt);
                R.fwriteF(x, 0, (int)(unchecked((ulong)(1))), S.Fhotstart2.file);
            }
            for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
            {
                x[0] = (float)(S.Node[i].newQual[j]);
                R.fwriteF(x, 0, (int)(unchecked((ulong)(1))), S.Fhotstart2.file);
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            x[0] = (float)(S.Link[i].newFlow);
            x[1] = (float)(S.Link[i].newDepth);
            x[2] = (float)(S.Link[i].setting);
            R.fwriteF(x, 0, (int)(unchecked((ulong)(3))), S.Fhotstart2.file);
            for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
            {
                x[0] = (float)(S.Link[i].newQual[j]);
                R.fwriteF(x, 0, (int)(unchecked((ulong)(1))), S.Fhotstart2.file);
            }
        }
    }

    // hotstart.c:267
    public static void readRouting()
    {
        int i = 0;
        int j = 0;
        float x = 0.0f;
        double[] xgw = new double[4];
        CFile f = S.Fhotstart1.file;
        if (S.fileVersion == 2)
        {
            xgw[2] = 0.0;
            xgw[3] = -10000000000.0;
            for (i = 0; i < S.Nobjects[S.SUBCATCH]; i++)
            {
                if (!((S.readFloat(ref x, f) != 0)))
                {
                    return;
                }
                xgw[0] = x;
                if (!((S.readFloat(ref x, f) != 0)))
                {
                    return;
                }
                xgw[1] = x;
                if (S.Subcatch[i].groundwater != null)
                {
                    S.gwater_setState(i, xgw);
                }
            }
        }
        for (i = 0; i < S.Nobjects[S.NODE]; i++)
        {
            if (!((S.readFloat(ref x, f) != 0)))
            {
                return;
            }
            S.Node[i].newDepth = x;
            if (!((S.readFloat(ref x, f) != 0)))
            {
                return;
            }
            S.Node[i].newLatFlow = x;
            if ((S.fileVersion >= 4) && (S.Node[i].type == S.STORAGE))
            {
                if (!((S.readFloat(ref x, f) != 0)))
                {
                    return;
                }
                j = S.Node[i].subIndex;
                S.Storage[j].hrt = x;
            }
            for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
            {
                if (!((S.readFloat(ref x, f) != 0)))
                {
                    return;
                }
                S.Node[i].newQual[j] = x;
            }
            if (S.fileVersion <= 2)
            {
                for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
                {
                    if (!((S.readFloat(ref x, f) != 0)))
                    {
                        return;
                    }
                }
            }
        }
        for (i = 0; i < S.Nobjects[S.LINK]; i++)
        {
            if (!((S.readFloat(ref x, f) != 0)))
            {
                return;
            }
            S.Link[i].newFlow = x;
            if (!((S.readFloat(ref x, f) != 0)))
            {
                return;
            }
            S.Link[i].newDepth = x;
            if (!((S.readFloat(ref x, f) != 0)))
            {
                return;
            }
            S.Link[i].setting = x;
            S.Link[i].targetSetting = x;
            S.link_setTargetSetting(i);
            S.link_setSetting(i, 0.0);
            for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
            {
                if (!((S.readFloat(ref x, f) != 0)))
                {
                    return;
                }
                S.Link[i].newQual[j] = x;
            }
        }
    }

    // hotstart.c:356
    public static void saveRunoff()
    {
        int i = 0;
        int j = 0;
        int k = 0;
        double[] x = new double[6];
        CFile f = S.Fhotstart2.file;
        for (i = 0; i < S.Nobjects[S.SUBCATCH]; i++)
        {
            for (j = 0; j < 3; j++)
            {
                x[j] = S.Subcatch[i].subArea[j].depth;
            }
            x[3] = S.Subcatch[i].newRunoff;
            R.fwriteD(x, 0, (int)(unchecked((ulong)(4))), f);
            for (j = 0; j < 6; j++)
            {
                x[j] = 0.0;
            }
            S.infil_getState(i, x);
            R.fwriteD(x, 0, (int)(unchecked((ulong)(6))), f);
            if (S.Subcatch[i].groundwater != null)
            {
                S.gwater_getState(i, x);
                R.fwriteD(x, 0, (int)(unchecked((ulong)(4))), f);
            }
            if (S.Subcatch[i].snowpack != null)
            {
                for (j = 0; j < 3; j++)
                {
                    S.snow_getState(i, j, x);
                    R.fwriteD(x, 0, (int)(unchecked((ulong)(5))), f);
                }
            }
            if (S.Nobjects[S.POLLUT] > 0)
            {
                for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
                {
                    x[0] = S.Subcatch[i].newQual[j];
                    R.fwriteD(x, 0, (int)(unchecked((ulong)(1))), f);
                }
                for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
                {
                    x[0] = S.Subcatch[i].pondedQual[j];
                    R.fwriteD(x, 0, (int)(unchecked((ulong)(1))), f);
                }
                for (k = 0; k < S.Nobjects[S.LANDUSE]; k++)
                {
                    for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
                    {
                        x[0] = S.Subcatch[i].landFactor[k].buildup[j];
                        R.fwriteD(x, 0, (int)(unchecked((ulong)(S.Nobjects[S.POLLUT]))), f);
                    }
                    x[0] = S.Subcatch[i].landFactor[k].lastSwept;
                    R.fwriteD(x, 0, (int)(unchecked((ulong)(1))), f);
                }
            }
        }
    }

    // hotstart.c:430
    public static void readRunoff()
    {
        int i = 0;
        int j = 0;
        int k = 0;
        double[] x = new double[6];
        CFile f = S.Fhotstart1.file;
        for (i = 0; i < S.Nobjects[S.SUBCATCH]; i++)
        {
            for (j = 0; j < 3; j++)
            {
                if (!((S.readDouble(ref S.Subcatch[i].subArea[j].depth, f) != 0)))
                {
                    return;
                }
            }
            if (!((S.readDouble(ref S.Subcatch[i].newRunoff, f) != 0)))
            {
                return;
            }
            for (j = 0; j < 6; j++)
            {
                if (!((S.readDouble(ref x[j], f) != 0)))
                {
                    return;
                }
            }
            S.infil_setState(i, x);
            if (S.Subcatch[i].groundwater != null)
            {
                for (j = 0; j < 4; j++)
                {
                    if (!((S.readDouble(ref x[j], f) != 0)))
                    {
                        return;
                    }
                }
                S.gwater_setState(i, x);
            }
            if (S.Subcatch[i].snowpack != null)
            {
                for (j = 0; j < 3; j++)
                {
                    for (k = 0; k < 5; k++)
                    {
                        if (!((S.readDouble(ref x[k], f) != 0)))
                        {
                            return;
                        }
                    }
                    S.snow_setState(i, j, x);
                }
            }
            if (S.Nobjects[S.POLLUT] > 0)
            {
                for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
                {
                    if (!((S.readDouble(ref S.Subcatch[i].newQual[j], f) != 0)))
                    {
                        return;
                    }
                }
                for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
                {
                    if (!((S.readDouble(ref S.Subcatch[i].pondedQual[j], f) != 0)))
                    {
                        return;
                    }
                }
                for (k = 0; k < S.Nobjects[S.LANDUSE]; k++)
                {
                    for (j = 0; j < S.Nobjects[S.POLLUT]; j++)
                    {
                        if (!((S.readDouble(ref S.Subcatch[i].landFactor[k].buildup[j], f) != 0)))
                        {
                            return;
                        }
                    }
                    if (!((S.readDouble(ref S.Subcatch[i].landFactor[k].lastSwept, f) != 0)))
                    {
                        return;
                    }
                }
            }
        }
    }

    // hotstart.c:499
    public static int readFloat(ref float x, CFile f)
    {
        R.freadF1(ref x, f);
        if ((x) != (x))
        {
            S.report_writeErrorMsg(S.ERR_HOTSTART_FILE_READ, S.L0);
            x = (float)(0.0);
            return 0;
        }
        return 1;
    }

    // hotstart.c:521
    public static int readDouble(ref double x, CFile f)
    {
        if ((R.feof(f) != 0))
        {
            x = 0.0;
            S.report_writeErrorMsg(S.ERR_HOTSTART_FILE_READ, S.L0);
            return 0;
        }
        R.freadD1(ref x, f);
        if ((x) != (x))
        {
            x = 0.0;
            return 0;
        }
        return 1;
    }

}
}
