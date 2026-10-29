// Generated from EPA SWMM 5.2.4 (transect.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int Ntransects = 0;
    public static int Nstations = 0;
    public static double[] Station = new double[1501];
    public static double[] Elev = new double[1501];
    public static double Nleft = 0.0;
    public static double Nright = 0.0;
    public static double Nchannel = 0.0;
    public static double Xleftbank = 0.0;
    public static double Xrightbank = 0.0;
    public static double Xfactor = 0.0;
    public static double Yfactor = 0.0;
    public static double Lfactor = 0.0;

    // transect.c:70
    public static int transect_create(int n)
    {
        S.Ntransects = n;
        if (n == 0)
        {
            return 0;
        }
        S.Transect = R.NewArray<TTransect>((int)(unchecked((ulong)(S.Ntransects))));
        if (S.Transect == null)
        {
            return S.ERR_MEMORY;
        }
        S.Nchannel = 0.0;
        S.Nleft = 0.0;
        S.Nright = 0.0;
        S.Nstations = 0;
        return 0;
    }

    // transect.c:90
    public static void transect_delete()
    {
        if (S.Ntransects == 0)
        {
            return;
        }
        {
            if ((S.Transect != null))
            {
                R.free(S.Transect);
                S.Transect = (null);
            }
        }
        ;
        S.Ntransects = 0;
    }

    // transect.c:104
    public static int transect_readParams(ref int count, CPtr[] tok, int ntoks)
    {
        int i = 0;
        int k = 0;
        int index = count;
        int errcode = 0;
        double[] x = new double[10];
        CPtr id = CPtr.Null;
        k = S.findmatch(tok[0], S.TransectKeyWords);
        if (k < 0)
        {
            return S.error_setInpError(S.ERR_KEYWORD, tok[0]);
        }
        switch (k)
        {
            case 0:
                S.transect_validate(index - 1);
                if (ntoks < 4)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                for (i = 1; i <= 3; i++)
                {
                    if (!((S.getDouble(tok[i], ref x[i]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                    }
                }
                return S.setManning(x);
            case 1:
                if (ntoks < 10)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                id = S.project_findID(S.TRANSECT, tok[1]);
                if (id.IsNull)
                {
                    return S.error_setInpError(S.ERR_NAME, tok[1]);
                }
                for (i = 2; i < 10; i++)
                {
                    if (!((S.getDouble(tok[i], ref x[i]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                    }
                }
                count = index + 1;
                return S.setParams(index, id, x);
            case 2:
                if ((ntoks - 1) % 2 > 0)
                {
                    return S.error_setInpError(S.ERR_ITEMS, S.L0);
                }
                i = 1;
                while (i < ntoks)
                {
                    if (!((S.getDouble(tok[i], ref x[1]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[i]);
                    }
                    if (!((S.getDouble(tok[i + 1], ref x[2]) != 0)))
                    {
                        return S.error_setInpError(S.ERR_NUMBER, tok[i + 1]);
                    }
                    errcode = S.addStation(x[1], x[2]);
                    if ((errcode != 0))
                    {
                        return errcode;
                    }
                    i += 2;
                }
                return 0;
        }
        return 0;
    }

    // transect.c:194
    public static void transect_validate(int j)
    {
        int i = 0;
        double ymin = 0.0;
        double ymax = 0.0;
        double oldNchannel = S.Nchannel;
        if ((j < 0) || (j >= S.Ntransects))
        {
            return;
        }
        if (S.Nstations < 2)
        {
            S.report_writeErrorMsg(S.ERR_TRANSECT_TOO_FEW, S.Transect[j].ID);
            return;
        }
        if (S.Nstations >= 1500)
        {
            S.report_writeErrorMsg(S.ERR_TRANSECT_TOO_MANY, S.Transect[j].ID);
            return;
        }
        if (S.Nchannel <= 0.0)
        {
            S.report_writeErrorMsg(S.ERR_TRANSECT_MANNING, S.Transect[j].ID);
            return;
        }
        if (S.Xleftbank > S.Xrightbank)
        {
            S.report_writeErrorMsg(S.ERR_TRANSECT_OVERBANK, S.Transect[j].ID);
            return;
        }
        S.Nchannel = S.Nchannel * Math.Sqrt(S.Lfactor);
        S.Transect[j].lengthFactor = S.Lfactor;
        ymax = S.Elev[1];
        ymin = S.Elev[1];
        for (i = 2; i <= S.Nstations; i++)
        {
            ymax = ((((S.Elev[i]) >= (ymax)) ? (S.Elev[i]) : (ymax)));
            ymin = ((((S.Elev[i]) <= (ymin)) ? (S.Elev[i]) : (ymin)));
        }
        if (ymin >= ymax)
        {
            S.report_writeErrorMsg(S.ERR_TRANSECT_NO_DEPTH, S.Transect[j].ID);
            return;
        }
        S.Station[0] = S.Station[1];
        S.Elev[0] = ymax;
        S.Nstations++;
        S.Station[S.Nstations] = S.Station[S.Nstations - 1];
        S.Elev[S.Nstations] = S.Elev[0];
        S.Transect[j].nTbl = 51;
        S.createTables(S.Transect[j], ymin, ymax);
        S.Transect[j].roughness = oldNchannel;
    }

    // transect.c:264
    public static void createTables(TTransect transect, double ymin, double ymax)
    {
        int i = 0;
        int nLast = 0;
        double dy = 0.0;
        double y = 0.0;
        transect.yFull = ymax - ymin;
        transect.wMax = 0.0;
        transect.areaTbl[0] = 0.0;
        transect.hradTbl[0] = 0.0;
        transect.widthTbl[0] = 0.0;
        dy = (ymax - ymin) / ((double)((transect.nTbl)) - 1);
        y = ymin;
        for (i = 1; i < transect.nTbl; i++)
        {
            y += dy;
            transect.areaTbl[i] = 0.0;
            transect.hradTbl[i] = 0.0;
            transect.widthTbl[i] = 0.0;
            S.getGeometry(transect, i, y);
        }
        S.setMaxSectionFactor(transect);
        nLast = transect.nTbl - 1;
        transect.aFull = transect.areaTbl[nLast];
        transect.rFull = transect.hradTbl[nLast];
        transect.wMax = transect.widthTbl[nLast];
        for (i = 1; i <= nLast; i++)
        {
            transect.areaTbl[i] /= transect.aFull;
            transect.hradTbl[i] /= transect.rFull;
            transect.widthTbl[i] /= transect.wMax;
        }
        transect.widthTbl[0] = transect.widthTbl[1];
    }

    // transect.c:312
    public static int setManning(double[] n)
    {
        int i = 0;
        for (i = 1; i <= 3; i++)
        {
            if (n[i] < 0.0)
            {
                return S.ERR_NUMBER;
            }
        }
        if (n[1] > 0.0)
        {
            S.Nleft = n[1];
        }
        if (n[2] > 0.0)
        {
            S.Nright = n[2];
        }
        if (n[3] > 0.0)
        {
            S.Nchannel = n[3];
        }
        if (S.Nleft == 0.0)
        {
            S.Nleft = S.Nchannel;
        }
        if (S.Nright == 0.0)
        {
            S.Nright = S.Nchannel;
        }
        return 0;
    }

    // transect.c:334
    public static int setParams(int j, CPtr id, double[] x)
    {
        if ((j < 0) || (j >= S.Ntransects))
        {
            return S.ERR_NUMBER;
        }
        S.Transect[j].ID = id;
        S.Xleftbank = x[3] / S.UCF(S.LENGTH);
        S.Xrightbank = x[4] / S.UCF(S.LENGTH);
        S.Lfactor = x[7];
        if (S.Lfactor == 0.0)
        {
            S.Lfactor = 1.0;
        }
        S.Xfactor = x[8];
        if (S.Xfactor == 0.0)
        {
            S.Xfactor = 1.0;
        }
        S.Xleftbank *= S.Xfactor;
        S.Xrightbank *= S.Xfactor;
        S.Yfactor = x[9] / S.UCF(S.LENGTH);
        S.Nstations = 0;
        return 0;
    }

    // transect.c:360
    public static int addStation(double y, double x)
    {
        if (S.Nstations < 0)
        {
            return S.ERR_TRANSECT_UNKNOWN;
        }
        S.Nstations++;
        if (S.Nstations >= 1500)
        {
            return 0;
        }
        S.Station[S.Nstations] = x * S.Xfactor / S.UCF(S.LENGTH);
        S.Elev[S.Nstations] = (y + S.Yfactor) / S.UCF(S.LENGTH);
        if ((S.Nstations > 1) && (S.Station[S.Nstations] < S.Station[S.Nstations - 1]))
        {
            return S.ERR_TRANSECT_SEQUENCE;
        }
        return 0;
    }

    // transect.c:388
    public static void getGeometry(TTransect transect, int i, double y)
    {
        int k = 0;
        double ylo = 0.0;
        double yhi = 0.0;
        double w = 0.0;
        double wp = 0.0;
        double wpSum = 0.0;
        double a = 0.0;
        double aSum = 0.0;
        double q = 0.0;
        double qSum = 0.0;
        int findFlow = 0;
        wpSum = 0.0;
        aSum = 0.0;
        qSum = 0.0;
        for (k = 1; k <= S.Nstations; k++)
        {
            if (S.Elev[k - 1] >= S.Elev[k])
            {
                yhi = S.Elev[k - 1];
                ylo = S.Elev[k];
            }
            else
            {
                yhi = S.Elev[k];
                ylo = S.Elev[k - 1];
            }
            if (ylo >= y)
            {
                continue;
            }
            S.getSliceGeom(k, y, ylo, yhi, ref w, ref a, ref wp);
            wpSum += wp;
            aSum += a;
            transect.areaTbl[i] += a;
            transect.widthTbl[i] += w;
            if (S.Elev[k] >= y)
            {
                findFlow = 1;
            }
            else
            {
                findFlow = 0;
            }
            q = S.getFlow(k, aSum, wpSum, findFlow);
            if (q > 0.0)
            {
                qSum += q;
                aSum = 0.0;
                wpSum = 0.0;
            }
        }
        aSum = transect.areaTbl[i];
        if (aSum == 0.0)
        {
            transect.hradTbl[i] = transect.hradTbl[i - 1];
        }
        else
        {
            transect.hradTbl[i] = Math.Pow(qSum * S.Nchannel / 1.49 / aSum, 1.5);
        }
    }

    // transect.c:468
    public static void getSliceGeom(int k, double y, double ylo, double yhi, ref double w, ref double a, ref double wp)
    {
        double width = 0.0;
        double ratio = 0.0;
        width = Math.Abs(S.Station[k] - S.Station[k - 1]);
        w = width;
        wp = Math.Sqrt(width * width + (yhi - ylo) * (yhi - ylo));
        a = 0.0;
        if (y > yhi)
        {
            a = width * ((y - yhi) + (y - ylo)) / 2.0;
        }
        else if (yhi > ylo)
        {
            ratio = (y - ylo) / (yhi - ylo);
            a = width * (yhi - ylo) / 2.0 * ratio * ratio;
            w *= ratio;
            wp *= ratio;
        }
    }

    // transect.c:519
    public static double getFlow(int k, double a, double wp, int findFlow)
    {
        double n = 0.0;
        if (findFlow == 0)
        {
            if (k == S.Nstations - 1)
            {
                findFlow = 1;
            }
            else if (S.Station[k] == S.Xleftbank)
            {
                if ((S.Nleft != S.Nchannel) && (S.Station[k] != S.Station[k - 1]))
                {
                    findFlow = 1;
                }
            }
            else if (S.Station[k] == S.Xrightbank)
            {
                if ((S.Nright != S.Nchannel) && (S.Station[k] != S.Station[k + 1]))
                {
                    findFlow = 1;
                }
            }
        }
        if ((findFlow != 0))
        {
            n = S.Nchannel;
            if (S.Station[k - 1] < S.Xleftbank)
            {
                n = S.Nleft;
            }
            if (S.Station[k] > S.Xrightbank)
            {
                n = S.Nright;
            }
            return 1.486 / n * a * Math.Pow(a / wp, 2.0 / 3.0);
        }
        return 0.0;
    }

    // transect.c:570
    public static void setMaxSectionFactor(TTransect transect)
    {
        int i = 0;
        double sf = 0.0;
        transect.aMax = 0.0;
        transect.sMax = 0.0;
        for (i = 1; i < transect.nTbl; i++)
        {
            sf = transect.areaTbl[i] * Math.Pow(transect.hradTbl[i], 2.0 / 3.0);
            if (sf > transect.sMax)
            {
                transect.sMax = sf;
                transect.aMax = transect.areaTbl[i];
            }
        }
    }

    // transect.c:596
    public static void transect_createStreetTransect(TStreet street)
    {
        double ymin = 0.0;
        double ymax = 0.0;
        double y1 = 0.0;
        double y3 = 0.0;
        double y4 = 0.0;
        double w1 = 0.0;
        double w2 = 0.0;
        double w3 = 0.0;
        double w4 = 0.0;
        ymin = 0.0;
        w1 = street.backWidth;
        w2 = street.gutterWidth;
        w3 = street.width;
        w4 = w3 - w2;
        y3 = street.gutterDepression + street.slope * w2;
        y1 = street.curbHeight + street.gutterDepression;
        ymax = street.backSlope * street.backWidth + y1;
        y4 = y3 + street.slope * w4;
        ymax = ((((ymax) >= (y4)) ? (ymax) : (y4)));
        S.Station[0] = 0.0;
        S.Elev[0] = ymax;
        S.Station[1] = w1;
        S.Elev[1] = y1;
        S.Station[2] = w1;
        S.Elev[2] = 0.0;
        S.Station[3] = w1 + w2;
        S.Elev[3] = y3;
        S.Station[4] = w1 + w3;
        S.Elev[4] = y4;
        if (street.sides == 1)
        {
            S.Station[5] = S.Station[4];
            S.Elev[5] = ymax;
            S.Nstations = 5;
            street.transect.nTbl = 51;
        }
        else
        {
            S.Station[5] = S.Station[4] + w4;
            S.Elev[5] = y3;
            S.Station[6] = S.Station[5] + w2;
            S.Elev[6] = 0.0;
            S.Station[7] = S.Station[6];
            S.Elev[7] = y1;
            S.Station[8] = S.Station[7] + w1;
            S.Elev[8] = ymax;
            S.Nstations = 8;
            street.transect.nTbl = 51;
        }
        S.Nchannel = street.roughness;
        if (street.backWidth == 0.0)
        {
            S.Nleft = S.Nchannel;
            S.Nright = S.Nchannel;
            S.Xleftbank = S.Station[0];
            S.Xrightbank = S.Station[S.Nstations];
        }
        else
        {
            S.Nleft = street.backRoughness;
            S.Nright = S.Nleft;
            S.Xleftbank = S.Station[1];
            if (street.sides == 2)
            {
                S.Xrightbank = S.Station[S.Nstations - 1];
            }
            else
            {
                S.Xrightbank = S.Station[S.Nstations];
            }
        }
        S.createTables(street.transect, ymin, ymax);
        street.transect.roughness = street.roughness;
    }

}
}
