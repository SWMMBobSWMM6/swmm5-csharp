// Generated from EPA SWMM 5.2.4 (shape.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static double Atotal = 0.0;
    public static double Ptotal = 0.0;

    // shape.c:42
    public static int shape_validate(TShape shape, TTable curve)
    {
        if (!((S.computeShapeTables(shape, curve) != 0)))
        {
            return 0;
        }
        if (!((S.normalizeShapeTables(shape) != 0)))
        {
            return 0;
        }
        return 1;
    }

    // shape.c:64
    public static int computeShapeTables(TShape shape, TTable curve)
    {
        int i = 0;
        int n = 0;
        double dy = 0.0;
        double y = 0.0;
        double y1 = 0.0;
        double y2 = 0.0;
        double w = 0.0;
        double w1 = 0.0;
        double w2 = 0.0;
        double yLast = 0.0;
        double wLast = 0.0;
        double wMax = 0.0;
        if (!((S.table_getFirstEntry(curve, ref y1, ref w1) != 0)))
        {
            return 0;
        }
        if (((y1 < 0.0) || (y1 >= 1.0)) || (w1 < 0.0))
        {
            return 0;
        }
        wMax = w1;
        if (y1 != 0.0)
        {
            y2 = y1;
            w2 = w1;
            y1 = 0.0;
            w1 = 0.0;
        }
        else
        {
            if (!((S.table_getNextEntry(curve, ref y2, ref w2) != 0)))
            {
                return 0;
            }
            if ((y2 < y1) || (w2 < 0.0))
            {
                return 0;
            }
            if (y2 > 1.0)
            {
                y2 = 1.0;
            }
            if (w2 > wMax)
            {
                wMax = w2;
            }
        }
        shape.nTbl = 51;
        n = shape.nTbl - 1;
        dy = 1.0 / (double)((n));
        shape.areaTbl[0] = 0.0;
        shape.hradTbl[0] = 0.0;
        shape.widthTbl[0] = w1;
        S.Ptotal = w1;
        S.Atotal = 0.0;
        y = 0.0;
        w = w1;
        for (i = 1; i <= n; i++)
        {
            yLast = y;
            wLast = w;
            y = y + dy;
            if (Math.Abs(y - 1.0) < 1e-06)
            {
                y = 1.0;
            }
            if (y > y2)
            {
                if (!((S.getNextInterval(curve, y, yLast, wLast, ref y1, ref y2, ref w1, ref w2, ref wMax) != 0)))
                {
                    return 0;
                }
                yLast = y1;
                wLast = w1;
            }
            w = S.shape_getWidth(y, y1, y2, w1, w2);
            S.Atotal += S.shape_getArea(y, w, yLast, wLast);
            S.Ptotal += S.getPerim(y, w, yLast, wLast);
            if (y == 1.0)
            {
                S.Ptotal += w2;
            }
            shape.widthTbl[i] = w;
            shape.areaTbl[i] = S.Atotal;
            if (S.Ptotal > 0.0)
            {
                shape.hradTbl[i] = S.Atotal / S.Ptotal;
            }
            else
            {
                shape.hradTbl[i] = 0.0;
            }
        }
        shape.aFull = shape.areaTbl[n];
        shape.rFull = shape.hradTbl[n];
        shape.wMax = wMax;
        S.getSmax(shape);
        return 1;
    }

    // shape.c:189
    public static void getSmax(TShape shape)
    {
        int i = 0;
        int n = shape.nTbl - 1;
        double sf = 0.0;
        shape.sMax = 0.0;
        shape.aMax = 0.0;
        for (i = 1; i <= n; i++)
        {
            sf = shape.areaTbl[i] * Math.Pow(shape.hradTbl[i], 2.0 / 3.0);
            if (sf > shape.sMax)
            {
                shape.sMax = sf;
                shape.aMax = shape.areaTbl[i];
            }
        }
    }

    // shape.c:216
    public static int normalizeShapeTables(TShape shape)
    {
        int i = 0;
        int n = shape.nTbl - 1;
        double aFull = shape.aFull;
        double rFull = shape.rFull;
        double wMax = shape.wMax;
        if (((aFull == 0.0) || (rFull == 0.0)) || (wMax == 0.0))
        {
            return 0;
        }
        for (i = 0; i <= n; i++)
        {
            shape.areaTbl[i] /= aFull;
            shape.hradTbl[i] /= rFull;
            shape.widthTbl[i] /= wMax;
        }
        return 1;
    }

    // shape.c:246
    public static int getNextInterval(TTable curve, double y, double yLast, double wLast, ref double y1, ref double y2, ref double w1, ref double w2, ref double wMax)
    {
        while (y > y2)
        {
            if (y2 > yLast)
            {
                S.Atotal += S.shape_getArea(y2, w2, yLast, wLast);
                S.Ptotal += S.getPerim(y2, w2, yLast, wLast);
                yLast = y2;
                wLast = w2;
            }
            y1 = y2;
            w1 = w2;
            if (!((S.table_getNextEntry(curve, ref y2, ref w2) != 0)))
            {
                y2 = 1.0;
                return 1;
            }
            if (w2 > wMax)
            {
                wMax = w2;
            }
            if ((y2 < y1) || (w2 < 0.0))
            {
                return 0;
            }
            if (y2 > 1.0)
            {
                y2 = 1.0;
            }
        }
        return 1;
    }

    // shape.c:307
    public static double shape_getWidth(double y, double y1, double y2, double w1, double w2)
    {
        if (y2 == y1)
        {
            return w2;
        }
        return w1 + (y - y1) / (y2 - y1) * (w2 - w1);
    }

    // shape.c:328
    public static double shape_getArea(double y, double w, double y1, double w1)
    {
        double wMin = 0.0;
        double wMax = 0.0;
        if (w > w1)
        {
            wMin = w1;
            wMax = w;
        }
        else
        {
            wMin = w;
            wMax = w1;
        }
        return (wMin + (wMax - wMin) / 2.0) * (y - y1);
    }

    // shape.c:353
    public static double getPerim(double y, double w, double y1, double w1)
    {
        double dy = y - y1;
        double dw = Math.Abs(w - w1) / 2.0;
        return 2.0 * Math.Sqrt(dy * dy + dw * dw);
    }

}
}
