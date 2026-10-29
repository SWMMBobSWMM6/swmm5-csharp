// Generated from EPA SWMM 5.2.4 (runoff.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int IsRaining = 0;
    public static int HasRunoff = 0;
    public static int HasSnow = 0;
    public static int runoff_Nsteps = 0;
    public static int MaxSteps = 0;
    public static long MaxStepsPos = 0L;
    public static int HasWetLids = 0;
    public static double[] OutflowLoad = null;

    // runoff.c:82
    public static int runoff_open()
    {
        S.IsRaining = 0;
        S.HasRunoff = 0;
        S.HasSnow = 0;
        S.runoff_Nsteps = 0;
        if (!((S.odesolve_open(4) != 0)))
        {
            S.report_writeErrorMsg(S.ERR_ODE_SOLVER, S.L0);
        }
        S.OutflowLoad = (null);
        if (S.Nobjects[S.POLLUT] > 0)
        {
            S.OutflowLoad = new double[(int)(unchecked((ulong)(S.Nobjects[S.POLLUT])))];
            if (!(S.OutflowLoad != null))
            {
                S.report_writeErrorMsg(S.ERR_MEMORY, S.L0);
            }
        }
        switch (S.Frunoff.mode)
        {
            case S.USE_FILE:
                if ((S.Frunoff.file = R.fopen(S.Frunoff.name, S.L165)) == null)
                {
                    S.report_writeErrorMsg(S.ERR_RUNOFF_FILE_OPEN, S.Frunoff.name);
                }
                else
                {
                    S.runoff_initFile();
                }
                break;
            case S.SAVE_FILE:
                if ((S.Frunoff.file = R.fopen(S.Frunoff.name, S.L166)) == null)
                {
                    S.report_writeErrorMsg(S.ERR_RUNOFF_FILE_OPEN, S.Frunoff.name);
                }
                else
                {
                    S.runoff_initFile();
                }
                break;
        }
        return S.ErrorCode;
    }

    // runoff.c:124
    public static void runoff_close()
    {
        S.odesolve_close();
        {
            if ((S.OutflowLoad != null))
            {
                R.free(S.OutflowLoad);
                S.OutflowLoad = (null);
            }
        }
        ;
        if ((S.Frunoff.file != null))
        {
            if (S.Frunoff.mode == S.SAVE_FILE)
            {
                R.fseek(S.Frunoff.file, S.MaxStepsPos, 0);
                R.fwriteI1(S.runoff_Nsteps, S.Frunoff.file);
            }
            R.fclose(S.Frunoff.file);
        }
        if ((S.Fclimate.file != null))
        {
            R.fclose(S.Fclimate.file);
        }
    }

    // runoff.c:155
    public static void runoff_execute()
    {
        int j = 0;
        int day = 0;
        double runoffStep = 0.0;
        double oldRunoffStep = 0.0;
        double runoff = 0.0;
        double currentDate = 0.0;
        int canSweep = 0;
        if ((S.ErrorCode != 0))
        {
            return;
        }
        oldRunoffStep = (S.NewRunoffTime - S.OldRunoffTime) / 1000.0;
        currentDate = S.getDateTime(S.NewRunoffTime);
        S.climate_setState(currentDate);
        if (S.Nobjects[S.SUBCATCH] == 0)
        {
            S.OldRunoffTime = S.NewRunoffTime;
            S.NewRunoffTime += (double)((S.DryStep)) * 1000.0;
            S.NewRunoffTime = ((((S.NewRunoffTime) <= (S.TotalDuration)) ? (S.NewRunoffTime) : (S.TotalDuration)));
            return;
        }
        S.IsRaining = 0;
        for (j = 0; j < S.Nobjects[S.GAGE]; j++)
        {
            S.gage_setState(j, currentDate);
            if (S.Gage[j].rainfall > 0.0)
            {
                S.IsRaining = 1;
            }
        }
        if (S.Frunoff.mode == S.USE_FILE)
        {
            S.runoff_readFromFile();
            return;
        }
        day = S.datetime_dayOfYear(currentDate);
        canSweep = 0;
        if (S.SweepStart <= S.SweepEnd)
        {
            if ((day >= S.SweepStart) && (day <= S.SweepEnd))
            {
                canSweep = 1;
            }
        }
        else if ((day <= S.SweepEnd) || (day >= S.SweepStart))
        {
            canSweep = 1;
        }
        runoffStep = S.runoff_getTimeStep(currentDate);
        if (runoffStep <= 0.0)
        {
            S.ErrorCode = S.ERR_TIMESTEP;
            return;
        }
        S.OldRunoffTime = S.NewRunoffTime;
        S.NewRunoffTime += (1000 * runoffStep);
        if (S.NewRunoffTime > S.TotalDuration)
        {
            runoffStep = (S.TotalDuration - S.OldRunoffTime) / 1000.0;
            S.NewRunoffTime = S.TotalDuration;
        }
        for (j = 0; j < S.Nobjects[S.GAGE]; j++)
        {
            S.gage_updatePastRain(j, R.D2I(runoffStep));
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.subcatch_setOldState(j);
        }
        if (oldRunoffStep > 0.0)
        {
            S.runoff_getOutfallRunon(oldRunoffStep);
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            if (S.Subcatch[j].area == 0.0)
            {
                continue;
            }
            S.subcatch_getRunon(j);
            if (!(S.IgnoreSnowmelt != 0))
            {
                S.snow_plowSnow(j, runoffStep);
            }
        }
        S.HasSnow = 0;
        S.HasRunoff = 0;
        S.HasWetLids = 0;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            if (S.Subcatch[j].area == 0.0)
            {
                continue;
            }
            runoff = S.subcatch_getRunoff(j, runoffStep);
            if (runoff > 0.0)
            {
                S.HasRunoff = 1;
            }
            if (S.Subcatch[j].newSnowDepth > 0.0)
            {
                S.HasSnow = 1;
            }
            if ((S.IgnoreQuality != 0))
            {
                continue;
            }
            if (runoff < 2.31481e-08)
            {
                S.surfqual_getBuildup(j, runoffStep);
            }
            if ((canSweep != 0) && (S.Subcatch[j].rainfall <= 2.31481e-08))
            {
                S.surfqual_sweepBuildup(j, currentDate);
            }
            S.surfqual_getWashoff(j, runoff, runoffStep);
        }
        S.stats_updateMaxRunoff();
        S.runoff_Nsteps++;
        if (S.Frunoff.mode == S.SAVE_FILE)
        {
            S.runoff_saveToFile((float)(runoffStep));
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.Subcatch[j].runon = 0.0;
        }
    }

    // runoff.c:299
    public static double runoff_getTimeStep(double currentDate)
    {
        int j = 0;
        long timeStep = 0L;
        long maxStep = S.DryStep;
        timeStep = S.datetime_timeDiff(S.climate_getNextEvapDate(), currentDate);
        if ((timeStep > 0.0) && (timeStep < maxStep))
        {
            maxStep = timeStep;
        }
        for (j = 0; j < S.Nobjects[S.GAGE]; j++)
        {
            timeStep = S.datetime_timeDiff(S.gage_getNextRainDate(j, currentDate), currentDate);
            if ((timeStep > 0) && (timeStep < maxStep))
            {
                maxStep = timeStep;
            }
        }
        if ((((S.IsRaining != 0) || (S.HasSnow != 0)) || (S.HasRunoff != 0)) || (S.HasWetLids != 0))
        {
            timeStep = S.WetStep;
        }
        else
        {
            timeStep = S.DryStep;
        }
        if (timeStep > maxStep)
        {
            timeStep = maxStep;
        }
        return (double)(timeStep);
    }

    // runoff.c:335
    public static void runoff_initFile()
    {
        int nSubcatch = 0;
        int nPollut = 0;
        int flowUnits = 0;
        CPtr fileStamp = R.BufFrom(S.L533, 13);
        CPtr fStamp = R.BufFrom(S.L533, 13);
        S.MaxSteps = 0;
        if (S.Frunoff.mode == S.SAVE_FILE)
        {
            nSubcatch = S.Nobjects[S.SUBCATCH];
            nPollut = S.Nobjects[S.POLLUT];
            flowUnits = S.FlowUnits;
            R.fwriteC(fileStamp, 0, (int)(R.strlen(fileStamp)), S.Frunoff.file);
            R.fwriteI1(nSubcatch, S.Frunoff.file);
            R.fwriteI1(nPollut, S.Frunoff.file);
            R.fwriteI1(flowUnits, S.Frunoff.file);
            S.MaxStepsPos = R.ftell(S.Frunoff.file);
            R.fwriteI1(S.MaxSteps, S.Frunoff.file);
        }
        if (S.Frunoff.mode == S.USE_FILE)
        {
            R.freadC(fStamp, 0, (int)(R.strlen(fileStamp)), S.Frunoff.file);
            if (R.strcmp(fStamp, fileStamp) != 0)
            {
                S.report_writeErrorMsg(S.ERR_RUNOFF_FILE_FORMAT, S.L0);
                return;
            }
            nSubcatch = -1;
            nPollut = -1;
            flowUnits = -1;
            R.freadI1(ref nSubcatch, S.Frunoff.file);
            R.freadI1(ref nPollut, S.Frunoff.file);
            R.freadI1(ref flowUnits, S.Frunoff.file);
            R.freadI1(ref S.MaxSteps, S.Frunoff.file);
            if ((((nSubcatch != S.Nobjects[S.SUBCATCH]) || (nPollut != S.Nobjects[S.POLLUT])) || (flowUnits != S.FlowUnits)) || (S.MaxSteps <= 0))
            {
                S.report_writeErrorMsg(S.ERR_RUNOFF_FILE_FORMAT, S.L0);
            }
        }
    }

    // runoff.c:391
    public static void runoff_saveToFile(float tStep)
    {
        int j = 0;
        int n = 9 + S.Nobjects[S.POLLUT] - 1;
        R.fwriteF1(tStep, S.Frunoff.file);
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.subcatch_getResults(j, 1.0, S.SubcatchResults);
            R.fwriteF(S.SubcatchResults, 0, (int)(unchecked((ulong)(n))), S.Frunoff.file);
        }
    }

    // runoff.c:412
    public static void runoff_readFromFile()
    {
        int i = 0;
        int j = 0;
        int nResults = 0;
        int kount = 0;
        float tStep = 0.0f;
        TGroundwater gw = null;
        if (S.runoff_Nsteps > S.MaxSteps)
        {
            S.report_writeErrorMsg(S.ERR_RUNOFF_FILE_END, S.L0);
            return;
        }
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            S.subcatch_setOldState(j);
        }
        kount = 0;
        kount += (int)(R.freadF1(ref tStep, S.Frunoff.file));
        nResults = 9 + S.Nobjects[S.POLLUT] - 1;
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            kount += (int)(R.freadF(S.SubcatchResults, 0, (int)(unchecked((ulong)(nResults))), S.Frunoff.file));
            S.Subcatch[j].newSnowDepth = S.SubcatchResults[S.SUBCATCH_SNOWDEPTH] / S.UCF(S.RAINDEPTH);
            S.Subcatch[j].evapLoss = S.SubcatchResults[S.SUBCATCH_EVAP] / S.UCF(S.RAINFALL);
            S.Subcatch[j].infilLoss = S.SubcatchResults[S.SUBCATCH_INFIL] / S.UCF(S.RAINFALL);
            S.Subcatch[j].newRunoff = S.SubcatchResults[S.SUBCATCH_RUNOFF] / S.UCF(S.FLOW);
            gw = S.Subcatch[j].groundwater;
            if ((gw != null))
            {
                gw.newFlow = S.SubcatchResults[S.SUBCATCH_GW_FLOW] / S.UCF(S.FLOW);
                gw.lowerDepth = S.Aquifer[gw.aquifer].bottomElev - (S.SubcatchResults[S.SUBCATCH_GW_ELEV] / S.UCF(S.LENGTH));
                gw.theta = S.SubcatchResults[S.SUBCATCH_SOIL_MOIST];
            }
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                S.Subcatch[j].newQual[i] = S.SubcatchResults[S.SUBCATCH_WASHOFF + i];
            }
        }
        if (kount < 1 + S.Nobjects[S.SUBCATCH] * nResults)
        {
            S.report_writeErrorMsg(S.ERR_RUNOFF_FILE_READ, S.L0);
            return;
        }
        S.OldRunoffTime = S.NewRunoffTime;
        S.NewRunoffTime = S.OldRunoffTime + (tStep) * 1000.0;
        S.NewRunoffTime = ((((S.NewRunoffTime) <= (S.TotalDuration)) ? (S.NewRunoffTime) : (S.TotalDuration)));
        S.runoff_Nsteps++;
    }

    // runoff.c:491
    public static void runoff_getOutfallRunon(double tStep)
    {
        int i = 0;
        int k = 0;
        int p = 0;
        double w = 0.0;
        for (i = 0; i < S.Nnodes[S.OUTFALL]; i++)
        {
            k = S.Outfall[i].routeTo;
            if (k < 0)
            {
                continue;
            }
            if (S.Subcatch[k].area == 0.0)
            {
                continue;
            }
            S.subcatch_addRunonFlow(k, S.Outfall[i].vRouted / tStep);
            S.massbal_updateRunoffTotals(S.RUNOFF_RUNON, S.Outfall[i].vRouted);
            S.Outfall[i].vRouted = 0.0;
            for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
            {
                w = S.Outfall[i].wRouted[p] * 28.317;
                S.massbal_updateLoadingTotals(S.DEPOSITION_LOAD, p, w * S.Pollut[p].mcf);
                S.Subcatch[k].newQual[p] += w / tStep;
                S.Outfall[i].wRouted[p] = 0.0;
            }
        }
    }

}
}
