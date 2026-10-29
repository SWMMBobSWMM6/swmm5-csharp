// Generated from EPA SWMM 5.2.4 (culvert.c) by tools/c2x/emit_cs.py. Do not edit by hand;
// fix the translator or runtime/ instead.
using System;
namespace Swmm5
{
public static partial class S
{
    public static int MAX_CULVERT_CODE = 57;
    public static double[][] Params = new double[][] { new double[] { 0.0, 0.0, 0.0, 0.0, 0.00 }, new double[] { 1.0, 0.0098, 2.00, 0.0398, 0.67 }, new double[] { 1.0, 0.0018, 2.00, 0.0292, 0.74 }, new double[] { 1.0, 0.0045, 2.00, 0.0317, 0.69 }, new double[] { 1.0, 0.0078, 2.00, 0.0379, 0.69 }, new double[] { 1.0, 0.0210, 1.33, 0.0463, 0.75 }, new double[] { 1.0, 0.0340, 1.50, 0.0553, 0.54 }, new double[] { 1.0, 0.0018, 2.50, 0.0300, 0.74 }, new double[] { 1.0, 0.0018, 2.50, 0.0243, 0.83 }, new double[] { 1.0, 0.026, 1.0, 0.0347, 0.81 }, new double[] { 1.0, 0.061, 0.75, 0.0400, 0.80 }, new double[] { 1.0, 0.061, 0.75, 0.0423, 0.82 }, new double[] { 2.0, 0.510, 0.667, 0.0309, 0.80 }, new double[] { 2.0, 0.486, 0.667, 0.0249, 0.83 }, new double[] { 2.0, 0.515, 0.667, 0.0375, 0.79 }, new double[] { 2.0, 0.495, 0.667, 0.0314, 0.82 }, new double[] { 2.0, 0.486, 0.667, 0.0252, 0.865 }, new double[] { 2.0, 0.545, 0.667, 0.04505, 0.73 }, new double[] { 2.0, 0.533, 0.667, 0.0425, 0.705 }, new double[] { 2.0, 0.522, 0.667, 0.0402, 0.68 }, new double[] { 2.0, 0.498, 0.667, 0.0327, 0.75 }, new double[] { 2.0, 0.497, 0.667, 0.0339, 0.803 }, new double[] { 2.0, 0.493, 0.667, 0.0361, 0.806 }, new double[] { 2.0, 0.495, 0.667, 0.0386, 0.71 }, new double[] { 2.0, 0.497, 0.667, 0.0302, 0.835 }, new double[] { 2.0, 0.495, 0.667, 0.0252, 0.881 }, new double[] { 2.0, 0.493, 0.667, 0.0227, 0.887 }, new double[] { 1.0, 0.0083, 2.00, 0.0379, 0.69 }, new double[] { 1.0, 0.0145, 1.75, 0.0419, 0.64 }, new double[] { 1.0, 0.0340, 1.50, 0.0496, 0.57 }, new double[] { 1.0, 0.0100, 2.00, 0.0398, 0.67 }, new double[] { 1.0, 0.0018, 2.50, 0.0292, 0.74 }, new double[] { 1.0, 0.0045, 2.00, 0.0317, 0.69 }, new double[] { 1.0, 0.0100, 2.00, 0.0398, 0.67 }, new double[] { 1.0, 0.0018, 2.50, 0.0292, 0.74 }, new double[] { 1.0, 0.0095, 2.00, 0.0317, 0.69 }, new double[] { 1.0, 0.0083, 2.00, 0.0379, 0.69 }, new double[] { 1.0, 0.0300, 1.00, 0.0463, 0.75 }, new double[] { 1.0, 0.0340, 1.50, 0.0496, 0.57 }, new double[] { 1.0, 0.0300, 1.50, 0.0496, 0.57 }, new double[] { 1.0, 0.0088, 2.00, 0.0368, 0.68 }, new double[] { 1.0, 0.0030, 2.00, 0.0269, 0.77 }, new double[] { 1.0, 0.0300, 1.50, 0.0496, 0.57 }, new double[] { 1.0, 0.0088, 2.00, 0.0368, 0.68 }, new double[] { 1.0, 0.0030, 2.00, 0.0269, 0.77 }, new double[] { 1.0, 0.0083, 2.00, 0.0379, 0.69 }, new double[] { 1.0, 0.0300, 1.00, 0.0473, 0.75 }, new double[] { 1.0, 0.0340, 1.50, 0.0496, 0.57 }, new double[] { 2.0, 0.534, 0.555, 0.0196, 0.90 }, new double[] { 2.0, 0.519, 0.640, 0.0210, 0.90 }, new double[] { 2.0, 0.536, 0.622, 0.0368, 0.83 }, new double[] { 2.0, 0.5035, 0.719, 0.0478, 0.80 }, new double[] { 2.0, 0.547, 0.800, 0.0598, 0.75 }, new double[] { 2.0, 0.475, 0.667, 0.0179, 0.97 }, new double[] { 2.0, 0.560, 0.667, 0.0446, 0.85 }, new double[] { 2.0, 0.560, 0.667, 0.0378, 0.87 }, new double[] { 2.0, 0.500, 0.667, 0.0446, 0.65 }, new double[] { 2.0, 0.500, 0.667, 0.0378, 0.71 } };

