// Generated from EPA SWMM 5.2.4 (report.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static long SysTime = 0L;

    // report.c:93
    public static int report_readOptions(CPtr[] tok, int ntoks)
    {
        int k = 0;
        int j = 0;
        int m = 0;
        int t = 0;
        if (ntoks < 2)
        {
            return S.error_setInpError(S.ERR_ITEMS, S.L0);
        }
        k = S.findmatch(tok[0], S.ReportWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[0]);
        }
        if ((k < 2) || (k > 4))
        {
            m = S.findmatch(tok[1], S.NoYesWords);
            if ((m < S.NO) || (m > S.YES))
            {
                return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
            }
            switch (k)
            {
                case 0:
                    S.RptFlags.disabled = m;
                    return 0;
                case 1:
                    S.RptFlags.input = m;
                    return 0;
                case 5:
                    S.RptFlags.continuity = m;
                    return 0;
                case 6:
                    S.RptFlags.flowStats = m;
                    return 0;
                case 7:
                    S.RptFlags.controls = m;
                    return 0;
                case 8:
                    S.RptFlags.averages = m;
                    return 0;
                case 9:
                    return 0;
                default:
                    return S.error_setInpError(S.ERR_KEYWORD, tok[1]);
            }
        }
        else
        {
            switch (k)
            {
                case 2:
                    m = S.SUBCATCH;
                    break;
                case 3:
                    m = S.NODE;
                    break;
                case 4:
                    m = S.LINK;
                    break;
            }
        }
        if ((S.strcomp(tok[1], S.L355) != 0))
        {
            k = S.NONE;
        }
        else if ((S.strcomp(tok[1], S.L351) != 0))
        {
            k = S.ALL;
        }
        else
        {
            k = S.SOME;
            for (t = 1; t < ntoks; t++)
            {
                j = S.project_findObject(m, tok[t]);
                if (j < 0)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[t]);
                }
                switch (m)
                {
                    case S.SUBCATCH:
                        S.Subcatch[j].rptFlag = 1;
                        break;
                    case S.NODE:
                        S.Node[j].rptFlag = 1;
                        break;
                    case S.LINK:
                        S.Link[j].rptFlag = 1;
                        break;
                }
            }
        }
        switch (m)
        {
            case S.SUBCATCH:
                S.RptFlags.subcatchments = k;
                break;
            case S.NODE:
                S.RptFlags.nodes = k;
                break;
            case S.LINK:
                S.RptFlags.links = k;
                break;
        }
        return 0;
    }

    // report.c:171
    public static void report_writeLine(CPtr line)
    {
        if ((S.Frpt.file != null))
        {
            R.fprintf(S.Frpt.file, S.L356, line);
        }
    }

    // report.c:183
    public static void report_writeSysTime()
    {
        CPtr theTime = R.Buf(9);
        double elapsedTime = 0.0;
        long endTime = 0L;
        if ((S.Frpt.file != null))
        {
            R.fprintf(S.Frpt.file, S.L357, R.ctime(S.SysTime));
            endTime = R.time();
            R.fprintf(S.Frpt.file, S.L358, R.ctime(endTime));
            elapsedTime = (double)(endTime - S.SysTime);
            R.fprintf(S.Frpt.file, S.L359);
            if (elapsedTime < 1.0)
            {
                R.fprintf(S.Frpt.file, S.L360);
            }
            else
            {
                elapsedTime /= 86400;
                if (elapsedTime >= 1.0)
                {
                    R.fprintf(S.Frpt.file, S.L361, R.D2I(Math.Floor(elapsedTime)));
                    elapsedTime -= Math.Floor(elapsedTime);
                }
                S.datetime_timeToStr(elapsedTime, theTime);
                R.fprintf(S.Frpt.file, S.L169, theTime);
            }
        }
    }

    // report.c:220
    public static void report_writeLogo()
    {
        R.fprintf(S.Frpt.file, S.L362);
        R.fprintf(S.Frpt.file, S.L363);
        R.fprintf(S.Frpt.file, S.L276);
        S.SysTime = R.time();
    }

    // report.c:235
    public static void report_writeTitle()
    {
        int i = 0;
        int lineCount = 0;
        if ((S.ErrorCode != 0))
        {
            return;
        }
        for (i = 0; i < 3; i++)
        {
            if (R.strlen(S.Title[i]) > unchecked((ulong)(0)))
            {
                S.report_writeLine((S.Title[i]));
                lineCount++;
            }
        }
    }

    // report.c:254
    public static void report_writeOptions()
    {
        CPtr str = R.Buf(80);
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L227));
        S.report_writeLine((S.L364));
        S.report_writeLine((S.L227));
        R.fprintf(S.Frpt.file, S.L365, S.FlowUnitWords[S.FlowUnits]);
        R.fprintf(S.Frpt.file, S.L366);
        R.fprintf(S.Frpt.file, S.L367);
        if ((S.IgnoreRainfall != 0) || (S.Nobjects[S.GAGE] == 0))
        {
            R.fprintf(S.Frpt.file, S.L4);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L5);
        }
        R.fprintf(S.Frpt.file, S.L368);
        if ((S.IgnoreRDII != 0) || (S.Nobjects[S.UNITHYD] == 0))
        {
            R.fprintf(S.Frpt.file, S.L4);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L5);
        }
        R.fprintf(S.Frpt.file, S.L369);
        if ((S.IgnoreSnowmelt != 0) || (S.Nobjects[S.SNOWMELT] == 0))
        {
            R.fprintf(S.Frpt.file, S.L4);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L5);
        }
        R.fprintf(S.Frpt.file, S.L370);
        if ((S.IgnoreGwater != 0) || (S.Nobjects[S.AQUIFER] == 0))
        {
            R.fprintf(S.Frpt.file, S.L4);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L5);
        }
        R.fprintf(S.Frpt.file, S.L371);
        if ((S.IgnoreRouting != 0) || (S.Nobjects[S.LINK] == 0))
        {
            R.fprintf(S.Frpt.file, S.L4);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L5);
            R.fprintf(S.Frpt.file, S.L372);
            if ((S.AllowPonding != 0))
            {
                R.fprintf(S.Frpt.file, S.L5);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L4);
            }
        }
        R.fprintf(S.Frpt.file, S.L373);
        if ((S.IgnoreQuality != 0) || (S.Nobjects[S.POLLUT] == 0))
        {
            R.fprintf(S.Frpt.file, S.L4);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L5);
        }
        if (S.Nobjects[S.SUBCATCH] > 0)
        {
            R.fprintf(S.Frpt.file, S.L374, S.InfilModelWords[S.InfilModel]);
        }
        if (S.Nobjects[S.LINK] > 0)
        {
            R.fprintf(S.Frpt.file, S.L375, S.RouteModelWords[S.RouteModel]);
        }
        if (S.RouteModel == S.DW)
        {
            R.fprintf(S.Frpt.file, S.L376, S.SurchargeWords[S.SurchargeMethod]);
        }
        S.datetime_dateToStr(S.StartDate, str);
        R.fprintf(S.Frpt.file, S.L377, str);
        S.datetime_timeToStr(S.StartTime, str);
        R.fprintf(S.Frpt.file, S.L378, str);
        S.datetime_dateToStr(S.EndDate, str);
        R.fprintf(S.Frpt.file, S.L379, str);
        S.datetime_timeToStr(S.EndTime, str);
        R.fprintf(S.Frpt.file, S.L378, str);
        R.fprintf(S.Frpt.file, S.L380, S.StartDryDays);
        S.datetime_timeToStr(S.datetime_encodeTime(0, 0, S.ReportStep), str);
        R.fprintf(S.Frpt.file, S.L381, str);
        if (S.Nobjects[S.SUBCATCH] > 0)
        {
            S.datetime_timeToStr(S.datetime_encodeTime(0, 0, S.WetStep), str);
            R.fprintf(S.Frpt.file, S.L382, str);
            S.datetime_timeToStr(S.datetime_encodeTime(0, 0, S.DryStep), str);
            R.fprintf(S.Frpt.file, S.L383, str);
        }
        if (S.Nobjects[S.LINK] > 0)
        {
            R.fprintf(S.Frpt.file, S.L384, S.RouteStep);
            if (S.RouteModel == S.DW)
            {
                R.fprintf(S.Frpt.file, S.L385);
                if (S.CourantFactor > 0.0)
                {
                    R.fprintf(S.Frpt.file, S.L5);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L4);
                }
                R.fprintf(S.Frpt.file, S.L386, S.MaxTrials);
                R.fprintf(S.Frpt.file, S.L387, S.NumThreads);
                R.fprintf(S.Frpt.file, S.L388, S.HeadTol * S.UCF(S.LENGTH));
                if (S.UnitSystem == S.US)
                {
                    R.fprintf(S.Frpt.file, S.L389);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L390);
                }
            }
        }
        S.report_writeLine((S.L0));
    }

    // report.c:355
    public static void report_writeRainStats(int i, TRainStats r)
    {
        CPtr date1 = R.BufFrom(S.L391, 12);
        CPtr date2 = R.BufFrom(S.L391, 12);
        if ((S.RptFlags.disabled != 0))
        {
            return;
        }
        if (i < 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L255));
            S.report_writeLine((S.L392));
            S.report_writeLine((S.L255));
            R.fprintf(S.Frpt.file, S.L393);
            R.fprintf(S.Frpt.file, S.L394);
            R.fprintf(S.Frpt.file, S.L245);
        }
        else
        {
            if (r.startDate != -693594)
            {
                S.datetime_dateToStr(r.startDate, date1);
            }
            if (r.endDate != -693594)
            {
                S.datetime_dateToStr(r.endDate, date2);
            }
            R.fprintf(S.Frpt.file, S.L395, S.Gage[i].staID, date1, date2, S.Gage[i].rainInterval / 60, r.periodsRain, r.periodsMissing, r.periodsMalfunc);
        }
        S.report_writeLine((S.L0));
    }

    // report.c:396
    public static void report_writeRdiiStats(double rainVol, double rdiiVol)
    {
        double ratio = 0.0;
        double ucf1 = 0.0;
        double ucf2 = 0.0;
        if ((S.RptFlags.disabled != 0))
        {
            return;
        }
        ucf1 = S.UCF(S.LENGTH) * S.UCF(S.LANDAREA);
        if (S.UnitSystem == S.US)
        {
            ucf2 = 0.64632 / 86400;
        }
        else
        {
            ucf2 = 2.4466 / 86400;
        }
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L396);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L397);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L398);
        }
        R.fprintf(S.Frpt.file, S.L399);
        R.fprintf(S.Frpt.file, S.L400, rainVol * ucf1, rainVol * ucf2);
        R.fprintf(S.Frpt.file, S.L401, rdiiVol * ucf1, rdiiVol * ucf2);
        if (rainVol == 0.0)
        {
            ratio = 0.0;
        }
        else
        {
            ratio = rdiiVol / rainVol;
        }
        R.fprintf(S.Frpt.file, S.L402, ratio);
        S.report_writeLine((S.L0));
    }

    // report.c:441
    public static void report_writeControlActionsHeading()
    {
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L255));
        S.report_writeLine((S.L403));
        S.report_writeLine((S.L255));
        R.fprintf(S.Frpt.file, S.L276);
    }

    // report.c:452
    public static void report_writeControlAction(double aDate, CPtr linkID, double value, CPtr ruleID)
    {
        CPtr theDate = R.Buf(12);
        CPtr theTime = R.Buf(9);
        if ((S.RptFlags.disabled != 0))
        {
            return;
        }
        S.datetime_dateToStr(aDate, theDate);
        S.datetime_timeToStr(aDate, theTime);
        R.fprintf(S.Frpt.file, S.L404, theDate, theTime, linkID, value, ruleID);
    }

    // report.c:480
    public static void report_writeRunoffError(TRunoffTotals totals, double totalArea)
    {
        if (S.Frunoff.mode == S.USE_FILE)
        {
            S.report_writeLine((S.L0));
            R.fprintf(S.Frpt.file, S.L405, S.Frunoff.name);
            S.report_writeLine((S.L0));
            return;
        }
        if (totalArea == 0.0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L406);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L407);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L408);
        }
        R.fprintf(S.Frpt.file, S.L409);
        if (totals.initStorage > 0.0)
        {
            R.fprintf(S.Frpt.file, S.L410, totals.initStorage * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.initStorage / totalArea * S.UCF(S.RAINDEPTH));
        }
        if (S.Nobjects[S.SNOWMELT] > 0)
        {
            R.fprintf(S.Frpt.file, S.L411, totals.initSnowCover * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.initSnowCover / totalArea * S.UCF(S.RAINDEPTH));
        }
        R.fprintf(S.Frpt.file, S.L412, totals.rainfall * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.rainfall / totalArea * S.UCF(S.RAINDEPTH));
        if (totals.runon > 0.0)
        {
            R.fprintf(S.Frpt.file, S.L413, totals.runon * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.runon / totalArea * S.UCF(S.RAINDEPTH));
        }
        R.fprintf(S.Frpt.file, S.L414, totals.evap * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.evap / totalArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L415, totals.infil * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.infil / totalArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L416, totals.runoff * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.runoff / totalArea * S.UCF(S.RAINDEPTH));
        if (totals.drains > 0.0)
        {
            R.fprintf(S.Frpt.file, S.L417, totals.drains * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.drains / totalArea * S.UCF(S.RAINDEPTH));
        }
        if (S.Nobjects[S.SNOWMELT] > 0)
        {
            R.fprintf(S.Frpt.file, S.L418, totals.snowRemoved * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.snowRemoved / totalArea * S.UCF(S.RAINDEPTH));
            R.fprintf(S.Frpt.file, S.L419, totals.finalSnowCover * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.finalSnowCover / totalArea * S.UCF(S.RAINDEPTH));
        }
        R.fprintf(S.Frpt.file, S.L420, totals.finalStorage * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.finalStorage / totalArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L421, totals.pctError);
        S.report_writeLine((S.L0));
    }

    // report.c:577
    public static void report_writeLoadingError(TLoadingTotals[] totals)
    {
        int p1 = 0;
        int p2 = 0;
        p1 = 1;
        p2 = ((((5) <= (S.Nobjects[S.POLLUT])) ? (5) : (S.Nobjects[S.POLLUT])));
        while (p1 <= S.Nobjects[S.POLLUT])
        {
            S.report_LoadingErrors(p1 - 1, p2 - 1, totals);
            p1 = p2 + 1;
            p2 = p1 + 4;
            p2 = ((((p2) <= (S.Nobjects[S.POLLUT])) ? (p2) : (S.Nobjects[S.POLLUT])));
        }
    }

    // report.c:599
    public static void report_LoadingErrors(int p1, int p2, TLoadingTotals[] totals)
    {
        int i = 0;
        int p = 0;
        double cf = 1.0;
        CPtr units = R.Buf(15);
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L422);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L423, S.Pollut[p].ID);
        }
        R.fprintf(S.Frpt.file, S.L424);
        for (p = p1; p <= p2; p++)
        {
            i = S.UnitSystem;
            if (S.Pollut[p].units == S.COUNT)
            {
                i = 2;
            }
            S.sstrncpy(units, S.LoadUnitsWords[i], unchecked((ulong)(14)));
            R.fprintf(S.Frpt.file, S.L423, units);
        }
        R.fprintf(S.Frpt.file, S.L422);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L425);
        }
        R.fprintf(S.Frpt.file, S.L426);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].initLoad * cf);
        }
        R.fprintf(S.Frpt.file, S.L428);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].buildup * cf);
        }
        R.fprintf(S.Frpt.file, S.L429);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].deposition * cf);
        }
        R.fprintf(S.Frpt.file, S.L430);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].sweeping * cf);
        }
        R.fprintf(S.Frpt.file, S.L431);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].infil * cf);
        }
        R.fprintf(S.Frpt.file, S.L432);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].bmpRemoval * cf);
        }
        R.fprintf(S.Frpt.file, S.L433);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].runoff * cf);
        }
        R.fprintf(S.Frpt.file, S.L434);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].finalLoad * cf);
        }
        R.fprintf(S.Frpt.file, S.L435);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, totals[p].pctError);
        }
        S.report_writeLine((S.L0));
    }

    // report.c:685
    public static void report_writeGwaterError(TGwaterTotals totals, double gwArea)
    {
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L406);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L436);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L437);
        }
        R.fprintf(S.Frpt.file, S.L409);
        R.fprintf(S.Frpt.file, S.L438, totals.initStorage * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.initStorage / gwArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L439, totals.infil * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.infil / gwArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L440, totals.upperEvap * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.upperEvap / gwArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L441, totals.lowerEvap * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.lowerEvap / gwArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L442, totals.lowerPerc * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.lowerPerc / gwArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L443, totals.gwater * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.gwater / gwArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L420, totals.finalStorage * S.UCF(S.LENGTH) * S.UCF(S.LANDAREA), totals.finalStorage / gwArea * S.UCF(S.RAINDEPTH));
        R.fprintf(S.Frpt.file, S.L421, totals.pctError);
        S.report_writeLine((S.L0));
    }

    // report.c:737
    public static void report_writeFlowError(TRoutingTotals totals)
    {
        double ucf1 = 0.0;
        double ucf2 = 0.0;
        ucf1 = S.UCF(S.LENGTH) * S.UCF(S.LANDAREA);
        if (S.UnitSystem == S.US)
        {
            ucf2 = 0.64632 / 86400;
        }
        else
        {
            ucf2 = 2.4466 / 86400;
        }
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L444);
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L445);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L446);
        }
        R.fprintf(S.Frpt.file, S.L447);
        R.fprintf(S.Frpt.file, S.L448, totals.dwInflow * ucf1, totals.dwInflow * ucf2);
        R.fprintf(S.Frpt.file, S.L449, totals.wwInflow * ucf1, totals.wwInflow * ucf2);
        R.fprintf(S.Frpt.file, S.L450, totals.gwInflow * ucf1, totals.gwInflow * ucf2);
        R.fprintf(S.Frpt.file, S.L451, totals.iiInflow * ucf1, totals.iiInflow * ucf2);
        R.fprintf(S.Frpt.file, S.L452, totals.exInflow * ucf1, totals.exInflow * ucf2);
        R.fprintf(S.Frpt.file, S.L453, totals.outflow * ucf1, totals.outflow * ucf2);
        R.fprintf(S.Frpt.file, S.L454, totals.flooding * ucf1, totals.flooding * ucf2);
        R.fprintf(S.Frpt.file, S.L414, totals.evapLoss * ucf1, totals.evapLoss * ucf2);
        R.fprintf(S.Frpt.file, S.L455, totals.seepLoss * ucf1, totals.seepLoss * ucf2);
        R.fprintf(S.Frpt.file, S.L456, totals.initStorage * ucf1, totals.initStorage * ucf2);
        R.fprintf(S.Frpt.file, S.L457, totals.finalStorage * ucf1, totals.finalStorage * ucf2);
        R.fprintf(S.Frpt.file, S.L421, totals.pctError);
        S.report_writeLine((S.L0));
    }

    // report.c:800
    public static void report_writeQualError(TRoutingTotals[] QualTotals)
    {
        int p1 = 0;
        int p2 = 0;
        p1 = 1;
        p2 = ((((5) <= (S.Nobjects[S.POLLUT])) ? (5) : (S.Nobjects[S.POLLUT])));
        while (p1 <= S.Nobjects[S.POLLUT])
        {
            S.report_QualErrors(p1 - 1, p2 - 1, QualTotals);
            p1 = p2 + 1;
            p2 = p1 + 4;
            p2 = ((((p2) <= (S.Nobjects[S.POLLUT])) ? (p2) : (S.Nobjects[S.POLLUT])));
        }
    }

    // report.c:821
    public static void report_QualErrors(int p1, int p2, TRoutingTotals[] QualTotals)
    {
        int i = 0;
        int p = 0;
        CPtr units = R.Buf(15);
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L422);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L423, S.Pollut[p].ID);
        }
        R.fprintf(S.Frpt.file, S.L458);
        for (p = p1; p <= p2; p++)
        {
            i = S.UnitSystem;
            if (S.Pollut[p].units == S.COUNT)
            {
                i = 2;
            }
            S.sstrncpy(units, S.LoadUnitsWords[i], unchecked((ulong)(14)));
            R.fprintf(S.Frpt.file, S.L423, units);
        }
        R.fprintf(S.Frpt.file, S.L422);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L425);
        }
        R.fprintf(S.Frpt.file, S.L459);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].dwInflow);
        }
        R.fprintf(S.Frpt.file, S.L460);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].wwInflow);
        }
        R.fprintf(S.Frpt.file, S.L461);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].gwInflow);
        }
        R.fprintf(S.Frpt.file, S.L462);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].iiInflow);
        }
        R.fprintf(S.Frpt.file, S.L463);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].exInflow);
        }
        R.fprintf(S.Frpt.file, S.L464);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].outflow);
        }
        R.fprintf(S.Frpt.file, S.L465);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].flooding);
        }
        R.fprintf(S.Frpt.file, S.L466);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].seepLoss);
        }
        R.fprintf(S.Frpt.file, S.L467);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].reacted);
        }
        R.fprintf(S.Frpt.file, S.L468);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].initStorage);
        }
        R.fprintf(S.Frpt.file, S.L469);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].finalStorage);
        }
        R.fprintf(S.Frpt.file, S.L435);
        for (p = p1; p <= p2; p++)
        {
            R.fprintf(S.Frpt.file, S.L427, QualTotals[p].pctError);
        }
        S.report_writeLine((S.L0));
    }

    // report.c:923
    public static void report_writeMaxStats(TMaxStats[] maxMassBalErrs, TMaxStats[] maxCourantCrit, int nMaxStats)
    {
        int i = 0;
        int j = 0;
        int k = 0;
        if ((S.RouteModel != S.DW) || (S.Nobjects[S.LINK] == 0))
        {
            return;
        }
        if (nMaxStats <= 0)
        {
            return;
        }
        if (maxMassBalErrs[0].index >= 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L470));
            S.report_writeLine((S.L471));
            S.report_writeLine((S.L470));
            for (i = 0; i < nMaxStats; i++)
            {
                j = maxMassBalErrs[i].index;
                if (j < 0)
                {
                    continue;
                }
                R.fprintf(S.Frpt.file, S.L472, S.Node[j].ID, maxMassBalErrs[i].value);
            }
            S.report_writeLine((S.L0));
        }
        if (S.CourantFactor == 0.0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L473));
        S.report_writeLine((S.L474));
        S.report_writeLine((S.L473));
        k = 0;
        for (i = 0; i < nMaxStats; i++)
        {
            j = maxCourantCrit[i].index;
            if (j < 0)
            {
                continue;
            }
            k++;
            if (maxCourantCrit[i].objType == S.NODE)
            {
                R.fprintf(S.Frpt.file, S.L475, S.Node[j].ID);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L476, S.Link[j].ID);
            }
            R.fprintf(S.Frpt.file, S.L477, maxCourantCrit[i].value);
        }
        if (k == 0)
        {
            R.fprintf(S.Frpt.file, S.L478);
        }
        S.report_writeLine((S.L0));
    }

    // report.c:977
    public static void report_writeMaxFlowTurns(TMaxStats[] flowTurns, int nMaxStats)
    {
        int i = 0;
        int j = 0;
        if (S.Nobjects[S.LINK] == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L479));
        S.report_writeLine((S.L480));
        S.report_writeLine((S.L479));
        if ((nMaxStats <= 0) || (flowTurns[0].index <= 0))
        {
            R.fprintf(S.Frpt.file, S.L481);
        }
        else
        {
            for (i = 0; i < nMaxStats; i++)
            {
                j = flowTurns[i].index;
                if (j < 0)
                {
                    continue;
                }
                R.fprintf(S.Frpt.file, S.L482, S.Link[j].ID, flowTurns[i].value);
            }
        }
        S.report_writeLine((S.L0));
    }

    // report.c:1011
    public static void report_writeNonconvergedStats(TMaxStats[] maxNonconverged, int nMaxStats)
    {
        int i = 0;
        int j = 0;
        if ((S.Nobjects[S.NODE] == 0) || (S.RouteModel != S.DW))
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L483));
        S.report_writeLine((S.L484));
        S.report_writeLine((S.L483));
        if (((nMaxStats <= 0) || (maxNonconverged[0].index <= 0)) || (maxNonconverged[0].value < 0.00005))
        {
            R.fprintf(S.Frpt.file, S.L485);
        }
        else
        {
            for (i = 0; i < nMaxStats; i++)
            {
                j = maxNonconverged[i].index;
                if ((j < 0) || (maxNonconverged[i].value <= 0.0))
                {
                    continue;
                }
                R.fprintf(S.Frpt.file, S.L472, S.Node[j].ID, 100.0 * maxNonconverged[i].value);
            }
        }
        S.report_writeLine((S.L0));
    }

    // report.c:1037
    public static void report_writeTimeStepStats(TTimeStepStats timeStepStats)
    {
        double timeStepCount = timeStepStats.timeStepCount;
        double totalRoutingTime = 0.0;
        double fSteadyState = 0.0;
        if ((S.Nobjects[S.LINK] == 0) || (timeStepStats.timeStepCount == 0.0))
        {
            return;
        }
        totalRoutingTime = timeStepStats.steadyStateTime + timeStepStats.routingTime;
        if (totalRoutingTime > 0.0)
        {
            fSteadyState = 100.0 * timeStepStats.steadyStateTime / totalRoutingTime;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L470));
        S.report_writeLine((S.L486));
        S.report_writeLine((S.L470));
        R.fprintf(S.Frpt.file, S.L487, timeStepStats.minTimeStep);
        R.fprintf(S.Frpt.file, S.L488, timeStepStats.routingTime / timeStepCount);
        R.fprintf(S.Frpt.file, S.L489, timeStepStats.maxTimeStep);
        R.fprintf(S.Frpt.file, S.L490, ((((fSteadyState) <= (100.0)) ? (fSteadyState) : (100.0))));
        R.fprintf(S.Frpt.file, S.L491, timeStepStats.trialsCount / timeStepCount);
        R.fprintf(S.Frpt.file, S.L492, 100.0 * (double)(S.NonConvergeCount) / timeStepCount);
        if ((S.RouteModel == S.DW) && (S.CourantFactor > 0.0))
        {
            S.report_RouteStepFreq(timeStepStats);
        }
        S.report_writeLine((S.L0));
    }

    // report.c:1085
    public static void report_RouteStepFreq(TTimeStepStats timeStepStats)
    {
        double totalSteps = 0.0;
        int i = 0;
        for (i = 1; i < 6; i++)
        {
            totalSteps += timeStepStats.timeStepCounts[i];
        }
        if (totalSteps == 0)
        {
            return;
        }
        R.fprintf(S.Frpt.file, S.L493);
        for (i = 1; i < 6; i++)
        {
            R.fprintf(S.Frpt.file, S.L494, timeStepStats.timeStepIntervals[i - 1], timeStepStats.timeStepIntervals[i], 100.0 * (double)((timeStepStats.timeStepCounts[i])) / totalSteps);
        }
    }

    // report.c:1113
    public static void report_writeReport()
    {
        if ((S.ErrorCode != 0))
        {
            return;
        }
        if (S.Nperiods == 0)
        {
            return;
        }
        if ((S.RptFlags.subcatchments != S.NONE) && ((((S.IgnoreRainfall == 0) || (S.IgnoreSnowmelt == 0)) || (S.IgnoreGwater == 0))))
        {
            S.report_Subcatchments();
        }
        if ((S.IgnoreRouting == 1) && (S.IgnoreQuality == 1))
        {
            return;
        }
        if (S.RptFlags.nodes != S.NONE)
        {
            S.report_Nodes();
        }
        if (S.RptFlags.links != S.NONE)
        {
            S.report_Links();
        }
    }

    // report.c:1135
    public static void report_Subcatchments()
    {
        int j = 0;
        int p = 0;
        int k = 0;
        int period = 0;
        double days = 0.0;
        CPtr theDate = R.Buf(12);
        CPtr theTime = R.Buf(9);
        int hasSnowmelt = (((S.Nobjects[S.SNOWMELT] > 0) && (!(S.IgnoreSnowmelt != 0))) ? 1 : 0);
        int hasGwater = (((S.Nobjects[S.AQUIFER] > 0) && (!(S.IgnoreGwater != 0))) ? 1 : 0);
        int hasQuality = (((S.Nobjects[S.POLLUT] > 0) && (!(S.IgnoreQuality != 0))) ? 1 : 0);
        if (S.Nobjects[S.SUBCATCH] == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L479));
        S.report_writeLine((S.L495));
        S.report_writeLine((S.L479));
        for (j = 0; j < S.Nobjects[S.SUBCATCH]; j++)
        {
            k = S.Subcatch[j].rptFlag - 1;
            if (k >= 0)
            {
                S.report_SubcatchHeader(S.Subcatch[j].ID);
                for (period = 1; period <= S.Nperiods; period++)
                {
                    S.output_readDateTime(period, ref days);
                    S.datetime_dateToStr(days, theDate);
                    S.datetime_timeToStr(days, theTime);
                    S.output_readSubcatchResults(period, k);
                    R.fprintf(S.Frpt.file, S.L496, theDate, theTime, S.SubcatchResults[S.SUBCATCH_RAINFALL], S.SubcatchResults[S.SUBCATCH_EVAP] / 24.0 + S.SubcatchResults[S.SUBCATCH_INFIL], S.SubcatchResults[S.SUBCATCH_RUNOFF]);
                    if ((hasSnowmelt != 0))
                    {
                        R.fprintf(S.Frpt.file, S.L497, S.SubcatchResults[S.SUBCATCH_SNOWDEPTH]);
                    }
                    if ((hasGwater != 0))
                    {
                        R.fprintf(S.Frpt.file, S.L498, S.SubcatchResults[S.SUBCATCH_GW_ELEV], S.SubcatchResults[S.SUBCATCH_GW_FLOW]);
                    }
                    if ((hasQuality != 0))
                    {
                        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                        {
                            R.fprintf(S.Frpt.file, S.L499, S.SubcatchResults[S.SUBCATCH_WASHOFF + p]);
                        }
                    }
                }
                S.report_writeLine((S.L0));
            }
        }
    }

    // report.c:1192
    public static void report_SubcatchHeader(CPtr id)
    {
        int i = 0;
        int hasSnowmelt = (((S.Nobjects[S.SNOWMELT] > 0) && (!(S.IgnoreSnowmelt != 0))) ? 1 : 0);
        int hasGwater = (((S.Nobjects[S.AQUIFER] > 0) && (!(S.IgnoreGwater != 0))) ? 1 : 0);
        int hasQuality = (((S.Nobjects[S.POLLUT] > 0) && (!(S.IgnoreQuality != 0))) ? 1 : 0);
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L500, id);
        S.report_writeLine((S.L501));
        if (hasSnowmelt > 0)
        {
            R.fprintf(S.Frpt.file, S.L502);
        }
        if ((hasGwater != 0))
        {
            R.fprintf(S.Frpt.file, S.L503);
            R.fprintf(S.Frpt.file, S.L503);
        }
        if ((hasQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L503);
            }
        }
        R.fprintf(S.Frpt.file, S.L504);
        if ((hasSnowmelt != 0))
        {
            R.fprintf(S.Frpt.file, S.L505);
        }
        if ((hasGwater != 0))
        {
            R.fprintf(S.Frpt.file, S.L506);
        }
        if ((hasQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L507, S.Pollut[i].ID);
            }
        }
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L508, S.FlowUnitWords[S.FlowUnits]);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L509, S.FlowUnitWords[S.FlowUnits]);
        }
        if ((hasSnowmelt != 0))
        {
            if (S.UnitSystem == S.US)
            {
                R.fprintf(S.Frpt.file, S.L510);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L511);
            }
        }
        if ((hasGwater != 0))
        {
            if (S.UnitSystem == S.US)
            {
                R.fprintf(S.Frpt.file, S.L512, S.FlowUnitWords[S.FlowUnits]);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L513, S.FlowUnitWords[S.FlowUnits]);
            }
        }
        if ((hasQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L507, S.QualUnitsWords[S.Pollut[i].units]);
            }
        }
        S.report_writeLine((S.L501));
        if ((hasSnowmelt != 0))
        {
            R.fprintf(S.Frpt.file, S.L502);
        }
        if ((hasGwater != 0))
        {
            R.fprintf(S.Frpt.file, S.L503);
            R.fprintf(S.Frpt.file, S.L503);
        }
        if ((hasQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L503);
            }
        }
    }

    // report.c:1261
    public static void report_Nodes()
    {
        int j = 0;
        int p = 0;
        int k = 0;
        int period = 0;
        double days = 0.0;
        CPtr theDate = R.Buf(12);
        CPtr theTime = R.Buf(9);
        if (S.Nobjects[S.NODE] == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L514));
        S.report_writeLine((S.L515));
        S.report_writeLine((S.L514));
        for (j = 0; j < S.Nobjects[S.NODE]; j++)
        {
            k = S.Node[j].rptFlag - 1;
            if (k >= 0)
            {
                S.report_NodeHeader(S.Node[j].ID);
                for (period = 1; period <= S.Nperiods; period++)
                {
                    S.output_readDateTime(period, ref days);
                    S.datetime_dateToStr(days, theDate);
                    S.datetime_timeToStr(days, theTime);
                    S.output_readNodeResults(period, k);
                    R.fprintf(S.Frpt.file, S.L516, theDate, theTime, S.NodeResults[S.NODE_INFLOW], S.NodeResults[S.NODE_OVERFLOW], S.NodeResults[S.NODE_DEPTH], S.NodeResults[S.NODE_HEAD]);
                    if (!(S.IgnoreQuality != 0))
                    {
                        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                        {
                            R.fprintf(S.Frpt.file, S.L195, S.NodeResults[S.NODE_QUAL + p]);
                        }
                    }
                }
                S.report_writeLine((S.L0));
            }
        }
    }

    // report.c:1305
    public static void report_NodeHeader(CPtr id)
    {
        int i = 0;
        CPtr lengthUnits = R.Buf(9);
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L517, id);
        S.report_writeLine((S.L518));
        for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
        {
            R.fprintf(S.Frpt.file, S.L503);
        }
        R.fprintf(S.Frpt.file, S.L519);
        if (!(S.IgnoreQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L507, S.Pollut[i].ID);
            }
        }
        if (S.UnitSystem == S.US)
        {
            S.sstrncpy(lengthUnits, S.L520, unchecked((ulong)(8)));
        }
        else
        {
            S.sstrncpy(lengthUnits, S.L521, unchecked((ulong)(8)));
        }
        R.fprintf(S.Frpt.file, S.L522, S.FlowUnitWords[S.FlowUnits], S.FlowUnitWords[S.FlowUnits], lengthUnits, lengthUnits);
        if (!(S.IgnoreQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L507, S.QualUnitsWords[S.Pollut[i].units]);
            }
        }
        S.report_writeLine((S.L518));
        if (!(S.IgnoreQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L503);
            }
        }
    }

    // report.c:1339
    public static void report_Links()
    {
        int j = 0;
        int p = 0;
        int k = 0;
        int period = 0;
        double days = 0.0;
        CPtr theDate = R.Buf(12);
        CPtr theTime = R.Buf(9);
        if (S.Nobjects[S.LINK] == 0)
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L514));
        S.report_writeLine((S.L523));
        S.report_writeLine((S.L514));
        for (j = 0; j < S.Nobjects[S.LINK]; j++)
        {
            k = S.Link[j].rptFlag - 1;
            if (k >= 0)
            {
                S.report_LinkHeader(S.Link[j].ID);
                for (period = 1; period <= S.Nperiods; period++)
                {
                    S.output_readDateTime(period, ref days);
                    S.datetime_dateToStr(days, theDate);
                    S.datetime_timeToStr(days, theTime);
                    S.output_readLinkResults(period, k);
                    R.fprintf(S.Frpt.file, S.L516, theDate, theTime, S.LinkResults[S.LINK_FLOW], S.LinkResults[S.LINK_VELOCITY], S.LinkResults[S.LINK_DEPTH], S.LinkResults[S.LINK_CAPACITY]);
                    if (!(S.IgnoreQuality != 0))
                    {
                        for (p = 0; p < S.Nobjects[S.POLLUT]; p++)
                        {
                            R.fprintf(S.Frpt.file, S.L195, S.LinkResults[S.LINK_QUAL + p]);
                        }
                    }
                }
                S.report_writeLine((S.L0));
            }
        }
    }

    // report.c:1383
    public static void report_LinkHeader(CPtr id)
    {
        int i = 0;
        S.report_writeLine((S.L0));
        R.fprintf(S.Frpt.file, S.L524, id);
        S.report_writeLine((S.L518));
        for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
        {
            R.fprintf(S.Frpt.file, S.L503);
        }
        R.fprintf(S.Frpt.file, S.L525);
        if (!(S.IgnoreQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L507, S.Pollut[i].ID);
            }
        }
        if (S.UnitSystem == S.US)
        {
            R.fprintf(S.Frpt.file, S.L526, S.FlowUnitWords[S.FlowUnits]);
        }
        else
        {
            R.fprintf(S.Frpt.file, S.L527, S.FlowUnitWords[S.FlowUnits]);
        }
        if (!(S.IgnoreQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L528, S.QualUnitsWords[S.Pollut[i].units]);
            }
        }
        S.report_writeLine((S.L518));
        if (!(S.IgnoreQuality != 0))
        {
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L503);
            }
        }
    }

    // report.c:1422
    public static void report_writeErrorMsg(int code, CPtr s)
    {
        if ((S.Frpt.file != null))
        {
            S.report_writeLine((S.L0));
            R.fprintf(S.Frpt.file, S.error_getMsg(code, S.Msg), s);
        }
        S.ErrorCode = code;
        if ((S.ErrorCode <= S.ERR_INPUT) || (S.ErrorCode >= S.ERR_FILE_NAME))
        {
            R.snprintf(S.ErrorMsg, unchecked((ulong)(1024)), S.error_getMsg(S.ErrorCode, S.Msg), s);
        }
    }

    // report.c:1446
    public static void report_writeErrorCode()
    {
        if ((S.Frpt.file != null))
        {
            if (((((S.ErrorCode >= S.ERR_MEMORY) && (S.ErrorCode <= S.ERR_TIMESTEP))) || (((S.ErrorCode >= S.ERR_FILE_NAME) && (S.ErrorCode <= S.ERR_OUT_FILE)))) || (S.ErrorCode == S.ERR_SYSTEM))
            {
                R.fprintf(S.Frpt.file, S.L169, S.error_getMsg(S.ErrorCode, S.Msg));
            }
        }
    }

    // report.c:1464
    public static void report_writeInputErrorMsg(int k, int sect, CPtr line, long lineCount)
    {
        if ((S.Frpt.file != null))
        {
            S.report_writeErrorMsg(k, S.ErrString);
            if (sect < 0)
            {
                R.fprintf(S.Frpt.file, S.L529, lineCount);
            }
            else
            {
                R.fprintf(S.Frpt.file, S.L530, lineCount, S.SectWords[sect]);
            }
            R.fprintf(S.Frpt.file, S.L356, line);
        }
    }

    // report.c:1485
    public static void report_writeWarningMsg(CPtr msg, CPtr id)
    {
        R.fprintf(S.Frpt.file, S.L531, msg, id);
        S.Warnings++;
    }

    // report.c:1499
    public static void report_writeTseriesErrorMsg(int code, TTable tseries)
    {
        CPtr theDate = R.Buf(20);
        CPtr theTime = R.Buf(20);
        double x = 0.0;
        if (code == S.ERR_CURVE_SEQUENCE)
        {
            x = tseries.x2;
            S.datetime_dateToStr(x, theDate);
            S.datetime_timeToStr(x, theTime);
            S.report_writeErrorMsg(S.ERR_TIMESERIES_SEQUENCE, tseries.ID);
            R.fprintf(S.Frpt.file, S.L532, theDate, theTime);
        }
        else
        {
            S.report_writeErrorMsg(code, tseries.ID);
        }
    }

}
}
