// Generated from EPA SWMM 5.2.4 (lidproc.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static TLidUnit theLidUnit = null;
    public static TLidProc theLidProc = null;
    public static double lidproc_Tstep = 0.0;
    public static double lidproc_EvapRate = 0.0;
    public static double lidproc_MaxNativeInfil = 0.0;
    public static double SurfaceInflow = 0.0;
    public static double SurfaceInfil = 0.0;
    public static double SurfaceEvap = 0.0;
    public static double SurfaceOutflow = 0.0;
    public static double SurfaceVolume = 0.0;
    public static double PaveEvap = 0.0;
    public static double PavePerc = 0.0;
    public static double PaveVolume = 0.0;
    public static double SoilEvap = 0.0;
    public static double SoilPerc = 0.0;
    public static double SoilVolume = 0.0;
    public static double StorageInflow = 0.0;
    public static double StorageExfil = 0.0;
    public static double StorageEvap = 0.0;
    public static double StorageDrain = 0.0;
    public static double StorageVolume = 0.0;
    public static double[] Xold = new double[4];

    // lidproc.c:178
    public static void lidproc_initWaterBalance(TLidUnit lidUnit, double initVol)
    {
        lidUnit.waterBalance.inflow = 0.0;
        lidUnit.waterBalance.evap = 0.0;
        lidUnit.waterBalance.infil = 0.0;
        lidUnit.waterBalance.surfFlow = 0.0;
        lidUnit.waterBalance.drainFlow = 0.0;
        lidUnit.waterBalance.initVol = initVol;
        lidUnit.waterBalance.finalVol = initVol;
    }

    // lidproc.c:197
    public static double lidproc_getOutflow(TLidUnit lidUnit, TLidProc lidProc, double inflow, double evap, double infil, double maxInfil, double tStep, ref double lidEvap, ref double lidInfil, ref double lidDrain)
    {
        int i = 0;
        double[] x = new double[4];
        double[] xOld = new double[4];
        double[] xPrev = new double[4];
        double[] xMin = new double[4];
        double[] xMax = new double[4];
        double[] fOld = new double[4];
        double[] f = new double[4];
        double[] xTol = new double[] { 0.00328, 0.00328, 0.00328, 0.00328 };
        double omega = 0.0;
        FP2 fluxRates = (null);
        S.theLidProc = lidProc;
        S.theLidUnit = lidUnit;
        S.lidproc_EvapRate = evap;
        S.lidproc_MaxNativeInfil = maxInfil;
        S.lidproc_Tstep = tStep;
        x[S.SURF] = S.theLidUnit.surfaceDepth;
        x[S.SOIL] = S.theLidUnit.soilMoisture;
        x[S.STOR] = S.theLidUnit.storageDepth;
        x[S.PAVE] = S.theLidUnit.paveDepth;
        S.SurfaceVolume = 0.0;
        S.PaveVolume = 0.0;
        S.SoilVolume = 0.0;
        S.StorageVolume = 0.0;
        S.SurfaceInflow = inflow;
        S.SurfaceInfil = 0.0;
        S.SurfaceEvap = 0.0;
        S.SurfaceOutflow = 0.0;
        S.PaveEvap = 0.0;
        S.PavePerc = 0.0;
        S.SoilEvap = 0.0;
        S.SoilPerc = 0.0;
        S.StorageInflow = 0.0;
        S.StorageExfil = 0.0;
        S.StorageEvap = 0.0;
        S.StorageDrain = 0.0;
        for (i = 0; i < 4; i++)
        {
            f[i] = 0.0;
            fOld[i] = S.theLidUnit.oldFluxRates[i];
            xMin[i] = 0.0;
            xMax[i] = 10000000000.0;
            S.Xold[i] = x[i];
        }
        if (S.theLidProc.lidType == S.POROUS_PAVEMENT)
        {
            S.SurfaceInfil = 0.0;
        }
        else if (S.theLidUnit.soilInfil.Ks > 0.0)
        {
            S.SurfaceInfil = S.grnampt_getInfil(S.theLidUnit.soilInfil, S.lidproc_Tstep, S.SurfaceInflow, S.theLidUnit.surfaceDepth, S.MOD_GREEN_AMPT);
        }
        else
        {
            S.SurfaceInfil = infil;
        }
        if (S.theLidProc.soil.thickness > 0.0)
        {
            xMin[S.SOIL] = S.theLidProc.soil.wiltPoint;
            xMax[S.SOIL] = S.theLidProc.soil.porosity;
        }
        if (S.theLidProc.pavement.thickness > 0.0)
        {
            xMax[S.PAVE] = S.theLidProc.pavement.thickness;
        }
        if (S.theLidProc.storage.thickness > 0.0)
        {
            xMax[S.STOR] = S.theLidProc.storage.thickness;
        }
        if (S.theLidProc.lidType == S.GREEN_ROOF)
        {
            xMax[S.STOR] = S.theLidProc.drainMat.thickness;
        }
        switch (S.theLidProc.lidType)
        {
            case S.BIO_CELL:
            case S.RAIN_GARDEN:
                fluxRates = S.biocellFluxRates;
                break;
            case S.GREEN_ROOF:
                fluxRates = S.greenRoofFluxRates;
                break;
            case S.INFIL_TRENCH:
                fluxRates = S.trenchFluxRates;
                break;
            case S.POROUS_PAVEMENT:
                fluxRates = S.pavementFluxRates;
                break;
            case S.RAIN_BARREL:
                fluxRates = S.barrelFluxRates;
                break;
            case S.ROOF_DISCON:
                fluxRates = S.roofFluxRates;
                break;
            case S.VEG_SWALE:
                fluxRates = S.swaleFluxRates;
                omega = 0.5;
                break;
            default:
                return 0.0;
        }
        i = S.modpuls_solve(4, x, xOld, xPrev, xMin, xMax, xTol, fOld, f, tStep, omega, fluxRates);
        if ((S.theLidProc.surface.canOverflow != 0) || (S.theLidUnit.fullWidth == 0.0))
        {
            S.SurfaceOutflow += S.getSurfaceOverflowRate(ref x[S.SURF]);
        }
        S.theLidUnit.surfaceDepth = x[S.SURF];
        S.theLidUnit.paveDepth = x[S.PAVE];
        S.theLidUnit.soilMoisture = x[S.SOIL];
        S.theLidUnit.storageDepth = x[S.STOR];
        for (i = 0; i < 4; i++)
        {
            S.theLidUnit.oldFluxRates[i] = f[i];
        }
        lidEvap = S.SurfaceEvap + S.PaveEvap + S.SoilEvap + S.StorageEvap;
        lidInfil = S.StorageExfil;
        lidDrain = S.StorageDrain;
        return S.SurfaceOutflow;
    }

    // lidproc.c:360
    public static void lidproc_saveResults(TLidUnit lidUnit, double ucfRainfall, double ucfRainDepth)
    {
        double ucf = 0.0;
        double totalEvap = 0.0;
        double totalVolume = 0.0;
        double[] rptVars = new double[12];
        int isDry = 0;
        CPtr timeStamp = R.Buf(22);
        double elapsedHrs = 0.0;
        totalEvap = S.SurfaceEvap + S.PaveEvap + S.SoilEvap + S.StorageEvap;
        totalVolume = S.SurfaceVolume + S.PaveVolume + S.SoilVolume + S.StorageVolume;
        S.updateWaterBalance(S.theLidUnit, S.SurfaceInflow, totalEvap, S.StorageExfil, S.SurfaceOutflow, S.StorageDrain, totalVolume);
        if (((((S.SurfaceInflow < 2.3e-08) && (S.SurfaceOutflow < 2.3e-08)) && (S.StorageDrain < 2.3e-08)) && (S.StorageExfil < 2.3e-08)) && (totalEvap < 2.3e-08))
        {
            isDry = 1;
        }
        if (!(isDry != 0))
        {
            S.HasWetLids = 1;
        }
        if ((lidUnit.rptFile != null))
        {
            ucf = ucfRainfall;
            rptVars[S.SURF_INFLOW] = S.SurfaceInflow * ucf;
            rptVars[S.TOTAL_EVAP] = totalEvap * ucf;
            rptVars[S.SURF_INFIL] = S.SurfaceInfil * ucf;
            rptVars[S.PAVE_PERC] = S.PavePerc * ucf;
            rptVars[S.SOIL_PERC] = S.SoilPerc * ucf;
            rptVars[S.STOR_EXFIL] = S.StorageExfil * ucf;
            rptVars[S.SURF_OUTFLOW] = S.SurfaceOutflow * ucf;
            rptVars[S.STOR_DRAIN] = S.StorageDrain * ucf;
            ucf = ucfRainDepth;
            rptVars[S.SURF_DEPTH] = S.theLidUnit.surfaceDepth * ucf;
            rptVars[S.PAVE_DEPTH] = S.theLidUnit.paveDepth * ucf;
            rptVars[S.SOIL_MOIST] = S.theLidUnit.soilMoisture;
            rptVars[S.STOR_DEPTH] = S.theLidUnit.storageDepth * ucf;
            if ((!(isDry != 0)) && (S.theLidUnit.rptFile.wasDry > 1))
            {
                R.fprintf(S.theLidUnit.rptFile.file, S.L169, S.theLidUnit.rptFile.results);
            }
            elapsedHrs = S.NewRunoffTime / 1000.0 / 3600.0;
            S.datetime_getTimeStamp(1, S.getDateTime(S.NewRunoffTime), 21, timeStamp);
            R.snprintf(S.theLidUnit.rptFile.results, 256, S.L304, timeStamp, elapsedHrs, rptVars[0], rptVars[1], rptVars[2], rptVars[3], rptVars[4], rptVars[5], rptVars[6], rptVars[7], rptVars[8], rptVars[9], rptVars[10], rptVars[11]);
            if ((isDry != 0))
            {
                if (S.theLidUnit.rptFile.wasDry == 0)
                {
                    R.fprintf(S.theLidUnit.rptFile.file, S.L169, S.theLidUnit.rptFile.results);
                }
                S.theLidUnit.rptFile.wasDry++;
            }
            else
            {
                R.fprintf(S.theLidUnit.rptFile.file, S.L169, S.theLidUnit.rptFile.results);
                S.theLidUnit.rptFile.wasDry = 0;
            }
        }
    }

    // lidproc.c:469
    public static void roofFluxRates(double[] x, double[] f)
    {
        double surfaceDepth = x[S.SURF];
        S.lidproc_getEvapRates(surfaceDepth, 0.0, 0.0, 0.0, 1.0);
        S.SurfaceVolume = surfaceDepth;
        S.SurfaceInfil = 0.0;
        if (S.theLidProc.surface.alpha > 0.0)
        {
            S.SurfaceOutflow = S.getSurfaceOutflowRate(surfaceDepth);
        }
        else
        {
            S.getSurfaceOverflowRate(ref surfaceDepth);
        }
        S.StorageDrain = ((((S.theLidProc.drain.coeff / S.UCF(S.RAINFALL)) <= (S.SurfaceOutflow)) ? (S.theLidProc.drain.coeff / S.UCF(S.RAINFALL)) : (S.SurfaceOutflow)));
        S.SurfaceOutflow -= S.StorageDrain;
        f[S.SURF] = (S.SurfaceInflow - S.SurfaceEvap - S.StorageDrain - S.SurfaceOutflow);
    }

    // lidproc.c:491
    public static void greenRoofFluxRates(double[] x, double[] f)
    {
        double surfaceDepth = 0.0;
        double soilTheta = 0.0;
        double storageDepth = 0.0;
        double availVolume = 0.0;
        double maxRate = 0.0;
        double soilThickness = S.theLidProc.soil.thickness;
        double storageThickness = S.theLidProc.storage.thickness;
        double soilPorosity = S.theLidProc.soil.porosity;
        double storageVoidFrac = S.theLidProc.storage.voidFrac;
        double soilFieldCap = S.theLidProc.soil.fieldCap;
        double soilWiltPoint = S.theLidProc.soil.wiltPoint;
        surfaceDepth = x[S.SURF];
        soilTheta = x[S.SOIL];
        storageDepth = x[S.STOR];
        S.SurfaceVolume = surfaceDepth * S.theLidProc.surface.voidFrac;
        S.SoilVolume = soilTheta * soilThickness;
        S.StorageVolume = storageDepth * storageVoidFrac;
        availVolume = S.SoilVolume - soilWiltPoint * soilThickness;
        S.lidproc_getEvapRates(S.SurfaceVolume, 0.0, availVolume, S.StorageVolume, 1.0);
        if (soilTheta >= soilPorosity)
        {
            S.StorageEvap = 0.0;
        }
        S.SoilPerc = S.getSoilPercRate(soilTheta);
        availVolume = (soilTheta - soilFieldCap) * soilThickness;
        maxRate = ((((availVolume) >= (0.0)) ? (availVolume) : (0.0))) / S.lidproc_Tstep - S.SoilEvap;
        S.SoilPerc = ((((S.SoilPerc) <= (maxRate)) ? (S.SoilPerc) : (maxRate)));
        S.SoilPerc = ((((S.SoilPerc) >= (0.0)) ? (S.SoilPerc) : (0.0)));
        S.StorageExfil = 0.0;
        S.StorageDrain = S.getDrainMatOutflow(storageDepth);
        if ((soilTheta >= soilPorosity) && (storageDepth >= storageThickness))
        {
            maxRate = ((((S.SoilPerc) <= (S.StorageDrain)) ? (S.SoilPerc) : (S.StorageDrain)));
            S.SoilPerc = maxRate;
            S.StorageDrain = maxRate;
            S.SurfaceInfil = ((((S.SurfaceInfil) <= (maxRate)) ? (S.SurfaceInfil) : (maxRate)));
        }
        else
        {
            maxRate = storageDepth * storageVoidFrac / S.lidproc_Tstep - S.StorageEvap;
            if (storageDepth >= storageThickness)
            {
                maxRate += S.SoilPerc;
            }
            maxRate = ((((maxRate) >= (0.0)) ? (maxRate) : (0.0)));
            S.StorageDrain = ((((S.StorageDrain) <= (maxRate)) ? (S.StorageDrain) : (maxRate)));
            maxRate = (storageThickness - storageDepth) * storageVoidFrac / S.lidproc_Tstep + S.StorageDrain + S.StorageEvap;
            S.SoilPerc = ((((S.SoilPerc) <= (maxRate)) ? (S.SoilPerc) : (maxRate)));
            maxRate = (soilPorosity - soilTheta) * soilThickness / S.lidproc_Tstep + S.SoilPerc + S.SoilEvap;
            S.SurfaceInfil = ((((S.SurfaceInfil) <= (maxRate)) ? (S.SurfaceInfil) : (maxRate)));
        }
        S.SurfaceOutflow = S.getSurfaceOutflowRate(surfaceDepth);
        f[S.SURF] = (S.SurfaceInflow - S.SurfaceEvap - S.SurfaceInfil - S.SurfaceOutflow) / S.theLidProc.surface.voidFrac;
        f[S.SOIL] = (S.SurfaceInfil - S.SoilEvap - S.SoilPerc) / S.theLidProc.soil.thickness;
        f[S.STOR] = (S.SoilPerc - S.StorageEvap - S.StorageDrain) / S.theLidProc.storage.voidFrac;
    }

    // lidproc.c:589
    public static void biocellFluxRates(double[] x, double[] f)
    {
        double surfaceDepth = 0.0;
        double soilTheta = 0.0;
        double storageDepth = 0.0;
        double availVolume = 0.0;
        double maxRate = 0.0;
        double soilThickness = S.theLidProc.soil.thickness;
        double soilPorosity = S.theLidProc.soil.porosity;
        double soilFieldCap = S.theLidProc.soil.fieldCap;
        double soilWiltPoint = S.theLidProc.soil.wiltPoint;
        double storageThickness = S.theLidProc.storage.thickness;
        double storageVoidFrac = S.theLidProc.storage.voidFrac;
        surfaceDepth = x[S.SURF];
        soilTheta = x[S.SOIL];
        storageDepth = x[S.STOR];
        S.SurfaceVolume = surfaceDepth * S.theLidProc.surface.voidFrac;
        S.SoilVolume = soilTheta * soilThickness;
        S.StorageVolume = storageDepth * storageVoidFrac;
        availVolume = S.SoilVolume - soilWiltPoint * soilThickness;
        S.lidproc_getEvapRates(S.SurfaceVolume, 0.0, availVolume, S.StorageVolume, 1.0);
        if (soilTheta >= soilPorosity)
        {
            S.StorageEvap = 0.0;
        }
        S.SoilPerc = S.getSoilPercRate(soilTheta);
        availVolume = (soilTheta - soilFieldCap) * soilThickness;
        maxRate = ((((availVolume) >= (0.0)) ? (availVolume) : (0.0))) / S.lidproc_Tstep - S.SoilEvap;
        S.SoilPerc = ((((S.SoilPerc) <= (maxRate)) ? (S.SoilPerc) : (maxRate)));
        S.SoilPerc = ((((S.SoilPerc) >= (0.0)) ? (S.SoilPerc) : (0.0)));
        S.StorageExfil = S.getStorageExfilRate();
        S.StorageDrain = 0.0;
        if (S.theLidProc.drain.coeff > 0.0)
        {
            S.StorageDrain = S.getStorageDrainRate(storageDepth, soilTheta, 0.0, surfaceDepth);
        }
        if (storageThickness == 0.0)
        {
            S.StorageEvap = 0.0;
            maxRate = ((((S.SoilPerc) <= (S.StorageExfil)) ? (S.SoilPerc) : (S.StorageExfil)));
            S.SoilPerc = maxRate;
            S.StorageExfil = maxRate;
            maxRate = (soilPorosity - soilTheta) * soilThickness / S.lidproc_Tstep + S.SoilPerc + S.SoilEvap;
            S.SurfaceInfil = ((((S.SurfaceInfil) <= (maxRate)) ? (S.SurfaceInfil) : (maxRate)));
        }
        else
        {
            if ((soilTheta >= soilPorosity) && (storageDepth >= storageThickness))
            {
                maxRate = S.StorageExfil + S.StorageDrain;
                if (S.SoilPerc < maxRate)
                {
                    maxRate = S.SoilPerc;
                    if (maxRate > S.StorageExfil)
                    {
                        S.StorageDrain = maxRate - S.StorageExfil;
                    }
                    else
                    {
                        S.StorageExfil = maxRate;
                        S.StorageDrain = 0.0;
                    }
                }
                else
                {
                    S.SoilPerc = maxRate;
                }
                S.SurfaceInfil = ((((S.SurfaceInfil) <= (maxRate)) ? (S.SurfaceInfil) : (maxRate)));
            }
            else
            {
                maxRate = S.SoilPerc - S.StorageEvap + storageDepth * storageVoidFrac / S.lidproc_Tstep;
                S.StorageExfil = ((((S.StorageExfil) <= (maxRate)) ? (S.StorageExfil) : (maxRate)));
                S.StorageExfil = ((((S.StorageExfil) >= (0.0)) ? (S.StorageExfil) : (0.0)));
                if (S.StorageDrain > 0.0)
                {
                    maxRate = -S.StorageExfil - S.StorageEvap;
                    if (storageDepth >= storageThickness)
                    {
                        maxRate += S.SoilPerc;
                    }
                    if (S.theLidProc.drain.offset <= storageDepth)
                    {
                        maxRate += (storageDepth - S.theLidProc.drain.offset) * storageVoidFrac / S.lidproc_Tstep;
                    }
                    maxRate = ((((maxRate) >= (0.0)) ? (maxRate) : (0.0)));
                    S.StorageDrain = ((((S.StorageDrain) <= (maxRate)) ? (S.StorageDrain) : (maxRate)));
                }
                maxRate = S.StorageExfil + S.StorageDrain + S.StorageEvap + (storageThickness - storageDepth) * storageVoidFrac / S.lidproc_Tstep;
                S.SoilPerc = ((((S.SoilPerc) <= (maxRate)) ? (S.SoilPerc) : (maxRate)));
                maxRate = (soilPorosity - soilTheta) * soilThickness / S.lidproc_Tstep + S.SoilPerc + S.SoilEvap;
                S.SurfaceInfil = ((((S.SurfaceInfil) <= (maxRate)) ? (S.SurfaceInfil) : (maxRate)));
            }
        }
        S.SurfaceOutflow = S.getSurfaceOutflowRate(surfaceDepth);
        f[S.SURF] = (S.SurfaceInflow - S.SurfaceEvap - S.SurfaceInfil - S.SurfaceOutflow) / S.theLidProc.surface.voidFrac;
        f[S.SOIL] = (S.SurfaceInfil - S.SoilEvap - S.SoilPerc) / S.theLidProc.soil.thickness;
        if (storageThickness == 0.0)
        {
            f[S.STOR] = 0.0;
        }
        else
        {
            f[S.STOR] = (S.SoilPerc - S.StorageEvap - S.StorageExfil - S.StorageDrain) / S.theLidProc.storage.voidFrac;
        }
    }

    // lidproc.c:735
    public static void trenchFluxRates(double[] x, double[] f)
    {
        double surfaceDepth = 0.0;
        double storageDepth = 0.0;
        double availVolume = 0.0;
        double maxRate = 0.0;
        double storageThickness = S.theLidProc.storage.thickness;
        double storageVoidFrac = S.theLidProc.storage.voidFrac;
        surfaceDepth = x[S.SURF];
        storageDepth = x[S.STOR];
        S.SurfaceVolume = surfaceDepth * S.theLidProc.surface.voidFrac;
        S.SoilVolume = 0.0;
        S.StorageVolume = storageDepth * storageVoidFrac;
        availVolume = (storageThickness - storageDepth) * storageVoidFrac;
        S.lidproc_getEvapRates(S.SurfaceVolume, 0.0, 0.0, S.StorageVolume, 1.0);
        if (surfaceDepth > 0.0)
        {
            S.StorageEvap = 0.0;
        }
        S.StorageInflow = S.SurfaceInflow + S.SurfaceVolume / S.lidproc_Tstep;
        S.StorageExfil = S.getStorageExfilRate();
        S.StorageDrain = 0.0;
        if (S.theLidProc.drain.coeff > 0.0)
        {
            S.StorageDrain = S.getStorageDrainRate(storageDepth, 0.0, 0.0, surfaceDepth);
        }
        maxRate = S.StorageInflow - S.StorageEvap + storageDepth * storageVoidFrac / S.lidproc_Tstep;
        S.StorageExfil = ((((S.StorageExfil) <= (maxRate)) ? (S.StorageExfil) : (maxRate)));
        S.StorageExfil = ((((S.StorageExfil) >= (0.0)) ? (S.StorageExfil) : (0.0)));
        if (S.StorageDrain > 0.0)
        {
            maxRate = -S.StorageExfil - S.StorageEvap;
            if (storageDepth >= storageThickness)
            {
                maxRate += S.StorageInflow;
            }
            if (S.theLidProc.drain.offset <= storageDepth)
            {
                maxRate += (storageDepth - S.theLidProc.drain.offset) * storageVoidFrac / S.lidproc_Tstep;
            }
            maxRate = ((((maxRate) >= (0.0)) ? (maxRate) : (0.0)));
            S.StorageDrain = ((((S.StorageDrain) <= (maxRate)) ? (S.StorageDrain) : (maxRate)));
        }
        maxRate = (storageThickness - storageDepth) * storageVoidFrac / S.lidproc_Tstep + S.StorageExfil + S.StorageEvap + S.StorageDrain;
        S.StorageInflow = ((((S.StorageInflow) <= (maxRate)) ? (S.StorageInflow) : (maxRate)));
        S.SurfaceInfil = S.StorageInflow;
        S.SurfaceOutflow = S.getSurfaceOutflowRate(surfaceDepth);
        f[S.SURF] = (S.SurfaceInflow - S.SurfaceEvap - S.StorageInflow - S.SurfaceOutflow) / S.theLidProc.surface.voidFrac;
        ;
        f[S.STOR] = (S.StorageInflow - S.StorageEvap - S.StorageExfil - S.StorageDrain) / S.theLidProc.storage.voidFrac;
        f[S.SOIL] = 0.0;
    }

    // lidproc.c:823
    public static void pavementFluxRates(double[] x, double[] f)
    {
        double surfaceDepth = 0.0;
        double paveDepth = 0.0;
        double soilTheta = 0.0;
        double storageDepth = 0.0;
        double pervFrac = (1.0 - S.theLidProc.pavement.impervFrac);
        double storageInflow = 0.0;
        double availVolume = 0.0;
        double maxRate = 0.0;
        double paveVoidFrac = S.theLidProc.pavement.voidFrac * pervFrac;
        double paveThickness = S.theLidProc.pavement.thickness;
        double soilThickness = S.theLidProc.soil.thickness;
        double soilPorosity = S.theLidProc.soil.porosity;
        double soilFieldCap = S.theLidProc.soil.fieldCap;
        double soilWiltPoint = S.theLidProc.soil.wiltPoint;
        double storageThickness = S.theLidProc.storage.thickness;
        double storageVoidFrac = S.theLidProc.storage.voidFrac;
        surfaceDepth = x[S.SURF];
        paveDepth = x[S.PAVE];
        soilTheta = x[S.SOIL];
        storageDepth = x[S.STOR];
        S.SurfaceVolume = surfaceDepth * S.theLidProc.surface.voidFrac;
        S.PaveVolume = paveDepth * paveVoidFrac;
        S.SoilVolume = soilTheta * soilThickness;
        S.StorageVolume = storageDepth * storageVoidFrac;
        availVolume = S.SoilVolume - soilWiltPoint * soilThickness;
        S.lidproc_getEvapRates(S.SurfaceVolume, S.PaveVolume, availVolume, S.StorageVolume, pervFrac);
        if ((paveDepth >= paveThickness) || (((soilThickness > 0.0) && (soilTheta >= soilPorosity))))
        {
            S.StorageEvap = 0.0;
        }
        S.SurfaceInfil = S.SurfaceInflow + (S.SurfaceVolume / S.lidproc_Tstep);
        S.PavePerc = S.getPavementPermRate() * pervFrac;
        S.SurfaceInfil = ((((S.SurfaceInfil) <= (S.PavePerc)) ? (S.SurfaceInfil) : (S.PavePerc)));
        maxRate = S.PaveVolume / S.lidproc_Tstep + S.SurfaceInfil - S.PaveEvap;
        maxRate = ((((maxRate) >= (0.0)) ? (maxRate) : (0.0)));
        S.PavePerc = ((((S.PavePerc) <= (maxRate)) ? (S.PavePerc) : (maxRate)));
        if (soilThickness > 0.0)
        {
            S.SoilPerc = S.getSoilPercRate(soilTheta);
            availVolume = (soilTheta - soilFieldCap) * soilThickness;
            maxRate = ((((availVolume) >= (0.0)) ? (availVolume) : (0.0))) / S.lidproc_Tstep - S.SoilEvap;
            S.SoilPerc = ((((S.SoilPerc) <= (maxRate)) ? (S.SoilPerc) : (maxRate)));
            S.SoilPerc = ((((S.SoilPerc) >= (0.0)) ? (S.SoilPerc) : (0.0)));
        }
        else
        {
            S.SoilPerc = S.PavePerc;
        }
        S.StorageExfil = S.getStorageExfilRate();
        S.StorageDrain = 0.0;
        if (S.theLidProc.drain.coeff > 0.0)
        {
            S.StorageDrain = S.getStorageDrainRate(storageDepth, soilTheta, paveDepth, surfaceDepth);
        }
        if (((soilThickness == 0.0) && (storageDepth >= storageThickness)) && (paveDepth >= paveThickness))
        {
            maxRate = S.StorageEvap + S.StorageDrain + S.StorageExfil;
            if (S.PavePerc > maxRate)
            {
                S.PavePerc = maxRate;
            }
            else
            {
                S.StorageExfil = ((((S.StorageExfil) <= (S.PavePerc)) ? (S.StorageExfil) : (S.PavePerc)));
                S.StorageDrain = S.PavePerc - S.StorageExfil;
            }
            S.SoilPerc = S.PavePerc;
            S.SurfaceInfil = ((((S.SurfaceInfil) <= (S.PavePerc)) ? (S.SurfaceInfil) : (S.PavePerc)));
        }
        else if ((((soilThickness > 0) && (storageDepth >= storageThickness)) && (soilTheta >= soilPorosity)) && (paveDepth >= paveThickness))
        {
            maxRate = S.StorageExfil + S.StorageDrain;
            if (S.SoilPerc < maxRate)
            {
                maxRate = S.SoilPerc;
            }
            else
            {
                maxRate = ((((maxRate) <= (S.PavePerc)) ? (maxRate) : (S.PavePerc)));
            }
            if (maxRate > S.StorageExfil)
            {
                S.StorageDrain = maxRate - S.StorageExfil;
            }
            else
            {
                S.StorageExfil = maxRate;
                S.StorageDrain = 0.0;
            }
            S.SoilPerc = maxRate;
            S.PavePerc = maxRate;
            S.SurfaceInfil = ((((S.SurfaceInfil) <= (S.PavePerc)) ? (S.SurfaceInfil) : (S.PavePerc)));
        }
        else if (((soilThickness > 0.0) && (storageDepth >= storageThickness)) && (soilTheta >= soilPorosity))
        {
            maxRate = S.StorageDrain + S.StorageExfil;
            if (S.SoilPerc > maxRate)
            {
                S.SoilPerc = maxRate;
            }
            else
            {
                S.StorageExfil = ((((S.StorageExfil) <= (S.SoilPerc)) ? (S.StorageExfil) : (S.SoilPerc)));
                S.StorageDrain = S.SoilPerc - S.StorageExfil;
            }
            S.PavePerc = ((((S.PavePerc) <= (S.SoilPerc)) ? (S.PavePerc) : (S.SoilPerc)));
            availVolume = (paveThickness - paveDepth) * paveVoidFrac;
            maxRate = availVolume / S.lidproc_Tstep + S.PavePerc + S.PaveEvap;
            S.SurfaceInfil = ((((S.SurfaceInfil) <= (maxRate)) ? (S.SurfaceInfil) : (maxRate)));
        }
        else if (((soilThickness > 0.0) && (paveDepth >= paveThickness)) && (soilTheta >= soilPorosity))
        {
            S.PavePerc = ((((S.PavePerc) <= (S.SoilPerc)) ? (S.PavePerc) : (S.SoilPerc)));
            S.SoilPerc = S.PavePerc;
            S.SurfaceInfil = ((((S.SurfaceInfil) <= (S.PavePerc)) ? (S.SurfaceInfil) : (S.PavePerc)));
            maxRate = ((((S.StorageVolume / S.lidproc_Tstep + S.SoilPerc - S.StorageEvap) >= (0.0)) ? (S.StorageVolume / S.lidproc_Tstep + S.SoilPerc - S.StorageEvap) : (0.0)));
            S.StorageExfil = ((((S.StorageExfil) <= (maxRate)) ? (S.StorageExfil) : (maxRate)));
        }
        else
        {
            maxRate = S.SoilPerc - S.StorageEvap + S.StorageVolume / S.lidproc_Tstep;
            maxRate = ((((0.0) >= (maxRate)) ? (0.0) : (maxRate)));
            S.StorageExfil = ((((S.StorageExfil) <= (maxRate)) ? (S.StorageExfil) : (maxRate)));
            if (S.StorageDrain > 0.0)
            {
                maxRate = -S.StorageExfil - S.StorageEvap;
                if (storageDepth >= storageThickness)
                {
                    maxRate += S.SoilPerc;
                }
                if (S.theLidProc.drain.offset <= storageDepth)
                {
                    maxRate += (storageDepth - S.theLidProc.drain.offset) * storageVoidFrac / S.lidproc_Tstep;
                }
                maxRate = ((((maxRate) >= (0.0)) ? (maxRate) : (0.0)));
                S.StorageDrain = ((((S.StorageDrain) <= (maxRate)) ? (S.StorageDrain) : (maxRate)));
            }
            availVolume = (storageThickness - storageDepth) * storageVoidFrac;
            maxRate = availVolume / S.lidproc_Tstep + S.StorageEvap + S.StorageDrain + S.StorageExfil;
            maxRate = ((((maxRate) >= (0.0)) ? (maxRate) : (0.0)));
            if (soilThickness > 0.0)
            {
                S.SoilPerc = ((((S.SoilPerc) <= (maxRate)) ? (S.SoilPerc) : (maxRate)));
                maxRate = (soilPorosity - soilTheta) * soilThickness / S.lidproc_Tstep + S.SoilPerc;
            }
            S.PavePerc = ((((S.PavePerc) <= (maxRate)) ? (S.PavePerc) : (maxRate)));
            availVolume = (paveThickness - paveDepth) * paveVoidFrac;
            maxRate = availVolume / S.lidproc_Tstep + S.PavePerc + S.PaveEvap;
            S.SurfaceInfil = ((((S.SurfaceInfil) <= (maxRate)) ? (S.SurfaceInfil) : (maxRate)));
        }
        S.SurfaceOutflow = S.getSurfaceOutflowRate(surfaceDepth);
        f[S.SURF] = S.SurfaceInflow - S.SurfaceEvap - S.SurfaceInfil - S.SurfaceOutflow;
        f[S.PAVE] = (S.SurfaceInfil - S.PaveEvap - S.PavePerc) / paveVoidFrac;
        if (S.theLidProc.soil.thickness > 0.0)
        {
            f[S.SOIL] = (S.PavePerc - S.SoilEvap - S.SoilPerc) / soilThickness;
            storageInflow = S.SoilPerc;
        }
        else
        {
            f[S.SOIL] = 0.0;
            storageInflow = S.PavePerc;
            S.SoilPerc = 0.0;
        }
        f[S.STOR] = (storageInflow - S.StorageEvap - S.StorageExfil - S.StorageDrain) / storageVoidFrac;
    }

    // lidproc.c:1061
    public static void swaleFluxRates(double[] x, double[] f)
    {
        double depth = 0.0;
        double topWidth = 0.0;
        double botWidth = 0.0;
        double length = 0.0;
        double surfInflow = 0.0;
        double surfWidth = 0.0;
        double surfArea = 0.0;
        double flowArea = 0.0;
        double lidArea = 0.0;
        double hydRadius = 0.0;
        double slope = 0.0;
        double volume = 0.0;
        double dVdT = 0.0;
        double dStore = 0.0;
        double xDepth = 0.0;
        depth = x[S.SURF];
        depth = ((((depth) <= (S.theLidProc.surface.thickness)) ? (depth) : (S.theLidProc.surface.thickness)));
        dStore = 0.0;
        slope = S.theLidProc.surface.sideSlope;
        topWidth = S.theLidUnit.fullWidth;
        topWidth = ((((topWidth) >= (0.5)) ? (topWidth) : (0.5)));
        botWidth = topWidth - 2.0 * slope * S.theLidProc.surface.thickness;
        if (botWidth < 0.5)
        {
            botWidth = 0.5;
            slope = 0.5 * (topWidth - 0.5) / S.theLidProc.surface.thickness;
        }
        lidArea = S.theLidUnit.area;
        length = lidArea / topWidth;
        surfWidth = botWidth + 2.0 * slope * depth;
        surfArea = length * surfWidth;
        flowArea = (depth * (botWidth + slope * depth)) * S.theLidProc.surface.voidFrac;
        volume = length * flowArea;
        surfInflow = S.SurfaceInflow * lidArea;
        S.SurfaceEvap = S.lidproc_EvapRate * surfArea;
        S.SurfaceEvap = ((((S.SurfaceEvap) <= (volume / S.lidproc_Tstep)) ? (S.SurfaceEvap) : (volume / S.lidproc_Tstep)));
        S.StorageExfil = S.SurfaceInfil * surfArea;
        xDepth = depth - dStore;
        if (xDepth <= 1e-10)
        {
            S.SurfaceOutflow = 0.0;
        }
        else
        {
            flowArea -= (dStore * (botWidth + slope * dStore)) * S.theLidProc.surface.voidFrac;
            if (flowArea < 1e-10)
            {
                S.SurfaceOutflow = 0.0;
            }
            else
            {
                botWidth = botWidth + 2.0 * dStore * slope;
                hydRadius = botWidth + 2.0 * xDepth * Math.Sqrt(1.0 + slope * slope);
                hydRadius = flowArea / hydRadius;
                S.SurfaceOutflow = S.theLidProc.surface.alpha * flowArea * Math.Pow(hydRadius, 2.0 / 3.0);
            }
        }
        dVdT = surfInflow - S.SurfaceEvap - S.StorageExfil - S.SurfaceOutflow;
        if ((depth == S.theLidProc.surface.thickness) && (dVdT > 0.0))
        {
            S.SurfaceOutflow += dVdT;
            dVdT = 0.0;
        }
        S.SurfaceEvap /= lidArea;
        S.StorageExfil /= lidArea;
        S.SurfaceOutflow /= lidArea;
        f[S.SURF] = dVdT / surfArea;
        f[S.SOIL] = 0.0;
        f[S.STOR] = 0.0;
        S.SurfaceVolume = volume / lidArea;
        S.SoilVolume = 0.0;
        S.StorageVolume = 0.0;
    }

    // lidproc.c:1176
    public static void barrelFluxRates(double[] x, double[] f)
    {
        double storageDepth = x[S.STOR];
        double head = 0.0;
        double maxValue = 0.0;
        S.SurfaceVolume = 0.0;
        S.SoilVolume = 0.0;
        S.StorageVolume = storageDepth;
        S.SurfaceInfil = 0.0;
        S.SurfaceOutflow = 0.0;
        S.StorageDrain = 0.0;
        if ((S.theLidProc.drain.delay == 0.0) || (S.theLidUnit.dryTime >= S.theLidProc.drain.delay))
        {
            head = storageDepth - S.theLidProc.drain.offset;
            if (head > 0.0)
            {
                S.StorageDrain = S.getStorageDrainRate(storageDepth, 0.0, 0.0, 0.0);
                maxValue = (head / S.lidproc_Tstep);
                S.StorageDrain = ((((S.StorageDrain) <= (maxValue)) ? (S.StorageDrain) : (maxValue)));
            }
        }
        S.StorageInflow = S.SurfaceInflow;
        maxValue = (S.theLidProc.storage.thickness - storageDepth) / S.lidproc_Tstep + S.StorageDrain;
        S.StorageInflow = ((((S.StorageInflow) <= (maxValue)) ? (S.StorageInflow) : (maxValue)));
        S.SurfaceInfil = S.StorageInflow;
        f[S.SURF] = S.SurfaceInflow - S.StorageInflow;
        f[S.STOR] = S.StorageInflow - S.StorageDrain;
        f[S.SOIL] = 0.0;
    }

    // lidproc.c:1226
    public static double getSurfaceOutflowRate(double depth)
    {
        double delta = 0.0;
        double outflow = 0.0;
        delta = depth - S.theLidProc.surface.thickness;
        if (delta < 0.0)
        {
            return 0.0;
        }
        outflow = S.theLidProc.surface.alpha * Math.Pow(delta, 5.0 / 3.0) * S.theLidUnit.fullWidth / S.theLidUnit.area;
        outflow = ((((outflow) <= (delta / S.lidproc_Tstep)) ? (outflow) : (delta / S.lidproc_Tstep)));
        return outflow;
    }

    // lidproc.c:1251
    public static double getPavementPermRate()
    {
        double permReduction = 0.0;
        double clogFactor = S.theLidProc.pavement.clogFactor;
        double regenDays = S.theLidProc.pavement.regenDays;
        if (clogFactor > 0.0)
        {
            if (regenDays > 0.0)
            {
                if (S.OldRunoffTime / 1000.0 / 86400 >= S.theLidUnit.nextRegenDay)
                {
                    S.theLidUnit.volTreated *= (1.0 - S.theLidProc.pavement.regenDegree);
                    S.theLidUnit.nextRegenDay += regenDays;
                }
            }
            permReduction = S.theLidUnit.volTreated / clogFactor;
            permReduction = ((((permReduction) <= (1.0)) ? (permReduction) : (1.0)));
        }
        return S.theLidProc.pavement.kSat * (1.0 - permReduction);
    }

    // lidproc.c:1293
    public static double getSoilPercRate(double theta)
    {
        double delta = 0.0;
        if (theta <= S.theLidProc.soil.fieldCap)
        {
            return 0.0;
        }
        delta = S.theLidProc.soil.porosity - theta;
        return S.theLidProc.soil.kSat * Math.Exp(-delta * S.theLidProc.soil.kSlope);
    }

    // lidproc.c:1313
    public static double getStorageExfilRate()
    {
        double infil = 0.0;
        double clogFactor = 0.0;
        if (S.theLidProc.storage.kSat == 0.0)
        {
            return 0.0;
        }
        if (S.lidproc_MaxNativeInfil == 0.0)
        {
            return 0.0;
        }
        clogFactor = S.theLidProc.storage.clogFactor;
        if (clogFactor > 0.0)
        {
            clogFactor = S.theLidUnit.waterBalance.inflow / clogFactor;
            clogFactor = ((((clogFactor) <= (1.0)) ? (clogFactor) : (1.0)));
        }
        infil = S.theLidProc.storage.kSat * (1.0 - clogFactor);
        return ((((infil) <= (S.lidproc_MaxNativeInfil)) ? (infil) : (S.lidproc_MaxNativeInfil)));
    }

    // lidproc.c:1344
    public static double getStorageDrainRate(double storageDepth, double soilTheta, double paveDepth, double surfaceDepth)
    {
        int curve = S.theLidProc.drain.qCurve;
        double head = storageDepth;
        double outflow = 0.0;
        double paveThickness = S.theLidProc.pavement.thickness;
        double soilThickness = S.theLidProc.soil.thickness;
        double soilPorosity = S.theLidProc.soil.porosity;
        double soilFieldCap = S.theLidProc.soil.fieldCap;
        double storageThickness = S.theLidProc.storage.thickness;
        if (storageDepth >= storageThickness)
        {
            if (soilThickness > 0.0)
            {
                if (soilTheta > soilFieldCap)
                {
                    head += (soilTheta - soilFieldCap) / (soilPorosity - soilFieldCap) * soilThickness;
                    if (soilTheta >= soilPorosity)
                    {
                        if (paveThickness > 0.0)
                        {
                            head += paveDepth;
                            if (paveDepth >= paveThickness)
                            {
                                head += surfaceDepth;
                            }
                        }
                        else
                        {
                            head += surfaceDepth;
                        }
                    }
                }
            }
            else if (paveThickness > 0.0)
            {
                head += paveDepth;
                if (paveDepth >= paveThickness)
                {
                    head += surfaceDepth;
                }
            }
        }
        if ((S.theLidUnit.oldDrainFlow == 0.0) && (head <= S.theLidProc.drain.hOpen))
        {
            return 0.0;
        }
        if ((S.theLidUnit.oldDrainFlow > 0.0) && (head <= S.theLidProc.drain.hClose))
        {
            return 0.0;
        }
        head -= S.theLidProc.drain.offset;
        if (head > 1e-10)
        {
            head *= S.UCF(S.RAINDEPTH);
            outflow = S.theLidProc.drain.coeff * Math.Pow(head, S.theLidProc.drain.expon);
            if (curve >= 0)
            {
                outflow *= S.table_lookup(S.Curve[curve], head);
            }
            outflow /= S.UCF(S.RAINFALL);
        }
        return outflow;
    }

    // lidproc.c:1436
    public static double getDrainMatOutflow(double depth)
    {
        double result = S.SoilPerc;
        if (S.theLidProc.drainMat.alpha > 0.0)
        {
            result = S.theLidProc.drainMat.alpha * Math.Pow(depth, 5.0 / 3.0) * S.theLidUnit.fullWidth / S.theLidUnit.area * S.theLidProc.drainMat.voidFrac;
        }
        return result;
    }

    // lidproc.c:1458
    public static void lidproc_getEvapRates(double surfaceVol, double paveVol, double soilVol, double storageVol, double pervFrac)
    {
        double availEvap = 0.0;
        availEvap = S.lidproc_EvapRate;
        S.SurfaceEvap = ((((availEvap) <= (surfaceVol / S.lidproc_Tstep)) ? (availEvap) : (surfaceVol / S.lidproc_Tstep)));
        S.SurfaceEvap = ((((0.0) >= (S.SurfaceEvap)) ? (0.0) : (S.SurfaceEvap)));
        availEvap = ((((0.0) >= ((availEvap - S.SurfaceEvap))) ? (0.0) : ((availEvap - S.SurfaceEvap))));
        availEvap *= pervFrac;
        if (S.SurfaceInfil > 0.0)
        {
            S.PaveEvap = 0.0;
            S.SoilEvap = 0.0;
            S.StorageEvap = 0.0;
        }
        else
        {
            S.PaveEvap = ((((availEvap) <= (paveVol / S.lidproc_Tstep)) ? (availEvap) : (paveVol / S.lidproc_Tstep)));
            availEvap = ((((0.0) >= ((availEvap - S.PaveEvap))) ? (0.0) : ((availEvap - S.PaveEvap))));
            S.SoilEvap = ((((availEvap) <= (soilVol / S.lidproc_Tstep)) ? (availEvap) : (soilVol / S.lidproc_Tstep)));
            availEvap = ((((0.0) >= ((availEvap - S.SoilEvap))) ? (0.0) : ((availEvap - S.SoilEvap))));
            S.StorageEvap = ((((availEvap) <= (storageVol / S.lidproc_Tstep)) ? (availEvap) : (storageVol / S.lidproc_Tstep)));
        }
    }

    // lidproc.c:1503
    public static double getSurfaceOverflowRate(ref double surfaceDepth)
    {
        double delta = surfaceDepth - S.theLidProc.surface.thickness;
        if (delta <= 0.0)
        {
            return 0.0;
        }
        surfaceDepth = S.theLidProc.surface.thickness;
        return delta * S.theLidProc.surface.voidFrac / S.lidproc_Tstep;
    }

    // lidproc.c:1518
    public static void updateWaterBalance(TLidUnit lidUnit, double inflow, double evap, double infil, double surfFlow, double drainFlow, double storage)
    {
        lidUnit.volTreated += inflow * S.lidproc_Tstep;
        lidUnit.waterBalance.inflow += inflow * S.lidproc_Tstep;
        lidUnit.waterBalance.evap += evap * S.lidproc_Tstep;
        lidUnit.waterBalance.infil += infil * S.lidproc_Tstep;
        lidUnit.waterBalance.surfFlow += surfFlow * S.lidproc_Tstep;
        lidUnit.waterBalance.drainFlow += drainFlow * S.lidproc_Tstep;
        lidUnit.waterBalance.finalVol = storage;
    }

    // lidproc.c:1544
    public static int modpuls_solve(int n, double[] x, double[] xOld, double[] xPrev, double[] xMin, double[] xMax, double[] xTol, double[] qOld, double[] q, double dt, double omega, FP2 derivs)
    {
        int i = 0;
        int canStop = 0;
        int steps = 1;
        int maxSteps = 20;
        for (i = 0; i < n; i++)
        {
            xOld[i] = x[i];
            xPrev[i] = x[i];
        }
        while (steps < maxSteps)
        {
            canStop = 1;
            derivs(x, q);
            for (i = 0; i < n; i++)
            {
                x[i] = xOld[i] + (omega * qOld[i] + (1.0 - omega) * q[i]) * dt;
                x[i] = ((((x[i]) <= (xMax[i])) ? (x[i]) : (xMax[i])));
                x[i] = ((((x[i]) >= (xMin[i])) ? (x[i]) : (xMin[i])));
                if ((omega > 0.0) && (Math.Abs(x[i] - xPrev[i]) > xTol[i]))
                {
                    canStop = 0;
                }
                xPrev[i] = x[i];
            }
            if ((canStop != 0))
            {
                return steps;
            }
            steps++;
        }
        return 0;
    }

}
}