    // culvert.c:174
    public static double culvert_getInflow(int j, double q0, double h)
    {
        int code = 0;
        int k = 0;
        int condition = 0;
        double y = 0.0;
        double y1 = 0.0;
        double y2 = 0.0;
        double q = 0.0;
        TCulvert culvert = new TCulvert();
        if (S.Link[j].type != S.CONDUIT)
        {
            return q0;
        }
        culvert.xsect = S.Link[j].xsect;
        code = culvert.xsect.culvertCode;
        if ((code <= 0) || (code > S.MAX_CULVERT_CODE))
        {
            return q0;
        }
        k = S.Link[j].subIndex;
        culvert.yFull = culvert.xsect.yFull;
        culvert.ad = culvert.xsect.aFull * Math.Sqrt(culvert.yFull);
        switch (code)
        {
            case 5:
            case 37:
            case 46:
                culvert.scf = -7.0 * S.Conduit[k].slope;
                break;
            default:
                culvert.scf = 0.5 * S.Conduit[k].slope;
                break;
        }
        y = h - (S.Node[S.Link[j].node1].invertElev + S.Link[j].offset1);
        y2 = culvert.yFull * (16.0 * S.Params[code][S.C] + S.Params[code][S.Y] - culvert.scf);
        if (y >= y2)
        {
            q = S.getSubmergedFlow(code, y, culvert);
            condition = 2;
        }
        else
        {
            y1 = 0.95 * culvert.yFull;
            if (y <= y1)
            {
                q = S.getUnsubmergedFlow(code, y, culvert);
                condition = 1;
            }
            else
            {
                q = S.getTransitionFlow(code, y, y1, y2, culvert);
                condition = 0;
            }
        }
        if (q < q0)
        {
            S.Link[j].inletControl = 1;
            S.Link[j].dqdh = culvert.dQdH;
            return q;
        }
        else
        {
            return q0;
        }
    }

    // culvert.c:257
    public static double getUnsubmergedFlow(int code, double h, TCulvert culvert)
    {
        double arg = 0.0;
        double q = 0.0;
        culvert.kk = S.Params[code][S.K];
        culvert.mm = S.Params[code][S.M];
        arg = h / culvert.yFull / culvert.kk;
        if (S.Params[code][S.FORM] == 1.0)
        {
            q = S.getForm1Flow(h, culvert);
        }
        else
        {
            q = culvert.ad * Math.Pow(arg, 1.0 / culvert.mm);
        }
        culvert.dQdH = q / h / culvert.mm;
        return q;
    }

    // culvert.c:288
    public static double getSubmergedFlow(int code, double h, TCulvert culvert)
    {
        double cc = S.Params[code][S.C];
        double yy = S.Params[code][S.Y];
        double arg = (h / culvert.yFull - yy + culvert.scf) / cc;
        double q = 0.0;
        if (arg <= 0.0)
        {
            culvert.dQdH = 0.0;
            return 10000000000.0;
        }
        q = Math.Sqrt(arg) * culvert.ad;
        culvert.dQdH = 0.5 * q / arg / culvert.yFull / cc;
        return q;
    }

    // culvert.c:316
    public static double getTransitionFlow(int code, double h, double h1, double h2, TCulvert culvert)
    {
        double q1 = S.getUnsubmergedFlow(code, h1, culvert);
        double q2 = S.getSubmergedFlow(code, h2, culvert);
        double q = q1 + (q2 - q1) * (h - h1) / (h2 - h1);
        culvert.dQdH = (q2 - q1) / (h2 - h1);
        return q;
    }

    // culvert.c:339
    public static double getForm1Flow(double h, TCulvert culvert)
    {
        culvert.hPlus = h / culvert.yFull + culvert.scf;
        S.findroot_Ridder(0.01 * h, h, 0.001, S.form1Eqn, culvert);
        return culvert.qc;
    }

    // culvert.c:363
    public static double form1Eqn(double yc, object p)
    {
        double ac = 0.0;
        double wc = 0.0;
        double yh = 0.0;
        TCulvert culvert = ((TCulvert)p);
        ac = S.xsect_getAofY(culvert.xsect, yc);
        wc = S.xsect_getWofY(culvert.xsect, yc);
        yh = ac / wc;
        culvert.qc = ac * Math.Sqrt(32.2 * yh);
        return culvert.hPlus - yc / culvert.yFull - yh / 2.0 / culvert.yFull - culvert.kk * Math.Pow(culvert.qc / culvert.ad, culvert.mm);
    }

}
}
