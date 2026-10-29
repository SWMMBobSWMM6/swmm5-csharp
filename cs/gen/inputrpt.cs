// Generated from EPA SWMM 5.2.4 (inputrpt.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    // inputrpt.c:27
    public static void inputrpt_writeInput()
    {
        int m = 0;
        int i = 0;
        int k = 0;
        int lidCount = 0;
        if ((S.ErrorCode != 0))
        {
            return;
        }
        S.report_writeLine((S.L0));
        S.report_writeLine((S.L206));
        S.report_writeLine((S.L207));
        S.report_writeLine((S.L206));
        R.fprintf(S.Frpt.file, S.L208, S.Nobjects[S.GAGE]);
        R.fprintf(S.Frpt.file, S.L209, S.Nobjects[S.SUBCATCH]);
        R.fprintf(S.Frpt.file, S.L210, S.Nobjects[S.NODE]);
        R.fprintf(S.Frpt.file, S.L211, S.Nobjects[S.LINK]);
        R.fprintf(S.Frpt.file, S.L212, S.Nobjects[S.POLLUT]);
        R.fprintf(S.Frpt.file, S.L213, S.Nobjects[S.LANDUSE]);
        if (S.Nobjects[S.POLLUT] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L214));
            S.report_writeLine((S.L215));
            S.report_writeLine((S.L214));
            R.fprintf(S.Frpt.file, S.L216);
            R.fprintf(S.Frpt.file, S.L217);
            R.fprintf(S.Frpt.file, S.L218);
            for (i = 0; i < S.Nobjects[S.POLLUT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L219, S.Pollut[i].ID, S.QualUnitsWords[S.Pollut[i].units], S.Pollut[i].pptConcen, S.Pollut[i].gwConcen, S.Pollut[i].kDecay * 86400);
                if (S.Pollut[i].coPollut >= 0)
                {
                    R.fprintf(S.Frpt.file, S.L220, S.Pollut[S.Pollut[i].coPollut].ID, S.Pollut[i].coFraction);
                }
            }
        }
        if (S.Nobjects[S.LANDUSE] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L221));
            S.report_writeLine((S.L222));
            S.report_writeLine((S.L221));
            R.fprintf(S.Frpt.file, S.L223);
            R.fprintf(S.Frpt.file, S.L224);
            R.fprintf(S.Frpt.file, S.L225);
            for (i = 0; i < S.Nobjects[S.LANDUSE]; i++)
            {
                R.fprintf(S.Frpt.file, S.L226, S.Landuse[i].ID, S.Landuse[i].sweepInterval, S.Landuse[i].sweepRemoval, S.Landuse[i].sweepDays0);
            }
        }
        if (S.Nobjects[S.GAGE] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L227));
            S.report_writeLine((S.L228));
            S.report_writeLine((S.L227));
            R.fprintf(S.Frpt.file, S.L229);
            R.fprintf(S.Frpt.file, S.L230);
            R.fprintf(S.Frpt.file, S.L231);
            for (i = 0; i < S.Nobjects[S.GAGE]; i++)
            {
                if (S.Gage[i].tSeries >= 0)
                {
                    R.fprintf(S.Frpt.file, S.L232, S.Gage[i].ID, S.Tseries[S.Gage[i].tSeries].ID);
                    R.fprintf(S.Frpt.file, S.L233, S.RainTypeWords[S.Gage[i].rainType], (S.Gage[i].rainInterval) / 60);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L234, S.Gage[i].ID, S.Gage[i].fname);
                }
            }
        }
        if (S.Nobjects[S.SUBCATCH] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L235));
            S.report_writeLine((S.L236));
            S.report_writeLine((S.L235));
            R.fprintf(S.Frpt.file, S.L237);
            R.fprintf(S.Frpt.file, S.L238);
            for (i = 0; i < S.Nobjects[S.SUBCATCH]; i++)
            {
                R.fprintf(S.Frpt.file, S.L239, S.Subcatch[i].ID, S.Subcatch[i].area * S.UCF(S.LANDAREA), S.Subcatch[i].width * S.UCF(S.LENGTH), S.Subcatch[i].fracImperv * 100.0, S.Subcatch[i].slope * 100.0, S.Gage[S.Subcatch[i].gage].ID);
                if (S.Subcatch[i].outNode >= 0)
                {
                    R.fprintf(S.Frpt.file, S.L240, S.Node[S.Subcatch[i].outNode].ID);
                }
                else if (S.Subcatch[i].outSubcatch >= 0)
                {
                    R.fprintf(S.Frpt.file, S.L240, S.Subcatch[S.Subcatch[i].outSubcatch].ID);
                }
                if ((S.Subcatch[i].lidArea != 0.0))
                {
                    lidCount++;
                }
            }
        }
        if (lidCount > 0)
        {
            S.lid_writeSummary();
        }
        if (S.Nobjects[S.NODE] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L241));
            S.report_writeLine((S.L242));
            S.report_writeLine((S.L241));
            R.fprintf(S.Frpt.file, S.L243);
            R.fprintf(S.Frpt.file, S.L244);
            R.fprintf(S.Frpt.file, S.L245);
            for (i = 0; i < S.Nobjects[S.NODE]; i++)
            {
                R.fprintf(S.Frpt.file, S.L246, S.Node[i].ID, S.NodeTypeWords[S.Node[i].type - S.JUNCTION], S.Node[i].invertElev * S.UCF(S.LENGTH), S.Node[i].fullDepth * S.UCF(S.LENGTH), S.Node[i].pondedArea * S.UCF(S.LENGTH) * S.UCF(S.LENGTH));
                if (((S.Node[i].extInflow != null) || (S.Node[i].dwfInflow != null)) || (S.Node[i].rdiiInflow != null))
                {
                    R.fprintf(S.Frpt.file, S.L247);
                }
            }
        }
        if (S.Nobjects[S.LINK] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L241));
            S.report_writeLine((S.L248));
            S.report_writeLine((S.L241));
            R.fprintf(S.Frpt.file, S.L249);
            R.fprintf(S.Frpt.file, S.L250);
            for (i = 0; i < S.Nobjects[S.LINK]; i++)
            {
                if (S.Link[i].direction == 1)
                {
                    R.fprintf(S.Frpt.file, S.L251, S.Link[i].ID, S.Node[S.Link[i].node1].ID, S.Node[S.Link[i].node2].ID);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L251, S.Link[i].ID, S.Node[S.Link[i].node2].ID, S.Node[S.Link[i].node1].ID);
                }
                if (S.Link[i].type == S.PUMP)
                {
                    k = S.Link[i].subIndex;
                    R.fprintf(S.Frpt.file, S.L252, S.PumpTypeWords[S.Pump[k].type]);
                }
                else
                {
                    R.fprintf(S.Frpt.file, S.L253, S.LinkTypeWords[S.Link[i].type - S.CONDUIT]);
                }
                if (S.Link[i].type == S.CONDUIT)
                {
                    k = S.Link[i].subIndex;
                    R.fprintf(S.Frpt.file, S.L254, S.Conduit[k].length * S.UCF(S.LENGTH), S.Conduit[k].slope * 100.0 * S.Link[i].direction, S.Conduit[k].roughness);
                }
            }
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L255));
            S.report_writeLine((S.L256));
            S.report_writeLine((S.L255));
            R.fprintf(S.Frpt.file, S.L257);
            R.fprintf(S.Frpt.file, S.L258);
            R.fprintf(S.Frpt.file, S.L259);
            for (i = 0; i < S.Nobjects[S.LINK]; i++)
            {
                if (S.Link[i].type == S.CONDUIT)
                {
                    k = S.Link[i].subIndex;
                    R.fprintf(S.Frpt.file, S.L260, S.Link[i].ID);
                    if (S.Link[i].xsect.type == S.CUSTOM)
                    {
                        R.fprintf(S.Frpt.file, S.L261, S.Curve[S.Link[i].xsect.transect].ID);
                    }
                    else if (S.Link[i].xsect.type == S.IRREGULAR)
                    {
                        R.fprintf(S.Frpt.file, S.L261, S.Transect[S.Link[i].xsect.transect].ID);
                    }
                    else if (S.Link[i].xsect.type == S.STREET_XSECT)
                    {
                        R.fprintf(S.Frpt.file, S.L261, S.Street[S.Link[i].xsect.transect].ID);
                    }
                    else
                    {
                        R.fprintf(S.Frpt.file, S.L261, S.XsectTypeWords[S.Link[i].xsect.type]);
                    }
                    R.fprintf(S.Frpt.file, S.L262, S.Link[i].xsect.yFull * S.UCF(S.LENGTH), S.Link[i].xsect.aFull * S.UCF(S.LENGTH) * S.UCF(S.LENGTH), S.Link[i].xsect.rFull * S.UCF(S.LENGTH), S.Link[i].xsect.wMax * S.UCF(S.LENGTH), S.Conduit[k].barrels, S.Link[i].qFull * S.UCF(S.FLOW));
                }
            }
        }
        if (S.Nobjects[S.SHAPE] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L206));
            S.report_writeLine((S.L263));
            S.report_writeLine((S.L206));
            for (i = 0; i < S.Nobjects[S.SHAPE]; i++)
            {
                k = S.Shape[i].curve;
                R.fprintf(S.Frpt.file, S.L264, S.Curve[k].ID);
                R.fprintf(S.Frpt.file, S.L265);
                for (m = 1; m < 51; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Shape[i].areaTbl[m]);
                }
                R.fprintf(S.Frpt.file, S.L268);
                for (m = 1; m < 51; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Shape[i].hradTbl[m]);
                }
                R.fprintf(S.Frpt.file, S.L269);
                for (m = 1; m < 51; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Shape[i].widthTbl[m]);
                }
            }
        }
        if (S.Nobjects[S.TRANSECT] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L227));
            S.report_writeLine((S.L270));
            S.report_writeLine((S.L227));
            for (i = 0; i < S.Nobjects[S.TRANSECT]; i++)
            {
                R.fprintf(S.Frpt.file, S.L271, S.Transect[i].ID);
                R.fprintf(S.Frpt.file, S.L265);
                for (m = 1; m < 51; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Transect[i].areaTbl[m]);
                }
                R.fprintf(S.Frpt.file, S.L268);
                for (m = 1; m < 51; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Transect[i].hradTbl[m]);
                }
                R.fprintf(S.Frpt.file, S.L269);
                for (m = 1; m < 51; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Transect[i].widthTbl[m]);
                }
            }
        }
        if (S.Nobjects[S.STREET] > 0)
        {
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L0));
            S.report_writeLine((S.L272));
            S.report_writeLine((S.L273));
            S.report_writeLine((S.L272));
            for (i = 0; i < S.Nobjects[S.STREET]; i++)
            {
                R.fprintf(S.Frpt.file, S.L274, S.Street[i].ID);
                R.fprintf(S.Frpt.file, S.L265);
                for (m = 1; m < S.Street[i].transect.nTbl; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Street[i].transect.areaTbl[m]);
                }
                R.fprintf(S.Frpt.file, S.L268);
                for (m = 1; m < S.Street[i].transect.nTbl; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Street[i].transect.hradTbl[m]);
                }
                R.fprintf(S.Frpt.file, S.L269);
                for (m = 1; m < S.Street[i].transect.nTbl; m++)
                {
                    if (m % 5 == 1)
                    {
                        R.fprintf(S.Frpt.file, S.L266);
                    }
                    R.fprintf(S.Frpt.file, S.L267, S.Street[i].transect.widthTbl[m]);
                }
            }
        }
        S.report_writeLine((S.L0));
    }

}
}
