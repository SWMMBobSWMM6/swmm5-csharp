// Generated from EPA SWMM 5.2.4 (lid.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static CPtr[] LidLayerWords = new CPtr[] { S.L991, S.L992, S.L742, S.L993, S.L994, S.L995, S.L996, CPtr.Null };
    public static CPtr[] LidTypeWords = new CPtr[] { S.L997, S.L998, S.L953, S.L999, S.L1000, S.L1001, S.L1002, S.L1003, CPtr.Null };
    public static TLidProc[] LidProcs = null;
    public static int LidCount = 0;
    public static LidGroup[] LidGroups = null;
    public static int GroupCount = 0;
    public static double lid_EvapRate = 0.0;
    public static double NativeInfil = 0.0;
    public static double lid_MaxNativeInfil = 0.0;
    public static int initLidRptFile_colCount = 14;
    public static CPtr[] initLidRptFile_head1 = new CPtr[] { S.L1022, S.L1023, S.L1024, S.L1024, S.L1025, S.L1026, S.L1027, S.L1028, S.L1025, S.L1029, S.L1025, S.L1026, S.L1027, S.L1030, CPtr.Null };
    public static CPtr[] initLidRptFile_head2 = new CPtr[] { S.L1022, S.L1031, S.L1032, S.L1033, S.L1034, S.L1035, S.L1035, S.L1036, S.L1037, S.L1038, S.L1039, S.L1039, S.L1040, S.L1041, CPtr.Null };
    public static CPtr[] initLidRptFile_units1 = new CPtr[] { S.L1042, S.L1043, S.L1044, S.L1044, S.L1044, S.L1044, S.L1044, S.L1044, S.L1044, S.L1044, S.L1045, S.L1045, S.L1046, S.L1047, CPtr.Null };
    public static CPtr[] initLidRptFile_units2 = new CPtr[] { S.L1042, S.L1043, S.L1048, S.L1048, S.L1048, S.L1048, S.L1048, S.L1048, S.L1048, S.L1048, S.L1049, S.L1049, S.L1046, S.L1050, CPtr.Null };
    public static CPtr initLidRptFile_line9 = R.BufFrom(S.L1051, 11);

    // lid.c:237
    public static void lid_create(int lidCount, int subcatchCount)
    {
        int j = 0;
        S.LidProcs = (null);
        S.LidGroups = (null);
        S.LidCount = lidCount;
        S.GroupCount = subcatchCount;
        if (S.GroupCount > 0)
        {
            S.LidGroups = new LidGroup[(int)(unchecked((ulong)(S.GroupCount)))];
            if (S.LidGroups == null)
            {
                S.ErrorCode = S.ERR_MEMORY;
                return;
            }
        }
        for (j = 0; j < S.GroupCount; j++)
        {
            S.LidGroups[j] = (null);
        }
        if (S.LidCount == 0)
        {
            return;
        }
        S.LidProcs = R.NewArray<TLidProc>((int)(unchecked((ulong)(S.LidCount))));
        if (S.LidProcs == null)
        {
            S.ErrorCode = S.ERR_MEMORY;
            return;
        }
        for (j = 0; j < S.LidCount; j++)
        {
            S.LidProcs[j].lidType = -1;
            S.LidProcs[j].surface.thickness = 0.0;
            S.LidProcs[j].surface.voidFrac = 1.0;
            S.LidProcs[j].surface.roughness = 0.0;
            S.LidProcs[j].surface.surfSlope = 0.0;
            S.LidProcs[j].pavement.thickness = 0.0;
            S.LidProcs[j].soil.thickness = 0.0;
            S.LidProcs[j].storage.thickness = 0.0;
            S.LidProcs[j].storage.kSat = 0.0;
            S.LidProcs[j].drain.coeff = 0.0;
            S.LidProcs[j].drain.offset = 0.0;
            S.LidProcs[j].drainMat.thickness = 0.0;
            S.LidProcs[j].drainMat.roughness = 0.0;
            S.LidProcs[j].drainRmvl = (null);
            S.LidProcs[j].drainRmvl = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            if (S.LidProcs[j].drainRmvl == null)
            {
                S.ErrorCode = S.ERR_MEMORY;
                return;
            }
        }
    }

    // lid.c:304
    public static void lid_delete()
    {
        int j = 0;
        for (j = 0; j < S.GroupCount; j++)
        {
            S.freeLidGroup(j);
        }
        {
            if ((S.LidGroups != null))
            {
                R.free(S.LidGroups);
                S.LidGroups = (null);
            }
        }
        ;
        for (j = 0; j < S.LidCount; j++)
        {
            if ((S.LidProcs[j].drainRmvl != null))
            {
                R.free(S.LidProcs[j].drainRmvl);
                S.LidProcs[j].drainRmvl = (null);
            }
        }
        ;
        {
            if ((S.LidProcs != null))
            {
                R.free(S.LidProcs);
                S.LidProcs = (null);
            }
        }
        ;
        S.GroupCount = 0;
        S.LidCount = 0;
    }

    // lid.c:322
    public static void freeLidGroup(int j)
    {
        LidGroup lidGroup = S.LidGroups[j];
        TLidList lidList = null;
        TLidUnit lidUnit = null;
        TLidList nextLidUnit = null;
        if (lidGroup == null)
        {
            return;
        }
        lidList = lidGroup.lidList;
        while ((lidList != null))
        {
            lidUnit = lidList.lidUnit;
            if ((lidUnit.rptFile != null))
            {
                if ((lidUnit.rptFile.file != null))
                {
                    R.fclose(lidUnit.rptFile.file);
                }
                R.free(lidUnit.rptFile);
            }
            nextLidUnit = lidList.nextLidUnit;
            R.free(lidUnit);
            R.free(lidList);
            lidList = nextLidUnit;
        }
        R.free(lidGroup);
        S.LidGroups[j] = (null);
    }

    // lid.c:355
    public static int lid_readProcParams(CPtr[] toks, int ntoks)
    {
        int j = 0;
        int m = 0;
        if (ntoks < 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.LID, toks[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, toks[0]);
        }
        if (S.LidProcs[j].ID.IsNull)
        {
            S.LidProcs[j].ID = S.project_findID(S.LID, toks[0]);
        }
        m = S.findmatch(toks[1], S.LidTypeWords);
        if (m >= 0)
        {
            S.LidProcs[j].lidType = m;
            return 0;
        }
        else
        {
            m = S.findmatch(toks[1], S.LidLayerWords);
        }
        switch (m)
        {
            case S.SURF:
                return S.readSurfaceData(j, toks, ntoks);
            case S.SOIL:
                return S.readSoilData(j, toks, ntoks);
            case S.STOR:
                return S.readStorageData(j, toks, ntoks);
            case S.PAVE:
                return S.readPavementData(j, toks, ntoks);
            case 5/*DRAIN*/:
                return S.readDrainData(j, toks, ntoks);
            case S.DRAINMAT:
                return S.readDrainMatData(j, toks, ntoks);
            case S.REMOVALS:
                return S.readRemovalsData(j, toks, ntoks);
        }
        return S.error_setInpError(S.ERR_KEYWORD, toks[1]);
    }

    // lid.c:415
    public static int lid_readGroupParams(CPtr[] toks, int ntoks)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        int n = 0;
        double[] x = new double[6];
        CPtr fname = CPtr.Null;
        int drainSubcatch = -1;
        int drainNode = -1;
        if (ntoks < 8)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        j = S.project_findObject(S.SUBCATCH, toks[0]);
        if (j < 0)
        {
            return S.error_setInpError(S.ERR_NAME, toks[0]);
        }
        k = S.project_findObject(S.LID, toks[1]);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_NAME, toks[1]);
        }
        n = R.atoi(toks[2]);
        if (n < 0)
        {
            return S.error_setInpError(S.ERR_NUMBER, toks[2]);
        }
        if (n == 0)
        {
            return 0;
        }
        for (i = 3; i <= 7; i++)
        {
            if ((!((S.getDouble(toks[i], ref x[i - 3]) != 0))) || (x[i - 3] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i]);
            }
        }
        for (i = 2; i <= 3; i++)
        {
            if (x[i] > 100.0)
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i + 3]);
            }
        }
        if ((ntoks >= 9) && (R.strcmp(toks[8], S.L275) != 0))
        {
            fname = toks[8];
        }
        if ((ntoks >= 10) && (R.strcmp(toks[9], S.L275) != 0))
        {
            drainSubcatch = S.project_findObject(S.SUBCATCH, toks[9]);
            if (drainSubcatch < 0)
            {
                drainNode = S.project_findObject(S.NODE, toks[9]);
                if (drainNode < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, toks[9]);
                }
            }
        }
        x[5] = 0.0;
        if (ntoks >= 11)
        {
            if (((!((S.getDouble(toks[10], ref x[5]) != 0))) || (x[5] < 0.0)) || (x[5] > 100.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[10]);
            }
        }
        return S.addLidUnit(j, k, n, x, fname, drainSubcatch, drainNode);
    }

    // lid.c:499
    public static int addLidUnit(int j, int k, int n, double[] x, CPtr fname, int drainSubcatch, int drainNode)
    {
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        lidGroup = S.LidGroups[j];
        if (!(lidGroup != null))
        {
            lidGroup = new LidGroup();
            if (!(lidGroup != null))
            {
                return S.error_setInpError(S.ERR_MEMORY, S.L0);
            }
            lidGroup.lidList = (null);
            S.LidGroups[j] = lidGroup;
        }
        lidUnit = new TLidUnit();
        if (!(lidUnit != null))
        {
            return S.error_setInpError(S.ERR_MEMORY, S.L0);
        }
        lidUnit.rptFile = (null);
        lidList = new TLidList();
        if (!(lidList != null))
        {
            R.free(lidUnit);
            return S.error_setInpError(S.ERR_MEMORY, S.L0);
        }
        lidList.lidUnit = lidUnit;
        lidList.nextLidUnit = lidGroup.lidList;
        lidGroup.lidList = lidList;
        lidUnit.lidIndex = k;
        lidUnit.number = n;
        lidUnit.area = x[0] / ((S.UCF(S.LENGTH)) * (S.UCF(S.LENGTH)));
        lidUnit.fullWidth = x[1] / S.UCF(S.LENGTH);
        lidUnit.initSat = x[2] / 100.0;
        lidUnit.fromImperv = x[3] / 100.0;
        lidUnit.toPerv = ((x[4] > 0.0) ? 1 : 0);
        lidUnit.fromPerv = x[5] / 100.0;
        lidUnit.drainSubcatch = drainSubcatch;
        lidUnit.drainNode = drainNode;
        if (!fname.IsNull)
        {
            if (!((S.createLidRptFile(lidUnit, fname) != 0)))
            {
                return S.error_setInpError(S.ERR_RPT_FILE, fname);
            }
        }
        return 0;
    }

    // lid.c:567
    public static int createLidRptFile(TLidUnit lidUnit, CPtr fname)
    {
        TLidRptFile rptFile = null;
        rptFile = new TLidRptFile();
        if (rptFile == null)
        {
            return 0;
        }
        lidUnit.rptFile = rptFile;
        rptFile.file = R.fopen(fname, S.L171);
        if (rptFile.file == null)
        {
            return 0;
        }
        return 1;
    }

    // lid.c:581
    public static int readSurfaceData(int j, CPtr[] toks, int ntoks)
    {
        int i = 0;
        double[] x = new double[5];
        if (ntoks < 7)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        for (i = 2; i < 7; i++)
        {
            if ((!((S.getDouble(toks[i], ref x[i - 2]) != 0))) || (x[i - 2] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i]);
            }
        }
        if (x[1] >= 1.0)
        {
            return S.error_setInpError(S.ERR_NUMBER, toks[3]);
        }
        if (x[0] == 0.0)
        {
            x[1] = 0.0;
        }
        S.LidProcs[j].surface.thickness = x[0] / S.UCF(S.RAINDEPTH);
        S.LidProcs[j].surface.voidFrac = 1.0 - x[1];
        S.LidProcs[j].surface.roughness = x[2];
        S.LidProcs[j].surface.surfSlope = x[3] / 100.0;
        S.LidProcs[j].surface.sideSlope = x[4];
        return 0;
    }

    // lid.c:616
    public static int readPavementData(int j, CPtr[] toks, int ntoks)
    {
        int i = 0;
        double[] x = new double[7];
        if (ntoks < 7)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        for (i = 2; i < 7; i++)
        {
            if ((!((S.getDouble(toks[i], ref x[i - 2]) != 0))) || (x[i - 2] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i]);
            }
        }
        x[5] = 0.0;
        if (ntoks > 7)
        {
            if ((!((S.getDouble(toks[7], ref x[5]) != 0))) || (x[5] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[7]);
            }
        }
        x[6] = 0.0;
        if (ntoks > 8)
        {
            if (((!((S.getDouble(toks[8], ref x[6]) != 0))) || (x[6] < 0.0)) || (x[6] > 1.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[8]);
            }
        }
        x[1] = x[1] / (x[1] + 1.0);
        S.LidProcs[j].pavement.thickness = x[0] / S.UCF(S.RAINDEPTH);
        S.LidProcs[j].pavement.voidFrac = x[1];
        S.LidProcs[j].pavement.impervFrac = x[2];
        S.LidProcs[j].pavement.kSat = x[3] / S.UCF(S.RAINFALL);
        S.LidProcs[j].pavement.clogFactor = x[4];
        S.LidProcs[j].pavement.regenDays = x[5];
        S.LidProcs[j].pavement.regenDegree = x[6];
        return 0;
    }

    // lid.c:669
    public static int readSoilData(int j, CPtr[] toks, int ntoks)
    {
        int i = 0;
        double[] x = new double[7];
        if (ntoks < 9)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        for (i = 2; i < 9; i++)
        {
            if ((!((S.getDouble(toks[i], ref x[i - 2]) != 0))) || (x[i - 2] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i]);
            }
        }
        S.LidProcs[j].soil.thickness = x[0] / S.UCF(S.RAINDEPTH);
        S.LidProcs[j].soil.porosity = x[1];
        S.LidProcs[j].soil.fieldCap = x[2];
        S.LidProcs[j].soil.wiltPoint = x[3];
        S.LidProcs[j].soil.kSat = x[4] / S.UCF(S.RAINFALL);
        S.LidProcs[j].soil.kSlope = x[5];
        S.LidProcs[j].soil.suction = x[6] / S.UCF(S.RAINDEPTH);
        return 0;
    }

    // lid.c:703
    public static int readStorageData(int j, CPtr[] toks, int ntoks)
    {
        int i = 0;
        int covered = 0;
        double[] x = new double[6];
        if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        for (i = 2; i < 6; i++)
        {
            if ((!((S.getDouble(toks[i], ref x[i - 2]) != 0))) || (x[i - 2] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i]);
            }
        }
        if (ntoks > 6)
        {
            if ((S.match(toks[6], S.L5) != 0))
            {
                covered = 1;
            }
        }
        x[1] = x[1] / (x[1] + 1.0);
        S.LidProcs[j].storage.thickness = x[0] / S.UCF(S.RAINDEPTH);
        S.LidProcs[j].storage.voidFrac = x[1];
        S.LidProcs[j].storage.kSat = x[2] / S.UCF(S.RAINFALL);
        S.LidProcs[j].storage.clogFactor = x[3];
        S.LidProcs[j].storage.covered = covered;
        return 0;
    }

    // lid.c:749
    public static int readDrainData(int j, CPtr[] toks, int ntoks)
    {
        int i = 0;
        double[] x = new double[6];
        if (ntoks < 6)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        for (i = 0; i < 6; i++)
        {
            x[i] = 0.0;
        }
        for (i = 2; i < 8; i++)
        {
            if ((ntoks > i) && (((!((S.getDouble(toks[i], ref x[i - 2]) != 0))) || (x[i - 2] < 0.0))))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i]);
            }
        }
        i = -1;
        if (ntoks >= 9)
        {
            i = S.project_findObject(S.CURVE, toks[8]);
            if (i < 0)
            {
                return S.error_setInpError(S.ERR_NAME, toks[8]);
            }
        }
        S.LidProcs[j].drain.coeff = x[0];
        S.LidProcs[j].drain.expon = x[1];
        S.LidProcs[j].drain.offset = x[2] / S.UCF(S.RAINDEPTH);
        S.LidProcs[j].drain.delay = x[3] * 3600.0;
        S.LidProcs[j].drain.hOpen = x[4] / S.UCF(S.RAINDEPTH);
        S.LidProcs[j].drain.hClose = x[5] / S.UCF(S.RAINDEPTH);
        S.LidProcs[j].drain.qCurve = i;
        return 0;
    }

    // lid.c:794
    public static int readDrainMatData(int j, CPtr[] toks, int ntoks)
    {
        int i = 0;
        double[] x = new double[3];
        if (ntoks < 5)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        if (S.LidProcs[j].lidType != S.GREEN_ROOF)
        {
            return 0;
        }
        for (i = 2; i < 5; i++)
        {
            if ((!((S.getDouble(toks[i], ref x[i - 2]) != 0))) || (x[i - 2] < 0.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i]);
            }
        }
        S.LidProcs[j].drainMat.thickness = x[0] / S.UCF(S.RAINDEPTH);
        ;
        S.LidProcs[j].drainMat.voidFrac = x[1];
        S.LidProcs[j].drainMat.roughness = x[2];
        return 0;
    }

    // lid.c:828
    public static int readRemovalsData(int j, CPtr[] toks, int ntoks)
    {
        int i = 2;
        int p = 0;
        double rmvl = 0.0;
        if (ntoks < 4)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        while (ntoks > i)
        {
            p = S.project_findObject(S.POLLUT, toks[i]);
            if (p < 0)
            {
                return S.error_setInpError(S.ERR_NAME, toks[i]);
            }
            i++;
            if (ntoks == i)
            {
                return S.error_setInpError(S.ERR_ITEMS, S.L0);
            }
            if (((!((S.getDouble(toks[i], ref rmvl) != 0))) || (rmvl < 0.0)) || (rmvl > 100.0))
            {
                return S.error_setInpError(S.ERR_NUMBER, toks[i]);
            }
            S.LidProcs[j].drainRmvl[p] = rmvl / 100.0;
            i++;
        }
        return 0;
    }

    // lid.c:869
    public static void lid_writeSummary()
    {
        int j = 0;
        int k = 0;
        double pctArea = 0.0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        R.fprintf(S.Frpt.file, S.L276);
        R.fprintf(S.Frpt.file, S.L276);
        R.fprintf(S.Frpt.file, S.L277);
        R.fprintf(S.Frpt.file, S.L278);
        R.fprintf(S.Frpt.file, S.L277);
        R.fprintf(S.Frpt.file, S.L279);
        R.fprintf(S.Frpt.file, S.L280);
        R.fprintf(S.Frpt.file, S.L281);
        for (j = 0; j < S.GroupCount; j++)
        {
            lidGroup = S.LidGroups[j];
            if (lidGroup == null)
            {
                continue;
            }
            lidList = lidGroup.lidList;
            while ((lidList != null))
            {
                lidUnit = lidList.lidUnit;
                k = lidUnit.lidIndex;
                pctArea = lidUnit.area * lidUnit.number / S.Subcatch[j].area * 100.0;
                R.fprintf(S.Frpt.file, S.L282, S.Subcatch[j].ID, S.LidProcs[k].ID);
                R.fprintf(S.Frpt.file, S.L283, lidUnit.number, lidUnit.area * ((S.UCF(S.LENGTH)) * (S.UCF(S.LENGTH))), lidUnit.fullWidth * S.UCF(S.LENGTH), pctArea, lidUnit.fromImperv * 100.0, lidUnit.fromPerv * 100.0);
                lidList = lidList.nextLidUnit;
            }
        }
    }

    // lid.c:918
    public static void lid_validate()
    {
        int j = 0;
        for (j = 0; j < S.LidCount; j++)
        {
            S.validateLidProc(j);
        }
        for (j = 0; j < S.GroupCount; j++)
        {
            S.validateLidGroup(j);
        }
    }

    // lid.c:932
    public static void validateLidProc(int j)
    {
        int layerMissing = 0;
        if (S.LidProcs[j].lidType < 0)
        {
            S.report_writeErrorMsg(S.ERR_LID_TYPE, S.LidProcs[j].ID);
            return;
        }
        switch (S.LidProcs[j].lidType)
        {
            case S.BIO_CELL:
            case S.RAIN_GARDEN:
                if (S.LidProcs[j].soil.thickness <= 0.0)
                {
                    layerMissing = 1;
                }
                break;
            case S.GREEN_ROOF:
                if (S.LidProcs[j].soil.thickness <= 0.0)
                {
                    layerMissing = 1;
                }
                if (S.LidProcs[j].drainMat.thickness <= 0.0)
                {
                    layerMissing = 1;
                }
                break;
            case S.POROUS_PAVEMENT:
                if (S.LidProcs[j].pavement.thickness <= 0.0)
                {
                    layerMissing = 1;
                }
                break;
            case S.INFIL_TRENCH:
                if (S.LidProcs[j].storage.thickness <= 0.0)
                {
                    layerMissing = 1;
                }
                break;
        }
        if ((layerMissing != 0))
        {
            S.report_writeErrorMsg(S.ERR_LID_LAYER, S.LidProcs[j].ID);
            return;
        }
        if (S.LidProcs[j].lidType == S.POROUS_PAVEMENT)
        {
            if (((((S.LidProcs[j].pavement.thickness <= 0.0) || (S.LidProcs[j].pavement.kSat <= 0.0)) || (S.LidProcs[j].pavement.voidFrac <= 0.0)) || (S.LidProcs[j].pavement.voidFrac > 1.0)) || (S.LidProcs[j].pavement.impervFrac > 1.0))
            {
                S.sstrncpy(S.Msg, S.LidProcs[j].ID, unchecked((ulong)(1024)));
                S.sstrcat(S.Msg, S.L284, unchecked((ulong)(1024)));
                S.report_writeErrorMsg(S.ERR_LID_PARAMS, S.Msg);
            }
        }
        if (S.LidProcs[j].soil.thickness > 0.0)
        {
            if (((((S.LidProcs[j].soil.porosity <= 0.0) || (S.LidProcs[j].soil.fieldCap >= S.LidProcs[j].soil.porosity)) || (S.LidProcs[j].soil.wiltPoint >= S.LidProcs[j].soil.fieldCap)) || (S.LidProcs[j].soil.kSat <= 0.0)) || (S.LidProcs[j].soil.kSlope < 0.0))
            {
                S.sstrncpy(S.Msg, S.LidProcs[j].ID, unchecked((ulong)(1024)));
                S.sstrcat(S.Msg, S.L285, unchecked((ulong)(1024)));
                S.report_writeErrorMsg(S.ERR_LID_PARAMS, S.Msg);
            }
        }
        if (S.LidProcs[j].storage.thickness > 0.0)
        {
            if ((S.LidProcs[j].storage.voidFrac <= 0.0) || (S.LidProcs[j].storage.voidFrac > 1.0))
            {
                S.sstrncpy(S.Msg, S.LidProcs[j].ID, unchecked((ulong)(1024)));
                S.sstrcat(S.Msg, S.L286, unchecked((ulong)(1024)));
                S.report_writeErrorMsg(S.ERR_LID_PARAMS, S.Msg);
            }
        }
        else
        {
            S.LidProcs[j].storage.voidFrac = 1.0;
            S.LidProcs[j].drain.offset = 0.0;
        }
        if ((S.LidProcs[j].drain.hOpen > 0.0) && (S.LidProcs[j].drain.hOpen <= S.LidProcs[j].drain.hClose))
        {
            S.sstrncpy(S.Msg, S.LidProcs[j].ID, unchecked((ulong)(1024)));
            S.sstrcat(S.Msg, S.L287, unchecked((ulong)(1024)));
            S.report_writeErrorMsg(S.ERR_LID_PARAMS, S.Msg);
        }
        if (S.LidProcs[j].lidType == S.VEG_SWALE)
        {
            if ((S.LidProcs[j].surface.roughness * S.LidProcs[j].surface.surfSlope <= 0.0) || (S.LidProcs[j].surface.thickness == 0.0))
            {
                S.sstrncpy(S.Msg, S.LidProcs[j].ID, unchecked((ulong)(1024)));
                S.sstrcat(S.Msg, S.L288, unchecked((ulong)(1024)));
                S.report_writeErrorMsg(S.ERR_LID_PARAMS, S.Msg);
            }
            else
            {
                S.LidProcs[j].surface.alpha = 1.49 * Math.Sqrt(S.LidProcs[j].surface.surfSlope) / S.LidProcs[j].surface.roughness;
            }
        }
        else
        {
            if (S.LidProcs[j].surface.roughness > 0.0)
            {
                S.LidProcs[j].surface.alpha = 1.49 / S.LidProcs[j].surface.roughness * Math.Sqrt(S.LidProcs[j].surface.surfSlope);
            }
            else
            {
                S.LidProcs[j].surface.alpha = 0.0;
            }
        }
        if (S.LidProcs[j].drainMat.roughness > 0.0)
        {
            S.LidProcs[j].drainMat.alpha = 1.49 / S.LidProcs[j].drainMat.roughness * Math.Sqrt(S.LidProcs[j].surface.surfSlope);
        }
        else
        {
            S.LidProcs[j].drainMat.alpha = 0.0;
        }
        if (S.LidProcs[j].pavement.thickness > 0.0)
        {
            S.LidProcs[j].pavement.clogFactor *= S.LidProcs[j].pavement.thickness * S.LidProcs[j].pavement.voidFrac * (1.0 - S.LidProcs[j].pavement.impervFrac);
        }
        if (S.LidProcs[j].storage.thickness > 0.0)
        {
            S.LidProcs[j].storage.clogFactor *= S.LidProcs[j].storage.thickness * S.LidProcs[j].storage.voidFrac;
        }
        else
        {
            S.LidProcs[j].storage.clogFactor = 0.0;
        }
        S.LidProcs[j].surface.canOverflow = 1;
        switch (S.LidProcs[j].lidType)
        {
            case S.ROOF_DISCON:
                S.LidProcs[j].surface.canOverflow = 0;
                break;
            case S.INFIL_TRENCH:
            case S.POROUS_PAVEMENT:
            case S.BIO_CELL:
            case S.RAIN_GARDEN:
            case S.GREEN_ROOF:
                if (S.LidProcs[j].surface.alpha > 0.0)
                {
                    S.LidProcs[j].surface.canOverflow = 0;
                }
                break;
        }
        if (S.LidProcs[j].lidType == S.RAIN_BARREL)
        {
            S.LidProcs[j].storage.voidFrac = 1.0;
            S.LidProcs[j].storage.kSat = 0.0;
        }
        if (S.LidProcs[j].lidType == S.GREEN_ROOF)
        {
            S.LidProcs[j].storage.thickness = S.LidProcs[j].drainMat.thickness;
            S.LidProcs[j].storage.voidFrac = S.LidProcs[j].drainMat.voidFrac;
            S.LidProcs[j].storage.clogFactor = 0.0;
            S.LidProcs[j].storage.kSat = 0.0;
        }
    }

    // lid.c:1113
    public static void validateLidGroup(int j)
    {
        int k = 0;
        double[] p = new double[3];
        double totalArea = S.Subcatch[j].area;
        double totalLidArea = 0.0;
        double fromImperv = 0.0;
        double fromPerv = 0.0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        lidGroup = S.LidGroups[j];
        if (lidGroup == null)
        {
            return;
        }
        lidList = lidGroup.lidList;
        while ((lidList != null))
        {
            lidUnit = lidList.lidUnit;
            k = lidUnit.lidIndex;
            totalLidArea += (lidUnit.area * lidUnit.number);
            fromImperv += lidUnit.fromImperv;
            fromPerv += lidUnit.fromPerv;
            lidUnit.soilInfil.Ks = 0.0;
            if (S.LidProcs[k].soil.thickness > 0.0)
            {
                p[0] = S.LidProcs[k].soil.suction * S.UCF(S.RAINDEPTH);
                p[1] = S.LidProcs[k].soil.kSat * S.UCF(S.RAINFALL);
                p[2] = (S.LidProcs[k].soil.porosity - S.LidProcs[k].soil.wiltPoint) * (1.0 - lidUnit.initSat);
                if (S.grnampt_setParams(lidUnit.soilInfil, p) == 0)
                {
                    S.sstrncpy(S.Msg, S.LidProcs[k].ID, unchecked((ulong)(1024)));
                    S.sstrcat(S.Msg, S.L285, unchecked((ulong)(1024)));
                    S.report_writeErrorMsg(S.ERR_LID_PARAMS, S.Msg);
                }
            }
            if (S.LidProcs[k].lidType == S.VEG_SWALE)
            {
                if ((S.Subcatch[j].infilModel == S.GREEN_AMPT) || (S.Subcatch[j].infilModel == S.MOD_GREEN_AMPT))
                {
                    S.grnampt_getParams(j, p);
                    if (S.grnampt_setParams(lidUnit.soilInfil, p) == 0)
                    {
                        S.sstrncpy(S.Msg, S.LidProcs[k].ID, unchecked((ulong)(1024)));
                        S.sstrcat(S.Msg, S.L289, unchecked((ulong)(1024)));
                        S.report_writeErrorMsg(S.ERR_LID_PARAMS, S.Msg);
                    }
                }
                if (lidUnit.fullWidth <= 0.0)
                {
                    S.sstrncpy(S.Msg, S.LidProcs[k].ID, unchecked((ulong)(1024)));
                    S.sstrcat(S.Msg, S.L290, unchecked((ulong)(1024)));
                    S.report_writeErrorMsg(S.ERR_LID_PARAMS, S.Msg);
                }
            }
            if (S.Subcatch[j].fracImperv >= 0.999)
            {
                lidUnit.toPerv = 0;
            }
            if ((lidUnit.drainNode == -1) && (lidUnit.drainSubcatch == -1))
            {
                lidUnit.drainNode = S.Subcatch[j].outNode;
                lidUnit.drainSubcatch = S.Subcatch[j].outSubcatch;
            }
            lidList = lidList.nextLidUnit;
        }
        if (totalLidArea > 1.001 * totalArea)
        {
            S.report_writeErrorMsg(S.ERR_LID_AREAS, S.Subcatch[j].ID);
        }
        if ((fromImperv > 1.001) || (fromPerv > 1.001))
        {
            S.report_writeErrorMsg(S.ERR_LID_CAPTURE_AREA, S.Subcatch[j].ID);
        }
        if (totalLidArea > 0.999 * totalArea)
        {
            totalLidArea = totalArea;
        }
        S.Subcatch[j].lidArea = totalLidArea;
    }

    // lid.c:1211
    public static void lid_initState()
    {
        int i = 0;
        int j = 0;
        int k = 0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        double initVol = 0.0;
        double initDryTime = S.StartDryDays * 86400;
        S.HasWetLids = 0;
        for (j = 0; j < S.GroupCount; j++)
        {
            lidGroup = S.LidGroups[j];
            if (lidGroup == null)
            {
                continue;
            }
            lidGroup.pervArea = 0.0;
            lidGroup.flowToPerv = 0.0;
            lidGroup.oldDrainFlow = 0.0;
            lidGroup.newDrainFlow = 0.0;
            lidList = lidGroup.lidList;
            while ((lidList != null))
            {
                lidUnit = lidList.lidUnit;
                k = lidUnit.lidIndex;
                lidUnit.surfaceDepth = 0.0;
                lidUnit.storageDepth = 0.0;
                lidUnit.soilMoisture = 0.0;
                lidUnit.paveDepth = 0.0;
                lidUnit.dryTime = initDryTime;
                lidUnit.volTreated = 0.0;
                lidUnit.nextRegenDay = S.LidProcs[k].pavement.regenDays;
                initVol = 0.0;
                if (S.LidProcs[k].soil.thickness > 0.0)
                {
                    lidUnit.soilMoisture = S.LidProcs[k].soil.wiltPoint + lidUnit.initSat * (S.LidProcs[k].soil.porosity - S.LidProcs[k].soil.wiltPoint);
                    initVol += lidUnit.soilMoisture * S.LidProcs[k].soil.thickness;
                }
                if (S.LidProcs[k].storage.thickness > 0.0)
                {
                    lidUnit.storageDepth = lidUnit.initSat * S.LidProcs[k].storage.thickness;
                    initVol += lidUnit.storageDepth * S.LidProcs[k].storage.voidFrac;
                }
                if (lidUnit.initSat > 0.0)
                {
                    S.HasWetLids = 1;
                }
                S.lidproc_initWaterBalance(lidUnit, initVol);
                lidUnit.volTreated = 0.0;
                if ((lidUnit.rptFile != null))
                {
                    S.initLidRptFile(S.Title[0], S.LidProcs[k].ID, S.Subcatch[j].ID, lidUnit);
                }
                lidUnit.oldDrainFlow = 0.0;
                lidUnit.newDrainFlow = 0.0;
                for (i = 0; i < 4; i++)
                {
                    lidUnit.oldFluxRates[i] = 0.0;
                }
                if (lidUnit.soilInfil.Ks > 0.0)
                {
                    S.grnampt_initState(lidUnit.soilInfil);
                }
                if ((S.isLidPervious(lidUnit.lidIndex) != 0))
                {
                    lidGroup.pervArea += (lidUnit.area * lidUnit.number);
                }
                lidList = lidList.nextLidUnit;
            }
        }
    }

    // lid.c:1302
    public static void lid_setOldGroupState(int j)
    {
        TLidList lidList = null;
        if (S.LidGroups[j] != null)
        {
            S.LidGroups[j].oldDrainFlow = S.LidGroups[j].newDrainFlow;
            S.LidGroups[j].newDrainFlow = 0.0;
            lidList = S.LidGroups[j].lidList;
            while ((lidList != null))
            {
                lidList.lidUnit.oldDrainFlow = lidList.lidUnit.newDrainFlow;
                lidList.lidUnit.newDrainFlow = 0.0;
                lidList = lidList.nextLidUnit;
            }
        }
    }

    // lid.c:1326
    public static int isLidPervious(int k)
    {
        return (((S.LidProcs[k].storage.thickness == 0.0) || (S.LidProcs[k].storage.kSat > 0.0)) ? 1 : 0);
    }

    // lid.c:1339
    public static double getSurfaceDepth(int j)
    {
        int k = 0;
        double depth = 0.0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        lidGroup = S.LidGroups[j];
        if (lidGroup == null)
        {
            return 0.0;
        }
        if (S.Subcatch[j].lidArea == 0.0)
        {
            return 0.0;
        }
        lidList = lidGroup.lidList;
        while ((lidList != null))
        {
            lidUnit = lidList.lidUnit;
            k = lidUnit.lidIndex;
            depth += lidUnit.surfaceDepth * S.LidProcs[k].surface.voidFrac * lidUnit.area * lidUnit.number;
            lidList = lidList.nextLidUnit;
        }
        return depth / S.Subcatch[j].lidArea;
    }

    // lid.c:1370
    public static double lid_getPervArea(int j)
    {
        if ((S.LidGroups[j] != null))
        {
            return S.LidGroups[j].pervArea;
        }
        else
        {
            return 0.0;
        }
    }

    // lid.c:1383
    public static double lid_getFlowToPerv(int j)
    {
        if (S.LidGroups[j] != null)
        {
            return S.LidGroups[j].flowToPerv;
        }
        return 0.0;
    }

    // lid.c:1397
    public static double lid_getStoredVolume(int j)
    {
        double total = 0.0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        lidGroup = S.LidGroups[j];
        if ((lidGroup == null) || (S.Subcatch[j].lidArea == 0.0))
        {
            return 0.0;
        }
        lidList = lidGroup.lidList;
        while ((lidList != null))
        {
            lidUnit = lidList.lidUnit;
            total += lidUnit.waterBalance.finalVol * lidUnit.area * lidUnit.number;
            lidList = lidList.nextLidUnit;
        }
        return total;
    }

    // lid.c:1424
    public static double lid_getDrainFlow(int j, int timePeriod)
    {
        if (S.LidGroups[j] != null)
        {
            if (timePeriod == S.PREVIOUS)
            {
                return S.LidGroups[j].oldDrainFlow;
            }
            else
            {
                return S.LidGroups[j].newDrainFlow;
            }
        }
        return 0.0;
    }

    // lid.c:1442
    public static void lid_addDrainLoads(int j, double[] c, double tStep)
    {
        int isRunoffLoad = 0;
        int p = 0;
        double r = 0.0;
        double w = 0.0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        lidGroup = S.LidGroups[j];
        if (lidGroup != null)
        {
            lidList = lidGroup.lidList;
            while ((lidList != null))
            {
                lidUnit = lidList.lidUnit;
                isRunoffLoad = (((lidUnit.drainNode >= 0) || (lidUnit.drainSubcatch == j)) ? 1 : 0);
                if (!(lidUnit.toPerv != 0))
                {
                    for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                    {
                        w = lidUnit.newDrainFlow * c[p] * tStep * 28.317 * S.Pollut[p].mcf;
                        r = S.LidProcs[lidUnit.lidIndex].drainRmvl[p];
                        S.massbal_updateLoadingTotals(S.BMP_REMOVAL_LOAD, p, r * w);
                        if ((isRunoffLoad != 0))
                        {
                            S.massbal_updateLoadingTotals(S.RUNOFF_LOAD, p, w * (1.0 - r));
                        }
                    }
                }
                lidList = lidList.nextLidUnit;
            }
        }
    }

    // lid.c:1497
    public static void lid_addDrainRunon(int j)
    {
        int i = 0;
        int k = 0;
        int p = 0;
        double q = 0.0;
        double w = 0.0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        lidGroup = S.LidGroups[j];
        if (lidGroup != null)
        {
            lidList = lidGroup.lidList;
            while ((lidList != null))
            {
                lidUnit = lidList.lidUnit;
                i = lidUnit.lidIndex;
                k = lidUnit.drainSubcatch;
                if ((k >= 0) && (k != j))
                {
                    q = lidUnit.oldDrainFlow;
                    S.subcatch_addRunonFlow(k, q);
                    for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                    {
                        w = q * S.Subcatch[j].oldQual[p] * 28.317;
                        w = w * (1.0 - S.LidProcs[i].drainRmvl[p]);
                        S.Subcatch[k].newQual[p] += w;
                    }
                }
                lidList = lidList.nextLidUnit;
            }
        }
    }

    // lid.c:1549
    public static void lid_addDrainInflow(int j, double f)
    {
        int i = 0;
        int k = 0;
        int p = 0;
        double q = 0.0;
        double w = 0.0;
        double w1 = 0.0;
        double w2 = 0.0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        lidGroup = S.LidGroups[j];
        if (lidGroup != null)
        {
            lidList = lidGroup.lidList;
            while ((lidList != null))
            {
                lidUnit = lidList.lidUnit;
                i = lidUnit.lidIndex;
                k = lidUnit.drainNode;
                if (k >= 0)
                {
                    q = (1.0 - f) * lidUnit.oldDrainFlow + f * lidUnit.newDrainFlow;
                    S.Node[k].newLatFlow += q;
                    S.massbal_addInflowFlow(S.WET_WEATHER_INFLOW, q);
                    for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                    {
                        w1 = lidUnit.oldDrainFlow * S.Subcatch[j].oldQual[p];
                        w2 = lidUnit.newDrainFlow * S.Subcatch[j].newQual[p];
                        w = (1.0 - f) * w1 + f * w2;
                        w = w * (1.0 - S.LidProcs[i].drainRmvl[p]);
                        S.Node[k].newQual[p] += w;
                        S.massbal_addInflowQual(S.WET_WEATHER_INFLOW, p, w);
                    }
                }
                lidList = lidList.nextLidUnit;
            }
        }
    }

    // lid.c:1609
    public static void lid_getRunoff(int j, double tStep)
    {
        LidGroup theLidGroup = null;
        TLidList lidList = null;
        TLidUnit lidUnit = null;
        double lidArea = 0.0;
        double qImperv = 0.0;
        double qPerv = 0.0;
        double lidInflow = 0.0;
        double qRunoff = 0.0;
        double qDrain = 0.0;
        double qReturn = 0.0;
        theLidGroup = S.LidGroups[j];
        if (!(theLidGroup != null))
        {
            return;
        }
        lidList = theLidGroup.lidList;
        if (!(lidList != null))
        {
            return;
        }
        S.lid_EvapRate = S.Evap.rate;
        if ((S.Evap.dryOnly != 0) && (S.Subcatch[j].rainfall > 0.0))
        {
            S.lid_EvapRate = 0.0;
        }
        S.findNativeInfil(j, tStep);
        if (S.Subcatch[j].area > S.Subcatch[j].lidArea)
        {
            qImperv = S.getImpervAreaRunoff(j);
            qPerv = S.getPervAreaRunoff(j);
        }
        while ((lidList != null))
        {
            lidUnit = lidList.lidUnit;
            lidArea = lidUnit.area * lidUnit.number;
            if (lidArea > 0.0)
            {
                lidInflow = (qImperv * lidUnit.fromImperv + qPerv * lidUnit.fromPerv) / lidArea;
                S.VlidIn += lidInflow * lidArea * tStep;
                lidInflow = lidInflow + S.getRainInflow(j, lidUnit);
                if (S.Subcatch[j].area == S.Subcatch[j].lidArea)
                {
                    lidInflow += S.Subcatch[j].runon;
                }
                S.evalLidUnit(j, lidUnit, lidArea, lidInflow, tStep, ref qRunoff, ref qDrain, ref qReturn);
            }
            lidList = lidList.nextLidUnit;
        }
        theLidGroup.newDrainFlow = qDrain;
        theLidGroup.flowToPerv = qReturn;
        S.VlidOut = qRunoff * tStep;
        S.VlidDrain = qDrain * tStep;
        S.VlidReturn = qReturn * tStep;
    }

    // lid.c:1697
    public static void findNativeInfil(int j, double tStep)
    {
        double nonLidArea = 0.0;
        nonLidArea = S.Subcatch[j].area - S.Subcatch[j].lidArea;
        if ((nonLidArea > 0.0) && (S.Subcatch[j].fracImperv < 1.0))
        {
            S.NativeInfil = S.Vinfil / nonLidArea / tStep;
        }
        else
        {
            S.NativeInfil = S.infil_getInfil(j, tStep, S.Subcatch[j].rainfall, S.Subcatch[j].runon, S.getSurfaceDepth(j));
        }
        if ((!(S.IgnoreGwater != 0)) && (S.Subcatch[j].groundwater != null))
        {
            S.lid_MaxNativeInfil = S.Subcatch[j].groundwater.maxInfilVol / tStep;
        }
        else
        {
            S.lid_MaxNativeInfil = 10000000000.0;
        }
    }

    // lid.c:1734
    public static double getRainInflow(int j, TLidUnit lidUnit)
    {
        TLidProc lidProc = S.LidProcs[lidUnit.lidIndex];
        if ((lidProc.lidType == S.RAIN_BARREL) && (lidProc.storage.covered == 1))
        {
            return 0.0;
        }
        return S.Subcatch[j].rainfall;
    }

    // lid.c:1751
    public static double getImpervAreaRunoff(int j)
    {
        int i = 0;
        double q = 0.0;
        double nonLidArea = 0.0;
        for (i = S.IMPERV0; i <= S.IMPERV1; i++)
        {
            q += S.Subcatch[j].subArea[i].runoff * S.Subcatch[j].subArea[i].fArea;
        }
        if ((S.Subcatch[j].subArea[S.IMPERV0].routeTo == S.TO_PERV) && (S.Subcatch[j].fracImperv < 1.0))
        {
            q *= S.Subcatch[j].subArea[S.IMPERV0].fOutlet;
        }
        nonLidArea = S.Subcatch[j].area - S.Subcatch[j].lidArea;
        return q * nonLidArea;
    }

    // lid.c:1781
    public static double getPervAreaRunoff(int j)
    {
        double q = 0.0;
        double nonLidArea = 0.0;
        q = S.Subcatch[j].subArea[S.PERV].runoff * S.Subcatch[j].subArea[S.PERV].fArea;
        if ((S.Subcatch[j].subArea[S.PERV].routeTo == S.TO_IMPERV) && (S.Subcatch[j].fracImperv > 0.0))
        {
            q *= S.Subcatch[j].subArea[S.PERV].fOutlet;
        }
        nonLidArea = S.Subcatch[j].area - S.Subcatch[j].lidArea;
        return q * nonLidArea;
    }

    // lid.c:1807
    public static void evalLidUnit(int j, TLidUnit lidUnit, double lidArea, double lidInflow, double tStep, ref double qRunoff, ref double qDrain, ref double qReturn)
    {
        TLidProc lidProc = null;
        double lidRunoff = 0.0;
        double lidEvap = 0.0;
        double lidInfil = 0.0;
        double lidDrain = 0.0;
        lidProc = S.LidProcs[lidUnit.lidIndex];
        lidEvap = 0.0;
        lidInfil = 0.0;
        lidRunoff = S.lidproc_getOutflow(lidUnit, lidProc, lidInflow, S.lid_EvapRate, S.NativeInfil, S.lid_MaxNativeInfil, tStep, ref lidEvap, ref lidInfil, ref lidDrain) * lidArea;
        lidDrain *= lidArea;
        if ((lidUnit.toPerv != 0) && (S.Subcatch[j].area > S.Subcatch[j].lidArea))
        {
            qReturn += lidRunoff;
            lidRunoff = 0.0;
            if ((lidUnit.drainNode == S.Subcatch[j].outNode) && (lidUnit.drainSubcatch == S.Subcatch[j].outSubcatch))
            {
                qReturn += lidDrain;
                lidDrain = 0.0;
            }
        }
        if (lidUnit.drainNode >= 0)
        {
            S.massbal_updateRunoffTotals(S.RUNOFF_DRAINS, lidDrain * tStep);
        }
        lidUnit.newDrainFlow = lidDrain;
        S.Vevap += lidEvap * tStep * lidArea;
        S.VlidInfil += lidInfil * tStep * lidArea;
        if ((S.isLidPervious(lidUnit.lidIndex) != 0))
        {
            S.Vpevap += lidEvap * tStep * lidArea;
        }
        if (S.Subcatch[j].rainfall > 2.31481e-08)
        {
            lidUnit.dryTime = 0.0;
        }
        else
        {
            lidUnit.dryTime += tStep;
        }
        S.lidproc_saveResults(lidUnit, S.UCF(S.RAINFALL), S.UCF(S.RAINDEPTH));
        qRunoff += lidRunoff;
        qDrain += lidDrain;
    }

    // lid.c:1890
    public static void lid_writeWaterBalance()
    {
        int j = 0;
        int k = 0;
        double ucf = S.UCF(S.RAINDEPTH);
        double inflow = 0.0;
        double outflow = 0.0;
        double err = 0.0;
        TLidUnit lidUnit = null;
        TLidList lidList = null;
        LidGroup lidGroup = null;
        for (j = 0; j < S.GroupCount; j++)
        {
            if ((S.LidGroups[j] != null))
            {
                k++;
            }
        }
        if (k == 0)
        {
            return;
        }
        R.fprintf(S.Frpt.file, S.L291);
        R.fprintf(S.Frpt.file, S.L292);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L293);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L294);
        }
        R.fprintf(S.Frpt.file, S.L295);
        for (j = 0; j < S.GroupCount; j++)
        {
            lidGroup = S.LidGroups[j];
            if ((!(lidGroup != null)) || (S.Subcatch[j].lidArea == 0.0))
            {
                continue;
            }
            lidList = lidGroup.lidList;
            while ((lidList != null))
            {
                lidUnit = lidList.lidUnit;
                k = lidUnit.lidIndex;
                R.fprintf(S.Frpt.file, S.L296, S.Subcatch[j].ID, S.LidProcs[k].ID);
                R.fprintf(S.Frpt.file, S.L297, lidUnit.waterBalance.inflow * ucf, lidUnit.waterBalance.evap * ucf, lidUnit.waterBalance.infil * ucf, lidUnit.waterBalance.surfFlow * ucf, lidUnit.waterBalance.drainFlow * ucf, lidUnit.waterBalance.initVol * ucf, lidUnit.waterBalance.finalVol * ucf);
                inflow = lidUnit.waterBalance.initVol + lidUnit.waterBalance.inflow;
                outflow = lidUnit.waterBalance.finalVol + lidUnit.waterBalance.evap + lidUnit.waterBalance.infil + lidUnit.waterBalance.surfFlow + lidUnit.waterBalance.drainFlow;
                if (inflow > 0.0)
                {
                    err = (inflow - outflow) / inflow;
                }
                else
                {
                    err = 1.0;
                }
                R.fprintf(S.Frpt.file, S.L298, err * 100.0);
                lidList = lidList.nextLidUnit;
            }
        }
    }

    // lid.c:1972
    public static void initLidRptFile(CPtr title, CPtr lidID, CPtr subcatchID, TLidUnit lidUnit)
    {
        int i = 0;
        CFile f = lidUnit.rptFile.file;
        if (f == null)
        {
            return;
        }
        R.fprintf(f, S.L299);
        R.fprintf(f, S.L300, title);
        R.fprintf(f, S.L301, lidID, subcatchID);
        for (i = 0; i < S.initLidRptFile_colCount; i++)
        {
            R.fprintf(f, S.L169, S.initLidRptFile_head1[i]);
        }
        for (i = 0; i < S.initLidRptFile_colCount; i++)
        {
            R.fprintf(f, S.L169, S.initLidRptFile_head2[i]);
        }
        if (S.UnitSystem == S.US)
        {
            for (i = 0; i < S.initLidRptFile_colCount; i++)
            {
                R.fprintf(f, S.L169, S.initLidRptFile_units1[i]);
            }
        }
        else
        {
            for (i = 0; i < S.initLidRptFile_colCount; i++)
            {
                R.fprintf(f, S.L169, S.initLidRptFile_units2[i]);
            }
        }
        R.fprintf(f, S.L302);
        for (i = 1; i < S.initLidRptFile_colCount; i++)
        {
            R.fprintf(f, S.L303, S.initLidRptFile_line9);
        }
        lidUnit.rptFile.wasDry = 1;
        S.sstrncpy(lidUnit.rptFile.results, S.L0, unchecked((ulong)(0)));
    }

}
}
