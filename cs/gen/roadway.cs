// Generated from EPA SWMM 5.2.4 (roadway.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int N_Cr_Low_Paved = 4;
    public static double[][] Cr_Low_Paved = new double[][] { new double[] { 0.0, 2.85 }, new double[] { 0.2, 2.95 }, new double[] { 0.7, 3.03 }, new double[] { 4.0, 3.05 } };
    public static int N_Cr_Low_Gravel = 8;
    public static double[][] Cr_Low_Gravel = new double[][] { new double[] { 0.0, 2.5 }, new double[] { 0.5, 2.7 }, new double[] { 1.0, 2.8 }, new double[] { 1.5, 2.9 }, new double[] { 2.0, 2.98 }, new double[] { 2.5, 3.02 }, new double[] { 3.0, 3.03 }, new double[] { 4.0, 3.05 } };
    public static int N_Cr_High_Paved = 2;
    public static double[][] Cr_High_Paved = new double[][] { new double[] { 0.15, 3.05 }, new double[] { 0.25, 3.10 } };
    public static int N_Cr_High_Gravel = 2;
    public static double[][] Cr_High_Gravel = new double[][] { new double[] { 0.15, 2.95 }, new double[] { 0.30, 3.10 } };
    public static int N_Kt_Paved = 9;
    public static double[][] Kt_Paved = new double[][] { new double[] { 0.8, 1.0 }, new double[] { 0.85, 0.98 }, new double[] { 0.90, 0.92 }, new double[] { 0.93, 0.85 }, new double[] { 0.95, 0.80 }, new double[] { 0.97, 0.70 }, new double[] { 0.98, 0.60 }, new double[] { 0.99, 0.50 }, new double[] { 1.00, 0.40 } };
    public static int N_Kt_Gravel = 12;
    public static double[][] Kt_Gravel = new double[][] { new double[] { 0.75, 1.00 }, new double[] { 0.80, 0.985 }, new double[] { 0.83, 0.97 }, new double[] { 0.86, 0.93 }, new double[] { 0.89, 0.90 }, new double[] { 0.90, 0.87 }, new double[] { 0.92, 0.80 }, new double[] { 0.94, 0.70 }, new double[] { 0.96, 0.60 }, new double[] { 0.98, 0.50 }, new double[] { 0.99, 0.40 }, new double[] { 1.00, 0.24 } };

    // roadway.c:85
    public static double roadway_getInflow(int j, double dir, double hRoad, double h1, double h2)
    {
        int k = 0;
        int roadSurf = 0;
        int useVariableCd = 0;
        double length = 0.0;
        double roadWidth = 0.0;
        double hWr = 0.0;
        double ht = 0.0;
        double cD = 0.0;
        double q = 0.0;
        double dqdh = 0.0;
        if (S.Link[j].type != S.WEIR)
        {
            return 0.0;
        }
        k = S.Link[j].subIndex;
        roadWidth = S.Weir[k].roadWidth;
        roadSurf = S.Weir[k].roadSurface;
        cD = S.Weir[k].cDisch1;
        if (S.UnitSystem == S.SI)
        {
            cD = cD / 0.552;
        }
        useVariableCd = 0;
        if ((roadWidth > 0.0) && (roadSurf >= 1))
        {
            useVariableCd = 1;
        }
        hWr = h1 - hRoad;
        ht = h2 - hRoad;
        if (hWr > 0.0001)
        {
            if ((useVariableCd != 0))
            {
                cD = S.getCd(hWr, ht, roadWidth, roadSurf);
            }
            length = S.Link[j].xsect.wMax;
            q = cD * length * Math.Pow(hWr, 1.5);
            dqdh = 1.5 * q / hWr;
        }
        S.Link[j].dqdh = dqdh;
        S.Link[j].newDepth = ((((h1 - hRoad) >= (0.0)) ? (h1 - hRoad) : (0.0)));
        S.Link[j].flowClass = S.SUBCRITICAL;
        if (hRoad > h2)
        {
            if (dir == 1.0)
            {
                S.Link[j].flowClass = S.DN_CRITICAL;
            }
            else
            {
                S.Link[j].flowClass = S.UP_CRITICAL;
            }
        }
        return dir * q;
    }

    // roadway.c:146
    public static double getCd(double hWr, double ht, double roadWidth, int roadSurf)
    {
        double kT = 1.0;
        double hL = 0.0;
        double htH = 0.0;
        double cR = 0.0;
        if (hWr <= 0.0)
        {
            return 0.0;
        }
        hL = hWr / roadWidth;
        if (hL <= 0.15)
        {
            if (roadSurf == S.PAVED)
            {
                cR = S.getY(hWr, S.Cr_Low_Paved, S.N_Cr_Low_Paved);
            }
            else
            {
                cR = S.getY(hWr, S.Cr_Low_Gravel, S.N_Cr_Low_Gravel);
            }
        }
        else
        {
            if (roadSurf == S.PAVED)
            {
                cR = S.getY(hL, S.Cr_High_Paved, S.N_Cr_High_Paved);
            }
            else
            {
                cR = S.getY(hL, S.Cr_High_Gravel, S.N_Cr_High_Gravel);
            }
        }
        if (ht > 0.0)
        {
            htH = ht / hWr;
            if (roadSurf == S.PAVED)
            {
                kT = S.getY(htH, S.Kt_Paved, S.N_Kt_Paved);
            }
            else
            {
                kT = S.getY(htH, S.Kt_Gravel, S.N_Kt_Gravel);
            }
        }
        return cR * kT;
    }

    // roadway.c:176
    public static double getY(double x, double[][] table, int n)
    {
        int i = 0;
        double x1 = 0.0;
        double y1 = 0.0;
        double dx = 0.0;
        double dy = 0.0;
        if (x <= table[0][0])
        {
            return table[0][1];
        }
        if (x >= (table[n - 1])[0])
        {
            return (table[n - 1])[1];
        }
        for (i = 1; i < n; i++)
        {
            if (x <= table[i][0])
            {
                x1 = (table[i - 1])[0];
                dx = table[i][0] - x1;
                y1 = (table[i - 1])[1];
                dy = table[i][1] - y1;
                return y1 + (x - x1) * dy / dx;
            }
        }
        return (table[n - 1])[1];
    }

}
}
